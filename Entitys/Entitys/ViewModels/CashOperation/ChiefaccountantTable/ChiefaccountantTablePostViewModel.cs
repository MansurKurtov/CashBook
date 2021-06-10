using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.ViewModels.CashOperation.ChiefaccountantTable
{
    public class ChiefaccountantTablePostViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public string FIO { get; set; }

        /// <summary>
        /// 
        /// </summary>        
        public int BankKod { get; set;}

        /// <summary>
        /// 
        /// </summary>        
        [NotMapped]
        public int UserId { get; set; }
    }
}
