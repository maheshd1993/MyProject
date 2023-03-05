using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class Purchase
    {
    }

    public class PurchaseRequitionModel
    {
        [Required(ErrorMessage="*")]
        public string PrDate { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionType { get; set; }
        public string DepartmentId { get; set; }
        public string RequesterId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Remarks { get; set; }       

        //User in another palce....
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string RequesterName { get; set; }
         public List<PRItemDetails> purchaseReqModelList { get; set; }

    }

    public class PRItemDetails
    {
        public string PartNo { get; set; }
        public string Itemid { get; set; }
        [Required(ErrorMessage = "*")]
        public string RequiredQty { get; set; }
        [Required(ErrorMessage = "*")]
        public string RequiredByDate { get; set; }
        public string Remarks { get; set; }

        //Common User Properties...
        public string ItemName { get; set; }
        public string Unitname { get; set; }
        public string SKU { get; set; }
    }

    public class ViewPRModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string PrNo { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Requester { get; set; }
        public string GenStatus { get; set; }
        public List<ViewPRModel> viewPRmodelList = new List<ViewPRModel>();
    }

    public class PurchaseOrderGenerateModel
    {
        public int Id { get; set; }
        public string PONo { get; set; }
        public string TransactionType { get; set; }
        public int DepartmentId { get; set; }
        public string PRNo { get; set; }
        public string PrDate { get; set; }
        public string POGenerateDate { get; set; }
        public string RequesterId { get; set; }
        public string SupplierId { get; set; }
        [Required(ErrorMessage="*")]
        public string SupplierAddress { get; set; }
        [Required(ErrorMessage = "*")]
        public string Remarks { get; set; }

        //Taxation properties.........
        public string ItmDiscountPer { get; set; }
        public string ItmDiscountAmt { get; set; }
        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string productApplyTaxesText { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }
        public List<PurchaseOrderGenerateItemModel> PurchaseOrderGenerateItemModelList{get;set;}
    }

    public class PurchaseOrderGenerateItemModel
    {
        public string PartNo { get; set; }
        public string Itemid { get; set; }
        [Required(ErrorMessage = "*")]
        public string Qty { get; set; }
        public string Rate { get; set; }
        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }

        //...............Other Use
        public string ItemName { get; set; }
        public string Unitname { get; set; }
        public string SKU { get; set; }
    }

   //Start The Product-Tax Model
    public class TaxGroupModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="*")]
        public string TaxgroupName { get; set; }
    }
    public class TaxvalesDetailsModel
    {
        public string Demo { get; set; }
        public int Id { get; set; }
        public int TaxGrouId { get; set; }
        [Required(ErrorMessage = "*")]
        public string TaxName { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal TaxValue { get; set; }
        public decimal Surcharge { get; set; }
        public List<TaxvalesDetailsModel> TaxValueinfoList = new List<TaxvalesDetailsModel>();
    }

    //Start Here the Service Tax Model......
    public class ServiceTaxGroupModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ServiceTaxgroupName { get; set; }
    }
    public class ServiceTaxGroupvalesDetailsModel
    {
        public string Demo { get; set; }
        public int Id { get; set; }
        public int TaxGrouId { get; set; }
        [Required(ErrorMessage = "*")]
        public string TaxName { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal TaxValue { get; set; }
        public List<ServiceTaxGroupvalesDetailsModel> ServiceTaxValueinfoList = new List<ServiceTaxGroupvalesDetailsModel>();
    }

    //Start Manage Quotation Type
    public class QuotationStatusModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string QuotationStatusName { get; set; }
    }

    public class ServiceDeatilsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string ServiceCode { get; set; }
        [Required(ErrorMessage = "*")]
        public string ServiceDescription { get; set; }
        [Required(ErrorMessage = "*")]
        public string Rate { get; set; }
    }

    //Start Customer Details....
    public class CustomerDetailsModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string CustomerName { get; set; }
        public string SvampowerCustomer { get; set; }
       
        public string MobNo { get; set; }
        public string MobNoAlternate { get; set; }
        [Required(ErrorMessage = "*")]
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }
        public string DiscountAllowed { get; set; }
        public string Taxpercent { get; set; }
        public string TinNo { get; set; }
        public string PanNo { get; set; }
        public string ServiceTaxNo { get; set; }
        public string Segment { get; set; }
        public string Remarks { get; set; }
        public string BillingAddress { get; set; }
    }
}