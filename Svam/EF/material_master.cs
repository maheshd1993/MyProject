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
    
    public partial class material_master
    {
        public int id { get; set; }
        public string materialid { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> Quanity { get; set; }
        public string Item_to_produce { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Expenses { get; set; }
        public string Specify_Default_Mc_For_Item_Generated { get; set; }
        public string Specify_Default_Mc_For_Item_Consumed { get; set; }
        public string Row_material_consumed_itemssku { get; set; }
        public string Row_material_consumed_qty { get; set; }
        public string Row_material_consumed_unit { get; set; }
        public string ByproductGenerated_itemssku { get; set; }
        public string ByproductGenerated_qty { get; set; }
        public string ByproductGenerated_unit { get; set; }
        public Nullable<int> CompanyID { get; set; }
        public Nullable<int> BranchCode { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
    }
}
