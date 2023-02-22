using System;
using System.Collections.Generic;
using DownloadingExcelFile.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;

namespace DownloadingExcelFile.Pages
{
    public class DownloadModel : PageModel
    {
        public IActionResult OnGet()
        {
            List<Fruit> fruits;
            var jsonData = System.IO.File.ReadAllText("jsondata.txt");
            fruits = JsonConvert.DeserializeObject<List<Fruit>>(jsonData);
            MemoryStream memoryStream = new MemoryStream();

            using (SpreadsheetDocument spreadsheet = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = GenerateStylesheet();
                stylesPart.Stylesheet.Save();

                Columns columns = new Columns(
                    new Column
                    {
                        Min = 1,
                        Max = 1,
                        Width = 8,
                        CustomWidth = true

                    },
                    new Column
                    {
                        Min = 2,
                        Max = 5,
                        Width = 25,
                        CustomWidth = true


                    },
                    new Column
                    {
                        Min = 6,
                        Max = 10,
                        Width = 10,
                        CustomWidth = true

                    }
                    );
                worksheetPart.Worksheet.AppendChild(columns);
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Fruits" };
                sheets.Append(sheet);
                workbookPart.Workbook.Save();
                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                Row row = new Row();

                row.Append(
                    ConstructCell("Id", CellValues.String, 2),
                    ConstructCell("Name", CellValues.String, 2),
                    ConstructCell("Family", CellValues.String, 2),
                    ConstructCell("Genus", CellValues.String, 2),
                    ConstructCell("Order", CellValues.String, 2),
                    ConstructCell("Carbohydrates", CellValues.String, 2),
                    ConstructCell("Protein", CellValues.String, 2),
                    ConstructCell("Fat", CellValues.String, 2),
                    ConstructCell("Calories", CellValues.String, 2),
                    ConstructCell("Sugar", CellValues.String, 2));



                sheetData.AppendChild(row);

                foreach (var fruit in fruits)
                {

                    row = new Row();

                    row.Append(

                            ConstructCell(fruit.id.ToString(), CellValues.Number, 1),
                            ConstructCell(fruit.name, CellValues.String, 1),
                            ConstructCell(fruit.family, CellValues.String, 1),
                            ConstructCell(fruit.genus, CellValues.String, 1),
                            ConstructCell(fruit.order, CellValues.String, 1),
                            ConstructCell(fruit.nutritions.carbohydrates.ToString(), CellValues.Number, 3),
                            ConstructCell(fruit.nutritions.protein.ToString(), CellValues.Number, 3),
                            ConstructCell(fruit.nutritions.fat.ToString(), CellValues.Number, 3),
                            ConstructCell(fruit.nutritions.calories.ToString(), CellValues.Number, 3),
                            ConstructCell(fruit.nutritions.sugar.ToString(), CellValues.Number, 3));



                    sheetData.AppendChild(row);

                }

                worksheetPart.Worksheet.Save();
                spreadsheet.Save();
                spreadsheet.Close();

            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var dateTime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");
            string fileFullName = "Output_" + dateTime + ".xlsx";
            return File(memoryStream, "application/octet-stream", fileFullName);
        }

        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }
        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true },// header
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, NumberFormatId = 2, ApplyBorder = true }

                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;

        }

    }
}

