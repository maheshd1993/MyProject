using MySql.Data.MySqlClient;
using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMAssignToUserController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Assign Leads based on user
        /// <summary>
        /// Assign Multiple Leads based on user
        /// Post :api/CRMAssignToUser
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="UserAssignTo"></param>
        /// <param name="MapperUserLeads"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post(int? LeadID, Int32? UID, string CompanyID, string BranchID, int? UserAssignTo, string Token)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);
                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user
                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                //APILeadModel LMM = new APILeadModel();
                if (UserAssignTo > 0)
                {
                    DateTime localTime = Constant.GetimeForApi(companyID);
                    if (LeadID > 0)
                    {
                        string constr = ConfigurationManager.ConnectionStrings["_ConnectionString"].ToString();
                        var con = new MySqlConnection(constr);
                        con.Open();

                        var LAS = new crm_leadassignhistorytbl
                        {
                            LeadId = Convert.ToInt64(LeadID),
                            LeadAssignBy = UID,
                            LeadAssignTo = UserAssignTo,
                            LeadAssignDate = localTime.Date.ToString("dd/MM/yyyy"),
                            LeadStatus = "",
                            CreatedDate = localTime,
                            BranchID = branchID,
                            CompanyID = companyID,
                        };
                        db.crm_leadassignhistorytbl.Add(LAS);
                        int i = db.SaveChanges();
                        if (i > 0)
                        {
                            string query = @"update crm_createleadstbl set AssignTo=@assign_To,AssignedDate=@assign_Date,AssignedBy=@assign_By where id=@lead_Id";

                            MySqlCommand m = new MySqlCommand(query, con);
                            m.Parameters.AddWithValue("@lead_Id", LeadID);
                            m.Parameters.AddWithValue("@assign_To", Convert.ToString(UserAssignTo));
                            m.Parameters.AddWithValue("@assign_Date", localTime);
                            m.Parameters.AddWithValue("@assign_By", Convert.ToString(UID));
                            m.ExecuteNonQuery();

                            //SuccessMessage = string.Format("Lead Assigned to User Succesfully");
                            SuccessMessage = string.Format("Lead Assigned Succesfully");
                            return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
                        }
                        else
                        {
                            ErrorMessage = string.Format("** Some technical error occurred! **");
                            HttpError err = new HttpError(ErrorMessage);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                        }
                    }
                    else
                    {
                        ErrorMessage = string.Format("** Please Select Lead to Assign **");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    //var leadData = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    //if (leadData != null)
                    //{
                    //    leadData.AssignTo = Convert.ToString(UserAssignTo);
                    //    //leadData.BranchID = branchID;
                    //    //leadData.CompanyID = companyID;
                    //    leadData.AssignedDate = localTime;
                    //    leadData.AssignedBy = Convert.ToString(UID);

                    //    var LAS = new crm_leadassignhistorytbl
                    //    {
                    //        LeadId = Convert.ToInt64(LeadID),
                    //        LeadAssignBy = UID,
                    //        LeadAssignTo = UserAssignTo,
                    //        LeadAssignDate = localTime.Date.ToString("dd/MM/yyyy"),
                    //        LeadStatus = Convert.ToString(leadData.LeadStatus),
                    //        CreatedDate = localTime,
                    //        BranchID = branchID,
                    //        CompanyID = companyID,
                    //    };
                    //    db.crm_leadassignhistorytbl.Add(LAS);
                    //    db.SaveChanges();

                    //    SuccessMessage = string.Format("Lead Assigned to User Succesfully");
                    //    return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
                    //}
                    //else
                    //{
                    //    ErrorMessage = string.Format("** Lead record not found **");
                    //    HttpError err = new HttpError(ErrorMessage);
                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    //}
                }
                else
                {
                    ErrorMessage = string.Format("** Please select the user to assign **");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                ExceptionLogging.SendExcepToDB(ex);
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            //if (ErrorMessage != string.Empty)
            //{
            //    HttpError err = new HttpError(ErrorMessage);
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            //}
            //else
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            //}
        }
        #endregion
    }
}
