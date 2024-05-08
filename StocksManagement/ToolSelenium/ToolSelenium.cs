using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksManagement.ToolSelenium
{
    public static class ToolSelenium
    {
        public static void WaitCountRow(ChromeDriver driver, string xpath)
        {
            bool flag = true;
            while (flag)
            {
                int flagCount = 0;
                try
                {
                    flagCount = driver.FindElements(By.XPath(xpath)).Count;

               
                    if (flagCount > 0 || driver.Url.Contains("BaoCaoTaiChinh_V2"))
                    {
                        flag = false;
                    }
                    else
                    {
                        Thread.Sleep(5000);

                    }
                }
                catch {
                    driver.Navigate().Refresh();
                }

            }
        }
        public static void WaitUntilFoundElement(ChromeDriver driver, string xpath)
        {
            bool flag = true;
            int countTry = 1;
            while (flag)
            {
                int flagCount = 0;
                try
                {
                    flagCount = driver.FindElements(By.XPath(xpath)).Count;
                }
                catch { }
                if (flagCount > 0)
                {
                    flag = false;
                }
                else
                {
                    Thread.Sleep(2000);
                    countTry++;
                    if(countTry > 4) {
                        driver.Navigate().Refresh();
                    }
                }

            }
        }
    }
}
