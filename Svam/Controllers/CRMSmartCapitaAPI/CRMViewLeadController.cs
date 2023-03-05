using Svam.EF;
using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMViewLeadController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        #region Get the List of Lead based on user login
        /// <summary>
        /// Get the List of Lead based on user login
        /// GET:api/CRMViewLead
        /// </summary>
        /// <param name="ProfileName"></param>
        /// <param name="FilterDateMode"></param>
        /// <param name="FilterStatus"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="LoginUserID"></param>
        /// <param name="SearchUserID"></param>
        /// <returns></returns>
        /// string ProfileName,string FilterDateMode,string FilterStatus,string CompanyID, string BranchID,string StartDate,string EndDate,string LoginUserID,string SearchUserID
        public HttpResponseMessage Get(Int32 CompanyID, Int32 BranchID, string ProfileName, Int32? LoginUserID, string Token)
        {
            List<APIViewLeadModel> viewleadsList = new List<APIViewLeadModel>();
            String ErrorMessage = String.Empty;
            String Records = String.Empty;
            //string Token = string.Empty;

            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);
                int UID = Convert.ToInt32(LoginUserID);

                int LoggedBranchId = Convert.ToInt32(BranchID);
                int LoggedCompanyId = Convert.ToInt32(CompanyID);

                var dd = Constant.GetimeForApi(companyID);
                DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

               
                #region Get all User and mapped users
                if (!string.IsNullOrEmpty(ProfileName) && ProfileName.Trim('"') == "SuperAdmin")
                {
                    UID = 0;                   
                }
                else
                {
                    UID = Convert.ToInt32(LoginUserID);                   
                }
                #endregion

                //check dynamic fields data according to company and branch id
                var data = db.crm_viewleadsetting.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();

                if (GetFormData == null)
                {
                    var GetFormData2 = new crm_customizedformfieldtextname();
                    GetFormData2.LeadOwnerTextName = "Lead Owner";
                    GetFormData2.CustomerTextName = "Customer Name";
                    GetFormData2.DesignationTextName = "Designation";
                    GetFormData2.MobileNoTextName = "Mobile Number";
                    GetFormData2.EmailIdTextName = "Customer e-mail";
                    GetFormData2.LeadResourceTextName = "Lead Source";
                    GetFormData2.LeadStatusTextName = "Lead Status";
                    GetFormData2.ProductTypeNameTextName = "Product Type";
                    GetFormData2.OrganizationNameTextName = "Organization Name";
                    GetFormData2.ZoneNameTextName = "Time Zone Name";
                    GetFormData2.FollowDateTextName = "Follow Up Date";
                    GetFormData2.FollowUpTimeTextName = "Follow Up Time";
                    GetFormData2.FollowupTimeinISTTextName = "Follow Up Time in IST";
                    GetFormData2.AddressTextNameTextName = "Address";
                    GetFormData2.CountryTextName = "Country";
                    GetFormData2.StateTextName = "State";
                    GetFormData2.CityTextName = "City/Location";
                    GetFormData2.UrlTextName = "URL";
                    GetFormData2.SkypeIdTextName = "Skype ID";
                    GetFormData2.FormTextName = "Create New Lead";
                    GetFormData2.DescriptionTextName = "Description";
                    GetFormData2.CreatedBy = UID;
                    GetFormData2.Createddate = Constant.GetimeForApi(CompanyID);
                    GetFormData2.CompanyID = CompanyID;
                    GetFormData2.BranchID = BranchID;
                    GetFormData2.Status = true;
                    GetFormData2.DateofBirthTextName = "Date Of Birth";
                    GetFormData2.MarriageAnniversaryTextName = "Marriage Anniversary";
                    GetFormData2.ExpectedDateTextName = "Expected Closing Date";
                    GetFormData2.ExpectedProductAmountTextName = "Expected Deal Amount";
                    
                    db.crm_customizedformfieldtextname.Add(GetFormData2);
                    db.SaveChanges();

                    GetFormData = GetFormData2;
                }


                #region field textname and visible/hide
                bool IsEMail = (data != null && data.IsEmail) ? true : false;
                bool IsCity = (data == null || data.IsCity) ? true : false;
                bool IsState = (data != null && data.IsState) ? true : false;
                bool IsCountry = (data != null && data.IsCountry) ? true : false;
                bool IsAssignedBy = (data == null || data.IsAssignedBy) ? true : false;
                bool IsAssinedTo = (data == null || data.IsAssignTo) ? true : false;
                bool IsAssignDate = (data == null || data.IsAssignedDate) ? true : false;
                bool IsExpectedDate = (data == null || data.IsExpClosingDate) ? true : false;
                bool IsExpectedProductAmount = (data == null || data.IsExpDealAmount) ? true : false;
                bool IsCreated_By = (data == null || data.IsCreatedBy) ? true : false;
                bool IsCreatedDate = (data == null || data.IsCreatedDate) ? true : false;
                bool IsModifiedDate = (data == null || data.IsModifiedDate) ? true : false;
                bool IsDateofBirth = (data != null && data.IsDOB) ? true : false;
                bool IsMarriageAnniversary = (data != null && data.IsMrgAnnivarsary) ? true : false;
                bool IsDesignation = (data != null && data.IsDesignation) ? true : false;
                bool IsProductType = (data != null && data.IsProductType) ? true : false;
                bool IsLeadResource = (data != null && data.IsLeadResource) ? true : false; 
                bool IsAddress = (data != null && data.IsAddress) ? true : false;
                bool IsURL = (data != null && data.IsUrl) ? true : false;
                bool IsSkypeId = (data != null && data.IsSkypeId) ? true : false;
                bool IsOrganizationName = (data != null && data.IsOrganization) ? true : false;
                bool IsExtraCol1 = (data != null && data.IsExtraCol1) ? true : false;
                bool IsExtraCol2 = (data != null && data.IsExtraCol2) ? true : false;
                bool IsExtraCol3 = (data != null && data.IsExtraCol3) ? true : false;
                bool IsExtraCol4 = (data != null && data.IsExtraCol4) ? true : false;
                bool IsExtraCol5 = (data != null && data.IsExtraCol5) ? true : false;
                bool IsExtraCol6 = (data != null && data.IsExtraCol6) ? true : false;
                bool IsExtraCol7 = (data != null && data.IsExtraCol7) ? true : false;
                bool IsExtraCol8 = (data != null && data.IsExtraCol8) ? true : false;
                bool IsExtraCol9 = (data != null && data.IsExtraCol9) ? true : false;
                bool IsExtraCol10 = (data != null && data.IsExtraCol10) ? true : false;
                bool IsExtraCol11 = (data != null && data.IsExtraCol11) ? true : false;
                bool IsExtraCol12 = (data != null && data.IsExtraCol12) ? true : false;
                bool IsExtraCol13 = (data != null && data.IsExtraCol13) ? true : false;
                bool IsExtraCol14 = (data != null && data.IsExtraCol14) ? true : false;
                bool IsExtraCol15 = (data != null && data.IsExtraCol15) ? true : false;
                bool IsExtraCol16 = (data != null && data.IsExtraCol16) ? true : false;
                bool IsExtraCol17 = (data != null && data.IsExtraCol17) ? true : false;
                bool IsExtraCol18 = (data != null && data.IsExtraCol18) ? true : false;
                bool IsExtraCol19 = (data != null && data.IsExtraCol19) ? true : false;
                bool IsExtraCol20 = (data != null && data.IsExtraCol20) ? true : false; 


                string AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.AddressTextNameTextName : string.Empty;
                IsAddress = string.IsNullOrEmpty(AddressTextNameTextName) ? false : IsAddress;
                string CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";                
                string MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";               
                string EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Email";               
                string DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : string.Empty;  
                IsDateofBirth = string.IsNullOrEmpty(DateofBirthTextName) ? false : IsDateofBirth;

                string DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : string.Empty;
                IsDesignation = string.IsNullOrEmpty(DesignationTextName) ? false : IsDesignation;

                string ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : string.Empty;
                IsExpectedDate = string.IsNullOrEmpty(ExpectedDateTextName) ? false : IsExpectedDate;

                string ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : string.Empty;
                IsExpectedProductAmount = string.IsNullOrEmpty(ExpectedProductAmountTextName) ? false : IsExpectedProductAmount;

                string LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                string LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                string FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                string FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                string FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";
                string CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";
                string StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";
                string CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City";

                string MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : string.Empty;
                IsMarriageAnniversary = string.IsNullOrEmpty(MarriageAnniversaryTextName) ? false : IsMarriageAnniversary;

                string OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : string.Empty;
                IsOrganizationName = string.IsNullOrEmpty(OrganizationNameTextName) ? false : IsOrganizationName;

                string ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                string SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : string.Empty;
                IsSkypeId = string.IsNullOrEmpty(SkypeIdTextName) ? false :IsSkypeId;
                string UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : string.Empty;
                IsURL = string.IsNullOrEmpty(UrlTextName) ? false : IsURL;
                string ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";

                string ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol1TextName))
                {
                    IsExtraCol1 = false;
                }
                string ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol2TextName))
                {
                    IsExtraCol2 = false;
                }
                string ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol3TextName))
                {
                    IsExtraCol3 = false;
                }
                string ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol4TextName))
                {
                    IsExtraCol4 = false;
                }
                string ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol5TextName))
                {
                    IsExtraCol5 = false;
                }
                string ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol6TextName))
                {
                    IsExtraCol6 = false;
                }
                string ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol7TextName))
                {
                    IsExtraCol7 = false;
                }
                string ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol8TextName))
                {
                    IsExtraCol8 = false;
                }
                string ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol9TextName))
                {
                    IsExtraCol9 = false;
                }
                string ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol10TextName))
                {
                    IsExtraCol10 = false;
                }
                string ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol11TextName))
                {
                    IsExtraCol11 = false;
                }
                string ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol12TextName))
                {
                    IsExtraCol12 = false;
                }
                string ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol13TextName))
                {
                    IsExtraCol13 = false;
                }
                string ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol14TextName))
                {
                    IsExtraCol14 = false;
                }
                string ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol15TextName))
                {
                    IsExtraCol15 = false;
                }
                string ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol16TextName))
                {
                    IsExtraCol16 = false;
                }
                string ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol7TextName))
                {
                    IsExtraCol7 = false;
                }
                string ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol18TextName))
                {
                    IsExtraCol18 = false;
                }
                string ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol19TextName))
                {
                    IsExtraCol19 = false;
                }
                string ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                if (string.IsNullOrEmpty(ExtraCol20TextName))
                {
                    IsExtraCol20 = false;
                }

                #endregion

                // DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + branchID + "," + companyID + ")");
                string FilterType = ""; string filterText = "";
                DataTable GetRecords = DataAccessLayer.GetDataTable(" call CRM_ViewLeadsDefaultOrFilter(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + FilterType + "','" + filterText + "' )");

                if (GetRecords.Rows.Count > 0)
                {
                    List<APIViewLeadModel> vlmList = new List<APIViewLeadModel>();
                    for (int i = 0; i < GetRecords.Rows.Count; i++)
                    {
                        APIViewLeadModel vlm = new APIViewLeadModel();
                        int spaceIndex = 0;
                        string afterSpaceValue = "";
                        vlm.LeadId = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
                        vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);

                        spaceIndex = vlm.LeadName.IndexOf(' ');//get index of fist space
                        afterSpaceValue = spaceIndex > 0 && vlm.LeadName.Length > (spaceIndex + 1) ? vlm.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
                        vlm.ShortName = !string.IsNullOrEmpty(vlm.LeadName) ? vlm.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

                        vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]).Trim();
                        vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
                        vlm.Country = Convert.ToString(GetRecords.Rows[i]["CountryName"]);
                        vlm.State = Convert.ToString(GetRecords.Rows[i]["StateName"]);
                        vlm.City = Convert.ToString(GetRecords.Rows[i]["CityName"]);
                        vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
                        vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
                        vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
                        vlm.RowBackground = Convert.ToString(GetRecords.Rows[i]["ColorHexValue"]);

                        vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                        vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
                        vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
                        if (Convert.ToString(GetRecords.Rows[i]["AssignedTo"]) != null && Convert.ToString(GetRecords.Rows[i]["AssignedTo"]) != string.Empty)
                        {
                            vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
                            vlm.AssignTo = GetRecords.Rows[i]["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["AssignTo"]);
                        }
                        vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
                        vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
                        vlm.IsDOB = !string.IsNullOrEmpty(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
                        vlm.IsMA = !string.IsNullOrEmpty(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";

                        vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
                        vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
                        vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
                        vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
                        vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? string.Empty : String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]);
                        vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);
                        vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
                        vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
                        vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
                        vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
                        vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
                        vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
                        vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
                        vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
                        vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
                        vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
                        vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
                        vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
                        vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
                        vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

                        vlm.ExtraCol11 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol11"]);
                        vlm.ExtraCol12 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol12"]);
                        vlm.ExtraCol13 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol13"]);
                        vlm.ExtraCol14 = Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol14"]);
                        vlm.ExtraCol15 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol15"]);
                        vlm.ExtraCol16 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol16"]);
                        vlm.ExtraCol17 = Convert.ToInt32(GetRecords.Rows[i]["ExtraCol17"]);
                        vlm.ExtraCol18 = Convert.ToString(GetRecords.Rows[i]["ExtraCol18"]);
                        vlm.ExtraCol19 = Convert.ToString(GetRecords.Rows[i]["ExtraCol19"]);
                        vlm.ExtraCol20 = Convert.ToString(GetRecords.Rows[i]["ExtraCol20"]);
                        ////////////////////////////show column on view leads start///////////////////////////////////////
                        
                        vlm.IsCustomerName = true;
                        vlm.CustomerNameLabel = CustomerTextName;

                        vlm.IsMob = true;
                        vlm.MobLabel = MobileNoTextName;

                        vlm.IsFollowupDate = true;
                        vlm.FollowupDateLabel = FollowDateTextName;

                        vlm.IsLeadStatus = true;
                        vlm.LeadStatusLabel = LeadStatusTextName;

                        vlm.IsEMail = IsEMail;
                        vlm.EMailLabel = EmailIdTextName;

                        vlm.IsCity = IsCity;
                        vlm.CityLabel = CityTextName;

                        vlm.IsState = IsState;
                        vlm.StateLabel = StateTextName;

                        vlm.IsCountry = IsCountry;
                        vlm.CountryLabel = CountryTextName;

                        vlm.IsAssignedBy = IsAssignedBy;
                        vlm.AssignedByLabel = "Assigned By";

                        vlm.IsAssinedTo = IsAssinedTo;
                        vlm.AssinedToLabel = "Assigned To";

                        vlm.IsAssignDate = IsAssignDate;
                        vlm.AssignDateLabel = "Assigned Date";

                        vlm.IsExpectedDate = IsExpectedDate;
                        vlm.ExpectedDateLabel = ExpectedDateTextName;

                        vlm.IsExpectedProductAmount = IsExpectedProductAmount;
                        vlm.ExpectedProductAmountLabel = ExpectedProductAmountTextName;

                        vlm.IsCreated_By = IsCreated_By;
                        vlm.Created_ByLabel = "Created By";

                        vlm.IsCreatedDate = IsCreatedDate;
                        vlm.CreatedDateLabel = "Created Date";

                        vlm.IsModifiedDate = IsModifiedDate;
                        vlm.ModifiedDateLabel = "Modified Date";

                        vlm.IsDateofBirth = IsDateofBirth;
                        vlm.DateofBirthLabel = DateofBirthTextName;

                        vlm.IsMarriageAnniversary = IsMarriageAnniversary;
                        vlm.MarriageAnniversaryLabel = MarriageAnniversaryTextName;

                        vlm.IsDesignation = IsDesignation;
                        vlm.DesignationLabel = DesignationTextName;

                        vlm.IsProductType = IsProductType;
                        vlm.ProductTypeLabel = ProductTypeNameTextName;

                        vlm.IsLeadResource = IsLeadResource;
                        vlm.LeadResourceLabel = LeadResourceTextName;

                        vlm.IsAddress = IsAddress;
                        vlm.AddressLabel = AddressTextNameTextName;

                        vlm.IsURL = IsURL;
                        vlm.URLLabel = UrlTextName;

                        vlm.IsSkypeId = IsSkypeId;
                        vlm.SkypeIdLabel = SkypeIdTextName;

                        vlm.IsOrganizationName = IsOrganizationName;
                        vlm.OrganizationNameLabel = OrganizationNameTextName;

                        vlm.IsExtraCol1 = IsExtraCol1;
                        vlm.ExtraCol1Label = ExtraCol1TextName;

                        vlm.IsExtraCol2 = IsExtraCol2;
                        vlm.ExtraCol2Label = ExtraCol2TextName;

                        vlm.IsExtraCol3 = IsExtraCol3;
                        vlm.ExtraCol3Label = ExtraCol3TextName;

                        vlm.IsExtraCol4 = IsExtraCol4;
                        vlm.ExtraCol4Label = ExtraCol4TextName;

                        vlm.IsExtraCol5 = IsExtraCol5;
                        vlm.ExtraCol5Label = ExtraCol5TextName;

                        vlm.IsExtraCol6 = IsExtraCol6;
                        vlm.ExtraCol6Label = ExtraCol6TextName;

                        vlm.IsExtraCol7 = IsExtraCol7;
                        vlm.ExtraCol7Label = ExtraCol7TextName;

                        vlm.IsExtraCol8 = IsExtraCol8;
                        vlm.ExtraCol8Label = ExtraCol8TextName;

                        vlm.IsExtraCol9 = IsExtraCol9;
                        vlm.ExtraCol9Label = ExtraCol9TextName;

                        vlm.IsExtraCol10 = IsExtraCol10;
                        vlm.ExtraCol10Label = ExtraCol10TextName;

                        vlm.IsExtraCol11 = IsExtraCol11;
                        vlm.ExtraCol11Label = ExtraCol11TextName;

                        vlm.IsExtraCol12 = IsExtraCol12;
                        vlm.ExtraCol12Label = ExtraCol12TextName;

                        vlm.IsExtraCol13 = IsExtraCol13;
                        vlm.ExtraCol13Label = ExtraCol13TextName;

                        vlm.IsExtraCol14 = IsExtraCol14;
                        vlm.ExtraCol14Label = ExtraCol14TextName;

                        vlm.IsExtraCol15 = IsExtraCol15;
                        vlm.ExtraCol15Label = ExtraCol15TextName;

                        vlm.IsExtraCol16 = IsExtraCol16;
                        vlm.ExtraCol16Label = ExtraCol16TextName;

                        vlm.IsExtraCol17 = IsExtraCol17;
                        vlm.ExtraCol17Label = ExtraCol17TextName;

                        vlm.IsExtraCol18 = IsExtraCol18;
                        vlm.ExtraCol18Label = ExtraCol18TextName;

                        vlm.IsExtraCol19 = IsExtraCol19;
                        vlm.ExtraCol19Label = ExtraCol19TextName;

                        vlm.IsExtraCol20 = IsExtraCol20;
                        vlm.ExtraCol20Label = ExtraCol20TextName;
                        vlmList.Add(vlm);
                    }
                    viewleadsList = vlmList;

                    //viewleadsList = viewleadsList.Where(em => em.LeadStatus != "Not Interested").ToList();
                }
                else
                {
                    //Records = string.Format("** No Record Found **");
                    ErrorMessage = "** No Record Found **";
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, viewleadsList);

                //if (Records != string.Empty)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, Records);
                //}
                //else
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, viewleadsList);
                //}
            }
        }

        #endregion

        #region old code not in use
        //public HttpResponseMessage Get(Int32 CompanyID, Int32 BranchID, string ProfileName, Int32? LoginUserID, string Token)
        //{
        //    List<APIViewLeadModel> viewleadsList = new List<APIViewLeadModel>();
        //    String ErrorMessage = String.Empty;
        //    String Records = String.Empty;
        //    //string Token = string.Empty;

        //    //var re = Request;
        //    //var headers = re.Headers;

        //    //if (headers.Contains("Token"))
        //    //{
        //    //    Token = headers.GetValues("Token").First();
        //    //}
        //    var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

        //    if (auth == false)
        //    {
        //        ErrorMessage = string.Format("** User authentication failed!");
        //        HttpError err = new HttpError(ErrorMessage);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //    try
        //    {
        //        Int32 branchID = Convert.ToInt32(BranchID);
        //        Int32 companyID = Convert.ToInt32(CompanyID);
        //        Int32 UID = Convert.ToInt32(LoginUserID);


        //        var dd = Constant.GetimeForApi(companyID);
        //        DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
        //        DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
        //        var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
        //        var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

        //        //var MappedUsers = new List<crm_usertbl>();

        //        //check dynamic fields data according to company and branch id
        //        var data = db.crm_viewleadsetting.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
        //        var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();

        //        if (!string.IsNullOrEmpty(ProfileName) && ProfileName.Trim('"') == "SuperAdmin")
        //        {
        //            UID = 0;
        //            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + branchID + "," + companyID + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<APIViewLeadModel> vlmList = new List<APIViewLeadModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    APIViewLeadModel vlm = new APIViewLeadModel();
        //                    int spaceIndex = 0;
        //                    string afterSpaceValue = "";
        //                    vlm.LeadId = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);

        //                    spaceIndex = vlm.LeadName.IndexOf(' ');//get index of fist space
        //                    afterSpaceValue = spaceIndex > 0 && vlm.LeadName.Length > (spaceIndex + 1) ? vlm.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
        //                    vlm.ShortName = !string.IsNullOrEmpty(vlm.LeadName) ? vlm.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]).Trim();
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["Country"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["State"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["City"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
        //                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);

        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    if (Convert.ToString(GetRecords.Rows[i]["AssignedTo"]) != null && Convert.ToString(GetRecords.Rows[i]["AssignedTo"]) != string.Empty)
        //                    {
        //                        vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                        vlm.AssignTo = GetRecords.Rows[i]["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["AssignTo"]);
        //                    }
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.IsDOB = !string.IsNullOrEmpty(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
        //                    vlm.IsMA = !string.IsNullOrEmpty(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";

        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? string.Empty : String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]);
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                    ////////////////////////////show column on view leads start///////////////////////////////////////
        //                    vlm.ReportTextName = (data != null && !string.IsNullOrEmpty(data.ReportTextName)) ? data.ReportTextName : "View Leads Report";

        //                    vlm.IsCustomerName = true;
        //                    vlm.CustomerNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";

        //                    vlm.IsMob = true;
        //                    vlm.MobLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";

        //                    vlm.IsFollowupDate = true;
        //                    vlm.FollowupDateLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";

        //                    vlm.IsLeadStatus = true;
        //                    vlm.LeadStatusLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";

        //                    vlm.IsEMail = (data != null && data.IsEmail) ? true : false;
        //                    vlm.EMailLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Email";

        //                    vlm.IsCity = (data == null || data.IsCity) ? true : false;
        //                    vlm.CityLabel = "City";

        //                    vlm.IsState = (data != null && data.IsState) ? true : false;
        //                    vlm.StateLabel = "State";

        //                    vlm.IsCountry = (data != null && data.IsCountry) ? true : false;
        //                    vlm.CountryLabel = "Country";

        //                    vlm.IsAssignedBy = (data == null || data.IsAssignedBy) ? true : false;
        //                    vlm.AssignedByLabel = "Assigned By";

        //                    vlm.IsAssinedTo = (data == null || data.IsAssignTo) ? true : false;
        //                    vlm.AssinedToLabel = "Assigned To";

        //                    vlm.IsAssignDate = (data == null || data.IsAssignedDate) ? true : false;
        //                    vlm.AssignDateLabel = "Assigned Date";

        //                    vlm.IsExpectedDate = (data == null || data.IsExpClosingDate) ? true : false;
        //                    vlm.ExpectedDateLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : "Expected Closing Date";

        //                    vlm.IsExpectedProductAmount = (data == null || data.IsExpDealAmount) ? true : false;
        //                    vlm.ExpectedProductAmountLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : "Expected Deal Amount";

        //                    vlm.IsCreated_By = (data == null || data.IsCreatedBy) ? true : false;
        //                    vlm.Created_ByLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Created By";

        //                    vlm.IsCreatedDate = (data == null || data.IsCreatedDate) ? true : false;
        //                    vlm.CreatedDateLabel = "Created Date";

        //                    vlm.IsModifiedDate = (data == null || data.IsModifiedDate) ? true : false;
        //                    vlm.ModifiedDateLabel = "Modified Date";

        //                    vlm.IsDateofBirth = (data != null && data.IsDOB) ? true : false;
        //                    vlm.DateofBirthLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : "Date Of Birth";

        //                    vlm.IsMarriageAnniversary = (data != null && data.IsMrgAnnivarsary) ? true : false;
        //                    vlm.MarriageAnniversaryLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : "Marriage Anniversary";

        //                    vlm.IsDesignation = (data != null && data.IsDesignation) ? true : false;
        //                    vlm.DesignationLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : "Designation";

        //                    vlm.IsProductType = (data != null && data.IsProductType) ? true : false;
        //                    vlm.ProductTypeLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";

        //                    vlm.IsLeadResource = data.IsLeadResource;
        //                    vlm.LeadResourceLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";

        //                    vlm.IsAddress = (data != null && data.IsAddress) ? true : false;
        //                    vlm.AddressLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.LeadStatusTextName : "Address";

        //                    vlm.IsURL = (data != null && data.IsUrl) ? true : false;
        //                    vlm.URLLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : "URL";

        //                    vlm.IsSkypeId = (data != null && data.IsSkypeId) ? true : false;
        //                    vlm.SkypeIdLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : "Skype ID";

        //                    vlm.IsOrganizationName = (data != null && data.IsOrganization) ? true : false;
        //                    vlm.OrganizationNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";

        //                    vlm.IsExtraCol1 = (data != null && data.IsExtraCol1) ? true : false;
        //                    vlm.ExtraCol1Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : "Additional 1";

        //                    vlm.IsExtraCol2 = (data != null && data.IsExtraCol2) ? true : false;
        //                    vlm.ExtraCol2Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : "Additional 2";

        //                    vlm.IsExtraCol3 = (data != null && data.IsExtraCol3) ? true : false;
        //                    vlm.ExtraCol3Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : "Additional 3";

        //                    vlm.IsExtraCol4 = (data != null && data.IsExtraCol4) ? true : false;
        //                    vlm.ExtraCol4Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : "Additional 4";

        //                    vlm.IsExtraCol5 = (data != null && data.IsExtraCol5) ? true : false;
        //                    vlm.ExtraCol5Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : "Additional 5";

        //                    vlm.IsExtraCol6 = (data != null && data.IsExtraCol6) ? true : false;
        //                    vlm.ExtraCol6Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : "Additional 6";

        //                    vlm.IsExtraCol7 = (data != null && data.IsExtraCol7) ? true : false;
        //                    vlm.ExtraCol7Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : "Additional 7";

        //                    vlm.IsExtraCol8 = (data != null && data.IsExtraCol8) ? true : false;
        //                    vlm.ExtraCol8Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : "Additional 8";

        //                    vlm.IsExtraCol9 = (data != null && data.IsExtraCol9) ? true : false;
        //                    vlm.ExtraCol9Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : "Additional 9";

        //                    vlm.IsExtraCol10 = (data != null && data.IsExtraCol10) ? true : false;
        //                    vlm.ExtraCol10Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : "Additional 10";


        //                    vlmList.Add(vlm);
        //                }
        //                viewleadsList = vlmList;

        //                viewleadsList = viewleadsList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //            }
        //            else
        //            {
        //                Records = string.Format("** No Record Found **");
        //            }
        //        }
        //        else
        //        {
        //            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetViewLeadsByDate(" + UID + ",'" + MStartDate + "','" + MEndDate + "'," + branchID + "," + companyID + ")");
        //            if (GetRecords.Rows.Count > 0)
        //            {
        //                List<APIViewLeadModel> vlmList = new List<APIViewLeadModel>();
        //                for (int i = 0; i < GetRecords.Rows.Count; i++)
        //                {
        //                    APIViewLeadModel vlm = new APIViewLeadModel();
        //                    int spaceIndex = 0;
        //                    string afterSpaceValue = "";

        //                    vlm.LeadId = Convert.ToInt32(GetRecords.Rows[i]["ID"]);
        //                    vlm.LeadName = Convert.ToString(GetRecords.Rows[i]["Customer"]);

        //                    spaceIndex = vlm.LeadName.IndexOf(' ');//get index of fist space
        //                    afterSpaceValue = spaceIndex > 0 && vlm.LeadName.Length > (spaceIndex + 1) ? vlm.LeadName[spaceIndex + 1].ToString() : "";//get fist letter after space
        //                    vlm.ShortName = !string.IsNullOrEmpty(vlm.LeadName) ? vlm.LeadName[0].ToString().ToUpper() + "" + afterSpaceValue.ToUpper() : "";//merge fist and after space value

        //                    vlm.Mob = Convert.ToString(GetRecords.Rows[i]["MobileNo"]).Trim();
        //                    vlm.EMail = Convert.ToString(GetRecords.Rows[i]["EmailId"]);
        //                    vlm.Country = Convert.ToString(GetRecords.Rows[i]["Country"]);
        //                    vlm.State = Convert.ToString(GetRecords.Rows[i]["State"]);
        //                    vlm.City = Convert.ToString(GetRecords.Rows[i]["City"]);
        //                    vlm.FollowupDate = Convert.ToString(GetRecords.Rows[i]["FollowDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.Created_By = Convert.ToString(GetRecords.Rows[i]["Fname"]) + " " + Convert.ToString(GetRecords.Rows[i]["Lname"]);
        //                    vlm.LeadStatusID = GetRecords.Rows[i]["LeadStatusID"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["LeadStatusID"]);
        //                    vlm.LeadStatus = Convert.ToString(GetRecords.Rows[i]["LeadStatus"]);
        //                    vlm.AssignBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.Address = Convert.ToString(GetRecords.Rows[i]["Address"]);
        //                    vlm.LeadOwner = Convert.ToString(GetRecords.Rows[i]["LeadOwner"]);
        //                    if (Convert.ToString(GetRecords.Rows[i]["AssignedTo"]) != null)
        //                    {
        //                        vlm.AssinedTo = Convert.ToString(GetRecords.Rows[i]["AssignedTo"]);
        //                        vlm.AssignTo = GetRecords.Rows[i]["AssignTo"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["AssignTo"]);
        //                    }
        //                    vlm.AssignedBy = Convert.ToString(GetRecords.Rows[i]["AssignedBy"]);
        //                    vlm.AssignDate = Convert.ToString(GetRecords.Rows[i]["AssignedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.ModifiedDate = Convert.ToString(GetRecords.Rows[i]["ModifiedDate"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.date = Convert.ToString(GetRecords.Rows[i]["date"]).Replace(" 00:00:00", "").Replace(" 12:00:00 AM", "");
        //                    vlm.IsDOB = !string.IsNullOrEmpty(Convert.ToString(GetRecords.Rows[i]["IsDOB"])) ? Convert.ToString(GetRecords.Rows[i]["IsDOB"]) : "0";
        //                    vlm.IsMA = !string.IsNullOrEmpty(Convert.ToString(GetRecords.Rows[i]["IsMA"])) ? Convert.ToString(GetRecords.Rows[i]["IsMA"]) : "0";

        //                    vlm.ProductTypeName = Convert.ToString(GetRecords.Rows[i]["ProductTypeName"]);
        //                    vlm.LeadsourceName = Convert.ToString(GetRecords.Rows[i]["LeadsourceName"]);
        //                    vlm.DateofBirth = Convert.ToString(GetRecords.Rows[i]["DateofBirth"]);
        //                    vlm.MarriageAnniversary = Convert.ToString(GetRecords.Rows[i]["MarriageAnniversary"]);
        //                    vlm.ExpectedDate = GetRecords.Rows[i]["ExpectedDate"] == DBNull.Value ? string.Empty : String.Format("{0:dd/MM/yyyy}", GetRecords.Rows[i]["ExpectedDate"]);
        //                    vlm.ExpectedProductAmount = Convert.ToDecimal(GetRecords.Rows[i]["ExpectedProductAmount"]);
        //                    vlm.URL = Convert.ToString(GetRecords.Rows[i]["URL"]);
        //                    vlm.SkypeId = Convert.ToString(GetRecords.Rows[i]["SkypeId"]);
        //                    vlm.Designation = Convert.ToString(GetRecords.Rows[i]["Designation"]);
        //                    vlm.OrganizationName = Convert.ToString(GetRecords.Rows[i]["OrganizationName"]);
        //                    vlm.ExtraCol1 = Convert.ToString(GetRecords.Rows[i]["ExtraCol1"]);
        //                    vlm.ExtraCol2 = Convert.ToString(GetRecords.Rows[i]["ExtraCol2"]);
        //                    vlm.ExtraCol3 = Convert.ToString(GetRecords.Rows[i]["ExtraCol3"]);
        //                    vlm.ExtraCol4 = Convert.ToString(GetRecords.Rows[i]["ExtraCol4"]);
        //                    vlm.ExtraCol5 = Convert.ToString(GetRecords.Rows[i]["ExtraCol5"]);
        //                    vlm.ExtraCol6 = GetRecords.Rows[i]["ExtraCol6"] == DBNull.Value ? 0 : Convert.ToDecimal(GetRecords.Rows[i]["ExtraCol6"]);
        //                    vlm.ExtraCol7 = GetRecords.Rows[i]["ExtraCol7"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol7"]);
        //                    vlm.ExtraCol8 = GetRecords.Rows[i]["ExtraCol8"] == DBNull.Value ? 0 : Convert.ToInt32(GetRecords.Rows[i]["ExtraCol8"]);
        //                    vlm.ExtraCol9 = Convert.ToString(GetRecords.Rows[i]["ExtraCol9"]);
        //                    vlm.ExtraCol10 = Convert.ToString(GetRecords.Rows[i]["ExtraCol10"]);

        //                    ////////////////////////////show column on view leads start///////////////////////////////////////
        //                    vlm.ReportTextName = (data != null && !string.IsNullOrEmpty(data.ReportTextName)) ? data.ReportTextName : "View Leads Report";

        //                    vlm.IsCustomerName = true;
        //                    vlm.CustomerNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";

        //                    vlm.IsMob = true;
        //                    vlm.MobLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";

        //                    vlm.IsFollowupDate = true;
        //                    vlm.FollowupDateLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";

        //                    vlm.IsLeadStatus = true;
        //                    vlm.LeadStatusLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";

        //                    vlm.IsEMail = (data != null && data.IsEmail) ? true : false;
        //                    vlm.EMailLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Email";

        //                    vlm.IsCity = (data == null || data.IsCity) ? true : false;
        //                    vlm.CityLabel = "City";

        //                    vlm.IsState = (data != null && data.IsState) ? true : false;
        //                    vlm.StateLabel = "State";

        //                    vlm.IsCountry = (data != null && data.IsCountry) ? true : false;
        //                    vlm.CountryLabel = "Country";

        //                    vlm.IsAssignedBy = (data == null || data.IsAssignedBy) ? true : false;
        //                    vlm.AssignedByLabel = "Assigned By";

        //                    vlm.IsAssinedTo = (data == null || data.IsAssignTo) ? true : false;
        //                    vlm.AssinedToLabel = "Assigned To";

        //                    vlm.IsAssignDate = (data == null || data.IsAssignedDate) ? true : false;
        //                    vlm.AssignDateLabel = "Assigned Date";

        //                    vlm.IsExpectedDate = (data == null || data.IsExpClosingDate) ? true : false;
        //                    vlm.ExpectedDateLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : "Expected Closing Date";

        //                    vlm.IsExpectedProductAmount = (data == null || data.IsExpDealAmount) ? true : false;
        //                    vlm.ExpectedProductAmountLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : "Expected Deal Amount";

        //                    vlm.IsCreated_By = (data == null || data.IsCreatedBy) ? true : false;
        //                    vlm.Created_ByLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Created By";

        //                    vlm.IsCreatedDate = (data == null || data.IsCreatedDate) ? true : false;
        //                    vlm.CreatedDateLabel = "Created Date";

        //                    vlm.IsModifiedDate = (data == null || data.IsModifiedDate) ? true : false;
        //                    vlm.ModifiedDateLabel = "Modified Date";

        //                    vlm.IsDateofBirth = (data != null && data.IsDOB) ? true : false;
        //                    vlm.DateofBirthLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : "Date Of Birth";

        //                    vlm.IsMarriageAnniversary = (data != null && data.IsMrgAnnivarsary) ? true : false;
        //                    vlm.MarriageAnniversaryLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : "Marriage Anniversary";

        //                    vlm.IsDesignation = (data != null && data.IsDesignation) ? true : false;
        //                    vlm.DesignationLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : "Designation";

        //                    vlm.IsProductType = (data != null && data.IsProductType) ? true : false;
        //                    vlm.ProductTypeLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";

        //                    vlm.IsLeadResource = data.IsLeadResource;
        //                    vlm.LeadResourceLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";

        //                    vlm.IsAddress = (data != null && data.IsAddress) ? true : false;
        //                    vlm.AddressLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.LeadStatusTextName : "Address";

        //                    vlm.IsURL = (data != null && data.IsUrl) ? true : false;
        //                    vlm.URLLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : "URL";

        //                    vlm.IsSkypeId = (data != null && data.IsSkypeId) ? true : false;
        //                    vlm.SkypeIdLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : "Skype ID";

        //                    vlm.IsOrganizationName = (data != null && data.IsOrganization) ? true : false;
        //                    vlm.OrganizationNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";

        //                    vlm.IsExtraCol1 = (data != null && data.IsExtraCol1) ? true : false;
        //                    vlm.ExtraCol1Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : "Additional 1";

        //                    vlm.IsExtraCol2 = (data != null && data.IsExtraCol2) ? true : false;
        //                    vlm.ExtraCol2Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : "Additional 2";

        //                    vlm.IsExtraCol3 = (data != null && data.IsExtraCol3) ? true : false;
        //                    vlm.ExtraCol3Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : "Additional 3";

        //                    vlm.IsExtraCol4 = (data != null && data.IsExtraCol4) ? true : false;
        //                    vlm.ExtraCol4Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : "Additional 4";

        //                    vlm.IsExtraCol5 = (data != null && data.IsExtraCol5) ? true : false;
        //                    vlm.ExtraCol5Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : "Additional 5";

        //                    vlm.IsExtraCol6 = (data != null && data.IsExtraCol6) ? true : false;
        //                    vlm.ExtraCol6Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : "Additional 6";

        //                    vlm.IsExtraCol7 = (data != null && data.IsExtraCol7) ? true : false;
        //                    vlm.ExtraCol7Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : "Additional 7";

        //                    vlm.IsExtraCol8 = (data != null && data.IsExtraCol8) ? true : false;
        //                    vlm.ExtraCol8Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : "Additional 8";

        //                    vlm.IsExtraCol9 = (data != null && data.IsExtraCol9) ? true : false;
        //                    vlm.ExtraCol9Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : "Additional 9";

        //                    vlm.IsExtraCol10 = (data != null && data.IsExtraCol10) ? true : false;
        //                    vlm.ExtraCol10Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : "Additional 10";

        //                    vlmList.Add(vlm);
        //                }
        //                viewleadsList = vlmList;

        //                //string UserddlName = Convert.ToString(LoginUserID);
        //                //viewleadsList = viewleadsList.Where(em => (em.LeadOwner == UserddlName && (em.AssignTo == null || em.AssignTo != LoginUserID)) || em.AssignTo == LoginUserID).ToList();
        //                //List<Userddl> Userddllist = new List<Userddl>();

        //                var GetUserData = db.crm_usertbl.Find(LoginUserID);

        //                if (GetUserData != null && GetUserData.MappedUsers != null)
        //                {
        //                    var GetMapUser = GetUserData.MappedUsers.Split(',');
        //                    Userddl u = new Userddl();
        //                    foreach (var item in GetMapUser)
        //                    {
        //                        var mapid = Convert.ToInt32(item);

        //                        List<APIViewLeadModel> VlieadList = viewleadsList.Where(em => (em.LeadOwner == item && (em.AssignTo == null || em.AssignTo != mapid)) || em.AssignTo == mapid).ToList();
        //                        viewleadsList.AddRange(VlieadList);
        //                        //var user = new Userddl
        //                        //{
        //                        //    uid = mapid
        //                        //};
        //                        //Userddllist.Add(user);
        //                    }
        //                    //foreach (var item in Userddllist)
        //                    //{
        //                    //    List<APIViewLeadModel> VlieadList = viewleadsList.Where(em => (em.LeadOwner == Convert.ToString(item.uid) && (em.AssignTo == null || em.AssignTo != Convert.ToInt32(item.uid))) || em.AssignTo == Convert.ToInt32(item.uid)).ToList();
        //                    //    viewleadsList.AddRange(VlieadList);
        //                    //}
        //                }

        //                viewleadsList = viewleadsList.Where(em => em.LeadStatus != "Not Interested").ToList();
        //                if (viewleadsList.Count == 0)
        //                {
        //                    Records = string.Format("** No Record Found **");
        //                }
        //            }
        //            else
        //            {
        //                Records = string.Format("** No Record Found **");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //        ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
        //    }

        //    if (ErrorMessage != string.Empty)
        //    {
        //        HttpError err = new HttpError(ErrorMessage);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //    else
        //    {
        //        if (Records != string.Empty)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, Records);
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, viewleadsList);
        //        }
        //    }
        //}
        #endregion
    }
}
