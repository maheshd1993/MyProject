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
    
    public partial class website_clients
    {
        public long id { get; set; }
        public long companies_id { get; set; }
        public long branch_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string img_name { get; set; }
        public System.DateTime updated_at { get; set; }
        public System.DateTime created_at { get; set; }
        public long created_by { get; set; }
        public long updated_by { get; set; }
        public byte status { get; set; }
        public Nullable<sbyte> img_type { get; set; }
        public string flag { get; set; }
    }
}
