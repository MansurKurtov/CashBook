using Entitys.Models.Main;
using Entitys.Models.Reference;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entitys.Models.Auth
{
    [Table("auth_users")]
    public class AuthUsers : IEntity<int>
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Фойдаланувчи логини
        /// </summary>
        [Column("username")]
        public string UserName { get; set; }


        /// <summary>
        /// Парол учун салт
        /// </summary>
        [Column("salt")]
        public string Salt { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Column("lastname")]
        public string LastName { get; set; }
        /// <summary>
        /// Исми
        /// </summary>
        [Column("firstname")]
        public string FirstName { get; set; }
        /// <summary>
        /// Отасининг исми
        /// </summary>
        [Column("middlename")]
        public string MiddleName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Column("email")]
        public string Email { get; set; }
        /// <summary>
        /// Telefon
        /// </summary>
        [Column("telefon")]
        public string Telefon { get; set; }
        /// <summary>
        /// Тизим админ
        /// </summary>
        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("is_main")]
        public bool? IsMain { get; set; }
        /// <summary>
        /// Актив
        /// </summary>
        [Column("active")]
        public bool Active { get; set; }
        /// <summary>
        /// Яратилган сана
        /// </summary>
        [Column("created")]
        public DateTime Created { get; set; }
        /// <summary>
        /// Яратган фойдаланувчи
        /// </summary>
        [Column("createdby")]
       // [ForeignKey("UsersModel")]
        public int? CreatedBy { get; set; }

      //  public virtual AuthUsers CreatedByModel { get; set; }

        /// <summary>
        /// Ташкилот коди
        /// </summary>
        [Column("org_id")]        
        public int? OrgId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Структура коди
        /// </summary>
        [Column("structure_id")]
        [ForeignKey("RefStructureModel")]
        [Required]
        public int StructureId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        public virtual RefStructure RefStructureModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("cashier_type_id")]
        public int? CashierTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("IsLogged")]
        public bool? IsLogged { get; set; }
    }
}
