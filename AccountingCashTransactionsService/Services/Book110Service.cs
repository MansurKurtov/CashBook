using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AutoMapper;
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
using System.Data;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class Book110Service : EntityRepositoryCore<Journal110>, IBook110Service
    {
        private readonly DataContext _context;
        private CommonHelper _commonHelper;
        IMapper _mapper;
        public Book110Service(IDbContext contexts, IMapper mapper, DataContext context) : base(contexts)
        {
            _context = context;
            _mapper = mapper;
            _commonHelper = new CommonHelper(context);
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
                var result = new Journal109WorthResultQuerys();
                var dates = date.ToString("dd.MM.yyyy");
                string sqltxt = $"select * from table(return_table_110(to_date('{dates}','DD.MM.YYYY'),{bankCode}))";
                var entity = _context.Query<QueryModelFor109Worth>().FromSql(sqltxt).OrderBy(f => f.Id).ToList();
                result.Data = entity;
                result.Total = entity.Count();
                
                //_commonHelper.SaveUserEvent(userId, ModuleType.Journal110, EventType.Read);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public ResponseCoreData GetAll155CashValue(DateTime date, int userId, int operationId)
        {
            try
            {

                var entity = _context.Book155s.Where(f => f.Date == date && f.ToCashierId == userId && f.OperationId == operationId).ToList();
                var result = entity.Sum(f => f.CashValue);
                
                //_commonHelper.SaveUserEvent(userId, ModuleType.Journal110, EventType.Read);

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return ex;
            }
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
        /// <returns></returns>
        public byte[] ToExport(int bankCode, Journal110ExcelModel model, string user)
        {
            var bankName = GetBankName(bankCode);
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                var ws = excel.Workbook.Worksheets.Add("Journal110");

                string[] headerColumn = { "B", "C", "D", "E", "F", "H", "I" };
                string[] rows = { "B", "C", "D", "E", "F", "G", "H", "I" };
                string[] headerColumnMerge = { "B", "C", "D", "E", "G", "H", "I" };
                //string[] columName = { "Символ номи", "Сони", "Сумма" };

                string[] columName = {
                    "Т/р",
                    "Символ коди",
                    "Символ номи",
                    @"Чиқим операцияларини назорат қилувчи масъул ходимнинг Ф.И.О.",
                    "Чиқим ҳужжатлари",
                    "Назорат қилувчи масъул ходимнинг имзоси",
                    "Ҳисоб бериш шарти билан олинган нақд пул ва бошқа қимматликлар суммаси"
                };
                string[] rowSpanHeaderName = { "Сони", "Сумма" };

                string[] headerName = { bankName, "Ҳисоб бериш шарти билан олинган ҳамда чиқим қилинган нақд пул ва бошқа қимматликлар суммаси, шунингдек, чиқим касса ҳужжатларининг сони тўғрисидаги МАЪЛУМОТНОМА ", "КИТОБИ " };

                int[] columWidth = { 5, 10, 20, 15, 25, 10, 15 };
                int[] headerCount = { 2, 4, 6 };
                int[] mergedHeader = { 1, 1, 1, 1, 0, 1, 1 };

                var a = 11;
                int count = 0;
                double summa = 0;

                ws.Cells["I1"].Value = "110-Шакл";
                ws.Cells["I1"].Style.Font.Bold = true;
                ws.Cells["I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                for (int i = 0; i < headerCount.Length; i++)
                {
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Value = $"{headerName[i]}";
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Merge = true;
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Style.WrapText = true;
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"B{headerCount[i]}:I{headerCount[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                ws.Cells["H8"].Value = "Сана:";
                ws.Cells[$"I8"].Value = $"{model.Data.FirstOrDefault().Date:dd.MM.yyyy}";

                for (int i = 0; i < headerColumn.Length; i++)
                {
                    ws.Column(i + 2).Width = columWidth[i];
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Value = $"{columName[i]}";
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Merge = true;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.WrapText = true;
                    //ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.;

                    ws.Cells[$"{headerColumn[i]}{9}:{headerColumnMerge[i]}{9 + mergedHeader[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{headerColumn[i]}{9}", $"{headerColumnMerge[i]}{9 + mergedHeader[i]}");
                }

                ws.Cells["F10"].Value = rowSpanHeaderName[0];
                ws.Cells["F10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["F10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["F10"].Style.Font.Bold = true;
                ws.Cells["G10"].Value = rowSpanHeaderName[1];
                ws.Cells["G10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["G10"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["G10"].Style.Font.Bold = true;
                ExcelAroundBorder.AroundBorder(ws, $"F10");
                ExcelAroundBorder.AroundBorder(ws, $"G10");


                foreach (var item in model.Data)
                {
                    for (int i = 0; i < rows.Length - 1; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{rows[i]}{a}");

                    ws.Cells[$"B{a}"].Value = $"{a - 10}";
                    ws.Cells[$"C{a}"].Value = $"{item.Kod}";
                    ws.Cells[$"D{a}"].Value = $"{item.SymbolName}";
                    ws.Cells[$"D{a}"].Style.WrapText = true;
                    ws.Cells[$"E{a}"].Value = $"{item.Fio}";
                    ws.Cells[$"F{a}"].Value = $"{item.Count:N}";
                    ws.Cells[$"G{a}"].Value = $"{item.Summa:N}";
                    ws.Cells[$"H{a}"].Value = $"";
                    count += item.Count;
                    summa += item.Summa;
                    a++;
                }

                ws.Cells[$"I11:I{model.Data.Count + 10}"].Merge = true;
                ws.Cells[$"I11:I{model.Data.Count + 10}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[$"I11:I{model.Data.Count + 10}"].Value = $"{model.CashValue}";
                ExcelAroundBorder.AroundBorder(ws, $"I11", $"I{model.Data.Count + 10}");

                ws.Cells[$"B{model.Data.Count + 11}:E{model.Data.Count + 11}"].Value = "Жами:";
                ws.Cells[$"B{model.Data.Count + 11}:E{model.Data.Count + 11}"].Merge = true;
                ws.Cells[$"B{model.Data.Count + 11}:E{model.Data.Count + 11}"].Style.Font.Bold = true;
                ws.Cells[$"B{model.Data.Count + 11}:E{model.Data.Count + 11}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[$"F{model.Data.Count + 11}"].Value = $"{count}";
                ws.Cells[$"G{model.Data.Count + 11}"].Value = $"{summa}";
                ws.Cells[$"H{model.Data.Count + 11}"].Value = $"";
                ws.Cells[$"I{model.Data.Count + 11}"].Value = $"{model.CashValue}";
                ws.Cells[$"I{model.Data.Count + 11}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                ExcelAroundBorder.AroundBorder(ws, $"B{model.Data.Count + 11}", $"I{model.Data.Count + 11}");

                ws.Cells[$"F{a + 2}:I{a + 2}"].Value = $"Касса мудирига: {0:N}";
                ws.Cells[$"F{a + 2}:I{a + 2}"].Merge = true;
                ws.Cells[$"F{a + 2}:I{a + 2}"].Style.Font.Bold = true;
                ws.Cells[$"F{a + 2}:I{a + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[$"F{a + 3}:I{a + 3}"].Value = $"Кассир:{user} ________";
                ws.Cells[$"F{a + 3}:I{a + 3}"].Merge = true;
                ws.Cells[$"F{a + 3}:I{a + 3}"].Style.Font.Bold = true;
                ws.Cells[$"F{a + 3}:I{a + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[$"F{a + 4}:I{a + 4}"].Value = $"Назоратчи хисобчи: {model.HeadCashierName} ________";
                ws.Cells[$"F{a + 4}:I{a + 4}"].Merge = true;
                ws.Cells[$"F{a + 4}:I{a + 4}"].Style.Font.Bold = true;
                ws.Cells[$"F{a + 4}:I{a + 4}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["B2:I2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B4:I4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["B6:I6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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
