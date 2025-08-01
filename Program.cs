using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using TarifarioBackend.Data;
using TarifarioBackend.Services;
using TarifarioBackend.Helpers;
using TarifarioBackend.Models; // For EmailSettings and PdfSettings
using QuestPDF.Infrastructure; // Import the QuestPDF infrastructure namespace
using Microsoft.Extensions.Options; // Required for IOptions
using Microsoft.AspNetCore.Hosting; // Required for IWebHostEnvironment
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Configure QuestPDF to not show debug data
QuestPDF.Infrastructure.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database contexts
builder.Services.AddDbContext<GestionTarifasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestionTarifasDb")));

builder.Services.AddDbContext<SaadisNuevoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SaadisNuevoDb")));

// Configure EmailSettings and PdfSettings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<PdfSettings>(builder.Configuration.GetSection("PdfSettings"));

// Register services for dependency injection
builder.Services.AddScoped<TarifarioService>();
builder.Services.AddScoped<MaestrosService>();
builder.Services.AddScoped<ActualizacionService>();
builder.Services.AddScoped<ReportesService>();
builder.Services.AddScoped<NovedadesService>();
builder.Services.AddScoped<EmailService>();

// Add helpers (if they are not stateless singletons or need specific configuration)
// For SqlHelper, it needs a connection string, so it's scoped.
builder.Services.AddScoped(provider => new SqlHelper(builder.Configuration.GetConnectionString("GestionTarifasDb")!));
// PdfGenerator is now a scoped service as it depends on IOptions<PdfSettings> and IWebHostEnvironment
builder.Services.AddScoped<PdfGenerator>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Use the CORS policy

// Enable static files to serve PDFs and images
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "pdfs")),
    RequestPath = "/pdfs"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "images")),
    RequestPath = "/images"
});

app.UseAuthorization();

app.MapControllers();

app.Run();
