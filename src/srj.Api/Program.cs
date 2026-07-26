using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using srj.Application.Interface.Repository;
using srj.Application.Interface.Services;
using srj.Application.Interface.Services.Authentication;
using srj.Application.Interface.Services.Dashboard;
using srj.Application.Interface.Services.Gold;
using srj.Application.Interface.Services.Items;
using srj.Application.Interface.Services.Jewelery;
using srj.Application.Interface.Services.Silver;
using srj.Application.Services;
using srj.Application.Services.Dashboard;
using srj.Application.Services.Gold;
using srj.Application.Services.Items;
using srj.Application.Services.Jewelery;
using srj.Application.Services.Silver;
using srj.Infrastructure.DbContext;
using srj.Infrastructure.Identity;
using srj.Infrastructure.Repositories;
using TheSRJProject.Service;

var builder = WebApplication.CreateBuilder(args);

#region Controllers

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

#endregion

#region Swagger

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SRJ API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token only. Swagger will prepend Bearer automatically.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

#region Database

builder.Services.AddDbContext<JewelryDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Identity

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<JewelryDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

#endregion

#region Authentication

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

#endregion

#region Dependency Injection

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IJewelryItemRepository, JewelryItemRepository>();
builder.Services.AddScoped<IItemCategoryRepository, ItemCategoryRepository>();

builder.Services.AddScoped<IGoldRateRepository, GoldRateRepository>();
builder.Services.AddScoped<ISilverRateRepository, SilverRateRepository>();

builder.Services.AddScoped<IItemCategoryService, ItemCategoryService>();

builder.Services.AddScoped<IJewelryQueryService, JewelryQueryService>();

builder.Services.AddScoped<IGoldJewelryItemService, GoldJewelryItemService>();
builder.Services.AddScoped<ISilverJewelryItemService, SilverJewelryItemService>();

builder.Services.AddScoped<IGoldRateService, GoldRateService>();
builder.Services.AddScoped<ISilverRateService, SilverRateService>();

builder.Services.AddScoped<IGoldJewelryCalculationService, GoldJewelryCalculationService>();
builder.Services.AddScoped<ISilverJewelryCalculationService, SilverJewelryCalculationService>();

builder.Services.AddScoped<ISkuGenerator, SkuGenerator>();
builder.Services.AddScoped<IBarcodeService, BarcodeService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IIndiaDateTimeService, IndiaDateTimeService>();

builder.Services.AddScoped<IDashboardService, DashboardService>();

#endregion

var app = builder.Build();

#region Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();