using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class CRMTargetSaleModel
    {
         public int? TargetID { get; set; }
        [Required(ErrorMessage = "Please select sale person")]
        public int? SalePersonID { get; set; }
        public string SalePersonName { get; set; }
        public string DateFormat  { get; set; }
        [Required(ErrorMessage = "Please select from date")]
        public string FromDate { get; set; }
        public string sFromDate { get; set; }
        [Required(ErrorMessage = "Please select to date")]
        public string ToDate { get; set; }
        public string sToDate { get; set; }
        public string Language { get; set; }
        public DateTime CREATEDDATE { get; set; }
        public DateTime? sCREATEDDATE { get; set; }
        [Required(ErrorMessage = "Please enter Target Amount")]
        public decimal? TotalTargetAmount { get; set; }
        public decimal? TargetAchieveAmount { get; set; }
        public decimal? CurentTargetAdvanceAmount { get; set; }
        public decimal? CurentTargetAchieveAmount { get; set; }

        public decimal? ProjectValueDateGroupBy  { get; set; }

        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }

        public string MappedUser { get; set; }
        public List<CRMTargetSaleModel> oSalePersonList { get; set; }
        public List<CRMTargetSaleModel> oSaleTargetModelList { get; set; }
        public List<CRMTargetSaleModel> oSaleTargetClosedModelList { get; set; }
        //public List<StackedChartData> GetChartData { get; set; }

        public List<ChartModel> GetChartData { get; set; }

        public string FromToDate { get; set; }

        public decimal? SalePercentage { get; set; }
    }

    public class ChartModel 
    {

        public ChartModel(string label, double? y) 
        {
            this.label = label;
            this.y = y;
        }
        //public DateTime x { get; set; }
        public string label { get; set; }
        public double? y { get; set; }
        
       

    }


}