using System;

namespace Entitys.ViewModels.CashOperation.Book155
{
    public class GetBook155AllViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int FromCashierId { get; set; }
        public int ToCashierId { get; set; }
        public double CashValue { get; set; }
        public bool Accept { get; set; }
        public int OperationId { get; set; }
        public int? SprObjectId { get; set; }
        public string WorhAccount { get; set; }
        public DateTime? SystemDate { get; set; }
        public string FromCashierName { get; set; }
        public string ToCashierName { get; set; }
        public string CurrencyName { get; set; }
        public string WorthAccountName { get; set; }
        public string CounterCashierName { get; set; }
        public bool From175 { get; set; }
        public int? OtherInoutId { get; set; }
    }
}
