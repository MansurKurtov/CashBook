using AccountingCashTransactionsService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.QueryModel;
using Entitys.ViewModels.CashOperation.Journal109ViewModel;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class Journal110WorthService : EntityRepositoryCore<Journal110Worth>, IJournal110WorthService
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
        public Journal110WorthService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ResponseCoreData GetByDate(int bankCode, DateTime date)
        {
            try
            {
                var result = new Journal109WorthResultQuerys();
                var dates = date.ToString("dd.MM.yyyy");
                string sql = $"select * from table(RETURN_TABLE_110_WORTH(to_date('{dates}','DD.MM.YYYY'), {bankCode}))";
                var entity = _context.Query<QueryModelFor109Worth>().FromSql(sql).ToList();
                result.Data = entity;
                result.Total = entity.Count();

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public ResponseCoreData GetCashValue(int bankCode, DateTime date)
        {
            try
            {
                var dates = date.ToString("dd.MM.yyyy");
                string sql = $"select * from table(return_table_110(to_date('{dates}','DD.MM.YYYY'),{bankCode}))";
                var result = _context.Query<ReturnTable110Model>().FromSql(sql).ToList();

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        public ResponseCoreData GetAll155CashValue(DateTime date, int userId, int operationId)
        {
            try
            {
                double result = 0;
                var entity = _context.Book155s.Where(f => f.Date == date && f.ToCashierId == userId && f.OperationId == operationId).ToList();
                entity.ForEach(f => result += f.CashValue);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        public ResponseCoreData GetWorthCashValue(DateTime date, int toCashierId)
        {
            try
            {
                //select worth_account,sum(cash_value)  from book_155 where to_cashier_id=48 and "DATE" = to_date('03.06.2020','DD.MM.YYYY') GROUP BY worth_account;
                string dates = date.ToString("dd.MM.yyyy");
                string sql = $"select worth_account,sum(cash_value)  from book_155 where to_cashier_id={toCashierId} and \"DATE\" = to_date('{dates}','DD.MM.YYYY') GROUP BY worth_account";
                var result = _context.Query<GetWorthCashValue>().FromSql(sql).ToList();

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
        public byte[] ToExport(Journal110WorthExcelModel model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Journal110");

                string[] headerColumn = { "B", "C", "D", "E", "G", "H" };
                string[] rows = { "B", "C", "D", "E", "F", "G", "H" };
                string[] headerColumnMerge = { "B", "C", "D", "F", "G", "H" };

                string[] columName = {
                    "Т/р",
                    "Кимматлик номи",
                    @"Чиқим операцияларини назорат қилувчи масъул ходимнинг Ф.И.О.",
                    "Чиқим ҳужжатлари",
                    "Назорат қилувчи масъул ходимнинг имзоси",
                    "Ҳисоб бериш шарти билан олинган нақд пул ва бошқа қимматликлар суммаси"
                };
                string[] rowSpanHeaderName = { "Сони", "Сумма" };

                string[] headerName = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ", "КИТОБИ " };

                int[] columWidth = { 5, 10, 20, 15, 25, 10 };
                int[] headerCount = { 2, 4, 6 };
                int[] mergedHeader = { 1, 1, 1, 0, 1, 1 };

                var a = 11;
                int count = 0;
                double summa = 0;
                double cashValue = 0;
                ws.Cells["H1"].Value = "110-Шакл";
                ws.Cells["H1"].Style.Font.Bold = true;
                ws.Cells["H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < headerCount.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:H{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                for (int i = 0; i < headerColumn.Length; i++)
                {
                    ws.Column(i + 2).Width = columWidth[i];
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Value = $"{columName[i]}";
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Merge = true;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.WrapText = true;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{headerColumn[i]}{9}", $"{headerColumnMerge[i]}{9 + mergedHeader[i]}");
                }

                ws.Cells["E10"].Value = rowSpanHeaderName[0];
                ws.Cells["E10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["E10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["E10"].Style.Font.Bold = true;
                ws.Cells["F10"].Value = rowSpanHeaderName[1];
                ws.Cells["F10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["F10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F10"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"E10");
                ExcelAroundBorder.AroundBorder(ws, $"F10");

                int lastGroupCount = a;

                foreach (var group in model.Data)
                {
                    bool isFirst = true;
                    int index = 0;
                    foreach (var item in group?.Value)
                    {
                        for (int i = 0; i < rows.Length - 1; i++)
                            ExcelAroundBorder.AroundBorder(ws, $"{rows[i]}{a}");

                        ws.Cells[$"B{a}"].Value = $"{group.Position + 1 + index++}";
                        ws.Cells[$"C{a}"].Value = $"{item.Name}";
                        ws.Cells[$"D{a}"].Value = $"{item.Fio}";
                        ws.Cells[$"D{a}"].Style.WrapText = true;
                        ws.Cells[$"E{a}"].Value = $"{item.Count:N}";
                        ws.Cells[$"F{a}"].Value = $"{item.Summa:N}";
                        ws.Cells[$"G{a}"].Value = $"";
                        if (isFirst)
                        {
                            cashValue += (double)group?.Grouped;
                            int accum = (group?.Count != 0 ? -1 : 0);
                            ws.Cells[$"H{lastGroupCount}:H{lastGroupCount + group?.Count - accum}"].Merge = true;
                            ws.Cells[$"H{lastGroupCount}:H{lastGroupCount + group?.Count - accum}"].Value = $"{group?.Grouped}";
                            ExcelAroundBorder.AroundBorder(ws, $"H{lastGroupCount}", $"H{lastGroupCount + group?.Count - accum}");
                            lastGroupCount += group.Count;
                            isFirst = false;
                        }
                        count += item.Count;
                        summa += item.Summa;
                        a++;
                    }
                }


                ws.Cells[$"B{lastGroupCount}:E{lastGroupCount}"].Value = "Жами:";
                ws.Cells[$"B{lastGroupCount}:E{lastGroupCount}"].Merge = true;
                ws.Cells[$"B{lastGroupCount}:E{lastGroupCount}"].Style.Font.Bold = true;
                ws.Cells[$"E{lastGroupCount}"].Value = $"{count}";
                ws.Cells[$"F{lastGroupCount}"].Value = $"{summa}";
                ws.Cells[$"G{lastGroupCount}"].Value = $"";
                ws.Cells[$"H{lastGroupCount}"].Value = $"{cashValue}";
                ws.Cells[$"H{lastGroupCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                ExcelAroundBorder.AroundBorder(ws, $"B{lastGroupCount}", $"H{lastGroupCount}");

                ws.Cells[$"F{lastGroupCount + 2}:H{lastGroupCount + 2}"].Value = $"Касса мудирига: {0.00}";
                ws.Cells[$"F{lastGroupCount + 2}:H{lastGroupCount + 2}"].Merge = true;
                ws.Cells[$"F{lastGroupCount + 2}:H{lastGroupCount + 2}"].Style.Font.Bold = true;
                ws.Cells[$"F{lastGroupCount + 2}:H{lastGroupCount + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[$"F{lastGroupCount + 3}:H{lastGroupCount + 3}"].Value = $"Кассир: {user}_____";
                ws.Cells[$"F{lastGroupCount + 3}:H{lastGroupCount + 3}"].Merge = true;
                ws.Cells[$"F{lastGroupCount + 3}:H{lastGroupCount + 3}"].Style.Font.Bold = true;
                ws.Cells[$"F{lastGroupCount + 3}:H{lastGroupCount + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[$"F{lastGroupCount + 4}:H{lastGroupCount + 4}"].Value = $"Назоратчи хисобчи: {model.HeadCashier} _____";
                ws.Cells[$"F{lastGroupCount + 4}:H{lastGroupCount + 4}"].Merge = true;
                ws.Cells[$"F{lastGroupCount + 4}:H{lastGroupCount + 4}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[$"F{lastGroupCount + 4}:H{lastGroupCount + 4}"].Style.Font.Bold = true;



                ws.Cells["B2:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
