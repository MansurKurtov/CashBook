using Entitys.Models.CashOperation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Journal176ViewModel
{
    public class Journal176ViewModels
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Қоп рақами
        /// </summary>
        public string BagNumber { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public double Summa { get; set; }

        /// <summary>
        /// Камомад сумма
        /// </summary>
        public double LackSumma { get; set; }

        /// <summary>
        /// Яроқсиз сумма
        /// </summary>
        public double WorthlessSumma { get; set; }

        /// <summary>
        /// Чеклар сони
        /// </summary>
        public int ReceiptCount { get; set; }

        /// <summary>
        /// Чеклар суммаси
        /// </summary>
        public double ReceiptSumma { get; set; }

        /// <summary>
        /// Изоҳ
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Фойдаланувчи коди
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Сановчи кассир
        /// </summary>
        public int? CounterCashierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string  CounterCashierName { get; set; }

        /// <summary>
        /// Ортиқча сумма
        /// </summary>
        public double ExcessSumma { get; set; }

        /// <summary>
        /// Сервер вақти
        /// </summary>
        public DateTime RealTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double FakeSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date16 { get; set; }

        /// <summary>
        /// valyuta kodi
        /// </summary>
        public int SprObjectId { get; set; }

        /// <summary>
        /// valyuta nomi
        /// </summary>
        public string CurrencyName { get; set; }
    }
}
