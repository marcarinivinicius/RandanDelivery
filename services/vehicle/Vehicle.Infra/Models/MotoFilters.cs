using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Infra.Models
{
    public class MotoFilters
    {
        public string PlateCode { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string Country { get; set; }
        public DateOnly Fabrication { get; set; }
        public bool Active { get; set; }
    }
}
