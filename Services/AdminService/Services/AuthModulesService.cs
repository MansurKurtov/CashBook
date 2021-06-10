using AdminService.Interfaces;
using AvastInfrastructureRepository.Repositories.Services;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using EntityRepository.Context;
using EntityRepository.Repository;
using Entitys.DB;
using Entitys.Models.Auth;
using Entitys.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AdminService.Services
{
    public class AuthModulesService : EntityRepositoryCore<AuthModules>, IAuthModulesService
    {
        private readonly DataContext _context;

        public AuthModulesService(IDbContext context, DataContext dBcontext) : base(context)
        {
            _context = dBcontext;
        }
        public ResponseCoreData IsDayClosed(DateTime date, int bankCode)
        {
            try
            {
                var dateString = date.ToString("dd.MM.yyyy");
                int dayCloseStatus = 0;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    bool wasOpen = cmd.Connection.State == ConnectionState.Open;
                    if (!wasOpen) cmd.Connection.Open();
                    try
                    {
                        cmd.CommandText = $"select  IS_CLOSE_DAY_ALL(to_date('{dateString}', 'DD.MM.YYYY'), {bankCode})  as result from dual";
                        dayCloseStatus = Convert.ToInt32((decimal)cmd.ExecuteScalar());
                    }
                    finally
                    {
                        if (!wasOpen) cmd.Connection.Close();
                    }
                }
                var result = new IsDayCloseViewModel();
                result.DayCloseStatus = dayCloseStatus;
                return new ResponseCoreData(result, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }

        public ResponseCoreData CloseDay(int bankCode, int userId, DateTime date,DateTime nextDate)
        {
            if (date >= nextDate)
                return new ResponseCoreData(ResponseStatusCode.BadRequest);

            try
            {
                CloseDayProcedure(bankCode, userId, date,nextDate);
                return new ResponseCoreData(true, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        private int SetClearCloseDayAlls(int userId, DateTime date, int bankCode)
        {
            int insertedRowsCount;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_CLEAR_CLOSE_DAY_ALL";

                var userIdParam = new OracleParameter("USERID", OracleDbType.Int32, ParameterDirection.Input);
                userIdParam.Value = userId;
                cmd.Parameters.Add(userIdParam);

                var dateParam = new OracleParameter("KUN", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

                var rowsCountParam = new OracleParameter("INSERTED_ROWS", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(rowsCountParam);

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                DataTable budt = new DataTable();
                while (reader.Read())
                {
                    budt.Load(reader);
                }
                insertedRowsCount = int.Parse(rowsCountParam.Value.ToString());
                cmd.Connection.Close();
            }

            return insertedRowsCount;
        }

        public ResponseCoreData SetClearCloseDayAll(int userId, DateTime date, int bankCode)
        {
            try
            {
                SetClearCloseDayAlls(userId, date, bankCode);
                return new ResponseCoreData(true, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }
        public ResponseCoreData OpenDay(int bankCode, int userId, DateTime date)
        {
            try
            {
                OpenDayProcedure(bankCode, userId, date);
                return new ResponseCoreData(true, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new ResponseCoreData(ex);
            }
        }


        private int CloseDayProcedure(int bankCode, int userId, DateTime date, DateTime nextDate)
        {
            int insertedRowsCount;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_CLOSE_DAY_ALL";

                var userIdParam = new OracleParameter("USERID", OracleDbType.Int32, ParameterDirection.Input);
                userIdParam.Value = userId;
                cmd.Parameters.Add(userIdParam);

                var dateParam = new OracleParameter("KUN", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

                var next_Date = new OracleParameter("NEXT_DATE", OracleDbType.Date, ParameterDirection.Input);
                next_Date.Value = nextDate;
                cmd.Parameters.Add(next_Date);

                var rowsCountParam = new OracleParameter("INSERTED_ROWS", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(rowsCountParam);

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                DataTable budt = new DataTable();
                while (reader.Read())
                {
                    budt.Load(reader);
                }
                insertedRowsCount = int.Parse(rowsCountParam.Value.ToString());
                cmd.Connection.Close();
            }

            return insertedRowsCount;
        }

        private int OpenDayProcedure(int bankCode, int userId, DateTime date)
        {
            int insertedRowsCount;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SET_CLEAR_CLOSE_DAY_ALL";

                var userIdParam = new OracleParameter("USERID", OracleDbType.Int32, ParameterDirection.Input);
                userIdParam.Value = userId;
                cmd.Parameters.Add(userIdParam);

                var dateParam = new OracleParameter("KUN", OracleDbType.Date, ParameterDirection.Input);
                dateParam.Value = date;
                cmd.Parameters.Add(dateParam);

                var bankCodeParam = new OracleParameter("BANKKOD", OracleDbType.Int32, ParameterDirection.Input);
                bankCodeParam.Value = bankCode;
                cmd.Parameters.Add(bankCodeParam);

                var rowsCountParam = new OracleParameter("INSERTED_ROWS", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add(rowsCountParam);

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                DataTable budt = new DataTable();
                while (reader.Read())
                {
                    budt.Load(reader);
                }
                insertedRowsCount = int.Parse(rowsCountParam.Value.ToString());
                cmd.Connection.Close();
            }

            return insertedRowsCount;
        }

    }
}
