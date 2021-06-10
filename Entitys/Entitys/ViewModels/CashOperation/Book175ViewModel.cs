using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Entitys.ViewModels.CashOperation
{
    /// <summary>
    /// 
    /// </summary>
    public class Book175ViewModel
    {
       
        /// <summary>
        /// Сана
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Кассир коди(топширадиган)
        /// </summary>
        [BindNever]
        public int FromCasheirId { get; set; }

        /// <summary>
        /// Кассир коди(қабул қиладиган)
        /// </summary>
        public int ToCashierId { get; set; }

        /// <summary>
        /// Қиймати
        /// </summary>
        public double CashValue { get; set; }

        /// <summary>
        /// Олдим
        /// </summary>
        public bool Accept { get; set; }

        /// <summary>
        /// Номинал тури
        /// </summary>
        public int OperationId { get; set; }
    }
}
