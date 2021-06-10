using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    public class JsonSectorModel
    {
        public string name { get; set; }
        public List<JsonSeatModel> seat { get; set; }
        public JsonSectorModel()
        {
            seat = new List<JsonSeatModel>();
        }

    }
    public class JsonSeatModel
    {
        public int seatId { get; set; }
        public int row { get; set; }
        public int number { get; set; }
    }
}
