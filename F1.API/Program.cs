using F1.API.Data;
using F1.Lib.Interfaces.Genericas;
using Microsoft.EntityFrameworkCore;
using F1.API.Data.Repositories.Genericos;
using F1.API.Services.Relatorio.Services;
using F1.API.Services.GpServices.Services;
using F1.Lib.Interfaces.Especificas.Query;
using F1.API.Services.Relatorio.Interfaces;
using F1.API.Services.GpServices.Interfaces;
using F1.API.Services.EquipeServices.Services;
using F1.API.Services.PilotoServices.Services;
using F1.API.Services.EquipeServices.Interfaces;
using F1.API.Services.PilotoServices.Interfaces;
using F1.API.Data.Repositories.Especificos.Query;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("F1Context");

builder.Services.AddDbContext<F1Context>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICreateEquipeService, CreateEquipeService>();
builder.Services.AddScoped<IReadEquipeService, ReadEquipeService>();
builder.Services.AddScoped<IUpdateEquipeService, UpdateEquipeService>();
builder.Services.AddScoped<IDeleteEquipeService, DeleteEquipeService>();

builder.Services.AddScoped<ICreateGrandePremioService, CreateGrandePremioService>();
builder.Services.AddScoped<IReadGrandePremioService, ReadGrandePremioService>();
builder.Services.AddScoped<IUpdateGrandePremioService, UpdateGrandePremioService>();
builder.Services.AddScoped<IDeleteGrandePremioService, DeleteGrandePremioService>();

builder.Services.AddScoped<ICreatePilotoService, CreatePilotoService>();
builder.Services.AddScoped<IReadPilotoService, ReadPilotoService>();
builder.Services.AddScoped<IUpdatePilotoService, UpdatePilotoService>();
builder.Services.AddScoped<IDeletePilotoService, DeletePilotoService>();

builder.Services.AddScoped<IReadRelatorioService, ReadRelatorioService>();

builder.Services.AddScoped(typeof(IQueryBase<>), typeof(QueryBase<>));
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

builder.Services.AddScoped<IEquipeQuery, EquipeQuery>();
builder.Services.AddScoped<IGrandePremioQuery, GrandePremioQuery>();
builder.Services.AddScoped<IPilotoQuery, PilotoQuery>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
