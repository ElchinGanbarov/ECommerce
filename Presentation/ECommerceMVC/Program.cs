using Castle.Components.DictionaryAdapter.Xml;
using ECommerce.Application;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastucture;
using ECommerce.Infrastucture.Filters;
using ECommerce.Persistence;
using ECommerce.Persistence.Contexts;
using ECommerceMVC.Extensions;
using ECommerceMVC.Filters;
using ECommerceMVC.Models;
using Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using System.Net;
using System.Security.Claims;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

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
builder.Services.AddHttpContextAccessor();//Client'tan gelen request neticvesinde oluþturulan HttpContext nesnesine katmanlardaki class'lar üzerinden(busineess logic) eriþebilmemizi saðlayan bir servistir.
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanýcý belirlediðimiz deðerdir. -> www.bilmemne.com
            ValidateIssuer = true, //Oluþturulacak token deðerini kimin daðýttýný ifade edeceðimiz alandýr. -> www.myapi.com
            ValidateLifetime = true, //Oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr.
            ValidateIssuerSigningKey = true, //Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden suciry key verisinin doðrulanmasýdýr.
            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
            NameClaimType = ClaimTypes.Name //JWT üzerinde Name claimne karþýlýk gelen deðeri User.Identity.Name propertysinden elde edebiliriz.,

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



