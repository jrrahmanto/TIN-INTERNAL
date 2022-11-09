using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulatorTimah
{
    class ctdDaily
    {
        public int idCtd { get; set; }
        public string receiveNumber { get; set; }
        public int ctdReff { get; set; }
        public string ctdNumber { get; set; }
        public DateTime ctdDate { get; set; }
        public int ctdStatus { get; set; }
        public int idSeller { get; set; }
        public string sellerName { get; set; }
        public string tradeAccount { get; set; }
        public string brandCode { get; set; }
        public string brandName { get; set; }
        public decimal volume { get; set; }
        public int whNumber { get; set; }
        public string whLocation { get; set; }
        public string coaNumber { get; set; }
        public string qualityCode { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public string sync { get; set; }
        public string status { get; set; }
    }
}
