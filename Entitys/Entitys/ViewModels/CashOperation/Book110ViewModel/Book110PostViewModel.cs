using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Book110
{
    public class Book110PostViewModel
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
    }
}
