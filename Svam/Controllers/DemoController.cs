using System;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Traders.Controllers
{
    public class demoController : Controller
    {
        //
        // GET: /demo/

        public ActionResult Index()
        {
            #region Dm1
            //int a = -5;
            //int b = 9;
            //var c = a + b;
            //var dd = System.DateTime.Now;
            //DateTime monthStartDate = new DateTime(dd.Year, dd.Month, 1);
            //DateTime MonthendDate = monthStartDate.AddMonths(1).AddDays(-1);
            //var MDate = monthStartDate.ToString("MM/dd/yyyy");
            //var MEDate = MonthendDate.ToString("MM/dd/yyyy");


            //var ph = "12313131333";
            //var newph=ph.Replace("'","");
            //var getph = newph;
            #endregion

            #region dm2
            var ct = System.DateTime.Now;
            var newct = ct.AddHours(-5);
            newct = newct.AddMinutes(-30);
            var newct2 = newct.AddHours(-5);
            var dt = newct2;
            #endregion


            #region dm3


            //var a = "23/11/2016";
            //var b = "11:15 AM";
            //var c = a + " "+b;
            //var ac = Convert.ToDateTime(c);
            //var date = "23/11/2016 11:15:00 AM";
            //var tm = "11:15 AM";
            //var newdate =Convert.ToDateTime(date);
            


            //var cst = -6;
            //var ct = System.DateTime.Now;
            //var newct = ct.AddHours(-5);
            //newct = newct.AddMinutes(-30);
            //var newcst = newct.AddHours(cst);
            //double hr=(newdate - newcst).TotalMinutes;

            //var currntdate = DateTime.Now.AddMinutes(hr);

            #endregion

            #region DM4

            //var getPrevDate = System.DateTime.Now.AddDays(-1);
            //var getNewPrev = getPrevDate.ToString("MM/dd/yyyy");

            //var datetime = System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");

            //var dtt = Convert.ToDateTime(datetime).ToShortDateString();
            //var dttt = Convert.ToDateTime(datetime).ToShortTimeString();
            #endregion

            

            #region Get-MacAddress

            var mac = GetMACAddress();
            if (mac != string.Empty)
            {
                var newmac = Regex.Replace(mac, ".{2}", "$0-").TrimEnd('-');
            }
            var mac2 = "94-DE-80-4C-C0-AC-";
            var st = mac2.TrimEnd('-');
            

            #endregion
            return View();
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                    
                    //sMacAddress = adapter.GetIPv4Statistics().ToString();
                    //sMacAddress = adapter.GetIPStatistics().ToString();
                }
            } return sMacAddress;
        }

        public ActionResult test()
        {
            var date = Convert.ToDateTime("05/06/2017").DayOfWeek;
            var date2 = Convert.ToDateTime("05/07/2017").DayOfWeek;
            
            if (Convert.ToString(date) == "Saturday")
            {
                //var ab = 5;
            }
            return View();
        }

    }
}
