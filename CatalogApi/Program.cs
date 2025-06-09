using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Asp.Versioning;
using CatalogApi.AutoMappers;
using CatalogApi.Data;
using CatalogApi.Interfaces;
using CatalogApi.Models;
using CatalogApi.Repositories;
using CatalogApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
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

builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://apirequest.io").WithMethods("GET").AllowAnyHeader();
            }

        );
        options.AddPolicy("SourcesWithAllowedAccess", policy =>
            {
                policy.WithOrigins("https://apirequest.io", "http://localhost:5250").WithMethods("GET", "POST", "PUT", "DELETE").AllowAnyHeader();
            }
        );
    }
);

builder.Services.AddRateLimiter(options =>
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext,string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(
                httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 5,
                    QueueLimit = 0,
                    Window = TimeSpan.FromSeconds(5)
                }    
            )
        );
        options.AddFixedWindowLimiter("FixedWindowLimiter", policy =>
            {
                policy.PermitLimit = 1;
                policy.QueueLimit = 0;
                policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                policy.Window = TimeSpan.FromSeconds(5);
            }
        );
        options.AddSlidingWindowLimiter("slidingWindowLimiter", policy =>
            {
                policy.PermitLimit = 8;
                policy.QueueLimit = 2;
                policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                policy.SegmentsPerWindow = 5;
                policy.Window = TimeSpan.FromSeconds(10);
            }
        );
        options.AddConcurrencyLimiter("concurrencyLimiter", policy =>
            {

            }
        );
        options.AddTokenBucketLimiter("tokenBucketLimiter", policy =>
            {

            }
        );
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    }
);

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0, "Ok");
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("version"), new UrlSegmentApiVersionReader());
        options.UnsupportedApiVersionStatusCode = StatusCodes.Status403Forbidden;
    }
).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();
app.Run();