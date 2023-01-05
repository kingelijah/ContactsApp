using ContactsApp.ViewModel;
using DataAccess.EFCore;
using DataAccess.EFCore.Repositories;
using Domain;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ContactsAppContext") ?? throw new InvalidOperationException("Connection string 'ContactsAppContext' not found.")));

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    builder.Services.AddTransient<IContactRepository, ContactRepository>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IValidator<ContactViewModel>, ContactValidator>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddSwaggerGen();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    app.UseCors("corsapp");

    app.Run();
}

catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}