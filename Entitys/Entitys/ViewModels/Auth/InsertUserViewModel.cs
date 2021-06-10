using System.Collections.Generic;

namespace Entitys.ViewModels.Auth
{
    /// <summary>
    /// User insert qilinganda qaytadigan Response Model
    /// </summary>
    public class InsertUserViewModel
    {
        /// <summary>
        /// Foydalanuvchi Idisi
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User banki nomlanishi
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Kassir tipi nomlanishi
        /// </summary>
        public string CashierTypeName { get; set; }

        /// <summary>
        /// Userga biriktirilgan Permission Idlar
        /// </summary>
        public List<int> PermissionIds { get; set; }

        /// <summary>
        /// Userga biriktirilgan Permission nomlari
        /// </summary>
        public string PermissionNames { get; set; }
    }
}
