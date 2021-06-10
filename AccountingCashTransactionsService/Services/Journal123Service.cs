using AccountingCashTransactionsService.Helper;
using AccountingCashTransactionsService.Interfaces;
using AutoMapper;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using Entitys.DB;
using Entitys.Enums;
using Entitys.Helper.ExcelAround;
using Entitys.Models;
using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Journal123;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AccountingCashTransactionsService.Services
{
    public class Journal123Service : EntityRepositoryCore<Journal123>, IJournal123Service
    {
        private readonly DataContext _context;
        private CommonHelper _commonHelper;
        IMapper _mapper;
        public Journal123Service(IDbContext contexts, IMapper mapper, DataContext context) : base(contexts)
        {
            _context = context;
            _mapper = mapper;
            _commonHelper = new CommonHelper(context);
        }

        #region J123
        public ResponseCoreData AddDate(int bankCode, int userId, Journal123PostModel model)
        {
            try
            {
                var entity = new Journal123();
                entity.Date = GetWorkingDate(bankCode);                
                entity.UserId = userId;
                entity.SystemDate = DateTime.Now;
                var item = _context.Journal123s.Where(f => f.Date.Date == entity.Date).ToList().FirstOrDefault();
                if (item == null)
                    Add(entity);
                else
                {
                    entity.Id = item.Id;
                    Update(entity);
                }

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData("Bu kun uchun sana mavjud!");
            }
        }
        public ResponseCoreData UpdateDate(int bankCode, int userId, Journal123UpdateModel model)
        {
            try
            {
                var entity = _context.Journal123s.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);
                entity.Date = GetWorkingDate(bankCode).Date;
                Update(entity);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData GetDateById(int id)
        {
            try
            {
                var entity = _context.Journal123s.Where(f => f.Id == id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData GetDateByDate(DateTime date)
        {
            try
            {
                var entity = _context.Journal123s.Where(f => f.Date.Date == date.Date).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData DeleteDateById(int bankCode, int userId, int id)
        {
            try
            {
                var entity1 = _context.Journal123s.Where(f => f.Id == id).ToList().FirstOrDefault();
                if (entity1 == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);
                Delete(entity1);

                var entity2 = _context.Journal123Contents.Where(w => w.Journal123Id == id).ToList();
                _context.Journal123Contents.RemoveRange(entity2);

                var entity3 = _context.Journal123Fios.Where(w => w.Journal123Id == id).ToList();
                _context.Journal123Fios.RemoveRange(entity3);

                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        #endregion

        #region J123Content
        public ResponseCoreData AddContent(int bankCode, int userId, Journal123ContentPostModel model)
        {
            try
            {
                var entity = new Journal123Content();
                entity.BankCode = bankCode;
                entity.UserId = userId;
                entity.SystemDate = DateTime.Now;
                entity.Name = model.Name;
                entity.Count = model.Count;
                entity.Value = model.Value;
                entity.Summa = model.Summa;
                entity.Target = model.Target;
                entity.Journal123Id = model.Journal123Id;
                entity.Fio = model.Fio;

                _context.Journal123Contents.Add(entity);
                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData UpdateContent(int bankCode, int userId, Journal123ContentUpdateModel model)
        {
            try
            {
                var entity = _context.Journal123Contents.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);
                entity.Name = model.Name;
                entity.Count = model.Count;
                entity.Value = model.Value;
                entity.Summa = model.Summa;
                entity.Target = model.Target;
                entity.Journal123Id = model.Journal123Id;
                entity.Fio = model.Fio;
                _context.Journal123Contents.Update(entity);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData GetContentById(int id)
        {
            try
            {
                var entity = _context.Journal123Contents.Where(f => f.Id == id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData GetContentByDate(DateTime date)
        {
            try
            {
                var item = _context.Journal123s.Where(f => f.Date.Date == date.Date).ToList().FirstOrDefault();
                if (item == null)
                    return new ResponseCoreData("Не найдено данных Journal123 на эту дату", ResponseStatusCode.BadRequest);

                var result = _context.Journal123Contents.Where(f => f.Journal123Id == item.Id).ToList();
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData GetContentsByJournal123Id(int journal123Id)
        {
            try
            {
                var entityies = _context.Journal123Contents.Where(f => f.Journal123Id == journal123Id).ToList().FirstOrDefault();

                return new ResponseCoreData(entityies, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData DeleteContentById(int bankCode, int userId, int id)
        {
            try
            {
                var entity = _context.Journal123Contents.Where(f => f.Id == id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);
                _context.Journal123Contents.Remove(entity);
                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Delete);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        #endregion

        #region J123Fio
        public ResponseCoreData AddFio(int bankCode, int userId, Journal123FioPostModel model)
        {
            try
            {
                var entity = new Journal123Fio();
                entity.UserId = userId;
                entity.Journal123Id = model.Journal123Id;
                entity.Fio = model.Fio;
                _context.Journal123Fios.Add(entity);
                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData UpdateFio(int bankCode, int userId, Journal123FioUpdateModel model)
        {
            try
            {
                var item = _context.Journal123Fios.Where(f => f.Id == model.Id).ToList().FirstOrDefault();
                if (item == null)
                    return new ResponseCoreData("Не найдено!", ResponseStatusCode.BadRequest);

                item.Fio = model.Fio;
                _context.Journal123Fios.Update(item);

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Edit);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData GetFioById(int id)
        {
            try
            {
                var entity = _context.Journal123Fios.Where(f => f.Id == id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);

                return new ResponseCoreData(entity, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData GetFioByDate(DateTime date)
        {
            try
            {
                var item = _context.Journal123s.Where(f => f.Date.Date == date.Date).ToList().FirstOrDefault();
                if (item == null)
                    return new ResponseCoreData("Не найдено данных Journal123 на эту дату", ResponseStatusCode.BadRequest);

                var result = _context.Journal123Fios.Where(f => f.Journal123Id == item.Id).ToList();
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData GetFiosByJournal123Id(int journal123Id)
        {
            try
            {
                var result = _context.Journal123Fios.Where(w => w.Journal123Id == journal123Id).ToList();

                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData DeleteFioById(int bankCode, int userId, int id)
        {
            try
            {
                var entity = _context.Journal123Fios.Where(f => f.Id == id).ToList().FirstOrDefault();
                if (entity == null)
                    return new ResponseCoreData("Не найдено", ResponseStatusCode.BadRequest);
                _context.Journal123Fios.Remove(entity);
                _context.SaveChanges();

                _commonHelper.SaveUserEvent(bankCode, userId, ModuleType.Journal123, EventType.Delete);

                return new ResponseCoreData(ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        private DateTime GetWorkingDate(int bankCode)
        {
            try
            {
                string sqltxt = $"select GET_WORKING_DATE({bankCode}) as result from dual";
                var entity = _context.Query<GetWorkingDates>().FromSql(sqltxt).ToList().FirstOrDefault();
                var result = entity.RESULT ?? DateTime.Today;

                return result.Date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] ToExport(List<ExcelFor123Froms> model, string user)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var excel = new ExcelPackage();
                double summa = 0;
                string[] head = { "____________________________________________________________________________________________________________________________", "(банк / филиал номи)", "ПУЛ ОМБОРИДАН НАЗОРАТДА ҚАЙТА САНАШ УЧУН ОЛИБ", "ЧИҚИЛГАН ҲАМДА ПУЛ ОМБОРИГА ҚАЙТАРИЛГАН", "НАҚД ПУЛ ВА БОШҚА ҚИММАТЛИКЛАРНИ ҚАЙД ЭТИШ", "КИТОБИ" };
                string[] headcoll = { "A4", "A5", "A8", "A9", "A10", "A12", };
                string[] headrow = { "J4", "J5", "J8", "J9", "J10", "J12", };
                var ws = excel.Workbook.Worksheets.Add("Journal123");
                string[] columns = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
                for (int i = 0; i < head.Length; i++)
                {
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Value = $"{head[i]}";
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Merge = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.WrapText = true;
                    ws.Cells[$"{headcoll[i]}:{headrow[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                int a = 17;
                string[] columnLetter1 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "I", "I", "B", "G" };
                string[] columnLetter2 = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "J", "J", "H", "H" };
                int[] columnId1 = { a, a + 1, a + 1, a + 1, a + 1, a + 1, a + 3, a + 3, a + 3, a + 3, a + 2, a, a, a + 1 };
                int[] columnId2 = { a + 3, a + 3, a + 3, a + 3, a + 3, a + 3, a + 3, a + 3, a + 3, a + 3, a + 2, a + 1, a, a + 2 };
                int[] columnWith = { 10, 20, 10, 15, 30, 25, 20, 10, 20, 10, 10, 10, 10, 10 };

                string[] c = { "Сана", "номи", "сони", "қиймати", "суммаси (рақам ва сўз билан)", "олиб чиқиш мақсади", "Ф.И.О.", "имзоси", "Ф.И.О.", "имзоси", "Уч моддий жавобгар шахснинг", "Нақд пул ва бошқа қимматликлар пул омборига қайтариб қўйилди", "Пул омборидан олиб чиқилаётган нақд пул ва бошқа қимматликлар", "қабул қилиб олган назоратчи-кассир" };

                for (int i = 0; i < columnLetter1.Length; i++)
                {                    
                    ws.Column(i + 1).Width = columnWith[i];
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Value = $"{c[i]}";
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Merge = true;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.WrapText = true;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.Font.Bold = true;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[$"{columnLetter1[i]}{columnId1[i]}:{columnLetter2[i]}{columnId2[i]}"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ExcelAroundBorder.AroundBorder(ws, $"{columnLetter1[i]}{columnId1[i]}", $"{columnLetter2[i]}{columnId2[i]}");
                }
                a += 4;
                foreach (var item in model)
                {
                    for (int i = 0; i < columns.Length; i++)
                        ExcelAroundBorder.AroundBorder(ws, $"{columns[i]}{a}");

                    ws.Cells[$"A{a}"].Value = $"{item.Date:dd.MM.yyyy}";
                    ws.Cells[$"A{a}"].Style.WrapText = true;
                    ws.Cells[$"A{a}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[$"B{a}"].Value = $"{item.Name}";
                    ws.Cells[$"B{a}"].Style.WrapText = true;
                    ws.Cells[$"C{a}"].Value = $"{item.Count}";
                    ws.Cells[$"C{a}"].Style.WrapText = true;
                    ws.Cells[$"D{a}"].Value = $"{item.Value}";
                    ws.Cells[$"D{a}"].Style.WrapText = true;
                    ws.Cells[$"E{a}"].Value = $"{item.Summa}";
                    ws.Cells[$"E{a}"].Style.WrapText = true;
                    ws.Cells[$"F{a}"].Value = $"{item.Target}";
                    ws.Cells[$"F{a}"].Style.WrapText = true;
                    ws.Cells[$"G{a}"].Value = $"{item.Fio}";
                    ws.Cells[$"G{a}"].Style.WrapText = true;
                    ws.Cells[$"H{a}"].Value = $" ";
                    ws.Cells[$"H{a}"].Style.WrapText = true;
                    ws.Cells[$"I{a}"].Value = $"{item.Journal123Fio}";
                    ws.Cells[$"I{a}"].Style.WrapText = true;
                    ws.Cells[$"J{a}"].Value = $" ";
                    ws.Cells[$"J{a}"].Style.WrapText = true;
                    a++;
                }
                var stream = new MemoryStream();
                excel.SaveAs(stream);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
