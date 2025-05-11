using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProductDemo.Web.Repositories.Interfaces;
using ProductDemo.Web.Repositories;
using ProductDemo.Web.Services.Interfaces;
using ProductDemo.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ProductDemo.Web.Helpers;
using ProductDemo.Web.Helpers.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis;
using ProductDemo.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

// Register Dapper context
builder.Services.AddSingleton<DapperContext>();

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtBearer").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);
// Register Repositories and Services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAdminUserRepository, AdminUserRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtHelper>();

#region Authorization


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtBearer:AccessKey"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["JwtBearer:Issuer"],
                ValidAudience = builder.Configuration["JwtBearer:Audience"],
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["AccessToken"];
                    return Task.CompletedTask;
                }
            };
        }
        );

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
