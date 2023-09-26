using ECommerce.Application;
using ECommerce.Infrastucture;
using ECommerce.Persistence;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();//Client'tan gelen request neticvesinde oluþturulan HttpContext nesnesine katmanlardaki class'lar üzerinden(busineess logic) eriþebilmemizi saðlayan bir servistir.
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices(); 
builder.Services.AddInfrastructureServices();


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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
                    name: "Area",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                    );
app.Run();
