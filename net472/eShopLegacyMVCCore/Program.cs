
using eShopLegacy.Models;
using eShopLegacyMVC.Models;
using eShopLegacyMVC.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSystemWebAdapters()
    .AddJsonSessionSerializer(options =>
    {
        options.RegisterKey<string>("MachineName");
        options.RegisterKey<DateTime>("SessionStartTime");
        options.RegisterKey<SessionDemoModel>("DemoItem");
    })
    .AddRemoteAppClient(options =>
    {
        options.RemoteAppUrl = new Uri(builder.Configuration["ProxyTo"]);
        options.ApiKey = builder.Configuration["RemoteAppApiKey"];
    })
    .AddSessionClient();

builder.Services.AddHttpForwarder();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<CatalogDBContext>();
builder.Services.AddSingleton<CatalogItemHiLoGenerator>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"]).Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

app.MapControllerRoute("Default", "{controller=Catalog}/{action=Index}/{id?}")
    .RequireSystemWebAdapterSession();

app.Run();