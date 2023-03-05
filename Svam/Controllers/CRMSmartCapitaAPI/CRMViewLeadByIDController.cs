using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMViewLeadByIDController : ApiController
    { 
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        /// <summary>
        /// Get View Lead by LeadID
        /// </summary>
        /// <param name="LeadID"></param>
        /// <param name="BranchID"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage Get(int? LeadID, int? BranchID, int? CompanyID, string Token)
        {
            APILeadModel apiLead = new APILeadModel();
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
            //string Token = string.Empty;

            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(Convert.ToInt32(CompanyID), Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            try
            {
                
                if (LeadID > 0)
                {
                   string DateFormat = Constant.DateFormatForApi(Convert.ToInt32(CompanyID));//get date format by company id
                    DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_GetViewLeadById(" + LeadID + "," + BranchID + "," + CompanyID + ")");
                    if (GetRecords.Rows.Count > 0)
                    {
                        int i = 0;
                        
                        //for (int i = 0; i < GetRecords.Rows.Count; i++)
                        //{                           
                        apiLead.LeadID = Convert.ToInt32(GetRecords.Rows[i]["Id"]);
                        apiLead.Customer = Convert.ToString(GetRecords.Rows[i]["Customer"]);
                        apiLead.MobileNo = Convert.ToString(GetRecords.Rows[i]["MobileNo"]);
                        apiLead.EmailId = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                        apiLead.CountryID = GetRecords.Rows[i]["CountryID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["CountryID"]);
                        apiLead.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
                        apiLead.StateID = GetRecords.Rows[i]["StateID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["StateID"]);
                        apiLead.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
                        apiLead.CityID = GetRecords.Rows[i]["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["CityID"]);
                        apiLead.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
                        apiLead.FollowDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        apiLead.FollowupTime = Convert.ToString(GetRecords.Rows[i]["FollowUpTime"]);
                        apiLead.FollowupTimeIST = GetRecords.Rows[i]["ConvertedFupDateTime"] == DBNull.Value ? string.Empty :string.Format("{0:"+DateFormat+ " HH:mm}", Convert.ToDateTime(GetRecords.Rows[i]["ConvertedFupDateTime"])) ; //Convert.ToString(GetRecords.Rows[i]["FollowupTimeinIST"]);

                        string ZoneName = GetRecords.Rows[i]["ZoneName"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ZoneName"]);
                        if (!string.IsNullOrEmpty(ZoneName) && ZoneName!= "Select Time Zone")
                        {
                            if (ZoneName == "IST")
                            {
                                apiLead.TimeZoneName = "India Standard Time";
                              
                            }
                            else if (ZoneName == "CST")
                            {
                                apiLead.TimeZoneName = "Central Standard Time";
                            }
                            else if (ZoneName == "EST")
                            {
                                apiLead.TimeZoneName = "Eastern Standard Time";
                            }
                            else if (ZoneName == "MST")
                            {
                                apiLead.TimeZoneName = "Mountain Standard Time";
                            }
                            else if (ZoneName == "PST")
                            {
                                apiLead.TimeZoneName = "Pacific Standard Time";
                            }
                            else
                            {
                                apiLead.TimeZoneName = ZoneName;
                            }
                        }
                        apiLead.LeadStatusName = GetRecords.Rows[i]["LeadStatus"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                        apiLead.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
                        apiLead.Description= Convert.ToString(GetRecords.Rows[i]["Description"]);
                        apiLead.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                        apiLead.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwnerName"]);
                        apiLead.LeadOwnerID = Convert.ToInt32(GetRecords.Rows[i]["LeadOwner"]);
                        apiLead.createdDate = Convert.ToString(GetRecords.Rows[i]["Date"]).Replace(" 00:00:00", "")/*.Replace(" 12:00:00 AM", "")*/;
                        apiLead.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExpectedDate"]);
                        apiLead.ExpectedProductAmount = Convert.ToString(GetRecords.Rows[i]["ExpectedProductAmount"]);
                        apiLead.ProductTypeID = GetRecords.Rows[i]["ProductTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ProductTypeID"]);
                        apiLead.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
                        apiLead.LeadSourceID = GetRecords.Rows[i]["LeadSourceID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadSourceID"]);
                        apiLead.LeadSourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
                        apiLead.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
                        apiLead.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
                        apiLead.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
                        apiLead.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
                        apiLead.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
                        apiLead.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
                        apiLead.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
                        apiLead.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
                        apiLead.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
                        apiLead.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
                        apiLead.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
                        apiLead.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol6"]);
                        apiLead.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol7"]);
                        apiLead.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol8"]);
                        apiLead.ExtraCol9 = GetRecords.Rows[i]["ExtraCol9"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
                        apiLead.ExtraCol10 = GetRecords.Rows[i]["ExtraCol10"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);
                        apiLead.ExtraCol11 = GetRecords.Rows[i]["ExtraCol11"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol11"]);
                        apiLead.ExtraCol12 = GetRecords.Rows[i]["ExtraCol12"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol12"]);
                        apiLead.ExtraCol13 = GetRecords.Rows[i]["ExtraCol13"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol13"]);
                        apiLead.ExtraCol14 = GetRecords.Rows[i]["ExtraCol14"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol14"]);
                        apiLead.ExtraCol15 = GetRecords.Rows[i]["ExtraCol15"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol15"]);
                        apiLead.ExtraCol16 = GetRecords.Rows[i]["ExtraCol16"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol16"]);
                        apiLead.ExtraCol17 = GetRecords.Rows[i]["ExtraCol17"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol17"]);
                        apiLead.ExtraCol18 = GetRecords.Rows[i]["ExtraCol18"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
                        apiLead.ExtraCol19 = GetRecords.Rows[i]["ExtraCol19"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
                        apiLead.ExtraCol20 = GetRecords.Rows[i]["ExtraCol20"] == DBNull.Value ? string.Empty : Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
                        //}
                    }
                    else
                    {
                        ErrorMessage = string.Format("** No Record Found **");
                    }
                    
                }
                else
                {
                    ErrorMessage = string.Format("** No Record Found **");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("Somthing went wrong, while reading data, Please check the GET Data Format");
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, apiLead);
            }
        }


        #region old code not in work
        //public HttpResponseMessage Get(Int32? LeadID, Int32? BranchID, Int32? CompanyID, string Token)
        //{
        //    APILeadModel apiLead = new APILeadModel();
        //    string ErrorMessage = string.Empty;
        //    string SuccessMessage = string.Empty;
        //    //string Token = string.Empty;

        //    //var re = Request;
        //    //var headers = re.Headers;

        //    //if (headers.Contains("Token"))
        //    //{
        //    //    Token = headers.GetValues("Token").First();
        //    //}
        //    var auth = Utility.TokenVerify(Convert.ToInt32(CompanyID), Token);//verify token for is authorized user

        //    if (auth == false)
        //    {
        //        ErrorMessage = string.Format("** User authentication failed!");
        //        HttpError err = new HttpError(ErrorMessage);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //    try
        //    {
               
        //        if (LeadID > 0)
        //        {

        //            var GetLeadsData = (from ld in db.crm_createleadstbl
        //                                join usr in db.crm_usertbl on ld.LeadOwner equals usr.Id
        //                                join ldStatus in db.crm_leadstatus_tbl on ld.LeadStatusID equals ldStatus.Id into ld_Sts
        //                                from lStatus in ld_Sts.DefaultIfEmpty()//left join of lead status table
        //                                join ldSource in db.crm_leadsource_tbl on ld.LeadSourceID equals ldSource.Id into ld_Srs
        //                                from lSource in ld_Srs.DefaultIfEmpty()//left join of lead source table
        //                                join pType in db.crm_producttypetbl on ld.ProductTypeID equals pType.Id into p_type
        //                                from pdType in p_type.DefaultIfEmpty()//left join of product type table
        //                                join ldCountry in db.acc_countries on ld.CountryID equals ldCountry.id into ld_cont
        //                                from country in ld_cont.DefaultIfEmpty()//left join of country table
        //                                join ldState in db.com_state on ld.StateID equals ldState.ID into ld_state
        //                                from state in ld_state.DefaultIfEmpty()//left join of state table
        //                                join ldCity in db.com_city on ld.CityID equals ldCity.ID into ld_city
        //                                from city in ld_city.DefaultIfEmpty()//left join of city table
        //                                where ld.Id == LeadID
        //                                select new APILeadModel
        //                                {
        //                                    LeadID = LeadID,
        //                                    LeadOwnerID = ld.LeadOwner ?? 0,
        //                                    LeadOwner = usr.Fname + " " + usr.Lname,
        //                                    MobileNo = ld.MobileNo.Trim(),
        //                                    EmailId = ld.EmailId,
        //                                    OtherNo = ld.OtherNo,
        //                                    LeadStatusID = ld.LeadStatusID,
        //                                    LeadStatusName = lStatus != null ? lStatus.LeadStatusName : "",
        //                                    LeadSourceID = ld.LeadSourceID,
        //                                    LeadSourceName = lSource != null ? lSource.LeadsourceName : "",
        //                                    ProductTypeID = ld.ProductTypeID,
        //                                    ProductTypeName = pdType != null ? pdType.ProductTypeName : "",
        //                                    Description = ld.Description,
        //                                    Customer = ld.Customer,
        //                                    Designation = ld.Designation,
        //                                    OrganizationName = ld.OrganizationName,
        //                                    CountryID = ld.CountryID,
        //                                    Country = country != null ? country.country_name : "",
        //                                    StateID = ld.StateID,
        //                                    State = state != null ? state.State.Substring(3) : "",
        //                                    CityID = ld.CityID,
        //                                    City = city != null ? city.City : "",
        //                                    Address = ld.Address,
        //                                    FollowDate = ld.FollowDate.Value.ToString(),
        //                                    FollowupTime = ld.FollowUpTime,
        //                                    FollowupTimeIST = ld.FollowupTimeinIST,
        //                                    TimeZoneName = ld.ZoneName,
        //                                    DateofBirth = ld.DateofBirth,
        //                                    MarriageAnniversary = ld.MarriageAnniversary,
        //                                    createdDate = ld.date,
        //                                    BranchID = BranchID.ToString(),
        //                                    CompanyID = CompanyID.ToString(),
        //                                    ExpectedDate = ld.ExpectedDate != null ? ld.ExpectedDate.Value.ToString() : "",
        //                                    ExpectedProductAmount = ld.ExpectedProductAmount,
        //                                    ExtraCol1 = ld.ExtraCol1,
        //                                    ExtraCol2 = ld.ExtraCol2,
        //                                    ExtraCol3 = ld.ExtraCol3,
        //                                    ExtraCol4 = ld.ExtraCol4,
        //                                    ExtraCol5 = ld.ExtraCol5,
        //                                    ExtraCol6 = ld.ExtraCol6,
        //                                    ExtraCol7 = ld.ExtraCol7,
        //                                    ExtraCol8 = ld.ExtraCol8,
        //                                    ExtraCol9 = ld.ExtraCol9 != null ? SqlFunctions.StringConvert(ld.ExtraCol9.Value.ToString("dd/MM/yyyy")) : "",
        //                                    ExtraCol10 = ld.ExtraCol10 != null ? ld.ExtraCol10.Value.ToString("dd/MM/yyyy") : ""
        //                                }).SingleOrDefault();



        //            //var GetLeadsData = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //            if (GetLeadsData != null)
        //            {
        //                apiLead = GetLeadsData;

        //                //var GetLeadOwnerInfo = db.crm_usertbl.Where(em => em.Id == GetLeadsData.LeadOwner && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
        //                //if (GetLeadOwnerInfo != null)
        //                //{
        //                //    apiLead.LeadID = Convert.ToInt32(LeadID);
        //                //    apiLead.LeadOwnerID = Convert.ToInt32(GetLeadsData.LeadOwner);
        //                //    apiLead.LeadOwner = Convert.ToString(GetLeadOwnerInfo.Fname + " " + GetLeadOwnerInfo.Lname);
        //                //    apiLead.MobileNo = GetLeadsData.MobileNo.Trim();
        //                //    apiLead.EmailId = GetLeadsData.EmailId;
        //                //    apiLead.OtherNo = GetLeadsData.OtherNo;
        //                //    apiLead.LeadStatusID = GetLeadsData.LeadStatusID;
        //                //    apiLead.LeadStatusName = GetLeadsData.LeadStatus;
        //                //    apiLead.LeadSourceID = GetLeadsData.LeadSourceID;
        //                //    apiLead.LeadSourceName = GetLeadsData.LeadResource;
        //                //    apiLead.ProductTypeID = GetLeadsData.ProductTypeID;
        //                //    apiLead.ProductTypeName = GetLeadsData.ProductTypeName;
        //                //    apiLead.Description = GetLeadsData.Description;                           
        //                //    apiLead.Customer = GetLeadsData.Customer;
        //                //    apiLead.Designation = GetLeadsData.Designation;
        //                //    apiLead.OrganizationName = GetLeadsData.OrganizationName;                            
        //                //    apiLead.CountryID = GetLeadsData.CountryID;
        //                //    apiLead.Country = GetLeadsData.Country;
        //                //    apiLead.StateID = GetLeadsData.StateID;
        //                //    apiLead.State = GetLeadsData.State;                          
        //                //    apiLead.CityID = GetLeadsData.CityID;
        //                //    apiLead.City = GetLeadsData.City;
        //                //    apiLead.Address = Convert.ToString(GetLeadsData.Address);
        //                //    apiLead.FollowDate = GetLeadsData.FollowDate.Value.ToShortDateString();
        //                //    apiLead.FollowupTime = GetLeadsData.FollowUpTime;
        //                //    apiLead.FollowupTimeIST = GetLeadsData.FollowupTimeinIST;                                                 
        //                //    apiLead.TimeZoneName = GetLeadsData.ZoneName;
        //                //    if (!String.IsNullOrWhiteSpace(GetLeadsData.DateofBirth))
        //                //    {
        //                //        apiLead.DateofBirth = Convert.ToString(GetLeadsData.DateofBirth);
        //                //    }
        //                //    if (!String.IsNullOrWhiteSpace(GetLeadsData.MarriageAnniversary))
        //                //    {
        //                //        apiLead.MarriageAnniversary = Convert.ToString(GetLeadsData.MarriageAnniversary);
        //                //    }
        //                //    apiLead.createdDate = GetLeadsData.date;
        //                //    apiLead.BranchID = Convert.ToString(BranchID);
        //                //    apiLead.CompanyID = Convert.ToString(CompanyID);
        //                //}
        //            }
        //        }
        //        else
        //        {
        //            ErrorMessage = string.Format("** No Record Found **");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = string.Format("Somthing went wrong, while reading data, Please check the GET Data Format");
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }

        //    if (ErrorMessage != string.Empty)
        //    {
        //        HttpError err = new HttpError(ErrorMessage);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, apiLead);
        //    }
        //}


        #endregion
    }
}