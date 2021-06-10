using Entitys.Enums;
using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entitys.Models.Main
{
    [Table("ent_org")]
    public class EntOrg : IEntity<int>
    {


        /// <summary>
        /// Primary Key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Номи
        /// </summary>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Қисқа номи
        /// </summary>
        [Column("short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// Манзил
        /// </summary>
        [Column("address")]
        public string Address { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [Column("phone")]
        public string Phone { get; set; }


        /// <summary>
        /// Эл. Почта
        /// </summary>
        [Column("email")]
        public string Email { get; set; }


        /// <summary>
        /// Вебсайт
        /// </summary>
        [Column("website")]
        public string WebSite { get; set; }


        /// <summary>
        /// ИНН номер
        /// </summary>
        [Column("inn")]
        public string Inn { get; set; }

        /// <summary>
        /// ОКОНХ
        /// </summary>
        [Column("okonh")]
        public string Okonh { get; set; }

        /// <summary>
        /// ОКЭД
        /// </summary>
        [Column("oked")]
        public string Oked { get; set; }

        /// <summary>
        ///  Вилоят коди
        /// </summary>
        [Column("region")]
      //  [ForeignKey("RegionModel")]
        public int? Region { get; set; }
        //[IgnoreDataMember]
        //public virtual RefRegion RegionModel { get; set; }
        /// <summary>
        /// Туман коди
        /// </summary>
        [Column("district")]
      //  [ForeignKey("DistrictModel")]
        public int? District { get; set; }
        //[IgnoreDataMember]
        //public virtual RefDistrict DistrictModel { get; set; }

        /// <summary>
        /// Қўшимча маълумот
        /// </summary>
        [Column("add_info")]
        public string AddInfo { get; set; }

        /// <summary>
        /// Юқори ташкилот коди
        /// </summary>
        [Column("parent_id")]
        [ForeignKey("OrgModel")]
        public int? ParentId { get; set; }

        [IgnoreDataMember]
        public virtual EntOrg OrgModel { get; set; }

        [Column("facebook")]
        public string Facebook { get; set; }

        [Column("Telegram")]
        public string Telegram { get; set; }

        [Column("TelegramChannel")]
        public string TelegramChannel { get; set; }

        [Column("contact_name")]
        public string ContactName { get; set; }

        [Column("status")]
        public OrganizationStatus Status { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("structure_id")]
        public int StructureId { get; set; }
    }
}
