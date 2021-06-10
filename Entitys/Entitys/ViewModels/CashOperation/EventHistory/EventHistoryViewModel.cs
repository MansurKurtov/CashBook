using Entitys.Enums;
using System;
using System.Collections.Generic;

namespace Entitys.ViewModels.CashOperation.EventHistory
{
    public class EventHistoryViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime SystemDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ModulName { get; set; }        

        /// <summary>
        /// 
        /// </summary>
        public string UserEventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BankKod { get; set; }
    }

    public class ResultEventHistoryModel 
    {
        public List<EventHistoryViewModel> Data { get; set; }
        public int Total { get; set; }
    }
}
