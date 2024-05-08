using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksManagement.Model
{
    public class Row
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public int Sort { get; set; }
        public List<RowValue> RowValues { get; set; }
    }
}
