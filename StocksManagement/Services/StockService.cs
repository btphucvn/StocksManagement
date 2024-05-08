using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using StocksManagement.Controllers;
using StocksManagement.DB;
using StocksManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.Design.AxImporter;

namespace StocksManagement.Services
{
    public static class StockService
    {
        public static void GetAllCode()
        {
            List<Stock> stocks = StockController.GetAllStock();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");


            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);
            DateTime now = DateTime.Now;
            driver.Navigate().GoToUrl("https://s.cafef.vn/du-lieu-doanh-nghiep.chn");
            bool flag = true;

            while (flag)
            {
                int flagCount = 0;
                try
                {
                    flagCount = driver.FindElements(By.XPath("//*[@class='table-data-business table-data-business-item-0']/tbody/tr")).Count;
                }
                catch { }
                if (flagCount > 0)
                {
                    flag = false;
                }
                else
                {
                    Thread.Sleep(5000);

                }

            }
            int iRowsCount = driver.FindElements(By.XPath("//*[@class='table-data-business table-data-business-item-0']/tbody/tr")).Count;

            //string name = driver.FindElement(By.XPath("//*[@class='GridViewHienThi']/tbody/tr[" + i + "]/td[2]")).Text;
            for (int a = 1; a <= 82; a++)
            {
                for (int i = 1; i <= iRowsCount; i++)
                {
                    string xpath = "//*[@class='table-data-business table-data-business-item-0']/tbody/tr[" + i + "]/td[1]/a";
                    string code = driver.FindElement(By.XPath("//*[@class='table-data-business table-data-business-item-0']/tbody/tr[" + i + "]/td[1]/p/a")).Text;

                    string name = driver.FindElement(By.XPath("//*[@class='table-data-business table-data-business-item-0']/tbody/tr[" + i + "]/td[2]/p")).Text;
                    string exchange = driver.FindElement(By.XPath("//*[@class='table-data-business table-data-business-item-0']/tbody/tr[" + i + "]/td[4]/p")).Text;

                    Stock stock = new Stock();
                    stock.Code = code;
                    stock.Name = name;
                    stock.Exchange = exchange;
                    int id = StockController.Insert(stock);
                    if (id < 0)
                    {
                        MessageBox.Show("Lỗi: " + stock.Code);
                    }
                }
                driver.FindElement(By.XPath("//*[@class='fa fa-chevron-right']")).Click();
                Thread.Sleep(2000);
            }

            driver.Close();
            driver.Quit();



        }

        public static void InsertBussinessResultRow(List<Stock> stocks)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");
            stocks.OrderBy(x => x.Name);
            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);
            foreach (Stock stock in stocks)
            {
                if (stock.Exchange == "OTC") { continue; }
                VaoTrangWebCoPhieu(ref driver, stock.Code);



                if (driver.FindElements(By.XPath("//*[@class='flex items-center text-lg']")).Count > 0
                    && driver.FindElement(By.XPath("//*[@class='flex items-center text-lg']")).Text.Trim() == "Chưa niêm yết"
                    )
                {

                    continue;
                }
                //if (driver.FindElements(By.XPath("//*[@class='pb-2 overflow-hidden text-xl font-semibold whitespace-nowrap text-ellipsis']")).Count > 0)
                //{

                //    continue;
                //} 
                if (!VaoKetQuaKinhDoanh(ref driver, stock.Code))
                {
                    continue;
                }

              List<IWebElement> childRows = new List<IWebElement>();
                try { childRows = driver.FindElements(By.XPath("//*[@class='w-full']//tr")).ToList(); } catch { }
                if (childRows == null || childRows.Count == 0)
                {
                    continue;
                }
                //chờ load xong table

                //xổ table ra hết thì thôi

                ExpandTable(driver);
                //duyệt từng dòng table

                List<IWebElement> tableRows = driver.FindElements(By.XPath("//table[@class='w-full']/tbody/tr")).ToList();
                int sort = 1;
                for (int iRow = 1; iRow <= tableRows.Count; iRow++)
                {
                    Row rowInsert = new Row();
                    rowInsert.Sort = sort;
                    rowInsert.Code = stock.Code;
                    rowInsert.Name = driver.FindElement(By.XPath("//table[@class='w-full']/tbody/tr[" + iRow + "]/td")).Text;
                    rowInsert.Type = 2;
                    RowController.Insert(rowInsert);
                    sort++;
                }

            }
            driver.Close();
            driver.Quit();



        }

        public static void InsertBalanceSheetRow_CafeF(List<Stock> stocks)
        {


            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");

            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);
            foreach (Stock stock in stocks)
            {

                driver.Navigate().GoToUrl("https://www.google.com/");
                while (true)
                {
                    try
                    {
                        driver.Navigate().GoToUrl("https://s.cafef.vn/bao-cao-tai-chinh-chung-khoan/" + stock.Code + "/bsheet/2023/4/0/0/cong-ty-co-phan-chung-khoan-ssi.chn");
                        break;
                    }
                    catch
                    {
                        driver.Navigate().GoToUrl("https://www.google.com/");
                    }
                }

                ToolSelenium.ToolSelenium.WaitCountRow(driver, "//*[@id='tableContent']/tbody/tr");

                int rowCount = 0;

                if (!driver.Url.Contains("BaoCaoTaiChinh_V2"))
                {
                    rowCount = driver.FindElements(By.XPath("//*[@id='tableContent']/tbody/tr")).Count;
                }

                for (int i = 1; i < rowCount; i++)
                {
                    string name = driver.FindElement(By.XPath("//*[@id='tableContent']/tbody/tr[" + i + "]/td[1]")).Text;
                    Row row = new Row();
                    row.Name = name;
                    row.Code = stock.Code;
                    row.Sort = i;
                    row.Type = 1;
                    string err = RowCController.Insert(row);
                    if (!Double.TryParse(err, out _))
                    {
                        MessageBox.Show(err);
                    }
                }
                //driver.Close();
                //driver.Quit();
            }

        }

        public static void InsertValueBalanceSheet_CafeF(int fromYear, int toYear)
        {


            List<Stock> stocks = StockController.GetAllStock();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");
            stocks.OrderBy(x => x.Name);
            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);
            foreach (Stock stock in stocks)
            {
                //Stock stock = stocks.Where(x => x.Code == "ssi").FirstOrDefault();
                for (int year = toYear; year >= fromYear; year--)
                {
                    driver.Navigate().GoToUrl("https://www.google.com/");

                    while (true)
                    {
                        try
                        {
                            driver.Navigate().GoToUrl("https://s.cafef.vn/bao-cao-tai-chinh-chung-khoan/" + stock.Code + "/bsheet/"+year+"/4/0/0/cong-ty-co-phan-chung-khoan-ssi.chn");
                            //driver.Navigate().GoToUrl("https://s.cafef.vn/bao-cao-tai-chinh-chung-khoan/ssi/bsheet/" + year + "/4/0/0/cong-ty-co-phan-chung-khoan-ssi.chn");
                            break;
                        }
                        catch
                        {
                            driver.Close();
                            driver.Quit();
                            driver = new ChromeDriver(driverService, options);
                            driver.Navigate().GoToUrl("https://www.google.com/");
                        }
                    }

                    ToolSelenium.ToolSelenium.WaitCountRow(driver, "//*[@id='tableContent']/tbody/tr");



                    if (driver.Url.Contains("BaoCaoTaiChinh_V2"))
                    {
                        break;
                    }
                    if (!driver.FindElement(By.XPath("//*[@id='tblGridData']/tbody/tr[1]/td[5]")).Text.Contains("-"))
                    {
                        break;
                    }

                    int rowCount = driver.FindElements(By.XPath("//*[@id='tableContent']/tbody/tr")).Count;
                    for (int i = 1; i < rowCount; i++)
                    {
                        string name = driver.FindElement(By.XPath("//*[@id='tableContent']/tbody/tr[" + i + "]/td[1]")).Text;
                        //Row row = RowController.
                        List<Row> rows = RowController.GetRowByCode(stock.Code);
                        Row rowFound = new Row();
                        try
                        {
                            rowFound = rows.Where(x => x.Name == name).FirstOrDefault();
                        }
                        catch { }
                        if (rowFound != null)
                        {

                            for (int timeValue = 2; timeValue <= 5; timeValue++)
                            {
                                if (driver.FindElement(By.XPath("//*[@id='tblGridData']/tbody/tr[1]/td[" + timeValue + "]")).Text.Contains("-"))
                                {
                                    RowValue rowValue = new RowValue();
                                    try
                                    {
                                        rowValue.Value = double.Parse(driver.FindElement(By.XPath("//*[@id='tableContent']/tbody/tr[" + i + "]/td[" + timeValue + "]")).Text.Replace(",", ""));
                                    }
                                    catch { }
                                    rowValue.RowID = rowFound.ID;
                                    string strQuater = "01-" + driver.FindElement(By.XPath("//*[@id='tblGridData']/tbody/tr[1]/td[" + timeValue + "]")).Text
                                        .Replace("Quý 1", "3").Replace("Quý 2", "6").Replace("Quý 3", "9").Replace("Quý 4", "12");
                                    string tmp = Tool.Tool.FormatDateTo_DDMMYYY(strQuater);
                                    rowValue.TimeStamp = Tool.Tool.Convert_DDMMYYYY_To_Timestamp(Tool.Tool.FormatDateTo_DDMMYYY(strQuater));
                                    rowValue.Quater = driver.FindElement(By.XPath("//*[@id='tblGridData']/tbody/tr[1]/td[" + timeValue + "]")).Text;
                                    if (rowValue.Value != null)
                                    {
                                        RowValueCController.InsertAndUpdate(rowValue);
                                    }
                                }

                            }

                        }

                    }
                    int tmpEndLoop = 0;
                }
            }
            driver.Close();
            driver.Quit();
            
        }

        public static bool VaoKetQuaKinhDoanh(ref ChromeDriver driver, string code)
        {
            int counTry = 0;
            while (true)
            {
                try
                {
                    driver.FindElement(By.XPath("//*[@class='flex flex-row mb-2 text-[90%] md:text-[100%]']//li[6]")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[text()='Báo cáo tài chính']")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[text()='Kết quả kinh doanh']")).Click();
                    Thread.Sleep(3000);
                    return true;
                }
                catch
                {
                    driver.Close();
                    driver.Quit();
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--window-size=1300,1000");
                    //Ẩn command dos
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    driver = new ChromeDriver(driverService, options);
                    driver.Navigate().GoToUrl("https://www.google.com/");
                    VaoTrangWebCoPhieu(ref driver, code);
                    Thread.Sleep(1000);
                    if (counTry > 5)
                    {
                        return false;
                    }
                    counTry++;
                }
            }
        }

        public static bool VaoBangCanDoiKeToan(ref ChromeDriver driver,string code)
        {
            int counTry = 0;
            while (true)
            {
                try
                {
                    driver.FindElement(By.XPath("//*[@class='flex flex-row mb-2 text-[90%] md:text-[100%]']//li[6]")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[text()='Báo cáo tài chính']")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[text()='Cân đối kế toán']")).Click();
                    Thread.Sleep(3000);
                    return true;
                }
                catch
                {
                    driver.Close();
                    driver.Quit();
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--window-size=1300,1000");
                    //Ẩn command dos
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    driver = new ChromeDriver(driverService, options);
                    driver.Navigate().GoToUrl("https://www.google.com/");
                    VaoTrangWebCoPhieu(ref driver, code);
                    Thread.Sleep(1000);
                    if (counTry > 5)
                    {
                        return false;
                    }
                    counTry++;
                }
            }
        }
        public static void VaoTrangWebCoPhieu(ref ChromeDriver driver,string code)
        {

            while (true)
            {
                try
                {
                    driver.Navigate().GoToUrl("https://www.google.com/");
                    driver.Navigate().GoToUrl("https://fireant.vn/ma-chung-khoan/" + code);
                    
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                    IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(driver.FindElement(By.XPath("//a[text()='Trang chủ']"))));
                    Thread.Sleep(2000);

                    if (driver.FindElements(By.XPath("//button[text()='Để sau']")).Count > 0)
                    { driver.FindElement(By.XPath("//button[text()='Để sau']")).Click(); };
                    break;
                }
                catch
                {
                    driver.Close();
                    driver.Quit();
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--window-size=1300,1000");
                    //Ẩn command dos
                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    driver = new ChromeDriver(driverService, options);
                    driver.Navigate().GoToUrl("https://www.google.com/");
                }
            }
        }
        public static void ExpandTable(ChromeDriver driver)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                js.ExecuteScript(
                    "var l = document.getElementsByClassName('b24-widget-button-wrapper b24-widget-button-position-bottom-left b24-widget-button-visible')[0]; " +
                    "l.parentNode.removeChild(l);");
            }
            catch { }
            List<IWebElement> childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList();
            Thread.Sleep(500);
            while (childRows.Count > 0)
            {
                foreach (IWebElement childRow in childRows)
                {
                    try
                    {
                        childRow.Click();
                        Thread.Sleep(500);
                        //js.ExecuteScript("window.scrollTo(0, (50*"+countTry+");");

                    }
                    catch
                    {
                    }
                }
                childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList();
                if (childRows.Count > 0)
                {
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(childRows.FirstOrDefault());
                    actions.Perform();
                }
            }
        }
        public static void InserNameBalanceSheetFireAnt(List<Stock> stocks)
        {
            //List<Stock> stocks = StockController.GetAllStock();
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");
            stocks.OrderBy(x => x.Name);
            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);
            foreach (Stock stock in stocks)
            {
                if (stock.Exchange == "OTC") { continue; }
                VaoTrangWebCoPhieu(ref driver,stock.Code);



                if (driver.FindElements(By.XPath("//*[@class='flex items-center text-lg']")).Count > 0
                    && driver.FindElement(By.XPath("//*[@class='flex items-center text-lg']")).Text.Trim()=="Chưa niêm yết"
                    )
                {
                   
                    continue;
                }
                //if (driver.FindElements(By.XPath("//*[@class='pb-2 overflow-hidden text-xl font-semibold whitespace-nowrap text-ellipsis']")).Count > 0)
                //{

                //    continue;
                //} 
                if(!VaoBangCanDoiKeToan(ref driver,stock.Code))
                {
                    continue;
                }

                List<IWebElement> childRows = new List<IWebElement>();
                try { childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList(); } catch { }
                if(childRows==null||childRows.Count == 0)
                {
                    continue;
                }
                //chờ load xong table

                //int countBreak = 0;
                //int countTry = 0;
                //while (childRows.Count <= 0) 
                //{
                //    Thread.Sleep(500);
                //    try { childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList(); } catch { }
                //    if (countBreak > 20) { 
                //        driver.Close();
                //        driver.Quit();
                //        driver = new ChromeDriver(driverService, options);
                //        VaoTrangWebCoPhieu(driver, stock.Code);
                //        VaoBangCanDoiKeToan(driver, stock.Code);
                //        countBreak = 0;
                //        if (countTry > 5)
                //        {
                //            continue;
                //        }
                //        countTry++;
                //    }
                //    countBreak++; 
                //}
                //xổ table ra hết thì thôi

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript(
                    "var l = document.getElementsByClassName('b24-widget-button-wrapper b24-widget-button-position-bottom-left b24-widget-button-visible')[0]; " +
                    "l.parentNode.removeChild(l);");
                childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList();
                Thread.Sleep(500);
                while(childRows.Count > 0)
                {
                    foreach(IWebElement childRow in childRows)
                    {
                        try
                        {
                            childRow.Click();
                            Thread.Sleep(500);
                            //js.ExecuteScript("window.scrollTo(0, (50*"+countTry+");");

                        }
                        catch { 
                        }
                    }
                    childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList();
                    if(childRows.Count > 0)
                    {
                        Actions actions = new Actions(driver);
                        actions.MoveToElement(childRows.FirstOrDefault());
                        actions.Perform();
                    }
                }
                //duyệt từng dòng table

                List<IWebElement> tableRows = driver.FindElements(By.XPath("//table[@class='w-full']/tbody/tr")).ToList();
                int sort = 1;
                for(int iRow = 1; iRow <= tableRows.Count; iRow++)
                {
                    Row rowInsert = new Row();
                    rowInsert.Sort = sort;
                    rowInsert.Code = stock.Code;
                    rowInsert.Name = driver.FindElement(By.XPath("//table[@class='w-full']/tbody/tr["+iRow+"]/td")).Text;
                    rowInsert.Type = 1;
                    RowController.Insert(rowInsert);
                    sort++;
                }

            }
            driver.Close();
            driver.Quit();
            
                
        }
        public static void InsertValueBalanceSheetFireAnt(List<Stock> stocks)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");
            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);


            foreach (Stock stock in stocks)
            {
                if (stock.Exchange == "OTC") { continue; }
                VaoTrangWebCoPhieu(ref driver, stock.Code);



                if (driver.FindElements(By.XPath("//*[@class='flex items-center text-lg']")).Count > 0
                    && driver.FindElement(By.XPath("//*[@class='flex items-center text-lg']")).Text.Trim() == "Chưa niêm yết"
                    )
                {

                    continue;
                }
                //if (driver.FindElements(By.XPath("//*[@class='pb-2 overflow-hidden text-xl font-semibold whitespace-nowrap text-ellipsis']")).Count > 0)
                //{

                //    continue;
                //} 
                if (!VaoBangCanDoiKeToan(ref driver, stock.Code))
                {
                    continue;
                }

                List<IWebElement> childRows = new List<IWebElement>();
                try { childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList(); } catch { }
                if (childRows == null || childRows.Count == 0)
                {
                    continue;
                }
                //chờ load xong table

                //int countBreak = 0;
                //int countTry = 0;
                //while (childRows.Count <= 0) 
                //{
                //    Thread.Sleep(500);
                //    try { childRows = driver.FindElements(By.XPath("//button//*[@class='lucide lucide-plus']/parent::button")).ToList(); } catch { }
                //    if (countBreak > 20) { 
                //        driver.Close();
                //        driver.Quit();
                //        driver = new ChromeDriver(driverService, options);
                //        VaoTrangWebCoPhieu(driver, stock.Code);
                //        VaoBangCanDoiKeToan(driver, stock.Code);
                //        countBreak = 0;
                //        if (countTry > 5)
                //        {
                //            continue;
                //        }
                //        countTry++;
                //    }
                //    countBreak++; 
                //}


                //xổ table ra hết thì thôi
                ExpandTable(driver);
                //duyệt từng dòng table

                List<IWebElement> tableRows = driver.FindElements(By.XPath("//table[@class='w-full']/tbody/tr")).ToList();
                int sort = 1;
                while (true)
                {
                    for (int iRow = 1; iRow <= tableRows.Count; iRow++)
                    {
                        for (int iCol = 3; iCol <= 7; iCol++)
                        {

                            string name = driver.FindElement(By.XPath("//table[@class='w-full']/tbody/tr[" + iRow + "]/td")).Text;
                            Row row = new Row();
                            row = stocks.Where(x => x.Code == stock.Code).FirstOrDefault()
                                .Rows.Where(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.Type==1).FirstOrDefault();
                            if (row != null)
                            {
                                RowValue rowValue = new RowValue();
                                string strQuater = "";
                                try
                                {
                                    strQuater = "01-" + driver.FindElement(By.XPath("//table[@class='w-full']/thead//th[" + iCol + "]")).Text
                                .Replace(" ", "-").Trim().Replace("Q1", "03").Replace("Q2", "06").Replace("Q3", "09").Replace("Q4", "12");
                                }
                                catch { }
                                if (strQuater == "") {
                                    break;
                                }
                                
                                string quater = driver.FindElement(By.XPath("//table[@class='w-full']/thead//th[" + iCol + "]")).Text;
                                rowValue.Quater = quater;
                                double timeStamp = Tool.Tool.Convert_DDMMYYYY_To_Timestamp(Tool.Tool.FormatDateTo_DDMMYYY(strQuater));
                                rowValue.TimeStamp = timeStamp;
                                rowValue.RowID = row.ID;
                                try
                                {
                                    string tmp = driver.FindElement(By.XPath("//table[@class='w-full']//tbody/tr["+ iRow + "]/td[" + iCol + "]")).Text;
                                    rowValue.Value = double.Parse(driver.FindElement(By.XPath("//table[@class='w-full']//tbody/tr[" + iRow + "]/td[" + iCol + "]")).Text.Replace(",", ""))*1000000;
                                }
                                catch { }
                                RowValueController.InsertAndUpdate(rowValue);
                            }

                        }


                    }
                    if(!driver.FindElement(By.XPath("//*[@class='flex flex-row items-center']//*[@class='lucide lucide-chevron-left']/parent::button")).Enabled)
                    {
                        break;
                    }
                    break;
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    if (driver.FindElement(By.XPath("//*[@class='flex flex-row items-center']//*[@class='lucide lucide-chevron-left']/parent::button")).Enabled)
                    //    {
                    //        driver.FindElement(By.XPath("//*[@class='flex flex-row items-center']//*[@class='lucide lucide-chevron-left']")).Click();
                    //        Thread.Sleep(1000);

                    //    }
                    //}
                    //ExpandTable(driver);
                }

            }
            driver.Close();
            driver.Quit();


        }
        public static void InsertValueBussinessFireAnt(List<Stock> stocks)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--window-size=1300,1000");
            //Ẩn command dos
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            ChromeDriver driver = new ChromeDriver(driverService, options);


            foreach (Stock stock in stocks)
            {
                if (stock.Exchange == "OTC") { continue; }
                VaoTrangWebCoPhieu(ref driver, stock.Code);



                if (driver.FindElements(By.XPath("//*[@class='flex items-center text-lg']")).Count > 0
                    && driver.FindElement(By.XPath("//*[@class='flex items-center text-lg']")).Text.Trim() == "Chưa niêm yết"
                    )
                {

                    continue;
                }
                //if (driver.FindElements(By.XPath("//*[@class='pb-2 overflow-hidden text-xl font-semibold whitespace-nowrap text-ellipsis']")).Count > 0)
                //{

                //    continue;
                //} 
                if (!VaoKetQuaKinhDoanh(ref driver, stock.Code))
                {
                    continue;
                }

                List<IWebElement> childRows = new List<IWebElement>();
                try { childRows = driver.FindElements(By.XPath("//*[@class='w-full']//tr")).ToList(); } catch { }
                if (childRows == null || childRows.Count == 0)
                {
                    continue;
                }
                //chờ load xong table



                //xổ table ra hết thì thôi
                ExpandTable(driver);
                //duyệt từng dòng table

                List<IWebElement> tableRows = driver.FindElements(By.XPath("//table[@class='w-full']/tbody/tr")).ToList();
                int sort = 1;
                while (true)
                {
                    for (int iRow = 1; iRow <= tableRows.Count; iRow++)
                    {
                        for (int iCol = 3; iCol <= 7; iCol++)
                        {

                            string name = driver.FindElement(By.XPath("//table[@class='w-full']/tbody/tr[" + iRow + "]/td")).Text;
                            Row row = new Row();
                            row = stocks.Where(x => x.Code == stock.Code).FirstOrDefault()
                                .Rows.Where(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.Type==2).FirstOrDefault();
                            if (row != null)
                            {
                                RowValue rowValue = new RowValue();
                                string strQuater = "";
                                try
                                {
                                    strQuater = "01-" + driver.FindElement(By.XPath("//table[@class='w-full']/thead//th[" + iCol + "]")).Text
                                .Replace(" ", "-").Trim().Replace("Q1", "03").Replace("Q2", "06").Replace("Q3", "09").Replace("Q4", "12");
                                }
                                catch { }
                                if (strQuater == "")
                                {
                                    break;
                                }

                                string quater = driver.FindElement(By.XPath("//table[@class='w-full']/thead//th[" + iCol + "]")).Text;
                                rowValue.Quater = quater;
                                double timeStamp = Tool.Tool.Convert_DDMMYYYY_To_Timestamp(Tool.Tool.FormatDateTo_DDMMYYY(strQuater));
                                rowValue.TimeStamp = timeStamp;
                                rowValue.RowID = row.ID;
                                try
                                {
                                    string tmp = driver.FindElement(By.XPath("//table[@class='w-full']//tbody/tr[" + iRow + "]/td[" + iCol + "]")).Text;
                                    rowValue.Value = double.Parse(driver.FindElement(By.XPath("//table[@class='w-full']//tbody/tr[" + iRow + "]/td[" + iCol + "]")).Text.Replace(",", "")) * 1000000;
                                }
                                catch { }
                                string err = RowValueController.InsertAndUpdate(rowValue);
                                if (err != "")
                                {
                                    Console.WriteLine("Lỗi insert update: "+err);
                                }
                            }

                        }


                    }
                    if (!driver.FindElement(By.XPath("//*[@class='flex flex-row items-center']//*[@class='lucide lucide-chevron-left']/parent::button")).Enabled)
                    {
                        break;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (driver.FindElement(By.XPath("//*[@class='flex flex-row items-center']//*[@class='lucide lucide-chevron-left']/parent::button")).Enabled)
                        {
                            driver.FindElement(By.XPath("//*[@class='flex flex-row items-center']//*[@class='lucide lucide-chevron-left']")).Click();
                            Thread.Sleep(1000);

                        }
                    }
                    ExpandTable(driver);
                }

            }
            driver.Close();
            driver.Quit();


        }

    }
}
