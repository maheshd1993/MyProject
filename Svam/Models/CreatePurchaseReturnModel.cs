using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class ViewPurchaseReturnModel
    {
        public int Id { get; set; }
        public string PurchaseReturnNo { get; set; }
        public string PreturnDate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string SupplierName { get; set; }
        public string FreightAmt { get; set; }
        public string TaxGroupTextName { get; set; }
        public string TaxGroupApplyAmt { get; set; }
        public string SubtotalAmt { get; set; }

        public List<ViewPurchaseReturnModel> ViewPurchaseReturnModelList = new List<ViewPurchaseReturnModel>();
        public List<ViewPurchaseReturnitemdetailModel> ViewPurchaseReturnitemdetailModelList = new List<ViewPurchaseReturnitemdetailModel>();

    }
    public class ViewPurchaseReturnitemdetailModel
    {
        public int Id { get; set; }
        public string PONo { get; set; }
        public string ItemName { get; set; }
        public string PartNo { get; set; }
        public string UnitName { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string ItemStatus { get; set; }

    }





    //Start Here to create the PurchaseReturn.......
    public class CreatePurchaseReturnModel
    {
        [Required(ErrorMessage = "*")]
        public string PReturnNo { get; set; }
        [Required(ErrorMessage = "*")]
        public string PReturnDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string PurchaseOrderNo { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string SupplierId { get; set; }

        //For sub-total item details..
        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string ItmTaxGroupName { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string SubtotalAmt { get; set; }

        public List<CreatePurchaseReturnItemDetailsModel> createPurchaseReturnItemDetailsModelList { get; set; }
    }

    public class CreatePurchaseReturnItemDetailsModel
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
        [Required(ErrorMessage = "*")]
        public string ReturnQty { get; set; }
        public string ReturnStatus { get; set; }
    }
}