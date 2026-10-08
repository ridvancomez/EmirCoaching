// Gizlilik Politikası PDF üretici
// gizlilik-politikasi.html'deki politika metnini (başlık, özet, meta, bölümler, tablo,
// revizyon geçmişi) okur, yazdırmaya uygun beyaz zeminli bir A4 belgesine yerleştirir ve
// headless Edge/Chrome ile images/gizlilik-politikasi.pdf olarak kaydeder.
// Metin HTML'den alındığı için sayfa güncellenince yeniden çalıştırmak yeterlidir:
//   node tools/gizlilik-pdf/build.mjs
import { spawn } from "node:child_process";
import { existsSync, mkdtempSync, readFileSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath, pathToFileURL } from "node:url";

const ROOT = resolve(dirname(fileURLToPath(import.meta.url)), "../..");
const SOURCE = join(ROOT, "gizlilik-politikasi.html");
const OUTPUT = join(ROOT, "images", "gizlilik-politikasi.pdf");
const BROWSERS = [
  process.env.BROWSER,
  "C:/Program Files (x86)/Microsoft/Edge/Application/msedge.exe",
  "C:/Program Files/Microsoft/Edge/Application/msedge.exe",
  "C:/Program Files/Google/Chrome/Application/chrome.exe",
  "/usr/bin/google-chrome",
  "/usr/bin/chromium",
].filter(Boolean);

// --- 1) İçeriği sayfadan al -------------------------------------------------
const html = readFileSync(SOURCE, "utf8");
const pick = (pattern, label) => {
  const match = html.match(pattern);
  if (!match) throw new Error(`gizlilik-politikasi.html içinde bulunamadı: ${label}`);
  return match[1].trim();
};
const stripIcons = (s) => s.replace(/<iconify-icon[^>]*>\s*<\/iconify-icon>/g, "");

const title = pick(/<h1[^>]*class="gizlilik-hero__title"[^>]*>([\s\S]*?)<\/h1>/, "başlık");
const lead = pick(/<p class="gizlilik-hero__lead">([\s\S]*?)<\/p>/, "özet");
const meta = pick(/(<dl class="gizlilik-meta">[\s\S]*?<\/dl>)/, "meta");
const article = stripIcons(pick(/<article class="gizlilik-body__content"[^>]*>([\s\S]*?)<\/article>/, "politika metni"));
const metaText = (label) => {
  const re = new RegExp(`${label}</dt>\\s*<dd[^>]*>(?:<time[^>]*>)?([^<]+)`);
  return (meta.match(re) || [])[1] || "";
};
const version = metaText("Versiyon");

// --- 2) Yazdırma belgesi ----------------------------------------------------
const doc = `<!DOCTYPE html>
<html lang="tr">
<head>
<meta charset="UTF-8">
<title>${title} — EMIRCOACHING</title>
<link href="https://fonts.googleapis.com/css2?family=Geologica:wght@600;700&family=Inter:wght@400;500;600&family=JetBrains+Mono&display=swap" rel="stylesheet">
<style>
  @page { size: A4; margin: 24mm 18mm 20mm; }
  * { box-sizing: border-box; }
  body { margin: 0; font-family: Inter, sans-serif; font-size: 10.5pt; line-height: 1.6; color: #2a2f3d; }
  a { color: #1b6dff; text-decoration: none; }
  .brand { display: flex; justify-content: space-between; align-items: baseline; padding-bottom: 10pt; border-bottom: 2pt solid #000636; }
  .brand__name { font-family: Geologica, sans-serif; font-size: 15pt; font-weight: 700; letter-spacing: 0.02em; color: #000636; }
  .brand__name span { color: #1b6dff; }
  .brand__doc { font-family: Geologica, sans-serif; font-size: 9pt; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #6b7080; }
  h1 { margin: 22pt 0 0; font-family: Geologica, sans-serif; font-size: 26pt; line-height: 1.2; color: #000636; }
  .lead { margin: 8pt 0 0; font-size: 11pt; color: #4a5060; }
  .gizlilik-meta { display: flex; gap: 28pt; margin: 16pt 0 0; padding: 10pt 0; border-top: 0.75pt solid #d5d9e4; border-bottom: 0.75pt solid #d5d9e4; }
  .gizlilik-meta dt { font-family: Geologica, sans-serif; font-size: 7.5pt; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #6b7080; }
  .gizlilik-meta dd { margin: 2pt 0 0; font-size: 10.5pt; font-weight: 500; color: #000636; }
  .gizlilik-bolum { margin-top: 20pt; break-inside: avoid; }
  .gizlilik-bolum > * + * { margin-top: 8pt; }
  .gizlilik-bolum__title { margin: 0; font-family: Geologica, sans-serif; font-size: 15pt; line-height: 1.3; color: #000636; break-after: avoid; }
  .gizlilik-bolum__no { display: inline-block; min-width: 1.6em; color: #1b6dff; }
  p { margin: 0; }
  .gizlilik-liste { padding: 0; list-style: none; }
  .gizlilik-liste__item { position: relative; padding-left: 16pt; }
  .gizlilik-liste__item + .gizlilik-liste__item { margin-top: 4pt; }
  .gizlilik-liste__item::before { content: ""; position: absolute; top: 0.62em; left: 4pt; width: 4.5pt; height: 4.5pt; border-radius: 50%; background: #1b6dff; }
  .gizlilik-liste--tanim dt, .gizlilik-liste--tanim dd { display: inline; margin: 0; }
  .gizlilik-liste--tanim dt { font-weight: 600; color: #000636; }
  .gizlilik-tablo { break-inside: avoid; }
  table { width: 100%; border-collapse: collapse; font-size: 9.5pt; line-height: 1.4; text-align: left; }
  th, td { padding: 7pt 8pt; border-bottom: 0.75pt solid #d5d9e4; vertical-align: top; }
  thead th { white-space: nowrap; background: #eef1f7; font-family: Geologica, sans-serif; font-size: 7.5pt; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #4a5060; }
  tbody th { font-weight: 600; color: #000636; }
  .gizlilik-callout { padding: 10pt 14pt; border-left: 3pt solid #1b6dff; background: #eef4ff; break-inside: avoid; }
  .gizlilik-callout__title { font-family: Geologica, sans-serif; font-size: 8pt; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #000636; }
  .gizlilik-callout__text { margin-top: 3pt; }
  .gizlilik-revizyon { margin-top: 24pt; padding-top: 12pt; border-top: 0.75pt solid #d5d9e4; break-inside: avoid; }
  .gizlilik-revizyon__title { margin: 0; font-family: Geologica, sans-serif; font-size: 8pt; font-weight: 600; letter-spacing: 0.08em; text-transform: uppercase; color: #000636; }
  .gizlilik-revizyon__list { margin: 6pt 0 0; padding: 0; list-style: none; font-family: "JetBrains Mono", monospace; font-size: 9pt; line-height: 1.8; color: #4a5060; }
</style>
</head>
<body>
  <header class="brand">
    <span class="brand__name">EMIR<span>COACHING</span></span>
    <span class="brand__doc">${title} · <span style="text-transform:none">${version}</span></span>
  </header>
  <h1>${title}</h1>
  <p class="lead">${lead}</p>
  ${meta}
  <main>${article}</main>
</body>
</html>`;

const work = mkdtempSync(join(tmpdir(), "gizlilik-pdf-"));
const docPath = join(work, "gizlilik-politikasi.print.html");
writeFileSync(docPath, doc);

// --- 3) Headless tarayıcı ile PDF ---------------------------------------------
const exe = BROWSERS.find((path) => existsSync(path));
if (!exe) throw new Error("Edge/Chrome bulunamadı; BROWSER ortam değişkeniyle yol verin.");
const port = 9300 + Math.floor(Math.random() * 500);
const browser = spawn(exe, ["--headless=new", "--disable-gpu", `--remote-debugging-port=${port}`, `--user-data-dir=${join(work, "profile")}`, "about:blank"], { stdio: "ignore" });
const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

try {
  let target;
  for (let i = 0; i < 50 && !target; i++) {
    await sleep(200);
    try {
      target = (await (await fetch(`http://127.0.0.1:${port}/json/list`)).json()).find((t) => t.type === "page");
    } catch {}
  }
  if (!target) throw new Error("Tarayıcıya bağlanılamadı.");
  const ws = new WebSocket(target.webSocketDebuggerUrl);
  await new Promise((r) => (ws.onopen = r));
  let id = 0;
  const pending = new Map();
  ws.onmessage = (e) => {
    const m = JSON.parse(e.data);
    if (pending.has(m.id)) {
      pending.get(m.id)(m);
      pending.delete(m.id);
    }
  };
  const send = (method, params = {}) => new Promise((r) => {
    const i = ++id;
    pending.set(i, r);
    ws.send(JSON.stringify({ id: i, method, params }));
  });

  await send("Page.enable");
  await send("Page.navigate", { url: pathToFileURL(docPath).href });
  await sleep(1500);
  await send("Runtime.evaluate", { expression: "document.fonts.ready.then(() => true)", awaitPromise: true });

  const footer = `<div style="width:100%;padding:0 18mm;font-family:Inter,sans-serif;font-size:7pt;color:#8c90a1;display:flex;justify-content:space-between;">
    <span>EMIRCOACHING · ${title} · ${version} · Yürürlük: ${metaText("Yürürlük Tarihi")} · Son güncelleme: ${metaText("Son Güncelleme")}</span>
    <span>Sayfa <span class="pageNumber"></span> / <span class="totalPages"></span></span></div>`;
  const result = await send("Page.printToPDF", {
    printBackground: true,
    preferCSSPageSize: true,
    displayHeaderFooter: true,
    headerTemplate: "<span></span>",
    footerTemplate: footer,
  });
  if (result.error) throw new Error(result.error.message);
  writeFileSync(OUTPUT, Buffer.from(result.result.data, "base64"));
  ws.close();
  console.log(`PDF yazıldı: ${OUTPUT}`);
} finally {
  browser.kill();
}
