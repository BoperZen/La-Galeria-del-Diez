using La_Galeria_del_Diez.Application.Profiles;
using La_Galeria_del_Diez.Application.Profiles.Api;
using La_Galeria_del_Diez.Application.Services.Implementatios;
using La_Galeria_del_Diez.Application.Services.Implementatios.Api;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Application.Services.Interfaces.Api;
using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Repository.Implementations;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IServiceAuction, ServiceAuction>();
builder.Services.AddScoped<IAuctionApiService, AuctionApiService>();
builder.Services.AddScoped<IRepositoryAuction, RepositoryAuction>();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<AuctionProfile>();
    config.AddProfile<BiddingProfile>();
    config.AddProfile<ImageProfile>();
    config.AddProfile<RolProfile>();
    config.AddProfile<StateProfile>();
    config.AddProfile<UserProfile>();
    config.AddProfile<AuctionableObjectProfile>();
    config.AddProfile<AuctionApiProfile>();
});

var connectionString = builder.Configuration.GetConnectionString("SqlServerDataBase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión 'SqlServerDataBase' en appsettings.json / appsettings.Development.json.");
}

builder.Services.AddDbContext<La_Galeria_del_Diez_Context>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.Run();