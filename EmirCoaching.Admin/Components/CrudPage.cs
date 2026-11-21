using EmirCoaching.Admin.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace EmirCoaching.Admin.Components;

// Basit kaynak sayfalarının ortak akışı: liste → "Yeni ekle"/"Düzenle" penceresi → kaydet → listeyi yenile; tekli ve toplu silme.
// TForm = Create isteği; güncellemede aynı alanlar Update isteğine kopyalanır (UpdateAsync içinde).
// Sayfa yalnızca API çağrılarını ve satırdan forma çeviriyi verir.
public abstract class CrudPage<TItem, TForm> : ComponentBase
    where TItem : class
    where TForm : class, new()
{
    [Inject] protected ApiCaller Api { get; set; } = default!;
    [Inject] protected IDialogService Dialogs { get; set; } = default!;

    protected List<TItem> Items { get; set; } = [];
    protected HashSet<TItem> Selected { get; set; } = [];
    protected bool Loading { get; set; } = true;

    protected bool DialogOpen { get; set; }
    protected bool Saving { get; set; }
    protected TForm Form { get; set; } = new();
    protected int? EditingId { get; set; }

    // Pencere başlığı için: "Yeni soru" / "Soruyu düzenle"
    protected abstract string NewTitle { get; }
    protected abstract string EditTitle { get; }
    protected string DialogTitle => EditingId is null ? NewTitle : EditTitle;

    protected abstract Task<List<TItem>> FetchAsync();
    protected abstract int IdOf(TItem item);
    protected abstract string LabelOf(TItem item);
    protected abstract Task CreateAsync(TForm form);
    protected abstract Task UpdateAsync(int id, TForm form);
    protected abstract Task DeleteAsync(int id);
    protected abstract Task DeleteManyAsync(List<int> ids);

    // Varsayılan: aynı adlı alanlar kopyalanır; *Url → *Path gibi farklar sayfada ezilir
    protected virtual Task<TForm?> ToFormAsync(TItem item) => Task.FromResult<TForm?>(ObjectCopy.To<TForm>(item));

    // Yeni kayıt varsayılanları (ör. sıra = son + 1, aktif = true)
    protected virtual TForm NewForm() => new();

    protected override Task OnInitializedAsync() => ReloadAsync();

    protected async Task ReloadAsync()
    {
        Loading = true;
        Items = await Api.GetAsync(FetchAsync) ?? [];
        Selected = [];
        Loading = false;
    }

    protected void OpenCreate()
    {
        EditingId = null;
        Form = NewForm();
        DialogOpen = true;
    }

    protected async Task OpenEditAsync(TItem item)
    {
        var form = await ToFormAsync(item);
        if (form is null)
            return;
        Form = form;
        EditingId = IdOf(item);
        DialogOpen = true;
    }

    protected async Task SaveAsync()
    {
        Saving = true;
        var ok = await Api.RunAsync(() => EditingId is { } id ? UpdateAsync(id, Form) : CreateAsync(Form), "Kaydedildi.");
        Saving = false;
        if (!ok)
            return;
        DialogOpen = false;
        await ReloadAsync();
    }

    protected async Task ConfirmDeleteAsync(TItem item)
    {
        if (!await ConfirmAsync($"\"{LabelOf(item)}\" silinsin mi?"))
            return;
        if (await Api.RunAsync(() => DeleteAsync(IdOf(item)), "Silindi."))
            await ReloadAsync();
    }

    // API ya hepsini siler ya hiçbirini; engel varsa sebepleri mesajda gelir
    protected async Task ConfirmDeleteSelectedAsync()
    {
        var ids = Selected.Select(IdOf).ToList();
        if (ids.Count == 0 || !await ConfirmAsync($"Seçilen {ids.Count} kayıt silinsin mi?"))
            return;
        if (await Api.RunAsync(() => DeleteManyAsync(ids), $"{ids.Count} kayıt silindi."))
            await ReloadAsync();
    }

    protected async Task<bool> ConfirmAsync(string message, string yesText = "Sil") =>
        await Dialogs.ShowMessageBoxAsync("Onay", message, yesText: yesText, cancelText: "Vazgeç") == true;

    // Sıralı listelerde yeni kayıt sona eklenir
    protected int NextSortOrder(Func<TItem, int> sortOrder) => Items.Count == 0 ? 1 : Items.Max(sortOrder) + 1;
}
