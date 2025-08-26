using Domain.Seedwork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PensamientoAlternativo.Application.Commands.FormCommand;
using PensamientoAlternativo.Application.DTOs.CategoriesDTOs;
using PensamientoAlternativo.Application.Handlers.CategoriesHandler;
using PensamientoAlternativo.Application.Handlers.FaqHandlers;
using PensamientoAlternativo.Application.Handlers.ImageHandlers;
using PensamientoAlternativo.Application.Handlers.OpinionsHandlers;
using PensamientoAlternativo.Application.Handlers.VideoHandlers;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Domain.SeedWork;
using PensamientoAlternativo.Infrastructure;
using PensamientoAlternativo.Infrastructure.Services;
using PensamientoAlternativo.Infrastructure.Storage;
using PensamientoAlternativo.Persistance;
using PensamientoAlternativo.Persistance.Repositories;
using System.Security.Claims;
using System.Text;

namespace API_PensamientoAlternativo
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //builder.Services
            //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    var cfg = builder.Configuration;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = cfg["Jwt:Issuer"],
            //        ValidAudience = cfg["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!)),
            //        ClockSkew = TimeSpan.Zero
            //    };
            //    options.RequireHttpsMetadata = false; // útil en dev si pegas por http
            //    options.SaveToken = true;

            //    // LOG para diagnóstico de 401
            //    options.Events = new JwtBearerEvents
            //    {
            //        OnAuthenticationFailed = ctx =>
            //        {
            //            Console.WriteLine("JWT FAILED: " + ctx.Exception.Message);
            //            return Task.CompletedTask;
            //        },
            //        OnChallenge = ctx =>
            //        {
            //            Console.WriteLine($"JWT CHALLENGE: err={ctx.Error} desc={ctx.ErrorDescription}");
            //            return Task.CompletedTask;
            //        },
            //        OnTokenValidated = ctx =>
            //        {
            //            Console.WriteLine("JWT OK for: " + ctx.Principal?.FindFirst(ClaimTypes.Email)?.Value);
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            //builder.Services.AddAuthorization();

            builder.Services.AddCors(o => o.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            builder.Services.Configure<FormOptions>(o =>
            {
                o.MultipartBodyLengthLimit = 600L * 1024 * 1024; // 600 MB
                                                                 // opcionalmente:
                                                                 // o.MultipartHeadersLengthLimit = 64 * 1024;
                                                                 // o.ValueLengthLimit = int.MaxValue;
            });

            builder.WebHost.ConfigureKestrel(o =>
            {
                o.Limits.MaxRequestBodySize = 600L * 1024 * 1024; // 600 MB
            });

            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new() { Title = "PA API", Version = "v1" });

            //    var securityScheme = new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Description = "JWT en el header. Ej: Bearer {token}",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "bearer", 
            //        BearerFormat = "JWT",
            //        Reference = new OpenApiReference 
            //        {
            //            Type = ReferenceType.SecurityScheme,
            //            Id = "Bearer"
            //        }
            //    };

            //    c.AddSecurityDefinition("Bearer", securityScheme);

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        { securityScheme, Array.Empty<string>() }
            //    });
            //});
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
            builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
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
