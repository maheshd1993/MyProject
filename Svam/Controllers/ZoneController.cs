using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Svam.EF;
using Svam.Models;
using Svam.Models.DTO;
using Svam.UtilityManager;

namespace Traders.Controllers
{
    public class ZoneController : Controller
    {
        niscrmEntities db = new niscrmEntities();       
        public ActionResult GetFollowupTimeInIST(string ZoneName,string FDate,string ftime)
        {
            FollowUpTimeConvertedDTO fupDto = new FollowUpTimeConvertedDTO();
            //var ReturnMsg = "";
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string companyTimeZone= Constant.GetCompanyTimeZone(CompanyID);//get companytime zone
            try
            {
                string timeZoneName = string.Empty;
                var DateFormat = Constant.DateFormat();

                var finalDateTime = string.Format("{0} {1}", FDate, ftime);//Convert.ToString(fDateTime);

                if(!string.IsNullOrEmpty(ZoneName))
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
               

                if(!string.IsNullOrEmpty(FDate) && !string.IsNullOrEmpty(ftime))
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

                    fupDto.SavedDateTime = string.Format("{0:MM-dd-yyyy HH:mm:ss}", convertedDate); 
                  
                    string resultDate = string.Format("{0:" + Dformat + "}", convertedDate);
                    fupDto.DisplayedDateTime = resultDate;
                   // ReturnMsg = resultDate;
                    fupDto.Message = "ok";
                }
                else
                {
                    fupDto.Message = "empty";
                }
                

                //if (!string.IsNullOrEmpty(ZoneName) && ZoneName != "IST")
                //{                   
                //    string format1 = String.Format("{0}", DateFormat + " hh:mm tt");
                //    var FinalFDateTime = DateTime.ParseExact(finalDateTime, format1, CultureInfo.InvariantCulture); //Convert.ToDateTime(finalDateTime);
                //    var GetData = db.crm_zonetbl.Where(em => em.ZoneName == ZoneName).FirstOrDefault();
                //    if (GetData != null)
                //    {
                //        var IST = System.DateTime.Now;
                //        var NewIST = IST.AddHours(-5);
                //        NewIST = NewIST.AddMinutes(-30);
                //        if (GetData.ZoneHours >0)
                //        {
                //            NewIST = NewIST.AddHours(GetData.ZoneHours);
                //        }
                //        else if (GetData.ZoneMin >0)
                //        {
                //            NewIST = NewIST.AddMinutes(GetData.ZoneMin);
                //        }
                //        double TotalMinute = (FinalFDateTime - NewIST).TotalMinutes;

                //        //To get the Predict Date and Time
                //        var PridictDateTime = DateTime.Now.AddMinutes(TotalMinute);

                //        string format = String.Format("{0}", DateFormat + " hh:mm tt");
                //        string resultDate = String.Format("{0:" + format + "}", PridictDateTime);
                //        ReturnMsg = resultDate;
                //    }
                //}
                //else
                //{
                //    string format = String.Format("{0}", DateFormat + " hh:mm tt");
                //    var PridictDateTime = DateTime.ParseExact(finalDateTime, format, CultureInfo.InvariantCulture); //Convert.ToDateTime(finalDateTime);

                //    string resultDate = String.Format("{0:" + format + "}", PridictDateTime);
                //    //ReturnMsg = Convert.ToString(finalDateTime);
                //    ReturnMsg = resultDate;
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);  
                //ReturnMsg = "fail";
                fupDto.Message = "fail";
            }
            return Json(fupDto,JsonRequestBehavior.AllowGet);
        }
    }
}
