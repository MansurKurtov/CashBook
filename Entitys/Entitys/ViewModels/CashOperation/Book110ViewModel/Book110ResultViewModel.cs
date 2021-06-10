using Entitys.Models.CashOperation;
using Entitys.ViewModels.CashOperation.Book110;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.CashOperation.Book110ViewModels
{
    public class Book110ResultViewModel
    {      
        public List<Journal110> Data { get; set; }
        public int Total { get; set; }
    }
}
