using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMPaymentModel
    {
        public Int64? PaymentID { get; set; }
        public String BillNumber { get; set; }
      
        public Decimal? BalanceAmount { get; set; }
        public DateTime? BillDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string PaymentMode { get; set; }
        public String PaymentStatus { get; set; }
        public Int32? CustomerID { get; set; }
        public String CustomerName { get; set; }
        public string DateFormat { get; set; }
        public Int32? UserID { get; set; }
        public String UserName{ get; set; }

        public List<CRMPaymentModel> CRMPaymentModelList { get; set; }
    }
}