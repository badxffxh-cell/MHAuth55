using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MHAuth55;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do CORS (Permite que qualquer site acesse a API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 2. Configuração do JWT Secret Key
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ChaveSecretaSuperSeguraParaAuthApi123*!";
var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. Ativa o CORS (Deve vir antes do Authentication e dos Endpoints)
app.UseCors("AllowAll");

// Ativa o Swagger em produção no Railway
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// 4. Mapeia os endpoints que estão no arquivo AuthApi.cs
app.MapAuthEndpoints(jwtKey);

app.Run();
