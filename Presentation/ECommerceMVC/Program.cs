using ECommerce.Application;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastucture;
using ECommerce.Infrastucture.Filters;
using ECommerce.Persistence;
using ECommerce.Persistence.Contexts;
using ECommerceMVC.Filters;
using Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using System.Net;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();


builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ValidationFilter>();
    //options.Filters.Add<RolePermissionFilter>();
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

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
    // you may also check requests path to do this only for specific methods       
    // && request.Path.Value.StartsWith("/specificPath")

    {
        response.Redirect("/account/login");
    }
});

app.UseMiddleware<ExceptionHandlerMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
                    name: "Area",
                    pattern: "{area=Admins}/{controller=Dashboard}/{action=Index}/{id?}"
                    );


app.Run();
