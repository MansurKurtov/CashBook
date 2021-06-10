using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.PostModels.CashOperations
{
    public class SupervisingAccountantViewModel
    {
        public int Id { get; set; }
        
        public string Fio { get; set; }
        
        public int BankCode { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? UpdatedUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
