using System;
using System.Diagnostics;
using System.IO;
using BE;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Restaurar_v1_0.Reportes
{
    public static class PdfRendicion
    {
        public static string Generar(BERendicion r, BECaja caja, string carpetaDestino = null)
        {
            if (r == null) throw new ArgumentNullException(nameof(r));
            if (caja == null) throw new ArgumentNullException(nameof(caja));

            string carpeta = carpetaDestino ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Informes");
            Directory.CreateDirectory(carpeta);

            string fileName = $"Rendicion_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string path = Path.Combine(carpeta, fileName);

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var doc = new Document(PageSize.A4, 36, 36, 36, 36))
            {
                PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var fontTitulo = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 14);
                var fontBold = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 10);
                var fontText = FontFactory.GetFont(BaseFont.HELVETICA, 10);
                var fontSmall = FontFactory.GetFont(BaseFont.HELVETICA, 8);

                doc.Add(new Paragraph("Restaurar TF — Rendición de valores", fontTitulo) { SpacingAfter = 8f });

                // Info de cabecera
                var info = new PdfPTable(2) { WidthPercentage = 100 };
                info.SetWidths(new float[] { 28f, 72f });

                void Row(PdfPTable tbl, string k, string v)
                {
                    var kcell = new PdfPCell(new Phrase(k, fontBold)) { Border = Rectangle.NO_BORDER, Padding = 4f, BackgroundColor = new BaseColor(245, 245, 245) };
                    var vcell = new PdfPCell(new Phrase(v ?? "", fontText)) { Border = Rectangle.NO_BORDER, Padding = 4f };
                    tbl.AddCell(kcell); tbl.AddCell(vcell);
                }

                Row(info, "Caja", $"{caja.Nombre}  |  Punto {caja.Punto}  |  Turno {caja.Turno}");
                Row(info, "Apertura", caja.FechaApertura.ToString("dd/MM/yyyy HH:mm"));
                Row(info, "Responsable", caja.Responsable);
                Row(info, "Mozo (usuario)", r.MozoUsuario);
                Row(info, "Fecha rendición", r.Fecha.ToString("dd/MM/yyyy HH:mm"));
                doc.Add(info);

                doc.Add(new Paragraph("\n"));

                // Tabla de importes
                var grid = new PdfPTable(4) { WidthPercentage = 100 };
                grid.SetWidths(new float[] { 30f, 23f, 23f, 24f });

                PdfPCell Head(string txt) => new PdfPCell(new Phrase(txt, fontBold))
                { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5f, BackgroundColor = new BaseColor(230, 230, 230) };

                grid.AddCell(Head("Medio")); grid.AddCell(Head("Esperado")); grid.AddCell(Head("Entregado")); grid.AddCell(Head("Diferencia"));

                void Line(string medio, decimal esp, decimal ent)
                {
                    var dif = ent - esp;
                    grid.AddCell(new PdfPCell(new Phrase(medio, fontText)) { Padding = 4f });
                    grid.AddCell(new PdfPCell(new Phrase(esp.ToString("n2"), fontText)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 4f });
                    grid.AddCell(new PdfPCell(new Phrase(ent.ToString("n2"), fontText)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 4f });
                    grid.AddCell(new PdfPCell(new Phrase(dif.ToString("n2"), fontText)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 4f });
                }

                Line("Efectivo", r.EspEfectivo, r.EntEfectivo);
                Line("Tarjeta", r.EspTarjeta, r.EntTarjeta);
                Line("QR", r.EspQR, r.EntQR);

                var totalLbl = new PdfPCell(new Phrase("Diferencia total", fontBold)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 6f };
                var totalVal = new PdfPCell(new Phrase(r.DiferenciaTotal.ToString("n2"), fontBold)) { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 6f };
                grid.AddCell(totalLbl); grid.AddCell(totalVal);

                doc.Add(grid);
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph($"Estado: {r.Estado}", fontBold));
                if (!string.IsNullOrWhiteSpace(r.Observacion))
                    doc.Add(new Paragraph($"Obs.: {r.Observacion}", fontSmall));

                doc.Close();
            }

            try { Process.Start(path); } catch { }
            return path;
        }
    }
}
