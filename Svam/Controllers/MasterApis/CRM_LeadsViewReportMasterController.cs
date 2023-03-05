using AutoMapper;
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
using System.Threading.Tasks;
using System.Web.Http;

namespace Svam.Controllers.MasterApis
{
    public class CRM_LeadsViewReportMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_LeadsViewReportMaster/GetLeadsViewReportList?CompanyID=307&BranchID=184&UID=61&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public async Task<HttpResponseMessage> GetLeadsViewReportList(string CompanyID, string BranchID,int?UID, string Token)
        {
            string ErrorMessage = string.Empty;

            var fieldsList = new List<LeadsViewReportApiModel>();

            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);

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
                var model = new ViewLeadSettingDTO();
                var GetData = await db.crm_viewleadsetting.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefaultAsync();

                if (GetData != null)
                {
                    var data = Mapper.Map<ViewLeadSettingDTO>(GetData);
                    model = data;
                }
                else
                {
                    var data = new crm_viewleadsetting();
                    data.IsCity = true;
                    data.IsCreatedBy = true;
                    data.IsCreatedDate = true;
                    data.IsAssignedBy = true;
                    data.IsAssignedDate = true;
                    data.IsAssignTo = true;
                    data.IsModifiedDate = true;
                    data.IsExpClosingDate = true;
                    data.IsExpDealAmount = true;
                    data.CreatedDate = dt;
                    data.BranchID = branchID;
                    data.CompanyID = companyID;
                    data.IsActive = true;
                    db.crm_viewleadsetting.Add(data);
                    int i = await db.SaveChangesAsync();
                    if (i > 0)
                    {
                        var data1 = Mapper.Map<ViewLeadSettingDTO>(data);
                        model = data1;
                    }
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
                    GetFormData2.CreatedBy = Convert.ToInt32(UID);
                    GetFormData2.Createddate = dt;
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


                #region fields names list for show row wise
                string CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel =CustomerTextName, ActiveColumnName = "NA",FilterColumnName= "IsCustomerNameFilter", IsActive = true, IsFilter = model.IsCustomerNameFilter, IsActiveButtonAction = false, IsFilterButtonAction = true });

                string EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = EmailIdTextName, ActiveColumnName = "IsEmail", FilterColumnName= "IsEmailFilter", IsActive = model.IsEmail, IsFilter = model.IsEmailFilter, IsActiveButtonAction = true, IsFilterButtonAction = true });
               
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = "Created By", ActiveColumnName = "IsCreatedBy",FilterColumnName= "NA", IsActive = model.IsCreatedBy, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = MobileNoTextName, ActiveColumnName = "NA",FilterColumnName= "IsMobNoFilter", IsActive = true, IsFilter = model.IsMobNoFilter, IsActiveButtonAction = false, IsFilterButtonAction = true });

                string LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = LeadResourceTextName, ActiveColumnName = "IsLeadResource",FilterColumnName= "IsLeadSourceFilter", IsActive = model.IsLeadResource, IsFilter = model.IsLeadSourceFilter, IsActiveButtonAction = true, IsFilterButtonAction = true });

                string LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = LeadStatusTextName, ActiveColumnName = "NA",FilterColumnName= "NA", IsActive = true, IsFilter = true, IsActiveButtonAction = false, IsFilterButtonAction = false });

                string FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = FollowDateTextName, ActiveColumnName = "NA", FilterColumnName = "NA", IsActive = true, IsFilter = true, IsActiveButtonAction = false, IsFilterButtonAction = false });

                string FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = FollowUpTimeTextName, ActiveColumnName = "IsFollowUpTime", FilterColumnName = "NA", IsActive = model.IsFollowUpTime, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });

                //string FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";
                //fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = FollowupTimeinISTTextName, ActiveColumnName = "IsFollowUpTime", FilterColumnName = "NA", IsActive = model.IsFollowUpTime, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = CountryTextName, ActiveColumnName = "IsCountry", FilterColumnName = "NA", IsActive = model.IsCountry, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = StateTextName, ActiveColumnName = "IsState", FilterColumnName = "NA", IsActive = model.IsState, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City/Location";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = CityTextName, ActiveColumnName = "IsCity", FilterColumnName = "NA", IsActive = model.IsCity, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";
                fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ZoneNameTextName, ActiveColumnName = "IsTimeZoneName", FilterColumnName = "NA", IsActive = model.IsTimeZoneName, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : string.Empty;
                if(!string.IsNullOrEmpty(ExpectedProductAmountTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExpectedProductAmountTextName, ActiveColumnName = "IsExpDealAmount", FilterColumnName = "NA", IsActive = model.IsExpDealAmount, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.AddressTextNameTextName : string.Empty;
                if (!string.IsNullOrEmpty(AddressTextNameTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = AddressTextNameTextName, ActiveColumnName = "IsAddress", FilterColumnName = "NA", IsActive = model.IsAddress, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }
                string DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : string.Empty;
                if (!string.IsNullOrEmpty(DateofBirthTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = DateofBirthTextName, ActiveColumnName = "IsDOB", FilterColumnName = "NA", IsActive = model.IsDOB, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }
                string DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : string.Empty;
                if (!string.IsNullOrEmpty(DesignationTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = DesignationTextName, ActiveColumnName = "IsDesignation", FilterColumnName = "IsDesigFilter", IsActive = model.IsDesignation, IsFilter = model.IsDesigFilter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }
                string ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : string.Empty;
                if (!string.IsNullOrEmpty(ExpectedDateTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExpectedDateTextName, ActiveColumnName = "IsExpClosingDate", FilterColumnName = "NA", IsActive = model.IsExpClosingDate, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }
                string MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : string.Empty;
                if (!string.IsNullOrEmpty(MarriageAnniversaryTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = MarriageAnniversaryTextName, ActiveColumnName = "IsMrgAnnivarsary", FilterColumnName = "NA", IsActive = model.IsMrgAnnivarsary, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }
                string OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : string.Empty;
                if (!string.IsNullOrEmpty(OrganizationNameTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = OrganizationNameTextName, ActiveColumnName = "IsOrganization", FilterColumnName = "IsOrgNameFilter", IsActive = model.IsOrganization, IsFilter = model.IsOrgNameFilter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }

                string ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";                
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ProductTypeNameTextName, ActiveColumnName = "IsProductType", FilterColumnName = "IsProductTypeFilter", IsActive = model.IsProductType, IsFilter = model.IsProductTypeFilter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                
                string SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : string.Empty;
                if (!string.IsNullOrEmpty(SkypeIdTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = SkypeIdTextName, ActiveColumnName = "IsFilter", FilterColumnName = "IsSkypIdFilter", IsActive = model.IsSkypeId, IsFilter = model.IsSkypIdFilter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }

                string UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : string.Empty;
                if (!string.IsNullOrEmpty(UrlTextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = UrlTextName, ActiveColumnName = "IsUrl", FilterColumnName = "NA", IsActive = model.IsUrl, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }
                string ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol1TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol1TextName, ActiveColumnName = "IsExtraCol1", FilterColumnName = "NA", IsActive = model.IsExtraCol1, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol2TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol2TextName, ActiveColumnName = "IsExtraCol2", FilterColumnName = "NA", IsActive = model.IsExtraCol2, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol3TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol3TextName, ActiveColumnName = "IsExtraCol3", FilterColumnName = "NA", IsActive = model.IsExtraCol3, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol4TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol4TextName, ActiveColumnName = "IsExtraCol4", FilterColumnName = "NA", IsActive = model.IsExtraCol4, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol5TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol5TextName, ActiveColumnName = "IsExtraCol5", FilterColumnName = "NA", IsActive = model.IsExtraCol5, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol6TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol6TextName, ActiveColumnName = "IsExtraCol6", FilterColumnName = "NA", IsActive = model.IsExtraCol6, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol7TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol7TextName, ActiveColumnName = "IsExtraCol7", FilterColumnName = "NA", IsActive = model.IsExtraCol7, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol8TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol8TextName, ActiveColumnName = "IsExtraCol8", FilterColumnName = "NA", IsActive = model.IsExtraCol8, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol9TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol9TextName, ActiveColumnName = "IsExtraCol9", FilterColumnName = "IsExtraCol9Filter", IsActive = model.IsExtraCol9, IsFilter = model.IsExtraCol9Filter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }

                string ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol10TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol10TextName, ActiveColumnName = "IsExtraCol10", FilterColumnName = "IsExtraCol10Filter", IsActive = model.IsExtraCol10, IsFilter = model.IsExtraCol10Filter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }
                string ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol11TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol11TextName, ActiveColumnName = "IsExtraCol11", FilterColumnName = "NA", IsActive = model.IsExtraCol11, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol12TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol12TextName, ActiveColumnName = "IsExtraCol12", FilterColumnName = "NA", IsActive = model.IsExtraCol12, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol13TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol13TextName, ActiveColumnName = "IsExtraCol13", FilterColumnName = "NA", IsActive = model.IsExtraCol13, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol14TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol14TextName, ActiveColumnName = "IsExtraCol14", FilterColumnName = "NA", IsActive = model.IsExtraCol14, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol15TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol15TextName, ActiveColumnName = "IsExtraCol15", FilterColumnName = "NA", IsActive = model.IsExtraCol15, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol16TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol16TextName, ActiveColumnName = "IsExtraCol16", FilterColumnName = "NA", IsActive = model.IsExtraCol16, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol17TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol17TextName, ActiveColumnName = "IsExtraCol17", FilterColumnName = "NA", IsActive = model.IsExtraCol17, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                string ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol18TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol18TextName, ActiveColumnName = "IsExtraCol18", FilterColumnName = "IsExtraCol18Filter", IsActive = model.IsExtraCol18, IsFilter = model.IsExtraCol18Filter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }

                string ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol19TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol19TextName, ActiveColumnName = "IsExtraCol19", FilterColumnName = "IsExtraCol19Filter", IsActive = model.IsExtraCol19, IsFilter = model.IsExtraCol19Filter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }

                string ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol20TextName))
                {
                    fieldsList.Add(new LeadsViewReportApiModel { FieldLabel = ExtraCol20TextName, ActiveColumnName = "IsExtraCol20", FilterColumnName = "IsExtraCol20Filter", IsActive = model.IsExtraCol20, IsFilter = model.IsExtraCol20Filter, IsActiveButtonAction = true, IsFilterButtonAction = true });
                }
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

        //api/CRM_LeadsViewReportMaster/ChangeActive
        [HttpPost]
        public HttpResponseMessage ChangeActive([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                UpdateTicketsViewPostDTO CIM = JsonConvert.DeserializeObject<UpdateTicketsViewPostDTO>(postData.ToString());

                int branchID = Convert.ToInt32(CIM.BranchID);
                int companyID = Convert.ToInt32(CIM.CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, CIM.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                #region check nullable validation

                if (string.IsNullOrEmpty(CIM.ActiveColumnName))
                {
                    ErrorMessage = "Please enter column name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                int status = CIM.Status == true ? 1 : 0;
                if(CIM.ActiveColumnName!="NA")
                {                   
                    db.Database.ExecuteSqlCommand(@"update crm_viewleadsetting set " + CIM.ActiveColumnName + "=" + status + " where BranchID =" + branchID + "  and CompanyID = " + companyID + "");
                    SuccessMessage = "Status changed successfully";
                }
                else
                {
                    string msg= CIM.Status == true ? "Activate" : "Deactivate";
                    ErrorMessage = "Sorry you can not "+msg+" this field.";
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
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }

        //api/CRM_LeadsViewReportMaster/ChangeFilter
        [HttpPost]
        public HttpResponseMessage ChangeFilter([FromBody]JToken postData, HttpRequestMessage request) 
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                UpdateTicketsViewPostDTO CIM = JsonConvert.DeserializeObject<UpdateTicketsViewPostDTO>(postData.ToString());

                int branchID = Convert.ToInt32(CIM.BranchID);
                int companyID = Convert.ToInt32(CIM.CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, CIM.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                #region check nullable validation

                if (string.IsNullOrEmpty(CIM.FilterColumnName))
                {
                    ErrorMessage = "Please enter column name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                int status = CIM.Status == true ? 1 : 0;
                if(CIM.FilterColumnName!="NA")
                {
                    db.Database.ExecuteSqlCommand(@"update crm_viewleadsetting set " + CIM.FilterColumnName + "=" + status + " where BranchID =" + branchID + "  and CompanyID = " + companyID + "");
                    SuccessMessage = "Status changed successfully";
                }
                else
                {
                    string msg = CIM.Status == true ? "Filter" : "Defilter";
                    ErrorMessage = "Sorry you can not " + msg + " this field.";
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
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }
    }
}
