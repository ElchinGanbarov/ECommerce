using Castle.Components.DictionaryAdapter.Xml;
using ECommerce.Application;
using ECommerce.Application.Const;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastucture;
using ECommerce.Infrastucture.Filters;
using ECommerce.Persistence;
using ECommerce.Persistence.Contexts;
using ECommerceMVC.Extensions;
using ECommerceMVC.Filters;
using ECommerceMVC.MIddlewares;
using ECommerceMVC.Models;
using Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using System.Globalization;
using System.Net;
using System.Security.Claims;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
//builder.Services.AddControllersWithViews();


builder.Configuration
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true, reloadOnChange: true);


builder.Services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);



builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
});

builder.Services.AddLocalization(opt =>
{
    opt.ResourcesPath = "Resources";
});


builder.Services.AddRazorPages()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

//builder.Services.Configure<RequestLocalizationOptions>(options =>
//{
//    var supportedCultures = new[]
//    {
//        new CultureInfo("en-US"),
//        new CultureInfo("az-Latn-AZ")
//    };

//    options.SupportedCultures = supportedCultures;
//    options.SupportedUICultures = supportedCultures;
//    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");

//});
builder.Configuration.GetSection(nameof(Config.Mail)).Bind(Config.Mail);
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = GoogleDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = GoogleDefaults.AuthenticationScheme;

    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    })
    .AddGoogle(googleoptions =>
    {
        googleoptions.ClientId = builder.Configuration["ExternalLoginSettings:Google:Client_ID"];
        googleoptions.ClientSecret = builder.Configuration["ExternalLoginSettings:Google:Client_Secret"];
        googleoptions.CallbackPath = "/signin-google";
    });


builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();//Client'tan gelen request neticvesinde oluşturulan HttpContext nesnesine katmanlardaki class'lar üzerinden(busineess logic) erişebilmemizi sağlayan bir servistir.
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //Oluşturulacak token değerini kimlerin/hangi originlerin/sitelerin kullanıcı belirlediğimiz değerdir. -> www.bilmemne.com
            ValidateIssuer = true, //Oluşturulacak token değerini kimin dağıttını ifade edeceğimiz alandır. -> www.myapi.com
            ValidateLifetime = true, //Oluşturulan token değerinin süresini kontrol edecek olan doğrulamadır.
            ValidateIssuerSigningKey = true, //Üretilecek token değerinin uygulamamıza ait bir değer olduğunu ifade eden suciry key verisinin doğrulanmasıdır.
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karşılık gelen değeri User.Identity.Name propertysinden elde edebiliriz.,

        };
    });

var app = builder.Build();

//app.Services.CreateScope().ServiceProvider.GetRequiredService<ECommerceDbContext>().Database.Migrate(); // deploying to prod db migrate

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{

    app.ApplyMigrations();

}

app.UseRequestLocalization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();




// Add a custom route to handle the redirection
app.UseStatusCodePages(async context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
        response.Redirect("/account/login");
});

app.UseExceptionHandlerMiddleware();


app.MapControllerRoute(
    name: "LocalizedDefault",
    pattern: "{lang:lang=en}/{controller}/{action}/{id?}");

app.MapControllerRoute(
                    name: "Area",
                    pattern: "{area=Admins}/{controller=Dashboard}/{action=Index}/{id?}"
                    );


app.Run();

