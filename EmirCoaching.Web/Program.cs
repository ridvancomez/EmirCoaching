var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// Statik site (wwwroot); veri EmirCoaching.API'den gelir
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
