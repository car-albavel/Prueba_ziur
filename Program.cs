using Prueba_Ziur.Components;
using Prueba_Ziur.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOptions<ZiurApiOptions>()
    .Bind(builder.Configuration.GetSection(ZiurApiOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<IZiurCatalogService, ZiurCatalogService>((sp, client) =>
{
    client.Timeout = TimeSpan.FromSeconds(
        sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<ZiurApiOptions>>().Value.TimeoutSeconds);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
