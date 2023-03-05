using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.ApiModel
{
    public class ViewTicketApiModel
    {
        public long? TicketID { get; set; } 

        public bool IsTicketNo { get; set; }
        public string TicketNo { get; set; }
        public string TicketNoLabel { get; set; }

        public bool IsCustomerName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNameLabel  { get; set; }

        public bool IsEmailID { get; set; }
        public string EmailID { get; set; }
        public string EmailIDLabel  { get; set; }

        public bool IsPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberLabel  { get; set; }

        public bool IsErrorTypeName { get; set; }
        public string ErrorTypeName { get; set; }
        public string ErrorTypeNameLabel { get; set; }

        public bool IsProductTypeName { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeNameLabel { get; set; }

        public bool IsUrgencyName { get; set; }
        public string UrgencyName { get; set; }
        public string UrgencyNameLabel { get; set; }

        public bool IsTicketSubject { get; set; }
        public string TicketSubject { get; set; }
        public string TicketSubjectLabel { get; set; }

        public bool IsAssignedToName { get; set; }
        public string AssignedToName { get; set; }
        public string AssignedToNameLabel { get; set; }

        public bool IsAssignedByName { get; set; }
        public string AssignedByName { get; set; }
        public string AssignedByNameLabel { get; set; }

        public bool IsCreatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByLabel { get; set; }

        public bool IsCreatedDate { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedDateLabel { get; set; }

        public bool IsModifiedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedDateLabel { get; set; }

        public bool IsTicketStatusName { get; set; }
        public string TicketStatusName { get; set; }
        public string TicketStatusNameLabel { get; set; }
      
        public bool IsAssignedDate { get; set; }
        public string AssignedDate { get; set; }
        public string AssignedDateLabel { get; set; }

        public bool IsExtraCol1 { get; set; }
        public string ExtraCol1 { get; set; }
        public string ExtraCol1Label { get; set; }

        public bool IsExtraCol2 { get; set; }
        public string ExtraCol2 { get; set; }
        public string ExtraCol2Label  { get; set; }

        public bool IsExtraCol3 { get; set; }
        public string ExtraCol3 { get; set; }
        public string ExtraCol3Label { get; set; }

        public bool IsExtraCol4 { get; set; }
        public string ExtraCol4 { get; set; }
        public string ExtraCol4Label { get; set; }

        public bool IsExtraCol5 { get; set; }
        public string ExtraCol5 { get; set; }
        public string ExtraCol5Label { get; set; }

        public bool IsExtraCol6 { get; set; }
        public string ExtraCol6 { get; set; }
        public string ExtraCol6Label { get; set; }

        public bool IsExtraCol7 { get; set; }
        public decimal ExtraCol7 { get; set; }
        public string ExtraCol7Label { get; set; }

        public bool IsExtraCol8 { get; set; }
        public decimal ExtraCol8 { get; set; }
        public string ExtraCol8Label { get; set; }

        public bool IsExtraCol9 { get; set; }
        public string ExtraCol9 { get; set; }
        public string ExtraCol9Label { get; set; }

        public bool IsExtraCol10 { get; set; }
        public string ExtraCol10 { get; set; }
        public string ExtraCol10Label { get; set; }

        public bool IsExtraCol11 { get; set; }
        public int ExtraCol11 { get; set; }
        public string ExtraCol11Label { get; set; }

        public bool IsExtraCol12 { get; set; }
        public int ExtraCol12 { get; set; }
        public string ExtraCol12Label { get; set; }

    }
}