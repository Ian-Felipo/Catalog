using System.Text.Json.Serialization;
using CatalogApi.Data;
using CatalogApi.Extensions;
using CatalogApi.Filters;
using CatalogApi.Interfaces;
using CatalogApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // ********************
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});

builder.Services.AddDbContext<CatalogApiDbContext>(options =>
    {
        string? connectionString = builder.Configuration["ConnectionStrings:MySql"];
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
);

builder.Services.AddScoped<LoggingFilter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();