using Microsoft.EntityFrameworkCore;
using OnlineKitapMagazasi.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnlineKitapMagazasi.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// JWT konfig�rasyonu
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {    // buradaki ayarlar ile gelen token ge�erli mi kontrol ediliyor
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,  // ileride belki eklerim
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// veritaban� ba�lant�s�n� buradan yap�yorum
builder.Services.AddDbContext<KitapDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KitapDb")));

// kendi yazd���m token servisi
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddAuthorization();
builder.Services.AddControllers();

// swagger ile api dok�mantasyonu (geli�tirme i�in)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<KitapDbContext>(); 
builder.Services.AddScoped<TokenService>(); 


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
