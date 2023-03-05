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
    public class CRMStateController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get List of State
        /// <summary>
        /// Get List of State List based on country ID
        /// GET api/CRMState/?CountryID=123
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string CountryID)
        {
            List<ManageStateModel> stateList = new List<ManageStateModel>();
            string message = string.Empty;
            try
            {
                //string query = @"select ID as StateID, SUBSTRING(State, 4) AS StateName from com_state where Country="+ CountryID + "";
                //var GetStateList = db.Database.SqlQuery<ManageStateModel>(query).ToList();
                if(CountryID=="0")
                {
                    stateList.Add(new ManageStateModel { StateID = 0, StateName = "Select State" });
                }
                else
                {
                    var GetStateList = db.com_state.Where(em => em.Country == CountryID).Where(a=>a.ID!=0).ToList();
                    if (GetStateList.Count > 0)
                    {
                        //GetStateList.Insert(0, new ManageStateModel { StateID = 0, StateName = "Select State" });

                        stateList.Add(new ManageStateModel { StateID = 0, StateName = "Select State" });

                        foreach (var item in GetStateList)
                        {
                            ManageStateModel SM = new ManageStateModel();
                            int dashIndex = 0;
                            string stateName = string.Empty;

                            SM.StateID = item.ID;

                            dashIndex = item.State.IndexOf('-');//get index of fist dash
                            stateName = dashIndex > 0 ? item.State.Substring(dashIndex + 1) : item.State;//if dash index >0 then get state name after dash otherwise full state name

                            //SM.StateName = CountryID == "1" ? item.State.Substring(3) : item.State;
                            SM.StateName = stateName;
                            stateList.Add(SM);
                        }
                        //stateList = GetStateList;
                    }
                    else
                    {
                        message = string.Format("State List is Blank");
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
                return Request.CreateResponse(HttpStatusCode.OK, stateList);
            }
        }
        #endregion
    }
}
