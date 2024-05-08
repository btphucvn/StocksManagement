using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksManagement.Model
{
    public class RowValue
    {
        public int ID { get; set; }
        public double Value { get; set; }
        public double TimeStamp { get; set; }
        public int RowID { get; set; }
        public string Quater { get; set; }

    }
}
