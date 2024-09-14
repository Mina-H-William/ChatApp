using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReactApp.Server.Data;
using ReactApp.Server.Hubs;
using ReactApp.Server.Models;
using System.Text;

namespace ReactApp.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging(false)  // Disables sensitive data logging
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.Warning))));


            // Add Identity services
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });


            builder.Services.AddAuthorization();

            builder.Services.AddDistributedMemoryCache(); // Required for session to work

            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true; // Make the session cookie accessible only to the server
                options.Cookie.IsEssential = true; // Make the session cookie essential
            });

            builder.Services.AddSignalR();

            builder.Services.AddControllers();

            builder.Services.AddSingleton<UserConnectionManager>();


            var app = builder.Build();


            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization(); 

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.MapHub<ChatHub>("/chathub");

            app.Run();
        }
    }
}
