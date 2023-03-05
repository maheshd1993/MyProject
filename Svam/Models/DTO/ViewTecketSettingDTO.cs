using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class ViewTecketSettingDTO
    {
        public int Id { get; set; }

        public bool IsName { get; set; }
        public bool IsEmailID { get; set; }
        public bool IsPhoneNumber { get; set; }
        public bool IsErrorTypeID { get; set; }
        public bool IsProductTypeID { get; set; }
        public bool IsUrgencyID { get; set; }
        public bool Issubject { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsStatusID { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchID { get; set; }
        public bool IsCreatedBy { get; set; }
        public bool IsCreatedDate { get; set; }
        public bool IsAssignedTo { get; set; }
        public bool IsAssignedBy { get; set; }
        public bool IsModifiedDate { get; set; }
        public bool IsCustomerID { get; set; }
        public bool IsExtraCol1 { get; set; }
        public bool IsExtraCol2 { get; set; }
        public bool IsExtraCol3 { get; set; }
        public bool IsExtraCol4 { get; set; }
        public bool IsExtraCol5 { get; set; }
        public bool ISExtraCol6 { get; set; }
        public bool ISExtraCol7 { get; set; }
        public bool ISExtraCol8 { get; set; }
        public bool IsExtraCol9 { get; set; }
        public bool IsExtraCol10 { get; set; }
        public bool IsExtraCol11 { get; set; }
        public bool IsExtraCol12 { get; set; }
        public bool IsExtraColdropdown1 { get; set; }
        public bool IsExtraColdropdown2 { get; set; }
        public bool IsExtraColdropdown3 { get; set; }
        public bool IsExtraColdropdown4 { get; set; }
        public bool IsExtraColdropdown5 { get; set; }
        public bool IsImageCol1 { get; set; }
        public bool IsImageCol2 { get; set; }
        public bool IsImageCol3 { get; set; }
        public bool IsImageCol4 { get; set; }

        
        public string TicketNoText { get; set; }
        public string NameText { get; set; }
        public string EmailIDText { get; set; }
        public string PhoneNumberText { get; set; }
        public string ErrorTypeIDText { get; set; }
        public string ProductTypeIDText { get; set; }
        public string UrgencyIDText { get; set; }
        public string subjectText { get; set; }        
        public string StatusIDText { get; set; }      
        public string AssignedToText { get; set; }
        public string CustomerIDText { get; set; }
        public string ExtraCol1Text { get; set; }
        public string ExtraCol2Text { get; set; }
        public string ExtraCol3Text { get; set; }
        public string ExtraCol4Text { get; set; }
        public string ExtraCol5Text { get; set; }
        public string ExtraCol6Text { get; set; }
        public string ExtraCol7Text { get; set; }
        public string ExtraCol8Text { get; set; }
        public string ExtraCol9Text { get; set; }
        public string ExtraCol10Text { get; set; }
        public string ExtraCol11Text { get; set; }
        public string ExtraCol12Text { get; set; }
        public string ExtraColdropdown1Text { get; set; }
        public string ExtraColdropdown2Text { get; set; }
        public string ExtraColdropdown3Text { get; set; }
        public string ExtraColdropdown4Text { get; set; }
        public string ExtraColdropdown5Text { get; set; }
        public string ImageCol1Text { get; set; }
        public string ImageCol2Text { get; set; }
        public string ImageCol3Text { get; set; }
        public string ImageCol4Text { get; set; }
    }
}