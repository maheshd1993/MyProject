using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMAddLeadDescriptionController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Add Description based on Lead and CompanyID,BranchID
        /// <summary>
        /// Add Description based on Lead and CompanyID,BranchID
        /// Post:api/CRMAddLeadDescription
        /// </summary>
        /// <param name="LeadID"></param>
        /// <param name="UID"></param>
        /// <param name="UserName"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="txtDescription"></param>
        /// <param name="Postfile"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Post(int? LeadID, int? UID, string UserName, string CompanyID, string BranchID, string txtDescription, string FollowUpDate, int? LeadStatusID,string Token)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            try
            {
                Int32 branchID = Convert.ToInt32(BranchID);
                Int32 companyID = Convert.ToInt32(CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth= Utility.TokenVerify(companyID,Token);//verify token for is authorized user

                if(auth==false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                DateTime localTime = Constant.GetimeForApi(companyID);
                
                //DateTime utcTime = DateTime.UtcNow;
                //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                var CurrentDate = localTime.ToString("dd/MM/yyyy");

                if (LeadID != null)
                {
                    string LeadStatusName = string.Empty;
                    var cl =await db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                    if (cl != null)
                    {
                        DateTime PreFolloupDate = Convert.ToDateTime(cl.FollowDate);
                        //cl.Id = Convert.ToInt32(LeadID);
                        //if (!String.IsNullOrWhiteSpace(FollowUpDate))
                        //{
                        //    cl.FollowDate = DateTime.ParseExact(FollowUpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //}
                        if (!string.IsNullOrWhiteSpace(FollowUpDate))
                        {
                            if (Convert.ToDateTime(FollowUpDate).Date < localTime.Date)
                            {
                                ErrorMessage = "Can't select previous follow up date on current day.";
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            cl.FollowDate = Convert.ToDateTime(FollowUpDate);
                            //GetLeadsData.FollowDate = Convert.ToDateTime(FollowUpDate);

                            if (!string.IsNullOrEmpty(cl.FollowUpTime))
                            {
                                var finalDateTime = string.Format("{0} {1}", FollowUpDate, cl.FollowUpTime);
                                DateTime dateTime = Convert.ToDateTime(finalDateTime);
                                
                                //creatleadsTbl.FollowupTimeinIST = value.FollowupTimeIST;
                                cl.ZoneName = (!string.IsNullOrEmpty(cl.ZoneName) && !cl.ZoneName.ToLower().Contains("Select")) ? cl.ZoneName : Constant.GetCompanyTimeZone(companyID);
                                var dateFormat = Constant.DateFormatForApi(companyID);
                                var followUpDateForIst = string.Format("{0:" + dateFormat + "}", cl.FollowDate);
                                var istTime = Constant.GetFollowupTimeInIST(cl.ZoneName, followUpDateForIst, cl.FollowUpTime, companyID);
                                if (!string.IsNullOrEmpty(istTime))
                                {
                                    var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                                    //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                                    cl.ConvertedFupDateTime = cDate;
                                }
                                //else
                                //{
                                //    var td = localTime;
                                //    cl.ZoneName = (!string.IsNullOrEmpty(cl.ZoneName) && !cl.ZoneName.ToLower().Contains("Select")) ? cl.ZoneName : Constant.GetCompanyTimeZone(companyID);
                                //    cl.ConvertedFupDateTime = td;
                                //    //cl.FollowUpTime = td.ToString("hh:mm tt"); //set followup time                               
                                //}
                            }
                            //else
                            //{
                              
                            //    DateTime dateTime = Convert.ToDateTime(cl.FollowDate);
                           
                            //    cl.ZoneName = (!string.IsNullOrEmpty(cl.ZoneName) && !cl.ZoneName.ToLower().Contains("Select")) ? cl.ZoneName : Constant.GetCompanyTimeZone(companyID);
                            //    var dateFormat = Constant.DateFormatForApi(companyID);
                            //    var followUpDateForIst = string.Format("{0:" + dateFormat + "}", dateTime);
                            //    cl.FollowUpTime = localTime.ToString("hh:mm tt"); //set followup time 
                            //    var istTime = Constant.GetFollowupTimeInIST(cl.ZoneName, followUpDateForIst, cl.FollowUpTime, companyID);
                            //    if (!string.IsNullOrEmpty(istTime))
                            //    {
                            //        var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                            //        //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                            //        cl.ConvertedFupDateTime = cDate;
                            //    }
                            //    else
                            //    {
                            //        var td = localTime;
                            //        cl.ZoneName = (!string.IsNullOrEmpty(cl.ZoneName) && !cl.ZoneName.ToLower().Contains("Select")) ? cl.ZoneName : Constant.GetCompanyTimeZone(companyID);
                            //        cl.ConvertedFupDateTime = td;
                            //        cl.FollowUpTime = td.ToString("hh:mm tt"); //set followup time                               
                            //    }
                            //}
                        }
                        
                        if (LeadStatusID != null && LeadStatusID > 0)
                        {
                            LeadStatusName = db.crm_leadstatus_tbl.Where(a => a.Id == LeadStatusID).FirstOrDefault().LeadStatusName;
                        }

                        
                        crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                        LD.Description = txtDescription;
                        LD.LeadId = LeadID;
                        LD.Date = CurrentDate;
                        LD.ByUID = UID;
                        LD.ByUserName = UserName;
                        LD.CreatedDateTime = localTime;
                        LD.BranchID = branchID;
                        LD.CompanyID = companyID;
                        LD.LeadAttachment = null;
                        LD.LeadStatusName = LeadStatusName;
                        db.crm_leaddescriptiontbl.Add(LD);

                        //update lead delayed missed followup detail
                        //DateTime PreFolloupDate = Convert.ToDateTime(cl.FollowDate);
                        if (!string.IsNullOrWhiteSpace(FollowUpDate))
                        {
                            var existRecord =await db.crm_delayedfollowuprecordtbl.Where(em => em.LeadId == cl.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                            if (Convert.ToDateTime(FollowUpDate).Date > PreFolloupDate.Date && existRecord != null)
                            {
                                existRecord.PreFollowUpDate = PreFolloupDate.Date;
                                existRecord.CreatedDate = localTime.Date;
                                existRecord.CreatedDatetime = localTime;
                                //db.SaveChanges();
                            }
                            else
                            {
                                crm_delayedfollowuprecordtbl dfr = new crm_delayedfollowuprecordtbl();
                                if (Convert.ToDateTime(FollowUpDate).Date > PreFolloupDate.Date)
                                {
                                    dfr.CreatedBy = Convert.ToInt32(UID);
                                    dfr.CreatedDate = localTime.Date;
                                    dfr.CreatedDatetime = localTime;
                                    dfr.PreFollowUpDate = PreFolloupDate.Date;
                                    dfr.LeadId = Convert.ToInt32(cl.Id);
                                    dfr.BranchID = branchID;
                                    dfr.CompanyID = companyID;
                                    db.crm_delayedfollowuprecordtbl.Add(dfr);
                                }

                            }
                        }

                        cl.ModifiedDate = localTime;
                        if (LeadStatusID != null && LeadStatusID > 0)
                        {
                            cl.LeadStatusID = LeadStatusID;
                        }
                        db.SaveChanges();
                    }
                    SuccessMessage = string.Format("Description added succesfully.");
                }
                else
                {
                    ErrorMessage = "Please enter leadid";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters");
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

        #endregion
    }
}
