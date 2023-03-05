using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class CreateTicketDynamicFieldsAPIModel
    {
        public bool IsName { get; set; }
        public bool IsNameRequired  { get; set; }
        public string Name { get; set; }

        public bool IsEmailID { get; set; }
        public string EmailID { get; set; }
        public bool IsEmailIDRequired { get; set; }

        public bool IsPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberRequired  { get; set; }

        public bool IsErrorTypeID { get; set; }
        public bool IsErrorTypeIDRequired  { get; set; }
        public string ErrorTypeID { get; set; }

        public bool IsProductTypeID { get; set; }
        public bool IsProductTypeIDRequired  { get; set; }
        public string ProductTypeID { get; set; }

        public bool IsUrgencyID { get; set; }
        public bool IsUrgencyIDRequired { get; set; }
        public string UrgencyID { get; set; }

        public bool Issubject { get; set; }
        public bool IssubjectRequired { get; set; }
        public string subject { get; set; }

        public bool IsStatusID { get; set; } 
        public bool IsStatusIDRequired { get; set; }
        public string StatusID { get; set; }

        public bool IsExtraCol1 { get; set; }
        public bool IsExtraCol1Required { get; set; }
        public string ExtraCol1 { get; set; }


        public bool IsExtraCol2 { get; set; }
        public bool IsExtraCol2Required { get; set; }
        public string ExtraCol2 { get; set; }

        public bool IsExtraCol3 { get; set; }
        public bool IsExtraCol3Required { get; set; }
        public string ExtraCol3 { get; set; }


        public bool IsExtraCol4 { get; set; }
        public bool IsExtraCol4Required { get; set; }
        public string ExtraCol4 { get; set; }


        public bool IsExtraCol5 { get; set; }
        public bool IsExtraCol5Required { get; set; }
        public string ExtraCol5 { get; set; }


        public bool IsExtraCol6 { get; set; }
        public bool IsExtraCol6Required { get; set; }
        public string ExtraCol6 { get; set; }


        public bool IsExtraCol7 { get; set; }
        public bool IsExtraCol7Required { get; set; }
        public string ExtraCol7 { get; set; }

        public bool IsExtraCol8 { get; set; }
        public bool IsExtraCol8Required { get; set; }
        public string ExtraCol8 { get; set; }


        public bool IsExtraCol9 { get; set; }
        public bool IsExtraCol9Required { get; set; }
        public string ExtraCol9 { get; set; }


        public bool IsExtraCol10 { get; set; }
        public bool IsExtraCol10Required { get; set; }
        public string ExtraCol10 { get; set; }


        public bool IsExtraCol11 { get; set; }
        public bool IsExtraCol11Required { get; set; }
        public string ExtraCol11 { get; set; }


        public bool IsExtraCol12 { get; set; }
        public bool IsExtraCol12Required { get; set; }
        public string ExtraCol12 { get; set; }

    }
}