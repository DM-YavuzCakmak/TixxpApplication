using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using Tixxp.Business;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.CurrentUser;
using Tixxp.Business.Services.Concrete.Log;
using Tixxp.Infrastructure;
using Tixxp.WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Localization
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(); // BUNU EKLE

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("tr-TR"),
        new CultureInfo("en-US"),
        new CultureInfo("el-GR"),
        new CultureInfo("ja-JP"),
        new CultureInfo("fr-FR"),
        new CultureInfo("zh-CN"),
        new CultureInfo("ko-KR"),
        new CultureInfo("es-AR"),
        new CultureInfo("ru-RU")
    };

    options.DefaultRequestCulture = new RequestCulture("en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    // 🔥 Buraya ekliyoruz
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider(),
        new AcceptLanguageHeaderRequestCultureProvider()
    };
});
#endregion


// Services
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpCurrentUser>();
builder.Services.AddBusinessServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddTixxpDbContext(builder.Configuration);
builder.Services.AddHttpClient<ILogService, LogService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:44332/api/Indexes/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// ✅ Cookie Policy (IIS ve Chrome için önemli)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.Lax; // 'None' da olabilir
});

// ✅ Authentication servisi
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authorization/Index";
        options.AccessDeniedPath = "/Authorization/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax; // Cookie’nin gönderilmesini engellemesin
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // IIS HTTP'de sorun çıkmasın diye
        options.Cookie.Name = "TixxpAuth"; // Özel isim vererek çakışmayı önle
    });

var app = builder.Build();
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);
app.Use(async (context, next) =>
{
    var request = context.Request;
    var culture = context.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;

    if (request.Method == "GET" && !request.Query.ContainsKey("culture") && culture != null)
    {
        var query = request.QueryString.HasValue ? request.QueryString.Value + "&" : "?";
        var newUrl = $"{request.Path}{query}culture={culture}&ui-culture={culture}";
        context.Response.Redirect(newUrl);
        return;
    }

    await next();
}); ;

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Unexpected");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ IIS Proxy ve Cookie için Forwarded Headers
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// ✅ Cookie policy middleware
app.UseCookiePolicy();

// ✅ Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authorization}/{action=Index}/{id?}");

app.MapControllers();
app.Run();
