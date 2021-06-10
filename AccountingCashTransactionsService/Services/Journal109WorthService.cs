using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Journal109ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class Journal109WorthService : EntityRepositoryCore<Journal109Worth>, IJournal109WorthService
    {
        /// <summary>
        /// 
        /// </summary>
        DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        public Journal109WorthService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int bankCode, DateTime date)
        {
            try
            {
                var result = new Journal109WorthResultQuerys(); 
                var dates = date.ToString("dd.MM.yyyy");
                string sqltxt = $"select * from table(return_table_109_worth(to_date('{dates}','DD.MM.YYYY'),{bankCode}))";
                var entity = _context.Query<QueryModelFor109Worth>().FromSql(sqltxt).ToList();
                result.Data = entity;
                result.Total = entity.Count();

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private string GetBankNameById(int bankId)
        {
            var result = "---";
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                if (!wasOpen) cmd.Connection.Open();
                try
                {
                    cmd.CommandText = $"select GET_BANK_NAME({bankId}) from dual";
                    var scalaerResult = cmd.ExecuteScalar();
                    var bankName = scalaerResult == DBNull.Value ? string.Empty : (string)scalaerResult;
                    result = bankName;
                }
                finally
                {
                    if (!wasOpen) cmd.Connection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public byte[] ToExport(List<ExcelModel> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                string[] s = { "B", "C", "D", "E", "F", "G" };
                string[] s1 = { "D", "E", "F", "G" };
                string[] columName = { "Сони", "асосида сақлашга қабул қилинган қимматликлар бўйича берилган 004-шаклдаги квитанциялар сони", "Ф.И.О.", "имзоси" };
                int[] columWidth = { 5, 40, 20, 20, 25, 20 };
                var a = 11;
                string[] headerName = { bankName, "Кирим касса ҳужжатларининг сони ва суммаси ҳамда сақлашга қабул қилинган қимматликлар сони ва суммаси тўғрисида МАЪЛУМОТНОМА", "КИТОБИ " };
                int[] headerCount = { 2, 4, 6 };
                int count = 0;
                double summa = 0;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Journal109Worth");
                ws.Cells["D1"].Value = "109-Шакл";
                ws.Cells["D1"].Style.Font.Bold = true;
                ws.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < headerCount.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                ws.Cells[$"B9:B10"].Value = "T/p";
                ws.Cells[$"B9:B10"].Merge = true;
                ws.Cells[$"B9:B10"].Style.Font.Bold = true;
                ws.Cells[$"B9:B10"].Style.WrapText = true;
                ExcelAroundBorder.AroundBorder(ws, $"B9", "B10");
                ExcelAroundBorder.AroundBorder(ws, $"C9", "C10");
                ws.Cells[$"C9:C10"].Value = "Кирим қилинган нақд пул ва бошқа қимматликлар номи";
                ws.Cells[$"C9:C10"].Merge = true;
                ws.Cells[$"C9:C10"].Style.Font.Bold = true;
                ws.Cells[$"C9:C10"].Style.WrapText = true;
                ExcelAroundBorder.AroundBorder(ws, $"D9", "E9");
                ws.Cells[$"D9:E9"].Value = "Кирим ҳужжатлари";
                ws.Cells[$"D9:E9"].Merge = true;
                ws.Cells[$"D9:E9"].Style.Font.Bold = true;
                ws.Cells[$"D9:E9"].Style.WrapText = true;
                ExcelAroundBorder.AroundBorder(ws, $"F9", "G9");
                ws.Cells[$"F9:G9"].Value = "Кирим амалларини назорат қилувчи масъул ходим";
                ws.Cells[$"F9:G9"].Merge = true;
                ws.Cells[$"F9:G9"].Style.Font.Bold = true;
                ws.Cells[$"F9:G9"].Style.WrapText = true;

                for (int i = 0; i < columWidth.Length; i++)
                {
                    ws.Column(i + 2).Width = columWidth[i];
                    ws.Column(i + 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }


                for (int i = 0; i < s1.Length; i++)
                {
                    ws.Cells[$"{s[i]}10"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{s1[i]}10");
                    ws.Cells[$"{s1[i]}10"].Value = $"{columName[i]}";
                    ws.Cells[$"{s1[i]}10"].Style.Font.Bold = true;
                    ws.Cells[$"C{a}"].Style.WrapText = true;
                }

                foreach (var item in model)
                {
                    for (int i = 0; i < s.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");

                    ws.Cells["F8"].Value = "Сана:";
                    ws.Cells[$"G8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"B{a}"].Value = $"{a - 9}";
                    ws.Cells[$"C{a}"].Value = $"{item.SymbolName}";
                    ws.Cells[$"C{a}"].Style.WrapText = true;
                    ws.Cells[$"D{a}"].Value = $"{item.Count}";
                    ws.Cells[$"E{a}"].Value = $"{item.Summa:N}";
                    ws.Cells[$"F{a}"].Value = $"назоратчи бухгалтер (user name)";
                    ws.Cells[$"F{a}"].Style.WrapText = true ;
                    ws.Cells[$"G{a}"].Value = $"";
                    count += item.Count;
                    summa += item.Summa;
                    a++;
                }
                ws.Cells[$"B{a}:C{a}"].Value = "Жами:";
                ws.Cells[$"B{a}:C{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"B{a}:C{a}"].Style.Font.Bold = true;
                ws.Cells[$"B{a}:C{a}"].Merge = true;
                ws.Cells[$"E{a}"].Style.Font.Bold = true;
                ws.Cells[$"E{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"D{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"D{a}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a}", $"C{a}");
                ExcelAroundBorder.AroundBorder(ws, $"D{a}");
                ExcelAroundBorder.AroundBorder(ws, $"C{a}");
                ExcelAroundBorder.AroundBorder(ws, $"E{a}");
                ExcelAroundBorder.AroundBorder(ws, $"F{a}");
                ExcelAroundBorder.AroundBorder(ws, $"G{a}");
                ws.Cells[$"D{a}"].Value = $"{count}";
                ws.Cells[$"E{a}"].Value = $"{summa:N}";
                ws.Cells[$"D{a}"].Style.Font.Bold = true;
                ws.Cells[$"E{a}"].Style.Font.Bold = true;
                ws.Cells[$"D{a + 2}"].Value = $"Кассир:";
                ws.Cells[$"E{a + 2}"].Value = $"{user}";
                ws.Cells[$"D{a + 2}"].Style.Font.Bold = true;
                ws.Cells["B2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
