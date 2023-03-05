using Svam.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models
{
    public class CRMTicketModel
    {
        public Nullable<Int32> UserID { get; set; }
        public String UserName { get; set; }

        public Nullable<Int64> TicketID { get; set; }
        public string CustomerNM  { get; set; }
        public String TicketNo { get; set; }
        public string DateType { get; set; }
        public string ExistingNew { get; set; }
        public string DateFormat  { get; set; }
        public Nullable<Int32> CustomerID { get; set; }
        public String CustomerName { get; set; }
        public String NewCustomerName { get; set; }
        public bool IsProdTypeAdd { get; set; }
        public bool IsErrorTypeAdd { get; set; }
        public bool IsUrgencyTypeAdd  { get; set; }
        public string Sparepartstatus { get; set; }
        //[Required(ErrorMessage = "* Please enter email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "* E-mail is not valid")]
        [DataType(DataType.EmailAddress)]
        public String EmailID { get; set; }
        
        
        public String PhoneNumber { get; set; }
        
        //[Required(ErrorMessage = "* Please select error type")]
        public Nullable<Int32> ErrorTypeID { get; set; }
        public String ErrorTypeName { get; set; }

        //[Required(ErrorMessage = "* Please select product type")]
        public Nullable<Int32> ProductTypeID { get; set; }
        public String ProductTypeName { get; set; }

        //[Required(ErrorMessage = "* Please select urgency")]
        public Nullable<Int32> UrgencyID { get; set; }
        public String UrgencyName { get; set; }

        //[Required(ErrorMessage = "* Please enter ticket subject")]
        public String TicketSubject { get; set; }

        [AllowHtml]
        public String TicketDescription { get; set; }
        public string TicketAttachment { get; set; }


        public Nullable<Int32> leadCustomerID { get; set; }
        //public string CustomerName { get; set; }
        public String leadCustomerName { get; set; }
        [AllowHtml]
        public string TeamRemark { get; set; }

        public string MappedUser { get; set; }

        public Nullable<Int32> CompanyID { get; set; }
        public Nullable<Int32> BranchID { get; set; }

        public Nullable<Int32> AssignedTo { get; set; }
        public String AssignedToName { get; set; }

        public Nullable<Int32> AssignedBy { get; set; }
        public String AssignedByName { get; set; }

        public String CreatedBy { get; set; }
        public String CreatedDate { get; set; }
        public String ModifiedDate { get; set; }
        public String AssignDate { get; set; }
        //[Required(ErrorMessage = "* Please select tickect status")]
        public Nullable<Int32> TicketStatusID { get; set; }
        public String TicketStatusName { get; set; }

        
        public Int32? TotalTicket { get; set; }

        public bool IsCustomerTbl { get; set; }
        public int EscalateLevel { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string ExtraCol1 { get; set; }
        public string ExtraCol2 { get; set; }
        public string ExtraCol3 { get; set; }
        public string ExtraCol4 { get; set; }
        public string ExtraCol5 { get; set; }
        public string ExtraCol6 { get; set; }
        public decimal ExtraCol7 { get; set; }
        public decimal ExtraCol8 { get; set; }
        public string ExtraCol9 { get; set; }
        public string ExtraCol10 { get; set; }
        public int ExtraCol11 { get; set; }
        public int ExtraCol12 { get; set; }

        public string ImageCol1Ext { get; set; }
        public string ImageCol2Ext { get; set; }
        public string ImageCol3Ext { get; set; }
        public string ImageCol4Ext { get; set; }

        public string ImageCol1 { get; set; }
        public string ImageCol2 { get; set; }
        public string ImageCol3 { get; set; }
        public string ImageCol4 { get; set; }

        public string T_DropdownitemId1 { get; set; }
        public string T_DropdownitemId2 { get; set; }
        public string T_DropdownitemId3 { get; set; }
        public string T_DropdownitemId4 { get; set; }
        public string T_DropdownitemId5 { get; set; }

        public string T_DropDownItemName1 { get; set; }
        public string T_DropDownItemName2 { get; set; }
        public string T_DropDownItemName3 { get; set; }
        public string T_DropDownItemName4 { get; set; }
        public string T_DropDownItemName5 { get; set; }

        public string T_DropDownItemName { get; set; }
        public CreateTicketSettingDTO propVal { get; set; }
        public ViewTecketSettingDTO columnVal { get; set; }

        public List<CRMTicketModel> CustomerList { get; set; }
        public List<CRMTicketModel> TicketStatusList { get; set; }
        public List<CRMTicketModel> UrgencyList { get; set; }
        public List<CRMTicketModel> ProductTypeList { get; set; }
        public List<CRMTicketModel> ErrorTypeList { get; set; }
        public List<CRMTicketModel> UserList { get; set; }
        public List<CRMTicketModel> AssignUserList { get; set; }
        public List<CRMTICKETASSIGNlist> selectAssignUserList { get; set; }
        public List<CRMTicketModel> CRMTicketModelList { get; set; }
        public List<Ticketdropdownmodel1> Ticketdropdownlist1 { get; set; }
        public List<Ticketdropdownmodel2> Ticketdropdownlist2 { get; set; }
        public List<Ticketdropdownmodel3> Ticketdropdownlist3 { get; set; }
        public List<Ticketdropdownmodel4> Ticketdropdownlist4 { get; set; }
        public List<Ticketdropdownmodel5> Ticketdropdownlist5 { get; set; }
        public HttpPostedFileBase ImageCol1File { get; set; }
        public HttpPostedFileBase ImageCol2File { get; set; }
        public HttpPostedFileBase ImageCol3File { get; set; }
        public HttpPostedFileBase ImageCol4File { get; set; } 
        public bool CreateTicket { get; set; }
        public bool ViewTicket { get; set; }
    }

    public class CRMTICKETASSIGNlist
    {
        public Nullable<Int32> UserID { get; set; }
        public String UserName { get; set; }
        
    }

    public class TicketMap 
    {
        public String Message { get; set; }
        public String AttachmentFile { get; set; }
        public String UserName { get; set; }
        public String CreatedOn { get; set; }
        public String StatusName { get; set; }
    }
    public class Ticketdropdownmodel1
    {
        public int T_DropdownitemId { get; set; }
        public string T_DropDownItemName { get; set; }

    }
    public class Ticketdropdownmodel2
    {
        public int T_DropdownitemId { get; set; }
        public string T_DropDownItemName { get; set; }

    }
    public class Ticketdropdownmodel3
    {
        public int T_DropdownitemId { get; set; }
        public string T_DropDownItemName { get; set; }

    }
    public class Ticketdropdownmodel4
    {
        public int T_DropdownitemId { get; set; }
        public string T_DropDownItemName { get; set; }

    }
    public class Ticketdropdownmodel5
    {
        public int T_DropdownitemId { get; set; }
        public string T_DropDownItemName { get; set; }

    }
}