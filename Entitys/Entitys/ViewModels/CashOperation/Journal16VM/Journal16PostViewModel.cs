using System;

namespace Entitys.ViewModels.CashOperation.Journal16VM
{
    /// <summary>
    /// 
    /// </summary>
    public class Journal16PostViewModel
    {

        /// <summary>
        ///Йўналиш рақами 
        /// </summary>
        public string DirectionNumber { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }

        ///// <summary>
        ///// Инкассация ФИО лари
        ///// </summary>
        //public string CollectorName { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        public int UserId { get; set; }

        ///// <summary>
        ///// Қоплар сони
        ///// </summary>
        //public int BagsCount { get; set; }

        ///// <summary>
        ///// Сумма
        ///// </summary>
        //public double Summa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SystemDate { get; set; }
    }
}
