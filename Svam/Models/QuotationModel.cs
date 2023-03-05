using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class QuotationModel
    {
        public Nullable<Int32> QuotationID { get; set; }

        [Required(ErrorMessage = "* Please Enter First Name")]
        public Nullable<Int32> CustomerID { get; set; }
        public String CustomerName { get; set; }

        [Required(ErrorMessage = "* Please mobile Number")]
        public String MobileNumber { get; set; }

        [Required(ErrorMessage = "* Please mobile Number")]
        public String Address { get; set; }

        [Required(ErrorMessage = "* Please select state ")]
        public Nullable<Int32> StateID { get; set; }
        public String StateName { get; set; }

        [Required(ErrorMessage = "* Please enter bill date ")]
        public Nullable<DateTime> BillDate { get; set; }
        [Required(ErrorMessage = "* Please enter delivery date ")]
        public Nullable<DateTime> DeliveryDate { get; set; }

        public Nullable<Decimal> CreditLimit { get; set; }
        public Nullable<Decimal> Dues { get; set; }
        public String BillNumber { get; set; }

        public String ItemID { get; set; }
        public String ItemName { get; set; }

        public Nullable<Int32> UnitID { get; set; }
        public String UnitName { get; set; }

        public Nullable<Int32> AltUnitID { get; set; }
        public String AltUnitName { get; set; }

        public String SizeID { get; set; }
        public String SizeName { get; set; }

        public String ColorID { get; set; }
        public String ColorName { get; set; }
        public decimal GstPercent { get; set; }
        public decimal GstAmount { get; set; }
        public string DiscountType { get; set; }
        public Nullable<Decimal> CurrentStock { get; set; }
        public String ItemCode { get; set; }
        public Nullable<Decimal> Qty { get; set; }
        public Nullable<Decimal> TotalQty { get; set; }
        public Nullable<Decimal> OurPrice { get; set; }
        public Nullable<Decimal> MRP { get; set; }
        public Nullable<Decimal> DiscountPercent { get; set; }
        public Nullable<Decimal> DiscountAmount { get; set; }
        
        public Nullable<Decimal> AfterAmountDiscount { get; set; }

        public Nullable<Decimal> FinalTotalQty { get; set; }
        public Nullable<Decimal> FinalTotalAmount{ get; set; }
        public Nullable<Decimal> FinalExtraDiscPercent { get; set; }
        public Nullable<Decimal> FreightCharge { get; set; }
        public Nullable<Decimal> FinalTotalDiscount { get; set; }
        public Nullable<Decimal> GSTTotal { get; set; }
        public Nullable<Decimal> Receivable { get; set; }
        public Nullable<Decimal> BalanceAmount { get; set; }
        public Nullable<Decimal> GrandTotal { get; set; }

        public Nullable<Int32> CompanyID { get; set; }
        public Nullable<Int32> BranchID { get; set; }
        public string TaxType { get; set; }
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxMethod  { get; set; }
        public decimal? TaxPer  { get; set; }
        public string Pincode { get; set; }
        public List<QuotationModel> oQuotationModel { get; set; }
        public List<QuotationModel> oCustomerList { get; set; }
        public List<QuotationModel> oStateList { get; set; }
        public List<QuotationModel> oItemList { get; set; }
        public List<QuotationModel> oUnitList { get; set; }
        public List<QuotationModel> oAltUnitList { get; set; }
        public List<QuotationModel> oSizeList { get; set; }
        public List<QuotationModel> oColorList { get; set; }
        public List<string> saleTypes { get; set; }
        public List<TaxPerMaster> TaxDetails { get; set; }
    }

    public class TaxPerMaster
    {       
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public string TaxMethod { get; set; }
        public decimal? TaxPer { get; set; }
    }
}