using Domain.Seedwork;
using Microsoft.EntityFrameworkCore;
using PensamientoAlternativo.Application.Commands.FormCommand;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Infrastructure;
using PensamientoAlternativo.Infrastructure.Repositories;
using PensamientoAlternativo.Infrastructure.Services;
using PensamientoAlternativo.Persistance;
using PensamientoAlternativo.Persistance.Repositories;

namespace API_PensamientoAlternativo
{
    public static class Program 
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(SubmitContactFormCommand).Assembly));

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));


            builder.Services.AddScoped<IContactFormRepository, ContactFormRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IClientSettingsRepository, ClientSettingsRepository>();
            builder.Services.AddScoped<IHomeContentRepository, HomeContentRepository>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
