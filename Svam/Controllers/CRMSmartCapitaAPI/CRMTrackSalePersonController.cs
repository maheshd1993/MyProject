using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMTrackSalePersonController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        /// <summary>
        /// POST : Track Sale Person
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Address"></param>
        /// <param name="Country"></param>
        /// <param name="StateName"></param>
        /// <param name="CityName"></param>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post(APITrackSalePerson value)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            //string Token = string.Empty;

            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(Convert.ToInt32(value.CompanyID), value.Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            try
            {
                int? UserID = Convert.ToInt32(value.UserID);
                var user = db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefault();
                var dt = Constant.GetimeForApi(Convert.ToInt32(user.CompanyID));
                if (UserID > 0 && user!=null)
                {
                    crm_tracksaleperson tsp = new crm_tracksaleperson();
                    tsp.UserID = UserID;
                    tsp.Address = value.Address;
                    tsp.Country = value.CountryName;
                    tsp.StateName = value.StateName;
                    tsp.CityName = value.CityName;
                    tsp.Latitude = value.Latitude;
                    tsp.Longitude = value.Longitude;
                    tsp.TrackDatetime = dt;
                    db.crm_tracksaleperson.Add(tsp);
                    db.SaveChanges();
                    SuccessMessage = string.Format(" Track sale person succesfully.");
                }
                else
                {
                    SuccessMessage = "** No Record Found **";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(Int32? UserID, string SearchDate,int CompanyID,string Token)
        {
            String Records = String.Empty;
            string ErrorMessage = string.Empty;
            var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            List<APITrackSalePerson> ApiTrackList = new List<APITrackSalePerson>();
            try
            {
                if (UserID > 0)
                {
                    var GetTrackList = db.crm_tracksaleperson.Where(em => em.UserID == UserID).OrderByDescending(em => em.TrackDatetime).ToList();
                    if (!string.IsNullOrEmpty(SearchDate))
                    {
                        if (GetTrackList.Count > 0)
                        {
                            SearchDate = SearchDate.Trim('"');
                            DateTime dtsearch = DateTime.ParseExact(SearchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                            GetTrackList = GetTrackList.Where(em => em.TrackDatetime.Value.Date == dtsearch.Date).ToList();
                            foreach (var item in GetTrackList)
                            {
                                APITrackSalePerson ATrack = new APITrackSalePerson();
                                ATrack.Latitude = item.Latitude;
                                ATrack.Longitude = item.Longitude;
                                ApiTrackList.Add(ATrack);
                            }
                            if (GetTrackList.Count < 1)
                            {
                                Records = "** No Record Found **";
                            }
                        }
                        else
                        {
                            Records = "** No Record Found **";
                        }
                    }
                    else
                    {
                        if (GetTrackList.Count > 0)
                        {
                            DateTime dt = Constant.GetimeForApi(CompanyID);
                            GetTrackList = GetTrackList.Where(em => em.TrackDatetime.Value.Date == dt.Date).ToList();
                            foreach (var item in GetTrackList)
                            {
                                APITrackSalePerson ATrack = new APITrackSalePerson();
                                ATrack.Latitude = item.Latitude;
                                ATrack.Longitude = item.Longitude;
                                ApiTrackList.Add(ATrack);
                            }
                            if (GetTrackList.Count < 1)
                            {
                                Records = "** No Record Found **";
                            }
                        }
                        else
                        {
                            Records = "** No Record Found **";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                Records = ErrorMessage;
                ExceptionLogging.SendExcepToDB(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ErrorMessage);
            }

            if (ApiTrackList.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiTrackList);
            }
            else if (!string.IsNullOrEmpty(Records) && Records == "** No Record Found **")
            {
                return Request.CreateResponse(HttpStatusCode.OK, Records);
            }
            else if (!string.IsNullOrEmpty(Records) && Records != "** No Record Found **")
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, ApiTrackList);
            }
        }
    }
}