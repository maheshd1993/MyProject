using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Svam.Models.ApiModel
{
   
    public class CreateLeadDynamicAPIModel
    {
        //public string HeaderMenuTextName { get; set; }
        public string FormTextName { get; set; }
        public string LeadOwner { get; set; }

        public string Customer { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsCustomerRequired  { get; set; }

        public string MobileNo { get; set; }
        public bool IsMobileNo { get; set; }
        public bool IsMobileNoRequired { get; set; }

        public string EmailId { get; set; }
        public bool IsEmailId { get; set; }
        public bool IsEmailIdRequired { get; set; }

        public string OrganizationName { get; set; }
        public bool IsOrganizationName { get; set; }
        public bool IsOrganizationNameRequired  { get; set; }

        public string Address { get; set; }
        public bool IsAddress { get; set; }
        public bool IsAddressRequired { get; set; }

        public string LeadStatus { get; set; }
        public bool IsLeadStatus { get; set; }
        public bool IsLeadStatusRequired { get; set; }

        public string LeadResource { get; set; }
        public bool IsLeadResource { get; set; }
        public bool IsLeadResourceRequired  { get; set; }

        public string ProductTypeName { get; set; }
        public bool IsProductTypeName  { get; set; }
        public bool IsProductTypeNameRequired  { get; set; }

        public string Description { get; set; }
        public bool IsDescription  { get; set; }
        public bool IsDescriptionRequired  { get; set; }

        public string Designation { get; set; }
        public bool IsDesignation { get; set; }
        public bool IsDesignationRequired  { get; set; }

        public string Country { get; set; }
        public bool IsCountry { get; set; }
        public bool IsCountryRequired  { get; set; }

        public string State { get; set; }
        public bool IsState { get; set; }
        public bool IsStateRequired  { get; set; }

        public string City { get; set; }
        public bool IsCity { get; set; }
        public bool IsCityRequired  { get; set; }

        public string FollowDate { get; set; }
        public bool IsFollowDate { get; set; }
        public bool IsFollowDateRequired { get; set; }

        public string ZoneName { get; set; }
        public bool IsZoneName { get; set; }
        public bool IsZoneNameRequired { get; set; }

        public string FollowUpTime { get; set; }
        public bool IsFollowUpTime { get; set; }
        public bool IsFollowUpTimeRequired  { get; set; }

        public string FollowupTimeinIST { get; set; }
        public bool IsFollowupTimeinIST { get; set; }
        public bool IsFollowupTimeinISTRequired { get; set; }

        public string Url { get; set; }
        public bool IsUrl { get; set; }
        public bool IsUrlRequired { get; set; }

        public string SkypeId { get; set; }
        public bool IsSkypeId { get; set; }
        public bool IsSkypeIdRequired { get; set; } 

       
        public string DateofBirth { get; set; }
        public bool IsDateofBirth { get; set; }
        public bool IsDateofBirthRequired  { get; set; }

        public string MarriageAnniversary { get; set; }
        public bool IsMarriageAnniversary { get; set; }
        public bool IsMarriageAnniversaryRequired { get; set; }

        public string ExpectedDate { get; set; }
        public bool IsExpectedDate { get; set; }
        public bool IsExpectedDateRequired  { get; set; }

        public string ExpectedProductAmount { get; set; }
        public bool IsExpectedProductAmount { get; set; }
        public bool IsExpectedProductAmountRequired { get; set; }

        public string ExtraCol1 { get; set; }
        public bool IsExtraCol1 { get; set; }
        public bool IsExtraCol1Required  { get; set; }

        public string ExtraCol2 { get; set; }
        public bool IsExtraCol2 { get; set; }
        public bool IsExtraCol2Required { get; set; }

        public string ExtraCol3 { get; set; }
        public bool IsExtraCol3 { get; set; }
        public bool IsExtraCol3Required { get; set; }

        public string ExtraCol4 { get; set; }
        public bool IsExtraCol4 { get; set; }
        public bool IsExtraCol4Required  { get; set; }

        public string ExtraCol5 { get; set; }
        public bool IsExtraCol5 { get; set; }
        public bool IsExtraCol5Required  { get; set; }

        public string ExtraCol6 { get; set; }
        public bool IsExtraCol6 { get; set; }
        public bool IsExtraCol6Required  { get; set; }

        public string ExtraCol7 { get; set; }
        public bool IsExtraCol7 { get; set; }
        public bool IsExtraCol7Required  { get; set; }

        public string ExtraCol8 { get; set; }
        public bool IsExtraCol8 { get; set; }
        public bool IsExtraCol8Required  { get; set; }

        public string ExtraCol9 { get; set; }
        public bool IsExtraCol9 { get; set; }
        public bool IsExtraCol9Required { get; set; }

        public string ExtraCol10 { get; set; }
        public bool IsExtraCol10 { get; set; }
        public bool IsExtraCol10Required  { get; set; }

        public string ExtraCol11 { get; set; }
        public bool IsExtraCol11 { get; set; }
        public bool IsExtraCol11Required { get; set; }

        public string ExtraCol12 { get; set; }
        public bool IsExtraCol12 { get; set; }
        public bool IsExtraCol12Required { get; set; }

        public string ExtraCol13 { get; set; }
        public bool IsExtraCol13 { get; set; }
        public bool IsExtraCol13Required { get; set; }

        public string ExtraCol14 { get; set; }
        public bool IsExtraCol14 { get; set; }
        public bool IsExtraCol14Required { get; set; }

        public string ExtraCol15 { get; set; }
        public bool IsExtraCol15 { get; set; }
        public bool IsExtraCol15Required { get; set; }

        public string ExtraCol16 { get; set; }
        public bool IsExtraCol16 { get; set; }
        public bool IsExtraCol16Required { get; set; }

        public string ExtraCol17 { get; set; }
        public bool IsExtraCol17 { get; set; }
        public bool IsExtraCol17Required { get; set; }

        public string ExtraCol18 { get; set; }
        public bool IsExtraCol18 { get; set; }
        public bool IsExtraCol18Required { get; set; }

        public string ExtraCol19 { get; set; }
        public bool IsExtraCol19 { get; set; }
        public bool IsExtraCol19Required { get; set; }

        public string ExtraCol20 { get; set; }
        public bool IsExtraCol20 { get; set; } 
        public bool IsExtraCol20Required { get; set; }
    }

}