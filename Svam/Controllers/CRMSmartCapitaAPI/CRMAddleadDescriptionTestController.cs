using Newtonsoft.Json;
using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMAddleadDescriptionTestController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        [HttpPost]
        public async Task<HttpResponseMessage> Post(HttpRequestMessage httpmessage/*[FromBody]JToken postData*/)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                //AddDescTestModel value = JsonConvert.DeserializeObject<AddDescTestModel>(postData.ToString());

                #region this code for upload data in key value pair form-data with file
                NameValueCollection collection = HttpContext.Current.Request.Form;

                var items = collection.AllKeys.SelectMany(collection.GetValues, (k, v) => new { key = k, value = v });

                //We just collect your multiple form data key/value pair in this dictinary
                //The following code will be replaced by yours
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                var sb = new StringBuilder();
                sb.Append("{");//append open curlly braces in string builder for make json data
                               //string removeLastcomma = string.Empty;
                foreach (var item in items)
                {
                    keyValuePairs.Add(item.key, item.value);
                    sb.AppendFormat("\"{0}\" : \"{1}\",", item.key, item.value);
                }
                sb.Append("}");//append close curlly braces in string builder for make json data
                               //int index = sb.ToString().LastIndexOf(',');
                               //if(index>0)
                               //{
                               //    removeLastcomma= sb.ToString().Remove(index, 1);
                               //}
                var jsonData = sb.ToString();
                AddDescTestModel value = JsonConvert.DeserializeObject<AddDescTestModel>(jsonData);
                #endregion
                int branchID = Convert.ToInt32(value.BranchID);
                int companyID = Convert.ToInt32(value.CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, value.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                

                DateTime localTime = Constant.GetimeForApi(companyID);
                var CurrentDate = localTime.ToString("dd/MM/yyyy");



                if (value.LeadID != null)
                {
                    string LeadStatusName = string.Empty;

                    var cl = await db.crm_createleadstbl.Where(em => em.Id == value.LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                    if (cl != null)
                    {
                        DateTime PreFolloupDate = Convert.ToDateTime(cl.FollowDate);
                        //cl.Id = Convert.ToInt32(LeadID);
                        //if (!String.IsNullOrWhiteSpace(FollowUpDate))
                        //{
                        //    cl.FollowDate = DateTime.ParseExact(FollowUpDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        //}
                        if (!string.IsNullOrWhiteSpace(value.FollowUpDate))
                        {
                            if (Convert.ToDateTime(value.FollowUpDate).Date < localTime.Date)
                            {
                                ErrorMessage = "Can't select previous follow up date on current day.";
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            cl.FollowDate = Convert.ToDateTime(value.FollowUpDate);
                            //GetLeadsData.FollowDate = Convert.ToDateTime(FollowUpDate);
                            if (!string.IsNullOrEmpty(cl.FollowUpTime))
                            {
                                var finalDateTime = string.Format("{0} {1}", value.FollowUpDate, cl.FollowUpTime);
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
                        if (value.LeadStatusID != null && value.LeadStatusID > 0)
                        {
                            LeadStatusName = db.crm_leadstatus_tbl.Where(a => a.Id == value.LeadStatusID).FirstOrDefault().LeadStatusName;
                        }


                        string FileFullName = string.Empty;
                        string FileName = string.Empty;

                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        var httpRequest = HttpContext.Current.Request;
                        if (httpRequest.Files.Count > 0)
                        {
                            foreach (string file in httpRequest.Files)
                            {
                                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                                var postedFile = httpRequest.Files[file];
                                if (postedFile != null && postedFile.ContentLength > 0)
                                {
                                    int MaxContentLength = 1024 * 1024 * 5; //Size = 1 MB  

                                    IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                                    //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                    var ext = Path.GetExtension(postedFile.FileName);
                                    var extension = ext.ToLower();
                                    if (!AllowedFileExtensions.Contains(extension))
                                    {
                                        var message = string.Format("Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
                                        dict.Add("error", message);
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                    }
                                    else if (postedFile.ContentLength > MaxContentLength)
                                    {
                                        var message = string.Format("Please Upload a file upto 5 mb.");
                                        dict.Add("error", message);
                                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                                    }
                                    else
                                    {
                                        //var CLM =await db.crm_createleadstbl.Where(em => em.Id == value.LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                                        FileName = "Lead-" + value.UserName + "-" + cl.Customer + "-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + "";
                                        FileFullName = FileName + extension;
                                        var filePath = HttpContext.Current.Server.MapPath("~/LeadAttachment/" + FileName + extension);
                                        postedFile.SaveAs(filePath);
                                    }
                                }
                            }
                        }

                        crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl();
                        LD.Description = value.txtDescription;
                        LD.LeadId = value.LeadID;
                        LD.Date = CurrentDate;
                        LD.ByUID = value.UID;
                        LD.ByUserName = value.UserName;
                        LD.CreatedDateTime = localTime;
                        LD.BranchID = branchID;
                        LD.CompanyID = companyID;
                        LD.LeadAttachment = FileFullName;
                        LD.LeadStatusName = LeadStatusName;
                        db.crm_leaddescriptiontbl.Add(LD);

                        //update lead delayed missed followup detail
                        //DateTime PreFolloupDate = Convert.ToDateTime(cl.FollowDate);
                        if (!string.IsNullOrWhiteSpace(value.FollowUpDate))
                        {
                            var existRecord =await db.crm_delayedfollowuprecordtbl.Where(em => em.LeadId == cl.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                            if (Convert.ToDateTime(value.FollowUpDate).Date > PreFolloupDate.Date && existRecord != null)
                            {
                                existRecord.PreFollowUpDate = PreFolloupDate.Date;
                                existRecord.CreatedDate = localTime.Date;
                                existRecord.CreatedDatetime = localTime;
                                //db.SaveChanges();
                            }
                            else
                            {
                                crm_delayedfollowuprecordtbl dfr = new crm_delayedfollowuprecordtbl();
                                if (Convert.ToDateTime(value.FollowUpDate).Date > PreFolloupDate.Date)
                                {
                                    dfr.CreatedBy = Convert.ToInt32(value.UID);
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
                        if (value.LeadStatusID != null && value.LeadStatusID > 0)
                        {
                            cl.LeadStatusID = value.LeadStatusID;
                        }
                       await db.SaveChangesAsync();
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
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters");

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
    }
}