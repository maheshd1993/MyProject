using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMViewLeadDynamicFieldsController : ApiController
    {
        #region data entity
        niscrmEntities db = new niscrmEntities();
        #endregion

        ////GET api/crmviewleaddynamicfields?companyId=307&branchId=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        public HttpResponseMessage Get(int companyId, int branchId, string Token)
        {
            var model = new List<ViewLeadDynamicFildsApiModel>();
            var GetData = new ViewLeadDynamicFildsApiModel();
            string ErrorMessage = string.Empty;
            var auth = Utility.TokenVerify(companyId, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            
            try
            {

                var data  = db.crm_viewleadsetting.Where(em => em.BranchID == branchId && em.CompanyID == companyId).FirstOrDefault();
                var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchId && em.CompanyID == companyId).FirstOrDefault();

                ////////////////////////////show column on view leads start///////////////////////////////////////
                GetData.ReportTextName =(data!=null && !string.IsNullOrEmpty(data.ReportTextName))? data.ReportTextName: "View Leads Report";

                GetData.IsCustomerName = true;
                GetData.CustomerNameLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";

                GetData.IsMob = true;
                GetData.MobLabel  =GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";

                GetData.IsFollowupDate = true;
                GetData.FollowupDateLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";

                GetData.IsLeadStatus = true;
                GetData.LeadStatusLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";


                GetData.IsEMail = (data!=null && data.IsEmail)?true:false;
                GetData.EMailLabel=GetFormData!=null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Email";


                GetData.IsCity = (data == null || data.IsCity)?true:false;
                GetData.CityLabel = "City";

                GetData.IsState = (data != null && data.IsState) ? true : false;
                GetData.StateLabel = "State";

                GetData.IsCountry = (data != null && data.IsCountry) ? true : false;
                GetData.CountryLabel = "Country";

                GetData.IsAssignedBy =(data == null || data.IsAssignedBy)?true:false;
                GetData.AssignedByLabel = "Assigned By";

                GetData.IsAssinedTo =(data==null || data.IsAssignTo)?true:false;
                GetData.AssinedToLabel = "Assigned To";

                GetData.IsAssignDate =(data==null|| data.IsAssignedDate)?true:false;
                GetData.AssignDateLabel = "Assigned Date";

                GetData.IsExpectedDate =(data==null|| data.IsExpClosingDate)?true:false;
                GetData.ExpectedDateLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : "Expected Closing Date";

                GetData.IsExpectedProductAmount = (data == null || data.IsExpDealAmount)?true:false;
                GetData.ExpectedProductAmountLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : "Expected Deal Amount";

                GetData.IsCreated_By =(data==null|| data.IsCreatedBy)?true:false;
                GetData.Created_ByLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Created By";

                GetData.IsCreatedDate =(data==null || data.IsCreatedDate)?true:false;
                GetData.CreatedDateLabel = "Created Date";

                GetData.IsModifiedDate =(data==null|| data.IsModifiedDate)?true:false;
                GetData.ModifiedDateLabel = "Modified Date";

                //GetData.LeadSampleTextName = data.LeadSampleTextName;
                //GetData.FBLeadSampleTextName = data.FBLeadSampleTextName;
                //GetData.TotalLeadTextName = data.TotalLeadTextName;
                GetData.IsDateofBirth = (data != null && data.IsDOB) ? true : false;
                GetData.DateofBirthLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : "Date Of Birth";

                GetData.IsMarriageAnniversary = (data != null && data.IsMrgAnnivarsary) ? true : false;
                GetData.MarriageAnniversaryLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : "Marriage Anniversary";

                GetData.IsDesignation = (data != null && data.IsDesignation) ? true : false;
                GetData.DesignationLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : "Designation";


                GetData.IsProductType =(data!=null&& data.IsProductType)?true:false;
                GetData.ProductTypeLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";

                GetData.IsLeadResource = data.IsLeadResource;
                GetData.LeadResourceLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";

                GetData.IsAddress =(data!=null && data.IsAddress)?true:false;
                GetData.AddressLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.LeadStatusTextName : "Address";

                GetData.IsURL =(data!=null && data.IsUrl)?true:false;
                GetData.URLLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : "URL";

                GetData.IsSkypeId =(data!=null && data.IsSkypeId)?true:false;
                GetData.SkypeIdLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : "Skype ID";
                //GetData.IsTimeZoneName = data.IsTimeZoneName;

                GetData.IsOrganizationName =(data!=null && data.IsOrganization)?true:false;
                GetData.OrganizationNameLabel= GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";

                GetData.IsExtraCol1 =(data!=null&& data.IsExtraCol1)?true:false;
                GetData.ExtraCol1Label= GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : "Additional 1";

                GetData.IsExtraCol2 = (data != null && data.IsExtraCol2) ? true : false;
                GetData.ExtraCol2Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : "Additional 2";

                GetData.IsExtraCol3 = (data != null && data.IsExtraCol3) ? true : false;
                GetData.ExtraCol3Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : "Additional 3";

                GetData.IsExtraCol4 = (data != null && data.IsExtraCol4) ? true : false;
                GetData.ExtraCol4Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : "Additional 4";

                GetData.IsExtraCol5 = (data != null && data.IsExtraCol5) ? true : false;
                GetData.ExtraCol5Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : "Additional 5";

                GetData.IsExtraCol6 = (data != null && data.IsExtraCol6) ? true : false;
                GetData.ExtraCol6Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : "Additional 6";

                GetData.IsExtraCol7 = (data != null && data.IsExtraCol7) ? true : false;
                GetData.ExtraCol7Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : "Additional 7";

                GetData.IsExtraCol8 = (data != null && data.IsExtraCol8) ? true : false;
                GetData.ExtraCol8Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : "Additional 8";

                GetData.IsExtraCol9 = (data != null && data.IsExtraCol9) ? true : false;
                GetData.ExtraCol9Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : "Additional 9";

                GetData.IsExtraCol10 = (data != null && data.IsExtraCol10) ? true : false;
                GetData.ExtraCol10Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : "Additional 10";

                model.Add(GetData);
                //GetData.IsExtraCol9Filter = data.IsExtraCol9Filter;
                //GetData.IsExtraCol10Filter = data.IsExtraCol10Filter;
                //GetData.IsTermFilter = data.IsTermFilter;
                //GetData.IsProductTypeFilter = data.IsProductTypeFilter;
                //GetData.IsLeadSourceFilter = data.IsLeadSourceFilter;
                ////////////////////////////show column on view leads end///////////////////////////////////////

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
