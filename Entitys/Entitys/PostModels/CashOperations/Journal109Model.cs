using System;

namespace Entitys.PostModels.CashOperations
{
    /// <summary>
    /// 
    /// </summary>
    public class Journal109Model
    {
        public int Id { get; set; }
        public string SymbolName { get; set; }
        public int Count { get; set; }
        public double Summa { get; set; }
        public DateTime Date{get;set;}
        public int BankCode { get; set; }
        public int SymbolCode { get; set; }
    }
}
