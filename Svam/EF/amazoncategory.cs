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
    
    public partial class amazoncategory
    {
        public int ID { get; set; }
        public string CategoryID { get; set; }
        public string CategoryLevel { get; set; }
        public string CategoryName { get; set; }
        public string CategoryParentID { get; set; }
        public string CategoryNamePath { get; set; }
        public string CategoryIDPath { get; set; }
        public string LeafCategory { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string IsActive { get; set; }
        public string SyncID { get; set; }
        public string flag { get; set; }
    }
}
