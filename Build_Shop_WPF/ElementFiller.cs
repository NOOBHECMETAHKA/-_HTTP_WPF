using Build_Shop_WPF.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;

namespace Build_Shop_WPF
{
    public class ElementFiller
    {
        HttpManipulation httpManipulation;
        public ElementFiller()
        {
            this.httpManipulation = new HttpManipulation();
        }

        public async void FillDataGrid<T>(DataGrid dataGrid, string query)
        {
            var responce = await httpManipulation.GetStringAsync(query);
            var collection = JsonConvert.DeserializeObject<List<T>>(responce);
            dataGrid.ItemsSource = collection;
        }

        public async void FillComboBox<T>(ComboBox comboBox, string query)
        {
            HttpManipulation http = new HttpManipulation();
            var responce = await http.GetStringAsync(query);
            var collection = JsonConvert.DeserializeObject<List<T>>(responce);
            comboBox.ItemsSource = collection;
        }

        public async Task<List<T>> ReturnList<T>(string query)
        {
            var responce = await httpManipulation.GetStringAsync(query);
            var collection = JsonConvert.DeserializeObject<List<T>>(responce);
            return collection;
        }

        public async void GetFromApiToExcel(string uri)
        {
            var clients = new List<Client>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(httpManipulation._httpClient.BaseAddress + uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    clients = JsonConvert.DeserializeObject<List<Client>>(responseContent);
                }
            }

            var spreadsheetDocument = SpreadsheetDocument.Create("clients.xlsx", SpreadsheetDocumentType.Workbook);

            // Добавляем лист в документ
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);
            var sheets = new Sheets();
            var sheet = new Sheet { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Clients" };
            sheets.Append(sheet);
            workbookPart.Workbook = new Workbook();
            workbookPart.Workbook.Append(sheets);

            var headerRow = new Row();
            headerRow.Append(new Cell() { CellValue = new CellValue("ID"), DataType = CellValues.String });
            headerRow.Append(new Cell() { CellValue = new CellValue("Фамилия"), DataType = CellValues.String });
            headerRow.Append(new Cell() { CellValue = new CellValue("Имя"), DataType = CellValues.String });
            headerRow.Append(new Cell() { CellValue = new CellValue("Номер телефона"), DataType = CellValues.String });
            headerRow.Append(new Cell() { CellValue = new CellValue("Электронная почта"), DataType = CellValues.String });
            sheetData.AppendChild(headerRow);

            foreach (var client in clients)
            {
                var dataRow = new Row();
                dataRow.Append(new Cell() { CellValue = new CellValue(client.IdClient.ToString()), DataType = CellValues.Number });
                dataRow.Append(new Cell() { CellValue = new CellValue(client.FirstName), DataType = CellValues.String });
                dataRow.Append(new Cell() { CellValue = new CellValue(client.LastName), DataType = CellValues.String });
                dataRow.Append(new Cell() { CellValue = new CellValue(client.PhoneNumber), DataType = CellValues.String });
                dataRow.Append(new Cell() { CellValue = new CellValue(client.Email), DataType = CellValues.String });
                sheetData.AppendChild(dataRow);
            }

            // Сохраняем документ и закрываем его
            worksheetPart.Worksheet.Save();
            workbookPart.Workbook.Save();
            spreadsheetDocument.Close();
        }

        public async void GetFromApiToWord(string uri)
        {
            var projects = new List<Project>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(httpManipulation._httpClient.BaseAddress + uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    projects = JsonConvert.DeserializeObject<List<Project>>(responseContent);
                }

                using (var document = WordprocessingDocument.Create(Directory.GetCurrentDirectory()+ "\\Projects.docx", WordprocessingDocumentType.Document))
                {
                    // Добавляем главный элемент документа - Body
                    var mainPart = document.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = mainPart.Document.AppendChild(new Body());

                    // Добавляем заголовок документа
                    var title = new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("List of Projects")));
                    body.AppendChild(title);

                    // Добавляем таблицу
                    var table = new DocumentFormat.OpenXml.Wordprocessing.Table();

                    // Добавляем заголовки столбцов таблицы
                    var row = new TableRow();
                    row.Append(CreateTableCell("ID"));
                    row.Append(CreateTableCell("Name"));
                    row.Append(CreateTableCell("Date Create"));
                    row.Append(CreateTableCell("Time Create"));
                    row.Append(CreateTableCell("Date End"));
                    row.Append(CreateTableCell("Time End"));
                    row.Append(CreateTableCell("Client ID"));
                    table.AppendChild(row);

                    // Добавляем данные в таблицу
                    foreach (var project in projects)
                    {
                        row = new TableRow();
                        row.Append(CreateTableCell(project.IdProject.ToString()));
                        row.Append(CreateTableCell(project.Name));
                        row.Append(CreateTableCell(project.DateCreate.ToShortDateString()));
                        row.Append(CreateTableCell(project.TimeCreate.ToString()));
                        row.Append(CreateTableCell(project.DateEnd.ToShortDateString()));
                        row.Append(CreateTableCell(project.TimeEnd.ToString()));
                        row.Append(CreateTableCell(project.ClientId.ToString()));
                        table.AppendChild(row);
                    }

                    body.AppendChild(table);
                }
            }


        }

        private TableCell CreateTableCell(string text)
        {
            var cell = new TableCell(new Paragraph(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text(text))));
            return cell;
        }
    }
}
