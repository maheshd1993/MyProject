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
    
    public partial class customizedinvoiceformate
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public int BranchCode { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceName { get; set; }
        public string HInvoiceText { get; set; }
        public string HPhoneNumber { get; set; }
        public string HCompanyName { get; set; }
        public string HCompanyAddress { get; set; }
        public string HCompanyEmail { get; set; }
        public string HCompanyLogo { get; set; }
        public byte[] HCompanyLogoImage { get; set; }
        public string HCompanyOther1 { get; set; }
        public string HCompanyOther2 { get; set; }
        public string HCompanyOther3 { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string CustMobileNumber { get; set; }
        public string CustGSTIN { get; set; }
        public string CustAadharNo { get; set; }
        public string CustPanNo { get; set; }
        public string CustOther1 { get; set; }
        public string CustOther2 { get; set; }
        public string CustOther3 { get; set; }
        public string CustOther4 { get; set; }
        public string InvoiceNo { get; set; }
        public string PlaceOfSupply { get; set; }
        public string BillDate { get; set; }
        public string TransporterName { get; set; }
        public string VehicalNo { get; set; }
        public string DeleveryDate { get; set; }
        public string ISrNo { get; set; }
        public string IDescriptionOfGoods { get; set; }
        public string IDeleveryDate { get; set; }
        public string IHSNCode { get; set; }
        public string IMRP { get; set; }
        public string IQuantity { get; set; }
        public string IDiscountPercentage { get; set; }
        public string IDiscountAmount { get; set; }
        public string IGrossAmount { get; set; }
        public string DetailedTaxDescription { get; set; }
        public string TaxRate { get; set; }
        public string TaxableAmount { get; set; }
        public string CSGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string TotalTax { get; set; }
        public string GrossTotalTax { get; set; }
        public string NetTotalAmount { get; set; }
        public string NetTotalDiscount { get; set; }
        public string NetRoundOff { get; set; }
        public string NetFreightCharges { get; set; }
        public string NetAmountPayable { get; set; }
        public string NetReceived { get; set; }
        public string NetBalance { get; set; }
        public string RupeesInWords { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankIFSCCode { get; set; }
        public string TermAndCondition { get; set; }
        public string TermAndConditionLine1 { get; set; }
        public string TermAndConditionLine2 { get; set; }
        public string TermAndConditionLine3 { get; set; }
        public string TermAndConditionLine4 { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Flag { get; set; }
        public string SyncID { get; set; }
        public string BankNameShow { get; set; }
        public string IfscCode { get; set; }
        public string AccountNumber { get; set; }
        public string BankAddress { get; set; }
        public string Remark { get; set; }
        public string RemarkStatus { get; set; }
        public string ShowCompanyHeader { get; set; }
        public string ShowLedger { get; set; }
        public string ShowBankDetails { get; set; }
        public string ShowColor { get; set; }
        public string ShowSize { get; set; }
        public string RGBColourCode { get; set; }
        public string OnlineOrderNo { get; set; }
        public string grrno { get; set; }
        public string t_date { get; set; }
        public string station_place { get; set; }
        public string e_way_no { get; set; }
        public string consignee_name { get; set; }
        public string consignee_sales_person { get; set; }
        public string consignee_address { get; set; }
        public string consignee_mobileno { get; set; }
        public string consignee_mode_of_payment { get; set; }
        public string consignee_aadharno { get; set; }
        public string consignee_panno { get; set; }
        public string consignee_gstno { get; set; }
        public string category_name { get; set; }
        public string our_price { get; set; }
        public string ITotalQuantity { get; set; }
        public string NetTransportationCharges { get; set; }
        public string OtherCharge { get; set; }
        public string NetAmountReceivable { get; set; }
        public string AmountBeforeTax { get; set; }
        public string RackNo { get; set; }
        public string ExpiryDate { get; set; }
        public string ImeiNumber { get; set; }
        public string BatchNumber { get; set; }
        public string GSTRBilling { get; set; }
    }
}