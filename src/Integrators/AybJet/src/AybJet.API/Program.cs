using AybJet.Application;
using AybJet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// API Servisleri
builder.Services.AddControllers();

// Application Katmanı Servisleri
builder.Services.AddApplicationServices();

// Infrastructure Katmanı Servisleri
builder.Services.AddInfrastructureServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();