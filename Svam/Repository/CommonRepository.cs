using AutoMapper;
using Svam._Class;
using Svam.EF;
using Svam.Models.DTO;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Traders.Models;

namespace Svam.Repository
{
    public class CommonRepository
    {
        public CreatRoleModel GetUserRights()
        {
            niscrmEntities db = new niscrmEntities();

            //get user profileId, branchId and companyId by using session
            int ProfileId = HttpContext.Current.Session["UserProfileId"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["UserProfileId"]);
            int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            var UserType = HttpContext.Current.Session["UserType"] == null ? "" : Convert.ToString(HttpContext.Current.Session["UserType"]);

            crm_roleassigntbl GetPermission = new crm_roleassigntbl();
            CreatRoleModel CreateroleModel = new CreatRoleModel();
            if (UserType == "SuperAdmin")
            {
                //GetPermission.Id = 0;
                //GetPermission.ProfileName = UserType;
                //GetPermission.CreateLeads = true;
                //GetPermission.ViewLeads = true;
                //GetPermission.ViewSales = true;
                //GetPermission.AssignLeadManagement = true;
                //GetPermission.DeveloperReport = true;
                //GetPermission.CommonActivityRemark = true;
                //GetPermission.RoleManagement = true;
                //GetPermission.Notification = true;
                //GetPermission.LeadNotify = true;
                //GetPermission.ProjectManagement = true;
                //GetPermission.LeadManagement = true;
                //GetPermission.DailyWorkSchedule = true;
                //GetPermission.Status = true;
                //GetPermission.Viewpayment = true;
                //GetPermission.TicketManagement = true;
                //GetPermission.NewLeave = true;
                //GetPermission.ViewLeave = true;
                //GetPermission.FileManager = true;
                //GetPermission.TrackSalePerson = true;
                //GetPermission.ViewMapSalePerson = true;
                //GetPermission.AddQuotation = true;
                //GetPermission.CreateAssignWork = true;
                //GetPermission.ViewAssignedWorks = true;
                //GetPermission.LeaveManagement = true;
                //GetPermission.EmpAttendence = true;
                //GetPermission.EmpLoginHistory = true;
                //GetPermission.ManualAttandence = true;
                //GetPermission.ManageSalary = true;
                //GetPermission.ManageExtraPayment = true;
                //GetPermission.EmpLeaveRequest = true;
                //GetPermission.CreateInterviewSchedule = true;
                //GetPermission.ViewInterviewSchedule = true;
                //GetPermission.ManageUser = true;
                //GetPermission.CreateTicket = true;
                //GetPermission.ViewTicket = true;
                //GetPermission.MappedUser = true;
                //GetPermission.CreateDailyRemark = true;
                //GetPermission.ViewDailyRemark = true;
                //GetPermission.SaleTarget = true;
                //GetPermission.CreateLeadSetting = true;
                //GetPermission.ViewLeadSetting = true;


                var type = CreateroleModel.GetType();
                PropertyInfo[] fields = type.GetProperties(); // Obtain all property

                if (fields.Length != 0)
                    foreach (var field in fields) // Loop through properties
                    {
                        //var t = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;                      
                        if (field.PropertyType == typeof(bool)) // if it is a bool.
                            field.SetValue(CreateroleModel, true, null);  //set value true                                             
                    }
            }
            else
            {
                if (BranchID > 0)
                    GetPermission = db.crm_roleassigntbl.Where(em => em.Id == ProfileId && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                CreateroleModel = Mapper.Map<CreatRoleModel>(GetPermission);
            }


            return CreateroleModel;
        }

        public ViewLeadSettingDTO GetViewLeadsetting()
        {
            niscrmEntities db = new niscrmEntities();

            //get user branchId and companyId by using session
            int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            int UID = HttpContext.Current.Session["UID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["UID"]);
            var GetData = new ViewLeadSettingDTO();
            string placeHolder = string.Empty;
            var sb = new StringBuilder();
            if (BranchID > 0 && CompanyID > 0)
            {
                var data = db.crm_viewleadsetting.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                //GetData = Mapper.Map<ViewLeadSettingDTO>(data);
                if (data != null)
                {
                    GetData.BranchID = data.BranchID;
                    GetData.CompanyID = data.CompanyID;
                    GetData.IsEmail = data.IsEmail;
                    GetData.IsCity = data.IsCity;
                    GetData.IsAssignedBy = data.IsAssignedBy;
                    GetData.IsAssignTo = data.IsAssignTo;
                    GetData.IsAssignedDate = data.IsAssignedDate;
                    GetData.IsExpClosingDate = data.IsExpClosingDate;
                    GetData.IsExpDealAmount = data.IsExpDealAmount;
                    GetData.IsCreatedBy = data.IsCreatedBy;
                    GetData.IsCreatedDate = data.IsCreatedDate;
                    GetData.IsModifiedDate = data.IsModifiedDate;
                    //GetData.ReportTextName = data.ReportTextName;
                    //GetData.LeadSampleTextName = data.LeadSampleTextName;
                    //GetData.FBLeadSampleTextName = data.FBLeadSampleTextName;
                    //GetData.TotalLeadTextName = data.TotalLeadTextName;
                    GetData.IsCountry = data.IsCountry;
                    GetData.IsState = data.IsState;
                    GetData.IsProductType = data.IsProductType;
                    GetData.IsLeadResource = data.IsLeadResource;
                    GetData.IsAddress = data.IsAddress;
                    GetData.IsUrl = data.IsUrl;
                    GetData.IsSkypeId = data.IsSkypeId;
                    GetData.IsTimeZoneName = data.IsTimeZoneName;
                    GetData.IsOrganization = data.IsOrganization;
                    GetData.IsExtraCol1 = data.IsExtraCol1;
                    GetData.IsExtraCol2 = data.IsExtraCol2;
                    GetData.IsExtraCol3 = data.IsExtraCol3;
                    GetData.IsExtraCol4 = data.IsExtraCol4;
                    GetData.IsExtraCol5 = data.IsExtraCol5;
                    GetData.IsExtraCol6 = data.IsExtraCol6;
                    GetData.IsExtraCol7 = data.IsExtraCol7;
                    GetData.IsExtraCol8 = data.IsExtraCol8;
                    GetData.IsExtraCol9 = data.IsExtraCol9;
                    GetData.IsExtraCol10 = data.IsExtraCol10;
                    GetData.IsExtraCol11 = data.IsExtraCol11;
                    GetData.IsExtraCol12 = data.IsExtraCol12;
                    GetData.IsExtraCol13 = data.IsExtraCol13;
                    GetData.IsExtraCol14 = data.IsExtraCol14;
                    GetData.IsExtraCol15 = data.IsExtraCol15;
                    GetData.IsExtraCol16 = data.IsExtraCol16;
                    GetData.IsExtraCol17 = data.IsExtraCol17;
                    GetData.IsExtraCol18 = data.IsExtraCol18;
                    GetData.IsExtraCol19 = data.IsExtraCol19;
                    GetData.IsExtraCol20 = data.IsExtraCol20;
                    GetData.IsExtraCol1dropdown = data.IsExtraCol1dropdown;
                    GetData.IsExtraCol2dropdown = data.IsExtraCol2dropdown;
                    GetData.IsExtraCol3dropdown = data.IsExtraCol3dropdown;
                    GetData.IsExtraCol4dropdown = data.IsExtraCol4dropdown;
                    GetData.IsExtraCol5dropdown = data.IsExtraCol5dropdown;
                    GetData.IsDOB = data.IsDOB;
                    GetData.IsMrgAnnivarsary = data.IsMrgAnnivarsary;
                    GetData.IsDesignation = data.IsDesignation;
                    GetData.IsExtraCol9Filter = data.IsExtraCol9Filter;
                    GetData.IsExtraCol10Filter = data.IsExtraCol10Filter;
                    GetData.IsTermFilter = data.IsTermFilter;
                    GetData.IsProductTypeFilter = data.IsProductTypeFilter;
                    GetData.IsLeadSourceFilter = data.IsLeadSourceFilter;
                    GetData.IsCustomerNameFilter = data.IsCustomerNameFilter;
                    GetData.IsMobNoFilter = data.IsMobNoFilter;
                    GetData.IsEmailFilter = data.IsEmailFilter;
                    GetData.IsDesigFilter = data.IsDesigFilter;
                    GetData.IsOrgNameFilter = data.IsOrgNameFilter;
                    GetData.IsSkypIdFilter = data.IsSkypIdFilter;
                    GetData.IsFollowUpTime = data.IsFollowUpTime;
                    GetData.IsFollowUpTimeIST = data.IsFollowUpTimeIST;
                    GetData.IsExtraCol1dropdownFilter = data.IsExtraCol1dropdownFilter;
                    GetData.IsExtraCol2dropdownFilter = data.IsExtraCol2dropdownFilter;
                    GetData.IsExtraCol3dropdownFilter = data.IsExtraCol3dropdownFilter;
                    GetData.IsExtraCol4dropdownFilter = data.IsExtraCol4dropdownFilter;
                    GetData.IsExtraCol5dropdownFilter = data.IsExtraCol5dropdownFilter;
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
                    GetFormData2.CreatedBy = UID;
                    GetFormData2.Createddate = Constant.GetBharatTime();
                    GetFormData2.CompanyID = CompanyID;
                    GetFormData2.BranchID = BranchID;
                    GetFormData2.Status = true;
                    GetFormData2.DateofBirthTextName = "Date Of Birth";
                    GetFormData2.MarriageAnniversaryTextName = "Marriage Anniversary";
                    GetFormData2.ExpectedDateTextName = "Expected Closing Date";
                    GetFormData2.ExpectedProductAmountTextName = "Expected Deal Amount";
                    //GetFormData2.ExtraCol1TextName = null;
                    //GetFormData2.ExtraCol2TextName = null;
                    //GetFormData2.ExtraCol3TextName = null;
                    //GetFormData2.ExtraCol4TextName = null;
                    //GetFormData2.ExtraCol5TextName = null;
                    //GetFormData2.ExtraCol6TextName = null;
                    //GetFormData2.ExtraCol7TextName = null;
                    //GetFormData2.ExtraCol8TextName = null;
                    //GetFormData2.ExtraCol9TextName = null;
                    //GetFormData2.ExtraCol10TextName = null;
                    //GetFormData2.ExtraCol11TextName = null;
                    //GetFormData2.ExtraCol12TextName = null;
                    //GetFormData2.ExtraCol13TextName = null;
                    //GetFormData2.ExtraCol14TextName = null;
                    //GetFormData2.ExtraCol15TextName = null;
                    //GetFormData2.ExtraCol16TextName = null;
                    //GetFormData2.ExtraCol17TextName = null;
                    //GetFormData2.ExtraCol18TextName = null;
                    //GetFormData2.ExtraCol19TextName = null;
                    //GetFormData2.ExtraCol20TextName = null;

                    //GetFormData2.HeaderMenuTextName = model.HeaderMenuTextName;
                    db.crm_customizedformfieldtextname.Add(GetFormData2);
                    db.SaveChanges();

                    GetFormData = GetFormData2;
                }


                GetData.AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.AddressTextNameTextName : string.Empty;
                GetData.IsAddress = string.IsNullOrEmpty(GetData.AddressTextNameTextName) ? false : GetData.IsAddress;

                GetData.CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                if (data != null && data.IsCustomerNameFilter)
                {
                    sb.AppendFormat("{0},", GetData.CustomerTextName);
                }
                GetData.MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                if (data != null && data.IsMobNoFilter)
                {
                    sb.AppendFormat("{0},", GetData.MobileNoTextName);
                }
                GetData.EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Email";
                if (data != null && data.IsDesigFilter)
                {
                    sb.AppendFormat("{0},", GetData.EmailIdTextName);
                }
                GetData.DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : string.Empty;
                GetData.IsDOB = string.IsNullOrEmpty(GetData.DateofBirthTextName) ? false : GetData.IsDOB;

                //model.DescriptionTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";
                GetData.DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : string.Empty;
                GetData.IsDesignation = string.IsNullOrEmpty(GetData.DesignationTextName) ? false : GetData.IsDesignation;

                if (data != null && data.IsDesigFilter && !string.IsNullOrEmpty(GetData.DesignationTextName))
                {
                    sb.AppendFormat("{0},", GetData.DesignationTextName);
                }

                GetData.ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : string.Empty;
                GetData.IsExpClosingDate = string.IsNullOrEmpty(GetData.ExpectedDateTextName) ? false : GetData.IsExpClosingDate;

                GetData.ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : string.Empty;
                GetData.IsExpDealAmount = string.IsNullOrEmpty(GetData.ExpectedProductAmountTextName) ? false : GetData.IsExpDealAmount;

                GetData.LeadOwnerTextName = /*GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName :*/ "Created By";
                GetData.LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                GetData.LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                GetData.FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                GetData.FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                GetData.FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";

                GetData.CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";

                GetData.StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";

                GetData.CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City";

                GetData.MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : string.Empty;
                GetData.IsMrgAnnivarsary = string.IsNullOrEmpty(GetData.MarriageAnniversaryTextName) ? false : GetData.IsMrgAnnivarsary;

                GetData.OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : string.Empty;
                GetData.IsOrganization = string.IsNullOrEmpty(GetData.OrganizationNameTextName) ? false : GetData.IsOrganization;

                if (data != null && data.IsOrgNameFilter && !string.IsNullOrEmpty(GetData.OrganizationNameTextName))
                {
                    sb.AppendFormat("{0},", GetData.OrganizationNameTextName);
                }
                GetData.ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                GetData.SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : string.Empty;
                GetData.IsSkypeId = string.IsNullOrEmpty(GetData.SkypeIdTextName) ? false : GetData.IsSkypeId;

                if (data != null && data.IsSkypIdFilter && !string.IsNullOrEmpty(GetData.SkypeIdTextName))
                {
                    sb.AppendFormat("{0},", GetData.SkypeIdTextName);
                }
                GetData.UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : string.Empty;
                GetData.IsUrl = string.IsNullOrEmpty(GetData.UrlTextName) ? false : GetData.IsUrl;

                GetData.ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";

                GetData.ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol1TextName))
                {
                    GetData.IsExtraCol1 = false;
                }

                GetData.ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol2TextName))
                {
                    GetData.IsExtraCol2 = false;
                }

                GetData.ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol3TextName))
                {
                    GetData.IsExtraCol3 = false;
                }

                GetData.ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol4TextName))
                {
                    GetData.IsExtraCol4 = false;
                }

                GetData.ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol5TextName))
                {
                    GetData.IsExtraCol5 = false;
                }

                GetData.ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol6TextName))
                {
                    GetData.IsExtraCol6 = false;
                }

                GetData.ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol7TextName))
                {
                    GetData.IsExtraCol7 = false;
                }

                GetData.ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol8TextName))
                {
                    GetData.IsExtraCol8 = false;
                }

                GetData.ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol9TextName))
                {
                    GetData.IsExtraCol9 = false;
                }

                GetData.ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol10TextName))
                {
                    GetData.IsExtraCol10 = false;
                }

                GetData.ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol11TextName))
                {
                    GetData.IsExtraCol11 = false;
                }

                GetData.ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol12TextName))
                {
                    GetData.IsExtraCol12 = false;
                }

                GetData.ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol13TextName))
                {
                    GetData.IsExtraCol13 = false;
                }

                GetData.ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol14TextName))
                {
                    GetData.IsExtraCol14 = false;
                }

                GetData.ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol15TextName))
                {
                    GetData.IsExtraCol15 = false;
                }
                GetData.ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol16TextName))
                {
                    GetData.IsExtraCol16 = false;
                }

                GetData.ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol17TextName))
                {
                    GetData.IsExtraCol17 = false;
                }

                GetData.ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol18TextName))
                {
                    GetData.IsExtraCol18 = false;
                }

                GetData.ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol19TextName))
                {
                    GetData.IsExtraCol19 = false;
                }
                GetData.ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                if (string.IsNullOrEmpty(GetData.ExtraCol20TextName))
                {
                    GetData.IsExtraCol20 = false;
                }
                GetData.ExtraCol1dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1dropdown) ? GetFormData.ExtraCol1dropdown : string.Empty;
                //GetData.ExtraCol1Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol1dropdown))
                {
                    GetData.IsExtraCol1dropdown = false;
                    //GetData.IsExtraCol1dropdownMandatory = false;
                }

                GetData.ExtraCol2dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2dropdown) ? GetFormData.ExtraCol2dropdown : string.Empty;
                //GetData.ExtraCol2Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol2dropdown))
                {
                    GetData.IsExtraCol2dropdown = false;
                    // GetData.IsExtraCol2dropdownMandatory = false;
                }

                GetData.ExtraCol3dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3dropdown) ? GetFormData.ExtraCol3dropdown : string.Empty;
                //GetData.ExtraCol3Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol3dropdown))
                {
                    GetData.IsExtraCol3dropdown = false;
                    //GetData.IsExtraCol3dropdownMandatory = false;
                }

                GetData.ExtraCol4dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4dropdown) ? GetFormData.ExtraCol4dropdown : string.Empty;
                // GetData.ExtraCol4Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol4dropdown))
                {
                    GetData.IsExtraCol4dropdown = false;
                    //GetData.IsExtraCol4dropdownMandatory = false;
                }

                GetData.ExtraCol5dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5dropdown) ? GetFormData.ExtraCol5dropdown : string.Empty;
                //GetData.ExtraCol5Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol5dropdown))
                {
                    GetData.IsExtraCol5dropdown = false;
                    //GetData.IsExtraCol5dropdownMandatory = false;
                }
                placeHolder = sb.ToString().TrimEnd(',');
                if (!string.IsNullOrEmpty(placeHolder))
                {
                    GetData.FilterPlaceHolder = placeHolder;
                }
            }

            return GetData;
        }

        public CreateLeadSettingDTO GetCreateLeadsetting()
        {
            niscrmEntities db = new niscrmEntities();

            //get user branchId and companyId by using session
            int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);

            var GetData = new CreateLeadSettingDTO();
            var fieldPriorityList = new List<LeadFieldPriorityDTO>();
            if (BranchID > 0 && CompanyID > 0)
            {
                var data = db.crm_createleadsetting.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                var GetSeqData = db.crm_create_lead_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

                #region save data if first time user enter here
                if (data == null)
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
                    GetData1.IsExtraCol1dropdown = false;
                    GetData1.IsExtraCol2dropdown = false;
                    GetData1.IsExtraCol3dropdown = false;
                    GetData1.IsExtraCol4dropdown = false;
                    GetData1.IsExtraCol5dropdown = false;
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
                    GetData1.IsExtraCol1dropdownMandatory = false;
                    GetData1.IsExtraCol2dropdownMandatory = false;
                    GetData1.IsExtraCol3dropdownMandatory = false;
                    GetData1.IsExtraCol4dropdownMandatory = false;
                    GetData1.IsExtraCol5dropdownMandatory = false;
                    db.crm_createleadsetting.Add(GetData1);
                    db.SaveChanges();

                    GetData = Mapper.Map<CreateLeadSettingDTO>(GetData1);
                }
                else
                {
                    GetData = Mapper.Map<CreateLeadSettingDTO>(data);
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
                    GetFormData2.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UID"]);
                    GetFormData2.Createddate = Constant.GetBharatTime();
                    GetFormData2.ModifiedDate = Constant.GetBharatTime();
                    GetFormData2.CompanyID = CompanyID;
                    GetFormData2.BranchID = BranchID;
                    GetFormData2.Status = true;
                    GetFormData2.DateofBirthTextName = "Date Of Birth";
                    GetFormData2.MarriageAnniversaryTextName = "Marriage Anniversary";
                    GetFormData2.ExpectedDateTextName = "Expected Closing Date";
                    GetFormData2.ExpectedProductAmountTextName = "Expected Deal Amount";
                    //GetFormData2.ExtraCol1TextName = null;
                    //GetFormData2.ExtraCol2TextName = null;
                    //GetFormData2.ExtraCol3TextName = null;
                    //GetFormData2.ExtraCol4TextName = null;
                    //GetFormData2.ExtraCol5TextName = null;
                    //GetFormData2.ExtraCol6TextName = null;
                    //GetFormData2.ExtraCol7TextName = null;
                    //GetFormData2.ExtraCol8TextName = null;
                    //GetFormData2.ExtraCol9TextName = null;
                    //GetFormData2.ExtraCol10TextName = null;
                    //GetFormData2.ExtraCol11TextName = null;
                    //GetFormData2.ExtraCol12TextName = null;
                    //GetFormData2.ExtraCol13TextName = null;
                    //GetFormData2.ExtraCol14TextName = null;
                    //GetFormData2.ExtraCol15TextName = null;
                    //GetFormData2.ExtraCol16TextName = null;
                    //GetFormData2.ExtraCol17TextName = null;
                    //GetFormData2.ExtraCol18TextName = null;
                    //GetFormData2.ExtraCol19TextName = null;
                    //GetFormData2.ExtraCol20TextName = null;

                    //GetFormData2.HeaderMenuTextName = model.HeaderMenuTextName;
                    db.crm_customizedformfieldtextname.Add(GetFormData2);
                    db.SaveChanges();

                    GetFormData = GetFormData2;
                }
                #endregion



                //if (data != null)
                //{
                //    GetData = Mapper.Map<CreateLeadSettingDTO>(data);
                //}
                //else
                //{
                //    GetData.IsFollowUpTime = true;                   
                //}


                #region add field priority
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
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 45, FieldName = "ExtraCol1dropdown" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 46, FieldName = "ExtraCol2dropdown" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 47, FieldName = "ExtraCol3dropdown" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 48, FieldName = "ExtraCol4dropdown" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 49, FieldName = "ExtraCol5dropdown" });

                    var uid = Convert.ToInt32(HttpContext.Current.Session["UID"]);
                    foreach (var item in fieldPriorityList)
                    {
                        var fp = new crm_create_lead_field_sequence
                        {
                            Priority = item.Priority,
                            FieldName = item.FieldName,
                            CompanyID = CompanyID,
                            BranchID = BranchID,
                            CreatedBy = uid,
                            Createddate = Constant.GetBharatTime(),
                            ModifiedDate = Constant.GetBharatTime()
                        };
                        db.crm_create_lead_field_sequence.Add(fp);
                    }
                    db.SaveChanges();
                }
                #endregion

                #region fields names list with  show on create lead page

                GetData.LeadOwnerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadOwnerTextName) ? GetFormData.LeadOwnerTextName : "Lead Owner";
                GetData.LeadOwnerPriority = fieldPriorityList.Where(a => a.FieldName == "LeadOwnerTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.LeadStatusTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadStatusTextName) ? GetFormData.LeadStatusTextName : "Lead Status";
                GetData.LeadStatusPriority = fieldPriorityList.Where(a => a.FieldName == "LeadStatusTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.CustomerTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CustomerTextName) ? GetFormData.CustomerTextName : "Customer Name";
                GetData.CustomerPriority = fieldPriorityList.Where(a => a.FieldName == "CustomerTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.MobileNoTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MobileNoTextName) ? GetFormData.MobileNoTextName : "Mobile Number";
                GetData.MobPriority = fieldPriorityList.Where(a => a.FieldName == "MobileNoTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.FollowDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowDateTextName) ? GetFormData.FollowDateTextName : "Follow Up Date";
                GetData.FollowUpDatePriority = fieldPriorityList.Where(a => a.FieldName == "FollowDateTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.FollowUpTimeTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowUpTimeTextName) ? GetFormData.FollowUpTimeTextName : "Follow Up Time";
                GetData.FollowUpTimePriority = fieldPriorityList.Where(a => a.FieldName == "FollowUpTimeTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.FollowupTimeinISTTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.FollowupTimeinISTTextName) ? GetFormData.FollowupTimeinISTTextName : "Follow Up Time in IST";
                GetData.FollowUpTimeISTPriority = fieldPriorityList.Where(a => a.FieldName == "FollowupTimeinISTTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.ZoneNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ZoneNameTextName) ? GetFormData.ZoneNameTextName : "Time Zone Name";
                GetData.ZoneNamePriority = fieldPriorityList.Where(a => a.FieldName == "ZoneNameTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.DateofBirthTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DateofBirthTextName) ? GetFormData.DateofBirthTextName : string.Empty;
                GetData.DOBPriority = fieldPriorityList.Where(a => a.FieldName == "DateofBirthTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.DateofBirthTextName))
                {
                    GetData.IsDateofBirth = false;
                    GetData.IsDOBMandatory = false;
                }

                GetData.MarriageAnniversaryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.MarriageAnniversaryTextName) ? GetFormData.MarriageAnniversaryTextName : string.Empty;
                GetData.MerriageAnnivPriority = fieldPriorityList.Where(a => a.FieldName == "MarriageAnniversaryTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.MarriageAnniversaryTextName))
                {
                    GetData.IsMarriageAnniversary = false;
                    GetData.IsMrgAniMandatory = false;
                }

                GetData.DesignationTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DesignationTextName) ? GetFormData.DesignationTextName : string.Empty;
                GetData.DesigPriority = fieldPriorityList.Where(a => a.FieldName == "DesignationTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.DesignationTextName))
                {
                    GetData.IsDesignation = false;
                    GetData.IsDesigMandatory = false;
                }

                GetData.EmailIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIdTextName) ? GetFormData.EmailIdTextName : "Customer e-mail";
                GetData.EmailPriority = fieldPriorityList.Where(a => a.FieldName == "EmailIdTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.AddressTextNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.AddressTextNameTextName) ? GetFormData.AddressTextNameTextName : string.Empty;
                GetData.AddressPriority = fieldPriorityList.Where(a => a.FieldName == "AddressTextNameTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.AddressTextNameTextName))
                {
                    GetData.IsAddress = false;
                    GetData.IsAddressMandatory = false;
                }
                GetData.CountryTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CountryTextName) ? GetFormData.CountryTextName : "Country";
                GetData.CountryPriority = fieldPriorityList.Where(a => a.FieldName == "CountryTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.StateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StateTextName) ? GetFormData.StateTextName : "State";
                GetData.StatePriority = fieldPriorityList.Where(a => a.FieldName == "StateTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.CityTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.CityTextName) ? GetFormData.CityTextName : "City/Location";
                GetData.CityPriority = fieldPriorityList.Where(a => a.FieldName == "CityTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.UrlTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrlTextName) ? GetFormData.UrlTextName : string.Empty;
                GetData.UrlPriority = fieldPriorityList.Where(a => a.FieldName == "UrlTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.UrlTextName))
                {
                    GetData.IsUrl = false;
                    GetData.IsUrlMandatory = false;
                }

                GetData.SkypeIdTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.SkypeIdTextName) ? GetFormData.SkypeIdTextName : string.Empty;
                GetData.SkypeIdPriority = fieldPriorityList.Where(a => a.FieldName == "SkypeIdTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.SkypeIdTextName))
                {
                    GetData.IsSkypeIdMandatory = false;
                    GetData.IsSkypeId = false;
                }

                GetData.LeadResourceTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.LeadResourceTextName) ? GetFormData.LeadResourceTextName : "Lead Source";
                GetData.LeadSourcePriority = fieldPriorityList.Where(a => a.FieldName == "LeadResourceTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.ProductTypeNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeNameTextName) ? GetFormData.ProductTypeNameTextName : "Product Type";
                GetData.ProdTypePriority = fieldPriorityList.Where(a => a.FieldName == "ProductTypeNameTextName").Select(a => a.Priority).FirstOrDefault();

                GetData.ExpectedDateTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedDateTextName) ? GetFormData.ExpectedDateTextName : string.Empty;
                GetData.ExpDatePriority = fieldPriorityList.Where(a => a.FieldName == "ExpectedDateTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExpectedDateTextName))
                {
                    GetData.IsExpectedClosingDate = false;
                    GetData.IsExpClsDateMandatory = false;
                }

                GetData.ExpectedProductAmountTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExpectedProductAmountTextName) ? GetFormData.ExpectedProductAmountTextName : string.Empty;
                GetData.ExpAmountPriority = fieldPriorityList.Where(a => a.FieldName == "ExpectedProductAmountTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExpectedProductAmountTextName))
                {
                    GetData.IsExpectedDealAmount = false;
                    GetData.IsExpDealAmtMandatory = false;
                }

                GetData.DescriptionTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.DescriptionTextName) ? GetFormData.DescriptionTextName : "Description";

                GetData.OrganizationNameTextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.OrganizationNameTextName) ? GetFormData.OrganizationNameTextName : "Organization Name";
                GetData.OrgNamePriority = fieldPriorityList.Where(a => a.FieldName == "OrganizationNameTextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.OrganizationNameTextName))
                {
                    GetData.IsOrganizationName = false;
                    GetData.IsOrgNameMandatory = false;
                }

                GetData.ExtraCol1TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1TextName) ? GetFormData.ExtraCol1TextName : string.Empty;
                GetData.ExtraCol1Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol1TextName))
                {
                    GetData.IsExtraCol1 = false;
                    GetData.IsExtraCol1Mandatory = false;
                }

                GetData.ExtraCol2TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2TextName) ? GetFormData.ExtraCol2TextName : string.Empty;
                GetData.ExtraCol2Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol2TextName))
                {
                    GetData.IsExtraCol2 = false;
                    GetData.IsExtraCol2Mandatory = false;
                }

                GetData.ExtraCol3TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3TextName) ? GetFormData.ExtraCol3TextName : string.Empty;
                GetData.ExtraCol3Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol3TextName))
                {
                    GetData.IsExtraCol3 = false;
                    GetData.IsExtraCol3Mandatory = false;
                }

                GetData.ExtraCol4TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4TextName) ? GetFormData.ExtraCol4TextName : string.Empty;
                GetData.ExtraCol4Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol4TextName))
                {
                    GetData.IsExtraCol4 = false;
                    GetData.IsExtraCol4Mandatory = false;
                }

                GetData.ExtraCol5TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5TextName) ? GetFormData.ExtraCol5TextName : string.Empty;
                GetData.ExtraCol5Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol5TextName))
                {
                    GetData.IsExtraCol5 = false;
                    GetData.IsExtraCol5Mandatory = false;
                }

                GetData.ExtraCol6TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6TextName) ? GetFormData.ExtraCol6TextName : string.Empty;
                GetData.ExtraCol6Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol6TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol6TextName))
                {
                    GetData.IsExtraCol6 = false;
                    GetData.IsExtraCol6Mandatory = false;
                }

                GetData.ExtraCol7TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7TextName) ? GetFormData.ExtraCol7TextName : string.Empty;
                GetData.ExtraCol7Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol7TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol7TextName))
                {
                    GetData.IsExtraCol7 = false;
                    GetData.IsExtraCol7Mandatory = false;
                }

                GetData.ExtraCol8TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8TextName) ? GetFormData.ExtraCol8TextName : string.Empty;
                GetData.ExtraCol8Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol8TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol8TextName))
                {
                    GetData.IsExtraCol8 = false;
                    GetData.IsExtraCol8Mandatory = false;
                }

                GetData.ExtraCol9TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : string.Empty;
                GetData.ExtraCol9Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol9TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol9TextName))
                {
                    GetData.IsExtraCol9 = false;
                    GetData.IsExtraCol9Mandatory = false;
                }

                GetData.ExtraCol10TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : string.Empty;
                GetData.ExtraCol10Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol10TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol10TextName))
                {
                    GetData.IsExtraCol10 = false;
                    GetData.IsExtraCol10Mandatory = false;
                }
                GetData.ExtraCol11TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11TextName) ? GetFormData.ExtraCol11TextName : string.Empty;
                GetData.ExtraCol11Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol11TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol11TextName))
                {
                    GetData.IsExtraCol11 = false;
                    GetData.IsExtraCol11Mandatory = false;
                }

                GetData.ExtraCol12TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12TextName) ? GetFormData.ExtraCol12TextName : string.Empty;
                GetData.ExtraCol12Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol12TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol12TextName))
                {
                    GetData.IsExtraCol12 = false;
                    GetData.IsExtraCol12Mandatory = false;
                }

                GetData.ExtraCol13TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol13TextName) ? GetFormData.ExtraCol13TextName : string.Empty;
                GetData.ExtraCol13Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol13TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol13TextName))
                {
                    GetData.IsExtraCol13 = false;
                    GetData.IsExtraCol13Mandatory = false;
                }

                GetData.ExtraCol14TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol14TextName) ? GetFormData.ExtraCol14TextName : string.Empty;
                GetData.ExtraCol14Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol14TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol14TextName))
                {
                    GetData.IsExtraCol14 = false;
                    GetData.IsExtraCol14Mandatory = false;
                }

                GetData.ExtraCol15TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol15TextName) ? GetFormData.ExtraCol15TextName : string.Empty;
                GetData.ExtraCol15Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol15TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol15TextName))
                {
                    GetData.IsExtraCol15 = false;
                    GetData.IsExtraCol15Mandatory = false;
                }

                GetData.ExtraCol16TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol16TextName) ? GetFormData.ExtraCol16TextName : string.Empty;
                GetData.ExtraCol16Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol16TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol16TextName))
                {
                    GetData.IsExtraCol16 = false;
                    GetData.IsExtraCol16Mandatory = false;
                }

                GetData.ExtraCol17TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol17TextName) ? GetFormData.ExtraCol17TextName : string.Empty;
                GetData.ExtraCol17Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol17TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol17TextName))
                {
                    GetData.IsExtraCol17 = false;
                    GetData.IsExtraCol17Mandatory = false;
                }

                GetData.ExtraCol18TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : string.Empty;
                GetData.ExtraCol18Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol18TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol18TextName))
                {
                    GetData.IsExtraCol18 = false;
                    GetData.IsExtraCol18Mandatory = false;
                }

                GetData.ExtraCol19TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : string.Empty;
                GetData.ExtraCol19Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol19TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol19TextName))
                {
                    GetData.IsExtraCol19 = false;
                    GetData.IsExtraCol19Mandatory = false;
                }

                GetData.ExtraCol20TextName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : string.Empty;
                GetData.ExtraCol20Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol20TextName").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol20TextName))
                {
                    GetData.IsExtraCol20 = false;
                    GetData.IsExtraCol20Mandatory = false;
                }
                GetData.ExtraCol1dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1dropdown) ? GetFormData.ExtraCol1dropdown : string.Empty;
                GetData.ExtraCol1Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol1dropdown))
                {
                    GetData.IsExtraCol1dropdown = false;
                    GetData.IsExtraCol1dropdownMandatory = false;
                }

                GetData.ExtraCol2dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2dropdown) ? GetFormData.ExtraCol2dropdown : string.Empty;
                GetData.ExtraCol2Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol2dropdown))
                {
                    GetData.IsExtraCol2dropdown = false;
                    GetData.IsExtraCol2dropdownMandatory = false;
                }

                GetData.ExtraCol3dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3dropdown) ? GetFormData.ExtraCol3dropdown : string.Empty;
                GetData.ExtraCol3Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol3dropdown))
                {
                    GetData.IsExtraCol3dropdown = false;
                    GetData.IsExtraCol3dropdownMandatory = false;
                }

                GetData.ExtraCol4dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4dropdown) ? GetFormData.ExtraCol4dropdown : string.Empty;
                GetData.ExtraCol4Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol4dropdown))
                {
                    GetData.IsExtraCol4dropdown = false;
                    GetData.IsExtraCol4dropdownMandatory = false;
                }

                GetData.ExtraCol5dropdown = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5dropdown) ? GetFormData.ExtraCol5dropdown : string.Empty;
                GetData.ExtraCol5Prioritydropdown = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5dropdown").Select(a => a.Priority).FirstOrDefault();
                if (string.IsNullOrEmpty(GetData.ExtraCol5dropdown))
                {
                    GetData.IsExtraCol5dropdown = false;
                    GetData.IsExtraCol5dropdownMandatory = false;
                }
                #endregion

            }
            return GetData;
        }

        public UserCompanyBranchId GetUserCompanyBranch(int userId)
        {
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetUserDetailById(" + userId + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new UserCompanyBranchId
                           {
                               Id = Convert.ToInt32(dr["Id"]),
                               Fname = Convert.ToString(dr["Fname"]),
                               Lname = Convert.ToString(dr["Lname"]),
                               BranchID = Convert.ToInt32(dr["BranchID"]),
                               CompanyID = Convert.ToInt32(dr["CompanyID"]),
                           }).FirstOrDefault();
            return GetData;
        }
        #region UserProfilePicture
        public CommonActivityRemarkModel GetUserprofile()
        {
            int UID = HttpContext.Current.Session["UID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["UID"]);
            int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            var userdata = new CommonActivityRemarkModel();
            if (BranchID > 0 && CompanyID > 0 && UID > 0)
            {
                DataTable getuser = DataAccessLayer.GetDataTable("call Crm_UserProfileData('" + BranchID + "','" + CompanyID + "','" + UID + "')");
                if (getuser.Rows.Count > 0)
                {

                    userdata = (from dr in getuser.AsEnumerable()
                                select new CommonActivityRemarkModel
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    UserName = Convert.ToString(dr["UserName"]),
                                    UID = Convert.ToInt32(dr["UserId"]),
                                    Profile = Convert.ToString(dr["ProfileName"]),
                                    UserPicture = Convert.ToString(dr["ProfilePath"]),
                                }).FirstOrDefault();
                }
            }
            return userdata;
        }
        #endregion
        public ViewTecketSettingDTO GetViewTicketSetting()
        {
            niscrmEntities db = new niscrmEntities();

            //get user branchId and companyId by using session
            int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);

            var GetData = new ViewTecketSettingDTO();

            if (BranchID > 0 && CompanyID > 0)
            {
                var data = db.crm_ticketviewsetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                if (data != null)
                {
                    GetData = Mapper.Map<ViewTecketSettingDTO>(data);
                }

                #region fields names list for show row wise

                GetData.NameText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";

                GetData.EmailIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";

                GetData.PhoneNumberText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";

                GetData.ProductTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";

                GetData.ErrorTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";

                GetData.UrgencyIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";

                GetData.StatusIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";

                GetData.subjectText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";

                GetData.ExtraCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text : string.Empty;
                GetData.IsExtraCol1 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? data.IsExtraCol1 : false;

                GetData.ExtraCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : string.Empty;
                GetData.IsExtraCol2 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? data.IsExtraCol2 : false;

                GetData.ExtraCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : string.Empty;
                GetData.IsExtraCol3 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? data.IsExtraCol3 : false;

                GetData.ExtraCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : string.Empty;
                GetData.IsExtraCol4 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? data.IsExtraCol4 : false;

                GetData.ExtraCol5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text : string.Empty;
                GetData.IsExtraCol5 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? data.IsExtraCol5 : false;

                GetData.ExtraCol6Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : string.Empty;
                GetData.ISExtraCol6 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? data.ISExtraCol6 : false;

                GetData.ExtraCol7Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : string.Empty;
                GetData.ISExtraCol7 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? data.ISExtraCol7 : false;

                GetData.ExtraCol8Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : string.Empty;
                GetData.ISExtraCol8 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? data.ISExtraCol8 : false;

                GetData.ExtraCol9Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : string.Empty;
                GetData.IsExtraCol9 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? data.IsExtraCol9 : false;

                GetData.ExtraCol10Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : string.Empty;
                GetData.IsExtraCol10 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? data.IsExtraCol10 : false;

                GetData.ExtraCol11Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : string.Empty;
                GetData.IsExtraCol11 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? data.IsExtraCol11 : false;

                GetData.ExtraCol12Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : string.Empty;
                GetData.IsExtraCol12 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? data.IsExtraCol12 : false;

                GetData.ExtraColdropdown1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text) ? GetFormData.ExtraColdropdown1Text : string.Empty;
                GetData.IsExtraColdropdown1 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text) ? data.IsExtracoldropdown1 : false;

                GetData.ExtraColdropdown2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text) ? GetFormData.ExtraColdropdown2Text : string.Empty;
                GetData.IsExtraColdropdown2 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text) ? data.IsExtracoldropdown2 : false;

                GetData.ExtraColdropdown3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text) ? GetFormData.ExtraColdropdown3Text : string.Empty;
                GetData.IsExtraColdropdown3 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text) ? data.IsExtracoldropdown3 : false;

                GetData.ExtraColdropdown4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text) ? GetFormData.ExtraColdropdown4Text : string.Empty;
                GetData.IsExtraColdropdown4 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text) ? data.IsExtracoldropdown4 : false;

                GetData.ExtraColdropdown5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text) ? GetFormData.ExtraColdropdown5Text : string.Empty;
                GetData.IsExtraColdropdown5 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text) ? data.IsExtracoldropdown5 : false;
                //GetData.ImageCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol1Text) ? GetFormData.ImageCol1Text : string.Empty;
                //GetData.IsExtraCol1 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? data.IsExtraCol1 : false;

                //GetData.ImageCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol2Text) ? GetFormData.ImageCol2Text : string.Empty;

                //GetData.ImageCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol3Text) ? GetFormData.ImageCol3Text : string.Empty;

                //GetData.ImageCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol4Text) ? GetFormData.ImageCol4Text : string.Empty;

                #endregion

                //if (GetFormData != null)
                //{
                //    GetData.NameText = GetFormData.NameText;
                //    GetData.EmailIDText = GetFormData.EmailIDText;
                //    GetData.PhoneNumberText = GetFormData.PhoneNumberText;
                //    GetData.ErrorTypeIDText = GetFormData.ErrorTypeIDText;
                //    GetData.ProductTypeIDText = GetFormData.ProductTypeIDText;
                //    GetData.UrgencyIDText = GetFormData.UrgencyIDText;
                //    GetData.subjectText = GetFormData.subjectText;
                //    GetData.StatusIDText = GetFormData.StatusIDText;
                //    GetData.CustomerIDText = GetFormData.CustomerIDText;
                //    GetData.ExtraCol1Text = GetFormData.ExtraCol1Text;
                //    GetData.ExtraCol2Text = GetFormData.ExtraCol2Text;
                //    GetData.ExtraCol3Text = GetFormData.ExtraCol3Text;
                //    GetData.ExtraCol4Text = GetFormData.ExtraCol4Text;
                //    GetData.ExtraCol5Text = GetFormData.ExtraCol5Text;
                //    GetData.ExtraCol6Text = GetFormData.ExtraCol6Text;
                //    GetData.ExtraCol7Text = GetFormData.ExtraCol7Text;
                //    GetData.ExtraCol8Text = GetFormData.ExtraCol8Text;
                //    GetData.ExtraCol9Text = GetFormData.ExtraCol9Text;
                //    GetData.ExtraCol10Text = GetFormData.ExtraCol10Text;
                //    //GetData.FormTextName = GetFormData.FormTextName;
                //    //GetData.LeadOwnerTextName = GetFormData.LeadOwnerTextName;
                //    GetData.ExtraCol11Text = GetFormData.ExtraCol11Text;
                //    GetData.ExtraCol12Text = GetFormData.ExtraCol12Text;
                //    GetData.ImageCol1Text = GetFormData.ImageCol1Text;
                //    GetData.ImageCol2Text = GetFormData.ImageCol2Text;
                //    GetData.ImageCol3Text = GetFormData.ImageCol3Text;
                //    GetData.ImageCol4Text = GetFormData.ImageCol4Text;

                //}


            }

            return GetData;
        }

        public CreateTicketSettingDTO GetCreateTicketSetting()
        {
            niscrmEntities db = new niscrmEntities();

            //get user branchId and companyId by using session
            int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);

            var GetData = new CreateTicketSettingDTO();
            var fieldPriorityList = new List<LeadFieldPriorityDTO>();
            if (BranchID > 0 && CompanyID > 0)
            {
                var data = db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                var GetSeqData = db.crm_ticket_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToList();

                #region add data if null
                if (data == null)
                {
                    crm_ticketcreatesetting GetData1 = new crm_ticketcreatesetting();

                    GetData1.IsName = true;
                    GetData1.IsPhoneNumber = true;
                    GetData1.IsProductTypeID = true;
                    GetData1.IsProductTypeIDRequired = true;
                    GetData1.IsErrorTypeID = true;
                    GetData1.IsErrorTypeIDRequired = true;
                    GetData1.IsUrgencyID = true;
                    GetData1.IsErrorTypeIDRequired = true;
                    GetData1.IsStatusID = true;
                    GetData1.IsStatusIDRequired = true;
                    GetData1.BranchID = BranchID;
                    GetData1.CompanyId = CompanyID;
                    GetData1.CreatedOn = Constant.GetBharatTime();
                    GetData1.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UID"]);
                    GetData1.IsEmailID = true;
                    GetData1.Issubject = true;
                    GetData1.IsEmailIDRequired = true;
                    GetData1.IssubjectRequired = true;
                    db.crm_ticketcreatesetting.Add(GetData1);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        GetData = Mapper.Map<CreateTicketSettingDTO>(GetData1);
                    }
                }
                else
                {
                    GetData = Mapper.Map<CreateTicketSettingDTO>(data);
                }

                if (GetFormData == null)
                {
                    crm_ticketfieldnamecustomized GetFormData2 = new crm_ticketfieldnamecustomized();
                    GetFormData2.NameText = "Customer Name";
                    GetFormData2.EmailIDText = "Email Address";
                    GetFormData2.PhoneNumberText = "Phone Number";
                    GetFormData2.ProductTypeIDText = "Product Type";
                    GetFormData2.ErrorTypeIDText = "Error Type";
                    GetFormData2.UrgencyIDText = "Urgency Type";
                    GetFormData2.StatusIDText = "Ticket Status";
                    GetFormData2.BranchID = BranchID;
                    GetFormData2.CompanyId = CompanyID;
                    GetFormData2.CreatedOn = Constant.GetBharatTime();
                    GetFormData2.CreatedBy = Convert.ToInt32(HttpContext.Current.Session["UID"]);

                    db.crm_ticketfieldnamecustomized.Add(GetFormData2);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        GetFormData = GetFormData2;
                    }

                }

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
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 1, FieldName = "NameText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 2, FieldName = "EmailIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 3, FieldName = "PhoneNumberText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 4, FieldName = "ProductTypeIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 5, FieldName = "ErrorTypeIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 6, FieldName = "UrgencyIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 7, FieldName = "StatusIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 8, FieldName = "subjectText" });

                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 9, FieldName = "ExtraCol1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 10, FieldName = "ExtraCol2Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 11, FieldName = "ExtraCol3Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 12, FieldName = "ExtraCol4Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 13, FieldName = "ExtraCol5Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 14, FieldName = "ExtraCol6Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 15, FieldName = "ExtraCol7Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 16, FieldName = "ExtraCol8Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 17, FieldName = "ExtraCol9Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 18, FieldName = "ExtraCol10Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 19, FieldName = "ExtraCol11Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 20, FieldName = "ExtraCol12Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 21, FieldName = "ExtraColdropdown1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 22, FieldName = "ExtraColdropdown1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 23, FieldName = "ExtraColdropdown1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 24, FieldName = "ExtraColdropdown1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 25, FieldName = "ExtraColdropdown1Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 21, FieldName = "ImageCol1Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 22, FieldName = "ImageCol2Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 23, FieldName = "ImageCol3Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 24, FieldName = "ImageCol4Text" });

                    var uid = Convert.ToInt32(HttpContext.Current.Session["UID"]);
                    foreach (var item in fieldPriorityList)
                    {
                        var fp = new crm_ticket_field_sequence
                        {
                            Priority = item.Priority,
                            FieldName = item.FieldName,
                            CompanyID = CompanyID,
                            BranchID = BranchID,
                            CreatedBy = uid,
                            Createddate = Constant.GetBharatTime(),
                            ModifiedDate = Constant.GetBharatTime()
                        };
                        db.crm_ticket_field_sequence.Add(fp);
                    }
                    db.SaveChanges();
                }

                #endregion

                #region fields names list for show dropdown
                GetData.NameTextPriority = fieldPriorityList.Where(a => a.FieldName == "NameText").Select(a => a.Priority).FirstOrDefault();
                GetData.NameText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";

                GetData.EmailIDTextPriority = fieldPriorityList.Where(a => a.FieldName == "EmailIDText").Select(a => a.Priority).FirstOrDefault();
                GetData.EmailIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";

                GetData.PhoneNumberTextPriority = fieldPriorityList.Where(a => a.FieldName == "PhoneNumberText").Select(a => a.Priority).FirstOrDefault();
                GetData.PhoneNumberText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";

                GetData.ProductTypeIDTextPriority = fieldPriorityList.Where(a => a.FieldName == "ProductTypeIDText").Select(a => a.Priority).FirstOrDefault();
                GetData.ProductTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";

                GetData.ErrorTypeIDTextPriority = fieldPriorityList.Where(a => a.FieldName == "ErrorTypeIDText").Select(a => a.Priority).FirstOrDefault();
                GetData.ErrorTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";

                GetData.UrgencyIDTextPriority = fieldPriorityList.Where(a => a.FieldName == "UrgencyIDText").Select(a => a.Priority).FirstOrDefault();
                GetData.UrgencyIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";

                GetData.StatusIDTextPriority = fieldPriorityList.Where(a => a.FieldName == "StatusIDText").Select(a => a.Priority).FirstOrDefault();
                GetData.StatusIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";

                GetData.subjectTextPriority = fieldPriorityList.Where(a => a.FieldName == "subjectText").Select(a => a.Priority).FirstOrDefault();
                GetData.subjectText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";

                GetData.ExtraCol1TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text : string.Empty;

                GetData.ExtraCol2TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : string.Empty;

                GetData.ExtraCol3TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : string.Empty;

                GetData.ExtraCol4TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : string.Empty;

                GetData.ExtraCol5TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text : string.Empty;

                GetData.ExtraCol6TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol6Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol6Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : string.Empty;

                GetData.ExtraCol7TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol7Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol7Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : string.Empty;

                GetData.ExtraCol8TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol8Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol8Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : string.Empty;

                GetData.ExtraCol9TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol9Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol9Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : string.Empty;

                GetData.ExtraCol10TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol10Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol10Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : string.Empty;

                GetData.ExtraCol11TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol11Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol11Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : string.Empty;

                GetData.ExtraCol12TextPriority = fieldPriorityList.Where(a => a.FieldName == "ExtraCol12Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraCol12Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : string.Empty;

                GetData.ExtraColdropdown1Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown1Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraColdropdown1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text) ? GetFormData.ExtraColdropdown1Text : string.Empty;

                GetData.ExtraColdropdown2Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown2Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraColdropdown2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text) ? GetFormData.ExtraColdropdown2Text : string.Empty;

                GetData.ExtraColdropdown3Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown3Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraColdropdown3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text) ? GetFormData.ExtraColdropdown3Text : string.Empty;

                GetData.ExtraColdropdown4Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown4Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraColdropdown4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text) ? GetFormData.ExtraColdropdown4Text : string.Empty;

                GetData.ExtraColdropdown5Priority = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown5Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ExtraColdropdown5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text) ? GetFormData.ExtraColdropdown5Text : string.Empty;
                //GetData.ImageCol1TextPriority = fieldPriorityList.Where(a => a.FieldName == "ImageCol1Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ImageCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol1Text) ? GetFormData.ImageCol1Text : string.Empty;

                //GetData.ImageCol2TextPriority = fieldPriorityList.Where(a => a.FieldName == "ImageCol2Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ImageCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol2Text) ? GetFormData.ImageCol2Text : string.Empty;

                //GetData.ImageCol3TextPriority = fieldPriorityList.Where(a => a.FieldName == "ImageCol3Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ImageCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol3Text) ? GetFormData.ImageCol3Text : string.Empty;

                //GetData.ImageCol4TextPriority = fieldPriorityList.Where(a => a.FieldName == "ImageCol4Text").Select(a => a.Priority).FirstOrDefault();
                GetData.ImageCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol4Text) ? GetFormData.ImageCol4Text : string.Empty;


                #endregion


            }
            return GetData;
        }

        public ResellerDetailDTO GetReseller(int companyId, string emailId)
        {
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetResellerDetailByEmail(" + companyId + ",'" + emailId + "')");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new ResellerDetailDTO
                           {
                               ResellerId = Convert.ToInt32(dr["id"]),
                               ResellerName = Convert.ToString(dr["user_name"]),
                               ResellerContactNo = Convert.ToString(dr["contact_no"]),
                               ResellerStatus = Convert.ToString(dr["status"]),
                               ResellerCode = !string.IsNullOrEmpty(Convert.ToString(dr["status"])) && Convert.ToString(dr["status"]) == "Active" ? Convert.ToString(dr["reseller_code"]) : "",

                           }).FirstOrDefault();
            return GetData;
        }

        //public  object AddIf(this RouteValueDictionary dict, bool condition, string name, object value)
        //{
        //    if (condition)
        //        dict.Add(name, value);
        //    return dict;
        //}

        public bool GetCompanyCountry()
        {
            niscrmEntities db = new niscrmEntities();
            bool isIndia = false;
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            if (db.company_profile.Any(a => a.ID == CompanyID && a.Country == "1"))
            {
                isIndia = true;
            }
            return isIndia;
        }
        public int GetCompanyCountryId(int cmpId = 0)
        {
            niscrmEntities db = new niscrmEntities();
            int countryId = 0;
            if (cmpId > 0)
            {

                string qury = @"select (Case when(Country is null) then 1 else Country end) as CountryId from company_profile where ID=" + cmpId + "";
                countryId = db.Database.SqlQuery<int>(qury).FirstOrDefault();
            }
            else
            {
                int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
                string qury = @"select (Case when(Country is null) then 1 else Country end) as CountryId from company_profile where ID=" + CompanyID + "";
                countryId = db.Database.SqlQuery<int>(qury).FirstOrDefault();
            }

            return countryId;
        }
        public DateAndFormat GetDateFormat()
        {
            var data = new DateAndFormat();
            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);

            string zoneId = "India Standard Time";//by default set india time zone
            string dformat = "dd/MM/yyyy";//by default set indian date format

            #region date code
            DataTable GetRecords = DataAccessLayer.GetDataTable("call CRM_GetTimeZoneByCompanyId(" + CompanyID + ")");
            var GetData = (from dr in GetRecords.AsEnumerable()
                           select new TimeZoneDTO
                           {
                               ZoneName = Convert.ToString(dr["TimeZoneName"])
                           }).FirstOrDefault();

            if (GetData != null)
            {
                zoneId = GetData.ZoneName;
            }

            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            DateTime date = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            #endregion

            #region date format code
            DataTable GetDateFormat = DataAccessLayer.GetDataTable("call CRM_GetDateFormatByCompanyId(" + CompanyID + ")");
            var GetDFormat = (from dr in GetDateFormat.AsEnumerable()
                              select new TimeZoneDTO
                              {
                                  DateFormat = Convert.ToString(dr["DateFormat"])
                              }).FirstOrDefault();
            if (GetDFormat != null)
            {
                dformat = GetDFormat.DateFormat;
            }
            #endregion
            data.DateFormat = dformat;
            data.Date = date;
            return data;
        }

        public string GetCompanyLanguage(int? CompId)
        {
            niscrmEntities db = new niscrmEntities();

            //get user branchId and companyId by using session
            //int BranchID = HttpContext.Current.Session["BranchID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["BranchID"]);
            int? CompanyID = CompId > 0 ? CompId : HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            string query = @"select lang from company_profile where id=" + CompanyID;
            var language = db.Database.SqlQuery<string>(query).FirstOrDefault();
            return language;
        }
        public string Getgoogleads()
        {
            niscrmEntities db = new niscrmEntities();

            int CompanyID = HttpContext.Current.Session["CompanyID"] == null ? 0 : Convert.ToInt32(HttpContext.Current.Session["CompanyID"]);
            string dataquery = @"SELECT BranchID from crm_usertbl where CompanyId=" + CompanyID + "";
            var dataquery1 = db.Database.SqlQuery<string>(dataquery).FirstOrDefault();
            string comid = "";
            string googleads = "";
            if (!string.IsNullOrEmpty(dataquery))
            {
                comid = dataquery1;

            }
            if (comid != null)
            {
                string query = @"SELECT PaymentStatus FROM t_login where company_id=" + comid + " and  role_name='Company'";
                var ads = db.Database.SqlQuery<string>(query).FirstOrDefault();
                if (!string.IsNullOrEmpty(ads))
                {
                    googleads = ads;
                }

            }
            return googleads;



        }
        public string GetCompanyNodaTimeZone(int companyId)
        {
            niscrmEntities db = new niscrmEntities();
            string tzName = "Asia/Kolkata";
            string query = string.Format("select timeZone from company_profile where ID={0}", companyId);
            var data = db.Database.SqlQuery<string>(query).FirstOrDefault();
            if (!string.IsNullOrEmpty(data))
            {
                tzName = data;
            }
            return tzName;
        }
        public static void GetStandardName()
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            var date1 = new DateTime(2015, 1, 15);
            var date2 = new DateTime(2015, 7, 15);

            Console.WriteLine(String.Format("{0,-62} {1,-32} {2,-32} {3,-15} {4,-20} {5,-20}", "Display Name", "Standard Name", "Daylight Name", "Supports DST", "UTC Standard Offset", "UTC Daylight Offset"));
            Console.WriteLine(String.Format("{0}", "".PadRight(186, '-')));

            foreach (var timezone in timezones)
            {
                var o1 = timezone.GetUtcOffset(date1);
                var o2 = timezone.GetUtcOffset(date2);
                var o1String = " 00:00";
                var o2String = " 00:00";

                if (o1 < TimeSpan.Zero)
                    o1String = o1.ToString(@"\-hh\:mm");
                if (o1 > TimeSpan.Zero)
                    o1String = o1.ToString(@"\+hh\:mm");
                if (o2 < TimeSpan.Zero)
                    o2String = o2.ToString(@"\-hh\:mm");
                if (o2 > TimeSpan.Zero)
                    o2String = o2.ToString(@"\+hh\:mm");

                Console.WriteLine(String.Format("{0,-62} {1,-32} {2,-32} {3,-15} {4,-20} {5,-20}",
                                                timezone.DisplayName,
                                                timezone.StandardName,
                                                timezone.DaylightName,
                                                timezone.SupportsDaylightSavingTime ? "Yes" : "No",
                                                o1String,
                                                o2String));
            }
        }

    }


}