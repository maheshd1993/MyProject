using AutoMapper;
using Svam.EF;
using Svam.Models;
using Svam.Models.DTO;
using Svam.Repository;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Svam.Controllers
{
    [NoCache]
    public class LeadSetting1Controller : Controller
    {
        niscrmEntities db = new niscrmEntities();
        CommonRepository cr = new CommonRepository();

        public async Task<ActionResult> ViewLeadSetting()
        {

            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            var model = new ViewLeadSettingDTO();
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                var GetData = await db.crm_viewleadsetting.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();

                if (GetData == null)
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
                    data.CreatedDate = Constant.GetBharatTime();
                    data.BranchID = BranchID;
                    data.CompanyID = CompanyID;
                    data.IsActive = true;
                    db.crm_viewleadsetting.Add(data);
                    int i = await db.SaveChangesAsync();
                    if(i>0)
                    {
                        var data1 = Mapper.Map<ViewLeadSettingDTO>(data);
                        model = data1;
                    }
                    
                }
                //if (GetFormData != null)
                //{
              
                    //model.FormTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FormTextName) ? GetFormData.FormTextName : "Create New Lead";
                    //model.HeaderMenuTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.HeaderMenuTextName) ? GetFormData.HeaderMenuTextName : "Lead";
                    model.AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.LeadStatusTextName : "Address";
                    model.CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                    model.DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : "Date Of Birth";
                    //model.DescriptionTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";
                    model.DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : "Designation";
                    //model.DNDStatusTextName = GetFormData.DNDStatusTextName;
                    model.EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";
                    model.ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : "Expected Closing Date";
                    model.ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : "Expected Deal Amount";

                    model.LeadOwnerTextName = /*GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName :*/ "Created By";
                    model.LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                    model.LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                    model.FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                    model.FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                    model.FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";

                model.CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";
  
                model.StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";
                
                model.CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City";
                

                model.MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : "Marriage Anniversary";
                    model.MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                    model.OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";

                    model.ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                    model.SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : "Skype ID";
                    model.UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : "URL";
                    model.ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";

                    model.ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                    model.ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                    model.ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                    model.ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                    model.ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                    model.ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                    model.ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                    model.ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                    model.ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                    model.ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                model.ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                model.ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                model.ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                model.ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                model.ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                model.ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                model.ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                model.ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                model.ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                model.ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                //}
            }
            else
            {
                return Redirect("/home/login");
            }

            return View(model);
        }
        
        public async Task<ActionResult> CreateSetting()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            CreateLeadFieldDTO model = new CreateLeadFieldDTO();
            
            var fieldsList = new List<FieldsNameList>();
            var fieldPriorityList = new List<LeadFieldPriorityDTO>();
            //var GetSeqData = new List<crm_create_lead_field_sequence>();
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                var GetData = await db.crm_createleadsetting.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefaultAsync();
                var GetSeqData = await db.crm_create_lead_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToListAsync();

                #region save data if first time user enter here
                if (GetData == null)
                {
                    crm_createleadsetting GetData1 = new crm_createleadsetting();
                    GetData1.BranchID = BranchID;
                    GetData1.CompanyID = CompanyID;
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
                    GetData1.IsExtraCol1 = false;
                    GetData1.IsExtraCol2 = false;
                    GetData1.IsExtraCol3 = false;
                    GetData1.IsExtraCol4 = false;
                    GetData1.IsExtraCol5 = false;
                    GetData1.IsExtraCol6 = false;
                    GetData1.IsExtraCol7 = false;
                    GetData1.IsExtraCol8 = false;
                    GetData1.IsExtraCol9 = false;
                    GetData1.IsExtraCol10 = false;
                    GetData1.IsExtraCol11 = false;
                    GetData1.IsExtraCol12 = false;
                    GetData1.IsExtraCol13 = false;
                    GetData1.IsExtraCol14 = false;
                    GetData1.IsExtraCol15 = false;
                    GetData1.IsExtraCol16 = false;
                    GetData1.IsExtraCol17 = false;
                    GetData1.IsExtraCol18 = false;
                    GetData1.IsExtraCol19 = false;
                    GetData1.IsExtraCol20 = false;

                    GetData1.IsExtraCol1Mandatory = false;
                    GetData1.IsExtraCol2Mandatory = false;
                    GetData1.IsExtraCol3Mandatory = false;
                    GetData1.IsExtraCol4Mandatory = false;
                    GetData1.IsExtraCol5Mandatory = false;
                    GetData1.IsExtraCol6Mandatory = false;
                    GetData1.IsExtraCol7Mandatory = false;
                    GetData1.IsExtraCol8Mandatory = false;
                    GetData1.IsExtraCol9Mandatory = false;
                    GetData1.IsExtraCol10Mandatory = false;
                    GetData1.IsExtraCol11Mandatory = false;
                    GetData1.IsExtraCol12Mandatory = false;
                    GetData1.IsExtraCol13Mandatory = false;
                    GetData1.IsExtraCol14Mandatory = false;
                    GetData1.IsExtraCol15Mandatory = false;
                    GetData1.IsExtraCol16Mandatory = false;
                    GetData1.IsExtraCol17Mandatory = false;
                    GetData1.IsExtraCol18Mandatory = false;
                    GetData1.IsExtraCol19Mandatory = false;
                    GetData1.IsExtraCol20Mandatory = false;

                    db.crm_createleadsetting.Add(GetData1);
                    db.SaveChanges();
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
                    GetFormData2.CreatedBy = Convert.ToInt32(Session["UID"]);
                    GetFormData2.Createddate = Constant.GetBharatTime();
                    GetFormData2.ModifiedDate = Constant.GetBharatTime();
                    GetFormData2.CompanyID = CompanyID;
                    GetFormData2.BranchID = BranchID;
                    GetFormData2.Status = true;
                    GetFormData2.DateofBirthTextName = "Date Of Birth";
                    GetFormData2.MarriageAnniversaryTextName = "Marriage Anniversary";
                    GetFormData2.ExpectedDateTextName = "Expected Closing Date";
                    GetFormData2.ExpectedProductAmountTextName = "Expected Deal Amount";
                    GetFormData2.ExtraCol1TextName = null;
                    GetFormData2.ExtraCol2TextName = null;
                    GetFormData2.ExtraCol3TextName = null;
                    GetFormData2.ExtraCol4TextName = null;
                    GetFormData2.ExtraCol5TextName = null;
                    GetFormData2.ExtraCol6TextName = null;
                    GetFormData2.ExtraCol7TextName = null;
                    GetFormData2.ExtraCol8TextName = null;
                    GetFormData2.ExtraCol9TextName = null;
                    GetFormData2.ExtraCol10TextName = null;
                    GetFormData2.ExtraCol11TextName = null;
                    GetFormData2.ExtraCol12TextName = null;
                    GetFormData2.ExtraCol13TextName = null;
                    GetFormData2.ExtraCol14TextName = null;
                    GetFormData2.ExtraCol15TextName = null;
                    GetFormData2.ExtraCol16TextName = null;
                    GetFormData2.ExtraCol17TextName = null;
                    GetFormData2.ExtraCol18TextName = null;
                    GetFormData2.ExtraCol19TextName = null;
                    GetFormData2.ExtraCol20TextName = null;

                    //GetFormData2.HeaderMenuTextName = model.HeaderMenuTextName;
                    db.crm_customizedformfieldtextname.Add(GetFormData2);
                    db.SaveChanges();
                }
                #endregion

                #region save field sequence data

                if (GetSeqData!=null&&GetSeqData.Count>0)
                {
                    fieldPriorityList = (from fpData in GetSeqData
                                         select new LeadFieldPriorityDTO
                                         {
                                           Priority=fpData.Priority,
                                           FieldName=fpData.FieldName
                                         }
                                       ).ToList();
                }
                else
                {
                    
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 1, FieldName = "LeadStatusTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 2, FieldName = "CustomerTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=3,  FieldName = "MobileNoTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 4, FieldName = "EmailIdTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 5, FieldName = "DateofBirthTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 6, FieldName = "MarriageAnniversaryTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=7,  FieldName = "FollowDateTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 8, FieldName = "DesignationTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 9, FieldName = "OrganizationNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=10,  FieldName = "FollowUpTimeTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 11, FieldName = "ZoneNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=12,  FieldName = "FollowupTimeinISTTextName" });                   
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=13,  FieldName = "CountryTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=14,  FieldName = "StateTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=15,  FieldName = "CityTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 16, FieldName = "AddressTextNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=17,  FieldName = "UrlTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=18,  FieldName = "SkypeIdTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=19,  FieldName = "LeadResourceTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=20,  FieldName = "ProductTypeNameTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=21,  FieldName = "ExpectedDateTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=22,  FieldName = "ExpectedProductAmountTextName" });                   
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=23,  FieldName = "ExtraCol1TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=24,  FieldName = "ExtraCol2TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=25,  FieldName = "ExtraCol3TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=26,  FieldName = "ExtraCol4TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=27,  FieldName = "ExtraCol5TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=28,  FieldName = "ExtraCol6TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=29,  FieldName = "ExtraCol7TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=30,  FieldName = "ExtraCol8TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=31,  FieldName = "ExtraCol9TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=32,  FieldName = "ExtraCol10TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=33,  FieldName = "ExtraCol11TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=34,  FieldName = "ExtraCol12TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=35,  FieldName = "ExtraCol13TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=36,  FieldName = "ExtraCol14TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=37,  FieldName = "ExtraCol15TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=38,  FieldName = "ExtraCol16TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=39,  FieldName = "ExtraCol17TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=40,  FieldName = "ExtraCol18TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=41,  FieldName = "ExtraCol19TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO{ Priority=42,  FieldName = "ExtraCol20TextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority =43, FieldName = "LeadOwnerTextName" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 44, FieldName = "DescriptionTextName" });

                    var uid = Convert.ToInt32(Session["UID"]);
                    foreach (var item in fieldPriorityList)
                    {
                        var fp = new crm_create_lead_field_sequence
                        {
                          Priority=item.Priority,
                          FieldName=item.FieldName,
                          CompanyID=CompanyID,
                          BranchID=BranchID,
                          CreatedBy =uid ,
                          Createddate = Constant.GetBharatTime(),
                          ModifiedDate = Constant.GetBharatTime()
                        };
                        db.crm_create_lead_field_sequence.Add(fp);
                    }
                    db.SaveChanges();                   
                }

                #endregion

               
                #region fields names list for show dropdown

                string LeadOwnerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Lead Owner";
                int? ldSeqNo = fieldPriorityList.Where(a => a.FieldName == "LeadOwnerTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = LeadOwnerTextName, Values = "LeadOwnerTextName/true/true/NormalText",Priority=ldSeqNo });

                string LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                int? lsSeqNo = fieldPriorityList.Where(a=>a.FieldName== "LeadStatusTextName").Select(a=>a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = LeadStatusTextName, Values = "LeadStatusTextName/true/true/NormalText",Priority=lsSeqNo });
               
                string CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                int? cnmSeqNo = fieldPriorityList.Where(a => a.FieldName == "CustomerTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = CustomerTextName, Values = "CustomerTextName/true/true/NormalText",Priority=cnmSeqNo });

                string MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                int? mobSeqNo = fieldPriorityList.Where(a => a.FieldName == "MobileNoTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = MobileNoTextName, Values = "MobileNoTextName/true/true/NormalText",Priority=mobSeqNo });

                string FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                int? fudSeqNo = fieldPriorityList.Where(a => a.FieldName == "FollowDateTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = FollowDateTextName, Values = "FollowDateTextName/true/true/DateText",Priority=fudSeqNo });

                string IsFollowUpTime = (GetData == null || GetData.IsFollowUpTime) ? "true" : "false";
                string IsFollowUpTimeRequired = GetData != null && GetData.IsFUpTimeMandatory ? "true" : "false";
                string FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
               int? futSeqNo = fieldPriorityList.Where(a => a.FieldName == "FollowUpTimeTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = FollowUpTimeTextName, Values = "FollowUpTimeTextName/"+IsFollowUpTime+"/"+IsFollowUpTimeRequired+ "/TimeText",Priority=futSeqNo });


                string IsFollowupTimeinIST = (GetData == null || GetData.IsFollowupTimeinIST) ? "true" : "false";
                string IsFollowupTimeinISTRequired = GetData != null && GetData.IsFupTimeinISTMandatory ? "true" : "false";
                string FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";
                int? futistSeqNo = fieldPriorityList.Where(a => a.FieldName == "FollowupTimeinISTTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = FollowupTimeinISTTextName, Values = "FollowupTimeinISTTextName/"+IsFollowupTimeinIST+"/"+IsFollowupTimeinISTRequired+ "/TimeText",Priority=futistSeqNo });

                string IsTZone = (GetData == null || GetData.IsTimeZoneName) ? "true" : "false";
                string IsTZoneReq = (GetData != null && GetData.IsTZNameMandatory) ? "true" : "false";
                string ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";
                int? znSeqNo = fieldPriorityList.Where(a => a.FieldName == "ZoneNameTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = ZoneNameTextName, Values = "ZoneNameTextName/"+IsTZone+"/"+IsTZoneReq+ "/DropDownList",Priority=znSeqNo });

                string IsDateofBirth = (GetData == null || GetData.IsDateofBirth) ? "true" : "false";
                string IsDateofBirthRequired = (GetData != null && GetData.IsDOBMandatory) ? "true" : "false";
                string DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : "Date Of Birth";
                int? dobSeqNo = fieldPriorityList.Where(a => a.FieldName == "DateofBirthTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = DateofBirthTextName, Values = "DateofBirthTextName/"+IsDateofBirth+"/"+IsDateofBirthRequired+ "/DateText", Priority = dobSeqNo });

                string IsMrgAnni = (GetData == null || GetData.IsMarriageAnniversary) ? "true" : "false";
                string IsMrgAnniReq = (GetData != null && GetData.IsMrgAniMandatory) ? "true" : "false";
                string MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : "Marriage Anniversary";
                int? mrgSeqNo = fieldPriorityList.Where(a => a.FieldName == "MarriageAnniversaryTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = MarriageAnniversaryTextName, Values = "MarriageAnniversaryTextName/"+IsMrgAnni+"/"+IsMrgAnniReq+ "/DateText", Priority = mrgSeqNo });

                string IsDesignation = (GetData == null || GetData.IsDesignation) ? "true" : "false";
                string IsDesignationRequired = (GetData != null && GetData.IsDesigMandatory) ? "true" : "false";
                string DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : "Designation";
                int? dsgSeqNo = fieldPriorityList.Where(a => a.FieldName == "DesignationTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = DesignationTextName, Values = "DesignationTextName/"+IsDesignation+"/"+IsDesignationRequired+ "/NormalText", Priority = dsgSeqNo });

                string IsEmail = (GetData == null || GetData.IsEmailId) ? "true" : "false";
                string IsEmailReq = (GetData != null && GetData.IsEmailMandatory) ? "true" : "false";
                string EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";
                int? emlSeqNo = fieldPriorityList.Where(a => a.FieldName == "EmailIdTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = EmailIdTextName, Values = "EmailIdTextName/"+IsEmail+"/"+IsEmailReq+ "/EmailText", Priority = emlSeqNo  });

                string IsAddress = (GetData == null || GetData.IsAddress) ? "true" : "false";
                string IsAddressRequired = (GetData != null && GetData.IsAddressMandatory) ? "true" : "false";
                string AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.AddressTextNameTextName : "Address";
                int? addrSeqNo = fieldPriorityList.Where(a => a.FieldName == "AddressTextNameTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = AddressTextNameTextName, Values = "AddressTextNameTextName/"+IsAddress+"/"+IsAddressRequired+ "/NormalText", Priority = addrSeqNo });

                string IsCountry = (GetData == null || GetData.IsCountry) ? "true" : "false";
                string IsCountryReq = (GetData != null && GetData.IsCountryMandatory) ? "true" : "false";
                string CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";
                int? coutSeqNo = fieldPriorityList.Where(a => a.FieldName == "CountryTextName").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = CountryTextName, Values = "CountryTextName/" + IsCountry+"/"+IsCountryReq+ "/DropDownList", Priority = coutSeqNo  });

                string IsState = (GetData == null || GetData.IsState) ? "true" : "false";
                string IsStateReq = (GetData != null && GetData.IsStateMandatory) ? "true" : "false";
                string StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";
                int? statSeqNo = fieldPriorityList.Where(a => a.FieldName == "StateTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = StateTextName, Values = "StateTextName/" + IsState+"/"+IsStateReq+ "/DropDownList", Priority = statSeqNo });


                string IsCity = (GetData == null || GetData.IsCity) ? "true" : "false";
                string IsCityReq = (GetData != null && GetData.IsCityMandatory) ? "true" : "false";
                string CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City/Location";
                int? ctySeqNo = fieldPriorityList.Where(a => a.FieldName == "CityTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = CityTextName, Values = "CityTextName/" + IsCity+"/"+IsCityReq+ "/DropDownList", Priority = ctySeqNo  });

                string IsUrl = (GetData == null || GetData.IsUrl) ? "true" : "false";
                string IsUrlReq  = (GetData != null && GetData.IsUrlMandatory) ? "true" : "false";
                string UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : "URL";
                int? urlSeqNo = fieldPriorityList.Where(a => a.FieldName == "UrlTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = UrlTextName, Values = "UrlTextName/"+IsUrl+"/"+IsUrlReq+ "/NormalText", Priority = urlSeqNo  });

                string IsSkype = (GetData == null || GetData.IsSkypeId) ? "true" : "false";
                string IsSkypReq = (GetData != null && GetData.IsSkypeIdMandatory) ? "true" : "false";
                string SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : "Skype ID";
                int? skyidSeqNo = fieldPriorityList.Where(a => a.FieldName == "SkypeIdTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = SkypeIdTextName, Values = "SkypeIdTextName/"+IsSkype+"/"+IsSkypReq+ "/NormalText", Priority = skyidSeqNo  });

                string IsLeadResource = (GetData == null || GetData.IsLeadResource) ? "true" : "false";
                string IsLeadResourceRequired = (GetData != null && GetData.IsLdResMandatory) ? "true" : "false";
                string LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                int? lrSeqNo = fieldPriorityList.Where(a => a.FieldName == "LeadResourceTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = LeadResourceTextName, Values = "LeadResourceTextName/"+IsLeadResource+"/"+IsLeadResourceRequired+ "/DropDownList", Priority = lrSeqNo });

                string IsPrdT = (GetData == null || GetData.IsProductType) ? "true" : "false";
                string IsPrdTReq = (GetData != null && GetData.IsProdTypeMandatory) ? "true" : "false";
                string ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                int? ptSeqNo = fieldPriorityList.Where(a => a.FieldName == "ProductTypeNameTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ProductTypeNameTextName, Values = "ProductTypeNameTextName/"+IsPrdT+"/"+IsPrdTReq+ "/DropDownList", Priority = ptSeqNo  });

                string IsExpDate = (GetData == null || GetData.IsExpectedClosingDate) ? "true" : "false";
                string IsExpDateReq = (GetData != null && GetData.IsExpClsDateMandatory) ? "true" : "false";
                string ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : "Expected Closing Date";
                int? expDtSeqNo = fieldPriorityList.Where(a => a.FieldName == "ExpectedDateTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExpectedDateTextName, Values = "ExpectedDateTextName/"+IsExpDate+"/"+IsExpDateReq+ "/DateText", Priority = expDtSeqNo });

                string IsExpAmt = (GetData == null || GetData.IsExpectedDealAmount) ? "true" : "false";
                string IsExpAmtReq = (GetData != null && GetData.IsExpDealAmtMandatory) ? "true" : "false";
                string ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : "Expected Deal Amount";
                int? expAmtSeqNo = fieldPriorityList.Where(a => a.FieldName == "ExpectedProductAmountTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExpectedProductAmountTextName, Values = "ExpectedProductAmountTextName/"+IsExpAmt+"/"+IsExpAmtReq+ "/DecimalText", Priority = expAmtSeqNo });

                string DescriptionTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";
                int? descSeqNo = fieldPriorityList.Where(a => a.FieldName == "DescriptionTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = DescriptionTextName, Values = "DescriptionTextName/true/true/NormalText", Priority = descSeqNo  });

                string IsOrgName = (GetData == null || GetData.IsOrganizationName) ? "true" : "false";
                string IsOrgNameReq = (GetData != null && GetData.IsOrgNameMandatory) ? "true" : "false";
                string OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";
                int? orgSeqNo = fieldPriorityList.Where(a => a.FieldName == "OrganizationNameTextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = OrganizationNameTextName, Values = "OrganizationNameTextName/"+IsOrgName+"/"+IsOrgNameReq+ "/NormalText", Priority = orgSeqNo  });

                string IsExtraCol1 = (GetData != null && GetData.IsExtraCol1) ? "true" : "false";
                string IsExtraCol1Required = (GetData != null && GetData.IsExtraCol1Mandatory) ? "true" : "false";
                string ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                int? ext1SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol1TextName, Values = "ExtraCol1TextName/"+IsExtraCol1+"/"+IsExtraCol1Required+ "/NormalText", Priority = ext1SeqNo  });

                //model.NormalFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol1TextName))
                //{
                //    model.NormalFieldCount = 1;
                //}

                string IsExtraCol2 = (GetData != null && GetData.IsExtraCol2) ? "true" : "false";
                string IsExtraCol2Required = (GetData != null && GetData.IsExtraCol2Mandatory) ? "true" : "false";
                string ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                int? ext2SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol2TextName, Values = "ExtraCol2TextName/" + IsExtraCol2 + "/" + IsExtraCol2Required + "/NormalText", Priority = ext2SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol1TextName) && string.IsNullOrEmpty(ExtraCol2TextName))
                //{
                //    model.NormalFieldCount = 2;
                //}


                string IsExtraCol3 = (GetData != null && GetData.IsExtraCol3) ? "true" : "false";
                string IsExtraCol3Required = (GetData != null && GetData.IsExtraCol3Mandatory) ? "true" : "false";
                string ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                int? ext3SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol3TextName, Values = "ExtraCol3TextName/" + IsExtraCol3 + "/" + IsExtraCol3Required + "/NormalText", Priority = ext3SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol1TextName) && string.IsNullOrEmpty(ExtraCol2TextName) && string.IsNullOrEmpty(ExtraCol3TextName))
                //{
                //    model.NormalFieldCount = 3;
                //}

                string IsExtraCol4 = (GetData != null && GetData.IsExtraCol4) ? "true" : "false";
                string IsExtraCol4Required = (GetData != null && GetData.IsExtraCol4Mandatory) ? "true" : "false";
                string ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                int? ext4SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol4TextName, Values = "ExtraCol4TextName/" + IsExtraCol4 + "/" + IsExtraCol4Required + "/NormalText", Priority = ext4SeqNo });
                //if (string.IsNullOrEmpty(ExtraCol1TextName) && string.IsNullOrEmpty(ExtraCol2TextName) && string.IsNullOrEmpty(ExtraCol3TextName) && string.IsNullOrEmpty(ExtraCol4TextName))
                //{
                //    model.NormalFieldCount = 4;
                //}

                string IsExtraCol5 = (GetData != null && GetData.IsExtraCol5) ? "true" : "false";
                string IsExtraCol5Required = (GetData != null && GetData.IsExtraCol5Mandatory) ? "true" : "false";
                string ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                int? ext5SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol5TextName, Values = "ExtraCol5TextName/" + IsExtraCol5 + "/" + IsExtraCol5Required + "/NormalText", Priority = ext5SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol1TextName) && string.IsNullOrEmpty(ExtraCol2TextName) && string.IsNullOrEmpty(ExtraCol3TextName) && string.IsNullOrEmpty(ExtraCol4TextName) && string.IsNullOrEmpty(ExtraCol5TextName))
                //{
                //    model.NormalFieldCount = 5;
                //}

                string IsExtraCol6 = (GetData != null && GetData.IsExtraCol6) ? "true" : "false";
                string IsExtraCol6Required = (GetData != null && GetData.IsExtraCol6Mandatory) ? "true" : "false";
                string ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                int? ext6SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol6TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol6TextName, Values = "ExtraCol6TextName/" + IsExtraCol6 + "/" + IsExtraCol6Required + "/DecimalText", Priority = ext6SeqNo });
               // model.DecimalFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol6TextName))
                //{
                //    model.DecimalFieldCount = 1;
                //}

                string IsExtraCol7 = (GetData != null && GetData.IsExtraCol7) ? "true" : "false";
                string IsExtraCol7Required = (GetData != null && GetData.IsExtraCol7Mandatory) ? "true" : "false";
                string ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                int? ext7SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol7TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol7TextName, Values = "ExtraCol7TextName/" + IsExtraCol7 + "/" + IsExtraCol7Required + "/NumberText", Priority = ext7SeqNo  });
                //model.NumericFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol7TextName))
                //{
                //    model.NumericFieldCount = 1;
                //}

                string IsExtraCol8 = (GetData != null && GetData.IsExtraCol8) ? "true" : "false";
                string IsExtraCol8Required = (GetData != null && GetData.IsExtraCol8Mandatory) ? "true" : "false";
                string ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                int? ext8SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol8TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol8TextName, Values = "ExtraCol8TextName/" + IsExtraCol8 + "/" + IsExtraCol8Required + "/NumberText", Priority = ext8SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol8TextName) && string.IsNullOrEmpty(ExtraCol8TextName))
                //{
                //    model.NumericFieldCount = 2;
                //}

                string IsExtraCol9 = (GetData != null && GetData.IsExtraCol9) ? "true" : "false";
                string IsExtraCol9Required = (GetData != null && GetData.IsExtraCol9Mandatory) ? "true" : "false";
                string ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                int? ext9SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol9TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol9TextName, Values = "ExtraCol9TextName/" + IsExtraCol9 + "/" + IsExtraCol9Required + "/DateText", Priority = ext9SeqNo  });
                //model.DateFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol9TextName))
                //{
                //    model.NumericFieldCount = 1;
                //}

                string IsExtraCol10 = (GetData != null && GetData.IsExtraCol10) ? "true" : "false";
                string IsExtraCol10Required = (GetData != null && GetData.IsExtraCol10Mandatory) ? "true" : "false";
                string ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                int? ext10SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol10TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol10TextName, Values = "ExtraCol10TextName/" + IsExtraCol10 + "/" + IsExtraCol10Required + "/DateText", Priority = ext10SeqNo  });
                
                //if (string.IsNullOrEmpty(ExtraCol9TextName) && string.IsNullOrEmpty(ExtraCol10TextName))
                //{
                //    model.DateFieldCount = 2;
                //}

                string IsExtraCol11 = (GetData != null && GetData.IsExtraCol11) ? "true" : "false";
                string IsExtraCol11Required = (GetData != null && GetData.IsExtraCol11Mandatory) ? "true" : "false";
                string ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                int? ext11SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol11TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol11TextName, Values = "ExtraCol11TextName/" + IsExtraCol11 + "/" + IsExtraCol11Required + "/DecimalText", Priority = ext11SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol6TextName) && string.IsNullOrEmpty(ExtraCol11TextName))
                //{
                //    model.DecimalFieldCount = 2;
                //}

                string IsExtraCol12 = (GetData != null && GetData.IsExtraCol12) ? "true" : "false";
                string IsExtraCol12Required = (GetData != null && GetData.IsExtraCol12Mandatory) ? "true" : "false";
                string ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                int? ext12SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol12TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol12TextName, Values = "ExtraCol12TextName/" + IsExtraCol12 + "/" + IsExtraCol12Required + "/DecimalText", Priority = ext12SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol6TextName) && string.IsNullOrEmpty(ExtraCol11TextName) && string.IsNullOrEmpty(ExtraCol12TextName))
                //{
                //    model.DecimalFieldCount = 3;
                //}

                string IsExtraCol13 = (GetData != null && GetData.IsExtraCol13) ? "true" : "false";
                string IsExtraCol13Required = (GetData != null && GetData.IsExtraCol13Mandatory) ? "true" : "false";
                string ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                int? ext13SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol13TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol13TextName, Values = "ExtraCol13TextName/" + IsExtraCol13 + "/" + IsExtraCol13Required + "/DecimalText", Priority = ext13SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol6TextName) && string.IsNullOrEmpty(ExtraCol11TextName) && string.IsNullOrEmpty(ExtraCol12TextName) && string.IsNullOrEmpty(ExtraCol13TextName))
                //{
                //    model.DecimalFieldCount = 4;
                //}

                string IsExtraCol14 = (GetData != null && GetData.IsExtraCol14) ? "true" : "false";
                string IsExtraCol14Required = (GetData != null && GetData.IsExtraCol14Mandatory) ? "true" : "false";
                string ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                int? ext14SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol14TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol14TextName, Values = "ExtraCol14TextName/" + IsExtraCol14 + "/" + IsExtraCol14Required + "/DecimalText", Priority = ext14SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol6TextName) && string.IsNullOrEmpty(ExtraCol11TextName) && string.IsNullOrEmpty(ExtraCol12TextName) && string.IsNullOrEmpty(ExtraCol13TextName) && string.IsNullOrEmpty(ExtraCol14TextName))
                //{
                //    model.DecimalFieldCount = 5;
                //}

                string IsExtraCol15 = (GetData != null && GetData.IsExtraCol15) ? "true" : "false";
                string IsExtraCol15Required = (GetData != null && GetData.IsExtraCol15Mandatory) ? "true" : "false";
                string ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                int? ext15SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol15TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol15TextName, Values = "ExtraCol15TextName/" + IsExtraCol15 + "/" + IsExtraCol15Required + "/NumberText", Priority = ext15SeqNo });
                //if (string.IsNullOrEmpty(ExtraCol7TextName) && string.IsNullOrEmpty(ExtraCol8TextName) && string.IsNullOrEmpty(ExtraCol15TextName))
                //{
                //    model.NumericFieldCount = 3;
                //}

                string IsExtraCol16 = (GetData != null && GetData.IsExtraCol16) ? "true" : "false";
                string IsExtraCol16Required = (GetData != null && GetData.IsExtraCol16Mandatory) ? "true" : "false";
                string ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                int? ext16SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol16TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol16TextName, Values = "ExtraCol16TextName/" + IsExtraCol16 + "/" + IsExtraCol16Required + "/NumberText", Priority = ext16SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol7TextName) && string.IsNullOrEmpty(ExtraCol8TextName) && string.IsNullOrEmpty(ExtraCol15TextName) && string.IsNullOrEmpty(ExtraCol16TextName))
                //{
                //    model.NumericFieldCount = 4;
                //}

                string IsExtraCol17 = (GetData != null && GetData.IsExtraCol17) ? "true" : "false";
                string IsExtraCol17Required = (GetData != null && GetData.IsExtraCol17Mandatory) ? "true" : "false";
                string ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                int? ext17SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol17TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol17TextName, Values = "ExtraCol17TextName/" + IsExtraCol17 + "/" + IsExtraCol17Required + "/NumberText", Priority = ext17SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol7TextName) && string.IsNullOrEmpty(ExtraCol8TextName) && string.IsNullOrEmpty(ExtraCol15TextName) && string.IsNullOrEmpty(ExtraCol16TextName) && string.IsNullOrEmpty(ExtraCol17TextName))
                //{
                //    model.NumericFieldCount = 5;
                //}

                string IsExtraCol18 = (GetData != null && GetData.IsExtraCol18) ? "true" : "false";
                string IsExtraCol18Required = (GetData != null && GetData.IsExtraCol18Mandatory) ? "true" : "false";
                string ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                int? ext18SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol18TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol18TextName, Values = "ExtraCol18TextName/" + IsExtraCol18 + "/" + IsExtraCol18Required + "/DateText", Priority = ext18SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol9TextName) && string.IsNullOrEmpty(ExtraCol10TextName) && string.IsNullOrEmpty(ExtraCol18TextName))
                //{
                //    model.DateFieldCount = 3;
                //}

                string IsExtraCol19 = (GetData != null && GetData.IsExtraCol19) ? "true" : "false";
                string IsExtraCol19Required = (GetData != null && GetData.IsExtraCol19Mandatory) ? "true" : "false";
                string ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                int? ext19SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol19TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol19TextName, Values = "ExtraCol19TextName/" + IsExtraCol19 + "/" + IsExtraCol19Required + "/DateText", Priority = ext19SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol9TextName) && string.IsNullOrEmpty(ExtraCol10TextName) && string.IsNullOrEmpty(ExtraCol18TextName) && string.IsNullOrEmpty(ExtraCol19TextName))
                //{
                //    model.DateFieldCount = 4;
                //}

                string IsExtraCol20 = (GetData != null && GetData.IsExtraCol20) ? "true" : "false";
                string IsExtraCol20Required = (GetData != null && GetData.IsExtraCol20Mandatory) ? "true" : "false";
                string ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                int? ext20SeqNo = fieldPriorityList.Where(a => a.FieldName == "ExtraCol20TextName").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol20TextName, Values = "ExtraCol20TextName/" + IsExtraCol20 + "/" + IsExtraCol20Required + "/DateText", Priority = ext20SeqNo  });
                //if (string.IsNullOrEmpty(ExtraCol9TextName) && string.IsNullOrEmpty(ExtraCol10TextName) && string.IsNullOrEmpty(ExtraCol18TextName) && string.IsNullOrEmpty(ExtraCol19TextName) && string.IsNullOrEmpty(ExtraCol20TextName))
                //{
                //    model.DateFieldCount = 5;
                //}
                model.FieldNames = fieldsList;
                #endregion

                #region old code not in use form fields values
                //model.FormTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FormTextName) ? GetFormData.FormTextName : "Create New Lead";
                //model.HeaderMenuTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.HeaderMenuTextName) ? GetFormData.HeaderMenuTextName : "Lead";
                //model.AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.LeadStatusTextName : "Address";
                //model.CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                //model.DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : "Date Of Birth";
                //model.DescriptionTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";
                //model.DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : "Designation";
                ////model.DNDStatusTextName = GetFormData.DNDStatusTextName;
                //model.EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";
                //model.ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : "Expected Closing Date";
                //model.ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : "Expected Deal Amount";

                //model.LeadOwnerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Lead Owner";
                //model.LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                //model.LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                //model.FollowDateTextName= GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                //model.FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                //model.FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";

                //model.MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : "Marriage Anniversary";
                //model.MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                //model.OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";

                //model.ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                //model.SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : "Skype ID";
                //model.UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : "URL";
                //model.ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";

                //model.ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                //model.ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                //model.ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                //model.ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                //model.ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                //model.ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                //model.ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                //model.ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                //model.ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                //model.ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;

                //model.IsCustomer = true;
                //model.IsDescription = true;
                //model.IsMobileNo = true;
                //model.IsLeadStatus = true;
                //model.IsFollowupDate = true;

                #endregion

            }
            else
            {
                return Redirect("/home/login");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateSetting(CreateLeadFieldDTO model)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            //string msg = "";
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var GetData = db.crm_createleadsetting.Where(a => a.BranchID == BranchID && a.CompanyID == CompanyID).FirstOrDefault();
                        var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

                        if (GetFormData != null && GetData != null)
                        {
                            if (model.SaveType == "New" && !string.IsNullOrEmpty(model.FieldType) && !string.IsNullOrEmpty(model.FieldTextName))//check if save type is not null for add new field
                            {
                                if (model.FieldType == "NormalText")//check field type for insert column string data type
                                {
                                    if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                    {
                                        GetFormData.DesignationTextName = model.FieldTextName;
                                        GetData.IsDesignation = model.IsVisible;
                                        GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'DesignationTextName'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                    {
                                        GetFormData.OrganizationNameTextName = model.FieldTextName;
                                        GetData.IsOrganizationName = model.IsVisible;
                                        GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'OrganizationNameTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                    {
                                        GetFormData.AddressTextNameTextName = model.FieldTextName;
                                        GetData.IsAddress = model.IsVisible;
                                        GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'AddressTextNameTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                    {
                                        GetFormData.UrlTextName = model.FieldTextName;
                                        GetData.IsUrl = model.IsVisible;
                                        GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'UrlTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                    {
                                        GetFormData.SkypeIdTextName = model.FieldTextName;
                                        GetData.IsSkypeId = model.IsVisible;
                                        GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'SkypeIdTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                    {
                                        GetFormData.ExtraCol1TextName = model.FieldTextName;
                                        GetData.IsExtraCol1 = model.IsVisible;
                                        GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                    {
                                        GetFormData.ExtraCol2TextName = model.FieldTextName;
                                        GetData.IsExtraCol2 = model.IsVisible;
                                        GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                    {
                                        GetFormData.ExtraCol3TextName = model.FieldTextName;
                                        GetData.IsExtraCol3 = model.IsVisible;
                                        GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3TextName'");

                                        db.SaveChanges();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                    {
                                        GetFormData.ExtraCol4TextName = model.FieldTextName;
                                        GetData.IsExtraCol4 = model.IsVisible;
                                        GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                    {
                                        GetFormData.ExtraCol5TextName = model.FieldTextName;
                                        GetData.IsExtraCol5 = model.IsVisible;
                                        GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No Field available for normal text type";
                                    }
                                }
                                else if (model.FieldType == "DecimalText")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                    {
                                        GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                        GetData.IsExpectedDealAmount = model.IsVisible;
                                        GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExpectedProductAmountTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                    {
                                        GetFormData.ExtraCol6TextName = model.FieldTextName;
                                        GetData.IsExtraCol6 = model.IsVisible;
                                        GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                    {
                                        GetFormData.ExtraCol11TextName = model.FieldTextName;
                                        GetData.IsExtraCol11 = model.IsVisible;
                                        GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                    {
                                        GetFormData.ExtraCol12TextName = model.FieldTextName;
                                        GetData.IsExtraCol12 = model.IsVisible;
                                        GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                    {
                                        GetFormData.ExtraCol13TextName = model.FieldTextName;
                                        GetData.IsExtraCol13 = model.IsVisible;
                                        GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol13TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                    {
                                        GetFormData.ExtraCol14TextName = model.FieldTextName;
                                        GetData.IsExtraCol14 = model.IsVisible;
                                        GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol14TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for decimal type";
                                    }
                                }
                                else if (model.FieldType == "NumberText")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                    {
                                        GetFormData.ExtraCol7TextName = model.FieldTextName;
                                        GetData.IsExtraCol7 = model.IsVisible;
                                        GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                    {
                                        GetFormData.ExtraCol8TextName = model.FieldTextName;
                                        GetData.IsExtraCol8 = model.IsVisible;
                                        GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                    {
                                        GetFormData.ExtraCol15TextName = model.FieldTextName;
                                        GetData.IsExtraCol15 = model.IsVisible;
                                        GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol15TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                    {
                                        GetFormData.ExtraCol16TextName = model.FieldTextName;
                                        GetData.IsExtraCol16 = model.IsVisible;
                                        GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol16TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                    {
                                        GetFormData.ExtraCol17TextName = model.FieldTextName;
                                        GetData.IsExtraCol17 = model.IsVisible;
                                        GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol17TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for number type";
                                    }
                                }
                                else if (model.FieldType == "DateText")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                    {
                                        GetFormData.DateofBirthTextName = model.FieldTextName;
                                        GetData.IsDateofBirth = model.IsVisible;
                                        GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'DateofBirthTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                    {
                                        GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                        GetData.IsMarriageAnniversary = model.IsVisible;
                                        GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'MarriageAnniversaryTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                    {
                                        GetFormData.ExpectedDateTextName = model.FieldTextName;
                                        GetData.IsExpectedClosingDate = model.IsVisible;
                                        GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExpectedDateTextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                    {
                                        GetFormData.ExtraCol9TextName = model.FieldTextName;
                                        GetData.IsExtraCol9 = model.IsVisible;
                                        GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                    {
                                        GetFormData.ExtraCol10TextName = model.FieldTextName;
                                        GetData.IsExtraCol10 = model.IsVisible;
                                        GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                    {
                                        GetFormData.ExtraCol18TextName = model.FieldTextName;
                                        GetData.IsExtraCol18 = model.IsVisible;
                                        GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol18TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                    {
                                        GetFormData.ExtraCol19TextName = model.FieldTextName;
                                        GetData.IsExtraCol19 = model.IsVisible;
                                        GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol19TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                    {
                                        GetFormData.ExtraCol20TextName = model.FieldTextName;
                                        GetData.IsExtraCol20 = model.IsVisible;
                                        GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_create_lead_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol20TextName'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for date type";
                                    }
                                }

                            }//if end for add new field
                            else if (!string.IsNullOrEmpty(model.FieldTextName))//check field text name not null
                            {
                                var GetSeqData = db.crm_create_lead_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == model.FieldName).FirstOrDefault();
                                model.FieldPriority = GetSeqData.Priority;
                                if (model.FieldName == "CustomerTextName")
                                {
                                    GetFormData.CustomerTextName = model.FieldTextName;
                                    GetData.IsCustomer = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "MobileNoTextName")
                                {
                                    GetFormData.MobileNoTextName = model.FieldTextName;
                                    GetData.IsMobileNo = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "LeadStatusTextName")
                                {
                                    GetFormData.LeadStatusTextName = model.FieldTextName;
                                    GetData.IsLeadStatus = true;
                                    GetData.IsLdStatusMandatory = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "LeadOwnerTextName")
                                {
                                    GetFormData.LeadOwnerTextName = model.FieldTextName;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "FollowDateTextName")
                                {
                                    GetFormData.FollowDateTextName = model.FieldTextName;
                                    GetData.IsFollowupDate = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "DescriptionTextName")
                                {
                                    GetFormData.DescriptionTextName = model.FieldTextName;
                                    GetData.IsDescription = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "EmailIdTextName")
                                {
                                    GetFormData.EmailIdTextName = model.FieldTextName;
                                    GetData.IsEmailId = model.IsVisible;
                                    GetData.IsEmailMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "CountryTextName")
                                {
                                    GetData.IsCountry = model.IsVisible;
                                    GetData.IsCountryMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    GetFormData.CountryTextName = model.FieldTextName;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "StateTextName")
                                {
                                    GetData.IsState = model.IsVisible;
                                    GetData.IsStateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    GetFormData.StateTextName = model.FieldTextName;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "CityTextName")
                                {
                                    GetData.IsCity = model.IsVisible;
                                    GetData.IsCityMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    GetFormData.CityTextName = model.FieldTextName;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "FollowUpTimeTextName")
                                {
                                    GetFormData.FollowUpTimeTextName = model.FieldTextName;
                                    GetData.IsFollowUpTime = model.IsVisible;
                                    GetData.IsFUpTimeMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "FollowupTimeinISTTextName")
                                {
                                    GetFormData.FollowupTimeinISTTextName = model.FieldTextName;
                                    GetData.IsFollowupTimeinIST = model.IsVisible;
                                    GetData.IsFupTimeinISTMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "ZoneNameTextName")
                                {
                                    GetFormData.ZoneNameTextName = model.FieldTextName;
                                    GetData.IsTimeZoneName = model.IsVisible;
                                    GetData.IsTZNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";
                                }
                                else if (model.FieldName == "LeadResourceTextName")
                                {
                                    GetFormData.LeadResourceTextName = model.FieldTextName;
                                    GetData.IsLeadResource = model.IsVisible;
                                    GetData.IsLdResMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "ProductTypeNameTextName")
                                {
                                    GetFormData.ProductTypeNameTextName = model.FieldTextName;
                                    GetData.IsProductType = model.IsVisible;
                                    GetData.IsProdTypeMandatory = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "DesignationTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)//if current field type equal to previous field type then update
                                    {
                                        GetFormData.DesignationTextName = model.FieldTextName;
                                        GetData.IsDesignation = model.IsVisible;
                                        GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.DesignationTextName = null;
                                                GetData.IsDesignation = false;
                                                GetData.IsDesigMandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "DateofBirthTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)//if current field type equal to previous field type
                                    {
                                        GetFormData.DateofBirthTextName = model.FieldTextName;
                                        GetData.IsDateofBirth = model.IsVisible;
                                        GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.DateofBirthTextName = null;
                                                GetData.IsDateofBirth = false;
                                                GetData.IsDOBMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "MarriageAnniversaryTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                        GetData.IsMarriageAnniversary = model.IsVisible;
                                        GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.MarriageAnniversaryTextName = null;
                                                GetData.IsMarriageAnniversary = false;
                                                GetData.IsMrgAniMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "OrganizationNameTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.OrganizationNameTextName = model.FieldTextName;
                                        GetData.IsOrganizationName = model.IsVisible;
                                        GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.OrganizationNameTextName = null;
                                                GetData.IsOrganizationName = false;
                                                GetData.IsOrgNameMandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "AddressTextNameTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.AddressTextNameTextName = model.FieldTextName;
                                        GetData.IsAddress = model.IsVisible;
                                        GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.AddressTextNameTextName = null;
                                                GetData.IsAddress = false;
                                                GetData.IsAddressMandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "UrlTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.UrlTextName = model.FieldTextName;
                                        GetData.IsUrl = model.IsVisible;
                                        GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.UrlTextName = null;
                                                GetData.IsUrl = false;
                                                GetData.IsUrlMandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "SkypeIdTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.SkypeIdTextName = model.FieldTextName;
                                        GetData.IsSkypeId = model.IsVisible;
                                        GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.SkypeIdTextName = null;
                                                GetData.IsSkypeId = false;
                                                GetData.IsSkypeIdMandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExpectedDateTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExpectedDateTextName = model.FieldTextName;
                                        GetData.IsExpectedClosingDate = model.IsVisible;
                                        GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExpectedDateTextName = null;
                                                GetData.IsExpectedClosingDate = false;
                                                GetData.IsExpClsDateMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExpectedProductAmountTextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                        GetData.IsExpectedDealAmount = model.IsVisible;
                                        GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                       
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExpectedProductAmountTextName = null;
                                                GetData.IsExpectedDealAmount = false;
                                                GetData.IsExpDealAmtMandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol1TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol1TextName = model.FieldTextName;
                                        GetData.IsExtraCol1 = model.IsVisible;
                                        GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol2TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol2TextName = model.FieldTextName;
                                        GetData.IsExtraCol2 = model.IsVisible;
                                        GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol1TextName = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol2TextName = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol3TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol3TextName = model.FieldTextName;
                                        GetData.IsExtraCol3 = model.IsVisible;
                                        GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol4TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol4TextName = model.FieldTextName;
                                        GetData.IsExtraCol4 = model.IsVisible;
                                        GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol3TextName = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol4TextName = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol5TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol5TextName = model.FieldTextName;
                                        GetData.IsExtraCol5 = model.IsVisible;
                                        GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol5TextName = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol6TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol6TextName = model.FieldTextName;
                                        GetData.IsExtraCol6 = model.IsVisible;
                                        GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                       
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol6TextName = null;
                                                GetData.IsExtraCol6 = false;
                                                GetData.IsExtraCol6Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol7TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol7TextName = model.FieldTextName;
                                        GetData.IsExtraCol7 = model.IsVisible;
                                        GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                                                             
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol7TextName = null;
                                                GetData.IsExtraCol7 = false;
                                                GetData.IsExtraCol7Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol8TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol8TextName = model.FieldTextName;
                                        GetData.IsExtraCol8 = model.IsVisible;
                                        GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                                                             
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol8TextName = null;
                                                GetData.IsExtraCol8 = false;
                                                GetData.IsExtraCol8Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol9TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol9TextName = model.FieldTextName;
                                        GetData.IsExtraCol9 = model.IsVisible;
                                        GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol9TextName = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol10TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol10TextName = model.FieldTextName;
                                        GetData.IsExtraCol10 = model.IsVisible;
                                        GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol10TextName = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol11TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol11TextName = model.FieldTextName;
                                        GetData.IsExtraCol11 = model.IsVisible;
                                        GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                       
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol11TextName = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol12TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol12TextName = model.FieldTextName;
                                        GetData.IsExtraCol12 = model.IsVisible;
                                        GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                       
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol12TextName = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol13TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol13TextName = model.FieldTextName;
                                        GetData.IsExtraCol13 = model.IsVisible;
                                        GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                       
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol13TextName = null;
                                                GetData.IsExtraCol13 = false;
                                                GetData.IsExtraCol13Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol14TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol14TextName = model.FieldTextName;
                                        GetData.IsExtraCol14 = model.IsVisible;
                                        GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                       
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol14TextName = null;
                                                GetData.IsExtraCol14 = false;
                                                GetData.IsExtraCol14Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol15TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol15TextName = model.FieldTextName;
                                        GetData.IsExtraCol15 = model.IsVisible;
                                        GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                                                             
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol15TextName = null;
                                                GetData.IsExtraCol15 = false;
                                                GetData.IsExtraCol15Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol16TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol16TextName = model.FieldTextName;
                                        GetData.IsExtraCol16 = model.IsVisible;
                                        GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                                                             
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol16TextName = null;
                                                GetData.IsExtraCol16 = false;
                                                GetData.IsExtraCol16Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol17TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol17TextName = model.FieldTextName;
                                        GetData.IsExtraCol17 = model.IsVisible;
                                        GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DesignationTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition                                                                             
                                        else if (model.FieldType == "DateText")//else if start date text
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.DateofBirthTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.DateofBirthTextName = model.FieldTextName;
                                                GetData.IsDateofBirth = model.IsVisible;
                                                GetData.IsDOBMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.MarriageAnniversaryTextName = model.FieldTextName;
                                                GetData.IsMarriageAnniversary = model.IsVisible;
                                                GetData.IsMrgAniMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExpectedDateTextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExpectedDateTextName = model.FieldTextName;
                                                GetData.IsExpectedClosingDate = model.IsVisible;
                                                GetData.IsExpClsDateMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol9TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol9TextName = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol10TextName = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol18TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol18TextName = model.FieldTextName;
                                                GetData.IsExtraCol18 = model.IsVisible;
                                                GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol19TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol19TextName = model.FieldTextName;
                                                GetData.IsExtraCol19 = model.IsVisible;
                                                GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol20TextName))
                                            {
                                                GetFormData.ExtraCol17TextName = null;
                                                GetData.IsExtraCol17 = false;
                                                GetData.IsExtraCol17Mandatory = false;

                                                GetFormData.ExtraCol20TextName = model.FieldTextName;
                                                GetData.IsExtraCol20 = model.IsVisible;
                                                GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }//else if end of date text
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol18TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol18TextName = model.FieldTextName;
                                        GetData.IsExtraCol18 = model.IsVisible;
                                        GetData.IsExtraCol18Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol18TextName = null;
                                                GetData.IsExtraCol18 = false;
                                                GetData.IsExtraCol18Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol19TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol19TextName = model.FieldTextName;
                                        GetData.IsExtraCol19 = model.IsVisible;
                                        GetData.IsExtraCol19Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol19TextName = null;
                                                GetData.IsExtraCol19 = false;
                                                GetData.IsExtraCol19Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol20TextName")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol20TextName = model.FieldTextName;
                                        GetData.IsExtraCol20 = model.IsVisible;
                                        GetData.IsExtraCol20Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
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

                                                GetFormData.DesignationTextName = model.FieldTextName;
                                                GetData.IsDesignation = model.IsVisible;
                                                GetData.IsDesigMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.OrganizationNameTextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.OrganizationNameTextName = model.FieldTextName;
                                                GetData.IsOrganizationName = model.IsVisible;
                                                GetData.IsOrgNameMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.AddressTextNameTextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.AddressTextNameTextName = model.FieldTextName;
                                                GetData.IsAddress = model.IsVisible;
                                                GetData.IsAddressMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.UrlTextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.UrlTextName = model.FieldTextName;
                                                GetData.IsUrl = model.IsVisible;
                                                GetData.IsUrlMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.SkypeIdTextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.SkypeIdTextName = model.FieldTextName;
                                                GetData.IsSkypeId = model.IsVisible;
                                                GetData.IsSkypeIdMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol1TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol1TextName = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol2TextName = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol3TextName = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol4TextName = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol5TextName = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }//if end of normarl text condition
                                        else if (model.FieldType == "DecimalText")//else if decimal text condition start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExpectedProductAmountTextName = model.FieldTextName;
                                                GetData.IsExpectedDealAmount = model.IsVisible;
                                                GetData.IsExpDealAmtMandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol6TextName = model.FieldTextName;
                                                GetData.IsExtraCol6 = model.IsVisible;
                                                GetData.IsExtraCol6Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol11TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol11TextName = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol12TextName = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol13TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol13TextName = model.FieldTextName;
                                                GetData.IsExtraCol13 = model.IsVisible;
                                                GetData.IsExtraCol13Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol14TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol14TextName = model.FieldTextName;
                                                GetData.IsExtraCol14 = model.IsVisible;
                                                GetData.IsExtraCol14Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }//else if end fo decimal text type
                                        else if (model.FieldType == "NumberText")//else if number text start
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol7TextName = model.FieldTextName;
                                                GetData.IsExtraCol7 = model.IsVisible;
                                                GetData.IsExtraCol7Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol8TextName = model.FieldTextName;
                                                GetData.IsExtraCol8 = model.IsVisible;
                                                GetData.IsExtraCol8Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol15TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol15TextName = model.FieldTextName;
                                                GetData.IsExtraCol15 = model.IsVisible;
                                                GetData.IsExtraCol15Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol16TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol16TextName = model.FieldTextName;
                                                GetData.IsExtraCol16 = model.IsVisible;
                                                GetData.IsExtraCol16Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol17TextName))
                                            {
                                                GetFormData.ExtraCol20TextName = null;
                                                GetData.IsExtraCol20 = false;
                                                GetData.IsExtraCol20Mandatory = false;

                                                GetFormData.ExtraCol17TextName = model.FieldTextName;
                                                GetData.IsExtraCol17 = model.IsVisible;
                                                GetData.IsExtraCol17Mandatory = model.IsVisible == true ? model.IsRequired : false;
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }//else if end of number text

                                        #endregion
                                    }
                                }

                            }
                        }//getform data and get data if condition end


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ExceptionLogging.SendExcepToDB(ex);
                        TempData["alert"] = "Sorry! there is some technical problem";
                    }
                }

            }
            else
            {
                return Redirect("/home/login");
            }

            return RedirectToAction("CreateSetting");
        }


        public JsonResult ChangeFilterStatus(string FldNM) 
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_viewleadsetting set "+FldNM+ "=case when " + FldNM + "=1 then 0 else 1 end where BranchID ="+ BranchID + "  and CompanyID = "+ CompanyID + "");
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeVisibleStatus(string FldNM) 
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_viewleadsetting set " + FldNM + "=case when " + FldNM + "=1 then 0 else 1 end where BranchID =" + BranchID + "  and CompanyID = " + CompanyID + "");
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HideField(string FieldName) 
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var msg = "";
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {

                    var GetData = db.crm_createleadsetting.Where(a => a.BranchID == BranchID && a.CompanyID == CompanyID).FirstOrDefault();
                    var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                    var GetSeqData = db.crm_create_lead_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName==FieldName).FirstOrDefault();
                    if (GetFormData != null && GetData != null && GetSeqData!=null)
                    {

                        if (!string.IsNullOrEmpty(FieldName))//check field name not null
                        {

                            if (FieldName == "DesignationTextName")
                            {
                                GetFormData.DesignationTextName = null;
                                GetData.IsDesignation = false;
                                GetData.IsDesigMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "DateofBirthTextName")
                            {
                                GetFormData.DateofBirthTextName = null;
                                GetData.IsDateofBirth = false;
                                GetData.IsDOBMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "MarriageAnniversaryTextName")
                            {
                                GetFormData.MarriageAnniversaryTextName = null;
                                GetData.IsMarriageAnniversary = false;
                                GetData.IsMrgAniMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "OrganizationNameTextName")
                            {
                                GetFormData.OrganizationNameTextName = null;
                                GetData.IsOrganizationName = false;
                                GetData.IsOrgNameMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "AddressTextNameTextName")
                            {
                                GetFormData.AddressTextNameTextName = null;
                                GetData.IsAddress = false;
                                GetData.IsAddressMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "UrlTextName")
                            {
                                GetFormData.UrlTextName = null;
                                GetData.IsUrl = false;
                                GetData.IsUrlMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "SkypeIdTextName")
                            {
                                GetFormData.SkypeIdTextName = null;
                                GetData.IsSkypeId = false;
                                GetData.IsSkypeIdMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExpectedDateTextName")
                            {
                                GetFormData.ExpectedDateTextName = null;
                                GetData.IsExpectedClosingDate = false;
                                GetData.IsExpClsDateMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExpectedProductAmountTextName")
                            {
                                GetFormData.ExpectedProductAmountTextName = null;
                                GetData.IsExpectedDealAmount = false;
                                GetData.IsExpDealAmtMandatory = false;
                                GetSeqData.Priority = 0;
                                db.SaveChanges();
                                trans.Commit();
                                msg = "ok";
                                //return Json("ok", JsonRequestBehavior.AllowGet);
                            }
                            else if (FieldName == "ExtraCol1TextName")
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
                            msg = "err";
                            //return Json("err", JsonRequestBehavior.AllowGet);
                        }
                    }//getform data and get data if condition end
                    else
                    {
                        msg = "err";                       
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    //TempData["alert"] = "Sorry! there is some technical problem";
                    //return Json("err", JsonRequestBehavior.AllowGet);
                    msg = "err";
                }
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FieldPriority(string fieldName,  int Priority)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var uid = Convert.ToInt32(Session["UID"]);
            string msg = "";
            try
            {
                var GetSeqData = db.crm_create_lead_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == fieldName).FirstOrDefault();
                if (GetSeqData != null)
                {
                    GetSeqData.Priority = Priority;
                    GetSeqData.ModifiedDate = Constant.GetBharatTime();
                    db.SaveChanges();
                }
                else
                {
                    var fp = new crm_create_lead_field_sequence
                    {
                        Priority = Priority,
                        FieldName = fieldName,
                        CompanyID = CompanyID,
                        BranchID = BranchID,
                        CreatedBy = uid,
                        Createddate = Constant.GetBharatTime(),
                        ModifiedDate = Constant.GetBharatTime()
                    };
                    db.crm_create_lead_field_sequence.Add(fp);
                    db.SaveChanges();
                }
                msg = "ok";
            }
            catch(Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "err";
            }                       
            return Json(msg,JsonRequestBehavior.AllowGet);
        }
    }
}
