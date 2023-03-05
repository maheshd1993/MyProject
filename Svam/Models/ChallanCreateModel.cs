using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{

    public class ChallanCreateModel
    {
        public int ChlnId { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
        public string QuotationNo { get; set; }
        public string QID { get; set; }
        public string QuotationDate { get; set; }
        public string QuotationTypeId { get; set; }
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


        public List<ChallanItemDetailsModel> ChallanItemDetailsModelList { get; set; }

    }

    public class ChallanItemDetailsModel
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
        public string hdRemoveID { get; set; }
    }

    // View Challan Model........
    public class ViewChallanModel
    {
        public int ChlnId { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
        public string QID { get; set; }
        public string QuotationNo { get; set; }
        public string QuotationDate { get; set; }
        public string QuotationTypeId { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTinNo { get; set; }
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

        public List<ViewChallanModel> viewchallanModelList = new List<ViewChallanModel>();
        public List<ViewChallanItemDetailsModel> viewChallanItemDetailModelList = new List<ViewChallanItemDetailsModel>();
    }

    public class ViewChallanItemDetailsModel
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
        public string ItemName { get; set; }
        public string UnitName { get; set; }
        public string ChallanNo { get; set; }
        public string hdRemoveID { get; set; }
        public string HSNCode { get; set; }
    }

}