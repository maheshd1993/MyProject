using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class APITrackSalePerson
    {
        public Int32? UserID { get; set; }
        public string Address { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int? CompanyID { get; set; }
        public string Token { get; set; }
    }
}