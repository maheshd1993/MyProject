using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models.ViewModel
{
    public class SaleOrderVM
    {
        public string CustomerName  { get; set; }
        public int customerId { get; set; }
        public string customerpanNO { get; set; }
        public string Organization { get; set; }
        public string RegistrationNo  { get; set; }
        public string CompanyMobileNo  { get; set; }
        public string CompanyAlternateNo { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyAddress { get; set; } 
        public string PanNo { get; set; }
        public string OrderNo { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobileNo { get; set; }
        public string PlaceOfSupply  { get; set; }
        public string DeliveryDate   { get; set; }        
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateFormat { get; set; }
        public string TaxTypeName  { get; set; }     
        public string termcondition { get; set; }   
        public decimal NetAmount { get; set; } 
        public string AmountInWord { get; set; }
        public decimal TotalTax { get; set; }
        public decimal RoundOff { get; set; } 
        public string imagepath { get; set; }
        public string QuoNo { get; set; }
        public Nullable<System.DateTime> QuoDt { get; set; }
        public string CreatedBy { get; set; }
        public int companyid { get; set; }
        public int branchid { get; set; }
        public SelectList CustomerList { get; set; }
        public List<SaleOrderReportDTO> SaleReport { get; set; }
    }

    public class SaleOrderReportDTO 
    {
        public string BillNo { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal Price  { get; set; }
        public decimal Recivable { get; set; }
        public decimal GSTPer { get; set; }
        public decimal GSTAmount { get; set; } 
        public string CustomerName { get; set; }
        public string QuotationNo { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice  { get; set; }
        public decimal Total { get; set; }
        public decimal NetTaxable { get; set; } 
        public decimal OurPrice { get; set; } 
        public string BillDate  { get; set; }
        public string CreatedBy { get; set; }
        public string Delivery { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string RefNo { get; set; }
        public decimal DisPer { get; set; }
        public string TermCondition { get; set; }
        public string POS { get; set; }
        public string HSN  { get; set; }
        public string unit { get; set; }
        public decimal CGSTPer { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal IGSTPer { get; set; }
        public decimal IGSTAmount { get; set; }
    }
}