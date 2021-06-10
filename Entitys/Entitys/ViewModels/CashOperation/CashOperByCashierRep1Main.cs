using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation
{
    public class CashOperByCashierRep1Main
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AllSumma { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CashOperByCashierValue> ValuesDate { get; set; }
    }
    public class CashOperByCashierValue
    {

        /// <summary>
        /// 
        /// </summary>
        public double Summa { get; set; }
    }
    public class CashOperByCashierValueBottom
    {

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Summa { get; set; }
    }
}
