using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QTBWebBackend.Authorization;
using QTBWebBackend.Interfaces;
using QTBWebBackend.Models;
using QTBWebBackend.Repositories;
using QTBWebBackend.Services;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CORS
builder.Services.AddCors();

builder.Services.AddControllers();

// AppSettings configuration
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Registrazione Database Context con SQL Server
builder.Services.AddDbContext<QTBWebDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnessioneDefault")));

// Registrazione Repository
builder.Services.AddScoped<IAeroportiRepository, AeroportiRepository>();

//Registrazione servizi
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAeroportiService, AeroportiService>();

// ⭐ AUTENTICAZIONE JWT - SOLUZIONE NATIVA
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Secret"] ?? 
            throw new InvalidOperationException("La chiave segreta JWT (AppSettings:Secret) non è configurata."))),
            ValidateIssuer = false,  // Imposta a true se hai un Issuer specifico
            ValidateAudience = false, // Imposta a true se hai un Audience specifico
            ValidateLifetime = true,  // Verifica scadenza token
            ClockSkew = TimeSpan.Zero // Rimuove il grace period di default (5 minuti)
        };

        // Eventi opzionali per diagnostica
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Autenticazione JWT Fallita: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("JWT Token validato con successo");
                return Task.CompletedTask;
            }
        };
    });

// Autorizzazione (opzionale ma consigliata)
builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // UI moderna e interattiva
    app.MapControllers().AllowAnonymous();
}
else
{
    app.MapControllers();
}

    // CORS
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseHttpsRedirection();

// ⭐ AUTENTICAZIONE E AUTORIZZAZIONE - ORDINE CRITICO!
app.UseAuthentication(); // PRIMA Authentication
app.UseAuthorization();  // POI Authorization

app.MapControllers();

app.Run();
