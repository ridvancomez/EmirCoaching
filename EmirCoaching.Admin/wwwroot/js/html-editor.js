// CKEditor 5 (classic build, CDN) ile Blazor arasındaki köprü. Components/HtmlEditor.razor kullanır.
// Editör HTML üretir; içerik API'ye ContentHtml olarak gider (blog yazısı, yasal metin).
window.adminHtmlEditor = {
  create: async function (element, html, dotnet) {
    const editor = await ClassicEditor.create(element, {
      language: "tr",
      toolbar: [
        "heading", "|", "bold", "italic", "link", "|",
        "bulletedList", "numberedList", "blockQuote", "insertTable", "|",
        "undo", "redo"
      ],
      heading: {
        options: [
          { model: "paragraph", title: "Paragraf", class: "ck-heading_paragraph" },
          { model: "heading2", view: "h2", title: "Başlık", class: "ck-heading_heading2" },
          { model: "heading3", view: "h3", title: "Alt başlık", class: "ck-heading_heading3" }
        ]
      }
    });
    editor.setData(html || "");

    // Her tuşta değil, yazma durunca Blazor'a bildirilir
    let timer;
    editor.model.document.on("change:data", function () {
      clearTimeout(timer);
      timer = setTimeout(function () { dotnet.invokeMethodAsync("OnEditorChanged", editor.getData()); }, 300);
    });
    element._adminEditor = editor;
  },

  getData: function (element) {
    return element && element._adminEditor ? element._adminEditor.getData() : "";
  },

  setData: function (element, html) {
    if (element && element._adminEditor) element._adminEditor.setData(html || "");
  },

  destroy: function (element) {
    if (element && element._adminEditor) {
      element._adminEditor.destroy();
      element._adminEditor = null;
    }
  }
};
