using Microsoft.EntityFrameworkCore;
using MiniTicaret.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MiniTicaret.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using FluentValidation.AspNetCore;
using MiniTicaret.Application.Validations.AuthenticationValidations;
using MiniTicaret.Persistence.Services;
using MiniTicaret.Application.Abstracts.Services;
using MiniTicaret.Persistence.Services;
using MiniTicaret.Application.Mapping;
using MiniTicaret.Application.Abstracts.Repositories;
using MiniTicaret.Persistence.Repositories;
using MiniTicaret.Application.Shared.Permissions;
using MiniTicaret.WebApi.Middlewares;
using MiniTicaret.Application.Shared.Settings;


var builder = WebApplication.CreateBuilder(args);

// 1. Configuration: appsettings.json-dan JwtSettings bölməsini oxu və DI-ya əlavə et
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));

// 2. Identity xidmətləri əlavə et
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<MiniTicaretDbContext>()
.AddDefaultTokenProviders();

// 3. JWT Settings obyektini əldə et (burada `IOptions` istifadə etməyəcəyik, proqram başladıqda alırıq)
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTSettings>();

if (jwtSettings == null)
    throw new Exception("JwtSettings section is missing from appsettings.json");

if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
    throw new Exception("JWT SecretKey is missing in configuration.");

// 4. Authentication konfiqurasiyası: JWT Bearer token ilə
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine($"❌ Token xətası: {ctx.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = ctx =>
        {
            Console.WriteLine($"✅ Token təsdiq olundu: {ctx.Principal.Identity?.Name}");
            return Task.CompletedTask;
        }
    };

    options.RequireHttpsMetadata = false; // Development üçün false, Production-da true olmalıdır
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        ClockSkew = TimeSpan.Zero // Token vaxtının dəqiq yoxlanması üçün
    };
});

// 5. Swagger konfiqurasiyası: JWT Authorization əlavə olunur
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MiniTicaret API", Version = "v1" });

    var jwtSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        Description = "JWT Token yaz: Bearer {token}",

        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// 6. Authorization siyasəti əlavə et
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in PermissionHelper.GetAllPermissionList())
    {
        options.AddPolicy(permission, policy =>
            policy.RequireClaim("Permission", permission));
    }
});

// 7. DI: Xidmətləri əlavə et
builder.Services.AddScoped<MiniTicaret.Application.Abstracts.Services.IAuthenticationService, MiniTicaret.Persistence.Services.AuthenticationService>();
builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddHttpContextAccessor();

// 8. Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(AuthenticationRegisterDtoValidator).Assembly);

// 9. MVC & API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 10. AutoMapper profillər
builder.Services.AddAutoMapper(typeof(ProductProfile), typeof(UserProfile), typeof(CategoryProfile));

// 11. DB konfiqurasiyası
builder.Services.AddDbContext<MiniTicaretDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.SeedPermissionsAsync(roleManager);
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniTicaret API V1");
});

app.MapControllers();

app.Run();