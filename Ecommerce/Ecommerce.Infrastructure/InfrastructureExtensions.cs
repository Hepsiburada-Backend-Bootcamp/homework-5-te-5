using System;
using System.Data;
using System.Text;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Repositories;
using Ecommerce.Infrastructure.Context;
using Ecommerce.Infrastructure.DapperRepository;
using Ecommerce.Infrastructure.MongoRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Npgsql;

namespace Ecommerce.Infrastructure
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<EcommerceDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                        b => b.MigrationsAssembly("Ecommerce.API"))
                .UseSnakeCaseNamingConvention());

            //services.AddDefaultIdentity<User>().AddEntityFrameworkStores<EcommerceDbContext>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Tokens.AuthenticatorIssuer = configuration.GetSection("Jwt")["Issuer"];
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<EcommerceDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetSection("Jwt")["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("Jwt")["Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt")["Key"])),
                    //RequireExpirationTime = true,
                    ValidateLifetime = true,

                };
            });


            services.Configure<OrderDatabaseSettings>(configuration.GetSection(nameof(OrderDatabaseSettings)));
            services.AddSingleton<IOrderDatabaseSettings>(sp => 
                sp.GetRequiredService<IOptions<OrderDatabaseSettings>>().Value);
            
            services.AddScoped<IOrderMongoContext, OrderMongoContext>();
            
            services.AddScoped<IProductRepository,DapperProductRepository>();
            services.AddScoped<IOrderRepository, DapperOrderRepository>();
            services.AddScoped<IOrderRecordRepository, OrderMongoRepository>();

            services.AddScoped<IDbConnection>(db=>new NpgsqlConnection(configuration.GetConnectionString("PostgresConnection")));
            return services;
        }
    }
}