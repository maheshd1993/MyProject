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
    
    public partial class blog_sub_category
    {
        public int id { get; set; }
        public string cat_id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public int company_id { get; set; }
        public int branch { get; set; }
        public System.DateTime created { get; set; }
        public int status { get; set; }
    }
}