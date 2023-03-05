using AutoMapper;
using Svam.EF;
using Svam.Models;
using Svam.Models.DTO;
using Svam.Repository;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Svam.Controllers
{
    public class TicketCustomController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        CommonRepository cr = new CommonRepository();


        #region New Code

        public async Task<ActionResult> CreateTicketSetting()
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            var model = new CreateLeadFieldDTO();
            var fieldsList = new List<FieldsNameList>();
            var fieldPriorityList = new List<LeadFieldPriorityDTO>();
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                var GetData = await db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
                var GetSeqData = await db.crm_ticket_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID).ToListAsync();
                if (GetData == null)
                {
                    crm_ticketcreatesetting GetData1 = new crm_ticketcreatesetting();

                    GetData1.IsName = true;
                    GetData1.IsPhoneNumber = true;
                    GetData1.IsProductTypeID = true;
                    GetData1.IsProductTypeIDRequired = true;
                    GetData1.IsErrorTypeID = true;
                    GetData1.IsErrorTypeIDRequired = true;
                    GetData1.IsUrgencyID = true;
                    GetData1.IsErrorTypeIDRequired = true;
                    GetData1.IsStatusID = true;
                    GetData1.IsStatusIDRequired = true;
                    GetData1.BranchID = BranchID;
                    GetData1.CompanyId = CompanyID;
                    GetData1.CreatedOn = Constant.GetBharatTime();
                    GetData1.CreatedBy = Convert.ToInt32(Session["UID"]);
                    GetData1.IsEmailID = true;
                    GetData1.Issubject = true;
                    GetData1.IsEmailIDRequired = true;
                    GetData1.IssubjectRequired = true;
                    db.crm_ticketcreatesetting.Add(GetData1);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        GetData = GetData1;
                    }


                }

                if (GetFormData == null)
                {
                    crm_ticketfieldnamecustomized GetFormData2 = new crm_ticketfieldnamecustomized();
                    GetFormData.NameText = "Customer Name";
                    GetFormData.EmailIDText = "Email Address";
                    GetFormData.PhoneNumberText = "Phone Number";
                    GetFormData.ProductTypeIDText = "Product Type";
                    GetFormData.ErrorTypeIDText = "Error Type";
                    GetFormData.UrgencyIDText = "Urgency Type";
                    GetFormData.StatusIDText = "Ticket Status";
                    GetFormData2.BranchID = BranchID;
                    GetFormData2.CompanyId = CompanyID;
                    GetFormData2.CreatedOn = Constant.GetBharatTime();
                    GetFormData2.CreatedBy = Convert.ToInt32(Session["UID"]);

                    db.crm_ticketfieldnamecustomized.Add(GetFormData2);
                    int i = db.SaveChanges();
                    if (i > 0)
                    {
                        GetFormData = GetFormData2;
                    }

                }

                if (GetSeqData != null && GetSeqData.Count > 0)
                {
                    fieldPriorityList = (from fpData in GetSeqData
                                         select new LeadFieldPriorityDTO
                                         {
                                             Priority = fpData.Priority,
                                             FieldName = fpData.FieldName
                                         }
                                       ).ToList();
                }
                else
                {
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 1, FieldName = "NameText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 2, FieldName = "EmailIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 3, FieldName = "PhoneNumberText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 4, FieldName = "ProductTypeIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 5, FieldName = "ErrorTypeIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 6, FieldName = "UrgencyIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 7, FieldName = "StatusIDText" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 8, FieldName = "subjectText" });

                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 9, FieldName = "ExtraCol1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 10, FieldName = "ExtraCol2Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 11, FieldName = "ExtraCol3Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 12, FieldName = "ExtraCol4Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 13, FieldName = "ExtraCol5Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 14, FieldName = "ExtraCol6Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 15, FieldName = "ExtraCol7Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 16, FieldName = "ExtraCol8Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 17, FieldName = "ExtraCol9Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 18, FieldName = "ExtraCol10Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 19, FieldName = "ExtraCol11Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 20, FieldName = "ExtraCol12Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 21, FieldName = "ExtraColdropdown1Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 22, FieldName = "ExtraColdropdown2Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 23, FieldName = "ExtraColdropdown3Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 24, FieldName = "ExtraColdropdown4Text" });
                    fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 25, FieldName = "ExtraColdropdown5Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 21, FieldName = "ImageCol1Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 22, FieldName = "ImageCol2Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 23, FieldName = "ImageCol3Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 24, FieldName = "ImageCol4Text" });

                    var uid = Convert.ToInt32(Session["UID"]);
                    foreach (var item in fieldPriorityList)
                    {
                        var fp = new crm_ticket_field_sequence
                        {
                            Priority = item.Priority,
                            FieldName = item.FieldName,
                            CompanyID = CompanyID,
                            BranchID = BranchID,
                            CreatedBy = uid,
                            Createddate = Constant.GetBharatTime(),
                            ModifiedDate = Constant.GetBharatTime()
                        };
                        db.crm_ticket_field_sequence.Add(fp);
                    }
                    db.SaveChanges();
                }

                #region fields names list for show dropdown

                string NameText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";
                int? cnmNo = fieldPriorityList.Where(a => a.FieldName == "NameText").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new FieldsNameList { TextName = NameText, Values = "NameText/true/true/NormalText", Priority = cnmNo });

                string IsEmail = (GetData == null || GetData.IsEmailID) ? "true" : "false";
                string IsEmailReq = GetData == null || GetData.IsEmailIDRequired ? "true" : "false";
                string EmailIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";
                int? emlNo = fieldPriorityList.Where(a => a.FieldName == "EmailIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = EmailIDText, Values = "EmailIDText/" + IsEmail + "/" + IsEmailReq + "/EmailText", Priority = emlNo });

                string PhoneNumberText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";
                int? mobNo = fieldPriorityList.Where(a => a.FieldName == "PhoneNumberText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = PhoneNumberText, Values = "PhoneNumberText/true/true/NormalText", Priority = mobNo });

                string ProductTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";
                int? ptNo = fieldPriorityList.Where(a => a.FieldName == "ProductTypeIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ProductTypeIDText, Values = "ProductTypeIDText/true/true/DropDownList", Priority = ptNo });


                string ErrorTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";
                int? errTNo = fieldPriorityList.Where(a => a.FieldName == "ErrorTypeIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ErrorTypeIDText, Values = "ErrorTypeIDText/true/true/DropDownList", Priority = errTNo });

                string UrgencyIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";
                int? urgNo = fieldPriorityList.Where(a => a.FieldName == "UrgencyIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = UrgencyIDText, Values = "UrgencyIDText/true/true/DropDownList", Priority = urgNo });


                string StatusIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";
                int? stsNo = fieldPriorityList.Where(a => a.FieldName == "StatusIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = StatusIDText, Values = "StatusIDText/true/true/DropDownList", Priority = stsNo });

                //string Issubject = (GetData == null || GetData.Issubject) ? "true" : "false";
                //string IssubjectRequired  = (GetData == null || GetData.IssubjectRequired) ? "true" : "false";
                string subjectText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";
                //fieldsList.Add(new FieldsNameList { TextName = subjectText, Values = "subjectText/" + Issubject + "/" + IssubjectRequired + "/NormalText" });
                int? subNo = fieldPriorityList.Where(a => a.FieldName == "subjectText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = subjectText, Values = "subjectText/true/true/NormalText", Priority = subNo });


                string IsExtraCol1 = (GetData != null && GetData.IsExtraCol1) ? "true" : "false";
                string IsExtraCol1Required = (GetData != null && GetData.IsExtraCol1Required) ? "true" : "false";
                string ExtraCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text : string.Empty;
                int? ex1No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol1Text, Values = "ExtraCol1Text/" + IsExtraCol1 + "/" + IsExtraCol1Required + "/NormalText", Priority = ex1No });

                //model.NormalFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol1Text))
                //{
                //    model.NormalFieldCount = 1;
                //}

                string IsExtraCol2 = (GetData != null && GetData.IsExtraCol2) ? "true" : "false";
                string IsExtraCol2Required = (GetData != null && GetData.IsExtraCol2Required) ? "true" : "false";
                string ExtraCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : string.Empty;
                int? ex2No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol2Text, Values = "ExtraCol2Text/" + IsExtraCol2 + "/" + IsExtraCol2Required + "/NormalText", Priority = ex2No });
                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text))
                //{
                //    model.NormalFieldCount = 2;
                //}


                string IsExtraCol3 = (GetData != null && GetData.IsExtraCol3) ? "true" : "false";
                string IsExtraCol3Required = (GetData != null && GetData.IsExtraCol3Required) ? "true" : "false";
                string ExtraCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : string.Empty;
                int? ex3No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol3Text, Values = "ExtraCol3Text/" + IsExtraCol3 + "/" + IsExtraCol3Required + "/NormalText", Priority = ex3No });
                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text))
                //{
                //    model.NormalFieldCount = 3;
                //}

                string IsExtraCol4 = (GetData != null && GetData.IsExtraCol4) ? "true" : "false";
                string IsExtraCol4Required = (GetData != null && GetData.IsExtraCol4Required) ? "true" : "false";
                string ExtraCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : string.Empty;
                int? ex4No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol4Text, Values = "ExtraCol4Text/" + IsExtraCol4 + "/" + IsExtraCol4Required + "/NormalText", Priority = ex4No });
                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text) && string.IsNullOrEmpty(ExtraCol4Text))
                //{
                //    model.NormalFieldCount = 4;
                //}

                string IsExtraCol5 = (GetData != null && GetData.IsExtraCol5) ? "true" : "false";
                string IsExtraCol5Required = (GetData != null && GetData.IsExtraCol5Required) ? "true" : "false";
                string ExtraCol5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text : string.Empty;
                int? ex5No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol5Text, Values = "ExtraCol5Text/" + IsExtraCol5 + "/" + IsExtraCol5Required + "/NormalText", Priority = ex5No });
                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text) && string.IsNullOrEmpty(ExtraCol4Text) && string.IsNullOrEmpty(ExtraCol5Text))
                //{
                //    model.NormalFieldCount = 5;
                //}

                string IsExtraCol6 = (GetData != null && GetData.ISExtraCol6) ? "true" : "false";
                string IsExtraCol6Required = (GetData != null && GetData.ISExtraCol6Required) ? "true" : "false";
                string ExtraCol6Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : string.Empty;
                int? ex6No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol6Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol6Text, Values = "ExtraCol6Text/" + IsExtraCol6 + "/" + IsExtraCol6Required + "/NormalText", Priority = ex6No });
                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text) && string.IsNullOrEmpty(ExtraCol4Text) && string.IsNullOrEmpty(ExtraCol5Text) && string.IsNullOrEmpty(ExtraCol6Text))
                //{
                //    model.NormalFieldCount = 6;
                //}

                string IsExtraCol7 = (GetData != null && GetData.ISExtraCol7) ? "true" : "false";
                string IsExtraCol7Required = (GetData != null && GetData.ISExtraCol7Required) ? "true" : "false";
                string ExtraCol7Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : string.Empty;
                int? ex7No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol7Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol7Text, Values = "ExtraCol7Text/" + IsExtraCol7 + "/" + IsExtraCol7Required + "/DecimalText", Priority = ex7No });
                //model.DecimalFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol7Text))
                //{
                //    model.NumericFieldCount = 1;
                //}

                string IsExtraCol8 = (GetData != null && GetData.ISExtraCol8) ? "true" : "false";
                string IsExtraCol8Required = (GetData != null && GetData.ISExtraCol8Required) ? "true" : "false";
                string ExtraCol8Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : string.Empty;
                int? ex8No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol8Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol8Text, Values = "ExtraCol8Text/" + IsExtraCol8 + "/" + IsExtraCol8Required + "/DecimalText", Priority = ex8No });
                if (string.IsNullOrEmpty(ExtraCol7Text) && string.IsNullOrEmpty(ExtraCol8Text))
                {
                    model.NumericFieldCount = 2;
                }

                string IsExtraCol9 = (GetData != null && GetData.IsExtraCol9) ? "true" : "false";
                string IsExtraCol9Required = (GetData != null && GetData.IsExtraCol9Required) ? "true" : "false";
                string ExtraCol9Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : string.Empty;
                int? ex9No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol9Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol9Text, Values = "ExtraCol9Text/" + IsExtraCol9 + "/" + IsExtraCol9Required + "/DateText", Priority = ex9No });
                //model.DateFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol9Text))
                //{
                //    model.NumericFieldCount = 1;
                //}

                string IsExtraCol10 = (GetData != null && GetData.IsExtraCol10) ? "true" : "false";
                string IsExtraCol10Required = (GetData != null && GetData.IsExtraCol10Required) ? "true" : "false";
                string ExtraCol10Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : string.Empty;
                int? ex10No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol10Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol10Text, Values = "ExtraCol10Text/" + IsExtraCol10 + "/" + IsExtraCol10Required + "/DateText", Priority = ex10No });

                //if (string.IsNullOrEmpty(ExtraCol9Text) && string.IsNullOrEmpty(ExtraCol10Text))
                //{
                //    model.DateFieldCount = 2;
                //}

                string IsExtraCol11 = (GetData != null && GetData.IsExtraCol11) ? "true" : "false";
                string IsExtraCol11Required = (GetData != null && GetData.IsExtraCol11Required) ? "true" : "false";
                string ExtraCol11Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : string.Empty;
                int? ex11No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol11Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol11Text, Values = "ExtraCol11Text/" + IsExtraCol11 + "/" + IsExtraCol11Required + "/NumberText", Priority = ex11No });
                //model.NumericFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol11Text))
                //{
                //    model.NumericFieldCount = 1;
                //}

                string IsExtraCol12 = (GetData != null && GetData.IsExtraCol12) ? "true" : "false";
                string IsExtraCol12Required = (GetData != null && GetData.IsExtraCol12Required) ? "true" : "false";
                string ExtraCol12Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : string.Empty;
                int? ex12No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol12Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraCol12Text, Values = "ExtraCol12Text/" + IsExtraCol12 + "/" + IsExtraCol12Required + "/NumberText", Priority = ex12No });



                string IsExtraColdropdown1 = (GetData != null && GetData.IsExtraColdropdown1) ? "true" : "false";
                string IsExtraColdropdown1Required = (GetData != null && GetData.IsExtraColdropdown1Required) ? "true" : "false";
                string ExtraColdropdown1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text) ? GetFormData.ExtraColdropdown1Text : string.Empty;
                int? exdp1NO = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown1Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraColdropdown1Text, Values = "ExtraColdropdown1Text/" + IsExtraColdropdown1 + "/" + IsExtraColdropdown1Required + "/DropDownList", Priority = exdp1NO });

                string IsExtraColdropdown2 = (GetData != null && GetData.IsExtraColdropdown2) ? "true" : "false";
                string IsExtraColdropdown2Required = (GetData != null && GetData.IsExtraColdropdown2Required) ? "true" : "false";
                string ExtraColdropdown2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text) ? GetFormData.ExtraColdropdown2Text : string.Empty;
                int? exdp2NO = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown2Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraColdropdown2Text, Values = "ExtraColdropdown2Text/" + IsExtraColdropdown2 + "/" + IsExtraColdropdown2Required + "/DropDownList", Priority = exdp2NO });


                string IsExtraColdropdown3 = (GetData != null && GetData.IsExtraColdropdown3) ? "true" : "false";
                string IsExtraColdropdown3Required = (GetData != null && GetData.IsExtraColdropdown3Required) ? "true" : "false";
                string ExtraColdropdown3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text) ? GetFormData.ExtraColdropdown3Text : string.Empty;
                int? exdp3NO = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown3Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraColdropdown3Text, Values = "ExtraColdropdown3Text/" + IsExtraColdropdown3 + "/" + IsExtraColdropdown3Required + "/DropDownList", Priority = exdp3NO });


                string IsExtraColdropdown4 = (GetData != null && GetData.IsExtraColdropdown4) ? "true" : "false";
                string IsExtraColdropdown4Required = (GetData != null && GetData.IsExtraColdropdown4Required) ? "true" : "false";
                string ExtraColdropdown4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text) ? GetFormData.ExtraColdropdown4Text : string.Empty;
                int? exdp4NO = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown4Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraColdropdown4Text, Values = "ExtraColdropdown4Text/" + IsExtraColdropdown4 + "/" + IsExtraColdropdown4Required + "/DropDownList", Priority = exdp4NO });


                string IsExtraColdropdown5 = (GetData != null && GetData.IsExtraColdropdown5) ? "true" : "false";
                string IsExtraColdropdown5Required = (GetData != null && GetData.IsExtraColdropdown5Required) ? "true" : "false";
                string ExtraColdropdown5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text) ? GetFormData.ExtraColdropdown5Text : string.Empty;
                int? exdp5NO = fieldPriorityList.Where(a => a.FieldName == "ExtraColdropdown5Text").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new FieldsNameList { TextName = ExtraColdropdown5Text, Values = "ExtraColdropdown5Text/" + IsExtraColdropdown5 + "/" + IsExtraColdropdown5Required + "/DropDownList", Priority = exdp5NO });

                //if (string.IsNullOrEmpty(ExtraCol11Text) && string.IsNullOrEmpty(ExtraCol12Text))
                //{
                //    model.NumericFieldCount = 2;
                //}

                //string IsImageCol1 = (GetData != null && GetData.IsImageCol1) ? "true" : "false";
                //string IsImageCol1Required = (GetData != null && GetData.IsImageCol1Required) ? "true" : "false";
                //string ImageCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol1Text) ? GetFormData.ImageCol1Text : string.Empty;
                //int? ex13No = fieldPriorityList.Where(a => a.FieldName == "ImageCol1Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new FieldsNameList { TextName = ImageCol1Text, Values = "ImageCol1Text/" + IsImageCol1 + "/" + IsImageCol1Required + "/FilePath",Priority= ex13No });

                //string IsImageCol2 = (GetData != null && GetData.IsImageCol2) ? "true" : "false";
                //string IsImageCol2Required = (GetData != null && GetData.IsImageCol2Required) ? "true" : "false";
                //string ImageCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol2Text) ? GetFormData.ImageCol2Text : string.Empty;
                //int? ex14No = fieldPriorityList.Where(a => a.FieldName == "ImageCol2Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new FieldsNameList { TextName = ImageCol2Text, Values = "ImageCol2Text/" + IsImageCol2 + "/" + IsImageCol2Required + "/FilePath" ,Priority=ex14No});

                //string IsImageCol3 = (GetData != null && GetData.IsImageCol3) ? "true" : "false";
                //string IsImageCol3Required = (GetData != null && GetData.IsImageCol3Required) ? "true" : "false";
                //string ImageCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol3Text) ? GetFormData.ImageCol3Text : string.Empty;
                //int? ex15No = fieldPriorityList.Where(a => a.FieldName == "ImageCol3Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new FieldsNameList { TextName = ImageCol3Text, Values = "ImageCol3Text/" + IsImageCol3 + "/" + IsImageCol3Required + "/FilePath",Priority= ex15No});

                //string IsImageCol4 = (GetData != null && GetData.IsImageCol4) ? "true" : "false";
                //string IsImageCol4Required = (GetData != null && GetData.IsImageCol4Required) ? "true" : "false";
                //string ImageCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol4Text) ? GetFormData.ImageCol4Text : string.Empty;
                //int? ex16No = fieldPriorityList.Where(a => a.FieldName == "ImageCol4Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new FieldsNameList { TextName = ImageCol4Text, Values = "ImageCol4Text/" + IsImageCol4 + "/" + IsImageCol4Required + "/FilePath",Priority=ex16No });

                model.FieldNames = fieldsList;
                #endregion

            }
            else
            {
                return Redirect("/home/login");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTicketSetting(CreateLeadFieldDTO model)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string msg = "";
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var GetData = db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                        var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();

                        if (GetData != null && GetFormData != null)
                        {

                            if (model.SaveType == "New" && !string.IsNullOrEmpty(model.FieldType) && !string.IsNullOrEmpty(model.FieldTextName))//check if save type is not null for add new field
                            {
                                if (model.FieldType == "NormalText")//check field type for insert column string data type
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                    {
                                        GetFormData.ExtraCol1Text = model.FieldTextName;
                                        GetData.IsExtraCol1 = model.IsVisible;
                                        GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                    {
                                        GetFormData.ExtraCol2Text = model.FieldTextName;
                                        GetData.IsExtraCol2 = model.IsVisible;
                                        GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                    {
                                        GetFormData.ExtraCol3Text = model.FieldTextName;
                                        GetData.IsExtraCol3 = model.IsVisible;
                                        GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                    {
                                        GetFormData.ExtraCol4Text = model.FieldTextName;
                                        GetData.IsExtraCol4 = model.IsVisible;
                                        GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                    {
                                        GetFormData.ExtraCol5Text = model.FieldTextName;
                                        GetData.IsExtraCol5 = model.IsVisible;
                                        GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                    {
                                        GetFormData.ExtraCol6Text = model.FieldTextName;
                                        GetData.ISExtraCol6 = model.IsVisible;
                                        GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No Field available for normal text type";
                                    }
                                }
                                else if (model.FieldType == "DecimalText")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                    {
                                        GetFormData.ExtraCol7Text = model.FieldTextName;
                                        GetData.ISExtraCol7 = model.IsVisible;
                                        GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                    {
                                        GetFormData.ExtraCol8Text = model.FieldTextName;
                                        GetData.ISExtraCol8 = model.IsVisible;
                                        GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for decimal type";
                                    }
                                }
                                else if (model.FieldType == "NumberText")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                    {
                                        GetFormData.ExtraCol11Text = model.FieldTextName;
                                        GetData.IsExtraCol11 = model.IsVisible;
                                        GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                    {
                                        GetFormData.ExtraCol12Text = model.FieldTextName;
                                        GetData.IsExtraCol12 = model.IsVisible;
                                        GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for number type";
                                    }
                                }
                                else if (model.FieldType == "DateText")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                    {
                                        GetFormData.ExtraCol9Text = model.FieldTextName;
                                        GetData.IsExtraCol9 = model.IsVisible;
                                        GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                    {
                                        GetFormData.ExtraCol10Text = model.FieldTextName;
                                        GetData.IsExtraCol10 = model.IsVisible;
                                        GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");

                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for date type";
                                    }
                                }
                                else if (model.FieldType == "DropDownList")
                                {
                                    if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                    {
                                        GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown1 = model.IsVisible;
                                        GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                    {
                                        GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown2 = model.IsVisible;
                                        GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                    {
                                        GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown3 = model.IsVisible;
                                        GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                    {
                                        GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown4 = model.IsVisible;
                                        GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                    {
                                        GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown5 = model.IsVisible;
                                        GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                        db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field added successfully";
                                    }
                                    else
                                    {
                                        TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                    }

                                }
                                //else if (model.FieldType == "FilePath")
                                //{
                                //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                //    {
                                //        GetFormData.ImageCol1Text = model.FieldTextName;
                                //        GetData.IsImageCol1 = model.IsVisible;
                                //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                //        db.SaveChanges();
                                //        trans.Commit();
                                //        TempData["success"] = "Field added successfully";
                                //    }
                                //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                //    {
                                //        GetFormData.ImageCol2Text = model.FieldTextName;
                                //        GetData.IsImageCol2 = model.IsVisible;
                                //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                //        db.SaveChanges();
                                //        trans.Commit();
                                //        TempData["success"] = "Field added successfully";
                                //    }
                                //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                //    {
                                //        GetFormData.ImageCol3Text = model.FieldTextName;
                                //        GetData.IsImageCol3 = model.IsVisible;
                                //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                //        db.SaveChanges();
                                //        trans.Commit();
                                //        TempData["success"] = "Field added successfully";
                                //    }
                                //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                //    {
                                //        GetFormData.ImageCol4Text = model.FieldTextName;
                                //        GetData.IsImageCol4 = model.IsVisible;
                                //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                //        db.SaveChanges();
                                //        trans.Commit();
                                //        TempData["success"] = "Field added successfully";
                                //    }
                                //    else
                                //    {
                                //        TempData["alert"] = "Sorry! No field available for image/file type";
                                //    }
                                //}
                                //msg = "Field added successfully";
                            }
                            else if (!string.IsNullOrEmpty(model.FieldTextName))//check field text name not null
                            {
                                var GetSeqData = db.crm_ticket_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == model.FieldName).FirstOrDefault();
                                model.FieldPriority = GetSeqData.Priority;//set priority

                                if (model.FieldName == "NameText")
                                {
                                    GetFormData.NameText = model.FieldTextName;
                                    GetData.IsName = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "PhoneNumberText")
                                {
                                    GetFormData.PhoneNumberText = model.FieldTextName;
                                    GetData.IsPhoneNumber = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";
                                }
                                else if (model.FieldName == "ProductTypeIDText")
                                {
                                    GetFormData.ProductTypeIDText = model.FieldTextName;
                                    GetData.IsProductTypeID = true;
                                    GetData.IsProductTypeIDRequired = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "ErrorTypeIDText")
                                {
                                    GetFormData.ErrorTypeIDText = model.FieldTextName;
                                    GetData.IsErrorTypeID = true;
                                    GetData.IsErrorTypeIDRequired = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "UrgencyIDText")
                                {
                                    GetFormData.UrgencyIDText = model.FieldTextName;
                                    GetData.IsUrgencyID = true;
                                    GetData.IsUrgencyIDRequired = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "StatusIDText")
                                {
                                    GetFormData.StatusIDText = model.FieldTextName;
                                    GetData.IsStatusID = true;
                                    GetData.IsStatusIDRequired = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "EmailIDText")
                                {
                                    GetFormData.EmailIDText = model.FieldTextName;
                                    GetData.IsEmailID = model.IsVisible;
                                    GetData.IsEmailIDRequired = model.IsVisible == true ? model.IsRequired : false;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "subjectText")
                                {
                                    GetFormData.subjectText = model.FieldTextName;
                                    GetData.Issubject = true;
                                    GetData.IssubjectRequired = true;
                                    db.SaveChanges();
                                    trans.Commit();
                                    TempData["success"] = "Field updated successfully";

                                }
                                else if (model.FieldName == "ExtraCol1Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol1Text = model.FieldTextName;
                                        GetData.IsExtraCol1 = model.IsVisible;
                                        GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol1Text = null;
                                        //        GetData.IsExtraCol1 = false;
                                        //        GetData.IsExtraCol1Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol1Text = null;
                                        //        GetData.IsExtraCol1 = false;
                                        //        GetData.IsExtraCol1Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol1Text = null;
                                        //        GetData.IsExtraCol1 = false;
                                        //        GetData.IsExtraCol1Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol1Text = null;
                                        //        GetData.IsExtraCol1 = false;
                                        //        GetData.IsExtraCol1Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol2Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol2Text = model.FieldTextName;
                                        GetData.IsExtraCol2 = model.IsVisible;
                                        GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol2Text = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol2Text = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol2Text = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol2Text = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol2Text = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol2Text = null;
                                                GetData.IsExtraCol2 = false;
                                                GetData.IsExtraCol2Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }

                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol2Text = null;
                                        //        GetData.IsExtraCol2 = false;
                                        //        GetData.IsExtraCol2Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol2Text = null;
                                        //        GetData.IsExtraCol2 = false;
                                        //        GetData.IsExtraCol2Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol2Text = null;
                                        //        GetData.IsExtraCol2 = false;
                                        //        GetData.IsExtraCol2Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol2Text = null;
                                        //        GetData.IsExtraCol2 = false;
                                        //        GetData.IsExtraCol2Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol3Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol3Text = model.FieldTextName;
                                        GetData.IsExtraCol3 = model.IsVisible;
                                        GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol3Text = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol3Text = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol3Text = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol3Text = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol3Text = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol3Text = null;
                                                GetData.IsExtraCol3 = false;
                                                GetData.IsExtraCol3Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol3Text = null;
                                        //        GetData.IsExtraCol3 = false;
                                        //        GetData.IsExtraCol3Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol3Text = null;
                                        //        GetData.IsExtraCol3 = false;
                                        //        GetData.IsExtraCol3Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol3Text = null;
                                        //        GetData.IsExtraCol3 = false;
                                        //        GetData.IsExtraCol3Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol3Text = null;
                                        //        GetData.IsExtraCol3 = false;
                                        //        GetData.IsExtraCol3Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol4Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol4Text = model.FieldTextName;
                                        GetData.IsExtraCol4 = model.IsVisible;
                                        GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol4Text = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol4Text = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol4Text = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol4Text = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol4Text = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol4Text = null;
                                                GetData.IsExtraCol4 = false;
                                                GetData.IsExtraCol4Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }

                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol4Text = null;
                                        //        GetData.IsExtraCol4 = false;
                                        //        GetData.IsExtraCol4Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol4Text = null;
                                        //        GetData.IsExtraCol4 = false;
                                        //        GetData.IsExtraCol4Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol4Text = null;
                                        //        GetData.IsExtraCol4 = false;
                                        //        GetData.IsExtraCol4Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol4Text = null;
                                        //        GetData.IsExtraCol4 = false;
                                        //        GetData.IsExtraCol4Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol5Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol5Text = model.FieldTextName;
                                        GetData.IsExtraCol5 = model.IsVisible;
                                        GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol5Text = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol5Text = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol5Text = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol5Text = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol5Text = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol5Text = null;
                                                GetData.IsExtraCol5 = false;
                                                GetData.IsExtraCol5Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }

                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol5Text = null;
                                        //        GetData.IsExtraCol5 = false;
                                        //        GetData.IsExtraCol5Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol5Text = null;
                                        //        GetData.IsExtraCol5 = false;
                                        //        GetData.IsExtraCol5Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol5Text = null;
                                        //        GetData.IsExtraCol5 = false;
                                        //        GetData.IsExtraCol5Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol5Text = null;
                                        //        GetData.IsExtraCol5 = false;
                                        //        GetData.IsExtraCol5Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol6Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol6Text = model.FieldTextName;
                                        GetData.ISExtraCol6 = model.IsVisible;
                                        GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol6Text = null;
                                                GetData.ISExtraCol6 = false;
                                                GetData.ISExtraCol6Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field added successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol6Text = null;
                                                GetData.ISExtraCol6 = false;
                                                GetData.ISExtraCol6Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();

                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol6Text = null;
                                                GetData.ISExtraCol6 = false;
                                                GetData.ISExtraCol6Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol6Text = null;
                                                GetData.ISExtraCol6 = false;
                                                GetData.ISExtraCol6Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol6Text = null;
                                                GetData.ISExtraCol6 = false;
                                                GetData.ISExtraCol6Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol6Text = null;
                                                GetData.ISExtraCol6 = false;
                                                GetData.ISExtraCol6Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }

                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol6Text = null;
                                        //        GetData.ISExtraCol6 = false;
                                        //        GetData.ISExtraCol6Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol6Text = null;
                                        //        GetData.ISExtraCol6 = false;
                                        //        GetData.ISExtraCol6Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol6Text = null;
                                        //        GetData.ISExtraCol6 = false;
                                        //        GetData.ISExtraCol6Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol6Text = null;
                                        //        GetData.ISExtraCol6 = false;
                                        //        GetData.ISExtraCol6Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol7Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol7Text = model.FieldTextName;
                                        GetData.ISExtraCol7 = model.IsVisible;
                                        GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();

                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol7Text = null;
                                                GetData.ISExtraCol7 = false;
                                                GetData.ISExtraCol7Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }

                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol7Text = null;
                                        //        GetData.ISExtraCol7 = false;
                                        //        GetData.ISExtraCol7Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol7Text = null;
                                        //        GetData.ISExtraCol7 = false;
                                        //        GetData.ISExtraCol7Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol7Text = null;
                                        //        GetData.ISExtraCol7 = false;
                                        //        GetData.ISExtraCol7Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol7Text = null;
                                        //        GetData.ISExtraCol7 = false;
                                        //        GetData.ISExtraCol7Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol8Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol8Text = model.FieldTextName;
                                        GetData.ISExtraCol8 = model.IsVisible;
                                        GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12sText'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol8Text = null;
                                                GetData.ISExtraCol8 = false;
                                                GetData.ISExtraCol8Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }

                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol8Text = null;
                                        //        GetData.ISExtraCol8 = false;
                                        //        GetData.ISExtraCol8Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol8Text = null;
                                        //        GetData.ISExtraCol8 = false;
                                        //        GetData.ISExtraCol8Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol8Text = null;
                                        //        GetData.ISExtraCol8 = false;
                                        //        GetData.ISExtraCol8Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol8Text = null;
                                        //        GetData.ISExtraCol8 = false;
                                        //        GetData.ISExtraCol8Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol9Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol9Text = model.FieldTextName;
                                        GetData.IsExtraCol9 = model.IsVisible;
                                        GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol9Text = null;
                                                GetData.IsExtraCol9 = false;
                                                GetData.IsExtraCol9Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol9Text = null;
                                        //        GetData.IsExtraCol9 = false;
                                        //        GetData.IsExtraCol9Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol9Text = null;
                                        //        GetData.IsExtraCol9 = false;
                                        //        GetData.IsExtraCol9Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol9Text = null;
                                        //        GetData.IsExtraCol9 = false;
                                        //        GetData.IsExtraCol9Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol9Text = null;
                                        //        GetData.IsExtraCol9 = false;
                                        //        GetData.IsExtraCol9Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                                else if (model.FieldName == "ExtraCol10Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol10Text = model.FieldTextName;
                                        GetData.IsExtraCol10 = model.IsVisible;
                                        GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "NumberText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol11Text = model.FieldTextName;
                                                GetData.IsExtraCol11 = model.IsVisible;
                                                GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;

                                                GetFormData.ExtraCol12Text = model.FieldTextName;
                                                GetData.IsExtraCol12 = model.IsVisible;
                                                GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for number type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;


                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;


                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraCol10Text = null;
                                                GetData.IsExtraCol10 = false;
                                                GetData.IsExtraCol10Required = false;


                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol10Text = null;
                                        //        GetData.IsExtraCol10 = false;
                                        //        GetData.IsExtraCol10Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol10Text = null;
                                        //        GetData.IsExtraCol10 = false;
                                        //        GetData.IsExtraCol10Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol10Text = null;
                                        //        GetData.IsExtraCol10 = false;
                                        //        GetData.IsExtraCol10Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol10Text = null;
                                        //        GetData.IsExtraCol10 = false;
                                        //        GetData.IsExtraCol10Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol11Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol11Text = model.FieldTextName;
                                        GetData.IsExtraCol11 = model.IsVisible;
                                        GetData.IsExtraCol11Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        trans.Commit();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol1Text = null;
                                                GetData.IsExtraCol1 = false;
                                                GetData.IsExtraCol1Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;


                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;


                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraCol11Text = null;
                                                GetData.IsExtraCol11 = false;
                                                GetData.IsExtraCol11Required = false;


                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol11Text = null;
                                        //        GetData.IsExtraCol11 = false;
                                        //        GetData.IsExtraCol11Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol11Text = null;
                                        //        GetData.IsExtraCol11 = false;
                                        //        GetData.IsExtraCol11Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol11Text = null;
                                        //        GetData.IsExtraCol11 = false;
                                        //        GetData.IsExtraCol11Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol11Text = null;
                                        //        GetData.IsExtraCol11 = false;
                                        //        GetData.IsExtraCol11Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraCol12Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraCol12Text = model.FieldTextName;
                                        GetData.IsExtraCol12 = model.IsVisible;
                                        GetData.IsExtraCol12Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;


                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;


                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraCol12Text = null;
                                                GetData.IsExtraCol12 = false;
                                                GetData.IsExtraCol12Required = false;


                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraColdropdown1Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown1 = model.IsVisible;
                                        GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {

                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;


                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;

                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraColdropdown1Text = null;
                                                GetData.IsExtraColdropdown1 = false;
                                                GetData.IsExtraColdropdown1Required = false;


                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraColdropdown2Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown2 = model.IsVisible;
                                        GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {

                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;


                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;

                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraColdropdown2Text = null;
                                                GetData.IsExtraColdropdown2 = false;
                                                GetData.IsExtraColdropdown2Required = false;


                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraColdropdown3Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown3 = model.IsVisible;
                                        GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {

                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraColdropdown3Text = null;
                                                GetData.IsExtraColdropdown3 = false;
                                                GetData.IsExtraColdropdown3Required = false;

                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraColdropdown4Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown4 = model.IsVisible;
                                        GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {

                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text))
                                            {
                                                GetFormData.ExtraColdropdown4Text = null;
                                                GetData.IsExtraColdropdown4 = false;
                                                GetData.IsExtraColdropdown4Required = false;

                                                GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown5 = model.IsVisible;
                                                GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown5Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                else if (model.FieldName == "ExtraColdropdown5Text")
                                {
                                    if (model.FieldType == model.FieldPreviousType)
                                    {
                                        GetFormData.ExtraColdropdown5Text = model.FieldTextName;
                                        GetData.IsExtraColdropdown5 = model.IsVisible;
                                        GetData.IsExtraColdropdown5Required = model.IsVisible == true ? model.IsRequired : false;
                                        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        GetData.ModifiedOn = Constant.GetBharatTime();
                                        db.SaveChanges();
                                        TempData["success"] = "Field updated successfully";
                                    }
                                    else
                                    {
                                        #region extra1 replace according to field type
                                        if (model.FieldType == "NormalText")//check field type for insert column string data type
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol1Text = model.FieldTextName;
                                                GetData.IsExtraCol1 = model.IsVisible;
                                                GetData.IsExtraCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol2Text = model.FieldTextName;
                                                GetData.IsExtraCol2 = model.IsVisible;
                                                GetData.IsExtraCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol3Text = model.FieldTextName;
                                                GetData.IsExtraCol3 = model.IsVisible;
                                                GetData.IsExtraCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol4Text = model.FieldTextName;
                                                GetData.IsExtraCol4 = model.IsVisible;
                                                GetData.IsExtraCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol5Text = model.FieldTextName;
                                                GetData.IsExtraCol5 = model.IsVisible;
                                                GetData.IsExtraCol5Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol6Text = model.FieldTextName;
                                                GetData.ISExtraCol6 = model.IsVisible;
                                                GetData.ISExtraCol6Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No Field available for normal text type";
                                            }
                                        }
                                        else if (model.FieldType == "DecimalText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol7Text = model.FieldTextName;
                                                GetData.ISExtraCol7 = model.IsVisible;
                                                GetData.ISExtraCol7Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol8Text = model.FieldTextName;
                                                GetData.ISExtraCol8 = model.IsVisible;
                                                GetData.ISExtraCol8Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for decimal type";
                                            }
                                        }
                                        else if (model.FieldType == "DateText")
                                        {
                                            if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol9Text = model.FieldTextName;
                                                GetData.IsExtraCol9 = model.IsVisible;
                                                GetData.IsExtraCol9Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraCol10Text = model.FieldTextName;
                                                GetData.IsExtraCol10 = model.IsVisible;
                                                GetData.IsExtraCol10Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                //update field priority to replaced field priority
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                                GetSeqData.Priority = 0;//update field priority to 0

                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for date type";
                                            }
                                        }
                                        else if (model.FieldType == "DropDownList")
                                        {

                                            if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;


                                                GetFormData.ExtraColdropdown1Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown1 = model.IsVisible;
                                                GetData.IsExtraColdropdown1Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown1Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;


                                                GetFormData.ExtraColdropdown2Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown2 = model.IsVisible;
                                                GetData.IsExtraColdropdown2Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown2Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraColdropdown3Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown3 = model.IsVisible;
                                                GetData.IsExtraColdropdown3Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown3Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else if (string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text))
                                            {
                                                GetFormData.ExtraColdropdown5Text = null;
                                                GetData.IsExtraColdropdown5 = false;
                                                GetData.IsExtraColdropdown5Required = false;

                                                GetFormData.ExtraColdropdown4Text = model.FieldTextName;
                                                GetData.IsExtraColdropdown4 = model.IsVisible;
                                                GetData.IsExtraColdropdown4Required = model.IsVisible == true ? model.IsRequired : false;
                                                GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                                GetData.ModifiedOn = Constant.GetBharatTime();
                                                GetSeqData.Priority = 0;//update field sequence to 0
                                                db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + model.FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraColdropdown4Text'");
                                                db.SaveChanges();
                                                trans.Commit();
                                                TempData["success"] = "Field updated successfully";
                                            }
                                            else
                                            {
                                                TempData["alert"] = "Sorry! No field available for Dropdownlist";
                                            }
                                        }
                                        //else if (model.FieldType == "FilePath")
                                        //{
                                        //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol1Text = model.FieldTextName;
                                        //        GetData.IsImageCol1 = model.IsVisible;
                                        //        GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol2Text = model.FieldTextName;
                                        //        GetData.IsImageCol2 = model.IsVisible;
                                        //        GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol3Text = model.FieldTextName;
                                        //        GetData.IsImageCol3 = model.IsVisible;
                                        //        GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                        //    {
                                        //        GetFormData.ExtraCol12Text = null;
                                        //        GetData.IsExtraCol12 = false;
                                        //        GetData.IsExtraCol12Required = false;

                                        //        GetFormData.ImageCol4Text = model.FieldTextName;
                                        //        GetData.IsImageCol4 = model.IsVisible;
                                        //        GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                        //        GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                        //        GetData.ModifiedOn = Constant.GetBharatTime();
                                        //        db.SaveChanges();
                                        //        trans.Commit();
                                        //        TempData["success"] = "Field updated successfully";
                                        //    }
                                        //    else
                                        //    {
                                        //        TempData["alert"] = "Sorry! No field available for image/file type";
                                        //    }
                                        //}
                                        #endregion
                                    }

                                }
                                //else if (model.FieldName == "ImageCol1Text")
                                //{
                                //    GetFormData.ImageCol1Text = model.FieldTextName;
                                //    GetData.IsImageCol1 = model.IsVisible;
                                //    GetData.IsImageCol1Required = model.IsVisible == true ? model.IsRequired : false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    db.SaveChanges();
                                //    TempData["success"] = "Field updated successfully";

                                //}
                                //else if (model.FieldName == "ImageCol2Text")
                                //{
                                //    GetFormData.ImageCol2Text = model.FieldTextName;
                                //    GetData.IsImageCol2 = model.IsVisible;
                                //    GetData.IsImageCol2Required = model.IsVisible == true ? model.IsRequired : false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    db.SaveChanges();
                                //    TempData["success"] = "Field updated successfully";

                                //}
                                //else if (model.FieldName == "ImageCol3Text")
                                //{
                                //    GetFormData.ImageCol3Text = model.FieldTextName;
                                //    GetData.IsImageCol3 = model.IsVisible;
                                //    GetData.IsImageCol3Required = model.IsVisible == true ? model.IsRequired : false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    db.SaveChanges();
                                //    TempData["success"] = "Field updated successfully";

                                //}
                                //else if (model.FieldName == "ImageCol4Text")
                                //{
                                //    GetFormData.ImageCol4Text = model.FieldTextName;
                                //    GetData.IsImageCol4 = model.IsVisible;
                                //    GetData.IsImageCol4Required = model.IsVisible == true ? model.IsRequired : false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    db.SaveChanges();
                                //    TempData["success"] = "Field updated successfully";

                                //}
                            }

                            msg = "Data updated successfully";
                            db.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ExceptionLogging.SendExcepToDB(ex);
                        TempData["alert"] = "Sorry! there is some technical problem";
                    }
                }
            }
            else
            {
                return Redirect("/home/login");
            }

            return RedirectToAction("CreateTicketSetting");
        }

        public async Task<ActionResult> ViewTicketSetting()
        {

            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            var model = new ViewTecketSettingDTO();
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                var GetData = await db.crm_ticketviewsetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();

                if (GetData != null)
                {
                    var data = Mapper.Map<ViewTecketSettingDTO>(GetData);
                    model = data;
                }
                else
                {
                    var data = new crm_ticketviewsetting();
                    data.CreatedOn = Constant.GetBharatTime();
                    data.IsEmailID = true;
                    data.IsErrorTypeID = true;
                    data.IsUrgencyID = true;
                    data.IsStatusID = true;
                    data.IsCreatedBy = true;
                    data.IsCreatedDate = true;
                    data.IsAssignedTo = true;
                    data.IsAssignedBy = true;
                    data.IsModifiedDate = true;
                    data.BranchID = BranchID;
                    data.CompanyId = CompanyID;
                    data.IsName = true;
                    data.IsPhoneNumber = true;
                    db.crm_ticketviewsetting.Add(data);
                    int i = await db.SaveChangesAsync();
                    if (i > 0)
                    {
                        model = Mapper.Map<ViewTecketSettingDTO>(data);
                    }
                }

                #region fields names list for show row wise

                model.NameText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";

                model.EmailIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";

                model.PhoneNumberText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";

                model.ProductTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";

                model.ErrorTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";

                model.UrgencyIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";

                model.StatusIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";

                model.subjectText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";

                model.ExtraCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text : string.Empty;

                model.ExtraCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : string.Empty;

                model.ExtraCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : string.Empty;

                model.ExtraCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : string.Empty;

                model.ExtraCol5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text : string.Empty;

                model.ExtraCol6Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : string.Empty;

                model.ExtraCol7Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : string.Empty;

                model.ExtraCol8Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : string.Empty;

                model.ExtraCol9Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : string.Empty;

                model.ExtraCol10Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : string.Empty;

                model.ExtraCol11Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : string.Empty;

                model.ExtraCol12Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : string.Empty;

                model.ExtraColdropdown1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown1Text) ? GetFormData.ExtraColdropdown1Text : string.Empty;

                model.ExtraColdropdown2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown2Text) ? GetFormData.ExtraColdropdown2Text : string.Empty;

                model.ExtraColdropdown3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown3Text) ? GetFormData.ExtraColdropdown3Text : string.Empty;

                model.ExtraColdropdown4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown4Text) ? GetFormData.ExtraColdropdown4Text : string.Empty;

                model.ExtraColdropdown5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraColdropdown5Text) ? GetFormData.ExtraColdropdown5Text : string.Empty;

                //model.ImageCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol1Text) ? GetFormData.ImageCol1Text : string.Empty;

                //model.ImageCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol2Text) ? GetFormData.ImageCol2Text : string.Empty;

                //model.ImageCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol3Text) ? GetFormData.ImageCol3Text : string.Empty;

                //model.ImageCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol4Text) ? GetFormData.ImageCol4Text : string.Empty;

                #endregion

            }
            else
            {
                return Redirect("/home/login");
            }

            return View(model);
        }

        public JsonResult ChangeStatus(string FldNM)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_ticketviewsetting set " + FldNM + "=case when " + FldNM + "=1 then 0 else 1 end where BranchID =" + BranchID + "  and CompanyID = " + CompanyID + "");
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeFilterStatus(string FldNM)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_ticketviewsetting set " + FldNM + "=case when " + FldNM + "=1 then 0 else 1 end where BranchID =" + BranchID + "  and CompanyID = " + CompanyID + "");
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult HideField(string FieldName)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string msg = "";
            if (Session["BranchID"] != null && Session["CompanyID"] != null)
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {

                        var GetData = db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                        var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                        var GetSeqData = db.crm_ticket_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == FieldName).FirstOrDefault();
                        if (GetData != null && GetFormData != null)
                        {
                            if (!string.IsNullOrEmpty(FieldName))//check field text name not null
                            {
                                if (FieldName == "ExtraCol1Text")
                                {
                                    GetFormData.ExtraCol1Text = null;
                                    GetData.IsExtraCol1 = false;
                                    GetData.IsExtraCol1Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol2Text")
                                {
                                    GetFormData.ExtraCol2Text = null;
                                    GetData.IsExtraCol2 = false;
                                    GetData.IsExtraCol2Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol3Text")
                                {
                                    GetFormData.ExtraCol3Text = null;
                                    GetData.IsExtraCol3 = false;
                                    GetData.IsExtraCol3Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol4Text")
                                {
                                    GetFormData.ExtraCol4Text = null;
                                    GetData.IsExtraCol4 = false;
                                    GetData.IsExtraCol4Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol5Text")
                                {
                                    GetFormData.ExtraCol5Text = null;
                                    GetData.IsExtraCol5 = false;
                                    GetData.IsExtraCol5Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol6Text")
                                {
                                    GetFormData.ExtraCol6Text = null;
                                    GetData.ISExtraCol6 = false;
                                    GetData.ISExtraCol6Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol7Text")
                                {
                                    GetFormData.ExtraCol7Text = null;
                                    GetData.ISExtraCol7 = false;
                                    GetData.ISExtraCol7Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol8Text")
                                {
                                    GetFormData.ExtraCol8Text = null;
                                    GetData.ISExtraCol8 = false;
                                    GetData.ISExtraCol8Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol9Text")
                                {
                                    GetFormData.ExtraCol9Text = null;
                                    GetData.IsExtraCol9 = false;
                                    GetData.IsExtraCol9Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol10Text")
                                {
                                    GetFormData.ExtraCol10Text = null;
                                    GetData.IsExtraCol10 = false;
                                    GetData.IsExtraCol10Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol11Text")
                                {
                                    GetFormData.ExtraCol11Text = null;
                                    GetData.IsExtraCol11 = false;
                                    GetData.IsExtraCol11Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraCol12Text")
                                {
                                    GetFormData.ExtraCol12Text = null;
                                    GetData.IsExtraCol12 = false;
                                    GetData.IsExtraCol12Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraColdropdown1Text")
                                {
                                    GetFormData.ExtraColdropdown1Text = null;
                                    GetData.IsExtraColdropdown1 = false;
                                    GetData.IsExtraColdropdown1Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraColdropdown2Text")
                                {
                                    GetFormData.ExtraColdropdown2Text = null;
                                    GetData.IsExtraColdropdown2 = false;
                                    GetData.IsExtraColdropdown2Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraColdropdown3Text")
                                {
                                    GetFormData.ExtraColdropdown3Text = null;
                                    GetData.IsExtraColdropdown3 = false;
                                    GetData.IsExtraColdropdown3Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraColdropdown4Text")
                                {
                                    GetFormData.ExtraColdropdown4Text = null;
                                    GetData.IsExtraColdropdown4 = false;
                                    GetData.IsExtraColdropdown4Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                else if (FieldName == "ExtraColdropdown5Text")
                                {
                                    GetFormData.ExtraColdropdown5Text = null;
                                    GetData.IsExtraColdropdown5 = false;
                                    GetData.IsExtraColdropdown5Required = false;
                                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                    GetData.ModifiedOn = Constant.GetBharatTime();
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    msg = "ok";
                                }
                                //else if (FieldName == "ImageCol1Text")
                                //{
                                //    GetFormData.ImageCol1Text = null;
                                //    GetData.IsImageCol1 = false;
                                //    GetData.IsImageCol1Required =  false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    GetSeqData.Priority = 0;
                                //    db.SaveChanges();
                                //    trans.Commit();
                                //    msg = "ok";
                                //}
                                //else if (FieldName == "ImageCol2Text")
                                //{
                                //    GetFormData.ImageCol2Text = null;
                                //    GetData.IsImageCol2 = false;
                                //    GetData.IsImageCol2Required =  false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    GetSeqData.Priority = 0;
                                //    db.SaveChanges();
                                //    trans.Commit();
                                //    msg = "ok";
                                //}
                                //else if (FieldName == "ImageCol3Text")
                                //{
                                //    GetFormData.ImageCol3Text = null;
                                //    GetData.IsImageCol3 = false;
                                //    GetData.IsImageCol3Required =  false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    GetSeqData.Priority = 0;
                                //    db.SaveChanges();
                                //    trans.Commit();
                                //    msg = "ok";
                                //}
                                //else if (FieldName == "ImageCol4Text")
                                //{
                                //    GetFormData.ImageCol4Text = null;
                                //    GetData.IsImageCol4 = false;
                                //    GetData.IsImageCol4Required =  false;
                                //    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
                                //    GetData.ModifiedOn = Constant.GetBharatTime();
                                //    GetSeqData.Priority = 0;
                                //    db.SaveChanges();
                                //    trans.Commit();
                                //    msg = "ok";                                    
                                //}
                            }
                            else
                            {
                                msg = "err";
                                //return Json("err", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            msg = "err";
                            //return Json("err", JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ExceptionLogging.SendExcepToDB(ex);
                        msg = "err";
                    }
                }
            }
            else
            {
                msg = "err";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FieldPriority(string fieldName, int Priority)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var uid = Convert.ToInt32(Session["UID"]);
            string msg = "";
            try
            {
                var GetSeqData = db.crm_ticket_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == fieldName).FirstOrDefault();
                if (GetSeqData != null)
                {
                    GetSeqData.Priority = Priority;
                    GetSeqData.ModifiedDate = Constant.GetBharatTime();
                    db.SaveChanges();
                }
                else
                {
                    var fp = new crm_ticket_field_sequence
                    {
                        Priority = Priority,
                        FieldName = fieldName,
                        CompanyID = CompanyID,
                        BranchID = BranchID,
                        CreatedBy = uid,
                        Createddate = Constant.GetBharatTime(),
                        ModifiedDate = Constant.GetBharatTime()
                    };
                    db.crm_ticket_field_sequence.Add(fp);
                    db.SaveChanges();
                }
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult ViewTicketddl(string fieldname, string fieldtext)
        {
            TicketItemDropdownModel TPM = new TicketItemDropdownModel();
            try
            {
                TempData["fieldname"] = fieldname;
                TempData["fieldtext"] = fieldtext;
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                ViewBag.result = db.crm_ticketdropdownlist_tbl.Where(em => em.T_CompanyID == CompanyID && em.T_BranchID == BranchID && em.T_DropDownfield == fieldtext).ToList();
            }
            catch (Exception ex)
            {

            }
            return PartialView("_TicketDropDownModule", TPM);
        }
        public ActionResult ManageItemModule(string fieldname, string fieldtext, string Itemname, int Id)
        {
            string msg = "";
            try
            {
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                if (Id > 0)
                {

                    var getdata = db.crm_ticketdropdownlist_tbl.Where(em => em.T_CompanyID == CompanyID && em.T_BranchID == BranchID && em.T_DropdownitemId == Id).FirstOrDefault();
                    if (getdata != null)
                    {
                        getdata.T_DropDownItemName = Itemname;
                        db.SaveChanges();
                        msg = "Item Updated Successfully";
                    }
                }
                else
                {
                    crm_ticketdropdownlist_tbl CPT = new crm_ticketdropdownlist_tbl();
                    CPT.T_DropdownName = fieldname;
                    CPT.T_DropDownfield = fieldtext;
                    CPT.T_DropDownItemName = Itemname;
                    CPT.T_CompanyID = CompanyID;
                    CPT.T_BranchID = BranchID;
                    CPT.Status = false;
                    CPT.T_created_at = Constant.GetBharatTime();// System.DateTime.Now;                    
                    db.crm_ticketdropdownlist_tbl.Add(CPT);
                    if (db.SaveChanges() > 0)
                    {
                        msg = "Item Added Successfully";

                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
        public ActionResult RefreshItemName(string fieldtext)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var data = db.crm_ticketdropdownlist_tbl.Where(em => em.T_CompanyID == CompanyID && em.T_BranchID == BranchID && em.T_DropDownfield == fieldtext).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Ticket_changeStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_ticketdropdownlist_tbl set Status=case when Status=1 then 0 else 1 end where T_DropdownitemId=" + id);
                msg = "ok";
            }
            catch (Exception ex)
            {
                //Models.ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdatedItemName(int Itemid)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var result = db.crm_ticketdropdownlist_tbl.Where(em => em.T_CompanyID == CompanyID && em.T_BranchID == BranchID && em.T_DropdownitemId == Itemid).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region old code not in use
        #region old view ticket setting code
        //public async Task<ActionResult> ViewTicketSettingOld() 
        //{

        //    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //    var model = new ViewTecketSettingDTO();
        //    if (Session["BranchID"] != null && Session["CompanyID"] != null)
        //    {
        //        var GetData = await db.crm_ticketviewsetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
        //        var GetFormData = await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();

        //        if (GetData != null)
        //        {
        //            var data = Mapper.Map<ViewTecketSettingDTO>(GetData);
        //            model = data;
        //        }
        //        if (GetFormData != null)
        //        {
        //            model.NameText = GetFormData.NameText;
        //            model.EmailIDText = GetFormData.EmailIDText;
        //            model.PhoneNumberText = GetFormData.PhoneNumberText;
        //            model.ErrorTypeIDText = GetFormData.ErrorTypeIDText;
        //            model.ProductTypeIDText = GetFormData.ProductTypeIDText;
        //            model.UrgencyIDText = GetFormData.UrgencyIDText;
        //            model.subjectText = GetFormData.subjectText;
        //            model.StatusIDText = GetFormData.StatusIDText;
        //            model.CustomerIDText = GetFormData.CustomerIDText;
        //            model.ExtraCol1Text = GetFormData.ExtraCol1Text;
        //            model.ExtraCol2Text = GetFormData.ExtraCol2Text;
        //            model.ExtraCol3Text = GetFormData.ExtraCol3Text;
        //            model.ExtraCol4Text = GetFormData.ExtraCol4Text;
        //            model.ExtraCol5Text = GetFormData.ExtraCol5Text;
        //            model.ExtraCol6Text = GetFormData.ExtraCol6Text;
        //            model.ExtraCol7Text = GetFormData.ExtraCol7Text;
        //            model.ExtraCol8Text = GetFormData.ExtraCol8Text;
        //            model.ExtraCol9Text = GetFormData.ExtraCol9Text;
        //            model.ExtraCol10Text = GetFormData.ExtraCol10Text;                    
        //            model.ExtraCol11Text = GetFormData.ExtraCol11Text;
        //            model.ExtraCol12Text = GetFormData.ExtraCol12Text;
        //            model.ImageCol1Text = GetFormData.ImageCol1Text;
        //            model.ImageCol2Text = GetFormData.ImageCol2Text;
        //            model.ImageCol3Text = GetFormData.ImageCol3Text;
        //            model.ImageCol4Text = GetFormData.ImageCol4Text;

        //        }
        //    }
        //    else
        //    {
        //        return Redirect("/home/login");
        //    }

        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<ActionResult> ViewTicketSettingOld(ViewTecketSettingDTO model)
        //{
        //    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //    if (Session["BranchID"] != null && Session["CompanyID"] != null)
        //    {
        //        try
        //        {

        //            if (model.Id > 0)
        //            {
        //                var GetData = await db.crm_ticketviewsetting.FindAsync(model.Id);
        //                if (GetData != null)
        //                {
        //                    var data = Mapper.Map<crm_ticketviewsetting>(model);
        //                    data.BranchID = BranchID;
        //                    data.CompanyId = CompanyID;
        //                    data.IsName = true;
        //                    data.IsPhoneNumber = true;

        //                    db.crm_ticketviewsetting.AddOrUpdate(data);
        //                    //db.Entry(data).State = System.Data.Entity.EntityState.Modified;
        //                    await db.SaveChangesAsync();
        //                    TempData["success"] = "Data updated successfully";
        //                }
        //            }
        //            else
        //            {
        //                var data = Mapper.Map<crm_ticketviewsetting>(model);
        //                data.CreatedOn = Constant.GetBharatTime();
        //                data.BranchID = BranchID;
        //                data.CompanyId = CompanyID;
        //                data.IsName = true;
        //                data.IsPhoneNumber = true;
        //                db.crm_ticketviewsetting.Add(data);
        //                int i = await db.SaveChangesAsync();
        //                if (i > 0)
        //                {
        //                    TempData["success"] = "Data added successfully";
        //                }
        //                else
        //                {
        //                    TempData["alert"] = "Sorry! there is some technical problem";
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionLogging.SendExcepToDB(ex);
        //            TempData["alert"] = "Sorry! there is some technical problem";
        //        }
        //    }
        //    else
        //    {
        //        return Redirect("/home/login");
        //    }

        //    return RedirectToAction("ViewTicketSetting");
        //}
        #endregion
        #region old create code
        //public async Task<ActionResult> CreateTicketSettingOld() 
        //{
        //    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

        //    CreateTicketSettingDTO model = new CreateTicketSettingDTO();
        //    if (Session["BranchID"] != null && Session["CompanyID"] != null)
        //    {
        //        var GetData = await db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
        //        var GetFormData = await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();

        //        if (GetData != null)
        //        {
        //            var data = Mapper.Map<CreateTicketSettingDTO>(GetData);
        //            model = data;

        //            //model.IsExtraCol1 = GetData.IsExtraCol1;
        //            //model.IsExtraCol2 = GetData.IsExtraCol2;
        //            //model.IsExtraCol3 = GetData.IsExtraCol3;
        //            //model.IsExtraCol4 = GetData.IsExtraCol4;
        //            //model.IsExtraCol5 = GetData.IsExtraCol5;
        //            //model.ISExtraCol6 = GetData.ISExtraCol6;
        //            //model.ISExtraCol7 = GetData.ISExtraCol7;
        //            //model.ISExtraCol8 = GetData.ISExtraCol8;
        //            //model.IsExtraCol9 = GetData.IsExtraCol9;
        //            //model.IsExtraCol10 = GetData.IsExtraCol10;
        //            //model.IsExtraCol1Required = GetData.IsExtraCol1Required;
        //            //model.IsExtraCol2Required = GetData.IsExtraCol2Required;
        //            //model.IsExtraCol3Required = GetData.IsExtraCol3Required;
        //            //model.IsExtraCol4Required = GetData.IsExtraCol4Required;
        //            //model.IsExtraCol5Required = GetData.IsExtraCol5Required;
        //            //model.ISExtraCol6Required = GetData.ISExtraCol6Required;
        //            //model.ISExtraCol7Required = GetData.ISExtraCol7Required;
        //            //model.ISExtraCol8Required = GetData.ISExtraCol8Required;
        //            //model.IsExtraCol9Required = GetData.IsExtraCol9Required;
        //            //model.IsExtraCol10Required = GetData.IsExtraCol10Required;
        //        }

        //        if (GetFormData != null)
        //        {
        //            model.NameText = GetFormData.NameText;
        //            model.EmailIDText = GetFormData.EmailIDText;
        //            model.PhoneNumberText = GetFormData.PhoneNumberText;
        //            model.ErrorTypeIDText = GetFormData.ErrorTypeIDText;
        //            model.ProductTypeIDText = GetFormData.ProductTypeIDText;
        //            model.UrgencyIDText = GetFormData.UrgencyIDText;
        //            model.subjectText = GetFormData.subjectText;
        //            model.StatusIDText = GetFormData.StatusIDText;
        //            model.CustomerIDText = GetFormData.CustomerIDText;
        //            model.ExtraCol1Text = GetFormData.ExtraCol1Text;
        //            model.ExtraCol2Text = GetFormData.ExtraCol2Text;
        //            model.ExtraCol3Text = GetFormData.ExtraCol3Text;
        //            model.ExtraCol4Text = GetFormData.ExtraCol4Text;
        //            model.ExtraCol5Text = GetFormData.ExtraCol5Text;
        //            model.ExtraCol6Text = GetFormData.ExtraCol6Text;
        //            model.ExtraCol7Text = GetFormData.ExtraCol7Text;
        //            model.ExtraCol8Text = GetFormData.ExtraCol8Text;
        //            model.ExtraCol9Text = GetFormData.ExtraCol9Text;
        //            model.ExtraCol10Text = GetFormData.ExtraCol10Text;                    
        //            model.ExtraCol11Text = GetFormData.ExtraCol11Text;
        //            model.ExtraCol12Text = GetFormData.ExtraCol12Text;
        //            model.ImageCol1Text = GetFormData.ImageCol1Text;
        //            model.ImageCol2Text = GetFormData.ImageCol2Text;
        //            model.ImageCol3Text = GetFormData.ImageCol3Text;
        //            model.ImageCol4Text = GetFormData.ImageCol4Text;
        //        }
        //        model.IsName = true;
        //        model.IsPhoneNumber = true;
        //        model.IsProductTypeID = true;
        //        model.IsProductTypeIDRequired = true;
        //        model.IsErrorTypeID = true;
        //        model.IsErrorTypeIDRequired = true;
        //        model.IsUrgencyID = true;
        //        model.IsErrorTypeIDRequired = true;
        //        model.IsStatusID = true;
        //        model.IsStatusIDRequired = true;

        //    }
        //    else
        //    {
        //        return Redirect("/home/login");
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult  CreateTicketSettingOld(CreateTicketSettingDTO model)
        //{
        //    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //    string msg = "";
        //    if (Session["BranchID"] != null && Session["CompanyID"] != null)
        //    {
        //        using (var trans = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                model.IsName = true;
        //                model.IsPhoneNumber = true;
        //                model.IsProductTypeID = true;
        //                model.IsProductTypeIDRequired = true;
        //                model.IsErrorTypeID = true;
        //                model.IsErrorTypeIDRequired = true;
        //                model.IsUrgencyID = true;
        //                model.IsErrorTypeIDRequired = true;
        //                model.IsStatusID = true;
        //                model.IsStatusIDRequired = true;

        //                var GetData = db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
        //                var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefault();

        //                if (GetData != null)
        //                {
        //                    GetData.IsEmailID = model.IsEmailID;
        //                    GetData.Issubject = model.Issubject;
        //                    GetData.IsEmailIDRequired = model.IsEmailIDRequired;
        //                    GetData.IssubjectRequired = model.IssubjectRequired;
        //                    GetData.IsExtraCol1 = model.IsExtraCol1;
        //                    GetData.IsExtraCol2 = model.IsExtraCol2;
        //                    GetData.IsExtraCol3 = model.IsExtraCol3;
        //                    GetData.IsExtraCol4 = model.IsExtraCol4;
        //                    GetData.IsExtraCol5 = model.IsExtraCol5;
        //                    GetData.ISExtraCol6 = model.ISExtraCol6;
        //                    GetData.ISExtraCol7 = model.ISExtraCol7;
        //                    GetData.ISExtraCol8 = model.ISExtraCol8;
        //                    GetData.IsExtraCol9 = model.IsExtraCol9;
        //                    GetData.IsExtraCol10 = model.IsExtraCol10;
        //                    GetData.IsExtraCol11 = model.IsExtraCol11;
        //                    GetData.IsExtraCol12 = model.IsExtraCol12;
        //                    GetData.IsImageCol1 = model.IsImageCol1;
        //                    GetData.IsImageCol2 = model.IsImageCol2;
        //                    GetData.IsImageCol3 = model.IsImageCol3;
        //                    GetData.IsImageCol4 = model.IsImageCol4;
        //                    GetData.IsExtraCol1Required = model.IsExtraCol1Required;
        //                    GetData.IsExtraCol2Required = model.IsExtraCol2Required;
        //                    GetData.IsExtraCol3Required = model.IsExtraCol3Required;
        //                    GetData.IsExtraCol4Required = model.IsExtraCol4Required;
        //                    GetData.IsExtraCol5Required = model.IsExtraCol5Required;
        //                    GetData.ISExtraCol6Required = model.ISExtraCol6Required;
        //                    GetData.ISExtraCol7Required = model.ISExtraCol7Required;
        //                    GetData.ISExtraCol8Required = model.ISExtraCol8Required;
        //                    GetData.IsExtraCol9Required = model.IsExtraCol9Required;
        //                    GetData.IsExtraCol10Required = model.IsExtraCol10Required;
        //                    GetData.IsExtraCol11Required = model.IsExtraCol11Required;
        //                    GetData.IsExtraCol12Required = model.IsExtraCol12Required;
        //                    GetData.IsImageCol1Required = model.IsImageCol1Required;
        //                    GetData.IsImageCol2Required = model.IsImageCol2Required;
        //                    GetData.IsImageCol3Required = model.IsImageCol3Required;
        //                    GetData.IsImageCol4Required = model.IsImageCol4Required;

        //                    GetData.ModifiedBy = Convert.ToInt32(Session["UID"]);
        //                    GetData.ModifiedOn = Constant.GetBharatTime();
        //                    msg = "Data updated successfully";
        //                    db.SaveChanges();
        //                }
        //                else
        //                {
        //                    crm_ticketcreatesetting GetData1 = new crm_ticketcreatesetting();
        //                    GetData1.BranchID = BranchID;
        //                    GetData1.CompanyId = CompanyID;
        //                    GetData1.CreatedOn = Constant.GetBharatTime();
        //                    GetData1.CreatedBy = Convert.ToInt32(Session["UID"]);
        //                    GetData1.IsEmailID = model.IsEmailID;
        //                    GetData1.Issubject = model.Issubject;
        //                    GetData1.IsEmailIDRequired = model.IsEmailIDRequired;
        //                    GetData1.IssubjectRequired = model.IssubjectRequired;
        //                    GetData1.IsExtraCol1 = model.IsExtraCol1;
        //                    GetData1.IsExtraCol2 = model.IsExtraCol2;
        //                    GetData1.IsExtraCol3 = model.IsExtraCol3;
        //                    GetData1.IsExtraCol4 = model.IsExtraCol4;
        //                    GetData1.IsExtraCol5 = model.IsExtraCol5;
        //                    GetData1.ISExtraCol6 = model.ISExtraCol6;
        //                    GetData1.ISExtraCol7 = model.ISExtraCol7;
        //                    GetData1.ISExtraCol8 = model.ISExtraCol8;
        //                    GetData1.IsExtraCol9 = model.IsExtraCol9;
        //                    GetData1.IsExtraCol10 = model.IsExtraCol10;
        //                    GetData1.IsExtraCol11 = model.IsExtraCol11;
        //                    GetData1.IsExtraCol12 = model.IsExtraCol12;
        //                    GetData1.IsImageCol1 = model.IsImageCol1;
        //                    GetData1.IsImageCol2 = model.IsImageCol2;
        //                    GetData1.IsImageCol3 = model.IsImageCol3;
        //                    GetData1.IsImageCol4 = model.IsImageCol4;
        //                    GetData1.IsExtraCol1Required = model.IsExtraCol1Required;
        //                    GetData1.IsExtraCol2Required = model.IsExtraCol2Required;
        //                    GetData1.IsExtraCol3Required = model.IsExtraCol3Required;
        //                    GetData1.IsExtraCol4Required = model.IsExtraCol4Required;
        //                    GetData1.IsExtraCol5Required = model.IsExtraCol5Required;
        //                    GetData1.ISExtraCol6Required = model.ISExtraCol6Required;
        //                    GetData1.ISExtraCol7Required = model.ISExtraCol7Required;
        //                    GetData1.ISExtraCol8Required = model.ISExtraCol8Required;
        //                    GetData1.IsExtraCol9Required = model.IsExtraCol9Required;
        //                    GetData1.IsExtraCol10Required = model.IsExtraCol10Required;
        //                    GetData1.IsExtraCol11Required = model.IsExtraCol11Required;
        //                    GetData1.IsExtraCol12Required = model.IsExtraCol12Required;
        //                    GetData1.IsImageCol1Required = model.IsImageCol1Required;
        //                    GetData1.IsImageCol2Required = model.IsImageCol2Required;
        //                    GetData1.IsImageCol3Required = model.IsImageCol3Required;
        //                    GetData1.IsImageCol4Required = model.IsImageCol4Required;
        //                    db.crm_ticketcreatesetting.Add(GetData1);
        //                    db.SaveChanges();
        //                    if (GetFormData == null)
        //                    {
        //                        msg = "Data added successfully";
        //                    }
        //                }

        //                if (GetFormData != null)
        //                {
        //                    GetFormData.NameText = model.NameText;
        //                    GetFormData.EmailIDText = model.EmailIDText;
        //                    GetFormData.PhoneNumberText = model.PhoneNumberText;
        //                    GetFormData.ErrorTypeIDText = model.ErrorTypeIDText;
        //                    GetFormData.ProductTypeIDText = model.ProductTypeIDText;
        //                    GetFormData.UrgencyIDText = model.UrgencyIDText;
        //                    GetFormData.subjectText = model.subjectText;
        //                    GetFormData.StatusIDText = model.StatusIDText;
        //                    GetFormData.CustomerIDText = model.CustomerIDText;
        //                    GetFormData.ExtraCol1Text = model.ExtraCol1Text;
        //                    GetFormData.ExtraCol2Text = model.ExtraCol2Text;
        //                    GetFormData.ExtraCol3Text = model.ExtraCol3Text;
        //                    GetFormData.ExtraCol4Text = model.ExtraCol4Text;
        //                    GetFormData.ExtraCol5Text = model.ExtraCol5Text;
        //                    GetFormData.ExtraCol6Text = model.ExtraCol6Text;
        //                    GetFormData.ExtraCol7Text = model.ExtraCol7Text;
        //                    GetFormData.ExtraCol8Text = model.ExtraCol8Text;
        //                    GetFormData.ExtraCol9Text = model.ExtraCol9Text;
        //                    GetFormData.ExtraCol10Text = model.ExtraCol10Text;
        //                    GetFormData.ExtraCol11Text = model.ExtraCol11Text;
        //                    GetFormData.ExtraCol12Text = model.ExtraCol12Text;
        //                    GetFormData.ImageCol1Text = model.ImageCol1Text;
        //                    GetFormData.ImageCol2Text = model.ImageCol2Text;
        //                    GetFormData.ImageCol3Text = model.ImageCol3Text;
        //                    GetFormData.ImageCol4Text = model.ImageCol4Text;
        //                    GetFormData.ModifiedBy = Convert.ToInt32(Session["UID"]);
        //                    GetFormData.ModifiedOn = Constant.GetBharatTime();
        //                    db.SaveChanges();

        //                    msg = "Data updated successfully";

        //                }
        //                else
        //                {
        //                    crm_ticketfieldnamecustomized GetFormData2 = new crm_ticketfieldnamecustomized();
        //                    GetFormData2.BranchID = BranchID;
        //                    GetFormData2.CompanyId = CompanyID;
        //                    GetFormData2.CreatedOn = Constant.GetBharatTime();
        //                    GetFormData2.CreatedBy = Convert.ToInt32(Session["UID"]);
        //                    GetFormData2.NameText = model.NameText;
        //                    GetFormData2.EmailIDText = model.EmailIDText;
        //                    GetFormData2.PhoneNumberText = model.PhoneNumberText;
        //                    GetFormData2.ErrorTypeIDText = model.ErrorTypeIDText;
        //                    GetFormData2.ProductTypeIDText = model.ProductTypeIDText;
        //                    GetFormData2.UrgencyIDText = model.UrgencyIDText;
        //                    GetFormData2.subjectText = model.subjectText;
        //                    GetFormData2.StatusIDText = model.StatusIDText;
        //                    GetFormData2.CustomerIDText = model.CustomerIDText;
        //                    GetFormData2.ExtraCol1Text = model.ExtraCol1Text;
        //                    GetFormData2.ExtraCol2Text = model.ExtraCol2Text;
        //                    GetFormData2.ExtraCol3Text = model.ExtraCol3Text;
        //                    GetFormData2.ExtraCol4Text = model.ExtraCol4Text;
        //                    GetFormData2.ExtraCol5Text = model.ExtraCol5Text;
        //                    GetFormData2.ExtraCol6Text = model.ExtraCol6Text;
        //                    GetFormData2.ExtraCol7Text = model.ExtraCol7Text;
        //                    GetFormData2.ExtraCol8Text = model.ExtraCol8Text;
        //                    GetFormData2.ExtraCol9Text = model.ExtraCol9Text;
        //                    GetFormData2.ExtraCol10Text = model.ExtraCol10Text;
        //                    GetFormData2.ExtraCol11Text = model.ExtraCol11Text;
        //                    GetFormData2.ExtraCol12Text = model.ExtraCol12Text;
        //                    GetFormData2.ImageCol1Text = model.ImageCol1Text;
        //                    GetFormData2.ImageCol2Text = model.ImageCol2Text;
        //                    GetFormData2.ImageCol3Text = model.ImageCol3Text;
        //                    GetFormData2.ImageCol4Text = model.ImageCol4Text;

        //                    db.crm_ticketfieldnamecustomized.Add(GetFormData2);
        //                    db.SaveChanges();
        //                    if (GetData == null)
        //                    {
        //                        msg = "Data added successfully";
        //                    }
        //                }
        //                trans.Commit();
        //                TempData["success"] = msg;

        //            }
        //            catch (Exception ex)
        //            {
        //                trans.Rollback();
        //                ExceptionLogging.SendExcepToDB(ex);
        //                TempData["alert"] = "Sorry! there is some technical problem";
        //            }
        //        }

        //    }
        //    else
        //    {
        //        return Redirect("/home/login");
        //    }

        //    return RedirectToAction("CreateTicketSetting");
        //}
        #endregion
        #endregion
    }
}
