using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using NodaTime;
using NodaTime.TimeZones;
using Svam.Models;
using Svam.Models.DTO;
using TimeZoneConverter;
using Traders.Models;

namespace Svam.UtilityManager
{
    public class Constant
    {

        public static DateTime GetBharatTime()
        {
            //int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            string zoneId = "India Standard Time";//by default set india time zone
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetTimeZoneByCompanyId(" + CompanyID + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               ZoneName = Convert.ToString(dr["TimeZoneName"])                              
                           }).FirstOrDefault();

            if(GetData!=null)
            {
                zoneId = GetData.ZoneName;
            }
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            
            return localTime;
        }

        public static string GetCompanyTimeZone(int CompanyID) 
        {
            
            string zoneId = "India Standard Time";//by default set india time zone
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetTimeZoneByCompanyId(" + CompanyID + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               ZoneName = Convert.ToString(dr["TimeZoneName"])
                           }).FirstOrDefault();

            if (GetData != null)
            {
                zoneId = GetData.ZoneName;
            }            
            return zoneId;
        }

        public static DateTime GetimeForApi(int CompanyID)
        {
           
            string zoneId = "India Standard Time";//by default set india time zone
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetTimeZoneByCompanyId(" + CompanyID + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               ZoneName = Convert.ToString(dr["TimeZoneName"])
                           }).FirstOrDefault();

            if (GetData != null)
            {
                zoneId = GetData.ZoneName;
            }
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            return localTime;
        }

        public static TimeZoneInfo GetTimeZoneInfoForTzdbId(string tzdbId)
        {
            //string timeZone = tzdbId;
            //if(tzdbId== "Asia/Kathmandu")
            //{
            //    timeZone = "Asia/Katmandu";
            //}

            var mappings = TzdbDateTimeZoneSource.Default.WindowsMapping.MapZones;

            var map = mappings.FirstOrDefault(x => x.TzdbIds.Any(z => z.Equals(tzdbId, StringComparison.OrdinalIgnoreCase)));
            var tzi1 = map == null ? null : TimeZoneInfo.FindSystemTimeZoneById(map.WindowsId);
            var tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //var tzi = map == null ? null : TimeZoneInfo.FindSystemTimeZoneById(map.WindowsId);

            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);

            // Either of these will work on any platform:
            //TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo("Eastern Standard Time");            
            //string tz = TZConvert.IanaToWindows(tzdbId);

            //TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(tzdbId);
            //var tz = tzi == null ? null : tzi;
            return tzi;
        }

        public static string JsDateFormat(int? CompanyId)
        {
            string dformat = "dd/mm/yyyy";
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetDateFormatByCompanyId(" + CompanyId + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               DateFormat = Convert.ToString(dr["DateFormat"])
                           }).FirstOrDefault();
            if(GetData!=null)
            {
                if(GetData.DateFormat== "dd/MM/yyyy")
                {
                    dformat = "dd/mm/yyyy";
                }
                else if (GetData.DateFormat == "dd/MM/yy")
                {
                    dformat = "dd/mm/yy";
                }
                else if (GetData.DateFormat == "d/M/yy")
                {
                    dformat = "d/m/yy";
                }
                else if (GetData.DateFormat == "d.M.yy")
                {
                    dformat = "d.m.yy";
                }
                else if (GetData.DateFormat == "yyyy-MM-dd")
                {
                    dformat = "yyyy-mm-dd";
                }
                else if (GetData.DateFormat == "yyyy-MM-dd")
                {
                    dformat = "yyyy-mm-dd";
                }
                else if (GetData.DateFormat == "yyyy-MM-dd")
                {
                    dformat = "yyyy-mm-dd";
                }
                else if (GetData.DateFormat == "dd-MM-yyyy")
                {
                    dformat = "dd-mm-yyyy";
                }
                else if (GetData.DateFormat == "dd-MM-yy")
                {
                    dformat = "dd-mm-yy";
                }
                else if (GetData.DateFormat == "d-M-yy")
                {
                    dformat = "d-m-yy";
                }
                else if (GetData.DateFormat == "M/d/yyyy")
                {
                    dformat = "m/d/yyyy";
                }
                else if (GetData.DateFormat == "m/d/yy")
                {
                    dformat = "m/d/yy";
                }
                else if (GetData.DateFormat == "MM/dd/yy")
                {
                    dformat = "mm/dd/yy";
                }
                else if (GetData.DateFormat == "MM/dd/yyyy")
                {
                    dformat = "mm/dd/yyyy";
                }
            }
            return dformat;
        }

        public static string DateFormat()
        {
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            string dformat = "dd/MM/yyyy";
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetDateFormatByCompanyId(" + CompanyID + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               DateFormat = Convert.ToString(dr["DateFormat"])
                           }).FirstOrDefault();
            if (GetData != null)
            {
                dformat = GetData.DateFormat;
                //if (GetData.DateFormat == "dd/MM/yyyy")
                //{
                //    dformat = "dd/mm/yyyy";
                //}
                //else if (GetData.DateFormat == "dd/MM/yy")
                //{
                //    dformat = "dd/mm/yy";
                //}
                //else if (GetData.DateFormat == "d/M/yy")
                //{
                //    dformat = "d/m/yy";
                //}
                //else if (GetData.DateFormat == "d.M.yy")
                //{
                //    dformat = "d.m.yy";
                //}
                //else if (GetData.DateFormat == "yyyy-MM-dd")
                //{
                //    dformat = "yyyy-mm-dd";
                //}
                //else if (GetData.DateFormat == "yyyy-MM-dd")
                //{
                //    dformat = "yyyy-mm-dd";
                //}
                //else if (GetData.DateFormat == "yyyy-MM-dd")
                //{
                //    dformat = "yyyy-mm-dd";
                //}
                //else if (GetData.DateFormat == "dd-MM-yyyy")
                //{
                //    dformat = "dd-mm-yyyy";
                //}
                //else if (GetData.DateFormat == "dd-MM-yy")
                //{
                //    dformat = "dd-mm-yy";
                //}
                //else if (GetData.DateFormat == "d-M-yy")
                //{
                //    dformat = "d-m-yy";
                //}
                //else if (GetData.DateFormat == "M/d/yyyy")
                //{
                //    dformat = "m/d/yyyy";
                //}
                //else if (GetData.DateFormat == "m/d/yy")
                //{
                //    dformat = "m/d/yy";
                //}
                //else if (GetData.DateFormat == "MM/dd/yy")
                //{
                //    dformat = "mm/dd/yy";
                //}
                //else if (GetData.DateFormat == "MM/dd/yyyy")
                //{
                //    dformat = "mm/dd/yyyy";
                //}
            }
            return dformat;
        }

        public static string DateFormatForApi(int CompanyID)
        {
            
            string dformat = "dd/MM/yyyy";
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetDateFormatByCompanyId(" + CompanyID + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               DateFormat = Convert.ToString(dr["DateFormat"])
                           }).FirstOrDefault();
            if (GetData != null)
            {
                dformat = GetData.DateFormat;
                //if (GetData.DateFormat == "dd/MM/yyyy")
                //{
                //    dformat = "dd/mm/yyyy";
                //}
                //else if (GetData.DateFormat == "dd/MM/yy")
                //{
                //    dformat = "dd/mm/yy";
                //}
                //else if (GetData.DateFormat == "d/M/yy")
                //{
                //    dformat = "d/m/yy";
                //}
                //else if (GetData.DateFormat == "d.M.yy")
                //{
                //    dformat = "d.m.yy";
                //}
                //else if (GetData.DateFormat == "yyyy-MM-dd")
                //{
                //    dformat = "yyyy-mm-dd";
                //}
                //else if (GetData.DateFormat == "yyyy-MM-dd")
                //{
                //    dformat = "yyyy-mm-dd";
                //}
                //else if (GetData.DateFormat == "yyyy-MM-dd")
                //{
                //    dformat = "yyyy-mm-dd";
                //}
                //else if (GetData.DateFormat == "dd-MM-yyyy")
                //{
                //    dformat = "dd-mm-yyyy";
                //}
                //else if (GetData.DateFormat == "dd-MM-yy")
                //{
                //    dformat = "dd-mm-yy";
                //}
                //else if (GetData.DateFormat == "d-M-yy")
                //{
                //    dformat = "d-m-yy";
                //}
                //else if (GetData.DateFormat == "M/d/yyyy")
                //{
                //    dformat = "m/d/yyyy";
                //}
                //else if (GetData.DateFormat == "m/d/yy")
                //{
                //    dformat = "m/d/yy";
                //}
                //else if (GetData.DateFormat == "MM/dd/yy")
                //{
                //    dformat = "mm/dd/yy";
                //}
                //else if (GetData.DateFormat == "MM/dd/yyyy")
                //{
                //    dformat = "mm/dd/yyyy";
                //}
            }
            return dformat;
        }

        public static string GetFollowupTimeInIST(string ZoneName, string FDate, string ftime,int CompanyID)
        {
            //var ReturnMsg = "";   
            string returnDate=string.Empty;
            string companyTimeZone = GetCompanyTimeZone(CompanyID);//get companytime zone
            try
            {
                string timeZoneName = string.Empty;
                var DateFormat = DateFormatForApi(CompanyID);

                var finalDateTime = string.Format("{0} {1}", FDate, ftime);//Convert.ToString(fDateTime);

                if (!string.IsNullOrEmpty(ZoneName) && !ZoneName.ToLower().Contains("Select"))
                {
                    if (ZoneName == "IST")
                    {
                        timeZoneName = "India Standard Time";
                    }
                    else if (ZoneName == "CST")
                    {
                        timeZoneName = "Central Standard Time";
                    }
                    else if (ZoneName == "EST")
                    {
                        timeZoneName = "Eastern Standard Time";
                    }
                    else if (ZoneName == "MST")
                    {
                        timeZoneName = "Mountain Standard Time";
                    }
                    else if (ZoneName == "PST")
                    {
                        timeZoneName = "Pacific Standard Time";
                    }
                    else
                    {
                        timeZoneName = ZoneName;
                    }
                }
                else
                {
                    timeZoneName = companyTimeZone;
                }


                if (!string.IsNullOrEmpty(FDate) && !string.IsNullOrEmpty(ftime))
                {
                    string Dformat = string.Format("{0}", DateFormat + " HH:mm");
                    if (ftime.ToLower().Contains("am") || ftime.ToLower().Contains("pm"))
                    {
                        Dformat = string.Format("{0}", DateFormat + " hh:mm tt");
                    }
                                        
                    DateTime time = DateTime.ParseExact(finalDateTime, Dformat, CultureInfo.InvariantCulture);

                    TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName); // source Time Zone

                    //var  dayLight= estZone.IsDaylightSavingTime(time) ? estZone.DaylightName : estZone.StandardName;
                    TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(companyTimeZone); // destination Time Zone

                    DateTime convertedDate = TimeZoneInfo.ConvertTime(time, sourceTimeZone, destinationTimeZone);

                    returnDate = string.Format("{0:MM-dd-yyyy HH:mm:ss}", convertedDate);                     
                }
                else
                {
                    returnDate = string.Format("{0:MM-dd-yyyy HH:mm:ss}", GetBharatTime());
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                
            }
            return returnDate;
        }

        public static string[] DateFormates()
        {
            var arrayData = new string[] { "dd/MM/yyyy", "dd/MM/yy", "d/M/yy", "d.M.yy", "yyyy-MM-dd", "dd-MM-yyyy", "dd-MM-yy", "d-M-yy", "M/d/yyyy", "m/d/yy", "MM/dd/yy", "MM/dd/yyyy" };
            return arrayData;
        }

        public static void Main(String[] argv)
        {
            DateTime currentTime = DateTime.Now;

            Console.WriteLine("Los Angeles: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Pacific Standard Time"));
            Console.WriteLine("Chicago: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Central Standard Time"));
            Console.WriteLine("New York: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Eastern Standard Time"));
            Console.WriteLine("London: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "GMT Standard Time"));
            Console.WriteLine("Moscow: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Russian Standard Time"));
            Console.WriteLine("New Delhi: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "India Standard Time"));
            Console.WriteLine("Beijing: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "China Standard Time"));
            Console.WriteLine("Tokyo: {0}",
                              TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Tokyo Standard Time"));


        }

        //public static string AddStdZoneName(string tzdbId)
        //{
        //    string timeZone = "";

        //    if(tzdbId== "Antarctica/Troll")
        //    {
        //        timeZone = "Greenwich Standard Time";
        //    }
        //    else if (tzdbId == "America/Argentina/Buenos_Aires")
        //    {
        //        timeZone = "Argentina Standard Time";
        //    }
        //    else if (tzdbId == "America/Argentina/Cordoba")
        //    {
        //        timeZone = "Argentina Standard Time";
        //    }
        //    else if (tzdbId == "America/Argentina/Jujuy")
        //    {
        //        timeZone = "Argentina Standard Time";
        //    }
        //    else if (tzdbId == "America/Argentina/Catamarca")
        //    {
        //        timeZone = "Argentina Standard Time";
        //    }
        //    else if (tzdbId == "America/Argentina/Mendoza")
        //    {
        //        timeZone = "Argentina Standard Time";
        //    }
        //    else if (tzdbId == "America/Atikokan")
        //    {
        //        timeZone = "Eastern Standard Time";
        //    }
        //    else if (tzdbId == "Africa/Asmara")
        //    {
        //        timeZone = "E. Africa Standard Time";
        //    }
        //    else if (tzdbId == "Pacific/Chuuk")
        //    {
        //        timeZone = "E. Australia Standard Time";
        //    }
        //    else if (tzdbId == "Pacific/Pohnpei")
        //    {
        //        timeZone = "Sakhalin Standard Time";
        //    }
        //    else if (tzdbId == "Atlantic/Faroe")
        //    {
        //        timeZone = "GMT Standard Time";
        //    }
        //    else if (tzdbId == "America/Nuuk")
        //    {
        //        timeZone = "Greenland Standard Time";
        //    }
        //    else if (tzdbId == "Asia/Kolkata")
        //    {
        //        timeZone = "India Standard Time";
        //    }
        //    else if (tzdbId == "Asia/Yangon")
        //    {
        //        timeZone = "Myanmar Standard Time";
        //    }
        //    else if (tzdbId == "Asia/Kathmandu")
        //    {
        //        timeZone = "Nepal Standard Time";
        //    }
        //    else if (tzdbId == "America/Kentucky/Louisville")
        //    {
        //        timeZone = "Eastern Standard Time";
        //    }
        //    else if (tzdbId == "America/Indiana/Indianapolis")
        //    {
        //        timeZone = "Eastern Standard Time";
        //    }
        //    else if(tzdbId== "Asia/Ho_Chi_Minh")
        //    {
        //        timeZone = "SE Asia Standard Time";
        //    }
        //    else
        //    {
        //        var mappings = TzdbDateTimeZoneSource.Default.WindowsMapping.MapZones;

        //        var map = mappings.FirstOrDefault(x => x.TzdbIds.Any(z => z.Equals(tzdbId, StringComparison.OrdinalIgnoreCase)));
        //        timeZone = map == null ? null : map.WindowsId;
        //    }



        //    return timeZone;
        //}

        //public static string  GetWeekOfMonth(DateTime dd)
        //{


        //    // // using System.Globalization;
        //    string weekName = "";
        //    //List<string> weeList  = new List<string> ();
        //    var calendar = CultureInfo.CurrentCulture.Calendar;
        //    var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        //    var weekPeriods =
        //    Enumerable.Range(1, calendar.GetDaysInMonth(dd.Year, dd.Month))
        //              .Select(d =>
        //              {
        //                  var date = new DateTime(dd.Year, dd.Month, d);
        //                  var weekNumInYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, firstDayOfWeek);
        //                  return new { date, weekNumInYear };
        //              })
        //              .GroupBy(x => x.weekNumInYear)
        //              .Select(x => new { DateFrom = x.First().date, To = x.Last().date })
        //              .ToList();

        //    foreach (var d in weekPeriods)
        //    {
        //        weekName = d.DateFrom.ToString("dd/MMM/yyyy") + "-" + d.To.ToString("dd/MMM/yyyy");
        //        //weeList.Add(weekName);
        //    }

        //    return weekName;
        //}
    }


}