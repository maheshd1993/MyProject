using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.Repository;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMCreateLeadDynamicTextController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        // GET api/crmcreateleaddynamictext?companyId=307&branchId=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        public async Task<HttpResponseMessage>  Get(int companyId,int branchId,string Token)
        {
            var model = new List<CreateLeadDynamicAPIModel>();
            string ErrorMessage = string.Empty;
            var auth = Utility.TokenVerify(companyId, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            var GetData = new CreateLeadDynamicAPIModel();
            CommonRepository cr = new CommonRepository();
            try
            {
                var dataBool =await db.crm_createleadsetting.Where(em => em.BranchID == branchId && em.CompanyID == companyId).FirstOrDefaultAsync();
                var GetFormData =await db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchId && em.CompanyID == companyId).FirstOrDefaultAsync();
                var dt = Constant.GetimeForApi(companyId);
                #region save data if record null
                if (dataBool == null)
                {
                    crm_createleadsetting GetData1 = new crm_createleadsetting();
                    GetData1.BranchID = branchId;
                    GetData1.CompanyID = companyId;
                    GetData1.CreatedDate = dt;
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
                    dataBool = GetData1;
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
                    //GetFormData2.FormTextName = "Create New Lead";
                    GetFormData2.DescriptionTextName = "Description";
                    //GetFormData2.CreatedBy = UID;
                    GetFormData2.Createddate = Constant.GetimeForApi(companyId);
                    GetFormData2.CompanyID = companyId;
                    GetFormData2.BranchID = branchId;
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
                string CompanyNodaTimeZone = cr.GetCompanyNodaTimeZone(companyId);//get company noda time zone name
                // GetData.HeaderMenuTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.HeaderMenuTextName) ? GetFormData.HeaderMenuTextName : "Lead Managment";
                GetData.FormTextName ="Create New Lead";
                GetData.LeadOwner = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Lead Owner";

                GetData.IsCustomer = true;
                GetData.IsCustomerRequired = true;
                GetData.Customer = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";

                GetData.IsMobileNo = true;
                GetData.IsMobileNoRequired = true;
                GetData.MobileNo = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";

                GetData.IsFollowDate = true;
                GetData.IsFollowDateRequired = true;
                GetData.FollowDate = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";

                GetData.IsFollowUpTime = (dataBool == null || dataBool.IsFollowUpTime) ? true : false;
                GetData.IsFollowUpTimeRequired = dataBool != null && dataBool.IsFUpTimeMandatory ? true : false;
                GetData.FollowUpTime = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";

                GetData.IsFollowupTimeinIST = (dataBool == null || dataBool.IsFollowupTimeinIST) ? true : false;
                GetData.IsFollowupTimeinISTRequired = dataBool != null && dataBool.IsFupTimeinISTMandatory ? true : false;
                GetData.FollowupTimeinIST = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";
                GetData.FollowupTimeinIST = GetData.FollowupTimeinIST.Replace("IST", CompanyNodaTimeZone);//replace IST text to company time zone

                GetData.IsLeadStatus = true;
                GetData.IsLeadStatusRequired = true;
                GetData.LeadStatus = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";

                GetData.IsDescription = true;
                GetData.IsDescriptionRequired = true;
                GetData.Description = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";

                GetData.Address = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName)*/ ? GetFormData.AddressTextNameTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.Address))
                {
                    GetData.IsAddress =  false;
                    GetData.IsAddressRequired = false;
                }
                else
                {
                    GetData.IsAddress = (dataBool == null || dataBool.IsAddress) ? true : false;
                    GetData.IsAddressRequired = (dataBool != null && dataBool.IsAddressMandatory) ? true : false;
                }

                GetData.DateofBirth = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.DateofBirthTextName)*/ ? GetFormData.DateofBirthTextName :string.Empty;
                if(string.IsNullOrEmpty(GetData.DateofBirth))
                {
                    GetData.IsDateofBirth =  false;
                    GetData.IsDateofBirthRequired = false;
                }
                else
                {
                    GetData.IsDateofBirth = (dataBool == null || dataBool.IsDateofBirth) ? true : false;
                    GetData.IsDateofBirthRequired = (dataBool != null && dataBool.IsDOBMandatory) ? true : false;
                }

                GetData.Designation =GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.DesignationTextName)*/ ? GetFormData.DesignationTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.Designation))
                {
                    GetData.IsDesignation =  false;
                    GetData.IsDesignationRequired = false;
                }
                else
                {
                    GetData.IsDesignation = (dataBool == null || dataBool.IsDesignation) ? true : false;
                    GetData.IsDesignationRequired = (dataBool != null && dataBool.IsDesigMandatory) ? true : false;
                }
               


                GetData.IsLeadResource = (dataBool == null || dataBool.IsLeadResource) ? true : false;
                GetData.IsLeadResourceRequired = (dataBool != null && dataBool.IsLdResMandatory) ? true : false;
                GetData.LeadResource = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                //GetData.LeadsType = GetFormData.LeadsTypeTextName;

                GetData.MarriageAnniversary =GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName)*/ ? GetFormData.MarriageAnniversaryTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.MarriageAnniversary))
                {
                    GetData.IsMarriageAnniversary =  false;
                    GetData.IsMarriageAnniversaryRequired =  false;
                }
                else
                {
                    GetData.IsMarriageAnniversary = (dataBool == null || dataBool.IsMarriageAnniversary) ? true : false;
                    GetData.IsMarriageAnniversaryRequired = (dataBool != null && dataBool.IsMrgAniMandatory) ? true : false;
                }

                GetData.OrganizationName = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName)*/ ? GetFormData.OrganizationNameTextName :string.Empty;
                if(string.IsNullOrEmpty(GetData.OrganizationName))
                {
                    GetData.IsOrganizationName = false;
                    GetData.IsOrganizationNameRequired =  false;
                }
                else
                {
                    GetData.IsOrganizationName = (dataBool == null || dataBool.IsOrganizationName) ? true : false;
                    GetData.IsOrganizationNameRequired = (dataBool != null && dataBool.IsOrgNameMandatory) ? true : false;
                }
                
                //GetData.OtherNo = GetFormData.OtherNoTextName;

                GetData.IsProductTypeName = (dataBool == null || dataBool.IsProductType) ? true : false;
                GetData.IsProductTypeNameRequired = (dataBool != null && dataBool.IsProdTypeMandatory) ? true : false;
                GetData.ProductTypeName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";

                GetData.SkypeId = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.SkypeIdTextName)*/ ? GetFormData.SkypeIdTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.SkypeId))
                {
                    GetData.IsSkypeId = false;
                    GetData.IsSkypeIdRequired =  false;
                }
                else
                {
                    GetData.IsSkypeId = (dataBool == null || dataBool.IsSkypeId) ? true : false;
                    GetData.IsSkypeIdRequired = (dataBool != null && dataBool.IsSkypeIdMandatory) ? true : false;
                }

                GetData.Url = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.UrlTextName) */? GetFormData.UrlTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.Url))
                {
                    GetData.IsUrl = false;
                    GetData.IsUrlRequired = false;
                }
                else
                {
                    GetData.IsUrl = (dataBool == null || dataBool.IsUrl) ? true : false;
                    GetData.IsUrlRequired = (dataBool != null && dataBool.IsUrlMandatory) ? true : false;
                }

                GetData.IsZoneName = (dataBool == null || dataBool.IsTimeZoneName) ? true : false;
                GetData.IsZoneNameRequired = (dataBool != null && dataBool.IsTZNameMandatory) ? true : false;
                GetData.ZoneName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";

                GetData.IsEmailId = (dataBool == null || dataBool.IsEmailId) ? true : false;
                GetData.IsEmailIdRequired = (dataBool != null && dataBool.IsEmailMandatory) ? true : false;
                GetData.EmailId = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";

                GetData.ExpectedDate = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName)*/ ? GetFormData.ExpectedDateTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.ExpectedDate))
                {
                    GetData.IsExpectedDate =  false;
                    GetData.IsExpectedDateRequired =  false;
                }
                else
                {
                    GetData.IsExpectedDate = (dataBool == null || dataBool.IsExpectedClosingDate) ? true : false;
                    GetData.IsExpectedDateRequired = (dataBool != null && dataBool.IsExpClsDateMandatory) ? true : false;
                }
                

                GetData.IsCity = (dataBool == null || dataBool.IsCity) ? true : false;
                GetData.IsCityRequired = (dataBool != null && dataBool.IsCityMandatory) ? true : false;
                GetData.City = "City";

                GetData.IsState = (dataBool == null || dataBool.IsState) ? true : false;
                GetData.IsStateRequired = (dataBool != null && dataBool.IsStateMandatory) ? true : false;
                GetData.State = "State";

                GetData.IsCountry = (dataBool == null || dataBool.IsCountry) ? true : false;
                GetData.IsCountryRequired = (dataBool != null && dataBool.IsCountryMandatory) ? true : false;
                GetData.Country = "Country";

                GetData.ExpectedProductAmount = GetFormData != null ?GetFormData.ExpectedProductAmountTextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.ExpectedProductAmount))
                {
                    GetData.IsExpectedProductAmount =  false;
                    GetData.IsExpectedProductAmountRequired =  false;
                }
                else
                {
                    GetData.IsExpectedProductAmount = (dataBool == null || dataBool.IsExpectedDealAmount) ? true : false;
                    GetData.IsExpectedProductAmountRequired = (dataBool != null && dataBool.IsExpDealAmtMandatory) ? true : false;
                }
               
                
                GetData.ExtraCol1 = GetFormData != null ? GetFormData.ExtraCol1TextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.ExtraCol1))
                {
                    GetData.IsExtraCol1 =  false;
                    GetData.IsExtraCol1Required =  false;
                }
                else
                {
                    GetData.IsExtraCol1 = (dataBool != null && dataBool.IsExtraCol1) ? true : false;
                    GetData.IsExtraCol1Required = (dataBool != null && dataBool.IsExtraCol1Mandatory) ? true : false;
                }

                
                GetData.ExtraCol2 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName)*/ ? GetFormData.ExtraCol2TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol2))
                {
                    GetData.IsExtraCol2 =  false;
                    GetData.IsExtraCol2Required =  false;
                }
                else
                {
                    GetData.IsExtraCol2 = (dataBool != null && dataBool.IsExtraCol2) ? true : false;
                    GetData.IsExtraCol2Required = (dataBool != null && dataBool.IsExtraCol2Mandatory) ? true : false;
                }
                   
                GetData.ExtraCol3 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName)*/ ? GetFormData.ExtraCol3TextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.ExtraCol3))
                {
                    GetData.IsExtraCol3 =  false;
                    GetData.IsExtraCol3Required = false;
                }
                else
                {
                    GetData.IsExtraCol3 = (dataBool != null && dataBool.IsExtraCol3) ? true : false;
                    GetData.IsExtraCol3Required = (dataBool != null && dataBool.IsExtraCol3Mandatory) ? true : false;
                }

                
                GetData.ExtraCol4 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName)*/? GetFormData.ExtraCol4TextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.ExtraCol4))
                {
                    GetData.IsExtraCol4 =  false;
                    GetData.IsExtraCol4Required =  false;
                }
                else
                {
                    GetData.IsExtraCol4 = (dataBool != null && dataBool.IsExtraCol4) ? true : false;
                    GetData.IsExtraCol4Required = (dataBool != null && dataBool.IsExtraCol4Mandatory) ? true : false;
                }

                
                GetData.ExtraCol5 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName)*/ ? GetFormData.ExtraCol5TextName : string.Empty;
                if(string.IsNullOrEmpty(GetData.ExtraCol5))
                {
                    GetData.IsExtraCol5 =  false;
                    GetData.IsExtraCol5Required = false;
                }
                else
                {
                    GetData.IsExtraCol5 = (dataBool != null && dataBool.IsExtraCol5) ? true : false;
                    GetData.IsExtraCol5Required = (dataBool != null && dataBool.IsExtraCol5Mandatory) ? true : false;
                }
               
                GetData.ExtraCol6 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName)*/ ? GetFormData.ExtraCol6TextName :string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol6))
                {
                    GetData.IsExtraCol6 = false;
                    GetData.IsExtraCol6Required = false;
                }
                else
                {
                    GetData.IsExtraCol6 = (dataBool != null && dataBool.IsExtraCol6) ? true : false;
                    GetData.IsExtraCol6Required = (dataBool != null && dataBool.IsExtraCol6Mandatory) ? true : false;
                }

                
                GetData.ExtraCol7 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName)*/ ? GetFormData.ExtraCol7TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol7))
                {
                    GetData.IsExtraCol7 = false;
                    GetData.IsExtraCol7Required = false;
                }
                else
                {
                    GetData.IsExtraCol7 = (dataBool != null && dataBool.IsExtraCol7) ? true : false;
                    GetData.IsExtraCol7Required = (dataBool != null && dataBool.IsExtraCol7Mandatory) ? true : false;
                }

                
                GetData.ExtraCol8 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName)*/ ? GetFormData.ExtraCol8TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol8))
                {
                    GetData.IsExtraCol8 = false;
                    GetData.IsExtraCol8Required = false;
                }
                else
                {
                    GetData.IsExtraCol8 = (dataBool != null && dataBool.IsExtraCol8) ? true : false;
                    GetData.IsExtraCol8Required = (dataBool != null && dataBool.IsExtraCol8Mandatory) ? true : false;
                }

                
                GetData.ExtraCol9 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName)*/ ? GetFormData.ExtraCol9TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol9))
                {
                    GetData.IsExtraCol9 = false;
                    GetData.IsExtraCol9Required = false;
                }
                else
                {
                    GetData.IsExtraCol9 = (dataBool != null && dataBool.IsExtraCol9) ? true : false;
                    GetData.IsExtraCol9Required = (dataBool != null && dataBool.IsExtraCol9Mandatory) ? true : false;
                }

               
                GetData.ExtraCol10 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName)*/ ? GetFormData.ExtraCol10TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol10))
                {
                    GetData.IsExtraCol10 = false;
                    GetData.IsExtraCol10Required = false;
                }
                else
                {
                    GetData.IsExtraCol10 = (dataBool != null && dataBool.IsExtraCol10) ? true : false;
                    GetData.IsExtraCol10Required = (dataBool != null && dataBool.IsExtraCol10Mandatory) ? true : false;
                }
                
                GetData.ExtraCol11 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName)*/ ? GetFormData.ExtraCol11TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol11))
                {
                    GetData.IsExtraCol11 = false;
                    GetData.IsExtraCol11Required = false;
                }
                else
                {
                    GetData.IsExtraCol11 = (dataBool != null && dataBool.IsExtraCol11) ? true : false;
                    GetData.IsExtraCol11Required = (dataBool != null && dataBool.IsExtraCol11Mandatory) ? true : false;
                }
               
                GetData.ExtraCol12 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName)*/ ? GetFormData.ExtraCol12TextName :string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol12))
                {
                    GetData.IsExtraCol12 = false;
                    GetData.IsExtraCol12Required = false;
                }
                else
                {
                    GetData.IsExtraCol12 = (dataBool != null && dataBool.IsExtraCol12) ? true : false;
                    GetData.IsExtraCol12Required = (dataBool != null && dataBool.IsExtraCol12Mandatory) ? true : false;
                }
                
                GetData.ExtraCol13 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol13))
                {
                    GetData.IsExtraCol13 = false;
                    GetData.IsExtraCol13Required = false;
                }
                else
                {
                    GetData.IsExtraCol13 = (dataBool != null && dataBool.IsExtraCol13) ? true : false;
                    GetData.IsExtraCol13Required = (dataBool != null && dataBool.IsExtraCol13Mandatory) ? true : false;
                }
                
                GetData.ExtraCol14 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol14))
                {
                    GetData.IsExtraCol14 = false;
                    GetData.IsExtraCol14Required = false;
                }
                else
                {
                    GetData.IsExtraCol14 = (dataBool != null && dataBool.IsExtraCol14) ? true : false;
                    GetData.IsExtraCol14Required = (dataBool != null && dataBool.IsExtraCol14Mandatory) ? true : false;
                }
                
                GetData.ExtraCol15 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName)*/ ? GetFormData.ExtraCol15TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol15))
                {
                    GetData.IsExtraCol15 = false;
                    GetData.IsExtraCol15Required = false;
                }
                else
                {
                    GetData.IsExtraCol15 = (dataBool != null && dataBool.IsExtraCol15) ? true : false;
                    GetData.IsExtraCol15Required = (dataBool != null && dataBool.IsExtraCol15Mandatory) ? true : false;
                }
               
                GetData.ExtraCol16 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName)*/ ? GetFormData.ExtraCol16TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol16))
                {
                    GetData.IsExtraCol16 = false;
                    GetData.IsExtraCol16Required = false;
                }
                else
                {
                    GetData.IsExtraCol16 = (dataBool != null && dataBool.IsExtraCol16) ? true : false;
                    GetData.IsExtraCol16Required = (dataBool != null && dataBool.IsExtraCol16Mandatory) ? true : false;
                }
               
                GetData.ExtraCol17 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName)*/ ? GetFormData.ExtraCol17TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol17))
                {
                    GetData.IsExtraCol17 = false;
                    GetData.IsExtraCol17Required = false;
                }
                else
                {
                    GetData.IsExtraCol17 = (dataBool != null && dataBool.IsExtraCol17) ? true : false;
                    GetData.IsExtraCol17Required = (dataBool != null && dataBool.IsExtraCol17Mandatory) ? true : false;
                }
               
                GetData.ExtraCol18 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol18))
                {
                    GetData.IsExtraCol18 = false;
                    GetData.IsExtraCol18Required = false;
                }
                else
                {
                    GetData.IsExtraCol18 = (dataBool != null && dataBool.IsExtraCol18) ? true : false;
                    GetData.IsExtraCol18Required = (dataBool != null && dataBool.IsExtraCol18Mandatory) ? true : false;
                }
               
                GetData.ExtraCol19 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName)*/ ? GetFormData.ExtraCol19TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol19))
                {
                    GetData.IsExtraCol19 = false;
                    GetData.IsExtraCol19Required = false;
                }
                else
                {
                    GetData.IsExtraCol19 = (dataBool != null && dataBool.IsExtraCol19) ? true : false;
                    GetData.IsExtraCol19Required = (dataBool != null && dataBool.IsExtraCol19Mandatory) ? true : false;
                }
                GetData.IsExtraCol20 = (dataBool != null && dataBool.IsExtraCol20) ? true : false;
                GetData.IsExtraCol20Required = (dataBool != null && dataBool.IsExtraCol20Mandatory) ? true : false;
                GetData.ExtraCol20 = GetFormData != null /*&& !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName)*/ ? GetFormData.ExtraCol20TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol20))
                {
                    GetData.IsExtraCol20 = false;
                    GetData.IsExtraCol20Required = false;
                }
                else
                {
                    GetData.IsExtraCol20 = (dataBool != null && dataBool.IsExtraCol20) ? true : false;
                    GetData.IsExtraCol20Required = (dataBool != null && dataBool.IsExtraCol20Mandatory) ? true : false;
                }
                model.Add(GetData);


               
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("Somthing went wrong, while reading data, Please check the GET Data Format");
                ExceptionLogging.SendExcepToDB(ex);
                HttpError err = new HttpError(ErrorMessage);
               
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            return Request.CreateResponse(HttpStatusCode.OK, model);

        }

       
    }
}
