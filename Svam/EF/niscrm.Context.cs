﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class niscrmEntities : DbContext
    {
        public niscrmEntities()
            : base("name=niscrmEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<acc_accountingrulebook> acc_accountingrulebook { get; set; }
        public virtual DbSet<acc_accountingrulereferences> acc_accountingrulereferences { get; set; }
        public virtual DbSet<acc_accountsubtypes> acc_accountsubtypes { get; set; }
        public virtual DbSet<acc_accounttypes> acc_accounttypes { get; set; }
        public virtual DbSet<acc_advancepayment> acc_advancepayment { get; set; }
        public virtual DbSet<acc_advancereceipt> acc_advancereceipt { get; set; }
        public virtual DbSet<acc_countries> acc_countries { get; set; }
        public virtual DbSet<acc_countriesold> acc_countriesold { get; set; }
        public virtual DbSet<acc_expenses> acc_expenses { get; set; }
        public virtual DbSet<acc_journalvoucherlines> acc_journalvoucherlines { get; set; }
        public virtual DbSet<acc_journalvouchers> acc_journalvouchers { get; set; }
        public virtual DbSet<acc_tallyexports> acc_tallyexports { get; set; }
        public virtual DbSet<active> actives { get; set; }
        public virtual DbSet<address> addresses { get; set; }
        public virtual DbSet<addresstype> addresstypes { get; set; }
        public virtual DbSet<addto_wishlist> addto_wishlist { get; set; }
        public virtual DbSet<all_menu> all_menu { get; set; }
        public virtual DbSet<all_menu_17july19> all_menu_17july19 { get; set; }
        public virtual DbSet<all_menu_23july19> all_menu_23july19 { get; set; }
        public virtual DbSet<all_menu_30jan> all_menu_30jan { get; set; }
        public virtual DbSet<all_menu13march21> all_menu13march21 { get; set; }
        public virtual DbSet<allocate> allocates { get; set; }
        public virtual DbSet<amazoncategory> amazoncategories { get; set; }
        public virtual DbSet<amazonproduct> amazonproducts { get; set; }
        public virtual DbSet<amazonrootcategory> amazonrootcategories { get; set; }
        public virtual DbSet<ast_assetdepreciations> ast_assetdepreciations { get; set; }
        public virtual DbSet<ast_assetmaintenance> ast_assetmaintenance { get; set; }
        public virtual DbSet<attributes_label> attributes_label { get; set; }
        public virtual DbSet<bank_master> bank_master { get; set; }
        public virtual DbSet<barcodesetting> barcodesettings { get; set; }
        public virtual DbSet<barcodewholesale> barcodewholesales { get; set; }
        public virtual DbSet<blog> blogs { get; set; }
        public virtual DbSet<blog_category> blog_category { get; set; }
        public virtual DbSet<blog_sub_category> blog_sub_category { get; set; }
        public virtual DbSet<cashbankbook> cashbankbooks { get; set; }
        public virtual DbSet<cashbankbookcustomer> cashbankbookcustomers { get; set; }
        public virtual DbSet<chalandet> chalandets { get; set; }
        public virtual DbSet<chalandetblack> chalandetblacks { get; set; }
        public virtual DbSet<challan_returninwardlines> challan_returninwardlines { get; set; }
        public virtual DbSet<challan_returntaxdetails> challan_returntaxdetails { get; set; }
        public virtual DbSet<challan_setting> challan_setting { get; set; }
        public virtual DbSet<challan_taxdetails> challan_taxdetails { get; set; }
        public virtual DbSet<challanamst> challanamsts { get; set; }
        public virtual DbSet<challanamstblack> challanamstblacks { get; set; }
        public virtual DbSet<challanorder> challanorders { get; set; }
        public virtual DbSet<ci_session> ci_session { get; set; }
        public virtual DbSet<ci_session_temp> ci_session_temp { get; set; }
        public virtual DbSet<ci_sessions> ci_sessions { get; set; }
        public virtual DbSet<colour_name> colour_name { get; set; }
        public virtual DbSet<com_branches> com_branches { get; set; }
        public virtual DbSet<com_city> com_city { get; set; }
        public virtual DbSet<com_city_live> com_city_live { get; set; }
        public virtual DbSet<com_countrymaster> com_countrymaster { get; set; }
        public virtual DbSet<com_departments> com_departments { get; set; }
        public virtual DbSet<com_measurementunits> com_measurementunits { get; set; }
        public virtual DbSet<com_state> com_state { get; set; }
        public virtual DbSet<com_taskstatuscodes> com_taskstatuscodes { get; set; }
        public virtual DbSet<com_vehicles> com_vehicles { get; set; }
        public virtual DbSet<company_profile> company_profile { get; set; }
        public virtual DbSet<consignee> consignees { get; set; }
        public virtual DbSet<contactu> contactus { get; set; }
        public virtual DbSet<createprintorder> createprintorders { get; set; }
        public virtual DbSet<crm_assignedtootherorganization> crm_assignedtootherorganization { get; set; }
        public virtual DbSet<crm_assignitemtypeuser> crm_assignitemtypeuser { get; set; }
        public virtual DbSet<crm_commonactivityremarktbl> crm_commonactivityremarktbl { get; set; }
        public virtual DbSet<crm_create_lead_field_sequence> crm_create_lead_field_sequence { get; set; }
        public virtual DbSet<crm_createleadsetting> crm_createleadsetting { get; set; }
        public virtual DbSet<crm_createleadstbl> crm_createleadstbl { get; set; }
        public virtual DbSet<crm_customizedformfieldtextname> crm_customizedformfieldtextname { get; set; }
        public virtual DbSet<crm_delayedfollowuprecordtbl> crm_delayedfollowuprecordtbl { get; set; }
        public virtual DbSet<crm_emailsetting> crm_emailsetting { get; set; }
        public virtual DbSet<crm_emailtemplate> crm_emailtemplate { get; set; }
        public virtual DbSet<crm_errortype> crm_errortype { get; set; }
        public virtual DbSet<crm_exceptionlogging> crm_exceptionlogging { get; set; }
        public virtual DbSet<crm_extrapayment> crm_extrapayment { get; set; }
        public virtual DbSet<crm_fbleadadsmaster> crm_fbleadadsmaster { get; set; }
        public virtual DbSet<crm_filemanager> crm_filemanager { get; set; }
        public virtual DbSet<crm_indiamartsetting> crm_indiamartsetting { get; set; }
        public virtual DbSet<crm_intervewstatus> crm_intervewstatus { get; set; }
        public virtual DbSet<crm_interviewdescriptiontbl> crm_interviewdescriptiontbl { get; set; }
        public virtual DbSet<crm_interviewscheduletbl> crm_interviewscheduletbl { get; set; }
        public virtual DbSet<crm_itemtypetbl> crm_itemtypetbl { get; set; }
        public virtual DbSet<crm_jobprofiletbl> crm_jobprofiletbl { get; set; }
        public virtual DbSet<crm_leadassignhistorytbl> crm_leadassignhistorytbl { get; set; }
        public virtual DbSet<crm_leaddescriptiontbl> crm_leaddescriptiontbl { get; set; }
        public virtual DbSet<crm_leaddropdownlist_tbl> crm_leaddropdownlist_tbl { get; set; }
        public virtual DbSet<crm_leadreminderbyuser> crm_leadreminderbyuser { get; set; }
        public virtual DbSet<crm_leadrequesttbl> crm_leadrequesttbl { get; set; }
        public virtual DbSet<crm_leadsource_tbl> crm_leadsource_tbl { get; set; }
        public virtual DbSet<crm_leadstatus_tbl> crm_leadstatus_tbl { get; set; }
        public virtual DbSet<crm_leadsupdatetblhistory> crm_leadsupdatetblhistory { get; set; }
        public virtual DbSet<crm_leaverequest_tbl> crm_leaverequest_tbl { get; set; }
        public virtual DbSet<crm_formrequest_tbl> crm_formrequest_tbl { get; set; }
        public virtual DbSet<crm_leavetypename> crm_leavetypename { get; set; }
        public virtual DbSet<crm_mappeduserotherbranch> crm_mappeduserotherbranch { get; set; }
        public virtual DbSet<crm_newseventtbl> crm_newseventtbl { get; set; }
        public virtual DbSet<crm_producttypetbl> crm_producttypetbl { get; set; }
        public virtual DbSet<crm_projectmoduletbl> crm_projectmoduletbl { get; set; }
        public virtual DbSet<crm_projectstatus_tbl> crm_projectstatus_tbl { get; set; }
        public virtual DbSet<crm_projecttbl> crm_projecttbl { get; set; }
        public virtual DbSet<crm_roleassigntbl> crm_roleassigntbl { get; set; }
        public virtual DbSet<crm_salarydetail> crm_salarydetail { get; set; }
        public virtual DbSet<crm_salaryhistory> crm_salaryhistory { get; set; }
        public virtual DbSet<crm_saledetailtbl> crm_saledetailtbl { get; set; }
        public virtual DbSet<crm_salestarget> crm_salestarget { get; set; }
        public virtual DbSet<crm_tbl_nipl_emp_attendance> crm_tbl_nipl_emp_attendance { get; set; }
        public virtual DbSet<crm_tbl_nipldeveloper_dailyreort> crm_tbl_nipldeveloper_dailyreort { get; set; }
        public virtual DbSet<crm_termcondition> crm_termcondition { get; set; }
        public virtual DbSet<crm_tickestmap> crm_tickestmap { get; set; }
        public virtual DbSet<crm_ticket_field_sequence> crm_ticket_field_sequence { get; set; }
        public virtual DbSet<crm_ticketcreatesetting> crm_ticketcreatesetting { get; set; }
        public virtual DbSet<crm_ticketdropdownlist_tbl> crm_ticketdropdownlist_tbl { get; set; }
        public virtual DbSet<crm_ticketescalationmaster> crm_ticketescalationmaster { get; set; }
        public virtual DbSet<crm_ticketfieldnamecustomized> crm_ticketfieldnamecustomized { get; set; }
        public virtual DbSet<crm_ticketfilesimages> crm_ticketfilesimages { get; set; }
        public virtual DbSet<crm_ticketremarkforteam> crm_ticketremarkforteam { get; set; }
        public virtual DbSet<crm_tickets> crm_tickets { get; set; }
        public virtual DbSet<crm_ticketviewsetting> crm_ticketviewsetting { get; set; }
        public virtual DbSet<crm_tracksaleperson> crm_tracksaleperson { get; set; }
        public virtual DbSet<crm_urgency> crm_urgency { get; set; }
        public virtual DbSet<crm_usercompanytypetbl> crm_usercompanytypetbl { get; set; }
        public virtual DbSet<crm_userdocuments> crm_userdocuments { get; set; }
        public virtual DbSet<crm_userprofile> crm_userprofile { get; set; }
        public virtual DbSet<crm_userprofiles> crm_userprofiles { get; set; }
        public virtual DbSet<crm_usertbl> crm_usertbl { get; set; }
        public virtual DbSet<crm_viewleadsetting> crm_viewleadsetting { get; set; }
        public virtual DbSet<crm_workassigntbl> crm_workassigntbl { get; set; }
        public virtual DbSet<crm_workdescriptiontbl> crm_workdescriptiontbl { get; set; }
        public virtual DbSet<crm_zonetbl> crm_zonetbl { get; set; }
        public virtual DbSet<currentdesktopversion> currentdesktopversions { get; set; }
        public virtual DbSet<customer_address> customer_address { get; set; }
        public virtual DbSet<customer_ledger> customer_ledger { get; set; }
        public virtual DbSet<customercrdr> customercrdrs { get; set; }
        public virtual DbSet<customerpayinfo> customerpayinfoes { get; set; }
        public virtual DbSet<customerpayinfo_11jan2018> customerpayinfo_11jan2018 { get; set; }
        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<customers_sites> customers_sites { get; set; }
        public virtual DbSet<customersadvance> customersadvances { get; set; }
        public virtual DbSet<customersgroup> customersgroups { get; set; }
        public virtual DbSet<customersgroupdetail> customersgroupdetails { get; set; }
        public virtual DbSet<customertype> customertypes { get; set; }
        public virtual DbSet<customizedinvoiceformate> customizedinvoiceformates { get; set; }
        public virtual DbSet<dashboard> dashboards { get; set; }
        public virtual DbSet<departmentdetail> departmentdetails { get; set; }
        public virtual DbSet<discount_coupon> discount_coupon { get; set; }
        public virtual DbSet<discounted_offers> discounted_offers { get; set; }
        public virtual DbSet<dispatch_modes> dispatch_modes { get; set; }
        public virtual DbSet<division> divisions { get; set; }
        public virtual DbSet<einvoicesetup> einvoicesetups { get; set; }
        public virtual DbSet<emaildetail> emaildetails { get; set; }
        public virtual DbSet<emp_details> emp_details { get; set; }
        public virtual DbSet<emp_grade> emp_grade { get; set; }
        public virtual DbSet<expire> expires { get; set; }
        public virtual DbSet<faq_que_ans> faq_que_ans { get; set; }
        public virtual DbSet<faq_topic> faq_topic { get; set; }
        public virtual DbSet<fgstk> fgstks { get; set; }
        public virtual DbSet<fgstkline> fgstklines { get; set; }
        public virtual DbSet<flipkartcategory> flipkartcategories { get; set; }
        public virtual DbSet<flipkartproduct> flipkartproducts { get; set; }
        public virtual DbSet<gallery_group> gallery_group { get; set; }
        public virtual DbSet<goodsissuevoucher> goodsissuevouchers { get; set; }
        public virtual DbSet<goodsissuevoucherline> goodsissuevoucherlines { get; set; }
        public virtual DbSet<grnpaid_info> grnpaid_info { get; set; }
        public virtual DbSet<grnpaid_infoblack> grnpaid_infoblack { get; set; }
        public virtual DbSet<group> groups { get; set; }
        public virtual DbSet<gst_master> gst_master { get; set; }
        public virtual DbSet<interstoretransfer> interstoretransfers { get; set; }
        public virtual DbSet<interstoretransferline> interstoretransferlines { get; set; }
        public virtual DbSet<inv_addadjustment> inv_addadjustment { get; set; }
        public virtual DbSet<inv_adjust_type> inv_adjust_type { get; set; }
        public virtual DbSet<inv_billofmateriallines> inv_billofmateriallines { get; set; }
        public virtual DbSet<inv_fatmawamaster> inv_fatmawamaster { get; set; }
        public virtual DbSet<inv_inventorycentre> inv_inventorycentre { get; set; }
        public virtual DbSet<inv_inventoryrack> inv_inventoryrack { get; set; }
        public virtual DbSet<inv_inventorystock> inv_inventorystock { get; set; }
        public virtual DbSet<inv_inventorystock_remove> inv_inventorystock_remove { get; set; }
        public virtual DbSet<inv_inventorystock2> inv_inventorystock2 { get; set; }
        public virtual DbSet<inv_itebilltoragetypes> inv_itebilltoragetypes { get; set; }
        public virtual DbSet<inv_itembrand> inv_itembrand { get; set; }
        public virtual DbSet<inv_itemcategories> inv_itemcategories { get; set; }
        public virtual DbSet<inv_itemcategorytypes> inv_itemcategorytypes { get; set; }
        public virtual DbSet<inv_itempricecost> inv_itempricecost { get; set; }
        public virtual DbSet<inv_itempurchasealertsettings> inv_itempurchasealertsettings { get; set; }
        public virtual DbSet<inv_itemsku> inv_itemsku { get; set; }
        public virtual DbSet<inv_itemsku_4jan> inv_itemsku_4jan { get; set; }
        public virtual DbSet<inv_itemsku_prices> inv_itemsku_prices { get; set; }
        public virtual DbSet<inv_itemsku_remove> inv_itemsku_remove { get; set; }
        public virtual DbSet<inv_itemsku1> inv_itemsku1 { get; set; }
        public virtual DbSet<inv_itemstoragetypes> inv_itemstoragetypes { get; set; }
        public virtual DbSet<inv_itemsubcategories> inv_itemsubcategories { get; set; }
        public virtual DbSet<inv_itemsubsubcategories> inv_itemsubsubcategories { get; set; }
        public virtual DbSet<inv_itemtypes> inv_itemtypes { get; set; }
        public virtual DbSet<inv_stockadjustlog> inv_stockadjustlog { get; set; }
        public virtual DbSet<investor> investors { get; set; }
        public virtual DbSet<invoice_additionalformat> invoice_additionalformat { get; set; }
        public virtual DbSet<item_columns_access> item_columns_access { get; set; }
        public virtual DbSet<item_groups> item_groups { get; set; }
        public virtual DbSet<item_saleorder> item_saleorder { get; set; }
        public virtual DbSet<item_supplierpurchaseprices> item_supplierpurchaseprices { get; set; }
        public virtual DbSet<items_related_images> items_related_images { get; set; }
        public virtual DbSet<itemtype> itemtypes { get; set; }
        public virtual DbSet<jwl_addkarigar> jwl_addkarigar { get; set; }
        public virtual DbSet<jwl_inventorystock> jwl_inventorystock { get; set; }
        public virtual DbSet<jwl_itemcategories> jwl_itemcategories { get; set; }
        public virtual DbSet<jwl_itemsku> jwl_itemsku { get; set; }
        public virtual DbSet<jwl_itemsubcategories> jwl_itemsubcategories { get; set; }
        public virtual DbSet<jwl_itemsubsubcategories> jwl_itemsubsubcategories { get; set; }
        public virtual DbSet<key> keys { get; set; }
        public virtual DbSet<location> locations { get; set; }
        public virtual DbSet<login> logins { get; set; }
        public virtual DbSet<login_authentication> login_authentication { get; set; }
        public virtual DbSet<machine_rent> machine_rent { get; set; }
        public virtual DbSet<masterkeyversion> masterkeyversions { get; set; }
        public virtual DbSet<material_master> material_master { get; set; }
        public virtual DbSet<materialmasterexpens> materialmasterexpenses { get; set; }
        public virtual DbSet<menu_access> menu_access { get; set; }
        public virtual DbSet<multilabeldiscount> multilabeldiscounts { get; set; }
        public virtual DbSet<notification> notifications { get; set; }
        public virtual DbSet<online_addtocart> online_addtocart { get; set; }
        public virtual DbSet<online_cancel_orders> online_cancel_orders { get; set; }
        public virtual DbSet<online_orders> online_orders { get; set; }
        public virtual DbSet<online_return_orders> online_return_orders { get; set; }
        public virtual DbSet<orderitem> orderitems { get; set; }
        public virtual DbSet<orders_amazone> orders_amazone { get; set; }
        public virtual DbSet<orderupdatestatu> orderupdatestatus { get; set; }
        public virtual DbSet<partycrdr> partycrdrs { get; set; }
        public virtual DbSet<partycrdr_old11dec> partycrdr_old11dec { get; set; }
        public virtual DbSet<payment_gateways> payment_gateways { get; set; }
        public virtual DbSet<pincodewisetaxmaster> pincodewisetaxmasters { get; set; }
        public virtual DbSet<pmt_creditcardtypeslookup> pmt_creditcardtypeslookup { get; set; }
        public virtual DbSet<po_taxdetails> po_taxdetails { get; set; }
        public virtual DbSet<po_type> po_type { get; set; }
        public virtual DbSet<pogrn_taxdetails> pogrn_taxdetails { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<promotions_coupon> promotions_coupon { get; set; }
        public virtual DbSet<pur_pr> pur_pr { get; set; }
        public virtual DbSet<pur_prline> pur_prline { get; set; }
        public virtual DbSet<pur_purchasegoodsreceived> pur_purchasegoodsreceived { get; set; }
        public virtual DbSet<pur_purchasegoodsreceivedlines> pur_purchasegoodsreceivedlines { get; set; }
        public virtual DbSet<pur_purchasegrnorder> pur_purchasegrnorder { get; set; }
        public virtual DbSet<pur_purchasegrnorderlines> pur_purchasegrnorderlines { get; set; }
        public virtual DbSet<pur_purchaseorder> pur_purchaseorder { get; set; }
        public virtual DbSet<pur_purchaseorderlines> pur_purchaseorderlines { get; set; }
        public virtual DbSet<pur_purchaseordertypes> pur_purchaseordertypes { get; set; }
        public virtual DbSet<pur_return> pur_return { get; set; }
        public virtual DbSet<pur_returnlines> pur_returnlines { get; set; }
        public virtual DbSet<pur_returntaxdetails> pur_returntaxdetails { get; set; }
        public virtual DbSet<pur_supplierdeposit> pur_supplierdeposit { get; set; }
        public virtual DbSet<pur_suppliers> pur_suppliers { get; set; }
        public virtual DbSet<pur_suppliers_backup11sep> pur_suppliers_backup11sep { get; set; }
        public virtual DbSet<pur_suppliertypes> pur_suppliertypes { get; set; }
        public virtual DbSet<pur_taxdetails> pur_taxdetails { get; set; }
        public virtual DbSet<purchase_order_setting> purchase_order_setting { get; set; }
        public virtual DbSet<purchase_requisition_tbl> purchase_requisition_tbl { get; set; }
        public virtual DbSet<purchase_setting> purchase_setting { get; set; }
        public virtual DbSet<quotation_setting> quotation_setting { get; set; }
        public virtual DbSet<redeem_points> redeem_points { get; set; }
        public virtual DbSet<refrel_points> refrel_points { get; set; }
        public virtual DbSet<refunditem> refunditems { get; set; }
        public virtual DbSet<registration> registrations { get; set; }
        public virtual DbSet<route> routes { get; set; }
        public virtual DbSet<sal_discountrules> sal_discountrules { get; set; }
        public virtual DbSet<sal_paymentmode> sal_paymentmode { get; set; }
        public virtual DbSet<sal_quotaxdetails> sal_quotaxdetails { get; set; }
        public virtual DbSet<sal_returninwardlines> sal_returninwardlines { get; set; }
        public virtual DbSet<sal_returninwards> sal_returninwards { get; set; }
        public virtual DbSet<sal_returntaxdetails> sal_returntaxdetails { get; set; }
        public virtual DbSet<sal_salespersonmaster> sal_salespersonmaster { get; set; }
        public virtual DbSet<sal_sotaxdetails> sal_sotaxdetails { get; set; }
        public virtual DbSet<sal_taxdetails> sal_taxdetails { get; set; }
        public virtual DbSet<sale_finance> sale_finance { get; set; }
        public virtual DbSet<sale_finance_details> sale_finance_details { get; set; }
        public virtual DbSet<saleprice> saleprices { get; set; }
        public virtual DbSet<sales_order_setting> sales_order_setting { get; set; }
        public virtual DbSet<sales_setting> sales_setting { get; set; }
        public virtual DbSet<seo_setting> seo_setting { get; set; }
        public virtual DbSet<shipping_api> shipping_api { get; set; }
        public virtual DbSet<shipping_charge> shipping_charge { get; set; }
        public virtual DbSet<shipping_groups> shipping_groups { get; set; }
        public virtual DbSet<shipping_rates> shipping_rates { get; set; }
        public virtual DbSet<size_name> size_name { get; set; }
        public virtual DbSet<sms_template> sms_template { get; set; }
        public virtual DbSet<smsapisetting> smsapisettings { get; set; }
        public virtual DbSet<social_signin> social_signin { get; set; }
        public virtual DbSet<sr_tables> sr_tables { get; set; }
        public virtual DbSet<subscription_cart> subscription_cart { get; set; }
        public virtual DbSet<subscription_history> subscription_history { get; set; }
        public virtual DbSet<subscriptions_level> subscriptions_level { get; set; }
        public virtual DbSet<subscriptions_payment_history> subscriptions_payment_history { get; set; }
        public virtual DbSet<supplier_ledger> supplier_ledger { get; set; }
        public virtual DbSet<supplierpayinfo> supplierpayinfoes { get; set; }
        public virtual DbSet<syncstatu> syncstatus { get; set; }
        public virtual DbSet<sys_applicationpreferences> sys_applicationpreferences { get; set; }
        public virtual DbSet<t_login> t_login { get; set; }
        public virtual DbSet<t_roles> t_roles { get; set; }
        public virtual DbSet<t_userlogin> t_userlogin { get; set; }
        public virtual DbSet<t_year_of_operation> t_year_of_operation { get; set; }
        public virtual DbSet<taxmaster> taxmasters { get; set; }
        public virtual DbSet<taxrule> taxrules { get; set; }
        public virtual DbSet<taxtypemaster> taxtypemasters { get; set; }
        public virtual DbSet<tbl_bankcharges> tbl_bankcharges { get; set; }
        public virtual DbSet<tbl_daywisecash> tbl_daywisecash { get; set; }
        public virtual DbSet<tbl_daywisecashexpense> tbl_daywisecashexpense { get; set; }
        public virtual DbSet<tbl_description_spec> tbl_description_spec { get; set; }
        public virtual DbSet<tbl_disperitempercustomer> tbl_disperitempercustomer { get; set; }
        public virtual DbSet<tbl_fatmawalines> tbl_fatmawalines { get; set; }
        public virtual DbSet<tbl_groupname> tbl_groupname { get; set; }
        public virtual DbSet<tbl_headings> tbl_headings { get; set; }
        public virtual DbSet<tbl_permissions> tbl_permissions { get; set; }
        public virtual DbSet<tbl_productattribute> tbl_productattribute { get; set; }
        public virtual DbSet<tbl_productkey> tbl_productkey { get; set; }
        public virtual DbSet<tbl_productsubcription_type> tbl_productsubcription_type { get; set; }
        public virtual DbSet<tbl_quotation> tbl_quotation { get; set; }
        public virtual DbSet<tbl_reasons> tbl_reasons { get; set; }
        public virtual DbSet<tbl_saleorder> tbl_saleorder { get; set; }
        public virtual DbSet<tbl_schememaster> tbl_schememaster { get; set; }
        public virtual DbSet<tbl_selling_product> tbl_selling_product { get; set; }
        public virtual DbSet<tbl_selling_product_title> tbl_selling_product_title { get; set; }
        public virtual DbSet<tbl_synccheck> tbl_synccheck { get; set; }
        public virtual DbSet<tbl_tagcategory> tbl_tagcategory { get; set; }
        public virtual DbSet<tbl_tagcategorywithotherpage> tbl_tagcategorywithotherpage { get; set; }
        public virtual DbSet<tbl_useraddress> tbl_useraddress { get; set; }
        public virtual DbSet<tblcustomertransportdetail> tblcustomertransportdetails { get; set; }
        public virtual DbSet<tblserialno> tblserialnoes { get; set; }
        public virtual DbSet<tblsettingdetail> tblsettingdetails { get; set; }
        public virtual DbSet<tbltransporter> tbltransporters { get; set; }
        public virtual DbSet<temp_chalandet> temp_chalandet { get; set; }
        public virtual DbSet<temp_challanmast> temp_challanmast { get; set; }
        public virtual DbSet<temp_companyorders> temp_companyorders { get; set; }
        public virtual DbSet<time_zone> time_zone { get; set; }
        public virtual DbSet<transaction_details> transaction_details { get; set; }
        public virtual DbSet<user_access> user_access { get; set; }
        public virtual DbSet<vouchernumberformate> vouchernumberformates { get; set; }
        public virtual DbSet<wallet> wallets { get; set; }
        public virtual DbSet<website_banner> website_banner { get; set; }
        public virtual DbSet<website_clients> website_clients { get; set; }
        public virtual DbSet<website_gallery> website_gallery { get; set; }
        public virtual DbSet<website_images> website_images { get; set; }
        public virtual DbSet<website_services> website_services { get; set; }
        public virtual DbSet<website_statistics> website_statistics { get; set; }
        public virtual DbSet<whatsapp_setting> whatsapp_setting { get; set; }
        public virtual DbSet<whatsapp_share> whatsapp_share { get; set; }
        public virtual DbSet<acc_accountingrules> acc_accountingrules { get; set; }
        public virtual DbSet<acc_bankaccounts> acc_bankaccounts { get; set; }
        public virtual DbSet<acc_bankstatements> acc_bankstatements { get; set; }
        public virtual DbSet<acc_chartofaccounts> acc_chartofaccounts { get; set; }
        public virtual DbSet<acc_finopeningbalance> acc_finopeningbalance { get; set; }
        public virtual DbSet<acc_generalledger> acc_generalledger { get; set; }
        public virtual DbSet<acc_operatingexpenses> acc_operatingexpenses { get; set; }
        public virtual DbSet<acc_otherincomes> acc_otherincomes { get; set; }
        public virtual DbSet<acc_pettycash> acc_pettycash { get; set; }
        public virtual DbSet<acc_slgroups> acc_slgroups { get; set; }
        public virtual DbSet<all_custom_menu> all_custom_menu { get; set; }
        public virtual DbSet<ast_assettracking> ast_assettracking { get; set; }
        public virtual DbSet<checkbox> checkboxes { get; set; }
        public virtual DbSet<com_monthmaster> com_monthmaster { get; set; }
        public virtual DbSet<com_organizations> com_organizations { get; set; }
        public virtual DbSet<com_transactionterbill> com_transactionterbill { get; set; }
        public virtual DbSet<com_transactiontypes> com_transactiontypes { get; set; }
        public virtual DbSet<creditnote> creditnotes { get; set; }
        public virtual DbSet<debitnote> debitnotes { get; set; }
        public virtual DbSet<financeby> financebies { get; set; }
        public virtual DbSet<inv_itembrands> inv_itembrands { get; set; }
        public virtual DbSet<inv_itempricesales> inv_itempricesales { get; set; }
        public virtual DbSet<master1> master1 { get; set; }
        public virtual DbSet<masterdump> masterdumps { get; set; }
        public virtual DbSet<mm> mms { get; set; }
        public virtual DbSet<online_orders_log> online_orders_log { get; set; }
        public virtual DbSet<price> prices { get; set; }
        public virtual DbSet<pur_paymentmode> pur_paymentmode { get; set; }
        public virtual DbSet<sal_salesorderdeliveries> sal_salesorderdeliveries { get; set; }
        public virtual DbSet<sal_salesorderlines> sal_salesorderlines { get; set; }
        public virtual DbSet<sal_salesorderlinesvoid> sal_salesorderlinesvoid { get; set; }
        public virtual DbSet<sal_salesorders> sal_salesorders { get; set; }
        public virtual DbSet<sal_salesordertypes> sal_salesordertypes { get; set; }
        public virtual DbSet<sal_transferoutlines> sal_transferoutlines { get; set; }
        public virtual DbSet<sal_transferouts> sal_transferouts { get; set; }
        public virtual DbSet<sal_unsaveddatamisuse> sal_unsaveddatamisuse { get; set; }
        public virtual DbSet<sr_tablereservations> sr_tablereservations { get; set; }
        public virtual DbSet<sr_tablereservationtypes> sr_tablereservationtypes { get; set; }
        public virtual DbSet<sr_tabletypes> sr_tabletypes { get; set; }
        public virtual DbSet<tbl_report_setting> tbl_report_setting { get; set; }
        public virtual DbSet<tblprodecdemo> tblprodecdemoes { get; set; }
    }
}