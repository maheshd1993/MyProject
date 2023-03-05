using Svam.EF;
using Svam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMCountryController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of Country
        /// <summary>
        /// Get List of Country List
        /// GET api/CRMCountry
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            CreateLeadsModel CLM = new CreateLeadsModel();
            string message = string.Empty;
            try
            {
                
                string query = @"select id as CountryID, country_name as CountryName from acc_countries";
                var GetCountryList = db.Database.SqlQuery<ManageCountryModel>(query).ToList();
                //var GetCountryList = db.acc_countries.ToList();
                if (GetCountryList.Count > 0)
                {
                    //List<ManageCountryModel> cList = new List<ManageCountryModel>();
                    GetCountryList.Insert(0, new ManageCountryModel { CountryID = 0, CountryName = "Select Country" });
                    //cList.Add(new ManageCountryModel { CountryID = 0, CountryName = "Select Country" });
                    //foreach (var item in GetCountryList)
                    //{
                    //    ManageCountryModel CM = new ManageCountryModel();
                    //    CM.CountryID = item.id;
                    //    CM.CountryName = item.country_name;
                    //    cList.Add(CM);
                    //}
                    CLM.countryList = GetCountryList;
                }
                else
                {
                    message = string.Format("Country List is Blank");
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
                return Request.CreateResponse(HttpStatusCode.OK, CLM.countryList);
            }
        }
        #endregion
    }
}
