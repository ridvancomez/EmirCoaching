"""
Tasarım PDF'inden ölçü verisi çıkarır (design-check aracının 1. adımı).

Kullanım:
    python tools/design-check/extract_pdf.py "design/Diyet Planı Detay.pdf" > spec.json

Gereksinim: pip install pymupdf

Çıktı (JSON, PDF koordinatları = 1440px artboard pikseli):
    texts : görsel satır başına metin, font boyutu, bbox (font kutusu)
    boxes : dolgulu dikdörtgenler; içinde çerçevesi varsa stroke kalınlığı
    icons : 40px'ten küçük, metin olmayan vektör şekiller (çizilen boyut)
"""
import json
import sys

import pymupdf


def color_hex(rgb):
    if rgb is None:
        return None
    return "#%02x%02x%02x" % tuple(int(round(v * 255)) for v in rgb)


def extract_texts(page):
    lines = []
    for block in page.get_text("dict")["blocks"]:
        if block["type"] != 0:
            continue
        for line in block["lines"]:
            text = "".join(span["text"] for span in line["spans"]).replace("\n", " ").strip()
            if not text:
                continue
            lines.append({
                "text": text,
                "size": round(max(span["size"] for span in line["spans"]), 2),
                "bbox": [round(v, 1) for v in line["bbox"]],
            })

    # PDF bir görsel satırı birden çok parçaya bölebilir: aynı satırda ve
    # yan yana duran parçaları tek satırda birleştir.
    lines.sort(key=lambda l: (round(l["bbox"][1]), l["bbox"][0]))
    merged = []
    for line in lines:
        prev = merged[-1] if merged else None
        if (prev
                and abs(prev["bbox"][1] - line["bbox"][1]) < 2
                and abs(prev["size"] - line["size"]) < 0.5
                and -2 <= line["bbox"][0] - prev["bbox"][2] < line["size"] * 2.5):
            prev["text"] += ("" if line["bbox"][0] - prev["bbox"][2] < line["size"] * 0.2 else " ") + line["text"]
            prev["bbox"][2] = max(prev["bbox"][2], line["bbox"][2])
            prev["bbox"][3] = max(prev["bbox"][3], line["bbox"][3])
        else:
            merged.append(dict(line, bbox=list(line["bbox"])))
    return merged


def extract_shapes(page, text_boxes):
    def inside_text(r):
        return any(t[0] - 1 <= r.x0 and r.x1 <= t[2] + 1 and t[1] - 1 <= r.y0 and r.y1 <= t[3] + 1
                   for t in text_boxes)

    shapes = []
    for d in page.get_drawings():
        r = d["rect"]
        fill = d.get("fill")
        if r.width < 1 or r.height < 1 or fill is None:
            continue
        if fill == (0.0, 0.0, 0.0) and (d.get("fill_opacity") or 1) == 1 and len(d["items"]) <= 8:
            # Figma'nın gölge/maske için koyduğu siyah kopyalar
            continue
        shapes.append({
            "rect": r,
            "fill": color_hex(fill),
            "opacity": round(d.get("fill_opacity") or 1, 2),
            "items": len(d["items"]),
            "is_text": inside_text(r),
        })

    boxes, icons = [], []
    for s in shapes:
        r = s["rect"]
        if s["is_text"]:
            continue
        if r.width <= 40 and r.height <= 40:
            icons.append({"bbox": [round(v, 1) for v in r], "fill": s["fill"]})
        elif r.width > 40 and r.height > 20:
            boxes.append(s)

    # Çerçeve tespiti: bir şekil, başka bir dolgunun etrafını her yandan
    # eşit ve ince (<5px) bir payla sarıyorsa o pay çerçeve kalınlığıdır.
    result = []
    for b in boxes:
        r = b["rect"]
        stroke = None
        for o in boxes:
            q = o["rect"]
            if o is b or o["items"] <= b["items"]:
                continue
            m = [r.x0 - q.x0, r.y0 - q.y0, q.x1 - r.x1, q.y1 - r.y1]
            if all(0.3 < v < 5 for v in m) and max(m) - min(m) < 0.4:
                stroke = {"width": round(sum(m) / 4, 2), "color": o["fill"], "opacity": o["opacity"]}
                break
        result.append({
            "bbox": [round(v, 1) for v in r],
            "fill": b["fill"],
            "opacity": b["opacity"],
            "stroke": stroke,
        })

    # Çerçevenin kendisi olan halkaları (başka bir kutuyu saranlar) listeden çıkar
    rings = set()
    for b in result:
        if b["stroke"]:
            for o in result:
                if o is not b and o["fill"] == b["stroke"]["color"]:
                    x0, y0, x1, y1 = o["bbox"]
                    bx0, by0, bx1, by1 = b["bbox"]
                    if abs((bx0 - x0) - (x1 - bx1)) < 0.4 and 0.3 < bx0 - x0 < 5:
                        rings.add(id(o))
    result = [b for b in result if id(b) not in rings]
    return result, icons


def main():
    if len(sys.argv) < 2:
        sys.exit(__doc__)
    doc = pymupdf.open(sys.argv[1])
    page = doc[0]
    texts = extract_texts(page)
    boxes, icons = extract_shapes(page, [t["bbox"] for t in texts])
    json.dump({
        "width": page.rect.width,
        "height": page.rect.height,
        "texts": texts,
        "boxes": boxes,
        "icons": icons,
    }, sys.stdout, ensure_ascii=False, indent=1)


if __name__ == "__main__":
    main()
