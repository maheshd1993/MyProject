using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class ViewLeadSettingDTO
    {
        public int Id { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public bool IsEmail { get; set; }
        public bool IsCity { get; set; }
        public bool IsDesignation  { get; set; }
        public bool IsDOB  { get; set; }
        public bool IsMrgAnnivarsary  { get; set; }
        public bool IsAssignedBy { get; set; }
        public bool IsAssignTo { get; set; }
        public bool IsAssignedDate { get; set; }
        public bool IsExpClosingDate { get; set; }
        public bool IsExpDealAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsCreatedBy { get; set; }
        public bool IsCreatedDate { get; set; }
        public bool IsModifiedDate { get; set; }
        public string ReportTextName { get; set; }
        public string LeadSampleTextName { get; set; }
        public string FBLeadSampleTextName { get; set; }
        public string TotalLeadTextName { get; set; }
        public bool IsCountry { get; set; }
        public bool IsState { get; set; }
        public bool IsProductType { get; set; }
        public bool IsLeadResource { get; set; }
        public bool IsAddress { get; set; }
        public bool IsUrl { get; set; }
        public bool IsSkypeId { get; set; }
        public bool IsTimeZoneName { get; set; }
        public bool IsOrganization { get; set; }
        public bool IsExtraCol1 { get; set; }
        public bool IsExtraCol2 { get; set; }
        public bool IsExtraCol3 { get; set; }
        public bool IsExtraCol4 { get; set; }
        public bool IsExtraCol5 { get; set; }
        public bool IsExtraCol6 { get; set; }
        public bool IsExtraCol7 { get; set; }
        public bool IsExtraCol8 { get; set; }
        public bool IsExtraCol9 { get; set; }
        public bool IsExtraCol10 { get; set; }
        public bool IsExtraCol9Filter { get; set; }
        public bool IsExtraCol10Filter { get; set; }
        public bool IsTermFilter { get; set; }
        public bool IsProductTypeFilter { get; set; }
        public bool IsLeadSourceFilter { get; set; }
        public bool IsExtraCol11 { get; set; }
        public bool IsExtraCol12 { get; set; }
        public bool IsExtraCol13 { get; set; }
        public bool IsExtraCol14 { get; set; }
        public bool IsExtraCol15 { get; set; }
        public bool IsExtraCol16 { get; set; }
        public bool IsExtraCol17 { get; set; }
        public bool IsExtraCol18 { get; set; }
        public bool IsExtraCol19 { get; set; }
        public bool IsExtraCol20 { get; set; }
        public bool IsExtraCol18Filter { get; set; }
        public bool IsExtraCol19Filter { get; set; }
        public bool IsExtraCol20Filter { get; set; }
        public bool IsExtraCol1dropdown { get; set; }
        public bool IsExtraCol2dropdown { get; set; }
        public bool IsExtraCol3dropdown { get; set; }
        public bool IsExtraCol4dropdown { get; set; }
        public bool IsExtraCol5dropdown { get; set; }
        public bool IsCustomerNameFilter { get; set; }
        public bool IsMobNoFilter { get; set; }
        public bool IsEmailFilter { get; set; }
        public bool IsDesigFilter { get; set; }
        public bool IsOrgNameFilter { get; set; }
        public bool IsSkypIdFilter { get; set; }
        public bool IsFollowUpTime { get; set; }
        public bool IsFollowUpTimeIST { get; set; }
        ////////////customizedformfield/////////////////////
        public string FilterPlaceHolder { get; set; }
        public string LeadOwnerTextName { get; set; }
        public string CustomerTextName { get; set; }
        public string DesignationTextName { get; set; }
        public string MobileNoTextName { get; set; }
        public string EmailIdTextName { get; set; }
        //public string OtherNoTextName { get; set; }
       // public string DNDStatusTextName { get; set; }
        public string LeadsTypeTextName { get; set; }
        public string LeadResourceTextName { get; set; }
        public string LeadStatusTextName { get; set; }
        public string ProductTypeNameTextName { get; set; }
        public string OrganizationNameTextName { get; set; }
        public string ZoneNameTextName { get; set; }
        public string FollowDateTextName { get; set; }
        public string FollowUpTimeTextName { get; set; }
        public string FollowupTimeinISTTextName { get; set; }
        public string CountryTextName { get; set; }
        public string StateTextName { get; set; }
        public string CityTextName { get; set; }
        public string AddressTextNameTextName { get; set; }
        public string UrlTextName { get; set; }
        public string SkypeIdTextName { get; set; }
        //public string FormTextName { get; set; }
        //public string DescriptionTextName { get; set; }
       
        public string DateofBirthTextName { get; set; }
        public string MarriageAnniversaryTextName { get; set; }
        public string ExpectedDateTextName { get; set; }
        public string ExpectedProductAmountTextName { get; set; }
        public string ExtraCol1TextName { get; set; }
        public string ExtraCol2TextName { get; set; }
        public string ExtraCol3TextName { get; set; }
        public string ExtraCol4TextName { get; set; }
        public string ExtraCol5TextName { get; set; }
        public string ExtraCol6TextName { get; set; }
        public string ExtraCol7TextName { get; set; }
        public string ExtraCol8TextName { get; set; }
        public string ExtraCol9TextName { get; set; }
        public string ExtraCol10TextName { get; set; }
        public string ExtraCol11TextName { get; set; }
        public string ExtraCol12TextName { get; set; }
        public string ExtraCol13TextName { get; set; }
        public string ExtraCol14TextName { get; set; }
        public string ExtraCol15TextName { get; set; }
        public string ExtraCol16TextName { get; set; }
        public string ExtraCol17TextName { get; set; }
        public string ExtraCol18TextName { get; set; }
        public string ExtraCol19TextName { get; set; }
        public string ExtraCol20TextName { get; set; }
        public string HeaderMenuTextName { get; set; }
        public string ExtraCol1dropdown { get; set; }
        public string ExtraCol2dropdown { get; set; }
        public string ExtraCol3dropdown { get; set; }
        public string ExtraCol4dropdown { get; set; }
        public string ExtraCol5dropdown { get; set; }
        public bool IsExtraCol1dropdownFilter { get; set; }
        public bool IsExtraCol2dropdownFilter { get; set; }
        public bool IsExtraCol3dropdownFilter { get; set; }
        public bool IsExtraCol4dropdownFilter { get; set; }
        public bool IsExtraCol5dropdownFilter { get; set; }
    }
}