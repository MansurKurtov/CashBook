using System;

namespace Entitys.ViewModels.CashOperation.Book110
{
    public class Book110ViewModel
    {
       
        /// <summary>
        /// Символ номи
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Сони
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int BankCode { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int SymbolCode { get; set; }
    }
}
