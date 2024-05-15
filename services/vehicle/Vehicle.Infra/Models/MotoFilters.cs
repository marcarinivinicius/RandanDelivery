using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Infra.Models
{
    public class MotoFilters
    {
        public long Id { get; set; }
        public string PlateCode { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string Country { get; set; }
        public DateOnly Fabrication { get; set; }
        public bool Active { get; set; }
        public bool AllRecords { get; set; }

        public bool AllLocated { get; set; }

        public bool Located { get; set; }
    }
}
