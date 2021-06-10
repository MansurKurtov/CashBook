using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation;
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
    public class Journal111Service : EntityRepositoryCore<Journal111>, IJournal111Service
    {
        private DataContext _context;
        private CommonHelper _commonHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contexts"></param>
        /// <param name="context"></param>
        public Journal111Service(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
            _commonHelper = new CommonHelper(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="userId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int bankCode, int userId, DateTime date)
        {
            try
            {
                var result = new Journal111ResultQuery();
                var dates = $"{date:dd.MM.yyyy}";
                string sqltxt = $"select * from table(GET_JOURNAL_111(to_date('{dates}','DD.MM.YYYY'),{bankCode})) order by kod";
                var entity = _context.Query<QueryModel>().FromSql(sqltxt).ToList();
                result.Data = GetListSort(entity);
                result.Total = entity.Count();

                //_commonHelper.SaveUserEvent(userId, ModuleType.Journal111, EventType.Read);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        private List<Entitys.Models.QueryModelForJournal111> GetListSort(List<Entitys.Models.QueryModel> list)
        {
            var journal111s = new List<QueryModelForJournal111>();

            var defValues = GetDefaultValues();
            foreach (var item1 in list)
            {
                var defVal = defValues.Where(w => w.EngName.Equals(item1.SymbolName)).FirstOrDefault();
                var journal111 = new QueryModelForJournal111();
                journal111.Id = item1.Id;
                journal111.SymbolName = defVal?.Name ?? item1.SymbolName;
                journal111.Count = item1.Count;
                journal111.Summa = item1.Summa;
                journal111s.Add(journal111);
            }

            return journal111s;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<Lists> GetDefaultValues()
        {
            var result = new List<Lists>();
            result.Add(new Lists() { EngName = "INCOME", Name = "Бир кунлик жами кирим" });
            result.Add(new Lists() { EngName = "INCOME_BALANCE", Name = "Миллий валюта" });
            result.Add(new Lists() { EngName = "INCOME_OTHER", Name = "Кимматликлар" });
            result.Add(new Lists() { EngName = "INCOME_VALUT", Name = "Хорижий валюта" });
            result.Add(new Lists() { EngName = "OUTGO", Name = "Бир кунлик жами чиқим" });
            result.Add(new Lists() { EngName = "OUTGO_BALANCE", Name = "Миллий валюта" });
            result.Add(new Lists() { EngName = "OUTGO_OTHER", Name = "Кимматликлар" });
            result.Add(new Lists() { EngName = "OUTGO_VALUT", Name = "Хорижий валюта" });
            result.Add(new Lists() { EngName = "_VALUT", Name = " " });
            result.Add(new Lists() { EngName = "_BALANCE", Name = " " });
            result.Add(new Lists() { EngName = "_OTHER", Name = " " });


            return result;
        }

        private string GetBankName(int bankId)
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
        public byte[] ToExport(int bankCode, Journal111ExcelPostModel model, string user)
        {
            var bankName = GetBankName(bankCode);
            try
            {
                int num = 0;
                var dates = $"{model.Date:dd.MM.yyyy}";
                string sqltxt = $"select * from table(GET_JOURNAL_111(to_date('{dates}','DD.MM.YYYY'),{bankCode})) order by kod";
                var entity = _context.Query<QueryModel>().FromSql(sqltxt).ToList();
                var models = GetListSort(entity);
                string[] s = { "B", "C", "D", "E" };
                string[] columName = { "T/p", "Символ номи", "Сони", "Сумма" };
                int[] columWidth = { 7, 30, 20, 25 };
                int[] headerCount = { 2, 4, 6 };
                string[] headerName = { bankName, "Кунлик касса айланмалари бўйича йиғма МАЪЛУМОТНОМА ", "КИТОБИ " };

                var a = 10;

                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Journal111");
                ws.Cells["E1"].Value = "111-Шакл";
                ws.Cells["E1"].Style.Font.Bold = true;
                ws.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                for (int i = 0; i < headerName.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:E{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

                foreach (var item in models)
                {
                    for (int i = 0; i < s.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{s[i]}{a}");

                    ws.Cells["D8"].Value = "Сана:";
                    ws.Cells[$"E8"].Value = $"{dates}";
                    if (item.OrignText == "bold")
                    {
                        num++;
                        ws.Cells[$"B{a}"].Value = $"{num}";
                        ws.Cells[$"C{a}"].Value = $"{item.SymbolName}";
                        ws.Cells[$"B{a}"].Style.Font.Bold = true;
                        ws.Cells[$"C{a}"].Style.Font.Bold = true;
                        ws.Cells[$"B{a}"].Style.WrapText = true;
                        ws.Cells[$"C{a}"].Style.WrapText = true;
                    }
                    else
                        ws.Cells[$"C{a}"].Value = $"{item.SymbolName}";
                    ws.Cells[$"C{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    ws.Cells[$"D{a}"].Value = $"{item.Count}";
                    ws.Cells[$"E{a}"].Value = $"{item.Summa:N}";
                    a++;
                }

                ws.Cells[$"D{a + 1}"].Value = $"Кассир:";
                ws.Cells[$"E{a + 1}"].Value = $"{user}";
                ws.Cells[$"D{a + 2}"].Value = $"Бош Ҳисобчи:";
                ws.Cells[$"E{a + 2}"].Value = $"{model.BoshKassir}";
                ws.Cells[$"D{a + 2}"].Style.Font.Bold = true;
                ws.Cells[$"D{a + 1}"].Style.Font.Bold = true;
                ws.Cells["B2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        class Lists
        {
            public int Code { get; set; }
            public string EngName { get; set; }
            public string Name { get; set; }
        }
    }
}
