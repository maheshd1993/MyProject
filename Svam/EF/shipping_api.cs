//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Svam.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class shipping_api
    {
        public int id { get; set; }
        public string api_type { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string channel_id { get; set; }
        public string CompanyID { get; set; }
        public string BranchCode { get; set; }
        public string for_company { get; set; }
        public string pickup_location { get; set; }
        public string company_pickup_pincode { get; set; }
        public int status { get; set; }
    }
}
