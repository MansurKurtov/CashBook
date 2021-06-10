using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Book155
{
    public class Book155PutViewModel
    {
        /// <summary>
        /// Ёзув коди
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Кассир коди(қабул қиладиган)
        /// </summary>
        public int CashierId { get; set; }

        public DateTime? Date { get; set; }

        /// <summary>
        /// Қиймати
        /// </summary>
        public double CashValue { get; set; }

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
        /// Сановчи кассир коди
        /// </summary>
        public int CounterCashierId { get; set; }

        /// <summary>
        /// Қайта санаш кассасиданми
        /// </summary>
        public bool From175 { get; set; }
    }
}
