using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housekeepinggit.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime endDate { get; set; }
        public decimal budget { get; set; }
        public string category { get; set; }

        public Location location { get; set; }
    }
}
