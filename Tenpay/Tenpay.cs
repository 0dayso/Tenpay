using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Tenpay
{
    public class Tenpay
    {


        /// <summary>
        /// LoginTenPay 登录财付通
        /// </summary>
        /// <returns></returns>
        public IWebDriver LoginAndGetTransactionInfo()
        {
            var options = new ChromeOptions();
            options.AddArgument("--user-agent=Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A5355d Safari/8536.25");

            using (var driver = new ChromeDriver(options))
            {
                var navigation = driver.Navigate();
                navigation.GoToUrl("https://www.tenpay.com");


                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20 * 1000));
                //等待登录控件可被点击
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("btnLogin")));
                //登录控件
                var btnLogin = driver.FindElement(By.Id("btnLogin"));
                btnLogin.Click();

                //var page = driver.PageSource;

                var allWindowsId = driver.WindowHandles;
                foreach (var windowId in allWindowsId)
                {

                    driver.SwitchTo().Window(windowId);
                }

                //这里试了几种方法  CssSelector才成功 可能是因为在div a下面
                //判断该元素是否被加载在DOM中，并不代表该元素一定可见        
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("#quick_login>fieldset>div.form-links>a.js_wx_login")));
                var js_wx_login = driver.FindElement(By.CssSelector("#quick_login>fieldset>div.form-links>a.js_wx_login"));


                js_wx_login.Click();


                //var driveBefore = driver;
                //var pageBefore = driveBefore.PageSource;
                //Console.WriteLine(pageBefore);

                //Thread.Sleep(1000*20);

                //var driverAfter = driver;
                //var pageAfter = driverAfter.PageSource;
                //Console.WriteLine(pageAfter);

                //等待扫码
                IsLoggedInLoop(driver, 1000*10);

                //得到交易信息
                GetTransactionInfo(driver);

                return driver;
            }

        }



        /// <summary>
        /// isLoggedIn 是否登录
        /// </summary>
        /// <param name="pageSourceBefore"></param>
        /// <param name="pageSourceAfter"></param>
        /// <returns></returns>
        private bool IsLoggedIn(string pageSourceBefore,string pageSourceAfter)
        {

            return !pageSourceBefore.Equals(pageSourceAfter);
        }


        /// <summary>
        /// IsLoggedInLoop
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="sleepTime"></param>
        /// <returns></returns>
        private void IsLoggedInLoop(IWebDriver driver,int sleepTime)
        {
            var pageSourceBefore = driver.PageSource;
            var isLoggedIn = false;
            
            while (!isLoggedIn)
            {
                Thread.Sleep(sleepTime);
                var pageSourceAfter = driver.PageSource;
                isLoggedIn = IsLoggedIn(pageSourceBefore, pageSourceAfter);
            }

            //Thread.Sleep(sleepTime);

        }




        /// <summary>
        /// GetTransactionInfo 得到交易数据
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        private IWebDriver GetTransactionInfo(IWebDriver driver)
        {

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20*1000));
            //等待登录控件可被点击
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#pnl_accountInfos>div.account-item.normal>div>a:nth-child(2)")));
            var account_operate = driver.FindElement(By.CssSelector("#pnl_accountInfos>div.account-item.normal>div>a:nth-child(2)"));
            //Actions action = new Actions(driver);
            //action.MoveToElement(account_operate);
            //action.Click(account_operate);
            account_operate.Click();

            //var driveBefore = driver;
            //var pageBefore = driveBefore.PageSource;
            //Console.WriteLine(pageBefore);


            //Console.WriteLine("=========================================================================================");

            //Thread.Sleep(1000 * 20);

            //var driverAfter = driver;
            //var pageAfter = driverAfter.PageSource;
            //Console.WriteLine(pageAfter);

            //等待扫码
            IsLoggedInLoop(driver, 1000 * 10);


            return driver;
        }






    }
}
