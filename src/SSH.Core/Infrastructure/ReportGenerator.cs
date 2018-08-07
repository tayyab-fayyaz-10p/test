using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace SSH.Core.Infrastructure
{
    public static class ReportGenerator
    {
        public static bool CreateCSV<T>(List<T> list, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    CreateHeader(list, sw);
                    CreateRows(list, sw);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreatePDF<T>(List<T> list, string folderPath, string filename)
        {
            try
            {
                Document document = CreateDocument(list);
                //MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
                pdfRenderer.Document = document;
                pdfRenderer.RenderDocument();               
                pdfRenderer.PdfDocument.Save(filename);
                Process.Start(filename);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static Document CreateDocument<T>(List<T> list)
        {
            Document document = new Document();
            DefineHeader(document);
            DefineTables(document, list);
            return document;
        }

        private static void DefineHeader(Document document)
        {
            Section section = document.AddSection();
            //section.PageSetup.Orientation = Orientation.Landscape;
            Paragraph paragraph = section.AddParagraph("Finance House Reports");
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.Font.Size = 16;
            paragraph.Format.Font.Color = Colors.DarkRed;
            //Image image = section.AddImage("DefaultProfileImage.png");
            //Paragraph imageParagraph = section.AddParagraph();
            //Image image = imageParagraph.AddImage(System.Web.HttpContext.Current.Server.MapPath("~/images/DefaultProfileImage.png"));
            ////Image image = section.AddImage("../../SSH.API/images/DefaultProfileImage.png");
            //image.Width = "3cm";
            //image.Height = "3cm";
            //imageParagraph.Format.Alignment = ParagraphAlignment.Center;
        }

        private static void DefineTables<T>(Document document, List<T> list)
        {
            //Section section = document.AddSection();
            Section section = document.LastSection;
            #region page setup
            //section.PageSetup.PageFormat = PageFormat.Ledger;
            //section.PageSetup.Orientation = Orientation.Landscape;            
            //section.PageSetup.TopMargin = 0.25;
            //section.PageSetup.BottomMargin = 0.25;
            //section.PageSetup.LeftMargin = 0.25;
            //section.PageSetup.RightMargin = 0.25;
            //section.PageSetup.HeaderDistance = 1.0;
            //section.PageSetup.FooterDistance = 5.0;
            //section.PageSetup.PageHeight = Unit.FromInch(8.5);
            //section.PageSetup.PageWidth = Unit.FromInch(14.0); 
            #endregion

            //section.PageSetup.Orientation = Orientation.Landscape;

            //section.PageSetup.PageFormat = PageFormat.Ledger;
            //section.PageSetup.Orientation = Orientation.Landscape;            
            //section.PageSetup.TopMargin = Unit.FromCentimeter(2);
            //section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            //section.PageSetup.HeaderDistance = 1.0;
            //section.PageSetup.FooterDistance = 5.0;
            //section.PageSetup.PageHeight = Unit.FromInch(8.5);
            //section.PageSetup.PageWidth = Unit.FromInch(18.0);

            Table table = new Table();
            table.Borders.Width = 0.75;
            //table.Format.KeepTogether = true;

            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 0; i <= properties.Length - 1; i++)
            {
                Column column = table.AddColumn(Unit.FromCentimeter(4));
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;

            for (int i = 0; i <= properties.Length - 1; i++)
            {
                Cell cell = row.Cells[i];
                cell.AddParagraph(properties[i].Name);
                //cell.Format.KeepTogether = true;
            }

            foreach (var item in list)
            {
                Row dataRow = table.AddRow();
                PropertyInfo[] dataProperties = typeof(T).GetProperties();

                for (int i = 0; i <= dataProperties.Length - 1; i++)
                {
                    var prop = dataProperties[i];
                    Cell cell = dataRow.Cells[i];
                    if (prop.GetValue(item) != null)
                    {
                        cell.AddParagraph(prop.GetValue(item).ToString());
                    }
                }
            }

            //table.SetEdge(0, 0, properties.Length, list.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
            document.LastSection.Add(table);
        }

        private static void CreateHeader<T>(List<T> list, StreamWriter sw)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int i = 0; i < properties.Length - 1; i++)
            {
                sw.Write(properties[i].Name + ",");
            }

            var lastProp = properties[properties.Length - 1].Name;
            sw.Write(lastProp + sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();

                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    sw.Write(prop.GetValue(item) + ",");
                }

                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
        }
    }
}
