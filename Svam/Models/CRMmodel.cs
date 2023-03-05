using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Svam.EF;
using Svam.Models;
using System.Web;
using Svam.Models.ViewModel;
using Svam.Models.DTO;

namespace Traders.Models
{
    public class CRMmodel
    {
    }

    public class CreatRoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ProfileName { get; set; }
        public bool CreateLeads { get; set; }
        public bool ViewLeads { get; set; }
        public bool ViewSales { get; set; }
        public bool AssignLeadManagement { get; set; }
        public bool DeveloperReport { get; set; }
        public bool CommonActivityRemark { get; set; }
        public bool RoleManagement { get; set; }
        public bool Notification { get; set; }
        public bool LeadNotify { get; set; }
        public bool ProjectManagement { get; set; }
        public bool LeadManagement { get; set; }
        public bool DailyWorkSchedule { get; set; }
        public bool Status { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public bool Viewpayment { get; set; }
        public bool TicketManagement { get; set; }
        public bool NewLeave { get; set; }
        public bool ViewLeave { get; set; }
        public bool FileManager { get; set; }
        public bool TrackSalePerson { get; set; }
        public bool ViewMapSalePerson { get; set; }
        public bool AddQuotation { get; set; }
        public bool CreateAssignWork { get; set; }
        public bool ViewAssignedWorks { get; set; }
        public bool LeaveManagement { get; set; }
        public bool EmpAttendence { get; set; }
        public bool EmpLoginHistory { get; set; }
        public bool ManualAttandence { get; set; }
        public bool ManageSalary { get; set; }
        public bool ManageExtraPayment { get; set; }
        public bool EmpLeaveRequest { get; set; }
        public bool CreateInterviewSchedule { get; set; }
        public bool ViewInterviewSchedule { get; set; }
        public bool ManageUser { get; set; }
        public bool CreateTicket { get; set; }
        public bool ViewTicket { get; set; }
        public bool MappedUser { get; set; }
        public bool CreateDailyRemark { get; set; }
        public bool ViewDailyRemark { get; set; }
        public bool SaleTarget { get; set; }
        public bool CreateLeadSetting { get; set; }
        public bool ViewLeadSetting { get; set; }
        public bool IsIndia { get; set; }
        public bool IsEmailSetup { get; set; }
        public bool IsEmailTemplates { get; set; }
        public bool IsEmployeeProfiles { get; set; }
        public bool IsErrorTypes { get; set; }
        public bool IsIndiaMartSetting { get; set; }
        public bool IsInterviewStatus { get; set; }
        public bool IsLeadForm { get; set; }
        public bool IsLeadSource { get; set; }
        public bool IsLeadStatus { get; set; }
        public bool IsLeadsView { get; set; }
        public bool IsProductTypes { get; set; }
        public bool IsTicketForm { get; set; }
        public bool IsTicketsView { get; set; }
        public bool IsUrgencyTypes { get; set; }
        public bool IsManageEmployees { get; set; }
        public bool IsLeaveName { get; set; }
        public bool ViewQuotation { get; set; }
        public bool AddSaleOrder { get; set; }
        public bool ViewSaleOrder { get; set; }
        public bool DailyBackup { get; set; }
        public bool Dailyupdatesummary { get; set; }
        public bool ProjectAssign { get; set; }
        public bool ProjectPermission { get; set; }
        public bool IsProjectStatus { get; set; }
        public bool IsItemTypes { get; set; }
        public bool ViewAssignItem { get; set; }

    }
    public class CreateUserModel
    {
        [Required(ErrorMessage = "* Please Enter First Name")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "* Please Enter Last Name")]
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string DateFormat { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        public string DateofBirth { get; set; }
        [Required(ErrorMessage = "* Please Contact Number")]
        public string ContactNumber { get; set; }
        public string AadharNumber { get; set; }
        public string AlternateNumber { get; set; }
        public string DateofJoining { get; set; }
        [Required(ErrorMessage = "*  Please Enter e-mail")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "* E-mail is not valid")]
        [Remote("ValidateEmail", "common", ErrorMessage = "EmailId already exist!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "* Please Select Gender")]
        public Nullable<int> Gender { get; set; }
        [Required(ErrorMessage = "* Please Enter Employee Code")]
        public string EmployeeCode { get; set; }
        [Required(ErrorMessage = "* Please Enter User Name")]
       // [Remote("ValidateUserName", "common", ErrorMessage = "User name already exist!")]
        public string UserName { get; set; }
        public string Designation { get; set; }
        [Required(ErrorMessage = "* Please Enter Password")]
        public string UserPassword { get; set; }
        [Required(ErrorMessage = "* Password Mismatch")]
        [System.Web.Mvc.CompareAttribute("UserPassword", ErrorMessage = "Password mismatch")]
        public string ConfirmPassword { get; set; }
        public string TimeZoneName { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string RefName1 { get; set; }
        public string RefPhoneNumber1 { get; set; }
        public string RefEmail1 { get; set; }
        public string RefName2 { get; set; }
        public string RefPhoneNumber2 { get; set; }
        public string RefEmail2 { get; set; }
        public string UserProfile { get; set; }
        public string AddProfileName { get; set; }
        public int ProfileId { get; set; }
        public int MapUserId { get; set; }
        public Nullable<decimal> CasualLeave { get; set; }
        public Nullable<decimal> MedicalLeave { get; set; }
        public string Year { get; set; }
        public Nullable<System.DateTime> ExpiredDate { get; set; }
        public bool IsActive { get; set; }
        public string AssignToCompanyID { get; set; }
        public int SalaryID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Uid { get; set; }
        public string Employetype { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public Nullable<decimal> MonthlySalary { get; set; }
        public Nullable<decimal> AnnualSalary { get; set; }
        public Nullable<decimal> BasicSalary { get; set; }
        public Nullable<decimal> HRA { get; set; }
        public Nullable<decimal> TravellingAllowance { get; set; }
        public Nullable<decimal> MedicalAllowance { get; set; }
        public Nullable<decimal> PerformanceIncentive { get; set; }
        public Nullable<decimal> OtherBenefits { get; set; }
        public Nullable<decimal> PFEmployeeShare { get; set; }
        public Nullable<decimal> PFEmployerShare { get; set; }
        public Nullable<decimal> ESICEmployerEmployee { get; set; }
        public Nullable<decimal> TDS { get; set; }
        public Nullable<decimal> OtherDeduction { get; set; }
        public string LWF { get; set; }
        public string Security { get; set; }
        public string Advance { get; set; }
        public string LWP { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public int[] IsChecked { get; set; }
        [Required(ErrorMessage = "* Please select branch")]
        public Nullable<Int32> BranchID { get; set; }
        public String CompanyBranchName { get; set; }
        public int? EscalateUserId { get; set; }
        public int? TempEscalateUserId { get; set; }
        public int EscalateLevel { get; set; }
        public int? ReportingManagerId { get; set; }
        public int? TempReportingManagerId { get; set; }
        public float EscalateTime { get; set; }
        public int? itemuid { get; set; }
        public int? ItemTypeId { get; set; }
        public string ItemTypeName { get; set; }
        public string itemSerialno { get; set; }
        public string itemcheck { get; set; }
        public int? Quantity { get; set; }
        public int? Customergroupid { get; set; }
        public string Customergroupname { get; set; }
        public Nullable<decimal> Estimated_Cost { get; set; }
        public Nullable<System.DateTime> Expirydate { get; set; }
        public Nullable<Int32> CompanyTypeID { get; set; }
        public String CompanyTypeName { get; set; }
        public List<ItemAssignlist> ItemTypeList = new List<ItemAssignlist>();
        public List<ItemAssignlist> AllItemTypeList = new List<ItemAssignlist>();
        public List<Customergrouplist> Allcustgrouplist = new List<Customergrouplist>();
        public List<CreateUserModel> obranchList { get; set; }
        public List<CreateUserModel> mapUserList = new List<CreateUserModel>();
        public List<CreateUserModel> CompanyTypeList { get; set; }
        public List<ManageOrganizationVM> OtherBranchUserList = new List<ManageOrganizationVM>();
        public List<CreateUserModel> UserList { get; set; }
        public List<CreateUserModel> itemtypelistt { get; set; }


        //public List<TicketEscalationMasterDTO> EscalateUsers = new List<TicketEscalationMasterDTO>();

        public List<Userddl> UsersList { get; set; }

        public HttpPostedFileBase[] Documents { get; set; }
        public HttpPostedFileBase[] Userprofilepic { get; set; }
    }

    public class ItemAssignlist
    {
        public int Id { get; set; }
        public string ItemTypeName { get; set; }
        public Nullable<int> ItemTypeId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public decimal Estimated_Cost { get; set; }
        public Nullable<System.DateTime> Expirydate { get; set; }
        public Nullable<System.DateTime> Assigndate { get; set; }
        public string CheckStatus { get; set; }
        public string SerialNo { get; set; }
        public int? UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class Customergrouplist
    {
        public int ID { get; set; }
        public string CustGroupName { get; set; }
        public string Remarks { get; set; }
        public string CompanyId { get; set; }
        public string BranchCode { get; set; }

    }

    public class LoginModel
    {
        [Required(ErrorMessage = "** Enter username or email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "** Enter password")]
        public string Password { get; set; }
    }

    public class UserLogInfoModel
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public string TimeZone { get; set; }
        public string ProfileName { get; set; }
        public string MappedUsers { get; set; }
        public string ProfileId { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool? IsExpired { get; set; }
        public bool? IsActive { get; set; }
        public int ByUID { get; set; }
    }



    public class CreateLeadsModel
    {
        
        public Int32? LeadID { get; set; }
        public Int32? UID { get; set; }
        public string UserName { get; set; }
        public int? CompanyID { get; set; }
        public int? BranchID { get; set; }
        public string DateFormat { get; set; }
        public HttpPostedFileBase file { get; set; }
        public string Language { get; set; }
        public Int32 LeadOwnerID { get; set; }
        public string LeadOwner { get; set; }
        [Required(ErrorMessage = "* Please enter mobile number")]
        public string MobileNo { get; set; }
        public string Designation { get; set; }
        [Required(ErrorMessage = "* Please enter customer name")]
        public string Customer { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "* E-mail is not valid")]
        public string EmailId { get; set; }
        public string OrganizationName { get; set; }
        public string FollowupTime { get; set; }
        public string FollowupTimeIST { get; set; }
        public string URL { get; set; }
        public string SkypeId { get; set; }
        public string OtherNo { get; set; }
        public string LeadSource { get; set; }
        public string LeadStatus { get; set; }
        [Required(ErrorMessage = "* Please select follow up date")]
        public string FollowDate { get; set; }
        public string TimeZoneName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string CompanyNodaTimeZone { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string LeadAttachment { get; set; }
        public bool Notify { get; set; }
        public Int32 AssignTo { get; set; }
        public string createdDate { get; set; }
        public Int64 LID { get; set; }
        public string ZoneName { get; set; }
        //[Required(ErrorMessage = "* Please select product type")]
        public int? ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public string ProdTypeName { get; set; }
        [Required(ErrorMessage = "* Please select lead status")]
        public int? LeadStatusID { get; set; }
        public string LeadStatusName { get; set; }
        public string LeadName { get; set; }
        //[Required(ErrorMessage = "* Please select lead source")]
        public int? LeadSourceID { get; set; }
        public string LeadSourceName { get; set; }
        public string LdSourceName { get; set; }
        public string DateofBirth { get; set; }
        public string MarriageAnniversary { get; set; }

        public Int32? CountryID { get; set; }
        public Int32? StateID { get; set; }
        public Int32? CityID { get; set; }

        public string ExpectedDate { get; set; }
        public decimal ExpectedProductAmount { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid whatsapp number")]
        public string ExtraCol1 { get; set; }
        public string ExtraCol2 { get; set; }
        public string ExtraCol3 { get; set; }
        public string ExtraCol4 { get; set; }
        public string ExtraCol5 { get; set; }
        public decimal? ExtraCol6 { get; set; }
        public int? ExtraCol7 { get; set; }
        public int? ExtraCol8 { get; set; }
        public string ExtraCol9 { get; set; }
        public string ExtraCol10 { get; set; }

        public decimal? ExtraCol11 { get; set; }
        public decimal? ExtraCol12 { get; set; }
        public decimal? ExtraCol13 { get; set; }
        public decimal? ExtraCol14 { get; set; }
        public int? ExtraCol15 { get; set; }
        public int? ExtraCol16 { get; set; }
        public int? ExtraCol17 { get; set; }

        public string ExtraCol18 { get; set; }
        public string ExtraCol19 { get; set; }
        public string ExtraCol20 { get; set; }
        public int? Extracol1dropId { get; set; }
        public string Extracol1dropdown { get; set; }
        public string Extracol2dropdown { get; set; }
        public string Extracol3dropdown { get; set; }
        public string Extracol4dropdown { get; set; }
        public string Extracol5dropdown { get; set; }
        public int? Extracol1dropdownId1 { get; set; }
        public int? Extracol1dropdownId2 { get; set; }
        public int? Extracol1dropdownId3 { get; set; }
        public int? Extracol1dropdownId4 { get; set; }
        public int? Extracol1dropdownId5 { get; set; }
        public bool IsProductTypeAdd { get; set; }
        public bool IsLeadStatusAdd { get; set; }
        public bool IsLeadSourceAdd { get; set; }
        public DateTime? ConvertedFupDateTime { get; set; }
        public string FollowupTimeInTZ { get; set; }
        public string PastPerformance { get; set; }
        public string ProfitLoss { get; set; }
        public string BuySell { get; set; }
        public string StockName { get; set; }
        public string Price { get; set; }
        public string Target { get; set; }
        public string Target2 { get; set; }
        public string Target3 { get; set; }
        public string SI { get; set; }
        public string Remark { get; set; }
        public int? dropdownItemId { get; set; }
        public int? dropddownItemId1 { get; set; }
        public int? dropddownItemId2 { get; set; }
        public int? dropddownItemId3 { get; set; }
        public int? dropddownItemId4 { get; set; }
        public int? dropddownItemId5 { get; set; }
        public string DropDownItemNamw { get; set; }
        public CreateLeadSettingDTO propVal { get; set; }
        public List<ProductTypeModel> producttypetblList { get; set; }
        public List<LeadStatusModel> leadstatusList { get; set; }
        public List<LeadSourceModel> leadsourceList { get; set; }
        public List<Leaddropdownmodel1> leaddropdownlist { get; set; }
        public List<Leaddropdownmodel2> leaddropdownlist2 { get; set; }
        public List<Leaddropdownmodel3> leaddropdownlist3 { get; set; }
        public List<Leaddropdownmodel4> leaddropdownlist4 { get; set; }
        public List<Leaddropdownmodel5> leaddropdownlist5 { get; set; }
        public List<ManageCountryModel> countryList { get; set; }
        public List<ManageStateModel> stateList { get; set; }
        public List<ManageCityModel> cityList { get; set; }

        public List<CreateUserModel> AssignToList { get; set; }
        public SelectList TimeZones { get; set; }
        public List<crm_leaddescriptiontbl> descriptionList { get; set; }
        public List<crm_createleadstbl> createlist { get; set; }


    }
    public class bindbookingdate
    {
        //public string customername { get; set; }
        public string extracol9 { get; set; }
    }
    public class TZName
    {
        public string zone_name { get; set; }
        public string StandardTZName { get; set; }
    }

    public class ManageCountryModel
    {
        public Int32? CountryID { get; set; }
        public string CountryName { get; set; }
        public string country_code { get; set; }
    }

    public class ManageStateModel
    {
        public Int32? StateID { get; set; }
        public string StateName { get; set; }
    }

    public class ManageCityModel
    {
        public Int32? CityID { get; set; }
        public string CityName { get; set; }
    }

    public class ViewSalesModel
    {
        public List<ViewSalesModel> viewsalesList = new List<ViewSalesModel>();
        public Int64 Id { get; set; }
        public string LeadName { get; set; }
        public string FollowupDate { get; set; }
        public string ProductName { get; set; }
        public string LotsSize { get; set; }
        public string InvtSize { get; set; }
        public string LeadStatus { get; set; }
        public string Mob { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string DateFormat { get; set; }
        public string ClosedBy { get; set; }
        public string AdvancePayment { get; set; }
        public string ProjectValue { get; set; }
        public List<Userddl> Userddllist { get; set; }
        public List<userworkdetails> userworklist = new List<userworkdetails>();
        public List<userworkdetails> userworkclosedlist = new List<userworkdetails>();
    }

    public class userworkdetails
    {
        
        public int Id { get; set; }
        public int ByUID { get; set; }
        public string userName {get;set;}
        public string ModifiedDate { get; set; }
        public string LeadStatus { get; set; }
        public string CustomerName { get; set; }
        public string FollowDate { get; set; }
        public string AssignedDate { get; set; }
        public string Createddate { get; set; }
        public string PastPerformance { get; set; }
        public string ProfitLoss { get; set; }
        public string BuySell { get; set; }
        public string StockName { get; set; }
        public string Price { get; set; }
        public string Target { get; set; }
        public string Target2 { get; set; }
        public string Target3 { get; set; }
        public string SI { get; set; }
        public string Remark { get; set; }
        public string closedlead { get; set; }


    }

    public class ViewLeadsModel
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Customer { get; set; }
        public int LeadId { get; set; }
        public int? UserID { get; set; }
        public int AssignToOthers { get; set; }
        public string FilterDate { get; set; }
        public string TodayDate { get; set; }
        public string Language { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FollowupDate { get; set; }
        public string FollowUpTime { get; set; }
        public string EMail { get; set; }
        public string Created_By { get; set; }
        public string LeadName { get; set; }
        public string LeadStatus { get; set; }
        public int? LeadStatusID { get; set; }
        public string LeadActualStatus { get; set; }
        public int LeadReminder { get; set; }
        [Required(ErrorMessage = "* Please select lead status")]
        public string LeadStatusName { get; set; }
        public string LeadStatusColor { get; set; }
        public string Term { get; set; }
        public string CompanyName { get; set; }
        public string Mob { get; set; }    
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        //public string Date { get; set; }    
        public string date { get; set; }
        public DateTime LeadDate { get; set; }
        public string CreatedDate { get; set; }
        public string LeadOwner { get; set; }
        public string Address { get; set; }
        public string MappedUser { get; set; }

        public string UserddlName { get; set; }
        public string FilterType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string filterText { get; set; }
        public int page { get; set; }
        public int pagetypeType { get; set; }

        public string AssignTo { get; set; }
        public string AssignBy { get; set; }
        public string AssignDate { get; set; }

        public string AssinedTo { get; set; }
        public string AssignedBy { get; set; }

        public string PreFollowUpDate { get; set; }
        public string ModifiedDate { get; set; }
        public DateTime? ConvertedFupDateTime { get; set; }

        [Required(ErrorMessage = "*")]
        public string Description { get; set; }

        public string IsDOB { get; set; }
        public string IsMA { get; set; }

        public int? TotalLead { get; set; }

        public DateTime? ExpectedDate { get; set; }
        public decimal ExpectedProductAmount { get; set; }

        public int? ProductTypeID { get; set; }
        public int? LeadSourceID { get; set; }
        public string ProductTypeName { get; set; }
        public string LeadsourceName { get; set; }
        public string DateofBirth { get; set; }
        public string MarriageAnniversary { get; set; }
        public string URL { get; set; }
        public string SkypeId { get; set; }
        public string Designation { get; set; }
        public string OrganizationName { get; set; }

        public string ExtraCol1 { get; set; }
        public string ExtraCol2 { get; set; }
        public string ExtraCol3 { get; set; }
        public string ExtraCol4 { get; set; }
        public string ExtraCol5 { get; set; }
        public decimal? ExtraCol6 { get; set; }
        public int? ExtraCol7 { get; set; }
        public int? ExtraCol8 { get; set; }
        public string ExtraCol9 { get; set; }
        public string ExtraCol10 { get; set; }
        public decimal? ExtraCol11 { get; set; }
        public decimal? ExtraCol12 { get; set; }
        public decimal? ExtraCol13 { get; set; }
        public decimal? ExtraCol14 { get; set; }
        public int? ExtraCol15 { get; set; }
        public int? ExtraCol16 { get; set; }
        public int? ExtraCol17 { get; set; }
        public string ExtraCol18 { get; set; }
        public string ExtraCol19 { get; set; }
        public string ExtraCol20 { get; set; }
        public int? Extracol1dropdown1Id { get; set; }
        public int? Extracol1dropdown2Id { get; set; }
        public int? Extracol1dropdown3Id { get; set; }
        public int? Extracol1dropdown4Id { get; set; }
        public int? Extracol1dropdown5Id { get; set; }
        public int? Extracol1dropdownId1 { get; set; }
        public int? Extracol1dropdownId2 { get; set; }
        public int? Extracol1dropdownId3 { get; set; }
        public int? Extracol1dropdownId4 { get; set; }
        public int? Extracol1dropdownId5 { get; set; }
        public string ExtraCol1Dropdown { get; set; }
        public string ExtraCol2Dropdown { get; set; }
        public string ExtraCol3Dropdown { get; set; }
        public string ExtraCol4Dropdown { get; set; }
        public string ExtraCol5Dropdown { get; set; }
        public int ResellerId { get; set; }
        public string ResellerName { get; set; }
        public string ResellerContactNo { get; set; }
        public string ResellerCode { get; set; }
        public string ResellerStatus { get; set; }
        public string ResellerDocStatus { get; set; }
        public string SmartCapitaPlan { get; set; }
        public long? EmailTemplateID { get; set; }
        public String EmailTemplateName { get; set; }
        public string DateFormat { get; set; }
        [AllowHtml]
        public String EmailTempleteBody { get; set; }
        [Required(ErrorMessage = "Please select Lead Status")]
        public List<LeadStatusModel> leadstatusList { get; set; }

        public ViewLeadSettingDTO columnVal { get; set; }

        public List<Userddl> Userddllist { get; set; }
        public List<CreateUserModel> AssignToList { get; set; }
        public List<FileManger> oFileMangerList { get; set; }
        public List<ViewLeadsModel> oEmailTemplateModelList { get; set; }
        public List<DashBoardLeadsModel> TodayNewLeadsList = new List<DashBoardLeadsModel>();
        public List<DashBoardLeadsModel> TodayFollowUpLeadsList = new List<DashBoardLeadsModel>();
        public List<ViewLeadsModel> viewleadsList = new List<ViewLeadsModel>();
        public List<ViewLeadsModel> AssignToAssignByList = new List<ViewLeadsModel>();
        public List<ViewLeadsModel> AllviewleadsList = new List<ViewLeadsModel>();
        public List<Userddl> OtherBranchMappedUser { get; set; }
        public SelectList LeadSource { get; set; }
        public SelectList LeadDropdown1 { get; set; }
        public SelectList LeadDropdown2 { get; set; }
        public SelectList LeadDropdown3 { get; set; }
        public SelectList LeadDropdown4 { get; set; }
        public SelectList LeadDropdown5 { get; set; }
        public SelectList ProductType { get; set; }
        public SelectList CountryList { get; set; }
    }

    public class FileManger
    {
        public Int64? FileID { get; set; }
        public String FileName { get; set; }
        public String FileUpload { get; set; }
    }

    public class Userddl
    {
        public int uid { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        // public string UnmId  { get; set; }
    }

    public class projectddl
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool IsActive { get; set; }
        public List<projectddl> projectddllist { get; set; }
        //public SelectList pddllist { get; set; }
        // public string UnmId  { get; set; }
    }

    public class Modelddl
    {
        public int PID { get; set; }
        public int MID { get; set; }
        public string ModuleName { get; set; }
        public bool IsActive { get; set; }
        public List<Modelddl> modelddllist { get; set; }
        // public string UnmId  { get; set; }
    }

    public class Customerddl
    {
        public int PID { get; set; }
        public int MID { get; set; }
        public string CustomerName { get; set; }

        // public string UnmId  { get; set; }
    }

    public class workassignModel
    {
        public List<crm_workassigntbl> worklist { get; set; }
    }

    public class WorkdescModel
    {
        //public string workcreatedate { get; set; }
        //public string assignto { get; set; }
        //public string description { get; set; }
        //public string workattachment { get; set; }
        //public List<WorkdescModel> workdescModelList = new List<WorkdescModel>();
       public List<crm_workdescriptiontbl> desclist { get; set; }
        public List<crm_saledetailtbl> closedprojectlist { get; set; }
    }
    public class ViewArchivesModel
    {
        public List<ViewArchivesModel> viewarchivesList = new List<ViewArchivesModel>();
        public string LeadOwner { get; set; }
        public Int32 LeadOwnerID { get; set; }
        public string FollowupDate { get; set; }
        public string ProductName { get; set; }
        public string LotsSize { get; set; }
        public string InvtSize { get; set; }
        public string LeadStatus { get; set; }
    }
    public class MapUserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<MapUserModel> mapuserlist = new List<MapUserModel>();
    }

    public class NotificationModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string Leadstype { get; set; }
        public int NoofLeads { get; set; }
        public string State { get; set; }
        public List<NotificationModel> notificationList = new List<NotificationModel>();
    }

    public class LeadNotifyModel
    {
        public Int32 Id { get; set; }
        public string LeadName { get; set; }
        public DateTime Date { get; set; }
        public string Leadstype { get; set; }
        public int NoofLeads { get; set; }
        public string State { get; set; }
        public string MobileNo { get; set; }
        public string NotifyByUser { get; set; }
        public List<LeadNotifyModel> LeadNotifyList = new List<LeadNotifyModel>();
    }

    public class MappedUserModel
    {
        public int MapUserId { get; set; }
        public string EMpName { get; set; }
        public string Email { get; set; }
        public string Profile { get; set; }
        public List<MappedUserModel> mappeduserList = new List<MappedUserModel>();
    }

    public class NewsEventsModel
    {
        [Required(ErrorMessage = "*")]
        public string Events { get; set; }

    }

    public class PaymentModel
    {
        public string BankName { get; set; }
        public string AccountDetails { get; set; }
        public string ProductDetails { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string PayComment { get; set; }
    }


    public class AssignedLeadsModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Language { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DateFormat { get; set; }
        public string FollowUpDate { get; set; }
        public string LeadStatus { get; set; }
        public string AssignToUserName { get; set; }
        public string AssignToUID { get; set; }
        public string AssignByUserName { get; set; }
        public Int32 AssignByUID { get; set; }
        public string AssignDate { get; set; }
        public string LeadOwner { get; set; }
        public string MappedUser { get; set; }
        public string UserddlName { get; set; }
        public string AssignTo { get; set; }
        public string AssignedBy { get; set; }

        public List<AssignedLeadsModel> assignleadsModelList = new List<AssignedLeadsModel>();
        public List<CreateUserModel> Userddllist { get; set; }
        public List<LeadStatusModel> leadstatusList { get; set; }
    }

    public class LeadSummaryReportModel
    {
        public Int32? NewLeadCount { get; set; }
        public Int32? FollowUpCount { get; set; }
        public Int32? MissedFollowUpCount { get; set; }
        public Int32? DelayedFollowUpCount { get; set; }
        public Int32? NotInterestedCount { get; set; }
        public Int32? ClosedLeadsUpCount { get; set; }
        public Int32? SuspectLeadsCount { get; set; }
        public Int32? ProspectLeadsCount { get; set; }
    }

    public class DailyUpdateModel
    {

        public int Id { get; set; }
        public string Fname { get; set; }
        public string lname { get; set; }
        public int LeadId { get; set; }
        public int? UserID { get; set; }
        public int AssignToOthers { get; set; }
        public string FilterDate { get; set; }
        public string TodayDate { get; set; }
        public string Language { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string FollowupDate { get; set; }
        public string FollowUpTime { get; set; }
        public string EMail { get; set; }
        public string Created_By { get; set; }
        public string LeadName { get; set; }
        public string LeadStatus { get; set; }
        public int? LeadStatusID { get; set; }
        public string LeadActualStatus { get; set; }
        public int LeadReminder { get; set; }
        [Required(ErrorMessage = "* Please select lead status")]
        public string LeadStatusName { get; set; }
        public string LeadStatusColor { get; set; }
        public string Term { get; set; }
        public string CompanyName { get; set; }
        public string Mob { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        //public string Date { get; set; }    
        public string date { get; set; }
        public DateTime LeadDate { get; set; }
        public string CreatedDate { get; set; }
        public string LeadOwner { get; set; }
        public string Address { get; set; }
        public string MappedUser { get; set; }

        public string UserddlName { get; set; }
        public string FilterType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string filterText { get; set; }
        public int page { get; set; }

        public string AssignTo { get; set; }
        public string AssignBy { get; set; }
        public string AssignDate { get; set; }

        public string AssinedTo { get; set; }
        public string AssignedBy { get; set; }
        public string DateFormat { get; set; }
        public string PreFollowUpDate { get; set; }
        public string ModifiedDate { get; set; }
        public string LeadStatusNameDemo { get; set; }
        public string LeadStatusNameclosed { get; set; }
        public string LeadStatusNamepro { get; set; }
        public string statuscountfollow { get; set; }
        public List<Userddl> Userddllist { get; set; }
        public List<DailyUpdateModel> DailyUpdatelist = new List<DailyUpdateModel>();
    }
}