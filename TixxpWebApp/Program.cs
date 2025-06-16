using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Tixxp.Business;
using Tixxp.Infrastructure;
using Tixxp.WebApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBusinessServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddTixxpDbContext(builder.Configuration);

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
