using System;

namespace Entitys.ViewModels.CashOperation.Journal15
{
    /// <summary>
    /// 
    /// </summary>
    public class Journal15ViewModel
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SystemDate { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public int SprObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrencyName { get; set; }
    }
}
