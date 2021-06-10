using Entitys.DB;
using Entitys.Enums;
using Entitys.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace AccountingCashTransactionsService.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonHelper
    {
        private DataContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public CommonHelper(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="bankCode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsDayClosed(int moduleId, int bankCode, DateTime date)
        {
            try
            {
                var dateString = date.ToString("dd.MM.yyyy");
                bool dayClose = false;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                    if (!wasOpen) cmd.Connection.Open();
                    try
                    {
                        cmd.CommandText = $"select  IS_CLOSE_DAY({moduleId}, to_date('{dateString}', 'DD.MM.YYYY'), {bankCode})  as result from dual";
                        string dayCloses = Convert.ToString(cmd.ExecuteScalar());
                        if (dayCloses == "1") dayClose = true;

                    }
                    finally
                    {
                        if (!wasOpen) cmd.Connection.Close();
                    }
                }
                return dayClose;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveUserEvent(int bankCode, int userId, ModuleType moduleType, EventType eventType)
        {
            try
            {
                var userEvent = new EventHistory();
                userEvent.SystemDate = DateTime.Now;
                userEvent.UserId = userId;
                userEvent.ModuleId = moduleType;
                userEvent.UserEventType = eventType;
                userEvent.BankKod = bankCode;

                _context.EventHistories.Add(userEvent);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Log()
        {

        }
    }
}
