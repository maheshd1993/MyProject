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
    
    public partial class masterdump
    {
        public int Code { get; set; }
        public short MasterType { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string PrintName { get; set; }
        public int ParentGrp { get; set; }
        public Nullable<int> Stamp { get; set; }
        public Nullable<short> Level { get; set; }
        public Nullable<short> SrNo { get; set; }
        public Nullable<bool> External { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string MasterNotes { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreationTime { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModificationTime { get; set; }
        public string AuthorisedBy { get; set; }
        public Nullable<System.DateTime> AuthorisationTime { get; set; }
        public string HSNCode { get; set; }
        public string SENO { get; set; }
        public Nullable<decimal> OpBalCr { get; set; }
        public string OpBaltype { get; set; }
        public string RefNumber { get; set; }
        public Nullable<int> NatureOfGroup { get; set; }
        public string branch { get; set; }
        public string companyid { get; set; }
        public string flag { get; set; }
        public string SyncID { get; set; }
    }
}
