using System.Collections.Generic;

namespace Svam.Models
{
    public class ViewPOModel
    {
        public int Id { get; set; }
        public string PONo { get; set; }
        public string TransactionType { get; set; }
        public string PRno { get; set; }
        public string DepartMent { get; set; }
        public string PoDate { get; set; }
        public string PrDate { get; set; }
        public string RequesterName { get; set; }
        public string SupplierName { get; set; }
        public string SupllierAddress { get; set; }     

        public string FreightPercent { get; set; }
        public string FreightAmt { get; set; }
        public string TaxGroupTextName { get; set; }
        public string TaxGroupAmt { get; set; }
        public string ItmSubTotalTaxableAmt { get; set; }  
        public List<ViewPOModel> viewPomodelList = new List<ViewPOModel>();
        public List<ViewPOitemdetailModel> viewpoItmDetailListmodel = new List<ViewPOitemdetailModel>();
    }

    public class ViewPOitemdetailModel
    {
        public int Id { get; set; }
        public string PONo { get; set; }
        public string ItemName { get; set; }
        public string PartNo { get; set; }
        public string UnitName { get; set; }
        public string Qty { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
    }
}