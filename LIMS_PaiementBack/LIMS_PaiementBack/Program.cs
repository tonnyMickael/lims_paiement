using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Services;
using Microsoft.EntityFrameworkCore;
using LIMS_PaiementBack.Repositories;
using System.Text.Json.Serialization;
using LIMS_PaiementBack.Services.Depart;
using LIMS_PaiementBack.Repositories.Depart;
using LIMS_PaiementBack.Services.EtatJournalier;
using LIMS_PaiementBack.Services.EtatHebdomadaire;
using LIMS_PaiementBack.Repositories.EtatJournalier;
using LIMS_PaiementBack.Repositories.EtatHebdomadaire;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: "Authorized",
//                       policy =>
//                       {
//                           //policy.WithOrigins("http://localhost:5204")
//                           policy.AllowAnyOrigin()
//                           .AllowAnyMethod()
//                           //   Just for this time but we need it after The token thing and Auth
//                           .AllowAnyHeader();
//                       });
// });

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

// builder.Services.AddScoped<IContratRepository, ContratRepository>();
// builder.Services.AddScoped<IContratService, ContratService>();

builder.Services.AddScoped<IEspecePaiementRepository, EspecePaiementRepository>();
builder.Services.AddScoped<IEspecePaiementService, EspecePaiementService>();

builder.Services.AddScoped<IMobilePaiementRepository, MobilePaiementRepository>();
builder.Services.AddScoped<IMobilePaiementService, MobilePaiementService>();

builder.Services.AddScoped<IVirementPaiementRepository, VirementPaiementRepository>();
builder.Services.AddScoped<IVirementPaiementService, VirementPaiementService>();

builder.Services.AddScoped<IReceptionEspecePaiementRepository, ReceptionEspecePaiementRepository>();
builder.Services.AddScoped<IReceptionEspecePaiementService, ReceptionEspecePaiementService>();

builder.Services.AddScoped<IReceptionMobilePaiementRepository, ReceptionMobilePaiementRepository>();
builder.Services.AddScoped<IReceptionMobilePaiementService, ReceptionMobilePaiementService>();

builder.Services.AddScoped<IReceptionVirementPaiementRepository, ReceptionVirementPaiementRepository>();
builder.Services.AddScoped<IReceptionVirementPaiementService, ReceptionVirementPaiementService>();

builder.Services.AddScoped<ISousContratRepository, SousContratRepository>();
builder.Services.AddScoped<ISousContratService, SousContratService>();

builder.Services.AddScoped<IDestinataireRepository, DestinataireRepository>();
builder.Services.AddScoped<IDestinataireService, DestinataireService>();

builder.Services.AddScoped<IEtatJournalierRepository, EtatJournalierRepository>();
builder.Services.AddScoped<IEtatJournalierService, EtatJournalierService>();

builder.Services.AddScoped<IEtatHebdomadaireRepository, EtatHebdomadaireRepository>();
builder.Services.AddScoped<IEtatHebdomadaireService, EtatHebdomadaireService>();

builder.Services.AddScoped<IDepartRepository, DepartRepository>();
builder.Services.AddScoped<IDepartService, DepartService>();

builder.Services.AddScoped<IBanqueRepository, BanqueRepository>();
builder.Services.AddScoped<IBanqueService, BanqueService>();

// builder.Services.AddScoped<Email>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
