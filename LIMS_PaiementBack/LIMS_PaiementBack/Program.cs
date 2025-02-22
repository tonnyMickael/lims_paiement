using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Repositories;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContextEntity> ( options =>
    options.UseMySql (
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

builder.Services.AddScoped<IDemandeRepository, DemandeRepository>();
builder.Services.AddScoped<IDemandeService, DemandeService>();

builder.Services.AddScoped<IDelaiRepository, DelaiRepository>();
builder.Services.AddScoped<IDelaiService, DelaiService>();

builder.Services.AddScoped<IContratRepository, ContratRepository>();
builder.Services.AddScoped<IContratService, ContratService>();

builder.Services.AddScoped<IEspecePaiementRepository, EspecePaiementRepository>();
builder.Services.AddScoped<IEspecePaiementService, EspecePaiementService>();

builder.Services.AddScoped<IMobilePaiementRepository, MobilePaiementRepository>();
builder.Services.AddScoped<IMobilePaiementService, MobilePaiementService>();

builder.Services.AddScoped<IVirementPaiementRepository, VirementPaiementRepository>();
builder.Services.AddScoped<IVirementPaiementService, VirementPaiementService>();

builder.Services.AddScoped<Email>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
