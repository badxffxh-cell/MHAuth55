var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// Porta dinâmica para o Railway ou localhost
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://*:");
