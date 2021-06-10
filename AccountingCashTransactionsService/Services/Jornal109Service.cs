using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.PostModels.CashOperations;
using Entitys.ViewModels.CashOperation.Journal109ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class Jornal109Service : EntityRepositoryCore<Journal109>, IJournal109Service
    {
        private readonly DataContext _context;
        private CommonHelper _commonHelper;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dBcontext"></param>
        public Jornal109Service(IDbContext context, DataContext dBcontext) : base(context)
        {
            _context = dBcontext;
            _commonHelper = new CommonHelper(dBcontext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private Journal109 toEntity(Journal109Model model, int userId)
        {
            var result = new Journal109();
            result.Id = model.Id;
            result.SymbolName = model.SymbolName;
            result.Count = model.Count;
            result.Summa = model.Summa;
            result.UserId = userId;
            result.Date = model.Date;
            result.BankCode = model.BankCode;
            result.SymbolCode = model.SymbolCode;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetAll(int bankCode, int userId, DateTime date)
        {
            try
            {
                var result = new Journal109ResultQuery();
                var dates = date.ToString("dd.MM.yyyy");
                string sqltxt = $"select * from table(return_table_109(to_date('{dates}','DD.MM.YYYY'),{bankCode}))";
                var entity = _context.Query<QueryModel>().FromSql(sqltxt).OrderBy(f => f.Id).ToList();
                result.Data = entity;
                result.Total = entity.Count();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Journal109, EventType.Read);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
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
                string[] s = { "B", "C", "D", "E", "F" };
                string[] columName = { "T/P", "Символ коди", "Символ номи", "Сони", "Сумма" };
                int[] columWidth = { 5, 20, 40, 20, 25 };
                var a = 10;
                string[] headerName = { bankName, "Кирим касса ҳужжатларининг сони ва суммаси ҳамда сақлашга қабул қилинган қимматликлар сони ва суммаси тўғрисида МАЪЛУМОТНОМА", "КИТОБИ " };
                int[] headerCount = { 2, 4, 6 };
                int count = 0;
                double summa = 0;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Journal109");
                ws.Cells["F1"].Value = "109-Шакл";
                ws.Cells["F1"].Style.Font.Bold = true;
                ws.Cells["F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                for (int i = 0; i < headerCount.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:F{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                }

                for (int i = 0; i < columWidth.Length; i++)
                {
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

                    ws.Cells["E8"].Value = "Сана:";
                    ws.Cells[$"F8"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"B{a}"].Value = $"{a - 9}";
                    ws.Cells[$"C{a}"].Value = $"{item.Kod}";
                    ws.Cells[$"D{a}"].Value = $"{item.SymbolName}";
                    ws.Cells[$"D{a}"].Style.WrapText = true;
                    ws.Cells[$"E{a}"].Value = $"{item.Count}";
                    ws.Cells[$"F{a}"].Value = $"{item.Summa:N}";
                    count += item.Count;
                    summa += item.Summa;
                    a++;
                }
                ws.Cells[$"B{a}:D{a}"].Value = "Жами:";
                ws.Cells[$"B{a}:D{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"B{a}:D{a}"].Style.Font.Bold = true;
                ws.Cells[$"B{a}:D{a}"].Merge = true;
                ws.Cells[$"E{a}"].Style.Font.Bold = true;
                ws.Cells[$"E{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"F{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"F{a}"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"B{a}", $"D{a}");
                ExcelAroundBorder.AroundBorder(ws, $"E{a}");
                ExcelAroundBorder.AroundBorder(ws, $"F{a}");
                ws.Cells[$"E{a}"].Value = $"{count}";
                ws.Cells[$"F{a}"].Value = $"{summa:N}";
                ws.Cells[$"E{a}"].Style.Font.Bold = true;
                ws.Cells[$"F{a}"].Style.Font.Bold = true;
                ws.Cells[$"E{a + 2}"].Value = $"Кассир:";
                ws.Cells[$"F{a + 2}"].Value = $"{user}";
                ws.Cells[$"E{a + 2}"].Style.Font.Bold = true;
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
