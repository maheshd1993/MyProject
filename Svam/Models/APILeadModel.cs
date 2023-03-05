using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class APILeadModel
    {
        public Int32? LeadID { get; set; }
        public Int32 LeadOwnerID { get; set; }
        public string LeadOwner { get; set; }
        public Int32? UID { get; set; }
        public string UserName { get; set; }  
        public string MobileNo { get; set; }
        public string Designation { get; set; }
        public string Customer { get; set; }
        public string EmailId { get; set; }
        public string DOB { get; set; }
        public string MrgAnniversary  { get; set; }
        public string OrganizationName { get; set; } 
        public int? ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public int? LeadStatusID { get; set; }
        public string LeadStatusName { get; set; }
        public int? LeadSourceID { get; set; }
        public string LeadSourceName { get; set; }     
        public int? CountryID { get; set; }
        public string Country { get; set; }
        public int? CityID { get; set; }
        public string City { get; set; }
        public int? StateID { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string LeadAttachment { get; set; }      
        public string FollowDate { get; set; }
        public string FollowupTime { get; set; }
        public string FollowupTimeIST { get; set; }
        public string TimeZoneName { get; set; }
        public string ZoneName { get; set; }      
        public string DateofBirth { get; set; }
        public string MarriageAnniversary { get; set; }
        public string URL { get; set; }
        public string SkypeId { get; set; }
        public string OtherNo { get; set; }
        public string createdDate { get; set; }
        public string CompanyID { get; set; }
        public string BranchID { get; set; }
        public string File { get; set; }
        public string Token { get; set; }
        public string ExpectedDate { get; set; }
        public string ExpectedProductAmount { get; set; }

        public string ExtraCol1 { get; set; }
        public string ExtraCol2 { get; set; }
        public string ExtraCol3 { get; set; }
        public string ExtraCol4 { get; set; }
        public string ExtraCol5 { get; set; }
        public string ExtraCol6 { get; set; }
        public string ExtraCol7 { get; set; }
        public string ExtraCol8 { get; set; }
        public string ExtraCol9 { get; set; }
        public string ExtraCol10 { get; set; }
        public string ExtraCol11 { get; set; }
        public string ExtraCol12 { get; set; }
        public string ExtraCol13 { get; set; }
        public string ExtraCol14 { get; set; }
        public string ExtraCol15 { get; set; }
        public string ExtraCol16 { get; set; }
        public string ExtraCol17 { get; set; }
        public string ExtraCol18 { get; set; }
        public string ExtraCol19 { get; set; }
        public string ExtraCol20 { get; set; }
    }
   
    public class APIViewLeadModel 
    {
        public int Id { get; set; }
        public String ShortName { get; set; }
        public Int32 LeadId { get; set; }
        public string LeadOwner { get; set; }
        public string LeadName { get; set; }
        public string FollowupDate { get; set; }
        public string EMail { get; set; }
        public string Created_By { get; set; }
        public string LeadStatus { get; set; }
        public string RowBackground  { get; set; }
        public string LeadActualStatus { get; set; }
        public string Mob { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public string AssignBy { get; set; }
        public string AssignDate { get; set; }
        public string date { get; set; }
        public DateTime Createddate { get; set; }
        public string AssignToUserName { get; set; }
        public string AssignByUserName { get; set; }
        public string AssinedTo { get; set; }
        public string Address { get; set; }
      
        public string MappedUser { get; set; }
        public string UserddlName { get; set; }
        public Int32? AssignTo { get; set; }
        public string AssignedBy { get; set; }

        public string PreFollowUpDate { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }       
        public string Description { get; set; }

        public int? LeadStatusID { get; set; }
        public string LeadStatusName { get; set; }

        public string ProductTypeName { get; set; }
        public string LeadsourceName { get; set; }
        public int? UserID { get; set; }
        public string DateofBirth { get; set; }
        public string MarriageAnniversary { get; set; }
        public string URL { get; set; }
        public string SkypeId { get; set; }
        public string Designation { get; set; }
        public string OrganizationName { get; set; }
        public string IsDOB { get; set; }
        public string IsMA { get; set; }
        public string ExpectedDate { get; set; }
        public decimal ExpectedProductAmount { get; set; }

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

        public List<APIViewLeadModel> viewleadsList { get; set; }

        // view dymamic fields properties////////
        public string ReportTextName { get; set; }
        public bool IsCustomerName { get; set; }
        public string CustomerNameLabel { get; set; }

        public bool IsFollowupDate { get; set; }
        public string FollowupDateLabel { get; set; }

        public string EMailLabel { get; set; }
        public bool IsEMail { get; set; }

        public string Created_ByLabel { get; set; }
        public bool IsCreated_By { get; set; }

        public bool IsLeadStatus { get; set; }
        public string LeadStatusLabel { get; set; }

        public bool IsMob { get; set; }
        public string MobLabel { get; set; }

        public bool IsCountry { get; set; }
        public string CountryLabel { get; set; }

        public bool IsState { get; set; }
        public string StateLabel { get; set; }

        public bool IsCity { get; set; }
        public string CityLabel { get; set; }

        public bool IsProductType { get; set; }
        public string ProductTypeLabel { get; set; }

        public bool IsLeadResource { get; set; }
        public string LeadResourceLabel { get; set; }

        public bool IsCreatedDate { get; set; }
        public string CreatedDateLabel { get; set; }

        public bool IsAssignedBy { get; set; }
        public string AssignedByLabel { get; set; }

        public bool IsAssignDate { get; set; }
        public string AssignDateLabel { get; set; }

        public bool IsAssinedTo { get; set; }
        public string AssinedToLabel { get; set; }

        public bool IsAddress { get; set; }
        public string AddressLabel { get; set; }

        public bool IsModifiedDate { get; set; }
        public string ModifiedDateLabel { get; set; }

        public bool IsDateofBirth { get; set; }
        public string DateofBirthLabel { get; set; }

        public bool IsMarriageAnniversary { get; set; }
        public string MarriageAnniversaryLabel { get; set; }

        public bool IsURL { get; set; }
        public string URLLabel { get; set; }

        public bool IsSkypeId { get; set; }
        public string SkypeIdLabel { get; set; }

        public bool IsDesignation { get; set; }
        public string DesignationLabel { get; set; }

        public bool IsOrganizationName { get; set; }
        public string OrganizationNameLabel { get; set; }

        public bool IsExpectedDate { get; set; }
        public string ExpectedDateLabel { get; set; }

        public bool IsExpectedProductAmount { get; set; }
        public string ExpectedProductAmountLabel { get; set; }


        public bool IsExtraCol1 { get; set; }
        public string ExtraCol1Label { get; set; }

        public bool IsExtraCol2 { get; set; }
        public string ExtraCol2Label { get; set; }

        public bool IsExtraCol3 { get; set; }
        public string ExtraCol3Label { get; set; }

        public bool IsExtraCol4 { get; set; }
        public string ExtraCol4Label { get; set; }

        public bool IsExtraCol5 { get; set; }
        public string ExtraCol5Label { get; set; }

        public bool IsExtraCol6 { get; set; }
        public string ExtraCol6Label { get; set; }

        public bool IsExtraCol7 { get; set; }
        public string ExtraCol7Label { get; set; }

        public bool IsExtraCol8 { get; set; }
        public string ExtraCol8Label { get; set; }

        public bool IsExtraCol9 { get; set; }
        public string ExtraCol9Label { get; set; }

        public bool IsExtraCol10 { get; set; }
        public string ExtraCol10Label { get; set; }

        public bool IsExtraCol11 { get; set; }
        public string ExtraCol11Label { get; set; }

        public bool IsExtraCol12 { get; set; }
        public string ExtraCol12Label  { get; set; }

        public bool IsExtraCol13 { get; set; }
        public string ExtraCol13Label { get; set; }

        public bool IsExtraCol14 { get; set; }
        public string ExtraCol14Label { get; set; }

        public bool IsExtraCol15 { get; set; }
        public string ExtraCol15Label { get; set; }

        public bool IsExtraCol16 { get; set; }
        public string ExtraCol16Label { get; set; }

        public bool IsExtraCol17 { get; set; }
        public string ExtraCol17Label  { get; set; }

        public bool IsExtraCol18 { get; set; }
        public string ExtraCol18Label { get; set; }

        public bool IsExtraCol19 { get; set; }
        public string ExtraCol19Label { get; set; }

        public bool IsExtraCol20 { get; set; }
        public string ExtraCol20Label  { get; set; }
    }
}