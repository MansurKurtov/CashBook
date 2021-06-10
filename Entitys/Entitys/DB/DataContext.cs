using EntityRepository.Context;
using Entitys.Models;
using Entitys.Models.Auth;
using Entitys.Models.CashOperation;
using Entitys.Models.Enums;
using Entitys.Models.Reference;
using Entitys.QueryModel;
using Entitys.ViewModels.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.DB
{
    public class DataContext : DbContext, IDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(dbConfig.ConnectionString, k => k.UseOracleSQLCompatibility("11"));
            //optionsBuilder.UseOracle(dbConfig.ConnectionString, k => k.UseOracleSQLCompatibility("11"));
            //base.OnConfiguring(optionsBuilder);
        }
        */
        /// <summary>
        /// bazaga malumoti bilan migratsiya qilish
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OtherInout>().HasData(
                new OtherInout { Id = 1, Name = "Омбордан", InOutId = 1, IsMainCashier = true },
                new OtherInout { Id = 2, Name = "Омборга", InOutId = 2, IsMainCashier = true },
                new OtherInout { Id = 3, Name = "Жунатма", InOutId = 2, IsMainCashier = true },
                new OtherInout { Id = 4, Name = "Мадад", InOutId = 1, IsMainCashier = true },
                new OtherInout { Id = 5, Name = "Кирим", InOutId = 1, IsMainCashier = false },
                new OtherInout { Id = 6, Name = "Чиким", InOutId = 2, IsMainCashier = false }
                );

            modelBuilder.Entity<EntOperations155>().HasData(
                new EntOperations155 { Id = 1, Name = "Миллий валюта" },
                new EntOperations155 { Id = 2, Name = "Халқаро валюта" },
                new EntOperations155 { Id = 3, Name = "Қимматли" }
                );

            modelBuilder.Entity<InOut>().HasData(
                new InOut { Id = 1, Name = "Кирим" },
                new InOut { Id = 2, Name = "Чиқим" }
                );

            modelBuilder.Entity<ModifiedBookSaldos>().HasData(
                new ModifiedBookSaldos { Id = 1, Name = "120 - китоб" },
                new ModifiedBookSaldos { Id = 2, Name = "121- китоб" },
                new ModifiedBookSaldos { Id = 3, Name = "171- китоб" },
                new ModifiedBookSaldos { Id = 4, Name = "141- китоб" },
                new ModifiedBookSaldos { Id = 5, Name = "141А- китоб" },
                new ModifiedBookSaldos { Id = 6, Name = "111 - китоб" }
                );


            modelBuilder.Entity<CashierType>().HasData(
                new CashierType { Id = 1, Name = "Бош кассир" },
                new CashierType { Id = 2, Name = "Кирим кассир" },
                new CashierType { Id = 3, Name = "Чиқим кассир" },
                new CashierType { Id = 4, Name = "Кечки кассир" },
                new CashierType { Id = 5, Name = "Қайта санаш кассири" }
                );

            modelBuilder.Query<GetValType>();
            modelBuilder.Query<GetWorthAll>();
            modelBuilder.Query<GetWorthName>();
            modelBuilder.Query<GetCurrencyName>();
            modelBuilder.Query<GetBankAll>();
            modelBuilder.Query<GetBook155All>();
            modelBuilder.Query<GetBranch>();
            //modelBuilder.Query<FirstSaldoProcedureResult>();
        }
        DbContext IDbContext.DataContext { get { return this; } }

        #region query

        #endregion

        #region Auth
        public DbSet<AuthModules> AuthModules { get; set; }
        public DbSet<AuthPermissions> AuthPermissions { get; set; }
        public DbSet<AuthRolePermissions> AuthRolePermissions { get; set; }
        public DbSet<AuthRoles> AuthRoles { get; set; }
        public DbSet<AuthStructurePermission> AuthStructurePermission { get; set; }
        public DbSet<AuthUIElements> AuthUIElements { get; set; }
        public DbSet<AuthUserPermissions> AuthUserPermissions { get; set; }
        public DbSet<AuthUserRole> AuthUserRole { get; set; }
        public DbSet<AuthUserRTokens> AuthUserRTokens { get; set; }
        public DbSet<AuthUsers> AuthUsers { get; set; }

        public DbSet<RefCountry> RefCountry { get; set; }
        public DbSet<RefDistrict> RefDistrict { get; set; }
        public DbSet<RefRegion> RefRegion { get; set; }
        public DbSet<RefStructure> RefStructure { get; set; }
        public DbSet<EventHistory> EventHistories { get; set; }

        #endregion

        #region Book
        public DbSet<Book155> Book155s { get; set; }
        public DbSet<Book175> Book175s { get; set; }
        public DbSet<Journal15> Journal15s { get; set; }
        public DbSet<Journal16> Journal16s { get; set; }
        public DbSet<Journal109> Journal109s { get; set; }
        public DbSet<Journal111> Journal111s { get; set; }
        public DbSet<Journal110> Book110s { get; set; }
        public DbSet<Journal109Worth> Journal109Worths { get; set; }
        public DbSet<Journal110Worth> Journal110Worths { get; set; }
        public DbSet<Journal109Val> Journal109Val { get; set; }
        public DbSet<Journal110Val> Journal110Val { get; set; }
        public DbSet<Journal123> Journal123s { get; set; }
        public DbSet<Journal123Content> Journal123Contents { get; set; }
        public DbSet<Journal123Fio> Journal123Fios { get; set; }
        public DbSet<Book120> Book120s { get; set; }
        public DbSet<Book121> Book121s { get; set; }
        public DbSet<Book141> Book141Table { get; set; }
        public DbSet<Book141A> Book141ATable { get; set; }
        public DbSet<Book171> Book171s { get; set; }
        public DbSet<Journal176> Journal176s { get; set; }
        public DbSet<CounterCashier> CounterCashiers { get; set; }

        public DbSet<Collector> Collectors { get; set; }
        public DbSet<OtherInout> OutherInouts { get; set; }
        #endregion
        #region Enums
        public DbSet<EntOperations155> EntOperations155 { get; set; }
        public DbSet<InOut> InOut { get; set; }
        public DbSet<ModifiedBookSaldos> ModifiedBookSaldos { get; set; }
        public DbSet<CashierType> CashierTypes { get; set; }
        #endregion
        public DbQuery<PermissionQueryModel> permissionQueryModels { get; set; }
        public DbQuery<Models.QueryModel> JournalGetByDate { get; set; }
        public DbQuery<Models.QueryModelFor109Worth> QueryModelFor109Worth { get; set; }

        public DbQuery<Journal109ValQueryModel> Journal109ValQueryModels { get; set; }
        public DbQuery<Journal110ValQueryModel> Journal110ValQueryModels { get; set; }

        public DbQuery<QueryModels> QueryModels { get; set; }
        public DbQuery<Book121QueryModel> Book121QueryModel { get; set; }
        public DbQuery<Book171QueryModel> Book171QueryModel { get; set; }
        public DbSet<ChiefaccountantTable> ChiefAccountantTables { get; set; }
        public DbSet<SupervisingAccountant> SupervisingAccountants { get; set; }

        public DbQuery<FirstSaldoProcedureResult> FirstSaldoProcedureResults { get; set; }
        public DbQuery<ReturnTable110Model> ReturnTable110Models { get; set; }
        public DbQuery<GetWorkingDates> GetWorkingDates { get; set; }
        public DbQuery<GetWorthCashValue> GetWorthCashValue { get; set; }
        public DbQuery<GetValCashValue> GetValCashValue { get; set; }

        public DbQuery<Reports> CashOperationsByCashier { get; set; }
        public DbQuery<GetReport1Cashiers> GetReport1Cashiers { get; set; }
        public DbQuery<GetReport2CurrencyCashiers> GetReport2CurrencyCashiers { get; set; }
        public DbQuery<GetReport2CurrencySumms> GetReport2CurrencySumms { get; set; }
        public DbQuery<GetReport1Summs> GetReport1Summs { get; set; }
        public DbQuery<GetReport2Currency> GetReport2Currency { get; set; }



        [DbFunction("GET_CHIEFACCOUNTANT_NAME")]
        public static string GetCheifAccountant(int bankId)
        {
            throw new NotImplementedException();
        }

    }
}
