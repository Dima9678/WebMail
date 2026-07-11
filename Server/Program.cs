using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Persistence;
using Server.Service;
using Server.Validators;

//cloudflared tunnel --url http://localhost:49981

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Username=Mail;Password=11111111;Database=WebMail");
            });

            builder.Services.AddScoped<ValidationCheck>();
            builder.Services.AddScoped<LetterService>();

            builder.Services
            .AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {

                options.LoginPath = "/api/User/login";
                options.Cookie.Name = "auth_cookie";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("react", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:49981")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();

                });
            });
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("react");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
