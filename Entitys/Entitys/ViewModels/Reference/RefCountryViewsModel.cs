using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.ViewModels.Reference
{
    public class RefCountryViewsModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Номи
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Қисқача коди
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
