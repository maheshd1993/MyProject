using Svam.EF;
using Svam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMCityController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of State
        /// <summary>
        /// Get List of State List based on country ID
        /// GET api/CRMCity/?CountryID=123&StateID=12
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>   
        public HttpResponseMessage Get(string CountryID, string StateID)
        {
            List<ManageCityModel> cityList = new List<ManageCityModel>();
            string message = string.Empty;
            try
            {
                if(StateID=="0")
                {
                    cityList.Add(new ManageCityModel { CityID = 0, CityName = "Select City" });
                }
                else
                {
                    string query = @"select ID as CityID, City as CityName from com_city where  Country=" + CountryID + " and State=" + StateID + "";
                    var GetCityList = db.Database.SqlQuery<ManageCityModel>(query).ToList();
                    //var GetCityList = db.com_city.Where(em => em.Country == CountryID && em.State == StateID).ToList();
                    if (GetCityList.Count > 0)
                    {
                        GetCityList.Insert(0, new ManageCityModel { CityID = 0, CityName = "Select City" });
                        //cityList.Add(new ManageCityModel { CityID = 0, CityName = "Select City" });//by default set select value           

                        //foreach (var item in GetCityList)
                        //{
                        //    ManageCityModel CM = new ManageCityModel();
                        //    CM.CityID = item.ID;
                        //    CM.CityName = item.City;
                        //    cityList.Add(CM);
                        //}
                        cityList = GetCityList;
                    }
                    else
                    {
                        message = string.Format("City List is Blank");
                    }
                }
                
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (message != string.Empty)
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, cityList);
            }
        }
        #endregion
    }
}
