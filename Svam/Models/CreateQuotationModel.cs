using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class CreateQuotationModel
    {
        public string Qid { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public int LeadId { get; set; }
        public string QuotationTypeId { get; set; }
        public string RevisionNo { get; set; }
        [Required(ErrorMessage="*")]
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        [Required(ErrorMessage = "*")]
        public string ContactPerson { get; set; }
        [Required(ErrorMessage = "*")]
        public string ConatctNo { get; set; }
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string BillingAddress { get; set; }
        [Required(ErrorMessage = "*")]
        public string ShippingAddess { get; set; }
        public string QuotationStatusId { get; set; }
        public string SalesPersonName { get; set; }
        public int SalesPersonId { get; set; }
        public string BranchId { get; set; }
        public string TaxGroupId { get; set; }
        public string Remarks { get; set; }

        //Item sub Total......................
        public string ItmDiscountPer { get; set; }
        public string ItmDiscountAmt { get; set; }
        public string ItmHandlingChargePercentage { get; set; }
        public string ItmHandlingChargeAmt { get; set; }
        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string ItmTaxGroupName { get; set; }
        public string ApplyITMTaxGID { get; set; }
        public string ItmTaxGroupPercentage { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }

        //Service Subtotal....................
        public string SDdiscountPercent { get; set; }
        public string SDdiscountAmount { get; set; }
        public string SDserviceTaxGroupId { get; set; }
        public string ApplySDTaxGID { get; set; }
        public string SDtaxesName { get; set; }
        public string SDserviceTaxPercentage { get; set; }
        public string SDServiceTaxAmt { get; set; }
        public string SDTotalTaxAmt { get; set; }

        //Final Total Price......................
        public string GrandTotal { get; set; }
        public string AmtinWords { get; set; }

        //Collect All List.....
        public List<QuotationItemDetailsModel> QuotationItemDetailsModelList { get; set; }
        public List<QuotationServiceDetailsModel> QuotationServiceDetailsModelList { get; set; }
    }

    public class QuotationItemDetailsModel
    {
        public string Itemid { get; set; }
        public bool BillableChk { get; set; }
        public string PartNo { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "*")]
        public string Qty { get; set; }
        public string Rate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }
    }
    
    public class QuotationServiceDetailsModel
    {
        public string sid { get; set; }
        public string ServiceSKU { get; set; }
        public string PartNo { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
    }
    

    public class QuotationCustomerdetailsInfoModel
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public string ContactPerson { get; set; }
        public string contactno { get; set; }
        public string Email { get; set; }
        public List<QuotationCustomerdetailsInfoModel> QcustomerInfomodelList = new List<QuotationCustomerdetailsInfoModel>();
    }

    public class ConfirmOrderAddMoreModel
    {
        public string OrderStatus { get; set; }
        public string CustomerPONo { get; set; }
        public string PORefDate { get; set; }
        public string ExpDeliveryDate { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Remarks { get; set; }
    }
    public class crmtermcondition
    {
        public Nullable<Int32> orgId { get; set; }
        public string Organization { get; set; }
        public List<crmtermcondition> OrgList1 { get; set; }
        public List<crmtermcondition> OrgList2 { get; set; }

        public List<crmtermcondition> joinlist { get; set; }
        public int Id { get; set; }
        public string TermCondition { get; set; }
        public bool Status { get; set; }

    }
}