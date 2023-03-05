using Newtonsoft.Json;
using Svam.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Svam.EF;
using Svam.Repository;
using Svam.UtilityManager;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Traders.Models;
using System.Data.Entity;

namespace Svam.Controllers.TestApis
{
    public class CRMCreateLeadKeyValueController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        CommonRepository cr = new CommonRepository();
        #endregion
        //api/CRMCreateLeadKeyValue/PostKeyValueData
        [HttpPost]
        public async Task<HttpResponseMessage> PostKeyValueData()
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }


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
            APILeadModel value = JsonConvert.DeserializeObject<APILeadModel>(jsonData);
            #endregion

            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {


                    Int32 branchID = Convert.ToInt32(value.BranchID);
                    Int32 companyID = Convert.ToInt32(value.CompanyID);

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

                    string MobileNumber = value.MobileNo.Replace("+91", "");

                    int? CRMCustomerID = 0;
                    int? LeadID = value.LeadID;
                    int? UID = value.UID;

                    DateTime localTime = Constant.GetimeForApi(companyID);
                    if (LeadID > 0)
                    {
                        #region Update-Leads
                        var GetLeads =await db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                        //var GetLeads = db.crm_createleadstbl.Find(LeadID);
                        int BranchID = branchID;
                        int CompanyID = companyID;
                        if (GetLeads != null)
                        {
                            var date = localTime.ToString("dd/MM/yyyy");
                            //var CheckExistDescript = db.crm_leaddescriptiontbl.Where(em => em.LeadId == LeadID && em.Date == date && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();                        
                            //if (CheckExistDescript != null)
                            if (!string.IsNullOrEmpty(GetLeads.AssignTo))
                            {
                                var AssignTo = Convert.ToInt32(GetLeads.AssignTo);
                                var data = cr.GetUserCompanyBranch(AssignTo);
                                if (data != null)
                                {
                                    BranchID = data.BranchID;
                                    CompanyID = data.CompanyID;
                                }
                            }
                            if (!string.IsNullOrEmpty(value.Description))
                            {

                                string MobileNo = value.MobileNo.Substring(value.MobileNo.Length - 9, 9);//get last line digits 
                                //check for exist mobile
                                if (db.crm_createleadstbl.Any(em => em.Id != value.LeadID && em.CompanyID == companyID && em.BranchID == branchID && (!string.IsNullOrEmpty(em.MobileNo) && em.MobileNo.Substring(em.MobileNo.Length - 9, 9) == MobileNo)))
                                {
                                    ErrorMessage = string.Format("Same Mobile Number Already Exists,Please Try another Mobile number.");
                                    HttpError err = new HttpError(ErrorMessage);
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                                }
                                //check for exist emailid
                                if (!string.IsNullOrEmpty(value.EmailId) && db.crm_createleadstbl.Any(em => em.Id != value.LeadID && em.CompanyID == companyID && em.BranchID == branchID && em.EmailId == value.EmailId))
                                {
                                    ErrorMessage = string.Format("Email-Id Already Exists,Please Try another email id.");
                                    HttpError err = new HttpError(ErrorMessage);
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                                }

                                CRMCustomerID = value.LeadID;
                                #region Delayed Lead Record
                                if (!String.IsNullOrEmpty(GetLeads.FollowDate.Value.ToShortDateString()))
                                {
                                    DateTime PreFolloupDate = Convert.ToDateTime(GetLeads.FollowDate);
                                    var existRecord =await db.crm_delayedfollowuprecordtbl.Where(em => em.LeadId == CRMCustomerID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                                    if (Convert.ToDateTime(value.FollowDate).Date > PreFolloupDate.Date && existRecord != null)
                                    {
                                        existRecord.PreFollowUpDate = PreFolloupDate.Date;
                                        existRecord.CreatedDate = localTime.Date;
                                        existRecord.CreatedDatetime = localTime;
                                        db.SaveChanges();
                                    }
                                    else
                                    {

                                        crm_delayedfollowuprecordtbl dfr = new crm_delayedfollowuprecordtbl();
                                        if (Convert.ToDateTime(value.FollowDate).Date > PreFolloupDate.Date)
                                        {
                                            dfr.CreatedBy = UID;
                                            dfr.CreatedDate = localTime.Date;
                                            dfr.CreatedDatetime = localTime;
                                            dfr.PreFollowUpDate = PreFolloupDate.Date;
                                            dfr.LeadId = Convert.ToInt32(GetLeads.Id);
                                            dfr.BranchID = branchID;
                                            dfr.CompanyID = companyID;
                                            db.crm_delayedfollowuprecordtbl.Add(dfr);
                                        }
                                    }                                    
                                }
                                #endregion

                                #region Update-Lead
                                GetLeads.LeadStatusID = value.LeadStatusID;
                                GetLeads.LeadStatus = value.LeadStatusName;
                                if (value.ProductTypeID > 0)
                                {
                                    GetLeads.ProductTypeID = value.ProductTypeID;
                                    GetLeads.ProductTypeName = value.ProductTypeName;
                                }
                                if (value.LeadSourceID > 0)
                                {
                                    GetLeads.LeadSourceID = value.LeadSourceID;
                                    GetLeads.LeadResource = value.LeadSourceName;
                                }

                                GetLeads.Customer = value.Customer;
                                if (!string.IsNullOrEmpty(value.Designation))
                                {
                                    GetLeads.Designation = value.Designation;
                                }
                                GetLeads.MobileNo = value.MobileNo.Trim();
                                if (!string.IsNullOrEmpty(value.EmailId))
                                {
                                    GetLeads.EmailId = value.EmailId;
                                }
                                if (!string.IsNullOrEmpty(value.DOB))
                                {
                                    GetLeads.DateofBirth = value.DOB;
                                }
                                if (!string.IsNullOrEmpty(value.MrgAnniversary))
                                {
                                    GetLeads.MarriageAnniversary = value.MrgAnniversary;
                                }
                                if (!string.IsNullOrEmpty(value.OrganizationName))
                                {
                                    GetLeads.OrganizationName = value.OrganizationName;
                                }
                                if (!string.IsNullOrEmpty(value.Description))
                                {
                                    GetLeads.Description = value.Description;
                                }
                                if (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select"))
                                {
                                    GetLeads.ZoneName = value.TimeZoneName;
                                }
                                if (!string.IsNullOrWhiteSpace(value.FollowDate))
                                {
                                    //GetLeads.FollowDate = DateTime.ParseExact(value.FollowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    GetLeads.FollowDate = Convert.ToDateTime(value.FollowDate);
                                }
                                //if (!string.IsNullOrEmpty(value.FollowupTime))
                                //{
                                //    GetLeads.FollowUpTime = value.FollowupTime;
                                //}
                                //if (!string.IsNullOrEmpty(value.FollowupTimeIST))
                                //{
                                //    GetLeads.FollowupTimeinIST = value.FollowupTimeIST;
                                //}

                                if (!string.IsNullOrEmpty(value.FollowupTime))
                                {
                                    var finalDateTime = string.Format("{0} {1}", value.FollowDate, value.FollowupTime);
                                    DateTime dateTime = Convert.ToDateTime(finalDateTime);
                                    GetLeads.FollowUpTime = value.FollowupTime;
                                    //creatleadsTbl.FollowupTimeinIST = value.FollowupTimeIST;
                                    GetLeads.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                                    var dateFormat = Constant.DateFormatForApi(companyID);
                                    var followUpDateForIst = string.Format("{0:" + dateFormat + "}", GetLeads.FollowDate);
                                    var istTime = Constant.GetFollowupTimeInIST(GetLeads.ZoneName, followUpDateForIst, value.FollowupTime, companyID);
                                    if (!string.IsNullOrEmpty(istTime))
                                    {
                                        var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                                        //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                                        GetLeads.ConvertedFupDateTime = cDate;
                                    }
                                    //else
                                    //{
                                    //    var td = localTime;
                                    //    GetLeads.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                                    //    GetLeads.ConvertedFupDateTime = td;
                                    //    // GetLeads.FollowUpTime = td.ToString("hh:mm tt"); //set followup time                               
                                    //}

                                }
                                //else
                                //{
                                //    //var finalDateTime = string.Format("{0} {1}", value.FollowDate, value.FollowupTime);
                                //    DateTime dateTime = Convert.ToDateTime(value.FollowDate);
                                //    //creatleadsTbl.FollowupTimeinIST = value.FollowupTimeIST;
                                //    GetLeads.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                                //    var dateFormat = Constant.DateFormatForApi(companyID);
                                //    var followUpDateForIst = string.Format("{0:" + dateFormat + "}", dateTime);
                                //    GetLeads.FollowUpTime = localTime.ToString("hh:mm tt"); //set followup time 
                                //    var istTime = Constant.GetFollowupTimeInIST(GetLeads.ZoneName, followUpDateForIst, GetLeads.FollowUpTime, companyID);
                                //    if (!string.IsNullOrEmpty(istTime))
                                //    {
                                //        var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                                //        //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                                //        GetLeads.ConvertedFupDateTime = cDate;
                                //    }
                                //    else
                                //    {
                                //        var td = localTime;
                                //        GetLeads.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                                //        GetLeads.ConvertedFupDateTime = td;
                                //        GetLeads.FollowUpTime = td.ToString("hh:mm tt"); //set followup time                               
                                //    }
                                //}



                                if (value.CountryID != null)
                                {
                                    GetLeads.CountryID = value.CountryID;
                                    GetLeads.Country = value.Country;
                                }
                                if (value.CityID != null)
                                {
                                    GetLeads.CityID = value.CityID;
                                    GetLeads.City = value.City;
                                }
                                if (value.StateID != null)
                                {
                                    GetLeads.StateID = value.StateID;
                                    GetLeads.State = value.State;
                                }
                                if (!string.IsNullOrEmpty(value.Address))
                                {
                                    GetLeads.Address = value.Address;
                                }
                                GetLeads.ModifiedDate = localTime;
                                GetLeads.BranchID = branchID;
                                GetLeads.CompanyID = companyID;
                                if (!string.IsNullOrEmpty(value.ExpectedDate))
                                {
                                    GetLeads.ExpectedDate = Convert.ToDateTime(value.ExpectedDate);
                                }

                                if (!string.IsNullOrEmpty(value.ExpectedProductAmount))
                                {
                                    GetLeads.ExpectedProductAmount = Convert.ToDecimal(value.ExpectedProductAmount);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol1))
                                {
                                    GetLeads.ExtraCol1 = value.ExtraCol1;
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol2))
                                {
                                    GetLeads.ExtraCol2 = value.ExtraCol2;
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol3))
                                {
                                    GetLeads.ExtraCol3 = value.ExtraCol3;
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol4))
                                {
                                    GetLeads.ExtraCol4 = value.ExtraCol4;
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol5))
                                {
                                    GetLeads.ExtraCol5 = value.ExtraCol5;
                                }

                                if (!string.IsNullOrEmpty(value.ExtraCol6))
                                {
                                    GetLeads.ExtraCol6 = Convert.ToDecimal(value.ExtraCol6);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol7))
                                {
                                    GetLeads.ExtraCol7 = Convert.ToInt32(value.ExtraCol7);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol8))
                                {
                                    GetLeads.ExtraCol8 = Convert.ToInt32(value.ExtraCol8);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol9))
                                {
                                    GetLeads.ExtraCol9 = Convert.ToDateTime(value.ExtraCol9);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol10))
                                {
                                    GetLeads.ExtraCol10 = Convert.ToDateTime(value.ExtraCol10);
                                }
                                GetLeads.ExtraCol11 = !string.IsNullOrEmpty(value.ExtraCol11) ? Convert.ToDecimal(value.ExtraCol11) : 0;
                                GetLeads.ExtraCol12 = !string.IsNullOrEmpty(value.ExtraCol12) ? Convert.ToDecimal(value.ExtraCol12) : 0;
                                GetLeads.ExtraCol13 = !string.IsNullOrEmpty(value.ExtraCol13) ? Convert.ToDecimal(value.ExtraCol13) : 0;
                                GetLeads.ExtraCol14 = !string.IsNullOrEmpty(value.ExtraCol14) ? Convert.ToDecimal(value.ExtraCol14) : 0;
                                GetLeads.ExtraCol15 = !string.IsNullOrEmpty(value.ExtraCol15) ? Convert.ToInt32(value.ExtraCol15) : 0;
                                GetLeads.ExtraCol16 = !string.IsNullOrEmpty(value.ExtraCol16) ? Convert.ToInt32(value.ExtraCol16) : 0;
                                GetLeads.ExtraCol17 = !string.IsNullOrEmpty(value.ExtraCol17) ? Convert.ToInt32(value.ExtraCol17) : 0;
                                if (!string.IsNullOrEmpty(value.ExtraCol18))
                                {
                                    GetLeads.ExtraCol18 = Convert.ToDateTime(value.ExtraCol18);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol19))
                                {
                                    GetLeads.ExtraCol19 = Convert.ToDateTime(value.ExtraCol19);
                                }
                                if (!string.IsNullOrEmpty(value.ExtraCol20))
                                {
                                    GetLeads.ExtraCol20 = Convert.ToDateTime(value.ExtraCol20);
                                }

                                #endregion
                                db.SaveChanges();
                               
                                #region Add-Lead-Description and Attachment file
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
                                                var message = string.Format("** Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
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
                                                //var CLM = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                                                FileName = "Lead-" + value.UserName + "-" + GetLeads.Customer + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                                FileFullName = FileName + extension;
                                                var filePath = HttpContext.Current.Server.MapPath("~/LeadAttachment/" + FileFullName);
                                                postedFile.SaveAs(filePath);
                                            }
                                        }
                                    }
                                }


                                //CRMCustomerID = Convert.ToInt32(LeadID);
                                crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl
                                {
                                    LeadId = LeadID,
                                    Date = localTime.ToString("dd/MM/yyyy"),
                                    Description = value.Description,
                                    ByUID = UID,
                                    ByUserName = value.UserName,
                                    CreatedDateTime = localTime,
                                    BranchID = branchID,
                                    CompanyID = companyID,
                                    LeadAttachment = FileFullName
                                };
                                db.crm_leaddescriptiontbl.Add(LD);
                                await db.SaveChangesAsync();
                                #endregion

                                trans.Commit();
                                SuccessMessage = string.Format("Leads updated successfully");
                                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);

                            }
                            else
                            {
                                ErrorMessage = string.Format("** Please add a description before Update");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                        }
                        else
                        {
                            ErrorMessage = string.Format("Somthing went wrong.");
                            HttpError err = new HttpError(ErrorMessage);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                        }
                        #endregion
                    }
                    else
                    {
                        string MobileNo = value.MobileNo.Substring(value.MobileNo.Length - 9, 9);//get last line digits 
                        //check for exist mobile
                        if (db.crm_createleadstbl.Any(em => em.CompanyID == companyID && em.BranchID == branchID && (!string.IsNullOrEmpty(em.MobileNo) && em.MobileNo.Substring(em.MobileNo.Length - 9, 9) == MobileNo)))
                        {
                            ErrorMessage = string.Format("Same Mobile Number Already Exists,Please Try another Mobile number.");
                            HttpError err = new HttpError(ErrorMessage);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                        }
                        //check for exist emailid
                        if (!string.IsNullOrEmpty(value.EmailId) && db.crm_createleadstbl.Any(em => em.CompanyID == companyID && em.BranchID == branchID && em.EmailId == value.EmailId))
                        {
                            ErrorMessage = string.Format("Email-Id Already Exists,Please Try another email id.");
                            HttpError err = new HttpError(ErrorMessage);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                        }

                        #region Create-Leads
                        //var samemobile = db.crm_createleadstbl.Where(em => em.CompanyID == companyID && em.BranchID == branchID && em.MobileNo == MobileNumber).FirstOrDefault();

                        crm_createleadstbl creatleadsTbl = new crm_createleadstbl();
                        creatleadsTbl.LeadOwner = UID;
                        creatleadsTbl.LeadStatusID = value.LeadStatusID;
                        creatleadsTbl.LeadStatus = value.LeadStatusName;
                        creatleadsTbl.LeadSourceID = value.LeadSourceID;
                        creatleadsTbl.LeadResource = value.LeadSourceID > 0 ? value.LeadSourceName : null;
                        creatleadsTbl.ProductTypeID = value.ProductTypeID;
                        creatleadsTbl.ProductTypeName = value.ProductTypeID > 0 ? value.ProductTypeName : null;
                        creatleadsTbl.Customer = value.Customer;
                        creatleadsTbl.Designation = value.Designation;
                        creatleadsTbl.MobileNo = value.MobileNo.Trim();

                        creatleadsTbl.EmailId = !string.IsNullOrEmpty(value.EmailId) ? value.EmailId : null;

                        creatleadsTbl.DateofBirth = value.DOB;
                        creatleadsTbl.MarriageAnniversary = value.MrgAnniversary;

                        creatleadsTbl.OrganizationName = value.OrganizationName;
                        creatleadsTbl.CountryID = value.CountryID;
                        creatleadsTbl.Country = value.Country;
                        creatleadsTbl.CityID = value.CityID;
                        creatleadsTbl.City = value.City;
                        creatleadsTbl.StateID = value.StateID;
                        creatleadsTbl.State = value.State;
                        //if (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select"))
                        //{
                        //    creatleadsTbl.ZoneName = value.TimeZoneName;
                        //}
                        if (!string.IsNullOrWhiteSpace(value.FollowDate))
                        {
                            //creatleadsTbl.FollowDate = DateTime.ParseExact(value.FollowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            creatleadsTbl.FollowDate = Convert.ToDateTime(value.FollowDate);
                        }

                        if (!string.IsNullOrEmpty(value.FollowupTime))
                        {
                            var finalDateTime = string.Format("{0} {1}", value.FollowDate, value.FollowupTime);
                            DateTime dateTime = Convert.ToDateTime(finalDateTime);
                            creatleadsTbl.FollowUpTime = value.FollowupTime;
                            //creatleadsTbl.FollowupTimeinIST = value.FollowupTimeIST;
                            creatleadsTbl.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                            var dateFormat = Constant.DateFormatForApi(companyID);
                            var followUpDateForIst = string.Format("{0:" + dateFormat + "}", creatleadsTbl.FollowDate);
                            var istTime = Constant.GetFollowupTimeInIST(creatleadsTbl.ZoneName, followUpDateForIst, value.FollowupTime, companyID);
                            if (!string.IsNullOrEmpty(istTime))
                            {
                                var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                                //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                                creatleadsTbl.ConvertedFupDateTime = cDate;
                            }
                            //else
                            //{
                            //    var td = localTime;
                            //    creatleadsTbl.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                            //    creatleadsTbl.ConvertedFupDateTime = td;
                            //    creatleadsTbl.FollowUpTime = td.ToString("hh:mm tt"); //set followup time                               
                            //}

                        }
                        //else
                        //{
                        //    //var finalDateTime = string.Format("{0} {1}", value.FollowDate, value.FollowupTime);
                        //    DateTime dateTime = Convert.ToDateTime(value.FollowDate);
                        //    //creatleadsTbl.FollowupTimeinIST = value.FollowupTimeIST;
                        //    creatleadsTbl.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                        //    var dateFormat = Constant.DateFormatForApi(companyID);
                        //    var followUpDateForIst = string.Format("{0:" + dateFormat + "}", dateTime);
                        //    creatleadsTbl.FollowUpTime = localTime.ToString("hh:mm tt"); //set followup time 
                        //    var istTime = Constant.GetFollowupTimeInIST(creatleadsTbl.ZoneName, followUpDateForIst, creatleadsTbl.FollowUpTime, companyID);
                        //    if (!string.IsNullOrEmpty(istTime))
                        //    {
                        //        var cDate = DateTime.ParseExact(istTime, "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(istTime);
                        //        //GetLeadsData.FollowupTimeinIST = string.Format("{0:dd/MM/yyyy hh:mm tt}", cDate);
                        //        creatleadsTbl.ConvertedFupDateTime = cDate;
                        //    }
                        //    else
                        //    {
                        //        var td = localTime;
                        //        creatleadsTbl.ZoneName = (!string.IsNullOrEmpty(value.TimeZoneName) && !value.TimeZoneName.ToLower().Contains("Select")) ? value.TimeZoneName : Constant.GetCompanyTimeZone(companyID);
                        //        creatleadsTbl.ConvertedFupDateTime = td;
                        //        creatleadsTbl.FollowUpTime = td.ToString("hh:mm tt"); //set followup time                               
                        //    }
                        //}


                        creatleadsTbl.Url = value.URL;
                        creatleadsTbl.SkypeId = value.SkypeId;
                        creatleadsTbl.Description = value.Description;
                        creatleadsTbl.Status = true;
                        creatleadsTbl.LeadsType = "Manual";
                        creatleadsTbl.Address = value.Address;
                        creatleadsTbl.Createddate = localTime;
                        creatleadsTbl.date = localTime.ToString("dd/MM/yyyy");
                        creatleadsTbl.BranchID = branchID;
                        creatleadsTbl.CompanyID = companyID;
                        if (!string.IsNullOrEmpty(value.ExpectedDate))
                        {
                            creatleadsTbl.ExpectedDate = Convert.ToDateTime(value.ExpectedDate);
                        }

                        if (!string.IsNullOrEmpty(value.ExpectedProductAmount))
                        {
                            creatleadsTbl.ExpectedProductAmount = Convert.ToDecimal(value.ExpectedProductAmount);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol1))
                        {
                            creatleadsTbl.ExtraCol1 = value.ExtraCol1;
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol2))
                        {
                            creatleadsTbl.ExtraCol2 = value.ExtraCol2;
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol3))
                        {
                            creatleadsTbl.ExtraCol3 = value.ExtraCol3;
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol4))
                        {
                            creatleadsTbl.ExtraCol4 = value.ExtraCol4;
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol5))
                        {
                            creatleadsTbl.ExtraCol5 = value.ExtraCol5;
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol6))
                        {
                            creatleadsTbl.ExtraCol6 = Convert.ToDecimal(value.ExtraCol6);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol7))
                        {
                            creatleadsTbl.ExtraCol7 = Convert.ToInt32(value.ExtraCol7);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol8))
                        {
                            creatleadsTbl.ExtraCol8 = Convert.ToInt32(value.ExtraCol8);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol9))
                        {
                            creatleadsTbl.ExtraCol9 = Convert.ToDateTime(value.ExtraCol9);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol10))
                        {
                            creatleadsTbl.ExtraCol10 = Convert.ToDateTime(value.ExtraCol10);
                        }
                        creatleadsTbl.ExtraCol11 = !string.IsNullOrEmpty(value.ExtraCol11) ? Convert.ToDecimal(value.ExtraCol11) : 0;
                        creatleadsTbl.ExtraCol12 = !string.IsNullOrEmpty(value.ExtraCol12) ? Convert.ToDecimal(value.ExtraCol12) : 0;
                        creatleadsTbl.ExtraCol13 = !string.IsNullOrEmpty(value.ExtraCol13) ? Convert.ToDecimal(value.ExtraCol13) : 0;
                        creatleadsTbl.ExtraCol14 = !string.IsNullOrEmpty(value.ExtraCol14) ? Convert.ToDecimal(value.ExtraCol14) : 0;
                        creatleadsTbl.ExtraCol15 = !string.IsNullOrEmpty(value.ExtraCol15) ? Convert.ToInt32(value.ExtraCol15) : 0;
                        creatleadsTbl.ExtraCol16 = !string.IsNullOrEmpty(value.ExtraCol16) ? Convert.ToInt32(value.ExtraCol16) : 0;
                        creatleadsTbl.ExtraCol17 = !string.IsNullOrEmpty(value.ExtraCol17) ? Convert.ToInt32(value.ExtraCol17) : 0;
                        if (!string.IsNullOrEmpty(value.ExtraCol18))
                        {
                            creatleadsTbl.ExtraCol18 = Convert.ToDateTime(value.ExtraCol18);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol19))
                        {
                            creatleadsTbl.ExtraCol19 = Convert.ToDateTime(value.ExtraCol19);
                        }
                        if (!string.IsNullOrEmpty(value.ExtraCol20))
                        {
                            creatleadsTbl.ExtraCol20 = Convert.ToDateTime(value.ExtraCol20);
                        }
                        db.crm_createleadstbl.Add(creatleadsTbl);
                       await db.SaveChangesAsync();

                        #region Add-Lead-Description and Attachment file
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
                                    int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

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
                                        //var CLM = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                                        FileName = "Lead-" + value.UserName + "-" + value.Customer + "-" + localTime.ToString("ddMMyyyyhhmmss") + "";
                                        FileFullName = FileName + extension;
                                        var filePath = HttpContext.Current.Server.MapPath("~/LeadAttachment/" + FileFullName);
                                        postedFile.SaveAs(filePath);
                                    }
                                }
                            }
                        }


                        var lid = creatleadsTbl.Id;
                        CRMCustomerID = Convert.ToInt32(creatleadsTbl.Id);
                        crm_leaddescriptiontbl LD = new crm_leaddescriptiontbl
                        {
                            LeadId = Convert.ToInt64(lid),
                            Date = localTime.ToString("dd/MM/yyyy"),
                            Description = value.Description,
                            ByUID = UID,
                            ByUserName = value.UserName,
                            CreatedDateTime = localTime,
                            BranchID = branchID,
                            CompanyID = companyID,
                            LeadAttachment = FileFullName
                        };
                        db.crm_leaddescriptiontbl.Add(LD);
                       await db.SaveChangesAsync();
                        #endregion

                        #region add customer in customers table
                        string sMObile = value.MobileNo.Replace("+91", "").Trim();
                        #region get total record count from customer table
                        string query = @"select count(*) as rowCount from Customers where CompanyId = " + value.CompanyID + "";

                        int rowCount = db.Database.SqlQuery<int>(query).FirstOrDefault();
                        string customerId = "CUSCRM-" + (rowCount + 1);
                        #endregion
                        //var checkCustomer = db.customers.Where(em => em.CompanyID == value.CompanyID && em.BranchCode == branchID && em.MobileNo == sMObile).FirstOrDefault();
                        if (!db.customers.Any(em => em.CompanyID == value.CompanyID && em.BranchCode == branchID && em.MobileNo == sMObile))
                        {
                            int LastSyncID = 0;
                            DataTable dtLastSync = DataAccessLayer.GetDataTable("call CRM_CustomerLastSyncID('" + value.CompanyID + "'," + value.BranchID + ")");
                            if (dtLastSync.Rows.Count > 0)
                            {
                                LastSyncID = Convert.ToInt32(dtLastSync.Rows[0]["syncid"]) + 1;
                            }
                            else
                            {
                                LastSyncID = 1;
                            }
                            customer c = new customer();
                            //c.CustomerID = GetCustomerID(Convert.ToString(value.CompanyID));
                            c.CustomerID = customerId;
                            c.CompanyID = value.CompanyID;
                            c.BranchCode = branchID;
                            c.CrmCustomerID = CRMCustomerID;
                            c.CustomerName = value.Customer;
                            c.BillingAddress = value.Address;
                            c.DeliveryAddress = value.Address;
                            c.Country = value.CountryID;
                            c.State = value.StateID;
                            c.City = value.CityID;
                            c.MobileNo = value.MobileNo.Trim();
                            c.EmailID = value.EmailId;
                            c.CreatedDate = localTime;
                            c.CreatedBy = value.UserName;
                            c.flag = "N";
                            c.SyncID = "O" + LastSyncID;
                            db.customers.Add(c);
                           await db.SaveChangesAsync();
                            //SuccessMessage = string.Format("Leads created successfully");
                        }
                        #endregion


                        trans.Commit();
                        SuccessMessage = string.Format("Leads created successfully");
                        return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
                        #endregion
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the Post Data(Format) Parameters **");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
        }

       
        //[HttpPost]
        //public HttpResponseMessage PostKeyValueData()
        //{
        //    if (!Request.Content.IsMimeMultipartContent("form-data"))
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    #region this code for upload data in key value pair form-data with file
        //    NameValueCollection collection = HttpContext.Current.Request.Form;

        //    var items = collection.AllKeys.SelectMany(collection.GetValues, (k, v) => new { key = k, value = v });

        //    //We just collect your multiple form data key/value pair in this dictinary
        //    //The following code will be replaced by yours
        //    Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        //    var sb = new StringBuilder();
        //    sb.Append("{");//append open curlly braces in string builder for make json data
        //    foreach (var item in items)
        //    {
        //        keyValuePairs.Add(item.key, item.value);
        //        sb.AppendFormat("\"{0}\" : \"{1}\",", item.key, item.value);
        //    }
        //    sb.Append("}");//append close curlly braces in string builder for make json data
        //    var jsonData = sb.ToString();
        //    APILeadModel value = JsonConvert.DeserializeObject<APILeadModel>(jsonData);
        //    int leadId = Convert.ToInt32(value.LeadID);
        //    #endregion

        //    string ErrorMessage = string.Empty;
        //    string SuccessMessage = string.Empty;

        //    var localtime = DateTime.Now;
        //    #region Add-Lead-Description and Attachment file
        //    string FileFullName = string.Empty;
        //    string FileName = string.Empty;

        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    var httpRequest = HttpContext.Current.Request;

        //    if (httpRequest.Files.Count > 0)
        //    {
        //        foreach (string file in httpRequest.Files)
        //        {
        //            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
        //            var postedFile = httpRequest.Files[file];
        //            if (postedFile != null && postedFile.ContentLength > 0)
        //            {
        //                int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

        //                IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        //                //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
        //                var ext = Path.GetExtension(postedFile.FileName);
        //                var extension = ext.ToLower();
        //                if (!AllowedFileExtensions.Contains(extension))
        //                {
        //                    var message = string.Format("Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
        //                    dict.Add("error", message);
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
        //                }
        //                else if (postedFile.ContentLength > MaxContentLength)
        //                {
        //                    var message = string.Format("Please Upload a file upto 5 mb.");
        //                    dict.Add("error", message);
        //                    return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
        //                }
        //                else
        //                {
        //                    //var CLM = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
        //                    FileName = "Lead-KeyValue-Check" + localtime.ToString("ddMMyyyyhhmmss") + "";
        //                    FileFullName = FileName + extension;
        //                    var filePath = HttpContext.Current.Server.MapPath("~/LeadAttachment/" + FileFullName);
        //                    SuccessMessage = string.Format("LeadId:{0}, {1}", leadId, "Image getting successfully");
        //                    postedFile.SaveAs(filePath);
        //                }
        //            }
        //        }
        //    }

        //    //SuccessMessage = value.LeadID > 0 ? Convert.ToString(value.LeadID) : "0";
        //    return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
        //    #endregion
        //}
    }
}
