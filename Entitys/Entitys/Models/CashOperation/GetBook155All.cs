using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    public class GetBook155All
    {
        public int ID { get; set; }
        public DateTime SANA { get; set; }
        public int FROM_CASHIER_ID { get; set; }
        public int TO_CASHIER_ID { get; set; }
        public double CASH_VALUE { get; set; }
        public bool ACCEPT { get; set; }
        public int OPERATION_ID { get; set; }
        public int? SPR_OBJECT_ID { get; set; }
        public string WORTH_ACCOUNT { get; set; }
        public DateTime? SYSTEM_DATE { get; set; }
        public string FROM_CASHIER_NAME { get; set; }
        public string TO_CASHIER_NAME { get; set; }
        public string VALUT_NAME { get; set; }
        public string WORTH_ACCOUNT_NAME { get; set; }
        public string COUNTER_CASHIER_NAME { get; set; }
        public int FROM_175 { get; set; }        
    }
}
