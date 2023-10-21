using ECommerce.Application.Abstractions.Services.Authentications;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Repositories.Endpoints;
using ECommerce.Application.Repositories.Products;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repositories.Endpoints;
using ECommerce.Persistence.Repositories.Products;
using ECommerce.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ECommerceDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                
            }).AddEntityFrameworkStores<ECommerceDbContext>()
            .AddDefaultTokenProviders();


            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IExternalAuthentication, AuthService>();
            services.AddTransient<IInternalAuthentication, AuthService>();


        }

    }
}
