using System.Collections.Generic;

namespace Svam.Models
{
    public class PrintQuotationModel
    {
        public string QuotationNo { get; set; }
        public string QuotationType { get; set; }
        public string RevisionNo { get; set; }
        public string QuotationDate { get; set; }
        public string CustomerName { get; set; }
        public string LeadTitle { get; set; }
        public string ContactNo { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string QuotationStatus { get; set; }
        public string SalesPerson { get; set; }
        public string SourceBranch { get; set; }
        public string Remarks { get; set; }

        //Item Details Taxation.....Properties.
        public string DiscountPercent { get; set; }
        public string DiscountAmt { get; set; }
        public string HandlingPercent { get; set; }
        public string HandlingAmt { get; set; }
        public string FreightPercent { get; set; }
        public string FreightAmt { get; set; }
        public string TaxGroupTextNameText { get; set; }
        public string TaxGroupAmt { get; set; }
        public string ItmSubTotalTaxableAmt { get; set; }

        //Service Details Taxation.....Properties.
        public string SDDiscountPercent { get; set; }
        public string SDDiscountAmt { get; set; }
        public string SDServiceTaxTextName { get; set; }
        public string SDServiceTaxPercent { get; set; }
        public string SDServiceTaxAmt { get; set; }
        public string SDTotalAmt { get; set; }

        //Final........
        public string GrandTotalAmt { get; set; }
        public string AmountInWord { get; set; }
        public string CustomerPONO { get; set; }
        public string CustomerAssignPORefDate { get; set; }

        public List<PrintItemDetailsModel> printItemDetailsModelList = new List<PrintItemDetailsModel>();
        public List<PrintServiceDetailsModel> printServiceDetailsModelList = new List<PrintServiceDetailsModel>();

    }

    public class PrintItemDetailsModel
    {
        public string Itemid { get; set; }
        public bool BillableChk { get; set; }
        public string PartNo { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
    }

    public class PrintServiceDetailsModel
    {
        public string sid { get; set; }
        public string ServicePartNo { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
    }
}