using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using PixelV0.Api.Middlewares;
using PixelV0.Infrastructure.Authentication;
using PixelV0.Infrastructure.Data;
using PixelV0.Infrastructure.Repositories;
using PixelV0.Modules.Identity.Application.Interfaces;
using PixelV0.Modules.Identity.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Exception Handler and ProblemDetails
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 2. Add JWT Authentication
var jwt = builder.Configuration["JwtSettings:Secret"];
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwt!)) // ! eklendi
    };
});

// 3. Swagger configuration to include JWT token input
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v0", new OpenApiInfo
    {
        Version = "v0",
        Title = "Pixel API",
        Description = "Pixel Social Hub API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header, // Bu eksikti, eklendi
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
}); // EKSÝKTÝ: AddSwaggerGen bloðunu kapatan parantez ve noktalý virgül eklendi


// 4. Add services to the container.
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("PixelV0.Infrastructure")));

// 5. DI Registrations
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJWTProvider, JWTProvider>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// 6. Otomatik Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// 7. Middleware Pipeline (Sýralama Çok Önemli!)
app.UseExceptionHandler(); // EKSÝKTÝ: Global hata yönetimi aktif edildi

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v0/swagger.json", "Pixel API v0");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication(); // EKSÝKTÝ: Token'ýn okunmasý için UseAuthorization'dan ÖNCE gelmeli
app.UseAuthorization();

app.MapControllers();

app.Run();