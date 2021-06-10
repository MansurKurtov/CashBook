using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.ReportModels
{
    public class Report2ViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Rep2MainPart> MainPart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Rep2BottomPart> BottomPart { get; set; }
    }
    public class Report2ViewModelExemplar
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Rep2MainPartExemplar> MainPart { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Rep2BottomPart> BottomPart { get; set; }
    }
    public class Rep2MainPartExemplar
    {
        /// <summary>
        /// 
        /// </summary>
        public int CashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Position { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<Currency> Currency { get; set; }
    }

    public class Currency
    {
        /// <summary>
        /// 
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string CurrencyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AllSumma { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public List<CashOperByCashierValue> ValuesDate { get; set; }
    }
    public class Rep2MainPart
    {
        /// <summary>
        /// 
        /// </summary>
        public int CashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CashOperByCashierValue> ValuesDate { get; set; }
    }
    public class Rep2BottomPart
    {

        /// <summary>
        /// 
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrencyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AllValues { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CashOperByCashierValueBottom> ValuesDate { get; set; }
    }
}
