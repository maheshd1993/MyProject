using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class CreateTicketSettingDTO
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
        public bool IsEmailIDRequired { get; set; }
        public bool IsErrorTypeIDRequired { get; set; }
        public bool IsProductTypeIDRequired { get; set; }
        public bool IsUrgencyIDRequired { get; set; }
        public bool IssubjectRequired { get; set; }
        public bool IsStatusIDRequired { get; set; }
        public bool IsCustomerIDRequired { get; set; }
        public bool IsExtraCol1Required { get; set; }
        public bool IsExtraCol2Required { get; set; }
        public bool IsExtraCol3Required { get; set; }
        public bool IsExtraCol4Required { get; set; }
        public bool IsExtraCol5Required { get; set; }
        public bool ISExtraCol6Required { get; set; }
        public bool ISExtraCol7Required { get; set; }
        public bool ISExtraCol8Required { get; set; }
        public bool IsExtraCol9Required { get; set; }
        public bool IsExtraCol10Required { get; set; }
        public bool IsExtraCol11Required { get; set; }
        public bool IsExtraCol12Required { get; set; }
        public bool IsImageCol1Required { get; set; }
        public bool IsImageCol2Required { get; set; }
        public bool IsImageCol3Required { get; set; }
        public bool IsImageCol4Required { get; set; }
        public bool IsExtraColdropdown1Required { get; set; }
        public bool IsExtraColdropdown2Required { get; set; }
        public bool IsExtraColdropdown3Required { get; set; }
        public bool IsExtraColdropdown4Required { get; set; }
        public bool IsExtraColdropdown5Required { get; set; }
        //Column Text Name 
        public string TicketNoText { get; set; }
        public int NameTextPriority { get; set; }
        public string NameText { get; set; }
        public string EmailIDText { get; set; }
        public int EmailIDTextPriority { get; set; }

        public string PhoneNumberText { get; set; }
        public int PhoneNumberTextPriority { get; set; }

        public string ErrorTypeIDText { get; set; }
        public int ErrorTypeIDTextPriority { get; set; }

        public string ProductTypeIDText { get; set; }
        public int ProductTypeIDTextPriority { get; set; }

        public string UrgencyIDText { get; set; }
        public int UrgencyIDTextPriority { get; set; }

        public string subjectText { get; set; }
        public int subjectTextPriority { get; set; }

        public string StatusIDText { get; set; }
        public int StatusIDTextPriority { get; set; }

        public string AssignedToText { get; set; }
        public string CustomerIDText { get; set; }
        public int CustomerIDTextPriority { get; set; }

        public string ExtraCol1Text { get; set; }
        public int ExtraCol1TextPriority { get; set; }

        public string ExtraCol2Text { get; set; }
        public int ExtraCol2TextPriority { get; set; }

        public string ExtraCol3Text { get; set; }
        public int ExtraCol3TextPriority { get; set; }

        public string ExtraCol4Text { get; set; }
        public int ExtraCol4TextPriority { get; set; }

        public string ExtraCol5Text { get; set; }
        public int ExtraCol5TextPriority { get; set; }

        public string ExtraCol6Text { get; set; }
        public int ExtraCol6TextPriority { get; set; }

        public string ExtraCol7Text { get; set; }
        public int ExtraCol7TextPriority { get; set; }

        public string ExtraCol8Text { get; set; }
        public int ExtraCol8TextPriority { get; set; }

        public string ExtraCol9Text { get; set; }
        public int ExtraCol9TextPriority { get; set; }

        public string ExtraCol10Text { get; set; }
        public int ExtraCol10TextPriority { get; set; }

        public string ExtraCol11Text { get; set; }
        public int ExtraCol11TextPriority { get; set; }

        public string ExtraCol12Text { get; set; }
        public int ExtraCol12TextPriority { get; set; }
        public string ExtraColdropdown1Text { get; set; }
        public int ExtraColdropdown1Priority { get; set; }

        public string ExtraColdropdown2Text { get; set; }
        public int ExtraColdropdown2Priority { get; set; }

        public string ExtraColdropdown3Text { get; set; }
        public int ExtraColdropdown3Priority { get; set; }

        public string ExtraColdropdown4Text { get; set; }
        public int ExtraColdropdown4Priority { get; set; }

        public string ExtraColdropdown5Text { get; set; }
        public int ExtraColdropdown5Priority { get; set; }
        public string ImageCol1Text { get; set; }
        public int ImageCol1TextPriority { get; set; }

        public string ImageCol2Text { get; set; }
        public int ImageCol2TextPriority { get; set; }

        public string ImageCol3Text { get; set; }
        public int ImageCol3TextPriority { get; set; }

        public string ImageCol4Text { get; set; }
        public int ImageCol4TextPriority { get; set; }

    }
}