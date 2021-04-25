using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace housekeepinggit.Models
{
    public class Location
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }

        public ICollection<Task> tasks { get; set; }
        public ApplicationUser creator { get; set; }
    }
}
