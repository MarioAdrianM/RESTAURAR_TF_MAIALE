using System;
using System.Diagnostics;
using System.IO;
using BE;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Restaurar_v1_0.Reportes
{
    public static class PdfFactura
    {
        public static string Generar(BEFactura fac, string carpetaDestino = null)
        {
            if (fac == null) throw new ArgumentNullException(nameof(fac));
            string carpeta = carpetaDestino ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Informes");
            Directory.CreateDirectory(carpeta);

            string fileName = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
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

                doc.Add(new Paragraph("Restaurar TF — Maiale", fontTitulo) { SpacingAfter = 6f });

                var cbte = $"{fac.TipoCbte}-{fac.PuntoVenta:0000}-{fac.Numero:00000000}";
                doc.Add(new Paragraph($"Comprobante: {cbte}", fontBold) { SpacingAfter = 12f });

                var tbl = new PdfPTable(2) { WidthPercentage = 100 };
                tbl.SetWidths(new float[] { 28f, 72f });

                void Row(string k, string v)
                {
                    var c1 = new PdfPCell(new Phrase(k, fontBold)) { Border = Rectangle.NO_BORDER, Padding = 4f, BackgroundColor = new BaseColor(245, 245, 245) };
                    var c2 = new PdfPCell(new Phrase(v ?? "", fontText)) { Border = Rectangle.NO_BORDER, Padding = 4f };
                    tbl.AddCell(c1); tbl.AddCell(c2);
                }

                Row("Fecha", fac.Fecha.ToString("dd/MM/yyyy HH:mm"));
                Row("Mesa", fac.MesaId.ToString());
                Row("Estado", fac.Estado);
                Row("Total", fac.ImporteTotal.ToString("n2"));

                doc.Add(tbl);
                doc.Add(new Paragraph("\n"));

                string leyenda = fac.TipoCbte?.ToUpperInvariant() == "X"
                    ? "Documento NO fiscal"
                    : "Gracias por su visita";
                doc.Add(new Paragraph(leyenda, fontSmall) { Alignment = Element.ALIGN_RIGHT });

                doc.Close();
            }

            try { Process.Start(path); } catch { /* si no hay visor, solo guarda */ }
            return path;
        }
    }
}
