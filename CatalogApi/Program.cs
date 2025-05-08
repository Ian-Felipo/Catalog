using CatalogApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CatalogApiDbContext>(options =>
    {
        string? connectionString = builder.Configuration["ConnectionStrings:MySql"];
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();