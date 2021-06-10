using RepositoryCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Entitys.Models.Reference
{
    [Table("ref_regions")]
    public class RefRegion : IEntity<int>
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
        /// Қисқача коди
        /// </summary>
        [Column("shortname")]
        public string ShortName { get; set; }

        /// <summary>
        /// Display order
        /// </summary>
        [Column("display_order")]
        public int DisplayOrder { get; set; }


        /// <summary>
        /// Қайси ҳудудга тегишли
        /// </summary>
        [Column("country_id")]
        [ForeignKey("RefCountryModel")]
        public int? CountryId { get; set; }
        [IgnoreDataMember]
        public virtual RefCountry RefCountryModel { get; set; }


    }
}
