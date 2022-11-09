using DS2022_30442_Presecan_Alexandru_Assignment_1.Data;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Services;
using DS2022_30442_Presecan_Alexandru_Assignment_1_Backend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<DataContext>()
    .AddScoped<UserService>()
    .AddScoped<DeviceService>()
    .AddScoped<EnergyConsumptionService>()
    .AddAuthorization()
    .AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.UseAuthentication();
app.UseAuthorization();

app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
AdminInitializer.Initialize(app.Services.CreateScope().ServiceProvider);

app.Run();