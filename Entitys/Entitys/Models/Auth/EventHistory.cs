using Entitys.Enums;
using RepositoryCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entitys.Models.Auth
{
    [Table("EVENT_HISTORY")]
    public class EventHistory : IEntity<int>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("SYSTEM_DATE")]
        public DateTime SystemDate { get; set; }

        
        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("MODULE_ID")]        
        public ModuleType ModuleId { get; set; }

        [Column("BANK_KOD")]
        public int BankKod { get; set; }

        [Column("EVENT_TYPE")]
        public EventType UserEventType { get; set; }
    }
}
