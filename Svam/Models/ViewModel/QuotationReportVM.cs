using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Svam.Models.ViewModel
{
    public class QuotationReportVM
    {
        public int CustomerId  { get; set; }
        public int companyid { get; set; }
        public string FromDate { get; set; }
        public string ToDate  { get; set; }
        public string Organization { get; set; }
        public string RegistrationNo { get; set; }
        public string CompanyEmail  { get; set; }
        public int StateId  { get; set; }
        public int CityId  { get; set; }
        public string CompanyAddress  { get; set; }
        public string BranchName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string DateFormat  { get; set; }

        public string CompanyMobileNo  { get; set; }
        public string CustomerName  { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMobileNo { get; set; }
        public string QuotationNo  { get; set; }
        public DateTime QuotationDate { get; set; }
        public string PlaceOfSupply  { get; set; }
        public decimal? GrandTotalAmt { get; set; }
        public string AmountInWord { get; set; }
        public string TaxTypeName  { get; set; }
        public List<QuotationDTO> QtReport { get; set; }

        public List<QuotationItemDetail> QtItemList  { get; set; }
        public SelectList CustomerList { get; set; }
    }

    public class QuotationDTO
    {
        public string CustomerName { get; set; }
        public string QuotationNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public int TotalQuantity { get; set; }
        public int companyId { get; set; }
        public string Createdby { get; set; }
        public int status { get; set; }
    }

    public class QuotationItemDetail
    {       
        public string SKU { get; set; }
        public string ItemName  { get; set; }       
        public int? Qty { get; set; }
        public decimal? UnitPrice  { get; set; }
        public decimal? Total  { get; set; }
        public decimal? NetTotalAmt { get; set; }
        public decimal? CGSTPer { get; set; }
        public decimal? SGSTPer { get; set; }
        public decimal? IGSTPer  { get; set; }
        public decimal? CGSTAmount { get; set; }
        public decimal? SGSTAmount { get; set; }
        public decimal? IGSTAmount { get; set; }        
        public string HSNCode { get; set; }
    }
}