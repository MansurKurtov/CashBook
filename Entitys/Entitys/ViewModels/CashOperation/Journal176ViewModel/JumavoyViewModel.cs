using System;
using System.Collections.Generic;

namespace Entitys.ViewModels.CashOperation.Journal176ViewModel
{
    public class JumavoyViewModel
    {
        public int SprObjectId { get; set; }
        public string CurrencyName { get; set; }
        public double TotalSumma { get; set; }
        public double TotalLackSumma { get; set; }
        public double TotalWorthlessSumma { get; set; }
        public double TotalExcessSumma { get; set; }
        public double TotalFakeSumma { get; set; }
        public DateTime? AcceptDate { get; set; }
    }


}
