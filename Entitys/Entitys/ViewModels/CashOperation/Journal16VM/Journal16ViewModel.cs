using Entitys.ViewModels.CashOperation.Collector;
using Entitys.ViewModels.CashOperation.Journal15;
using System;
using System.Collections.Generic;

namespace Entitys.ViewModels.CashOperation.Journal16VM
{
    /// <summary>
    /// 
    /// </summary>
    public class Journal16ViewModel
    {
        public int Id { get; set; }
        public string DirectionNumber { get; set; }
        public DateTime Date { get; set; }
        public bool Accept { get; set; }
        public DateTime? AcceptDate { get; set; }
        public int UserId { get; set; }
        public DateTime SystemDate { get; set; }
        public List<Journal15ViewModel> Journal15List { get; set; }
        public List<Journal15ViewModel> Journal15EmptyBags { get; set; }
        public List<Journal15CurrencyViewModel> CurrencyInfos { get; set; }
        public List<CollectorViewModel> CollectorFioList { get; set; }
        public int BagsCount { get; set; }
        public int EmptyBags { get; set; }
        public double Summa { get; set; }
        public int Status { get; set; }
    }
}
