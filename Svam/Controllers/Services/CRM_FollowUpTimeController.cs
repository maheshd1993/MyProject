using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Svam.Controllers.Services
{
    public class CRM_FollowUpTimeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetTimeByTZName(string ZoneName, string FollowUpDate, string FollowUpTime, string dateFormat, int? CompanyID) 
        {
            string Message = string.Empty;
            string ConvertedFupDateTime = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(FollowUpTime) && !string.IsNullOrEmpty(ZoneName) && ZoneName!= "Select Time Zone" && !string.IsNullOrEmpty(FollowUpDate) && !string.IsNullOrEmpty(dateFormat) && CompanyID!=null && CompanyID!=0)
                {
                    //var finalDateTime = string.Format("{0} {1}", FollowUpDate, FollowUpTime);
                    //DateTime dateTime = DateTime.ParseExact(finalDateTime, ""+dateFormat+" HH:mm", CultureInfo.InvariantCulture);// Convert.ToDateTime(finalDateTime);
                    
                    //ZoneName = (!string.IsNullOrEmpty(cl.ZoneName) && !cl.ZoneName.ToLower().Contains("Select")) ? cl.ZoneName : Constant.GetCompanyTimeZone(companyID);
                    //var dateFormat = Constant.DateFormatForApi(companyID);
                    //var followUpDateForIst = string.Format("{0:" + dateFormat + "}", FollowUpDate);
                    var istTime = Constant.GetFollowupTimeInIST(ZoneName, FollowUpDate, FollowUpTime,Convert.ToInt32(CompanyID));
                    if (!string.IsNullOrEmpty(istTime))
                    {
                        var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); 
                        ConvertedFupDateTime = string.Format("{0:"+dateFormat+ " HH:mm}", cDate);//send converted time to dateformat
                    }
                    
                }
                else
                {
                    Message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    HttpError err = new HttpError(Message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch(Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                Message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                HttpError err = new HttpError(Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            return Request.CreateResponse(HttpStatusCode.OK, ConvertedFupDateTime);
        }
    }
}
