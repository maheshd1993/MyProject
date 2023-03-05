using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class CreateInvoiceModel
    {
        public string Invoiceid { get; set; }
        public string InvoiceNo { get; set; }
        public string QuotationNo { get; set; }
        public string QID { get; set; }
        public string QuotationDate { get; set; }
        public string RevisionNo { get; set; }
        [Required(ErrorMessage = "*")]
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
        public string ChallanId { get; set; }
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
        public string BranchId { get; set; }
        [Required(ErrorMessage = "*")]
        public string InvoiceDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Remarks { get; set; }

        //Item sub Total......................
        public string ItmDiscountPer { get; set; }
        public string ItmDiscountAmt { get; set; }
        public string ItmHandlingChargePercentage { get; set; }
        public string ItmHandlingChargeAmt { get; set; }
        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string ItmTaxGroupName { get; set; }
        public string ItmTaxGroupPercentage { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }
        public List<CreatInvoiceItemDetailsModel> CreatInvoiceItemDetailsModelList { get; set; }

    }

    public class CreatInvoiceItemDetailsModel
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

    // View Challan Model........
    public class ViewInvoiceModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string QuotationNo { get; set; }
        public string ChallanNo { get; set; }
        public string QuotationDate { get; set; }
        public string ChallanDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string ConatctNo { get; set; }
        public string Email { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddess { get; set; }
        public string BranchId { get; set; }
        public string BrachName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public string BranchTinNo { get; set; }
        public string Remarks { get; set; }

        //Item sub Total......................
        public string ItmDiscountPer { get; set; }
        public string ItmDiscountAmt { get; set; }
        public string ItmHandlingChargePercentage { get; set; }
        public string ItmHandlingChargeAmt { get; set; }
        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string ItmTaxGroupName { get; set; }
        public string ItmTaxGroupPercentage { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }

        public List<ViewInvoiceModel> viewInvoiceModelList = new List<ViewInvoiceModel>();
        public List<ViewInvoiceItemDetailsModel> viewInvoiceItemDetailModelList = new List<ViewInvoiceItemDetailsModel>();
    }

    public class ViewInvoiceItemDetailsModel
    {
        public string Itemid { get; set; }
        public string PartNo { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "*")]
        public string Qty { get; set; }
        public string Rate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ChallanNo { get; set; }
        public string hdRemoveID { get; set; }
    }

    //Start here for View Service Invoice Model......

    public class ViewServiceInvoiceModel
    {
        public int InvoiceId { get; set; }
        public string ServiceInvoiceNo { get; set; }
        public string ServiceInvoiceDate { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactPerson { get; set; }
        public string ConatctNo { get; set; }
        public string Email { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddess { get; set; }
        public string BranchId { get; set; }
        public string BrachName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }
        public string Remarks { get; set; }

        //Item sub Total......................
        public string SDDiscountPer { get; set; }
        public string SDDiscountAmt { get; set; }

        public string SDTaxGroupName { get; set; }
        public string SDTaxGroupPercentage { get; set; }
        public string SDTaxGroupAmt { get; set; }
        public string SDTotalTaxAmt { get; set; }
        public string SubTotalAmount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string OrderPONo { get; set; }
        public string OrderNo { get; set; }

        public List<ViewServiceInvoiceModel> ViewServiceInvoiceModelList = new List<ViewServiceInvoiceModel>();
        public List<ViewServiceInvoiceItemDetailsModel> viewServiceInvoiceItemDetailModelList = new List<ViewServiceInvoiceItemDetailsModel>();
    }


    //Start here to Service Invoice Model.........

    public class ViewServiceInvoiceItemDetailsModel
    {
        public string Itemid { get; set; }
        public string ServicePartNo { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "*")]
        public string Qty { get; set; }
        public string Rate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }
        public string ServiceName { get; set; }
        public string UnitName { get; set; }
        public string hdRemoveID { get; set; }
    }

    public class CreateServiceInvoiceModel
    {
        public string Invoiceid { get; set; }
        public string InvoiceNo { get; set; }
        public string QuotationNo { get; set; }
        public string QID { get; set; }
        public string QuotationDate { get; set; }
        public string RevisionNo { get; set; }
        [Required(ErrorMessage = "*")]
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
        public string BranchId { get; set; }
        [Required(ErrorMessage = "*")]
        public string InvoiceDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Remarks { get; set; }
        public bool ShowDate { get; set; }
        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }


        //Item sub Total......................
        public string SDDiscountPer { get; set; }
        public string SDDiscountAmt { get; set; }
        public string SDTaxgroupApplyId { get; set; }
        public string SDTaxGroupName { get; set; }
        public string SDTaxGroupPercentage { get; set; }
        public string SDTaxGroupAmt { get; set; }
        public string SDTotalTaxAmt { get; set; }
        public List<CreatServiceInvoiceItemDetailsModel> CreatServiceInvoiceItemDetailsModelList { get; set; }

    }

    public class CreatServiceInvoiceItemDetailsModel
    {
        public string SDItemid { get; set; }
        public bool BillableChk { get; set; }
        public string ServicePartNo { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "*")]
        public string Qty { get; set; }
        public string Rate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }
    }
}