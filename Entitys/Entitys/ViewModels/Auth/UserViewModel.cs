using System.Collections.Generic;

namespace Entitys.ViewModels.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Фойдаланувчи логини
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Telefon
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Bank kodi
        /// </summary>
        public int? BankId { get; set; }

        /// <summary>
        /// Bank nomi
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Adminmi?
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// kassir tipi idisi
        /// </summary>
        public int? CashierTypeId { get; set; }

        /// <summary>
        /// Kassir tipi
        /// </summary>
        public string CashierTypeName { get; set; }

        /// <summary>
        /// Permission Idlar
        /// </summary>
        public List<int> PermissionIds { get; set; }

        /// <summary>
        /// Permission nomlari
        /// </summary>
        public string PermissionNames { get; set; }
    }
}
