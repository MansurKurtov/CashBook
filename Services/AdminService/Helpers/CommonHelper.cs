using Entitys.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace AdminService.Helpers
{
    public class CommonHelper
    {
        private DataContext _context;
        public CommonHelper(DataContext context)
        {
            _context = context;
        }
        public bool isDayClosed(int moduleId, int bankCode, DateTime date)
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
                        dayClose = Convert.ToBoolean((int)cmd.ExecuteScalar());
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
                return false;
            }
        }
    }
}
