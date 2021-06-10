using System;

namespace Entitys.ViewModels.CashOperation.ReportModels
{
    /// <summary>
    /// 
    /// </summary>
    public class DetailedCurrencyReportInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Mfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Posision { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double CurrencySum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
