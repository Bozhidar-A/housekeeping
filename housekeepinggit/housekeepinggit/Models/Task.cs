using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string category { get; set; }
        [DisplayName("Status")]
        public string status { get; set; }

        public Location location { get; set; }
        public ApplicationUser creator { get; set; }
        public ApplicationUser houseKeeper { get; set; }
    }
}
