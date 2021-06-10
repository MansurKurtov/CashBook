using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public class MonitoringTicketSearchModel
    {
        public string date1 { get; set; }
        public string date2 { get; set; }

        public int? eventId { get; set; }
        public int? eventSessionId { get; set; }
        public int? orgId { get; set; }
        public int? userId { get; set; }
        public int? organizatorId { get; set; }

    }
}
