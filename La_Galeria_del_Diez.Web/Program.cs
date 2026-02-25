using La_Galeria_del_Diez.Application.Profiles;
using La_Galeria_del_Diez.Application.Services.Implementatios;
using La_Galeria_del_Diez.Application.Services.Interfaces;
using La_Galeria_del_Diez.Infraestructure.Data;
using La_Galeria_del_Diez.Infraestructure.Repository.Implementations;
using La_Galeria_del_Diez.Infraestructure.Repository.Interfaces;
using Libreria.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;

//***********
// =======================
// Configurar Serilog
// =======================
// Crear carpeta Logs automáticamente (evita errores si no existe)
Directory.CreateDirectory("Logs");

// Configuración Serilog
var logger = new LoggerConfiguration()
    // Nivel mínimo global (recomendado: Information)
    .MinimumLevel.Information()

    // Reducir ruido de logs internos de Microsoft
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    //Mostrar SQL ejecutado por EF Core
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)

    // Enriquecer logs con contexto (RequestId, etc.)
    .Enrich.FromLogContext()

    // Consola: útil para depurar en Visual Studio
    .WriteTo.Console()

    //Archivos separados por nivel (rolling diario)
    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File(@"Logs\Info-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
        .WriteTo.File(@"Logs\Warning-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File(@"Logs\Error-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
        .WriteTo.File(@"Logs\Fatal-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .CreateLogger();

// Paso obligatorio ANTES de crear builder
Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddControllersWithViews();

//*********** 
// ======================= 
// Configurar Dependency Injection 
// ======================= 
//*** Repositories 
builder.Services.AddTransient<IRepositoryUser, RepositoryUser>();

//*** Services 
builder.Services.AddTransient<IServiceUser, ServiceUser>();

// ======================= 
// Configurar AutoMapper 
// ======================= 
builder.Services.AddAutoMapper(config =>
{
    //*** Profiles 
    config.AddProfile<UserProfile>();
    config.AddProfile<RolProfile>();
});

// ======================= 
// Configurar SQL Server DbContext 
// ======================= 
var connectionString = builder.Configuration.GetConnectionString("SqlServerDataBase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión 'SqlServerDataBase' en appsettings.json / appsettings.Development.json." ); 
}

builder.Services.AddDbContext<La_Galeria_del_Diez_Context>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Reintentos ante fallos transitorios (recomendado) 
        sqlOptions.EnableRetryOnFailure();
    });

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

//*********** 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Middleware personalizado
    app.UseMiddleware<ErrorHandlingMiddleware>();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
