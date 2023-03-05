using System.Collections.Generic;

namespace Svam.Models
{
    public class ViewQuotationModel
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string QuotationNo { get; set; }
        public string RevisionNo { get; set; }
        public string QuotationDate { get; set; }
        public string CustomerName { get; set; }
        public string SalesPerson { get; set; }
        public string GrandTotal { get; set; }
        public decimal TotalTaxAmt { get; set; }
        public string QuotationType { get; set; }
        public string QuotationStatus { get; set; }
        public bool Editable { get; set; }        

        public List<ViewQuotationModel> viewquotationlistModel = new List<ViewQuotationModel>();
    }
}