using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTimah
{
    class Delivery
    {
        public int ctdReff { get; set; }
        public string ctdNumber { get; set; }
        public int ctdStatus { get; set; }
        public DateTime deliveryDate { get; set; }
        public string deliveryNumber { get; set; }
        public string vhcNumber { get; set; }
        public string driverName { get; set; }
        public string asal { get; set; }
        public string tujuan { get; set; }
        public decimal volume { get; set; }
    }
}
