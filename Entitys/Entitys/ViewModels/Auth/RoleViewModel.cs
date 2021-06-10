using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.Auth
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Номи
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Изоҳ
        /// </summary>
        public string Comment { get; set; }
        public int StructureId { get; set; }
    }
}
