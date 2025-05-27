using System.Text.Json.Serialization;
using CatalogApi.AutoMappers;
using CatalogApi.Data;
using CatalogApi.Extensions;
using CatalogApi.Filters;
using CatalogApi.Interfaces;
using CatalogApi.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); // ********************
builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(CategoryMapperProfile), typeof(ProductMapperProfile));
builder.Services.AddAuthentication("Bearer").AddBearerToken();
builder.Services.AddAuthorization();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ExceptionFilter));
    }
).AddNewtonsoftJson();

builder.Services.AddDbContext<CatalogApiDbContext>(options =>
    {
        string? connectionString = builder.Configuration["ConnectionStrings:MySql"];
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CatalogApiDbContext>().AddDefaultTokenProviders();

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