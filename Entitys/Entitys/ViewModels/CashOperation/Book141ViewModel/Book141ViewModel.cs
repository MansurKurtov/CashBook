using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Book141ViewModel
{
    public class Book141ViewModel
    {

        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Кун бошидаги сальдо
        /// </summary>
        public double SaldoBegin { get; set; }

        /// <summary>
        /// Кирим
        /// </summary>
        public double InCome { get; set; }

        /// <summary>
        /// Чиқим
        /// </summary>
        public double OutGo { get; set; }

        /// <summary>
        /// Кун охиридаги сальдо
        /// </summary>
        public double SaldoEnd { get; set; }
    }
}
