using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.PostModels.CashOperations
{
    public class SupervisingAccountantPostModel
    {
        public int Id { get; set; }
        
        public string Fio { get; set; }
        
        public int BankCode { get; set; }
        
        [NotMapped]
        public int CreatedUserId { get; set; }

        [NotMapped]
        public DateTime CreateDate { get; set; }
        [NotMapped]
        public int UpdatedUserId { get; set; }

        [NotMapped]
        public DateTime UpdatedDate { get; set; }
    }
}
