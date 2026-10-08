// Sayfayı tarayıcıda tasarım genişliğinde render edip PDF ölçüleriyle karşılaştırır
// (design-check aracının 2. adımı).
//
// Kullanım:
//   node tools/design-check/compare.mjs <sayfa.html> <tasarim.pdf> [esik-px]
//   örn: node tools/design-check/compare.mjs diyet-plani-detay.html "design/Diyet Planı Detay.pdf"
//
// Gereksinimler: Node 22+, Python + pymupdf (pip install pymupdf), Microsoft Edge
// (başka bir Chromium için EDGE_PATH ortam değişkeni).
//
// Rapor (fark > eşik, varsayılan 2px):
//   METİN : konum, genişlik, font boyutu; genişlik farkından harf aralığı tahmini
//   İKON  : Iconify/SVG ikonlarda çizilen şeklin boyutu ve konumu
//   KUTU  : kart/buton gibi kutuların kenarları ve çerçeve kalınlığı

import { spawn, execFileSync } from "node:child_process";
import { mkdtempSync, rmSync } from "node:fs";
import { tmpdir } from "node:os";
import { join, resolve, dirname } from "node:path";
import { fileURLToPath, pathToFileURL } from "node:url";

const [htmlArg, pdfArg, thresholdArg] = process.argv.slice(2);
if (!htmlArg || !pdfArg) {
  console.error("Kullanım: node tools/design-check/compare.mjs <sayfa.html> <tasarim.pdf> [esik-px]");
  process.exit(2);
}
const THRESHOLD = Number(thresholdArg || 2);
const here = dirname(fileURLToPath(import.meta.url));
const EDGE = process.env.EDGE_PATH || "C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe";
const PORT = 9400 + Math.floor(Math.random() * 400);

// ---------------------------------------------------------------- PDF ölçüleri
const spec = JSON.parse(
  execFileSync("python", [join(here, "extract_pdf.py"), resolve(pdfArg)], {
    encoding: "utf-8",
    env: { ...process.env, PYTHONIOENCODING: "utf-8" },
    maxBuffer: 64 * 1024 * 1024,
  })
);

// ---------------------------------------------------------------- Tarayıcı
const profile = mkdtempSync(join(tmpdir(), "design-check-"));
const browser = spawn(EDGE, [
  "--headless=new", "--disable-gpu", "--hide-scrollbars", `--remote-debugging-port=${PORT}`,
  "--allow-file-access-from-files", `--user-data-dir=${profile}`, "about:blank",
], { stdio: "ignore" });
const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

let target;
for (let i = 0; i < 60 && !target; i++) {
  try {
    const list = await (await fetch(`http://127.0.0.1:${PORT}/json`)).json();
    target = list.find((t) => t.type === "page");
  } catch { /* tarayıcı henüz açılmadı */ }
  if (!target) await sleep(250);
}
if (!target) { console.error("Tarayıcıya bağlanılamadı. EDGE_PATH doğru mu?"); process.exit(2); }

const ws = new WebSocket(target.webSocketDebuggerUrl);
await new Promise((r) => (ws.onopen = r));
let msgId = 0;
const pending = new Map();
ws.onmessage = (e) => {
  const m = JSON.parse(e.data);
  if (m.id && pending.has(m.id)) { pending.get(m.id)(m); pending.delete(m.id); }
};
const send = (method, params = {}) => new Promise((r) => {
  const id = ++msgId; pending.set(id, r); ws.send(JSON.stringify({ id, method, params }));
});
const evaluate = async (fn) => {
  const res = (await send("Runtime.evaluate", { expression: `(${fn})()`, awaitPromise: true, returnByValue: true })).result;
  if (res.exceptionDetails) throw new Error(JSON.stringify(res.exceptionDetails).slice(0, 400));
  return res.result.value;
};

await send("Page.enable");
await send("Emulation.setDeviceMetricsOverride", { width: Math.round(spec.width), height: 900, deviceScaleFactor: 1, mobile: false });
await send("Page.navigate", { url: pathToFileURL(resolve(htmlArg)).href });
await sleep(1500);
await evaluate(async function () { await document.fonts.ready; });
await sleep(1500); // Iconify ikonlarının yüklenmesi

// ---------------------------------------------------------------- DOM ölçüleri
const dom = await evaluate(function () {
  const BLOCK = /^(block|flex|grid|list-item|table|inline-block|inline-flex)$/;
  const visible = (el) => {
    const s = getComputedStyle(el);
    return s.display !== "none" && s.visibility !== "hidden" && parseFloat(s.opacity) > 0;
  };
  const blockOf = (el) => { while (el && !BLOCK.test(getComputedStyle(el).display)) el = el.parentElement; return el; };

  // Metin: karakter karakter ölçüp aynı blok + aynı satırdakileri birleştir
  const lines = [];
  const walker = document.createTreeWalker(document.body, NodeFilter.SHOW_TEXT);
  const range = document.createRange();
  for (let node = walker.nextNode(); node; node = walker.nextNode()) {
    const el = node.parentElement;
    if (!el || /^(SCRIPT|STYLE|NOSCRIPT)$/.test(el.tagName) || !visible(el) || !node.textContent.trim()) continue;
    const block = blockOf(el);
    const style = getComputedStyle(el);
    for (let i = 0; i < node.textContent.length; i++) {
      range.setStart(node, i); range.setEnd(node, i + 1);
      const r = range.getBoundingClientRect();
      if (!r.width && !r.height) continue;
      const ch = node.textContent[i];
      let line = lines.find((l) => l.block === block && Math.abs(l.top - r.top) < 3);
      if (!line) {
        line = { block, top: r.top, bottom: r.bottom, left: r.left, right: r.right, text: "",
          fontSize: parseFloat(style.fontSize), letterSpacing: parseFloat(style.letterSpacing) || 0,
          cls: (el.className && typeof el.className === "string" ? el.className.split(" ")[0] : el.tagName.toLowerCase()) };
        lines.push(line);
      }
      if (/\s/.test(ch)) { if (line.text && !line.text.endsWith(" ")) line.text += " "; continue; }
      line.text += ch;
      line.left = Math.min(line.left, r.left); line.right = Math.max(line.right, r.right);
      line.top = Math.min(line.top, r.top); line.bottom = Math.max(line.bottom, r.bottom);
      line.fontSize = Math.max(line.fontSize, parseFloat(getComputedStyle(el).fontSize));
    }
  }
  const sy = window.scrollY;
  const texts = lines.filter((l) => l.text.trim()).map((l) => ({
    text: l.text.trim(), cls: l.cls, fontSize: l.fontSize, letterSpacing: l.letterSpacing,
    // Son harfin ardındaki letter-spacing görsel genişliğe dahil değil
    left: l.left, right: l.right - l.letterSpacing, top: l.top + sy, bottom: l.bottom + sy,
  }));

  // İkon: Iconify (shadow DOM içindeki svg) ve küçük svg/img; çizilen şeklin kutusu
  const icons = [];
  document.querySelectorAll("iconify-icon, svg, img").forEach((el) => {
    if (!visible(el)) return;
    let shape = el;
    if (el.tagName === "ICONIFY-ICON") shape = el.shadowRoot && el.shadowRoot.querySelector("svg");
    if (!shape) return;
    let r = shape.getBoundingClientRect();
    if (shape.tagName && shape.tagName.toLowerCase() === "svg") {
      const parts = [...shape.querySelectorAll("path, circle, rect, polygon, ellipse, line")].map((p) => p.getBoundingClientRect()).filter((p) => p.width || p.height);
      if (parts.length) {
        r = { left: Math.min(...parts.map((p) => p.left)), top: Math.min(...parts.map((p) => p.top)),
          right: Math.max(...parts.map((p) => p.right)), bottom: Math.max(...parts.map((p) => p.bottom)) };
      }
    }
    const w = r.right - r.left, h = r.bottom - r.top;
    if (w > 0 && w <= 48 && h > 0 && h <= 48) {
      icons.push({ left: r.left, top: r.top + sy, right: r.right, bottom: r.bottom + sy,
        cls: (el.getAttribute("class") || el.tagName.toLowerCase()).split(" ")[0], box: el.tagName === "ICONIFY-ICON" ? el.getBoundingClientRect().width : null });
    }
  });

  // Kutu: arka planı veya çerçevesi olan görünür elemanlar
  const boxes = [];
  document.querySelectorAll("body *").forEach((el) => {
    if (!visible(el)) return;
    const s = getComputedStyle(el);
    const bg = s.backgroundColor !== "rgba(0, 0, 0, 0)" || s.backgroundImage !== "none";
    const bw = parseFloat(s.borderTopWidth);
    if (!bg && !(bw > 0 && s.borderTopStyle !== "none")) return;
    const r = el.getBoundingClientRect();
    if (r.width < 40 || r.height < 20) return;
    boxes.push({ left: r.left, top: r.top + sy, right: r.right, bottom: r.bottom + sy, borderWidth: s.borderTopStyle !== "none" ? bw : 0,
      cls: (el.getAttribute("class") || el.tagName.toLowerCase()).split(" ")[0] });
  });
  return { texts, icons, boxes };
});

ws.close();
browser.kill();
await sleep(300);
try { rmSync(profile, { recursive: true, force: true }); } catch { /* Edge dosyaları kilitlemiş olabilir */ }

// ---------------------------------------------------------------- Eşleştirme
// PDF'teki Type3 fontlar bazı Türkçe harfleri bozuk verir (ş→_, ı→1, İ→0, ğ→˜ ...).
// Bu karakterler joker sayılır; büyük/küçük harf ve Türkçe harfler katlanır.
const WILD = new Set(["_", "1", "0", "˜", "^", "˚", "º", "�"]);
const fold = (s) => s.toLocaleLowerCase("tr").replace(/[ıi̇]/g, "i").replace(/ş/g, "s").replace(/ğ/g, "g")
  .replace(/ü/g, "u").replace(/ö/g, "o").replace(/ç/g, "c").replace(/₺/g, "º").replace(/\s+/g, " ").trim();
function similarity(pdfText, domText) {
  const a = fold(pdfText), b = fold(domText);
  const eq = (x, y) => x === y || WILD.has(x);
  const dp = Array.from({ length: a.length + 1 }, (_, i) => [i, ...Array(b.length).fill(0)]);
  for (let j = 1; j <= b.length; j++) dp[0][j] = j;
  for (let i = 1; i <= a.length; i++)
    for (let j = 1; j <= b.length; j++)
      dp[i][j] = Math.min(dp[i - 1][j] + 1, dp[i][j - 1] + 1, dp[i - 1][j - 1] + (eq(a[i - 1], b[j - 1]) ? 0 : 1));
  return 1 - dp[a.length][b.length] / Math.max(a.length, b.length, 1);
}
const center = (r) => [(r[0] + r[2]) / 2, (r[1] + r[3]) / 2];
const dist = (p, q) => Math.hypot(p[0] - q[0], p[1] - q[1]);
const fmt = (v) => (v > 0 ? "+" : "") + v.toFixed(1);

const report = { text: [], icon: [], box: [] };
let matched = { text: 0, icon: 0, box: 0 };

// Her PDF satırı için en iyi DOM satırını bul; bir DOM satırı yalnızca en iyi
// eşleşen PDF satırıyla karşılaştırılır (PDF'teki gizli/tekrar katmanlar elenir).
const pairs = [];
for (const t of spec.texts) {
  let best = null;
  for (const d of dom.texts) {
    const sim = similarity(t.text, d.text);
    if (sim < 0.75) continue;
    const dd = dist(center(t.bbox), center([d.left, d.top, d.right, d.bottom]));
    if (dd > 150) continue;
    const score = sim - dd / 2000;
    if (!best || score > best.score) best = { t, d, sim, score };
  }
  if (best) pairs.push(best);
}
const bestForDom = new Map();
for (const p of pairs) {
  const cur = bestForDom.get(p.d);
  if (!cur || p.score > cur.score) bestForDom.set(p.d, p);
}

for (const { t, d, sim } of bestForDom.values()) {
  matched.text++;
  const pw = t.bbox[2] - t.bbox[0], dw = d.right - d.left;
  const dx = d.left - t.bbox[0];
  const dy = (d.top + d.bottom) / 2 - (t.bbox[1] + t.bbox[3]) / 2;
  const chars = d.text.length;
  const fsDiff = d.fontSize - t.size;
  const wDiff = dw - pw;
  const issues = [];
  if (Math.abs(fsDiff) > 0.3) issues.push(`font ${d.fontSize}px (PDF ${t.size}px)`);
  // Genişlik yalnızca satır içeriği aynıysa anlamlı (farklı kırılan satırlar hariç)
  const sameLine = sim >= 0.9 && Math.abs(fold(t.text).length - fold(d.text).length) <= 3;
  if (sameLine && Math.abs(wDiff) > Math.max(THRESHOLD, pw * 0.015)) {
    const ls = d.letterSpacing - wDiff / Math.max(chars - 1, 1);
    issues.push(`genişlik ${fmt(wDiff)}px → letter-spacing ≈ ${ls.toFixed(2)}px (şu an ${d.letterSpacing}px)`);
  }
  // Ortalı metinde genişlik farkı x'i de kaydırır; o durumda x ayrıca raporlanmaz
  if (Math.abs(dx) > THRESHOLD && !(sameLine && Math.abs(dx + wDiff / 2) <= THRESHOLD)) issues.push(`x ${fmt(dx)}px`);
  if (Math.abs(dy) > THRESHOLD) issues.push(`y ${fmt(dy)}px`);
  if (!sameLine) issues.push("(satır farklı kırılıyor)");
  if (issues.length && !(issues.length === 1 && !sameLine)) {
    report.text.push({ y: t.bbox[1], line: `"${d.text.slice(0, 40)}" (.${d.cls}): ${issues.join(", ")}` });
  }
}

const usedPdfIcons = new Set();
for (const ic of dom.icons) {
  const c = center([ic.left, ic.top, ic.right, ic.bottom]);
  let best = null;
  spec.icons.forEach((p, i) => {
    const dd = dist(c, center(p.bbox));
    if (dd < 24 && !usedPdfIcons.has(i) && (!best || dd < best.dd)) best = { p, i, dd };
  });
  if (!best) continue;
  usedPdfIcons.add(best.i);
  matched.icon++;
  const p = best.p.bbox;
  const dw = (ic.right - ic.left) - (p[2] - p[0]), dh = (ic.bottom - ic.top) - (p[3] - p[1]);
  const [cx, cy] = [c[0] - center(p)[0], c[1] - center(p)[1]];
  const issues = [];
  if (Math.abs(dw) > 1 || Math.abs(dh) > 1) {
    const drawn = Math.max(ic.right - ic.left, ic.bottom - ic.top);
    const target = Math.max(p[2] - p[0], p[3] - p[1]);
    const hint = ic.box ? ` → font-size ≈ ${(ic.box * target / drawn).toFixed(1)}px (kutu ${ic.box.toFixed(1)}px)` : "";
    issues.push(`çizilen boyut ${(ic.right - ic.left).toFixed(1)}×${(ic.bottom - ic.top).toFixed(1)} (PDF ${(p[2] - p[0]).toFixed(1)}×${(p[3] - p[1]).toFixed(1)})${hint}`);
  }
  if (Math.abs(cx) > THRESHOLD || Math.abs(cy) > THRESHOLD) issues.push(`merkez ${fmt(cx)}, ${fmt(cy)}px`);
  if (issues.length) report.icon.push({ y: p[1], line: `.${ic.cls} @${Math.round(p[0])},${Math.round(p[1])}: ${issues.join(", ")}` });
}

const iou = (a, b) => {
  const ix = Math.max(0, Math.min(a[2], b[2]) - Math.max(a[0], b[0]));
  const iy = Math.max(0, Math.min(a[3], b[3]) - Math.max(a[1], b[1]));
  const inter = ix * iy;
  return inter / ((a[2] - a[0]) * (a[3] - a[1]) + (b[2] - b[0]) * (b[3] - b[1]) - inter);
};
for (const b of spec.boxes) {
  const w = b.bbox[2] - b.bbox[0];
  if (w >= spec.width - 1 || b.bbox[0] < 0) continue; // tam genişlik zeminler
  let best = null;
  for (const d of dom.boxes) {
    // PDF kutusu dolgu kutusudur; DOM kutusu çerçeve dahil
    const outer = b.stroke ? [b.bbox[0] - b.stroke.width, b.bbox[1] - b.stroke.width, b.bbox[2] + b.stroke.width, b.bbox[3] + b.stroke.width] : b.bbox;
    const score = iou(outer, [d.left, d.top, d.right, d.bottom]);
    if (score > 0.85 && (!best || score > best.score)) best = { d, score, outer };
  }
  if (!best) continue;
  matched.box++;
  const { d, outer } = best;
  const edges = [["sol", d.left - outer[0]], ["üst", d.top - outer[1]], ["sağ", d.right - outer[2]], ["alt", d.bottom - outer[3]]]
    .filter(([, v]) => Math.abs(v) > THRESHOLD).map(([k, v]) => `${k} ${fmt(v)}px`);
  const issues = [...edges];
  if (b.stroke && Math.abs(d.borderWidth - b.stroke.width) > 0.3) issues.push(`çerçeve ${d.borderWidth}px (PDF ${b.stroke.width}px)`);
  if (issues.length) report.box.push({ y: b.bbox[1], line: `.${d.cls} @${Math.round(b.bbox[0])},${Math.round(b.bbox[1])} ${Math.round(w)}×${Math.round(b.bbox[3] - b.bbox[1])}: ${issues.join(", ")}` });
}

// ---------------------------------------------------------------- Rapor
console.log(`design-check: ${htmlArg} ↔ ${pdfArg} @ ${spec.width}px (eşik ${THRESHOLD}px)`);
console.log(`Eşleşen: ${matched.text}/${spec.texts.length} metin, ${matched.icon} ikon, ${matched.box} kutu\n`);
let total = 0;
for (const [key, title] of [["text", "METİN"], ["icon", "İKON"], ["box", "KUTU"]]) {
  const rows = report[key].sort((a, b) => a.y - b.y);
  total += rows.length;
  console.log(`${title} (${rows.length})`);
  rows.forEach((r) => console.log(`  y≈${Math.round(r.y)}  ${r.line}`));
  console.log("");
}
console.log(total ? `${total} fark bulundu.` : "Fark bulunmadı.");
process.exit(total ? 1 : 0);
