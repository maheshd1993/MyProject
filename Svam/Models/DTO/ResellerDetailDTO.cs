using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Svam.Models.DTO
{
    public class ResellerDetailDTO
    {
        public int ResellerId { get; set; }
        public string ResellerName   { get; set; }
        public string ResellerContactNo { get; set; }
        public string ResellerCode  { get; set; }
        public string ResellerStatus { get; set; }
    }
}