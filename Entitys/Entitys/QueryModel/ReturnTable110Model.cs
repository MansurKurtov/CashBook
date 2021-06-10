using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.QueryModel
{
    public class ReturnTable110Model
    {
        public int kod { get; set; }
        public string name { get; set; }
        public string FIO { get; set; }
        public int soni { get; set; }
        public double summa { get; set; }
    }
    public class GetWorthCashValue
    {
        [Column("WORTH_ACCOUNT")]
        public string WorthAccount { get; set; }
        [Column("SUM(CASH_VALUE)")]
        public double CashValue { get; set; }
    }
    public class GetValCashValue
    {
        [Column("SPR_OBJECT_ID")]
        public int SprObjectId { get; set; }
        [Column("SUM(CASH_VALUE)")]
        public double CashValue { get; set; }
    }
}
