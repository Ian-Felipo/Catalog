using System.Text;
using System.Text.Json.Serialization;
using CatalogApi.AutoMappers;
using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using CatalogApi.Repositories;
using CatalogApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(CategoryMapperProfile), typeof(ProductMapperProfile)); 

builder.Services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<CatalogApiDbContext>(options =>
    {
        string? connectionString = builder.Configuration["ConnectionStrings:MySql"];
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
);

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<CatalogApiDbContext>().AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid secret key!!");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    }
);

builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Bearer JWT ",
            }
        );

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            }
        );
    }
);

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
        options.AddPolicy("ExclusiveOnly", policy => policy.RequireAssertion(context => context.User.HasClaim(claim => claim.Type == "id" && claim.Value == "Naruto")));
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();