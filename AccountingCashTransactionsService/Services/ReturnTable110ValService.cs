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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace AccountingCashTransactionsService.Services
{
    public class ReturnTable110ValService : EntityRepositoryCore<Journal110Val>, IReturnTable110ValService
    {
        DataContext _context;

        public ReturnTable110ValService(IDbContext contexts, DataContext context) : base(contexts)
        {
            _context = context;
        }

        public ResponseCoreData GetByDate(int bankCode, DateTime date)
        {
            try
            {
                var result = new ReturnTable110ValResultQuery();
                //var result = new Journal110ValResultQuries();
                var dates = date.ToString("dd.MM.yyyy");
                string sql = $"select * from table(RETURN_TABLE_110_VAL(to_date('{dates}','DD.MM.YYYY'), {bankCode}))";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="toCashierId"></param>
        /// <returns></returns>
        public ResponseCoreData GetValCashValue(DateTime date, int toCashierId)
        {
            try
            {
                string dates = date.ToString("dd.MM.yyyy");
                string sql = $"select spr_object_id,sum(cash_value)  from book_155 where to_cashier_id={toCashierId} and \"DATE\" = to_date('{dates}','DD.MM.YYYY') GROUP BY spr_object_id";
                var result = _context.Query<GetValCashValue>().FromSql(sql).ToList();

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
        public byte[] ToExport(Jourlan110ReturnExcelModel model, string user, int bankCode)
        {
            var bankName = GetBankNameById(bankCode);
            try
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Journal110");

                string[] headerColumn = { "B", "C", "D", "E", "F", "G", "I", "J" };
                string[] rows = { "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                string[] headerColumnMerge = { "B", "C", "D", "E", "F", "H", "I", "J" };

                string[] columName = {
                    "Т/р",
                    "Символ коди",
                    "Символ номи",
                    "Валюта номи",
                    @"Чиқим операцияларини назорат қилувчи масъул ходимнинг Ф.И.О.",
                    "Чиқим ҳужжатлари",
                    "Назорат қилувчи масъул ходимнинг имзоси",
                    "Ҳисоб бериш шарти билан олинган нақд пул ва бошқа қимматликлар суммаси"
                };
                string[] rowSpanHeaderName = { "Сони", "Сумма" };

                string[] headerName = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ", "КИТОБИ " };

                int[] columWidth = { 5, 10, 15, 15, 15, 25, 10, 10 };
                int[] headerCount = { 2, 4, 6 };
                int[] mergedHeader = { 1, 1, 1, 1, 1, 0, 1, 1 };

                int count = 0;
                double summa = 0;
                double cashValue = 0;
                ws.Cells["J1"].Value = "110-Шакл";
                ws.Cells["J1"].Style.Font.Bold = true;
                ws.Cells["J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < headerCount.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Merge = true;                    
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:J{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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

                ws.Cells["G10"].Value = rowSpanHeaderName[0];
                ws.Cells["G10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["G10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G10"].Style.Font.Bold = true;
                ws.Cells["H10"].Value = rowSpanHeaderName[1];
                ws.Cells["H10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["H10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["H10"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"G10");
                ExcelAroundBorder.AroundBorder(ws, $"H10");

                int lastGroupCount = 11;

                foreach (var group in model.Data)
                {
                    bool isFirst = true;
                    int index = 0;
                    var a = lastGroupCount;
                    foreach (var item in group?.Value)
                    {
                        for (int i = 0; i < rows.Length - 1; i++)
                            ExcelAroundBorder.AroundBorder(ws, $"{rows[i]}{a}");

                        ws.Cells[$"B{a}"].Value = $"{group.Position + 1 + index++}";
                        ws.Cells[$"C{a}"].Value = $"{item.Kod}";
                        ws.Cells[$"D{a}"].Value = $"{item.SymbolName}";
                        ws.Cells[$"E{a}"].Value = $"{item.ValutName}";
                        ws.Cells[$"F{a}"].Value = $"{item.Fio}";
                        ws.Cells[$"F{a}"].Style.WrapText = true;
                        ws.Cells[$"G{a}"].Value = $"{item.Count:N}";
                        ws.Cells[$"H{a}"].Value = $"{item.Summa:N}";
                        ws.Cells[$"I{a}"].Value = $"";
                        if (isFirst)
                        {
                            cashValue += (double)group?.Grouped;
                            int accum = (group?.Count != 0 ? 1 : 0);
                            ws.Cells[$"J{lastGroupCount}:J{lastGroupCount + group?.Count - accum}"].Merge = true;
                            ws.Cells[$"J{lastGroupCount}:J{lastGroupCount + group?.Count - accum}"].Value = $"{group?.Grouped}";
                            ExcelAroundBorder.AroundBorder(ws, $"J{lastGroupCount}", $"J{lastGroupCount + group?.Count - accum}");
                            lastGroupCount += group.Count;
                            isFirst = false;
                        }
                        count += item.Count;
                        summa += item.Summa;
                        a++;
                    }
                    ws.Cells[$"B{lastGroupCount}:E{lastGroupCount}"].Value = $"{group?.Value[0]?.ValutName.TrimEnd()} жами:";
                    ws.Cells[$"B{lastGroupCount}:E{lastGroupCount}"].Merge = true;
                    ExcelAroundBorder.AroundBorder(ws, $"B{lastGroupCount}", $"E{lastGroupCount}");

                    ws.Cells[$"F{lastGroupCount}"].Value = $"";
                    ExcelAroundBorder.AroundBorder(ws, $"F{lastGroupCount}");
                    ws.Cells[$"G{lastGroupCount}"].Value = $"{group.ValutSoni}";
                    ExcelAroundBorder.AroundBorder(ws, $"G{lastGroupCount}");
                    ws.Cells[$"H{lastGroupCount}"].Value = $"{group.ValutSummasi}";
                    ExcelAroundBorder.AroundBorder(ws, $"H{lastGroupCount}");
                    ws.Cells[$"I{lastGroupCount}"].Value = $"";
                    ExcelAroundBorder.AroundBorder(ws, $"I{lastGroupCount}");
                    ws.Cells[$"J{lastGroupCount}"].Value = $"";
                    ws.Cells[$"J{lastGroupCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ExcelAroundBorder.AroundBorder(ws, $"J{lastGroupCount}");

                    lastGroupCount++;
                }
                foreach (var group in model.Data)
                {
                    ws.Cells[$"B{lastGroupCount}:F{lastGroupCount}"].Value = $"Жами: {group?.Value[0]?.ValutName.TrimEnd()}";
                    ws.Cells[$"B{lastGroupCount}:F{lastGroupCount}"].Merge = true;
                    ExcelAroundBorder.AroundBorder(ws, $"B{lastGroupCount}", $"F{lastGroupCount}");

                    ws.Cells[$"G{lastGroupCount}"].Value = $"{group.ValutSoni}";
                    ExcelAroundBorder.AroundBorder(ws, $"G{lastGroupCount}");
                    ws.Cells[$"H{lastGroupCount}"].Value = $"{group.ValutSummasi}";
                    ExcelAroundBorder.AroundBorder(ws, $"H{lastGroupCount}");
                    ws.Cells[$"I{lastGroupCount}"].Value = $"";
                    ExcelAroundBorder.AroundBorder(ws, $"I{lastGroupCount}");
                    ws.Cells[$"J{lastGroupCount}"].Value = $"";
                    ws.Cells[$"J{lastGroupCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ExcelAroundBorder.AroundBorder(ws, $"J{lastGroupCount}");
                    lastGroupCount++;
                }

                foreach (var group in model.Data)
                {
                    ws.Cells[$"C{lastGroupCount}:J{lastGroupCount}"].Value = $"Касса мудирига: {group?.Grouped - group.ValutSummasi}";
                    ws.Cells[$"C{lastGroupCount}:J{lastGroupCount}"].Merge = true;
                    ExcelAroundBorder.AroundBorder(ws, $"C{lastGroupCount}", $"J{lastGroupCount}");
                    lastGroupCount++;
                }
                ws.Cells[$"G{lastGroupCount}:J{lastGroupCount}"].Value = $"Кассир: ___________";
                ws.Cells[$"G{lastGroupCount}:J{lastGroupCount}"].Merge = true;
                lastGroupCount++;
                ws.Cells[$"G{lastGroupCount}:J{lastGroupCount}"].Value = $"Назоратчи хисобчи: {model.HeadCashier} ___________";
                ws.Cells[$"G{lastGroupCount}:J{lastGroupCount}"].Merge = true;

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
