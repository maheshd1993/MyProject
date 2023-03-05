using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class ViewSalesReturnModel
    {
        public int Id { get; set; }
        public string SaleReturnNo { get; set; }
        public string SaleReturnDate { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string FreightAmt { get; set; }
        public string TaxGroupTextName { get; set; }
        public string TaxGroupApplyAmt { get; set; }
        public string SubtotalAmt { get; set; }

        public List<ViewSalesReturnModel> ViewSalesReturnModelList = new List<ViewSalesReturnModel>();
        public List<ViewSalesReturnitemdetailModel> ViewSalesReturnitemdetailModelList = new List<ViewSalesReturnitemdetailModel>();

    }
    public class ViewSalesReturnitemdetailModel
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


    //Start To create Sales Return ...
    public class CreateSaleReturnModel
    {

        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SaleReturnNo { get; set; }
        public string SaleReturnDate { get; set; }
        public string SaleOrderNo { get; set; }
        public string SaleOrderDate { get; set; }
        public string ItmFreightPercentage { get; set; }
        public string ApplyItmTaxGID { get; set; }
        public string DiscountPercent { get; set; }
        public string DiscountAmt { get; set; }
        public string HandlingChargePercent { get; set; }
        public string HandlingChargeAmt { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string ItmTaxGroupName { get; set; }
        public string TaxGroupPercent { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }
        public string SubtotalAmt { get; set; }
        public List<CreateSaleReturnItemDetailModel> CreateSaleReturnItemDetailModelList { get; set; }
    }

    public class CreateSaleReturnItemDetailModel
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
        public string ReurnQty { get; set; }
        public string ReturnStatus { get; set; }
    }
}