using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class CreateLeadSettingDTO
    {
        public long Id { get; set; }
        public int CompanyID { get; set; }
        public int BranchID { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsDesignation { get; set; }
        public bool IsMobileNo { get; set; }
        public bool IsEmailId { get; set; }
        public bool IsLeadResource { get; set; }
        public bool IsLeadStatus { get; set; }
        public bool IsProductType { get; set; }
        public bool IsOrganizationName { get; set; }
        public bool IsTimeZoneName { get; set; }
        public bool IsFollowupDate { get; set; }
        public bool IsFollowUpTime { get; set; }
        public bool IsFollowupTimeinIST { get; set; }
        public bool IsCountry { get; set; }
        public bool IsCity { get; set; }
        public bool IsState { get; set; }
        public bool IsAddress { get; set; }
        public bool IsUrl { get; set; }
        public bool IsSkypeId { get; set; }
        public bool IsDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsDateofBirth { get; set; }
        public bool IsMarriageAnniversary { get; set; }
        public bool IsExpectedClosingDate { get; set; }
        public bool IsExpectedDealAmount { get; set; }
        public bool IsDesigMandatory { get; set; }
        public bool IsEmailMandatory { get; set; }
        public bool IsLdResMandatory { get; set; }
        public bool IsLdStatusMandatory { get; set; }
        public bool IsProdTypeMandatory { get; set; }
        public bool IsOrgNameMandatory { get; set; }
        public bool IsTZNameMandatory { get; set; }
        public bool IsFUpTimeMandatory { get; set; }
        public bool IsFupTimeinISTMandatory { get; set; }
        public bool IsCountryMandatory { get; set; }
        public bool IsCityMandatory { get; set; }
        public bool IsStateMandatory { get; set; }
        public bool IsAddressMandatory { get; set; }
        public bool IsUrlMandatory { get; set; }
        public bool IsSkypeIdMandatory { get; set; }
        public bool IsDOBMandatory { get; set; }
        public bool IsMrgAniMandatory { get; set; }
        public bool IsExpClsDateMandatory { get; set; }
        public bool IsExpDealAmtMandatory { get; set; }
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
        public bool IsExtraCol1Mandatory { get; set; }
        public bool IsExtraCol2Mandatory { get; set; }
        public bool IsExtraCol3Mandatory { get; set; }
        public bool IsExtraCol4Mandatory { get; set; }
        public bool IsExtraCol5Mandatory { get; set; }
        public bool IsExtraCol6Mandatory { get; set; }
        public bool IsExtraCol7Mandatory { get; set; }
        public bool IsExtraCol8Mandatory { get; set; }
        public bool IsExtraCol9Mandatory { get; set; }
        public bool IsExtraCol10Mandatory { get; set; }

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
        public bool IsExtraCol11Mandatory { get; set; }
        public bool IsExtraCol12Mandatory { get; set; }
        public bool IsExtraCol13Mandatory { get; set; }
        public bool IsExtraCol14Mandatory { get; set; }
        public bool IsExtraCol15Mandatory { get; set; }
        public bool IsExtraCol16Mandatory { get; set; }
        public bool IsExtraCol17Mandatory { get; set; }
        public bool IsExtraCol18Mandatory { get; set; }
        public bool IsExtraCol19Mandatory { get; set; }
        public bool IsExtraCol20Mandatory { get; set; }
        public bool IsExtraCol1dropdown { get; set; }
        public bool IsExtraCol2dropdown { get; set; }
        public bool IsExtraCol3dropdown { get; set; }
        public bool IsExtraCol4dropdown { get; set; }
        public bool IsExtraCol5dropdown { get; set; }

        public bool IsExtraCol1dropdownMandatory { get; set; }
        public bool IsExtraCol2dropdownMandatory { get; set; }
        public bool IsExtraCol3dropdownMandatory { get; set; }
        public bool IsExtraCol4dropdownMandatory { get; set; }
        public bool IsExtraCol5dropdownMandatory { get; set; }

        ////////////customizedformfield/////////////////////
        public string LeadOwnerTextName { get; set; }
        public int LeadOwnerPriority   { get; set; }

        public string CustomerTextName { get; set; }
        public int CustomerPriority { get; set; }

        public string DesignationTextName { get; set; }
        public int DesigPriority  { get; set; }

        public string MobileNoTextName { get; set; }
        public int MobPriority { get; set; }

        public string EmailIdTextName { get; set; }
        public int EmailPriority { get; set; } 

        public string CountryTextName { get; set; }
        public int CountryPriority { get; set; }

        public string StateTextName { get; set; }
        public int StatePriority { get; set; }

        public string CityTextName  { get; set; }
        public int CityPriority { get; set; } 

        public string LeadResourceTextName { get; set; }
        public int LeadSourcePriority { get; set; }

        public string LeadStatusTextName { get; set; }
        public int LeadStatusPriority { get; set; }

        public string ProductTypeNameTextName { get; set; }
        public int ProdTypePriority { get; set; } 

        public string OrganizationNameTextName { get; set; }
        public int OrgNamePriority { get; set; } 

        public string ZoneNameTextName { get; set; }
        public int ZoneNamePriority { get; set; } 

        public string FollowDateTextName { get; set; }
        public int FollowUpDatePriority { get; set; }

        public string FollowUpTimeTextName { get; set; }
        public int FollowUpTimePriority { get; set; } 

        public string FollowupTimeinISTTextName { get; set; }
        public int FollowUpTimeISTPriority { get; set; } 

        public string AddressTextNameTextName { get; set; }
        public int AddressPriority { get; set; }

        public string UrlTextName { get; set; }
        public int UrlPriority { get; set; }

        public string SkypeIdTextName { get; set; }
        public int SkypeIdPriority { get; set; }

        public string FormTextName { get; set; }
        
        public string DescriptionTextName { get; set; }
        public int?   CreatedBy { get; set; }
        public bool   Status { get; set; }
        //public DateTime? Createddate { get; set; }
        public DateTime? ModifiedDate { get; set; }
       
        public string DateofBirthTextName { get; set; }
        public int DOBPriority { get; set; } 

        public string MarriageAnniversaryTextName { get; set; }
        public int MerriageAnnivPriority { get; set; }

        public string ExpectedDateTextName { get; set; }
        public int ExpDatePriority { get; set; }

        public string ExpectedProductAmountTextName { get; set; }
        public int ExpAmountPriority { get; set; }

        public string ExtraCol1TextName { get; set; }
        public int ExtraCol1Priority { get; set; }

        public string ExtraCol2TextName { get; set; }
        public int ExtraCol2Priority { get; set; }

        public string ExtraCol3TextName { get; set; }
        public int ExtraCol3Priority { get; set; }

        public string ExtraCol4TextName { get; set; }
        public int ExtraCol4Priority { get; set; }

        public string ExtraCol5TextName { get; set; }
        public int ExtraCol5Priority { get; set; }

        public string ExtraCol6TextName { get; set; }
        public int ExtraCol6Priority { get; set; }

        public string ExtraCol7TextName { get; set; }
        public int ExtraCol7Priority { get; set; }

        public string ExtraCol8TextName { get; set; }
        public int ExtraCol8Priority { get; set; }

        public string ExtraCol9TextName { get; set; }
        public int ExtraCol9Priority { get; set; }

        public string ExtraCol10TextName { get; set; }
        public int ExtraCol10Priority { get; set; }

        public string ExtraCol11TextName { get; set; }
        public int ExtraCol11Priority { get; set; }

        public string ExtraCol12TextName { get; set; }
        public int ExtraCol12Priority { get; set; }

        public string ExtraCol13TextName { get; set; }
        public int ExtraCol13Priority { get; set; }

        public string ExtraCol14TextName { get; set; }
        public int ExtraCol14Priority { get; set; }

        public string ExtraCol15TextName { get; set; }
        public int ExtraCol15Priority { get; set; }

        public string ExtraCol16TextName { get; set; }
        public int ExtraCol16Priority { get; set; }

        public string ExtraCol17TextName { get; set; }
        public int ExtraCol17Priority { get; set; }

        public string ExtraCol18TextName { get; set; }
        public int ExtraCol18Priority { get; set; }

        public string ExtraCol19TextName { get; set; }
        public int ExtraCol19Priority { get; set; }

        public string ExtraCol20TextName { get; set; }
        public int ExtraCol20Priority { get; set; }

        public string HeaderMenuTextName { get; set; }
        public string ExtraCol1dropdown { get; set; }
        public int ExtraCol1Prioritydropdown { get; set; }

        public string ExtraCol2dropdown { get; set; }
        public int ExtraCol2Prioritydropdown { get; set; }

        public string ExtraCol3dropdown { get; set; }
        public int ExtraCol3Prioritydropdown { get; set; }

        public string ExtraCol4dropdown { get; set; }
        public int ExtraCol4Prioritydropdown { get; set; }

        public string ExtraCol5dropdown { get; set; }
        public int ExtraCol5Prioritydropdown { get; set; }

        public List<FieldsNameList> FieldNames { get; set; }
    }

    public class FieldsNameList
    {
        public string TextName { get; set; }
        public string Values  { get; set; }
        public int? Priority  { get; set; }
    }

    public class CreateLeadFieldDTO
    {
        public string HeaderMenuTextName { get; set; }
        public string FormTextName { get; set; }
        public string FieldName  { get; set; }
        public string FieldType   { get; set; }
        public string FieldTextName  { get; set; }
        public string FieldPreviousType { get; set; }
        public string SaveType { get; set; }
        public bool IsVisible  { get; set; }
        public bool IsRequired  { get; set; }
        public int NormalFieldCount  { get; set; }
        public int DecimalFieldCount  { get; set; }
        public int DateFieldCount  { get; set; }
        public int NumericFieldCount   { get; set; }
        public int FieldPriority { get; set; } 
        public List<FieldsNameList> FieldNames { get; set; }
    }
    public class ItemDropdownModel
    {
        public int Id { get; set; }
        public string DropDownItemNamw { get; set; }
        public string Status { get; set; }



    }
    public class TicketItemDropdownModel
    {
        public int T_Id { get; set; }
        public string T_DropDownItemName { get; set; }
        public string T_Status { get; set; }



    }
}