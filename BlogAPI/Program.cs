
using BlogAPI.Data;
using BlogAPI.Interfaces;
using BlogAPI.Models;
using BlogAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IRepository<Blog>, SqlRepository<Blog>>();
            builder.Services.AddScoped<IRepository<Category>, SqlRepository<Category>>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("BlogPost", policyBuilder =>
                {
                    policyBuilder.WithOrigins("http://localhost:4200") 
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("BlogPost");

            app.MapControllers();

            app.Run();
        }
    }
}
