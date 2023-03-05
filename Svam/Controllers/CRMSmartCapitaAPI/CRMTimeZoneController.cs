using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMTimeZoneController : ApiController
    { 
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of TimeZone
        /// <summary>
        /// Get List of TimeZone 
        /// GET api/CRMTimeZone
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Get(int? CountryId)
        {
            //List<crm_zonetbl> TimeZoneList = new List<crm_zonetbl>();
            List<TimeZoneApiModel> TimeZoneList = new List<TimeZoneApiModel>();
            string message = string.Empty;
            try
            {
                if(CountryId==null || CountryId==0)
                {
                    message = "Please pass country id";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                //var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                //string name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                var countryData =await db.acc_countries.Where(a => a.id == CountryId).FirstOrDefaultAsync();
                if(countryData!=null && !string.IsNullOrEmpty(countryData.country_code))
                {
                    string countryCode = countryData.country_code; 
                    //var GetZoneName = db.crm_zonetbl.Where(em => em.Status == true).ToList();
                    string qury = string.Format("select zone_name, standard_zone_name as StandardTZName from time_zone where country_code='{0}'", countryCode);
                    var GetZoneName = await db.Database.SqlQuery<TZName>(qury).ToListAsync();
                    if (GetZoneName!=null && GetZoneName.Count > 0)
                    {
                        TimeZoneList.Add(new TimeZoneApiModel { ZoneName = "", Text="Select Time Zone" });
                        foreach (var item in GetZoneName)
                        {
                            TimeZoneApiModel zone = new TimeZoneApiModel();
                            zone.ZoneName = item.StandardTZName;
                            zone.Text = item.zone_name;
                            TimeZoneList.Add(zone);
                        }
                    }
                    else
                    {
                        message = "No record found";
                        HttpError err = new HttpError(message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                }
                else
                {
                    message = "No record found";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                          
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                message = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            return Request.CreateResponse(HttpStatusCode.OK, TimeZoneList);

        }
        #endregion
    }
}
