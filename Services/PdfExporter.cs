using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ProdutividadeApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;

#if WINDOWS
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI.Xaml;
#endif

namespace ProdutividadeApp.Services
{
    public static class PdfExporter
    {
        public static async Task GenerateWeeklyReportAsync(List<DailyChartData> dados)
        {
            // Verificar se temos dados
            if (dados == null || !dados.Any())
            {
                Console.WriteLine("❌ Nenhum dado disponível para exportar.");
                return;
            }

            try
            {
                // Primeiro, garantir que licença está inicializada (se necessário)
                QuestPDF.Settings.License = LicenseType.Community;

                string filename = $"Relatorio_Semanal_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                string tempPath = Path.Combine(Path.GetTempPath(), filename);

                // Sempre usar método direto de geração para arquivo primeiro
                GenerateSimplePdf(dados, tempPath);

                // Verificar se o arquivo foi criado
                if (!File.Exists(tempPath))
                {
                    Console.WriteLine("❌ Falha ao criar arquivo PDF temporário.");
                    return;
                }

                Console.WriteLine($"✅ PDF criado em {tempPath} com tamanho {new FileInfo(tempPath).Length} bytes");

                // Plataforma específica - Windows
                if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                {
#if WINDOWS
                    try
                    {
                        var picker = new FileSavePicker();
                        var hwnd = ((MauiWinUIWindow)App.Current.Windows[0].Handler.PlatformView).WindowHandle;
                        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                        picker.FileTypeChoices.Add("PDF File", new List<string>() { ".pdf" });
                        picker.SuggestedFileName = filename;

                        StorageFile file = await picker.PickSaveFileAsync();

                        if (file != null)
                        {
                            // Copiar o arquivo temporário para o destino escolhido
                            byte[] fileBytes = File.ReadAllBytes(tempPath);
                            using (var stream = await file.OpenStreamForWriteAsync())
                            {
                                await stream.WriteAsync(fileBytes, 0, fileBytes.Length);
                                await stream.FlushAsync();
                            }
                            Console.WriteLine($"✅ PDF salvo em {file.Path}");
                        }
                        else
                        {
                            Console.WriteLine("⚠️ Usuário cancelou o diálogo de salvar.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Erro ao salvar PDF: {ex.Message}");
                    }
#endif
                }
                else
                {
                    // Mobile / outros dispositivos
                    var finalPath = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
                    File.Copy(tempPath, finalPath, true);

                    // Compartilhar o arquivo
                    await Share.RequestAsync(new ShareFileRequest
                    {
                        Title = "Relatório Semanal",
                        File = new ShareFile(finalPath)
                    });
                }

                // Limpar arquivo temporário
                try
                {
                    File.Delete(tempPath);
                }
                catch
                {
                    // Ignora erros de limpeza
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERRO GERAL na exportação do PDF: {ex}");
            }
        }

        private static void GenerateSimplePdf(List<DailyChartData> dados, string filePath)
        {
            Console.WriteLine("⚙️ Gerando PDF simples...");

            try
            {
                // Teste com um documento extremamente simples primeiro
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(50);
                        page.PageColor(QuestPDF.Helpers.Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Text("Relatório Semanal de Produtividade")
                            .SemiBold()
                            .FontSize(20);

                        page.Content()
                            .PaddingVertical(20)
                            .Column(column =>
                            {
                                column.Spacing(20);

                                column.Item()
                                    .Text($"Data de geração: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                    .FontSize(12);

                                column.Item()
                                    .Text($"Total de registros: {dados.Count}")
                                    .FontSize(14);

                                column.Item()
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(100);
                                            columns.ConstantColumn(150);
                                            columns.ConstantColumn(100);
                                            columns.ConstantColumn(100);
                                        });

                                        // Cabeçalho
                                        table.Header(header =>
                                        {
                                            header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten1).Text("Dia");
                                            header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten1).Text("Data");
                                            header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten1).Text("Emoji");
                                            header.Cell().Background(QuestPDF.Helpers.Colors.Grey.Lighten1).Text("Valor");
                                        });

                                        // Dados
                                        foreach (var item in dados)
                                        {
                                            table.Cell().Text(item.Label);
                                            table.Cell().Text(item.Date.ToString("dd/MM/yyyy"));
                                            table.Cell().Text(item.Emoji);
                                            table.Cell().Text(item.Value.ToString());
                                        }
                                    });
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Página ");
                                x.CurrentPageNumber();
                                x.Span(" de ");
                                x.TotalPages();
                            });
                    });
                })
                .GeneratePdf(filePath);

                Console.WriteLine($"✅ PDF gerado: {filePath} ({new FileInfo(filePath).Length} bytes)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERRO ao gerar PDF simples: {ex}");
                throw;
            }
        }
    }
}