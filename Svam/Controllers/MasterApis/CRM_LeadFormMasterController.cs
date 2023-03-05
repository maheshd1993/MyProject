using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.Models.DTO;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Svam.Controllers.MasterApis
{
    public class CRM_LeadFormMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_LeadFormMaster/GetLeadFormFields?CompanyID=307&BranchID=184&UID=61&Token=VwFdB3OPEwOoHnr15a5qgg==
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetLeadFormFields(string CompanyID, string BranchID, string UID, string Token) 
        {
            string ErrorMessage = string.Empty;

            var fieldsList = new List<TktFormFieldsModel>();

            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);
                var uid = string.IsNullOrEmpty(UID) ? Convert.ToInt32(UID) : 0;
                //System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //     Token = headers.GetValues("Token").First();
                //}

                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                var dt = Constant.GetimeForApi(companyID);
              
                var fieldPriorityList = new List<LeadFieldPriorityDTO>();

                var GetData = await db.crm_createleadsetting.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                var GetSeqData = await db.crm_create_lead_field_sequence.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToListAsync();

                #region save data if first time user enter here
                if (GetData == null)
                {
                    crm_createleadsetting GetData1 = new crm_createleadsetting();
                    GetData1.BranchID = branchID;
                    GetData1.CompanyID = companyID;
                    GetData1.CreatedDate = Constant.GetBharatTime();
                    GetData1.IsCustomer = true;
                    GetData1.IsDesignation = true;
                    GetData1.IsMobileNo = true;
                    GetData1.IsEmailId = true;
                    GetData1.IsLeadResource = true;
                    GetData1.IsLeadStatus = true;
                    GetData1.IsProductType = true;
                    GetData1.IsOrganizationName = true;
                    GetData1.IsTimeZoneName = true;
                    GetData1.IsFollowupDate = true;
                    GetData1.IsFollowUpTime = true;
                    GetData1.IsFollowupTimeinIST = true;
                    GetData1.IsCountry = true;
                    GetData1.IsCity = true;
                    GetData1.IsState = true;
                    GetData1.IsAddress = true;
                    GetData1.IsUrl = true;
                    GetData1.IsSkypeId = true;
                    GetData1.IsDescription = true;
                    GetData1.IsActive = true;
                    GetData1.IsDateofBirth = true;
                    GetData1.IsMarriageAnniversary = true;
                    GetData1.IsExpectedClosingDate = true;
                    GetData1.IsExpectedDealAmount = true;
                    GetData1.IsDesigMandatory = false;
                    GetData1.IsEmailMandatory = false;
                    GetData1.IsLdResMandatory = false;
                    GetData1.IsLdStatusMandatory = true;
                    GetData1.IsProdTypeMandatory = false;
                    GetData1.IsOrgNameMandatory = false;
                    GetData1.IsTZNameMandatory = false;
                    GetData1.IsFUpTimeMandatory = false;
                    GetData1.IsFupTimeinISTMandatory = false;
                    GetData1.IsCountryMandatory = false;
                    GetData1.IsCityMandatory = false;
                    GetData1.IsStateMandatory = false;
                    GetData1.IsAddressMandatory = false;
                    GetData1.IsUrlMandatory = false;
                    GetData1.IsSkypeIdMandatory = false;
                    GetData1.IsDOBMandatory = false;
                    GetData1.IsMrgAniMandatory = false;
                    GetData1.IsExpClsDateMandatory = false;
                    GetData1.IsExpDealAmtMandatory = false;
                    
                    db.crm_createleadsetting.Add(GetData1);
                   await db.SaveChangesAsync();
                }

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
                    GetFormData2.CreatedBy = uid;
                    GetFormData2.Createddate = dt;
                    GetFormData2.ModifiedDate = dt;
                    GetFormData2.CompanyID = companyID;
                    GetFormData2.BranchID = branchID;
                    GetFormData2.Status = true;
                    GetFormData2.DateofBirthTextName = "Date Of Birth";
                    GetFormData2.MarriageAnniversaryTextName = "Marriage Anniversary";
                    GetFormData2.ExpectedDateTextName = "Expected Closing Date";
                    GetFormData2.ExpectedProductAmountTextName = "Expected Deal Amount";
                    
                    db.crm_customizedformfieldtextname.Add(GetFormData2);
                   await db.SaveChangesAsync();

                    GetFormData = GetFormData2;
                }
                #endregion

                #region save field sequence data

                if (GetSeqData != null && GetSeqData.Count > 0)
                {
                    fieldPriorityList = (from fpData in GetSeqData
                                         select new LeadFieldPriorityDTO
                                         {
                                             Priority = fpData.Priority,
                                             FieldName = fpData.FieldName
                                         }
                                       ).ToList();
                }
                else
                {

                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 1, FieldName = "LeadStatusTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 2, FieldName = "CustomerTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 3, FieldName = "MobileNoTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 4, FieldName = "EmailIdTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 5, FieldName = "DateofBirthTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 6, FieldName = "MarriageAnniversaryTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 7, FieldName = "FollowDateTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 8, FieldName = "DesignationTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 9, FieldName = "OrganizationNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 10, FieldName = "CountryTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 11, FieldName = "ZoneNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 12, FieldName = "FollowUpTimeTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 13, FieldName = "FollowupTimeinISTTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 14, FieldName = "StateTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 15, FieldName = "CityTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 16, FieldName = "AddressTextNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 17, FieldName = "UrlTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 18, FieldName = "SkypeIdTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 19, FieldName = "LeadResourceTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 20, FieldName = "ProductTypeNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 21, FieldName = "ExpectedDateTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 22, FieldName = "ExpectedProductAmountTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 23, FieldName = "ExtraCol1TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 24, FieldName = "ExtraCol2TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 25, FieldName = "ExtraCol3TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 26, FieldName = "ExtraCol4TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 27, FieldName = "ExtraCol5TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 28, FieldName = "ExtraCol6TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 29, FieldName = "ExtraCol7TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 30, FieldName = "ExtraCol8TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 31, FieldName = "ExtraCol9TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 32, FieldName = "ExtraCol10TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 33, FieldName = "ExtraCol11TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 34, FieldName = "ExtraCol12TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 35, FieldName = "ExtraCol13TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 36, FieldName = "ExtraCol14TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 37, FieldName = "ExtraCol15TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 38, FieldName = "ExtraCol16TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 39, FieldName = "ExtraCol17TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 40, FieldName = "ExtraCol18TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 41, FieldName = "ExtraCol19TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 42, FieldName = "ExtraCol20TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 43, FieldName = "LeadOwnerTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 44, FieldName = "DescriptionTextName" });

                    
                    foreach (var item in fieldPriorityList)
                    {
                        var fp = new crm_create_lead_field_sequence
                        {
                            Priority = item.Priority,
                            FieldName = item.FieldName,
                            CompanyID = companyID,
                            BranchID = branchID,
                            CreatedBy = uid,
                            Createddate = dt
                           
                        };
                        db.crm_create_lead_field_sequence.Add(fp);
                    }
                   await db.SaveChangesAsync();
                }

                #endregion

                #region fields names list for show dropdown

                string LeadOwnerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Lead Owner";
                int? ldSeqNo = fieldPriorityList.Where(a => a.FieldName == "LeadOwnerTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = LeadOwnerTextName, ColumnName = "LeadOwnerTextName", IsActive = true, IsRequired = true, FieldType = "NormalText", Priority = ldSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                string LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                int? lsSeqNo = fieldPriorityList.Where(a => a.FieldName == "LeadStatusTextName").Select(a => a.Priority).FirstOrDefault();               
                fieldsList.Add(new TktFormFieldsModel { TextName = LeadStatusTextName, ColumnName = "LeadStatusTextName", IsActive = true, IsRequired = true, FieldType = "DropDownList", Priority = lsSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                string CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                int? cnmSeqNo = fieldPriorityList.Where(a => a.FieldName == "CustomerTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = CustomerTextName, ColumnName = "CustomerTextName", IsActive = true, IsRequired = true, FieldType = "NormalText", Priority = cnmSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                string MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                int? mobSeqNo = fieldPriorityList.Where(a => a.FieldName == "MobileNoTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = MobileNoTextName, ColumnName = "MobileNoTextName", IsActive = true, IsRequired = true, FieldType = "NormalText", Priority = mobSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                string FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                int? fudSeqNo = fieldPriorityList.Where(a => a.FieldName == "FollowDateTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = FollowDateTextName, ColumnName = "FollowDateTextName", IsActive = true, IsRequired = true, FieldType = "DateText", Priority = fudSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                bool IsFollowUpTime = (GetData == null || GetData.IsFollowUpTime) ? true : false;
                bool IsFollowUpTimeRequired = GetData != null && GetData.IsFUpTimeMandatory ? true : false;
                string FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                int? futSeqNo = fieldPriorityList.Where(a => a.FieldName == "FollowUpTimeTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = FollowUpTimeTextName, ColumnName = "FollowUpTimeTextName", IsActive = IsFollowUpTime, IsRequired = IsFollowUpTimeRequired, FieldType = "TimeText", Priority = futSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsFollowupTimeinIST = (GetData == null || GetData.IsFollowupTimeinIST) ? true : false;
                bool IsFollowupTimeinISTRequired = GetData != null && GetData.IsFupTimeinISTMandatory ? true : false;
                string FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";
                int? futistSeqNo = fieldPriorityList.Where(a => a.FieldName == "FollowupTimeinISTTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = FollowupTimeinISTTextName, ColumnName = "FollowupTimeinISTTextName", IsActive = IsFollowupTimeinIST, IsRequired = IsFollowupTimeinISTRequired, FieldType = "TimeText", Priority = futistSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsTZone = (GetData == null || GetData.IsTimeZoneName) ? true : false;
                bool IsTZoneReq = (GetData != null && GetData.IsTZNameMandatory) ? true : false;
                string ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";
                int? znSeqNo = fieldPriorityList.Where(a => a.FieldName == "ZoneNameTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ZoneNameTextName, ColumnName = "ZoneNameTextName", IsActive = IsTZone, IsRequired = IsTZoneReq, FieldType = "DropDownList", Priority = znSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsDateofBirth = (GetData == null || GetData.IsDateofBirth) ? true : false;
                bool IsDateofBirthRequired = (GetData != null && GetData.IsDOBMandatory) ? true :false;
                string DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : string.Empty;
                int? dobSeqNo = fieldPriorityList.Where(a => a.FieldName == "DateofBirthTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = DateofBirthTextName, ColumnName = "DateofBirthTextName", IsActive = IsDateofBirth, IsRequired = IsDateofBirthRequired, FieldType = "DateText", Priority = dobSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsMrgAnni = (GetData == null || GetData.IsMarriageAnniversary) ? true : false;
                bool IsMrgAnniReq = (GetData != null && GetData.IsMrgAniMandatory) ? true : false;
                string MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : string.Empty;
                int? mrgSeqNo = fieldPriorityList.Where(a => a.FieldName == "MarriageAnniversaryTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = MarriageAnniversaryTextName, ColumnName = "MarriageAnniversaryTextName", IsActive = IsMrgAnni, IsRequired = IsMrgAnniReq, FieldType = "DateText", Priority = mrgSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsDesignation = (GetData == null || GetData.IsDesignation) ? true : false;
                bool IsDesignationRequired = (GetData != null && GetData.IsDesigMandatory) ? true : false;
                string DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : string.Empty;
                int? dsgSeqNo = fieldPriorityList.Where(a => a.FieldName == "DesignationTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = DesignationTextName, ColumnName = "DesignationTextName", IsActive = IsDesignation, IsRequired = IsDesignationRequired, FieldType = "NormalText", Priority = dsgSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsEmail = (GetData == null || GetData.IsEmailId) ? true : false;
                bool IsEmailReq = (GetData != null && GetData.IsEmailMandatory) ? true : false;
                string EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";
                int? emlSeqNo = fieldPriorityList.Where(a => a.FieldName == "EmailIdTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = EmailIdTextName, ColumnName = "EmailIdTextName", IsActive = IsEmail, IsRequired = IsEmailReq, FieldType = "EmailText", Priority = emlSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });


                bool IsAddress = (GetData == null || GetData.IsAddress) ? true : false;
                bool IsAddressRequired = (GetData != null && GetData.IsAddressMandatory) ? true : false;
                string AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.AddressTextNameTextName : string.Empty;
                int? addrSeqNo = fieldPriorityList.Where(a => a.FieldName == "AddressTextNameTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = AddressTextNameTextName, ColumnName = "AddressTextNameTextName", IsActive = IsAddress, IsRequired = IsAddressRequired, FieldType = "NormalText", Priority = addrSeqNo, CanChangeFieldType = true, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsCountry = (GetData == null || GetData.IsCountry) ? true : false;
                bool IsCountryReq = (GetData != null && GetData.IsCountryMandatory) ? true : false;
                string CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";
                int? coutSeqNo = fieldPriorityList.Where(a => a.FieldName == "CountryTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = CountryTextName, ColumnName = "CountryTextName", IsActive = IsCountry, IsRequired = IsCountryReq, FieldType = "DropDownList", Priority = coutSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsState = (GetData == null || GetData.IsState) ? true : false;
                bool IsStateReq = (GetData != null && GetData.IsStateMandatory) ? true : false;
                string StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";
                int? statSeqNo = fieldPriorityList.Where(a => a.FieldName == "StateTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = StateTextName, ColumnName = "StateTextName", IsActive = IsState, IsRequired = IsStateReq, FieldType = "DropDownList", Priority = statSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });


                bool IsCity = (GetData == null || GetData.IsCity) ? true : false;
                bool IsCityReq = (GetData != null && GetData.IsCityMandatory) ? true : false;
                string CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City/Location";
                int? ctySeqNo = fieldPriorityList.Where(a => a.FieldName == "CityTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = CityTextName, ColumnName = "CityTextName", IsActive = IsCity, IsRequired = IsCityReq, FieldType = "DropDownList", Priority = ctySeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsUrl = (GetData == null || GetData.IsUrl) ? true : false;
                bool IsUrlReq = (GetData != null && GetData.IsUrlMandatory) ? true : false;
                string UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : string.Empty;
                int? urlSeqNo = fieldPriorityList.Where(a => a.FieldName == "UrlTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = UrlTextName, ColumnName = "UrlTextName", IsActive = IsUrl, IsRequired = IsUrlReq, FieldType = "NormalText", Priority = urlSeqNo, CanChangeFieldType = true, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsSkype = (GetData == null || GetData.IsSkypeId) ? true : false;
                bool IsSkypReq = (GetData != null && GetData.IsSkypeIdMandatory) ? true : false;
                string SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : string.Empty;
                int? skyidSeqNo = fieldPriorityList.Where(a => a.FieldName == "SkypeIdTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = SkypeIdTextName, ColumnName = "SkypeIdTextName", IsActive = IsSkype, IsRequired = IsSkypReq, FieldType = "NormalText", Priority = skyidSeqNo, CanChangeFieldType = true, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsLeadResource = (GetData == null || GetData.IsLeadResource) ? true : false;
                bool IsLeadResourceRequired = (GetData != null && GetData.IsLdResMandatory) ? true : false;
                string LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                int? lrSeqNo = fieldPriorityList.Where(a => a.FieldName == "LeadResourceTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = LeadResourceTextName, ColumnName = "LeadResourceTextName", IsActive = IsLeadResource, IsRequired = IsLeadResourceRequired, FieldType = "DropDownList", Priority = lrSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsPrdT = (GetData == null || GetData.IsProductType) ? true : false;
                bool IsPrdTReq = (GetData != null && GetData.IsProdTypeMandatory) ? true : false;
                string ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                int? ptSeqNo = fieldPriorityList.Where(a => a.FieldName == "ProductTypeNameTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ProductTypeNameTextName, ColumnName = "ProductTypeNameTextName", IsActive = IsPrdT, IsRequired = IsPrdTReq, FieldType = "DropDownList", Priority = ptSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsExpDate = (GetData == null || GetData.IsExpectedClosingDate) ? true : false;
                bool IsExpDateReq = (GetData != null && GetData.IsExpClsDateMandatory) ? true : false;
                string ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : string.Empty;
                int? expDtSeqNo = fieldPriorityList.Where(a => a.FieldName == "ExpectedDateTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExpectedDateTextName, ColumnName = "ExpectedDateTextName", IsActive = IsExpDate, IsRequired = IsExpDateReq, FieldType = "DateText", Priority = expDtSeqNo, CanChangeFieldType = true, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsExpAmt = (GetData == null || GetData.IsExpectedDealAmount) ? true : false;
                bool IsExpAmtReq = (GetData != null && GetData.IsExpDealAmtMandatory) ? true : false;
                string ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : string.Empty;
                int? expAmtSeqNo = fieldPriorityList.Where(a => a.FieldName == "ExpectedProductAmountTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExpectedProductAmountTextName, ColumnName = "ExpectedProductAmountTextName", IsActive = IsExpAmt, IsRequired = IsExpAmtReq, FieldType = "DecimalText", Priority = expAmtSeqNo, CanChangeFieldType = true, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                string DescriptionTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";
                int? descSeqNo = fieldPriorityList.Where(a => a.FieldName == "DescriptionTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = DescriptionTextName, ColumnName = "DescriptionTextName", IsActive = true, IsRequired = true, FieldType = "NormalText", Priority = descSeqNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                bool IsOrgName = (GetData == null || GetData.IsOrganizationName) ? true : false;
                bool IsOrgNameReq = (GetData != null && GetData.IsOrgNameMandatory) ? true : false;
                string OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : string.Empty;
                int? orgSeqNo = fieldPriorityList.Where(a => a.FieldName == "OrganizationNameTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = OrganizationNameTextName, ColumnName = "OrganizationNameTextName", IsActive = IsOrgName, IsRequired = IsOrgNameReq, FieldType = "NormalText", Priority = orgSeqNo, CanChangeFieldType = true, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol1 = (GetData != null && GetData.IsExtraCol1) ? true : false;
                bool IsExtraCol1Required = (GetData != null && GetData.IsExtraCol1Mandatory) ? true : false;
                string ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                int? ext1SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol1TextName, ColumnName = "ExtraCol1TextName", IsActive = IsExtraCol1, IsRequired = IsExtraCol1Required, FieldType = "NormalText", Priority = ext1SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol2 = (GetData != null && GetData.IsExtraCol2) ? true : false;
                bool IsExtraCol2Required = (GetData != null && GetData.IsExtraCol2Mandatory) ? true : false;
                string ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                int? ext2SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol2TextName, ColumnName = "ExtraCol2TextName", IsActive = IsExtraCol2, IsRequired = IsExtraCol2Required, FieldType = "NormalText", Priority = ext2SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                
                bool IsExtraCol3 = (GetData != null && GetData.IsExtraCol3) ? true : false;
                bool IsExtraCol3Required = (GetData != null && GetData.IsExtraCol3Mandatory) ? true : false;
                string ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                int? ext3SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol3TextName, ColumnName = "ExtraCol3TextName", IsActive = IsExtraCol3, IsRequired = IsExtraCol3Required, FieldType = "NormalText", Priority = ext3SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                
                bool IsExtraCol4 = (GetData != null && GetData.IsExtraCol4) ? true : false;
                bool IsExtraCol4Required = (GetData != null && GetData.IsExtraCol4Mandatory) ? true : false;
                string ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                int? ext4SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol4TextName, ColumnName = "ExtraCol4TextName", IsActive = IsExtraCol4, IsRequired = IsExtraCol4Required, FieldType = "NormalText", Priority = ext4SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol5 = (GetData != null && GetData.IsExtraCol5) ? true : false;
                bool IsExtraCol5Required = (GetData != null && GetData.IsExtraCol5Mandatory) ? true : false;
                string ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                int? ext5SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol5TextName, ColumnName = "ExtraCol5TextName", IsActive = IsExtraCol5, IsRequired = IsExtraCol5Required, FieldType = "NormalText", Priority = ext5SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                
                bool IsExtraCol6 = (GetData != null && GetData.IsExtraCol6) ? true : false;
                bool IsExtraCol6Required = (GetData != null && GetData.IsExtraCol6Mandatory) ? true : false;
                string ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                int? ext6SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol6TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol6TextName, ColumnName = "ExtraCol6TextName", IsActive = IsExtraCol6, IsRequired = IsExtraCol6Required, FieldType = "DecimalText", Priority = ext6SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                
                bool IsExtraCol7 = (GetData != null && GetData.IsExtraCol7) ? true : false;
                bool IsExtraCol7Required = (GetData != null && GetData.IsExtraCol7Mandatory) ? true : false;
                string ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                int? ext7SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol7TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol7TextName, ColumnName = "ExtraCol7TextName", IsActive = IsExtraCol7, IsRequired = IsExtraCol7Required, FieldType = "NumberText", Priority = ext7SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                
                bool IsExtraCol8 = (GetData != null && GetData.IsExtraCol8) ? true : false;
                bool IsExtraCol8Required = (GetData != null && GetData.IsExtraCol8Mandatory) ? true : false;
                string ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                int? ext8SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol8TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol8TextName, ColumnName = "ExtraCol8TextName", IsActive = IsExtraCol8, IsRequired = IsExtraCol8Required, FieldType = "NumberText", Priority = ext8SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                
                bool IsExtraCol9 = (GetData != null && GetData.IsExtraCol9) ? true : false;
                bool IsExtraCol9Required = (GetData != null && GetData.IsExtraCol9Mandatory) ? true : false;
                string ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                int? ext9SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol9TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol9TextName, ColumnName = "ExtraCol9TextName", IsActive = IsExtraCol9, IsRequired = IsExtraCol9Required, FieldType = "DateText", Priority = ext9SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol10 = (GetData != null && GetData.IsExtraCol10) ? true : false;
                bool IsExtraCol10Required = (GetData != null && GetData.IsExtraCol10Mandatory) ? true : false;
                string ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                int? ext10SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol10TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol10TextName, ColumnName = "ExtraCol10TextName", IsActive = IsExtraCol10, IsRequired = IsExtraCol10Required, FieldType = "DateText", Priority = ext10SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                
                bool IsExtraCol11 = (GetData != null && GetData.IsExtraCol11) ? true : false;
                bool IsExtraCol11Required = (GetData != null && GetData.IsExtraCol11Mandatory) ? true : false;
                string ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                int? ext11SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol11TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol11TextName, ColumnName = "ExtraCol11TextName", IsActive = IsExtraCol11, IsRequired = IsExtraCol11Required, FieldType = "DecimalText", Priority = ext11SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol12 = (GetData != null && GetData.IsExtraCol12) ? true : false;
                bool IsExtraCol12Required = (GetData != null && GetData.IsExtraCol12Mandatory) ? true : false;
                string ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                int? ext12SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol12TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol12TextName, ColumnName = "ExtraCol12TextName", IsActive = IsExtraCol12, IsRequired = IsExtraCol12Required, FieldType = "DecimalText", Priority = ext12SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol13 = (GetData != null && GetData.IsExtraCol13) ? true : false;
                bool IsExtraCol13Required = (GetData != null && GetData.IsExtraCol13Mandatory) ? true : false;
                string ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                int? ext13SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol13TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol13TextName, ColumnName = "ExtraCol13TextName", IsActive = IsExtraCol13, IsRequired = IsExtraCol13Required, FieldType = "DecimalText", Priority = ext13SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol14 = (GetData != null && GetData.IsExtraCol14) ? true : false;
                bool IsExtraCol14Required = (GetData != null && GetData.IsExtraCol14Mandatory) ? true : false;
                string ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                int? ext14SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol14TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol14TextName, ColumnName = "ExtraCol14TextName", IsActive = IsExtraCol14, IsRequired = IsExtraCol14Required, FieldType = "DecimalText", Priority = ext14SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol15 = (GetData != null && GetData.IsExtraCol15) ? true : false;
                bool IsExtraCol15Required = (GetData != null && GetData.IsExtraCol15Mandatory) ? true : false;
                string ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                int? ext15SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol15TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol15TextName, ColumnName = "ExtraCol15TextName", IsActive = IsExtraCol15, IsRequired = IsExtraCol15Required, FieldType = "NumberText", Priority = ext15SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol16 = (GetData != null && GetData.IsExtraCol16) ? true : false;
                bool IsExtraCol16Required = (GetData != null && GetData.IsExtraCol16Mandatory) ? true : false;
                string ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                int? ext16SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol16TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol16TextName, ColumnName = "ExtraCol16TextName", IsActive = IsExtraCol16, IsRequired = IsExtraCol16Required, FieldType = "NumberText", Priority = ext16SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                
                bool IsExtraCol17 = (GetData != null && GetData.IsExtraCol17) ? true : false;
                bool IsExtraCol17Required = (GetData != null && GetData.IsExtraCol17Mandatory) ? true : false;
                string ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                int? ext17SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol17TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol17TextName, ColumnName = "ExtraCol17TextName", IsActive = IsExtraCol17, IsRequired = IsExtraCol17Required, FieldType = "NumberText", Priority = ext17SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol18 = (GetData != null && GetData.IsExtraCol18) ? true : false;
                bool IsExtraCol18Required = (GetData != null && GetData.IsExtraCol18Mandatory) ? true : false;
                string ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                int? ext18SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol18TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol18TextName, ColumnName = "ExtraCol18TextName", IsActive = IsExtraCol18, IsRequired = IsExtraCol18Required, FieldType = "DateText", Priority = ext18SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                
                bool IsExtraCol19 = (GetData != null && GetData.IsExtraCol19) ? true : false;
                bool IsExtraCol19Required = (GetData != null && GetData.IsExtraCol19Mandatory) ? true : false;
                string ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                int? ext19SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol19TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol19TextName, ColumnName = "ExtraCol19TextName", IsActive = IsExtraCol19, IsRequired = IsExtraCol19Required, FieldType = "DateText", Priority = ext19SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                bool IsExtraCol20 = (GetData != null && GetData.IsExtraCol20) ? true : false;
                bool IsExtraCol20Required = (GetData != null && GetData.IsExtraCol20Mandatory) ? true : false;
                string ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                int? ext20SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol20TextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol20TextName, ColumnName = "ExtraCol20TextName", IsActive = IsExtraCol20, IsRequired = IsExtraCol20Required, FieldType = "DateText", Priority = ext20SeqNo, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });

                #endregion
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {

                return Request.CreateResponse(HttpStatusCode.OK, fieldsList);
            }
        }

        //api/CRM_LeadFormMaster/GetLeadFormFieldTypeDDL
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetLeadFormFieldTypeDDL() 
        {
            var fieldTypeList = new List<SelectListItem> {
                new SelectListItem { Text= "Choose Field Type",Value="" },
                new SelectListItem { Text= "Normal text (E.g.: Hello!)",Value="NormalText" },
                new SelectListItem { Text= "Date(E.g.: DD/MM/YYYY)",Value="DateText" },
                new SelectListItem { Text= "Number(E.g.:1 to 10)",Value="NumberText" },
                new SelectListItem { Text= "Decimal(E.g.: Rs. 2.50)",Value="DecimalText" },
                new SelectListItem { Text= "Email",Value="EmailText" },
                new SelectListItem { Text= "Dropdown List",Value="DropDownList" },
                new SelectListItem{Text="Time",Value="TimeText"}
            };
            return Request.CreateResponse(HttpStatusCode.OK, fieldTypeList);
        }

        //api/CRM_LeadFormMaster/DelLeadFormField?ColumnName=ExtraCol2TextName&CompanyID=307&BranchID=184&UID=61&Token=VwFdB3OPEwOoHnr15a5qgg==
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> DelLeadFormField(string ColumnName, string CompanyID, string BranchID, string UID, string Token)
        {

            string msg = string.Empty;

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    int branchID = Convert.ToInt32(BranchID);
                    int companyID = Convert.ToInt32(CompanyID);
                    var uid = string.IsNullOrEmpty(UID) ? Convert.ToInt32(UID) : 0;
                    //System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}

                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //     Token = headers.GetValues("Token").First();
                    //}

                    var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        msg = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(msg);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    var dt = Constant.GetimeForApi(companyID);
                    string FieldName = ColumnName;
                    var GetData = await db.crm_createleadsetting.Where(a => a.BranchID == branchID && a.CompanyID == companyID).FirstOrDefaultAsync();
                    var GetFormData = await db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                    var GetSeqData = await db.crm_create_lead_field_sequence.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.FieldName == ColumnName).FirstOrDefaultAsync();
                    if (GetData != null && GetFormData != null)
                    {
                        if (!string.IsNullOrEmpty(ColumnName))//check field name not null
                        {

                            //if (ColumnName == "DesignationTextName")
                            //{
                            //    GetFormData.DesignationTextName = null;
                            //    GetData.IsDesignation = false;
                            //    GetData.IsDesigMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "DateofBirthTextName")
                            //{
                            //    GetFormData.DateofBirthTextName = null;
                            //    GetData.IsDateofBirth = false;
                            //    GetData.IsDOBMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "MarriageAnniversaryTextName")
                            //{
                            //    GetFormData.MarriageAnniversaryTextName = null;
                            //    GetData.IsMarriageAnniversary = false;
                            //    GetData.IsMrgAniMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "OrganizationNameTextName")
                            //{
                            //    GetFormData.OrganizationNameTextName = null;
                            //    GetData.IsOrganizationName = false;
                            //    GetData.IsOrgNameMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "AddressTextNameTextName")
                            //{
                            //    GetFormData.AddressTextNameTextName = null;
                            //    GetData.IsAddress = false;
                            //    GetData.IsAddressMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "UrlTextName")
                            //{
                            //    GetFormData.UrlTextName = null;
                            //    GetData.IsUrl = false;
                            //    GetData.IsUrlMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "SkypeIdTextName")
                            //{
                            //    GetFormData.SkypeIdTextName = null;
                            //    GetData.IsSkypeId = false;
                            //    GetData.IsSkypeIdMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "ExpectedDateTextName")
                            //{
                            //    GetFormData.ExpectedDateTextName = null;
                            //    GetData.IsExpectedClosingDate = false;
                            //    GetData.IsExpClsDateMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "ExpectedProductAmountTextName")
                            //{
                            //    GetFormData.ExpectedProductAmountTextName = null;
                            //    GetData.IsExpectedDealAmount = false;
                            //    GetData.IsExpDealAmtMandatory = false;
                            //    GetSeqData.Priority = 0;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    msg = "ok";
                            //    //return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            if (FieldName == "ExtraCol1TextName")
                            {
                                GetFormData.ExtraCol1TextName = null;
                                GetData.IsExtraCol1 = false;
                                GetData.IsExtraCol1Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol2TextName")
                            {
                                GetFormData.ExtraCol2TextName = null;
                                GetData.IsExtraCol2 = false;
                                GetData.IsExtraCol2Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol3TextName")
                            {
                                GetFormData.ExtraCol3TextName = null;
                                GetData.IsExtraCol3 = false;
                                GetData.IsExtraCol3Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol4TextName")
                            {
                                GetFormData.ExtraCol4TextName = null;
                                GetData.IsExtraCol4 = false;
                                GetData.IsExtraCol4Mandatory = false;
                                db.SaveChanges();
                                GetSeqData.Priority = 0;
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol5TextName")
                            {
                                GetFormData.ExtraCol5TextName = null;
                                GetData.IsExtraCol5 = false;
                                GetData.IsExtraCol5Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol6TextName")
                            {
                                GetFormData.ExtraCol6TextName = null;
                                GetData.IsExtraCol6 = false;
                                GetData.IsExtraCol6Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol7TextName")
                            {
                                GetFormData.ExtraCol7TextName = null;
                                GetData.IsExtraCol7 = false;
                                GetData.IsExtraCol7Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol8TextName")
                            {
                                GetFormData.ExtraCol8TextName = null;
                                GetData.IsExtraCol8 = false;
                                GetData.IsExtraCol8Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol9TextName")
                            {
                                GetFormData.ExtraCol9TextName = null;
                                GetData.IsExtraCol9 = false;
                                GetData.IsExtraCol9Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol10TextName")
                            {
                                GetFormData.ExtraCol10TextName = null;
                                GetData.IsExtraCol10 = false;
                                GetData.IsExtraCol10Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol11TextName")
                            {
                                GetFormData.ExtraCol11TextName = null;
                                GetData.IsExtraCol11 = false;
                                GetData.IsExtraCol11Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol12TextName")
                            {
                                GetFormData.ExtraCol12TextName = null;
                                GetData.IsExtraCol12 = false;
                                GetData.IsExtraCol12Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol13TextName")
                            {
                                GetFormData.ExtraCol13TextName = null;
                                GetData.IsExtraCol13 = false;
                                GetData.IsExtraCol13Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol14TextName")
                            {
                                GetFormData.ExtraCol14TextName = null;
                                GetData.IsExtraCol14 = false;
                                GetData.IsExtraCol14Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol15TextName")
                            {
                                GetFormData.ExtraCol15TextName = null;
                                GetData.IsExtraCol15 = false;
                                GetData.IsExtraCol15Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol16TextName")
                            {
                                GetFormData.ExtraCol16TextName = null;
                                GetData.IsExtraCol16 = false;
                                GetData.IsExtraCol16Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol17TextName")
                            {
                                GetFormData.ExtraCol17TextName = null;
                                GetData.IsExtraCol17 = false;
                                GetData.IsExtraCol17Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol18TextName")
                            {
                                GetFormData.ExtraCol18TextName = null;
                                GetData.IsExtraCol18 = false;
                                GetData.IsExtraCol18Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol19TextName")
                            {
                                GetFormData.ExtraCol19TextName = null;
                                GetData.IsExtraCol19 = false;
                                GetData.IsExtraCol19Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol20TextName")
                            {
                                GetFormData.ExtraCol20TextName = null;
                                GetData.IsExtraCol20 = false;
                                GetData.IsExtraCol20Mandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            //else if (FieldName == "CustomerTextName")
                            //{
                            //    GetFormData.CustomerTextName = null;
                            //    GetData.IsCustomer = true;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "MobileNoTextName")
                            //{
                            //    GetFormData.MobileNoTextName = null;
                            //    GetData.IsMobileNo = true;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "LeadStatusTextName")
                            //{
                            //    GetFormData.LeadStatusTextName = null;
                            //    GetData.IsLeadStatus = true;
                            //    GetData.IsLdStatusMandatory = true;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "LeadOwnerTextName")
                            //{
                            //    GetFormData.LeadOwnerTextName = null;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "FollowDateTextName")
                            //{
                            //    GetFormData.FollowDateTextName = null;
                            //    GetData.IsFollowupDate = true;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "DescriptionTextName")
                            //{
                            //    GetFormData.DescriptionTextName = null;
                            //    GetData.IsDescription = true;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "EmailIdTextName")
                            //{
                            //    GetFormData.EmailIdTextName = null;
                            //    GetData.IsEmailId = false;
                            //    GetData.IsEmailMandatory =  false;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "CountryTextName")
                            //{
                            //    GetData.IsCountry = false;
                            //    GetData.IsCountryMandatory =  false;
                            //    GetFormData.CountryTextName = null;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "StateTextName")
                            //{
                            //    GetData.IsState = false;
                            //    GetData.IsStateMandatory =  false;
                            //    GetFormData.StateTextName = null;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "CityTextName")
                            //{
                            //    GetData.IsCity = false;
                            //    GetData.IsCityMandatory =  false;
                            //    GetFormData.CityTextName =null;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "FollowUpTimeTextName")
                            //{
                            //    GetFormData.FollowUpTimeTextName = null;
                            //    GetData.IsFollowUpTime = false;
                            //    GetData.IsFUpTimeMandatory = false;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "FollowupTimeinISTTextName")
                            //{
                            //    GetFormData.FollowupTimeinISTTextName = null;
                            //    GetData.IsFollowupTimeinIST = false;
                            //    GetData.IsFupTimeinISTMandatory =  false;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "ZoneNameTextName")
                            //{
                            //    GetFormData.ZoneNameTextName = null;
                            //    GetData.IsTimeZoneName = false;
                            //    GetData.IsTZNameMandatory =  false;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "LeadResourceTextName")
                            //{
                            //    GetFormData.LeadResourceTextName =null;
                            //    GetData.IsLeadResource = false;
                            //    GetData.IsLdResMandatory = false;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                            //else if (FieldName == "ProductTypeNameTextName")
                            //{
                            //    GetFormData.ProductTypeNameTextName = null;
                            //    GetData.IsProductType = false;
                            //    GetData.IsProdTypeMandatory =  false;
                            //    db.SaveChanges();
                            //    trans.Commit();
                            //    return Json("ok", JsonRequestBehavior.AllowGet);
                            //}
                        }
                        else
                        {
                            msg = string.Format("Please enter column name");
                            //return Json("err", JsonRequestBehavior.AllowGet);
                        }
                    }//if get data not null end
                    else
                    {
                        msg = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");

                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    msg = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                }
            }



            if (!string.IsNullOrEmpty(msg) && msg != "ok")
            {
                HttpError err = new HttpError(msg);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Delete field successfully");
            }

        }


        //api/CRM_LeadFormMaster/ChangeLeadFormFieldsSequnce
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ChangeLeadFormFieldsSequnce(HttpRequestMessage request)
        {
            var msg = "";
            try
            {
                var json = await request.Content.ReadAsStringAsync();
                FieldsSequenceModel Data = JsonConvert.DeserializeObject<FieldsSequenceModel>(json);
                int branchID = Convert.ToInt32(Data.BranchID);
                int companyID = Convert.ToInt32(Data.CompanyID);
                int uid = Convert.ToInt32(Data.UID);

                var auth = Utility.TokenVerify(companyID, Data.Token);//verify token for is authorized user

                if (auth == false)
                {
                    msg = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(msg);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                if (Data.Sequences != null && Data.Sequences.Count > 0)
                {

                    var dt = Constant.GetimeForApi(companyID);
                    for (int i = 0; i < Data.Sequences.Count; i++)
                    {
                        string fieldName = Data.Sequences[i].ColumnName;
                        int Priority = Data.Sequences[i].Priority;
                        var GetSeqData = await db.crm_create_lead_field_sequence.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.FieldName == fieldName).FirstOrDefaultAsync();
                        if (GetSeqData != null)
                        {
                            GetSeqData.Priority = Priority;
                            GetSeqData.ModifiedDate = dt;
                            db.SaveChanges();
                        }
                        else
                        {
                            var fp = new crm_create_lead_field_sequence
                            {
                                Priority = Priority,
                                FieldName = fieldName,
                                CompanyID = companyID,
                                BranchID = branchID,
                                CreatedBy = uid,
                                Createddate = dt
                            };
                            db.crm_create_lead_field_sequence.Add(fp);
                            await db.SaveChangesAsync();
                        }
                    }

                }
                else
                {
                    var message = string.Format("** Somthing went wrong, while reading data, Please check the Post Data Parameters **");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("** Somthing went wrong, while reading data, Please check the Post Data Parameters **");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "Fields priority updated suceessfully");
        }

        //api/CRM_LeadFormMaster/LeadFormFieldAdd_Update
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> LeadFormFieldAdd_Update([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    TicketFormFieldAddUpdateModel model = JsonConvert.DeserializeObject<TicketFormFieldAddUpdateModel>(postData.ToString());

                    int BranchID = Convert.ToInt32(model.BranchID);
                    int CompanyID = Convert.ToInt32(model.CompanyID);

                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}
                    var auth = Utility.TokenVerify(CompanyID, model.Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        ErrorMessage = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(model.SaveType))
                    {
                        ErrorMessage = string.Format("** Save type is empty");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    if (string.IsNullOrEmpty(model.FieldType))
                    {
                        ErrorMessage = string.Format("** Field type is empty");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    if (string.IsNullOrEmpty(model.TextName))
                    {
                        ErrorMessage = string.Format("** Text name is empty");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    //if (string.IsNullOrEmpty(model.FieldName))
                    //{
                    //    ErrorMessage = string.Format("** Field name is empty");
                    //    HttpError err = new HttpError(ErrorMessage);
                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    //}


                    var GetData = await db.crm_createleadsetting.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                    var GetFormData = await db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();

                    if (GetData != null && GetFormData != null)
                    {
                        int FieldPriority = 0;
                        var todayDate = Constant.GetimeForApi(CompanyID);
                        if (model.SaveType == "New" && !string.IsNullOrEmpty(model.FieldType) && !string.IsNullOrEmpty(model.TextName))//check if save type is not null for add new field
                        {
                            if (model.FieldType == "NormalText")//check field type for insert column string data type
                            {
                                if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                {
                                    GetFormData.DesignationTextName = model.TextName;
                                    GetData.IsDesignation = model.IsActive;
                                    GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'DesignationTextName'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                {
                                    GetFormData.OrganizationNameTextName = model.TextName;
                                    GetData.IsOrganizationName = model.IsActive;
                                    GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'OrganizationNameTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                {
                                    GetFormData.AddressTextNameTextName = model.TextName;
                                    GetData.IsAddress = model.IsActive;
                                    GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'AddressTextNameTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                {
                                    GetFormData.UrlTextName = model.TextName;
                                    GetData.IsUrl = model.IsActive;
                                    GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'UrlTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                {
                                    GetFormData.SkypeIdTextName = model.TextName;
                                    GetData.IsSkypeId = model.IsActive;
                                    GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'SkypeIdTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                {
                                    GetFormData.ExtraCol1TextName = model.TextName;
                                    GetData.IsExtraCol1 = model.IsActive;
                                    GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                {
                                    GetFormData.ExtraCol2TextName = model.TextName;
                                    GetData.IsExtraCol2 = model.IsActive;
                                    GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                {
                                    GetFormData.ExtraCol3TextName = model.TextName;
                                    GetData.IsExtraCol3 = model.IsActive;
                                    GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3TextName'");

                                    db.SaveChanges();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                {
                                    GetFormData.ExtraCol4TextName = model.TextName;
                                    GetData.IsExtraCol4 = model.IsActive;
                                    GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                {
                                    GetFormData.ExtraCol5TextName = model.TextName;
                                    GetData.IsExtraCol5 = model.IsActive;
                                    GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No Field available for normal text type";
                                }
                            }
                            else if (model.FieldType == "DecimalText")
                            {
                                if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                {
                                    GetFormData.ExpectedProductAmountTextName = model.TextName;
                                    GetData.IsExpectedDealAmount = model.IsActive;
                                    GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExpectedProductAmountTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                {
                                    GetFormData.ExtraCol6TextName = model.TextName;
                                    GetData.IsExtraCol6 = model.IsActive;
                                    GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                {
                                    GetFormData.ExtraCol11TextName = model.TextName;
                                    GetData.IsExtraCol11 = model.IsActive;
                                    GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                {
                                    GetFormData.ExtraCol12TextName = model.TextName;
                                    GetData.IsExtraCol12 = model.IsActive;
                                    GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                {
                                    GetFormData.ExtraCol13TextName = model.TextName;
                                    GetData.IsExtraCol13 = model.IsActive;
                                    GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol13TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                {
                                    GetFormData.ExtraCol14TextName = model.TextName;
                                    GetData.IsExtraCol14 = model.IsActive;
                                    GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol14TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No field available for decimal type";
                                }
                            }
                            else if (model.FieldType == "NumberText")
                            {
                                if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                {
                                    GetFormData.ExtraCol7TextName = model.TextName;
                                    GetData.IsExtraCol7 = model.IsActive;
                                    GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                {
                                    GetFormData.ExtraCol8TextName = model.TextName;
                                    GetData.IsExtraCol8 = model.IsActive;
                                    GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                {
                                    GetFormData.ExtraCol15TextName = model.TextName;
                                    GetData.IsExtraCol15 = model.IsActive;
                                    GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol15TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                {
                                    GetFormData.ExtraCol16TextName = model.TextName;
                                    GetData.IsExtraCol16 = model.IsActive;
                                    GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol16TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                {
                                    GetFormData.ExtraCol17TextName = model.TextName;
                                    GetData.IsExtraCol17 = model.IsActive;
                                    GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol17TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No field available for number type";
                                }
                            }
                            else if (model.FieldType == "DateText")
                            {
                                if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                {
                                    GetFormData.DateofBirthTextName = model.TextName;
                                    GetData.IsDateofBirth = model.IsActive;
                                    GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'DateofBirthTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                {
                                    GetFormData.MarriageAnniversaryTextName = model.TextName;
                                    GetData.IsMarriageAnniversary = model.IsActive;
                                    GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'MarriageAnniversaryTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                {
                                    GetFormData.ExpectedDateTextName = model.TextName;
                                    GetData.IsExpectedClosingDate = model.IsActive;
                                    GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExpectedDateTextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                {
                                    GetFormData.ExtraCol9TextName = model.TextName;
                                    GetData.IsExtraCol9 = model.IsActive;
                                    GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                {
                                    GetFormData.ExtraCol10TextName = model.TextName;
                                    GetData.IsExtraCol10 = model.IsActive;
                                    GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                {
                                    GetFormData.ExtraCol18TextName = model.TextName;
                                    GetData.IsExtraCol18 = model.IsActive;
                                    GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol18TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                {
                                    GetFormData.ExtraCol19TextName = model.TextName;
                                    GetData.IsExtraCol19 = model.IsActive;
                                    GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol19TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                {
                                    GetFormData.ExtraCol20TextName = model.TextName;
                                    GetData.IsExtraCol20 = model.IsActive;
                                    GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol20TextName'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No field available for date type";
                                }
                            }
                            else if (model.FieldType == "EmailText")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for email type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            else if (model.FieldType == "DropDownList")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for dropdown list type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            else if (model.FieldType == "TimeText")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for time type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            else
                            {
                                ErrorMessage = string.Format("Sorry! No field available for this field type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }

                        }
                        else if (!string.IsNullOrEmpty(model.TextName))//check field text name not null
                        {
                            if (model.FieldName != "EmailIdTextName" && model.FieldType == "EmailText")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for email type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            if ((model.FieldName != "CountryTextName" || model.FieldName != "ProductTypeNameTextName" || model.FieldName != "LeadResourceTextName" || model.FieldName != "ZoneNameTextName" || model.FieldName != "LeadStatusTextName" || model.FieldName != "StateTextName" || model.FieldName != "CityTextName") && model.FieldType == "DropDownList")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for dropdown list");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            if ((model.FieldName != "FollowUpTimeTextName" || model.FieldName != "FollowupTimeinISTTextName") && model.FieldType == "TimeText")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for time type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }

                            var GetSeqData = await db.crm_create_lead_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == model.FieldName).FirstOrDefaultAsync();
                            FieldPriority = GetSeqData == null ? 0 : GetSeqData.Priority;//set priority

                            if (model.FieldName == "CustomerTextName")
                            {
                                GetFormData.CustomerTextName = model.TextName;
                                GetData.IsCustomer = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "MobileNoTextName")
                            {
                                GetFormData.MobileNoTextName = model.TextName;
                                GetData.IsMobileNo = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "LeadStatusTextName")
                            {
                                GetFormData.LeadStatusTextName = model.TextName;
                                GetData.IsLeadStatus = true;
                                GetData.IsLdStatusMandatory = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "LeadOwnerTextName")
                            {
                                GetFormData.LeadOwnerTextName = model.TextName;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "FollowDateTextName")
                            {
                                GetFormData.FollowDateTextName = model.TextName;
                                GetData.IsFollowupDate = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "DescriptionTextName")
                            {
                                GetFormData.DescriptionTextName = model.TextName;
                                GetData.IsDescription = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "EmailIdTextName")
                            {
                                GetFormData.EmailIdTextName = model.TextName;
                                GetData.IsEmailId = model.IsActive;
                                GetData.IsEmailMandatory = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "CountryTextName")
                            {
                                GetData.IsCountry = model.IsActive;
                                GetData.IsCountryMandatory = model.IsActive == true ? model.IsRequired : false;
                                GetFormData.CountryTextName = model.TextName;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "StateTextName")
                            {
                                GetData.IsState = model.IsActive;
                                GetData.IsStateMandatory = model.IsActive == true ? model.IsRequired : false;
                                GetFormData.StateTextName = model.TextName;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "CityTextName")
                            {
                                GetData.IsCity = model.IsActive;
                                GetData.IsCityMandatory = model.IsActive == true ? model.IsRequired : false;
                                GetFormData.CityTextName = model.TextName;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "FollowUpTimeTextName")
                            {
                                GetFormData.FollowUpTimeTextName = model.TextName;
                                GetData.IsFollowUpTime = model.IsActive;
                                GetData.IsFUpTimeMandatory = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "FollowupTimeinISTTextName")
                            {
                                GetFormData.FollowupTimeinISTTextName = model.TextName;
                                GetData.IsFollowupTimeinIST = model.IsActive;
                                GetData.IsFupTimeinISTMandatory = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "ZoneNameTextName")
                            {
                                GetFormData.ZoneNameTextName = model.TextName;
                                GetData.IsTimeZoneName = model.IsActive;
                                GetData.IsTZNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";
                            }
                            else if (model.FieldName == "LeadResourceTextName")
                            {
                                GetFormData.LeadResourceTextName = model.TextName;
                                GetData.IsLeadResource = model.IsActive;
                                GetData.IsLdResMandatory = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "ProductTypeNameTextName")
                            {
                                GetFormData.ProductTypeNameTextName = model.TextName;
                                GetData.IsProductType = model.IsActive;
                                GetData.IsProdTypeMandatory = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "DesignationTextName")
                            {
                                if (model.FieldType == "NormalText")//if current field type equal to previous field type then update
                                {
                                    GetFormData.DesignationTextName = model.TextName;
                                    GetData.IsDesignation = model.IsActive;
                                    GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else//replace field to new field according field type
                                {
                                    #region replace designation field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.DesignationTextName = null;
                                            GetData.IsDesignation = false;
                                            GetData.IsDesigMandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "DateofBirthTextName")
                            {
                                if (model.FieldType == "DateText")//if current field type equal to previous field type
                                {
                                    GetFormData.DateofBirthTextName = model.TextName;
                                    GetData.IsDateofBirth = model.IsActive;
                                    GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }//if current field type equal to previous field type end
                                else
                                {
                                    #region replace date of birth field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.DateofBirthTextName = null;
                                            GetData.IsDateofBirth = false;
                                            GetData.IsDOBMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }
                            else if (model.FieldName == "MarriageAnniversaryTextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.MarriageAnniversaryTextName = model.TextName;
                                    GetData.IsMarriageAnniversary = model.IsActive;
                                    GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace merriage anniversary field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.MarriageAnniversaryTextName = null;
                                            GetData.IsMarriageAnniversary = false;
                                            GetData.IsMrgAniMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }
                            else if (model.FieldName == "OrganizationNameTextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.OrganizationNameTextName = model.TextName;
                                    GetData.IsOrganizationName = model.IsActive;
                                    GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else//replace field to new field according field type
                                {
                                    #region replace organization field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.OrganizationNameTextName = null;
                                            GetData.IsOrganizationName = false;
                                            GetData.IsOrgNameMandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "AddressTextNameTextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.AddressTextNameTextName = model.TextName;
                                    GetData.IsAddress = model.IsActive;
                                    GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else//replace field to new field according field type
                                {
                                    #region replace adress field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.AddressTextNameTextName = null;
                                            GetData.IsAddress = false;
                                            GetData.IsAddressMandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "UrlTextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.UrlTextName = model.TextName;
                                    GetData.IsUrl = model.IsActive;
                                    GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else//replace field to new field according field type
                                {
                                    #region replace url field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.UrlTextName = null;
                                            GetData.IsUrl = false;
                                            GetData.IsUrlMandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "SkypeIdTextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.SkypeIdTextName = model.TextName;
                                    GetData.IsSkypeId = model.IsActive;
                                    GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else//replace field to new field according field type
                                {
                                    #region replace url field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.SkypeIdTextName = null;
                                            GetData.IsSkypeId = false;
                                            GetData.IsSkypeIdMandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExpectedDateTextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExpectedDateTextName = model.TextName;
                                    GetData.IsExpectedClosingDate = model.IsActive;
                                    GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace expected date field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExpectedDateTextName = null;
                                            GetData.IsExpectedClosingDate = false;
                                            GetData.IsExpClsDateMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExpectedProductAmountTextName")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExpectedProductAmountTextName = model.TextName;
                                    GetData.IsExpectedDealAmount = model.IsActive;
                                    GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace designation field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                       
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExpectedProductAmountTextName = null;
                                            GetData.IsExpectedDealAmount = false;
                                            GetData.IsExpDealAmtMandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol1TextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol1TextName = model.TextName;
                                    GetData.IsExtraCol1 = model.IsActive;
                                    GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra1 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol2TextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol2TextName = model.TextName;
                                    GetData.IsExtraCol2 = model.IsActive;
                                    GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra1 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol1TextName = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol2TextName = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol3TextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol3TextName = model.TextName;
                                    GetData.IsExtraCol3 = model.IsActive;
                                    GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra1 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol4TextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol4TextName = model.TextName;
                                    GetData.IsExtraCol4 = model.IsActive;
                                    GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra1 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol3TextName = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol4TextName = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol5TextName")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol5TextName = model.TextName;
                                    GetData.IsExtraCol5 = model.IsActive;
                                    GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra1 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol5TextName = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol6TextName")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol6TextName = model.TextName;
                                    GetData.IsExtraCol6 = model.IsActive;
                                    GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace designation field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                       
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol6TextName = null;
                                            GetData.IsExtraCol6 = false;
                                            GetData.IsExtraCol6Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol7TextName")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol7TextName = model.TextName;
                                    GetData.IsExtraCol7 = model.IsActive;
                                    GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace designation field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                                                             
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol7TextName = null;
                                            GetData.IsExtraCol7 = false;
                                            GetData.IsExtraCol7Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol8TextName")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol8TextName = model.TextName;
                                    GetData.IsExtraCol8 = model.IsActive;
                                    GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace designation field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                                                             
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol8TextName = null;
                                            GetData.IsExtraCol8 = false;
                                            GetData.IsExtraCol8Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol9TextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol9TextName = model.TextName;
                                    GetData.IsExtraCol9 = model.IsActive;
                                    GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace expected date field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol9TextName = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol10TextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol10TextName = model.TextName;
                                    GetData.IsExtraCol10 = model.IsActive;
                                    GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace expected date field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol10TextName = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol11TextName")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol11TextName = model.TextName;
                                    GetData.IsExtraCol11 = model.IsActive;
                                    GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra11 field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                       
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol11TextName = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol12TextName")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol12TextName = model.TextName;
                                    GetData.IsExtraCol12 = model.IsActive;
                                    GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra12 field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                       
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol12TextName = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol13TextName")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol13TextName = model.TextName;
                                    GetData.IsExtraCol13 = model.IsActive;
                                    GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra13 field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                       
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol13TextName = null;
                                            GetData.IsExtraCol13 = false;
                                            GetData.IsExtraCol13Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol14TextName")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol14TextName = model.TextName;
                                    GetData.IsExtraCol14 = model.IsActive;
                                    GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra14 field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                       
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol14TextName = null;
                                            GetData.IsExtraCol14 = false;
                                            GetData.IsExtraCol14Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol15TextName")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol15TextName = model.TextName;
                                    GetData.IsExtraCol15 = model.IsActive;
                                    GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra15 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                                                             
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol15TextName = null;
                                            GetData.IsExtraCol15 = false;
                                            GetData.IsExtraCol15Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol16TextName")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol16TextName = model.TextName;
                                    GetData.IsExtraCol16 = model.IsActive;
                                    GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra16 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                                                             
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol16TextName = null;
                                            GetData.IsExtraCol16 = false;
                                            GetData.IsExtraCol16Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol17TextName")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol17TextName = model.TextName;
                                    GetData.IsExtraCol17 = model.IsActive;
                                    GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra17 field according to field type
                                    if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition                                                                             
                                    else if (model.FieldType == "DateText")//else if start date text
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.DateofBirthTextName = model.TextName;
                                            GetData.IsDateofBirth = model.IsActive;
                                            GetData.IsDOBMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.MarriageAnniversaryTextName = model.TextName;
                                            GetData.IsMarriageAnniversary = model.IsActive;
                                            GetData.IsMrgAniMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExpectedDateTextName = model.TextName;
                                            GetData.IsExpectedClosingDate = model.IsActive;
                                            GetData.IsExpClsDateMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol9TextName = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol10TextName = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol18TextName = model.TextName;
                                            GetData.IsExtraCol18 = model.IsActive;
                                            GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol19TextName = model.TextName;
                                            GetData.IsExtraCol19 = model.IsActive;
                                            GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                        {
                                            GetFormData.ExtraCol17TextName = null;
                                            GetData.IsExtraCol17 = false;
                                            GetData.IsExtraCol17Mandatory = false;

                                            GetFormData.ExtraCol20TextName = model.TextName;
                                            GetData.IsExtraCol20 = model.IsActive;
                                            GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }//else if end of date text
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol18TextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol18TextName = model.TextName;
                                    GetData.IsExtraCol18 = model.IsActive;
                                    GetData.IsExtraCol18Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra18 date field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol18TextName = null;
                                            GetData.IsExtraCol18 = false;
                                            GetData.IsExtraCol18Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol19TextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol19TextName = model.TextName;
                                    GetData.IsExtraCol19 = model.IsActive;
                                    GetData.IsExtraCol19Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra19 date field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol19TextName = null;
                                            GetData.IsExtraCol19 = false;
                                            GetData.IsExtraCol19Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol20TextName")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol20TextName = model.TextName;
                                    GetData.IsExtraCol20 = model.IsActive;
                                    GetData.IsExtraCol20Mandatory = model.IsActive == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
                                }
                                else
                                {
                                    #region replace extra19 date field according to field type
                                    if (model.FieldType == "NormalText")//check field type for insert column string data type
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.DesignationTextName = model.TextName;
                                            GetData.IsDesignation = model.IsActive;
                                            GetData.IsDesigMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.OrganizationNameTextName = model.TextName;
                                            GetData.IsOrganizationName = model.IsActive;
                                            GetData.IsOrgNameMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.AddressTextNameTextName = model.TextName;
                                            GetData.IsAddress = model.IsActive;
                                            GetData.IsAddressMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.UrlTextName = model.TextName;
                                            GetData.IsUrl = model.IsActive;
                                            GetData.IsUrlMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.SkypeIdTextName = model.TextName;
                                            GetData.IsSkypeId = model.IsActive;
                                            GetData.IsSkypeIdMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol1TextName = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol2TextName = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol3TextName = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol4TextName = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol5TextName = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }//if end of normarl text condition
                                    else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExpectedProductAmountTextName = model.TextName;
                                            GetData.IsExpectedDealAmount = model.IsActive;
                                            GetData.IsExpDealAmtMandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol6TextName = model.TextName;
                                            GetData.IsExtraCol6 = model.IsActive;
                                            GetData.IsExtraCol6Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol11TextName = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol12TextName = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol13TextName = model.TextName;
                                            GetData.IsExtraCol13 = model.IsActive;
                                            GetData.IsExtraCol13Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol14TextName = model.TextName;
                                            GetData.IsExtraCol14 = model.IsActive;
                                            GetData.IsExtraCol14Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }//else if end fo decimal text type
                                    else if (model.FieldType == "NumberText")//else if number text start
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol7TextName = model.TextName;
                                            GetData.IsExtraCol7 = model.IsActive;
                                            GetData.IsExtraCol7Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol8TextName = model.TextName;
                                            GetData.IsExtraCol8 = model.IsActive;
                                            GetData.IsExtraCol8Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol15TextName = model.TextName;
                                            GetData.IsExtraCol15 = model.IsActive;
                                            GetData.IsExtraCol15Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol16TextName = model.TextName;
                                            GetData.IsExtraCol16 = model.IsActive;
                                            GetData.IsExtraCol16Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                        {
                                            GetFormData.ExtraCol20TextName = null;
                                            GetData.IsExtraCol20 = false;
                                            GetData.IsExtraCol20Mandatory = false;

                                            GetFormData.ExtraCol17TextName = model.TextName;
                                            GetData.IsExtraCol17 = model.IsActive;
                                            GetData.IsExtraCol17Mandatory = model.IsActive == true ? model.IsRequired : false;
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }//else if end of number text

                                    #endregion
                                }
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    ErrorMessage = "** Somthing went wrong, while reading data, Please check the Post Data Parameters **";
                }
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
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
