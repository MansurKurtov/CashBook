using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Journal15
{
    public class Book15PostViewModel
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Йўналиш рақами
        /// </summary>
        public int Journal16Id { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// Изоҳ
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Қоп рақами/номи
        /// </summary>
        public string BagsNumber { get; set; }
    }
}
