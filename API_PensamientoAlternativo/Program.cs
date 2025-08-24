using Domain.Seedwork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PensamientoAlternativo.Application.Commands.FormCommand;
using PensamientoAlternativo.Application.DTOs.CategoriesDTOs;
using PensamientoAlternativo.Application.Handlers.CategoriesHandler;
using PensamientoAlternativo.Application.Handlers.FaqHandlers;
using PensamientoAlternativo.Application.Handlers.ImageHandlers;
using PensamientoAlternativo.Application.Handlers.OpinionsHandlers;
using PensamientoAlternativo.Application.Handlers.VideoHandlers;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Infrastructure;
using PensamientoAlternativo.Infrastructure.Services;
using PensamientoAlternativo.Infrastructure.Storage;
using PensamientoAlternativo.Persistance;
using PensamientoAlternativo.Persistance.Repositories;
using System.Text;

namespace API_PensamientoAlternativo
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var config = builder.Configuration;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });
            builder.Services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(SubmitContactFormCommand).Assembly));
            builder.Services.AddMediatR(cfg =>
                 cfg.RegisterServicesFromAssemblyContaining<CreateImageHandler>());
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateFaqHandler>());
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateOpinionHandler>());
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateBlogCategoryHandler>());
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblyContaining<CreateVideoHandler>());
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

            builder.Services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.AddScoped<IContactFormRepository, ContactFormRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IClientSettingsRepository, ClientSettingsRepository>();
            builder.Services.AddScoped<IHomeContentRepository, HomeContentRepository>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IArticleWriteRepository, ArticleWriteRepository>();
            builder.Services.AddScoped<IImageWriteRepository, ImageWriteRepository>();
            builder.Services.AddScoped<IFaqWriteRepository, FaqWriteRepository>();
            builder.Services.AddScoped<IOpinionWriteRepository, OpinionWriteRepository>();
            builder.Services.AddScoped<IBlogCategoryWriteRepository, BlogCategoryWriteRepository>();
            builder.Services.AddScoped<IVideoWriteRepository, VideoWriteRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IImageStorage, FirebaseImageStorage>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            var app = builder.Build();

            app.UseRouting();
            app.UseCors("AllowAllOrigins");

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
