using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entitys.Models
{
    public class GetBranch
    {
        [Column("KOD")]
        public int Kod { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("IS_CLOSE")]
        public int IsClose { get; set; }
    }
}
