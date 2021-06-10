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
using System.Text;

namespace AccountingCashTransactionsService.Services
{
    public class ReturnTable109ValService : EntityRepositoryCore<Journal109Val>, IReturnTable109ValService
    {
        DataContext _context;

        public ReturnTable109ValService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        public ResponseCoreData GetByDate(int bankCode, DateTime date)
        {
            try
            {
                var result = new Journal109ValResultQuries();
                var dates = date.ToString("dd.MM.yyyy");
                string sql = $"select * from table(RETURN_TABLE_109_VAL(to_date('{dates}','DD.MM.YYYY'), {bankCode}))";
                var entity = _context.Query<Journal109ValQueryModel>().FromSql(sql).ToList();
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
        /// <returns></returns>
        public byte[] ToExport(List<ExcelModel> model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                string[] s = { "B", "C", "D" };
                string[] columName = { "Символ номи", "Сони", "Сумма" };
                int[] columWidth = { 40, 20, 25 };
                var a = 10;
                int[] headerCount = { 2, 4, 6 };
                int count = 0;
                double summa = 0;
                string[] headerName = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ", "КИТОБИ " };
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var excel = new ExcelPackage();

                var ws = excel.Workbook.Worksheets.Add("Journal110Val");
                ws.Cells["D1"].Value = "110-Шакл";
                ws.Cells["D1"].Style.Font.Bold = true;
                ws.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < columWidth.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:D{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Column(i + 2).Width = columWidth[i];
                    ws.Column(i + 2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[$"{s[i]}9"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{s[i]}9");
                    ws.Cells[$"{s[i]}9"].Value = $"{columName[i]}";
                    ws.Cells[$"{s[i]}9"].Style.Font.Bold = true;
                }

                foreach (var item in model)
                {
                    for (int i = 0; i < s.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");

                    ws.Cells["C8"].Value = "Сана:";
                    ws.Cells[$"D8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"B{a}"].Value = $"{item.SymbolName}";
                    ws.Cells[$"B{a}"].Style.WrapText = true;
                    ws.Cells[$"C{a}"].Value = $"{item.Count}";
                    ws.Cells[$"D{a}"].Value = $"{item.Summa:N}";
                    count += item.Count;
                    summa += item.Summa;
                    a++;
                }

                ws.Cells[$"B{a}"].Value = "Жами:";
                ws.Cells[$"B{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"B{a}"].Style.Font.Bold = true;
                ws.Cells[$"C{a}"].Style.Font.Bold = true;
                ws.Cells[$"C{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"D{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"D{a}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a}");
                ExcelAroundBorder.AroundBorder(ws, $"D{a}");
                ExcelAroundBorder.AroundBorder(ws, $"C{a}");
                ws.Cells[$"C{a}"].Value = $"{count}";
                ws.Cells[$"D{a}"].Value = $"{summa:N}";
                ws.Cells[$"C{a}"].Style.Font.Bold = true;
                ws.Cells[$"D{a}"].Style.Font.Bold = true;
                ws.Cells[$"C{a + 2}"].Value = $"Кассир:";
                ws.Cells[$"D{a + 2}"].Value = $"{user}";
                ws.Cells[$"C{a + 2}"].Style.Font.Bold = true;
                ws.Cells["B2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
