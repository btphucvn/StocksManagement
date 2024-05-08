using StocksManagement.Controllers;
using StocksManagement.Model;
using StocksManagement.Services;

namespace StocksManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetCode_Click(object sender, EventArgs e)
        {
            //StockService.GetAllCode();
            
        }



        private void btnKetQuaKinhDoanh_Click(object sender, EventArgs e)
        {
            List<Stock> allStocks = StockController.GetAllStock(1001,1700);
            List<List<Stock>> listStocks = Tool.Tool.SplitList(allStocks, 40);
            foreach (List<Stock> stocks in listStocks)
            {
                Thread t = new Thread(() =>
                {
                    StockService.InsertValueBussinessFireAnt(stocks);
                });
                t.Start();
            }
        }

        private void btnBalanceSheet_Click(object sender, EventArgs e)
        {
            List<Stock> allStocks = StockController.GetAllStock(1,500);
            List<List<Stock>> listStocks = Tool.Tool.SplitList(allStocks, 40);
            foreach (List<Stock> stocks in listStocks)
            {
                Thread t = new Thread(() =>
                {
                    StockService.InsertValueBalanceSheetFireAnt(stocks);
                });
                t.Start();
            }
        }
    }
}