using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksManagement.Model
{
    public class Stock
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Exchange { get; set; }
        public int IndustryID { get; set; }

        public List<Row> Rows { get; set; }
    }
}
