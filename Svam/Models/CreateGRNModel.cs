using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svam.Models
{
    public class CreateGRNModel
    {
        public int GId { get; set; }
        public string GrnNo { get; set; }
        public string PONo { get; set; }
        public string TransactionType { get; set; }
        public int DepartmentId { get; set; }
        public string PODate { get; set; }
        public string GRNDate { get; set; }
        public string RequesterId { get; set; }
        public string SupplierId { get; set; }
        [Required(ErrorMessage="*")]
        public string SupplierAddress { get; set; }

        //Taxation properties.........
        public string ItmDiscountPer { get; set; }
        public string ItmDiscountAmt { get; set; }
        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string productApplyTaxesText { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }


        public List<CreateGRNItemModel> CreateGRNItemdetailModelList { get; set; }
    }
    public class CreateGRNItemModel
    {
        public string PartNo { get; set; }
        public string Itemid { get; set; }
        [Required(ErrorMessage = "*")]
        public string Qty { get; set; }
        public string Rate { get; set; }
        [Required(ErrorMessage = "*")]
        public string ReceiveQty { get; set; }
        [Required(ErrorMessage = "*")]
        public string Amount { get; set; }

        //...............Other Use
        public string ItemName { get; set; }
        public string Unitname { get; set; }
        public string SKU { get; set; }
    }


    public class ViewGrnModel
    {
        public Int64 GID { get; set; }
        public string GrnNo { get; set; }
        public string GRNDate { get; set; }
        public string PONo { get; set; }
        public string TransactionType { get; set; }
        public string Department { get; set; }
        public string PODate { get; set; }
        public string RequesterName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }

        public string ItmFreightPercentage { get; set; }
        public string ItmFreightChargeAmt { get; set; }
        public string productApplyTaxesText { get; set; }
        public string ItmTaxGroupAmt { get; set; }
        public string ItmTotalTaxAmt { get; set; }

        public List<ViewGrnModel> viewgrnmodelList = new List<ViewGrnModel>();

        public List<CreateGRNItemModel> grnItemDetailsList = new List<CreateGRNItemModel>();
        
    }
}