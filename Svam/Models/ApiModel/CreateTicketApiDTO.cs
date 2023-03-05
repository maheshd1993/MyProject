using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models.ApiModel
{
    public class CreateTicketApiDTO
    {
        public long? TicketID { get; set; }
        public string CustomerNM { get; set; }
        public string TicketNo { get; set; }
        public string DateType { get; set; }
        public string ExistingNew { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string NewCustomerName { get; set; }
        public string EmailID { get; set; }
        public string PhoneNumber { get; set; } 
        public int? ErrorTypeID { get; set; }
        public string ErrorTypeName { get; set; }
        public int? ProductTypeID { get; set; }
        public string ProductTypeName { get; set; }
        public int? UrgencyID { get; set; }
        public string UrgencyName { get; set; }
        public string subject  { get; set; }
        public string TicketDescription { get; set; }
        public string TicketAttachment { get; set; }
        public string TeamRemark { get; set; }       
        public int? CompanyID { get; set; }
        public int? BranchID { get; set; }
        public int? StatusID  { get; set; }
        public string TicketStatusName { get; set; }
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public bool IsCustomerTbl { get; set; }
        public int EscalateLevel { get; set; }
        public string ExtraCol1 { get; set; }
        public string ExtraCol2 { get; set; }
        public string ExtraCol3 { get; set; }
        public string ExtraCol4 { get; set; }
        public string ExtraCol5 { get; set; }
        public string ExtraCol6 { get; set; }
        public decimal? ExtraCol7 { get; set; }
        public decimal? ExtraCol8 { get; set; }
        public string ExtraCol9 { get; set; }
        public string ExtraCol10 { get; set; }
        public int? ExtraCol11 { get; set; }
        public int? ExtraCol12 { get; set; }
        public string Token { get; set; }
        //public string ImageCol1Ext { get; set; }
        //public string ImageCol2Ext { get; set; }
        //public string ImageCol3Ext { get; set; }
        //public string ImageCol4Ext { get; set; }

        //public string ImageCol1 { get; set; }
        //public string ImageCol2 { get; set; }
        //public string ImageCol3 { get; set; }
        //public string ImageCol4 { get; set; }

    }
}