using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TarifarioBackend.Models; // Asegúrate de que Novedad esté aquí

namespace TarifarioBackend.Helpers
{
    public class PdfGenerator
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<PdfGenerator> _logger;

        public PdfGenerator(IWebHostEnvironment env, ILogger<PdfGenerator> logger)
        {
            _env = env;
            _logger = logger;
            // Registrar la licencia de QuestPDF si tienes una
            // QuestPDF.Settings.License = LicenseType.Community; // O LicenseType.Professional, etc.
        }

        public async Task<string> GenerateNovedadPdf(Novedad novedad)
        {
            try
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "pdfs");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"Novedad_{novedad.IdNovedad}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string filePath = Path.Combine(uploadsFolder, fileName);

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(50);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Column(column =>
                            {
                                column.Item().Row(row =>
                                {
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().Text("Reporte de Novedad").FontSize(24).Bold();
                                        col.Item().Text($"Fecha de Reporte: {DateTime.Now:dd/MM/yyyy}");
                                    });

                                    row.ConstantItem(100).Image(Path.Combine(_env.WebRootPath, "images", "logo_esa.jpg"));
                                });

                                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2).MarginVertical(10);
                            });

                        page.Content()
                            .PaddingVertical(10)
                            .Column(column =>
                            {
                                column.Spacing(10);

                                column.Item().Text(text =>
                                {
                                    text.Span("ID Novedad: ").Bold();
                                    text.Span(novedad.IdNovedad.ToString());
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Tipo de Novedad: ").Bold();
                                    text.Span(novedad.TipoNovedad?.Nombre ?? "N/A");
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Fecha de la Novedad: ").Bold();
                                    text.Span(novedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm"));
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Descripción: ").Bold();
                                    text.Span(novedad.Descripcion);
                                });

                                column.Item().Text(text =>
                                {
                                    text.Span("Estado: ").Bold();
                                    text.Span(novedad.Ignorada ? "Ignorada" : "Activa");
                                });

                                if (novedad.Ignorada)
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Ignorada por: ").Bold();
                                        text.Span(novedad.UsuarioIgnora ?? "N/A");
                                    });
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Fecha de Ignorado: ").Bold();
                                        text.Span(novedad.FechaIgnorada?.ToString("dd/MM/yyyy HH:mm") ?? "N/A");
                                    });
                                }

                                column.Item().Text(text =>
                                {
                                    text.Span("Email Enviado: ").Bold();
                                    text.Span(novedad.EnviadoEmail ? "Sí" : "No");
                                });

                                if (novedad.EnviadoEmail)
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Email Destino: ").Bold();
                                        text.Span(novedad.EmailDestino ?? "N/A");
                                    });
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Fecha de Envío: ").Bold();
                                        text.Span(novedad.FechaEnvioEmail?.ToString("dd/MM/yyyy HH:mm") ?? "N/A");
                                    });
                                }

                                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2).MarginVertical(10);
                                column.Item().Text("Información Adicional:").FontSize(14).Bold();
                                column.Item().Text("Este documento es un reporte automático del sistema de gestión de novedades.");
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Página ").FontSize(10);
                                x.CurrentPageNumber().FontSize(10);
                                x.Span(" de ").FontSize(10);
                                x.TotalPages().FontSize(10);
                            });
                    });
                }).GeneratePdf(filePath);

                _logger.LogInformation($"PDF generado en: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar PDF para novedad ID {novedad.Id}: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
