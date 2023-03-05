using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class TimeZoneDTO
    {
        public int Id  { get; set; }
        public string ZoneName { get; set; }
        public string Time { get; set; }
        public string DateFormat  { get; set; }
    }
}