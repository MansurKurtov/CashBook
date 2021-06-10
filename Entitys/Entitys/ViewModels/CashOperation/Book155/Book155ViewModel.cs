using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Book155
{
    public class Book155ViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int FromCashierId { get; set; }
        public string FromCashierName { get; set; }

        public int ToCashierId { get; set; }
        public string ToCashierName { get; set; }

        public double CashValue { get; set; }
        public string CashValueText { get; set; }

        public bool Accept { get; set; }

        public int OperationId { get; set; }

        public int? SprObjectId { get; set; }
        public string SprObjectName { get; set; }

        public string WorthAccount { get; set; }
        public string WorthAccountName { get; set; }

        public int? CounterCashierId { get; set; }
        public bool From175 { get; set; }
        public int? OtherInoutId { get; set; }

    }

    public class Book155ViewModels
    {
        public DateTime Sana { get; set; }
        public int UserId { get; set; }
        public int FromCashierId { get; set; }
        public DateTime Date { get; set; }

        public string FromCashierName { get; set; }
        public string CurrencyName { get; set; }
        public string WorthAccountName { get; set; }

        public string ToCashierName { get; set; }

        public double CashValue { get; set; }
        public string BoshKassir { get; set; }
        public int OperationId { get; set; }
    }
}
