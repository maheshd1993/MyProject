using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class TrackSalePersonModel
    {
        public Int64? TrackID { get; set; }
        public Int32? UserID { get; set; }
        public String UserName { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String StateName { get; set; }
        public String CityName { get; set; }
        public String Latitude { get; set; }
        public String Longitude { get; set; }
        public DateTime? TrackDatetime { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateFormat { get; set; }
        public List<TrackSalePersonModel> TrackSalePersonList { get; set; }
        public List<TrackSalePersonModel> ViewMapSalePersonList { get; set; }
        public List<TrackSalePersonModel> SaleUserList { get; set; }
        public int CountMapUser { get; set; }

        public String lat { get; set; }
        public String lng { get; set; }
        public String description { get; set; }
        
    }
}