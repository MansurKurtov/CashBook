using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation
{
    public class Book175PostViewModel
    {
        
        /// <summary>
        /// Сана
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Кассир коди(топширадиган)
        /// </summary>
        public int FromCashierId { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        public int? SprObjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorthAccount { get; set; }

        /// <summary>
        /// Sistema vaqti
        /// </summary>
        public DateTime? SystemDate { get; set; }

        /// <summary>
        /// Сановчи кассир коди
        /// </summary>
        public int CounterCashierId { get; set; }

        /// <summary>
        /// Қайта санаш кассасиданми
        /// </summary>
        public bool From175 { get; set; }
    }
}
