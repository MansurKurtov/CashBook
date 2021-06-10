using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.ReportModels
{
    public class Rep2CurrencySumms
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Summa { get; set; }
    }
}
