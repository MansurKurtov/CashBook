using RepositoryCore.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.CashOperation
{
    [Table("OTHER_INOUT")]
    public class OtherInout : IEntity<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NAME")]
        public string Name { get; set; }

        [Column("IS_MAIN_CASHIER")]
        public bool IsMainCashier { get; set; }

        [Column("INOUT_ID")]
        public int InOutId { get; set; } 
    }
}
