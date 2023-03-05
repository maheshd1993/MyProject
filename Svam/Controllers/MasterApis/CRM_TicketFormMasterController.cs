using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.Models.DTO;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Svam.Controllers.MasterApis
{
    public class CRM_TicketFormMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_TicketFormMaster/GetTktFormFields?CompanyID=307&BranchID=184&UID=61&Token=VwFdB3OPEwOoHnr15a5qgg==
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> GetTktFormFields(string CompanyID, string BranchID,string UID, string Token) 
        {
            string ErrorMessage = string.Empty;
           
            var fieldsList = new List<TktFormFieldsModel>();

            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);
                var uid =string.IsNullOrEmpty(UID)? Convert.ToInt32(UID):0;
                //System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //     Token = headers.GetValues("Token").First();
                //}

                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                var dt= Constant.GetimeForApi(companyID);
                var fieldPriorityList = new List<LeadFieldPriorityDTO>();

                var GetData = await db.crm_ticketcreatesetting.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefaultAsync();
                var GetFormData = await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefaultAsync();
                var GetSeqData = await db.crm_ticket_field_sequence.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToListAsync();
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
                    GetData1.BranchID = branchID;
                    GetData1.CompanyId = companyID;
                    GetData1.CreatedOn = dt;
                    GetData1.CreatedBy = uid;
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
                    GetFormData2.BranchID = branchID;
                    GetFormData2.CompanyId = companyID;
                    GetFormData2.CreatedOn = dt;
                    GetFormData2.CreatedBy = uid;

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
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 21, FieldName = "ImageCol1Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 22, FieldName = "ImageCol2Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 23, FieldName = "ImageCol3Text" });
                    //fieldPriorityList.Add(new LeadFieldPriorityDTO { Priority = 24, FieldName = "ImageCol4Text" });

                   
                    foreach (var item in fieldPriorityList)
                    {
                        var fp = new crm_ticket_field_sequence
                        {
                            Priority = item.Priority,
                            FieldName = item.FieldName,
                            CompanyID = companyID,
                            BranchID = branchID,
                            CreatedBy = uid,
                            Createddate = dt
                           
                        };
                        db.crm_ticket_field_sequence.Add(fp);
                    }
                    db.SaveChanges();
                }

                #region fields names list for show dropdown

                string NameText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";
                int? cnmNo = fieldPriorityList.Where(a => a.FieldName == "NameText").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = NameText, ColumnName = "NameText",IsActive=true,IsRequired= true, FieldType="NormalText", Priority = cnmNo,CanChangeFieldType=false,CanDeleteThisField=false,AlwaysActive=true,AlwaysRequired=true });

                bool IsEmail = (GetData == null || GetData.IsEmailID) ? true : false;
                bool IsEmailReq = GetData == null || GetData.IsEmailIDRequired ? true : false;
                string EmailIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";
                int? emlNo = fieldPriorityList.Where(a => a.FieldName == "EmailIDText").Select(a => a.Priority).FirstOrDefault();
                fieldsList.Add(new TktFormFieldsModel { TextName = EmailIDText, ColumnName = "EmailIDText",IsActive= IsEmail ,IsRequired=IsEmailReq ,FieldType= "EmailText", Priority = emlNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = false, AlwaysRequired = false });

                string PhoneNumberText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";
                int? mobNo = fieldPriorityList.Where(a => a.FieldName == "PhoneNumberText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new TktFormFieldsModel { TextName = PhoneNumberText, ColumnName = "PhoneNumberText", IsActive=true, IsRequired=true,FieldType="NormalText", Priority = mobNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                string ProductTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";
                int? ptNo = fieldPriorityList.Where(a => a.FieldName == "ProductTypeIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new TktFormFieldsModel { TextName = ProductTypeIDText, ColumnName = "ProductTypeIDText",IsActive = true, IsRequired = true, FieldType = "DropDownList", Priority = ptNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });


                string ErrorTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";
                int? errTNo = fieldPriorityList.Where(a => a.FieldName == "ErrorTypeIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new TktFormFieldsModel { TextName = ErrorTypeIDText, ColumnName = "ErrorTypeIDText",IsActive=true, IsRequired=true,FieldType="DropDownList", Priority = errTNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                string UrgencyIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";
                int? urgNo = fieldPriorityList.Where(a => a.FieldName == "UrgencyIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new TktFormFieldsModel { TextName = UrgencyIDText, ColumnName = "UrgencyIDText", IsActive = true, IsRequired = true, FieldType = "DropDownList", Priority = urgNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });


                string StatusIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";
                int? stsNo = fieldPriorityList.Where(a => a.FieldName == "StatusIDText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new TktFormFieldsModel { TextName = StatusIDText, ColumnName = "StatusIDText",IsActive = true, IsRequired = true, FieldType = "DropDownList", Priority = stsNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });

                //string Issubject = (GetData == null || GetData.Issubject) ? "true" : "false";
                //string IssubjectRequired  = (GetData == null || GetData.IssubjectRequired) ? "true" : "false";
                string subjectText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";
                //fieldsList.Add(new TktFormFieldsModel { TextName = subjectText, Values = "subjectText/" + Issubject + "/" + IssubjectRequired + "/NormalText" });
                int? subNo = fieldPriorityList.Where(a => a.FieldName == "subjectText").Select(a => a.Priority).FirstOrDefault();

                fieldsList.Add(new TktFormFieldsModel { TextName = subjectText, ColumnName = "subjectText", IsActive = true, IsRequired = true, FieldType = "NormalText", Priority = subNo, CanChangeFieldType = false, CanDeleteThisField = false, AlwaysActive = true, AlwaysRequired = true });


               
                string ExtraCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol1Text))
                {
                    bool IsExtraCol1 = (GetData != null && GetData.IsExtraCol1) ? true : false;
                    bool IsExtraCol1Required = (GetData != null && GetData.IsExtraCol1Required) ? true : false;
                    int? ex1No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol1Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol1Text, ColumnName = "ExtraCol1Text",IsActive=IsExtraCol1 ,IsRequired=IsExtraCol1Required,FieldType="NormalText", Priority = ex1No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //model.NormalFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol1Text))
                //{
                //    model.NormalFieldCount = 1;
                //}

                
                string ExtraCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol2Text))
                {
                    bool IsExtraCol2 = (GetData != null && GetData.IsExtraCol2) ? true : false;
                    bool IsExtraCol2Required = (GetData != null && GetData.IsExtraCol2Required) ? true : false;
                    int? ex2No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol2Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol2Text, ColumnName = "ExtraCol2Text",IsActive=IsExtraCol2 ,IsRequired=IsExtraCol2Required,FieldType= "NormalText", Priority = ex2No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text))
                //{
                //    model.NormalFieldCount = 2;
                //}

                string ExtraCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol3Text))
                {
                    bool IsExtraCol3 = (GetData != null && GetData.IsExtraCol3) ? true : false;
                    bool IsExtraCol3Required = (GetData != null && GetData.IsExtraCol3Required) ? true : false;
                    int? ex3No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol3Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol3Text, ColumnName = "ExtraCol3Text", IsActive = IsExtraCol3, IsRequired = IsExtraCol3Required, FieldType = "NormalText", Priority = ex3No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text))
                //{
                //    model.NormalFieldCount = 3;
                //}
                string ExtraCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol4Text))
                {
                    bool IsExtraCol4 = (GetData != null && GetData.IsExtraCol4) ? true : false;
                    bool IsExtraCol4Required = (GetData != null && GetData.IsExtraCol4Required) ? true : false;
                    int? ex4No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol4Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol4Text, ColumnName = "ExtraCol4Text", IsActive=IsExtraCol4, IsRequired=IsExtraCol4Required,FieldType= "NormalText", Priority = ex4No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text) && string.IsNullOrEmpty(ExtraCol4Text))
                //{
                //    model.NormalFieldCount = 4;
                //}

                string ExtraCol5Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol5Text))
                {
                    bool IsExtraCol5 = (GetData != null && GetData.IsExtraCol5) ? true : false;
                    bool IsExtraCol5Required = (GetData != null && GetData.IsExtraCol5Required) ? true : false;
                    int? ex5No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol5Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol5Text, ColumnName = "ExtraCol5Text",IsActive=IsExtraCol5 ,IsRequired= IsExtraCol5Required,FieldType= "NormalText", Priority = ex5No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text) && string.IsNullOrEmpty(ExtraCol4Text) && string.IsNullOrEmpty(ExtraCol5Text))
                //{
                //    model.NormalFieldCount = 5;
                //}
                string ExtraCol6Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol6Text))
                {
                    bool IsExtraCol6 = (GetData != null && GetData.ISExtraCol6) ? true : false;
                    bool IsExtraCol6Required = (GetData != null && GetData.ISExtraCol6Required) ? true : false;
                    int? ex6No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol6Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol6Text, ColumnName = "ExtraCol6Text",IsActive=IsExtraCol6 ,IsRequired=IsExtraCol6Required, FieldType="NormalText", Priority = ex6No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol1Text) && string.IsNullOrEmpty(ExtraCol2Text) && string.IsNullOrEmpty(ExtraCol3Text) && string.IsNullOrEmpty(ExtraCol4Text) && string.IsNullOrEmpty(ExtraCol5Text) && string.IsNullOrEmpty(ExtraCol6Text))
                //{
                //    model.NormalFieldCount = 6;
                //}
                string ExtraCol7Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol7Text))
                {
                    bool IsExtraCol7 = (GetData != null && GetData.ISExtraCol7) ? true : false;
                    bool IsExtraCol7Required = (GetData != null && GetData.ISExtraCol7Required) ? true : false;
                    int? ex7No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol7Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol7Text, ColumnName = "ExtraCol7Text", IsActive = IsExtraCol7, IsRequired = IsExtraCol7Required, FieldType = "DecimalText", Priority = ex7No, CanChangeFieldType = true,CanDeleteThisField=true, AlwaysActive = false, AlwaysRequired = false });
                }

                //model.DecimalFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol7Text))
                //{
                //    model.NumericFieldCount = 1;
                //}
                string ExtraCol8Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol8Text))
                {
                    bool IsExtraCol8 = (GetData != null && GetData.ISExtraCol8) ? true : false;
                    bool IsExtraCol8Required = (GetData != null && GetData.ISExtraCol8Required) ? true : false;
                    int? ex8No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol8Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol8Text, ColumnName = "ExtraCol8Text",IsActive=IsExtraCol8,IsRequired=IsExtraCol8Required, FieldType= "DecimalText", Priority = ex8No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol7Text) && string.IsNullOrEmpty(ExtraCol8Text))
                //{
                //    model.NumericFieldCount = 2;
                //}
                string ExtraCol9Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol9Text))
                {
                    bool IsExtraCol9 = (GetData != null && GetData.IsExtraCol9) ? true : false;
                    bool IsExtraCol9Required = (GetData != null && GetData.IsExtraCol9Required) ? true : false;
                    int? ex9No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol9Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol9Text, ColumnName = "ExtraCol9Text",IsActive=IsExtraCol9 ,IsRequired= IsExtraCol9Required ,FieldType= "DateText", Priority = ex9No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //model.DateFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol9Text))
                //{
                //    model.NumericFieldCount = 1;
                //}
                string ExtraCol10Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol10Text))
                {
                    bool IsExtraCol10 = (GetData != null && GetData.IsExtraCol10) ? true : false;
                    bool IsExtraCol10Required = (GetData != null && GetData.IsExtraCol10Required) ? true : false;
                    int? ex10No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol10Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol10Text, ColumnName = "ExtraCol10Text",IsActive=IsExtraCol10 ,IsRequired= IsExtraCol10Required ,FieldType= "DateText", Priority = ex10No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol9Text) && string.IsNullOrEmpty(ExtraCol10Text))
                //{
                //    model.DateFieldCount = 2;
                //}
                string ExtraCol11Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : string.Empty;
                if(!string.IsNullOrEmpty(ExtraCol11Text))
                {
                    bool IsExtraCol11 = (GetData != null && GetData.IsExtraCol11) ? true : false;
                    bool IsExtraCol11Required = (GetData != null && GetData.IsExtraCol11Required) ? true : false;
                    int? ex11No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol11Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol11Text, ColumnName = "ExtraCol11Text" ,IsActive= IsExtraCol11 ,IsRequired= IsExtraCol11Required ,FieldType= "NumberText", Priority = ex11No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //model.NumericFieldCount = 0;//assign by default 0
                //if (string.IsNullOrEmpty(ExtraCol11Text))
                //{
                //    model.NumericFieldCount = 1;
                //}
                string ExtraCol12Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : string.Empty;
                if (!string.IsNullOrEmpty(ExtraCol12Text))
                {
                    bool IsExtraCol12Required = (GetData != null && GetData.IsExtraCol12Required) ? true : false;
                    bool IsExtraCol12 = (GetData != null && GetData.IsExtraCol12) ? true : false;
                    int? ex12No = fieldPriorityList.Where(a => a.FieldName == "ExtraCol12Text").Select(a => a.Priority).FirstOrDefault();

                    fieldsList.Add(new TktFormFieldsModel { TextName = ExtraCol12Text, ColumnName = "ExtraCol12Text", IsActive = IsExtraCol12, IsRequired = IsExtraCol12Required, FieldType = "NumberText", Priority = ex12No, CanChangeFieldType = true, CanDeleteThisField = true, AlwaysActive = false, AlwaysRequired = false });
                }

                //if (string.IsNullOrEmpty(ExtraCol11Text) && string.IsNullOrEmpty(ExtraCol12Text))
                //{
                //    model.NumericFieldCount = 2;
                //}

                //string IsImageCol1 = (GetData != null && GetData.IsImageCol1) ? "true" : "false";
                //string IsImageCol1Required = (GetData != null && GetData.IsImageCol1Required) ? "true" : "false";
                //string ImageCol1Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol1Text) ? GetFormData.ImageCol1Text : string.Empty;
                //int? ex13No = fieldPriorityList.Where(a => a.FieldName == "ImageCol1Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new TktFormFieldsModel { TextName = ImageCol1Text, Values = "ImageCol1Text/" + IsImageCol1 + "/" + IsImageCol1Required + "/FilePath",Priority= ex13No });

                //string IsImageCol2 = (GetData != null && GetData.IsImageCol2) ? "true" : "false";
                //string IsImageCol2Required = (GetData != null && GetData.IsImageCol2Required) ? "true" : "false";
                //string ImageCol2Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol2Text) ? GetFormData.ImageCol2Text : string.Empty;
                //int? ex14No = fieldPriorityList.Where(a => a.FieldName == "ImageCol2Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new TktFormFieldsModel { TextName = ImageCol2Text, Values = "ImageCol2Text/" + IsImageCol2 + "/" + IsImageCol2Required + "/FilePath" ,Priority=ex14No});

                //string IsImageCol3 = (GetData != null && GetData.IsImageCol3) ? "true" : "false";
                //string IsImageCol3Required = (GetData != null && GetData.IsImageCol3Required) ? "true" : "false";
                //string ImageCol3Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol3Text) ? GetFormData.ImageCol3Text : string.Empty;
                //int? ex15No = fieldPriorityList.Where(a => a.FieldName == "ImageCol3Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new TktFormFieldsModel { TextName = ImageCol3Text, Values = "ImageCol3Text/" + IsImageCol3 + "/" + IsImageCol3Required + "/FilePath",Priority= ex15No});

                //string IsImageCol4 = (GetData != null && GetData.IsImageCol4) ? "true" : "false";
                //string IsImageCol4Required = (GetData != null && GetData.IsImageCol4Required) ? "true" : "false";
                //string ImageCol4Text = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ImageCol4Text) ? GetFormData.ImageCol4Text : string.Empty;
                //int? ex16No = fieldPriorityList.Where(a => a.FieldName == "ImageCol4Text").Select(a => a.Priority).FirstOrDefault();

                //fieldsList.Add(new TktFormFieldsModel { TextName = ImageCol4Text, Values = "ImageCol4Text/" + IsImageCol4 + "/" + IsImageCol4Required + "/FilePath",Priority=ex16No });
               
                #endregion
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
            }

            if (ErrorMessage != string.Empty)
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                
                return Request.CreateResponse(HttpStatusCode.OK, fieldsList);
            }
        }

        //api/CRM_TicketFormMaster/GetTicketFieldTypeDDL
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetTicketFieldTypeDDL() 
        {
            var fieldTypeList = new List<SelectListItem> {
                new SelectListItem { Text= "Choose Field Type",Value="" },
                new SelectListItem { Text= "Normal text (E.g.: Hello!)",Value="NormalText" },
                new SelectListItem { Text= "Date(E.g.: DD/MM/YYYY)",Value="DateText" },
                new SelectListItem { Text= "Number(E.g.:1 to 10)",Value="NumberText" },
                new SelectListItem { Text= "Decimal(E.g.: Rs. 2.50)",Value="DecimalText" },
                new SelectListItem { Text= "Email",Value="EmailText" },
                new SelectListItem { Text= "Dropdown List",Value="DropDownList" }
            };
            return Request.CreateResponse(HttpStatusCode.OK, fieldTypeList);
        }

        //api/CRM_TicketFormMaster/DelTktFormField?ColumnName=ExtraCol1Text&CompanyID=307&BranchID=184&UID=61&Token=VwFdB3OPEwOoHnr15a5qgg==
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> DelTktFormField(string ColumnName, string CompanyID, string BranchID, string UID, string Token)
        {
            
               string ErrorMessage = string.Empty;
            
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                    int branchID = Convert.ToInt32(BranchID);
                    int companyID = Convert.ToInt32(CompanyID);
                    var uid = string.IsNullOrEmpty(UID) ? Convert.ToInt32(UID) : 0;
                    //System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}

                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //     Token = headers.GetValues("Token").First();
                    //}

                    var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        ErrorMessage = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    var dt = Constant.GetimeForApi(companyID);

                    var GetData =await db.crm_ticketcreatesetting.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefaultAsync();
                        var GetFormData =await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefaultAsync();
                        var GetSeqData =await db.crm_ticket_field_sequence.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.FieldName == ColumnName).FirstOrDefaultAsync();
                        if (GetData != null && GetFormData != null)
                        {
                            if (!string.IsNullOrEmpty(ColumnName))//check field text name not null
                            {
                                if (ColumnName == "ExtraCol1Text")
                                {
                                    GetFormData.ExtraCol1Text = null;
                                    GetData.IsExtraCol1 = false;
                                    GetData.IsExtraCol1Required = false;
                                    GetData.ModifiedBy = uid;
                                    GetData.ModifiedOn = dt;
                                    GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol2Text")
                                {
                                    GetFormData.ExtraCol2Text = null;
                                    GetData.IsExtraCol2 = false;
                                    GetData.IsExtraCol2Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol3Text")
                                {
                                    GetFormData.ExtraCol3Text = null;
                                    GetData.IsExtraCol3 = false;
                                    GetData.IsExtraCol3Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol4Text")
                                {
                                    GetFormData.ExtraCol4Text = null;
                                    GetData.IsExtraCol4 = false;
                                    GetData.IsExtraCol4Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol5Text")
                                {
                                    GetFormData.ExtraCol5Text = null;
                                    GetData.IsExtraCol5 = false;
                                    GetData.IsExtraCol5Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol6Text")
                                {
                                    GetFormData.ExtraCol6Text = null;
                                    GetData.ISExtraCol6 = false;
                                    GetData.ISExtraCol6Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol7Text")
                                {
                                    GetFormData.ExtraCol7Text = null;
                                    GetData.ISExtraCol7 = false;
                                    GetData.ISExtraCol7Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol8Text")
                                {
                                    GetFormData.ExtraCol8Text = null;
                                    GetData.ISExtraCol8 = false;
                                    GetData.ISExtraCol8Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol9Text")
                                {
                                    GetFormData.ExtraCol9Text = null;
                                    GetData.IsExtraCol9 = false;
                                    GetData.IsExtraCol9Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol10Text")
                                {
                                    GetFormData.ExtraCol10Text = null;
                                    GetData.IsExtraCol10 = false;
                                    GetData.IsExtraCol10Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol11Text")
                                {
                                    GetFormData.ExtraCol11Text = null;
                                    GetData.IsExtraCol11 = false;
                                    GetData.IsExtraCol11Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                else if (ColumnName == "ExtraCol12Text")
                                {
                                    GetFormData.ExtraCol12Text = null;
                                    GetData.IsExtraCol12 = false;
                                    GetData.IsExtraCol12Required = false;
                                GetData.ModifiedBy = uid;
                                GetData.ModifiedOn = dt;
                                GetSeqData.Priority = 0;
                                    db.SaveChanges();
                                    trans.Commit();
                                    ErrorMessage = "ok";
                                }
                                //else if (FieldName == "ImageCol1Text")
                                //{
                                //    GetFormData.ImageCol1Text = null;
                                //    GetData.IsImageCol1 = false;
                                //    GetData.IsImageCol1Required =  false;
                                //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                //    GetData.ModifiedOn = todayDate;
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
                                //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                //    GetData.ModifiedOn = todayDate;
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
                                //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                //    GetData.ModifiedOn = todayDate;
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
                                //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                //    GetData.ModifiedOn = todayDate;
                                //    GetSeqData.Priority = 0;
                                //    db.SaveChanges();
                                //    trans.Commit();
                                //    msg = "ok";                                    
                                //}
                            }
                            else
                            {
                            ErrorMessage = string.Format("Please enter column name");
                            //return Json("err", JsonRequestBehavior.AllowGet);
                            }
                        }//if get data not null end
                        else
                        {
                        ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                        
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ExceptionLogging.SendExcepToDB(ex);
                        ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                    }
                }


            if (string.IsNullOrEmpty(ErrorMessage))
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                if (ErrorMessage!="ok")
                {
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Delete field successfully");
                }
            }
        }


        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ChangeFieldsSequnce(HttpRequestMessage request) 
        {
            var msg = "";
            try
            {
                var json = await request.Content.ReadAsStringAsync();
                FieldsSequenceModel Data = JsonConvert.DeserializeObject<FieldsSequenceModel>(json);
                int branchID = Convert.ToInt32(Data.BranchID);
                int companyID = Convert.ToInt32(Data.CompanyID);
                int uid = Convert.ToInt32(Data.UID);

                var auth = Utility.TokenVerify(companyID, Data.Token);//verify token for is authorized user

                if (auth == false)
                {
                    msg = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(msg);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

              
                if (Data.Sequences!=null && Data.Sequences.Count > 0)
                {
                    
                    var dt = Constant.GetimeForApi(companyID);
                    for (int i = 0; i < Data.Sequences.Count; i++)
                    {
                        string fieldName = Data.Sequences[i].ColumnName;
                        int Priority= Data.Sequences[i].Priority;
                        var GetSeqData = await db.crm_ticket_field_sequence.Where(em => em.BranchID == branchID && em.CompanyID == companyID && em.FieldName == fieldName).FirstOrDefaultAsync();
                        if (GetSeqData != null)
                        {
                            GetSeqData.Priority = Priority;
                            GetSeqData.ModifiedDate = dt;
                            db.SaveChanges();
                        }
                        else
                        {
                            var fp = new crm_ticket_field_sequence
                            {
                                Priority = Priority,
                                FieldName = fieldName,
                                CompanyID = companyID,
                                BranchID = branchID,
                                CreatedBy = uid,
                                Createddate = dt                               
                            };
                            db.crm_ticket_field_sequence.Add(fp);
                            db.SaveChanges();
                        }
                    }
                    
                }
                else
                {
                    var message = string.Format("** Somthing went wrong, while reading data, Please check the Post Data Parameters **");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }
            }
            catch (Exception ex)
            {
                var message = string.Format("** Somthing went wrong, while reading data, Please check the Post Data Parameters **");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
           
            return Request.CreateResponse(HttpStatusCode.OK, "Fields priority updated suceessfully");
        }

        //api/CRM_TicketFormMaster/TicketFormFieldAdd_Update
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> TicketFormFieldAdd_Update([FromBody]JToken postData, HttpRequestMessage request)
        {            
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                   
                    TicketFormFieldAddUpdateModel model  = JsonConvert.DeserializeObject<TicketFormFieldAddUpdateModel>(postData.ToString());

                    int BranchID = Convert.ToInt32(model.BranchID);
                    int CompanyID = Convert.ToInt32(model.CompanyID);

                    //string Token = string.Empty;

                    //var re = Request;
                    //var headers = re.Headers;

                    //if (headers.Contains("Token"))
                    //{
                    //    Token = headers.GetValues("Token").First();
                    //}
                    var auth = Utility.TokenVerify(CompanyID, model.Token);//verify token for is authorized user

                    if (auth == false)
                    {
                        ErrorMessage = string.Format("** User authentication failed!");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }

                    if (string.IsNullOrEmpty(model.SaveType))
                    {
                        ErrorMessage = string.Format("** Save type is empty");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    if (string.IsNullOrEmpty(model.FieldType))
                    {
                        ErrorMessage = string.Format("** Field type is empty");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    if (string.IsNullOrEmpty(model.TextName))
                    {
                        ErrorMessage = string.Format("** Text name is empty");
                        HttpError err = new HttpError(ErrorMessage);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    }
                    //if (string.IsNullOrEmpty(model.FieldName))
                    //{
                    //    ErrorMessage = string.Format("** Field name is empty");
                    //    HttpError err = new HttpError(ErrorMessage);
                    //    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                    //}

                    
                    var GetData = await db.crm_ticketcreatesetting.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();
                    var GetFormData = await db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == BranchID && em.CompanyId == CompanyID).FirstOrDefaultAsync();

                    if (GetData != null && GetFormData != null)
                    {
                        int FieldPriority = 0;
                        var todayDate = Constant.GetimeForApi(CompanyID);
                        if (model.SaveType == "New" && !string.IsNullOrEmpty(model.FieldType) && !string.IsNullOrEmpty(model.TextName))//check if save type is not null for add new field
                        {
                            if (model.FieldType == "NormalText")//check field type for insert column string data type
                            {
                                if (string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                                {
                                    GetFormData.ExtraCol1Text = model.TextName;
                                    GetData.IsExtraCol1 = model.IsActive;
                                    GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                    
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                {
                                    GetFormData.ExtraCol2Text = model.TextName;
                                    GetData.IsExtraCol2 = model.IsActive;
                                    GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                {
                                    GetFormData.ExtraCol3Text = model.TextName;
                                    GetData.IsExtraCol3 = model.IsActive;
                                    GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                {
                                    GetFormData.ExtraCol4Text = model.TextName;
                                    GetData.IsExtraCol4 = model.IsActive;
                                    GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                {
                                    GetFormData.ExtraCol5Text = model.TextName;
                                    GetData.IsExtraCol5 = model.IsActive;
                                    GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                {
                                    GetFormData.ExtraCol6Text = model.TextName;
                                    GetData.ISExtraCol6 = model.IsActive;
                                    GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No Field available for normal text type";
                                }
                            }
                            else if (model.FieldType == "DecimalText")
                            {
                                if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                {
                                    GetFormData.ExtraCol7Text = model.TextName;
                                    GetData.ISExtraCol7 = model.IsActive;
                                    GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                {
                                    GetFormData.ExtraCol8Text = model.TextName;
                                    GetData.ISExtraCol8 = model.IsActive;
                                    GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No field available for decimal type";
                                }
                            }
                            else if (model.FieldType == "NumberText")
                            {
                                if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                {
                                    GetFormData.ExtraCol11Text = model.TextName;
                                    GetData.IsExtraCol11 = model.IsActive;
                                    GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                {
                                    GetFormData.ExtraCol12Text = model.TextName;
                                    GetData.IsExtraCol12 = model.IsActive;
                                    GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No field available for number type";
                                }
                            }
                            else if (model.FieldType == "DateText")
                            {
                                if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                {
                                    GetFormData.ExtraCol9Text = model.TextName;
                                    GetData.IsExtraCol9 = model.IsActive;
                                    GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                {
                                    GetFormData.ExtraCol10Text = model.TextName;
                                    GetData.IsExtraCol10 = model.IsActive;
                                    GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                    db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = 0 where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");

                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field added successfully";
                                }
                                else
                                {
                                    ErrorMessage = "Sorry! No field available for date type";
                                }
                            }
                           else if (model.FieldType == "EmailText")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for email type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                           else if (model.FieldType == "DropDownList")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for dropdown list");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            else
                            {
                                ErrorMessage = "Sorry! No field available for this field type";
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);                                
                            }
                        }
                        else if (!string.IsNullOrEmpty(model.TextName))//check field text name not null
                        {
                            if (model.FieldName != "EmailIDText" && model.FieldType == "EmailText")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for email type");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            if ((model.FieldName != "ProductTypeIDText" || model.FieldName != "ErrorTypeIDText" || model.FieldName != "UrgencyIDText" || model.FieldName != "StatusIDText") && model.FieldType == "DropDownList")
                            {
                                ErrorMessage = string.Format("Sorry! No field available for dropdown list");
                                HttpError err = new HttpError(ErrorMessage);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                            }
                            var GetSeqData = await db.crm_ticket_field_sequence.Where(em => em.BranchID == BranchID && em.CompanyID == CompanyID && em.FieldName == model.FieldName).FirstOrDefaultAsync();
                            FieldPriority = GetSeqData==null ? 0: GetSeqData.Priority;//set priority

                            if (model.FieldName == "NameText")
                            {
                                GetFormData.NameText = model.TextName;
                                GetData.IsName = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "PhoneNumberText")
                            {
                                GetFormData.PhoneNumberText = model.TextName;
                                GetData.IsPhoneNumber = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";
                            }
                            else if (model.FieldName == "ProductTypeIDText")
                            {
                                GetFormData.ProductTypeIDText = model.TextName;
                                GetData.IsProductTypeID = true;
                                GetData.IsProductTypeIDRequired = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "ErrorTypeIDText")
                            {
                                GetFormData.ErrorTypeIDText = model.TextName;
                                GetData.IsErrorTypeID = true;
                                GetData.IsErrorTypeIDRequired = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "UrgencyIDText")
                            {
                                GetFormData.UrgencyIDText = model.TextName;
                                GetData.IsUrgencyID = true;
                                GetData.IsUrgencyIDRequired = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "StatusIDText")
                            {
                                GetFormData.StatusIDText = model.TextName;
                                GetData.IsStatusID = true;
                                GetData.IsStatusIDRequired = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "EmailIDText")
                            {
                                GetFormData.EmailIDText = model.TextName;
                                GetData.IsEmailID = model.IsActive;
                                GetData.IsEmailIDRequired = model.IsActive == true ? model.IsRequired : false;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "subjectText")
                            {
                                GetFormData.subjectText = model.TextName;
                                GetData.Issubject = true;
                                GetData.IssubjectRequired = true;
                                db.SaveChanges();
                                trans.Commit();
                                SuccessMessage = "Field updated successfully";

                            }
                            else if (model.FieldName == "ExtraCol1Text")
                            {
                                if (model.FieldType== "NormalText")
                                {
                                    GetFormData.ExtraCol1Text = model.TextName;
                                    GetData.IsExtraCol1 = model.IsActive;
                                    GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol1Text = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol1Text = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol1Text = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol1Text = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol1Text = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol1Text = null;
                                    //        GetData.IsExtraCol1 = false;
                                    //        GetData.IsExtraCol1Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol1Text = null;
                                    //        GetData.IsExtraCol1 = false;
                                    //        GetData.IsExtraCol1Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol1Text = null;
                                    //        GetData.IsExtraCol1 = false;
                                    //        GetData.IsExtraCol1Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol1Text = null;
                                    //        GetData.IsExtraCol1 = false;
                                    //        GetData.IsExtraCol1Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol2Text")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol2Text = model.TextName;
                                    GetData.IsExtraCol2 = model.IsActive;
                                    GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol2Text = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol2Text = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol2Text = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol2Text = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol2Text = null;
                                            GetData.IsExtraCol2 = false;
                                            GetData.IsExtraCol2Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol2Text = null;
                                    //        GetData.IsExtraCol2 = false;
                                    //        GetData.IsExtraCol2Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol2Text = null;
                                    //        GetData.IsExtraCol2 = false;
                                    //        GetData.IsExtraCol2Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol2Text = null;
                                    //        GetData.IsExtraCol2 = false;
                                    //        GetData.IsExtraCol2Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol2Text = null;
                                    //        GetData.IsExtraCol2 = false;
                                    //        GetData.IsExtraCol2Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol3Text")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol3Text = model.TextName;
                                    GetData.IsExtraCol3 = model.IsActive;
                                    GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol3Text = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol3Text = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol3Text = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol3Text = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol3Text = null;
                                            GetData.IsExtraCol3 = false;
                                            GetData.IsExtraCol3Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol3Text = null;
                                    //        GetData.IsExtraCol3 = false;
                                    //        GetData.IsExtraCol3Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol3Text = null;
                                    //        GetData.IsExtraCol3 = false;
                                    //        GetData.IsExtraCol3Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol3Text = null;
                                    //        GetData.IsExtraCol3 = false;
                                    //        GetData.IsExtraCol3Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol3Text = null;
                                    //        GetData.IsExtraCol3 = false;
                                    //        GetData.IsExtraCol3Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol4Text")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol4Text = model.TextName;
                                    GetData.IsExtraCol4 = model.IsActive;
                                    GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol4Text = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol4Text = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol4Text = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol4Text = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol4Text = null;
                                            GetData.IsExtraCol4 = false;
                                            GetData.IsExtraCol4Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol4Text = null;
                                    //        GetData.IsExtraCol4 = false;
                                    //        GetData.IsExtraCol4Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol4Text = null;
                                    //        GetData.IsExtraCol4 = false;
                                    //        GetData.IsExtraCol4Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol4Text = null;
                                    //        GetData.IsExtraCol4 = false;
                                    //        GetData.IsExtraCol4Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol4Text = null;
                                    //        GetData.IsExtraCol4 = false;
                                    //        GetData.IsExtraCol4Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol5Text")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol5Text = model.TextName;
                                    GetData.IsExtraCol5 = model.IsActive;
                                    GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol5Text = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol5Text = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol5Text = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol5Text = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol5Text = null;
                                            GetData.IsExtraCol5 = false;
                                            GetData.IsExtraCol5Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            GetSeqData.Priority = 0;//update field sequence to 0
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol5Text = null;
                                    //        GetData.IsExtraCol5 = false;
                                    //        GetData.IsExtraCol5Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol5Text = null;
                                    //        GetData.IsExtraCol5 = false;
                                    //        GetData.IsExtraCol5Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol5Text = null;
                                    //        GetData.IsExtraCol5 = false;
                                    //        GetData.IsExtraCol5Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol5Text = null;
                                    //        GetData.IsExtraCol5 = false;
                                    //        GetData.IsExtraCol5Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol6Text")
                            {
                                if (model.FieldType == "NormalText")
                                {
                                    GetFormData.ExtraCol6Text = model.TextName;
                                    GetData.ISExtraCol6 = model.IsActive;
                                    GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field added successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol6Text = null;
                                            GetData.ISExtraCol6 = false;
                                            GetData.ISExtraCol6Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;

                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol6Text = null;
                                            GetData.ISExtraCol6 = false;
                                            GetData.ISExtraCol6Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol6Text = null;
                                            GetData.ISExtraCol6 = false;
                                            GetData.ISExtraCol6Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol6Text = null;
                                            GetData.ISExtraCol6 = false;
                                            GetData.ISExtraCol6Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol6Text = null;
                                            GetData.ISExtraCol6 = false;
                                            GetData.ISExtraCol6Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol6Text = null;
                                    //        GetData.ISExtraCol6 = false;
                                    //        GetData.ISExtraCol6Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol6Text = null;
                                    //        GetData.ISExtraCol6 = false;
                                    //        GetData.ISExtraCol6Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol6Text = null;
                                    //        GetData.ISExtraCol6 = false;
                                    //        GetData.ISExtraCol6Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol6Text = null;
                                    //        GetData.ISExtraCol6 = false;
                                    //        GetData.ISExtraCol6Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol7Text")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol7Text = model.TextName;
                                    GetData.ISExtraCol7 = model.IsActive;
                                    GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol1Text = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;

                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol2Text = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol3Text = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0
                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol4Text = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol5Text = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol6Text = model.TextName;
                                            GetData.ISExtraCol6 = model.IsActive;
                                            GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol7Text = null;
                                            GetData.ISExtraCol7 = false;
                                            GetData.ISExtraCol7Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol7Text = null;
                                    //        GetData.ISExtraCol7 = false;
                                    //        GetData.ISExtraCol7Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol7Text = null;
                                    //        GetData.ISExtraCol7 = false;
                                    //        GetData.ISExtraCol7Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol7Text = null;
                                    //        GetData.ISExtraCol7 = false;
                                    //        GetData.ISExtraCol7Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol7Text = null;
                                    //        GetData.ISExtraCol7 = false;
                                    //        GetData.ISExtraCol7Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol8Text")
                            {
                                if (model.FieldType == "DecimalText")
                                {
                                    GetFormData.ExtraCol8Text = model.TextName;
                                    GetData.ISExtraCol8 = model.IsActive;
                                    GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol1Text = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol2Text = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol3Text = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol4Text = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol5Text = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol6Text = model.TextName;
                                            GetData.ISExtraCol6 = model.IsActive;
                                            GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12sText'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol8Text = null;
                                            GetData.ISExtraCol8 = false;
                                            GetData.ISExtraCol8Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol8Text = null;
                                    //        GetData.ISExtraCol8 = false;
                                    //        GetData.ISExtraCol8Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol8Text = null;
                                    //        GetData.ISExtraCol8 = false;
                                    //        GetData.ISExtraCol8Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol8Text = null;
                                    //        GetData.ISExtraCol8 = false;
                                    //        GetData.ISExtraCol8Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol8Text = null;
                                    //        GetData.ISExtraCol8 = false;
                                    //        GetData.ISExtraCol8Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol9Text")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol9Text = model.TextName;
                                    GetData.IsExtraCol9 = model.IsActive;
                                    GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol1Text = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol2Text = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol3Text = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol4Text = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol5Text = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol6Text = model.TextName;
                                            GetData.ISExtraCol6 = model.IsActive;
                                            GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }
                                    else if (model.FieldType == "DecimalText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol9Text = null;
                                            GetData.IsExtraCol9 = false;
                                            GetData.IsExtraCol9Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol9Text = null;
                                    //        GetData.IsExtraCol9 = false;
                                    //        GetData.IsExtraCol9Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol9Text = null;
                                    //        GetData.IsExtraCol9 = false;
                                    //        GetData.IsExtraCol9Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol9Text = null;
                                    //        GetData.IsExtraCol9 = false;
                                    //        GetData.IsExtraCol9Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol9Text = null;
                                    //        GetData.IsExtraCol9 = false;
                                    //        GetData.IsExtraCol9Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else if (model.FieldName == "ExtraCol10Text")
                            {
                                if (model.FieldType == "DateText")
                                {
                                    GetFormData.ExtraCol10Text = model.TextName;
                                    GetData.IsExtraCol10 = model.IsActive;
                                    GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol1Text = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol2Text = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol3Text = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol4Text = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol5Text = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol6Text = model.TextName;
                                            GetData.ISExtraCol6 = model.IsActive;
                                            GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }
                                    else if (model.FieldType == "DecimalText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "NumberText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol11Text = model.TextName;
                                            GetData.IsExtraCol11 = model.IsActive;
                                            GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol11Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                                        {
                                            GetFormData.ExtraCol10Text = null;
                                            GetData.IsExtraCol10 = false;
                                            GetData.IsExtraCol10Required = false;

                                            GetFormData.ExtraCol12Text = model.TextName;
                                            GetData.IsExtraCol12 = model.IsActive;
                                            GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol12Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for number type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol10Text = null;
                                    //        GetData.IsExtraCol10 = false;
                                    //        GetData.IsExtraCol10Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol10Text = null;
                                    //        GetData.IsExtraCol10 = false;
                                    //        GetData.IsExtraCol10Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol10Text = null;
                                    //        GetData.IsExtraCol10 = false;
                                    //        GetData.IsExtraCol10Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol10Text = null;
                                    //        GetData.IsExtraCol10 = false;
                                    //        GetData.IsExtraCol10Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol11Text")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol11Text = model.TextName;
                                    GetData.IsExtraCol11 = model.IsActive;
                                    GetData.IsExtraCol11Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    trans.Commit();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol1Text = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol2Text = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol3Text = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol4Text = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol5Text = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol6Text = model.TextName;
                                            GetData.ISExtraCol6 = model.IsActive;
                                            GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }
                                    else if (model.FieldType == "DecimalText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                        {
                                            GetFormData.ExtraCol1Text = null;
                                            GetData.IsExtraCol1 = false;
                                            GetData.IsExtraCol1Required = false;

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol11Text = null;
                                            GetData.IsExtraCol11 = false;
                                            GetData.IsExtraCol11Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol11Text = null;
                                    //        GetData.IsExtraCol11 = false;
                                    //        GetData.IsExtraCol11Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol11Text = null;
                                    //        GetData.IsExtraCol11 = false;
                                    //        GetData.IsExtraCol11Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol11Text = null;
                                    //        GetData.IsExtraCol11 = false;
                                    //        GetData.IsExtraCol11Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol11Text = null;
                                    //        GetData.IsExtraCol11 = false;
                                    //        GetData.IsExtraCol11Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }

                            }
                            else if (model.FieldName == "ExtraCol12Text")
                            {
                                if (model.FieldType == "NumberText")
                                {
                                    GetFormData.ExtraCol12Text = model.TextName;
                                    GetData.IsExtraCol12 = model.IsActive;
                                    GetData.IsExtraCol12Required = model.IsActive == true ? model.IsRequired : false;
                                    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    GetData.ModifiedOn = todayDate;
                                    db.SaveChanges();
                                    SuccessMessage = "Field updated successfully";
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

                                            GetFormData.ExtraCol1Text = model.TextName;
                                            GetData.IsExtraCol1 = model.IsActive;
                                            GetData.IsExtraCol1Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol1Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol2Text = model.TextName;
                                            GetData.IsExtraCol2 = model.IsActive;
                                            GetData.IsExtraCol2Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol2Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol3Text = model.TextName;
                                            GetData.IsExtraCol3 = model.IsActive;
                                            GetData.IsExtraCol3Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol3Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol4Text = model.TextName;
                                            GetData.IsExtraCol4 = model.IsActive;
                                            GetData.IsExtraCol4Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol4Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol5Text = model.TextName;
                                            GetData.IsExtraCol5 = model.IsActive;
                                            GetData.IsExtraCol5Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol5Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol6Text = model.TextName;
                                            GetData.ISExtraCol6 = model.IsActive;
                                            GetData.ISExtraCol6Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol6Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No Field available for normal text type";
                                        }
                                    }
                                    else if (model.FieldType == "DecimalText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol7Text = model.TextName;
                                            GetData.ISExtraCol7 = model.IsActive;
                                            GetData.ISExtraCol7Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol7Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol8Text = model.TextName;
                                            GetData.ISExtraCol8 = model.IsActive;
                                            GetData.ISExtraCol8Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol8Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for decimal type";
                                        }
                                    }
                                    else if (model.FieldType == "DateText")
                                    {
                                        if (string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol9Text = model.TextName;
                                            GetData.IsExtraCol9 = model.IsActive;
                                            GetData.IsExtraCol9Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol9Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else if (string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                                        {
                                            GetFormData.ExtraCol12Text = null;
                                            GetData.IsExtraCol12 = false;
                                            GetData.IsExtraCol12Required = false;

                                            GetFormData.ExtraCol10Text = model.TextName;
                                            GetData.IsExtraCol10 = model.IsActive;
                                            GetData.IsExtraCol10Required = model.IsActive == true ? model.IsRequired : false;
                                            GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                            GetData.ModifiedOn = todayDate;
                                            //update field priority to replaced field priority
                                            db.Database.ExecuteSqlCommand("SET SQL_SAFE_UPDATES = 0;update crm_ticket_field_sequence set Priority = " + FieldPriority + " where BranchID = " + BranchID + " and CompanyID = " + CompanyID + " and FieldName = 'ExtraCol10Text'");
                                            GetSeqData.Priority = 0;//update field priority to 0

                                            db.SaveChanges();
                                            trans.Commit();
                                            SuccessMessage = "Field updated successfully";
                                        }
                                        else
                                        {
                                            ErrorMessage = "Sorry! No field available for date type";
                                        }
                                    }
                                    //else if (model.FieldType == "FilePath")
                                    //{
                                    //    if (string.IsNullOrEmpty(GetFormData.ImageCol1Text))
                                    //    {
                                    //        GetFormData.ExtraCol12Text = null;
                                    //        GetData.IsExtraCol12 = false;
                                    //        GetData.IsExtraCol12Required = false;

                                    //        GetFormData.ImageCol1Text = model.TextName;
                                    //        GetData.IsImageCol1 = model.IsActive;
                                    //        GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol2Text))
                                    //    {
                                    //        GetFormData.ExtraCol12Text = null;
                                    //        GetData.IsExtraCol12 = false;
                                    //        GetData.IsExtraCol12Required = false;

                                    //        GetFormData.ImageCol2Text = model.TextName;
                                    //        GetData.IsImageCol2 = model.IsActive;
                                    //        GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol3Text))
                                    //    {
                                    //        GetFormData.ExtraCol12Text = null;
                                    //        GetData.IsExtraCol12 = false;
                                    //        GetData.IsExtraCol12Required = false;

                                    //        GetFormData.ImageCol3Text = model.TextName;
                                    //        GetData.IsImageCol3 = model.IsActive;
                                    //        GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else if (string.IsNullOrEmpty(GetFormData.ImageCol4Text))
                                    //    {
                                    //        GetFormData.ExtraCol12Text = null;
                                    //        GetData.IsExtraCol12 = false;
                                    //        GetData.IsExtraCol12Required = false;

                                    //        GetFormData.ImageCol4Text = model.TextName;
                                    //        GetData.IsImageCol4 = model.IsActive;
                                    //        GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                                    //        GetData.ModifiedBy = Convert.ToInt32(model.UID);
                                    //        GetData.ModifiedOn = todayDate;
                                    //        db.SaveChanges();
                                    //        trans.Commit();
                                    //        SuccessMessage = "Field updated successfully";
                                    //    }
                                    //    else
                                    //    {
                                    //        ErrorMessage = "Sorry! No field available for image/file type";
                                    //    }
                                    //}
                                    #endregion
                                }

                            }
                            //else if (model.FieldName == "ImageCol1Text")
                            //{
                            //    GetFormData.ImageCol1Text = model.TextName;
                            //    GetData.IsImageCol1 = model.IsActive;
                            //    GetData.IsImageCol1Required = model.IsActive == true ? model.IsRequired : false;
                            //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                            //    GetData.ModifiedOn = todayDate;
                            //    db.SaveChanges();
                            //    SuccessMessage = "Field updated successfully";

                            //}
                            //else if (model.FieldName == "ImageCol2Text")
                            //{
                            //    GetFormData.ImageCol2Text = model.TextName;
                            //    GetData.IsImageCol2 = model.IsActive;
                            //    GetData.IsImageCol2Required = model.IsActive == true ? model.IsRequired : false;
                            //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                            //    GetData.ModifiedOn = todayDate;
                            //    db.SaveChanges();
                            //    SuccessMessage = "Field updated successfully";

                            //}
                            //else if (model.FieldName == "ImageCol3Text")
                            //{
                            //    GetFormData.ImageCol3Text = model.TextName;
                            //    GetData.IsImageCol3 = model.IsActive;
                            //    GetData.IsImageCol3Required = model.IsActive == true ? model.IsRequired : false;
                            //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                            //    GetData.ModifiedOn = todayDate;
                            //    db.SaveChanges();
                            //    SuccessMessage = "Field updated successfully";

                            //}
                            //else if (model.FieldName == "ImageCol4Text")
                            //{
                            //    GetFormData.ImageCol4Text = model.TextName;
                            //    GetData.IsImageCol4 = model.IsActive;
                            //    GetData.IsImageCol4Required = model.IsActive == true ? model.IsRequired : false;
                            //    GetData.ModifiedBy = Convert.ToInt32(model.UID);
                            //    GetData.ModifiedOn = todayDate;
                            //    db.SaveChanges();
                            //    SuccessMessage = "Field updated successfully";
                            //}
                        }
                                            
                    }

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionLogging.SendExcepToDB(ex);
                    ErrorMessage = "** Somthing went wrong, while reading data, Please check the Post Data Parameters **";
                }
            }

            if(!string.IsNullOrEmpty(ErrorMessage))
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }


        //api/CRM_TicketsViewMaster/ChangeStatus
        //[System.Web.Http.HttpPost]
        //public HttpResponseMessage ChangeStatus([FromBody]JToken postData, HttpRequestMessage request)
        //{
        //    string ErrorMessage = string.Empty;
        //    string SuccessMessage = string.Empty;

        //    try
        //    {

        //        UpdateTicketsViewPostDTO CIM = JsonConvert.DeserializeObject<UpdateTicketsViewPostDTO>(postData.ToString());

        //        int branchID = Convert.ToInt32(CIM.BranchID);
        //        int companyID = Convert.ToInt32(CIM.CompanyID);

        //        //string Token = string.Empty;

        //        //var re = Request;
        //        //var headers = re.Headers;

        //        //if (headers.Contains("Token"))
        //        //{
        //        //    Token = headers.GetValues("Token").First();
        //        //}
        //        var auth = Utility.TokenVerify(companyID, CIM.Token);//verify token for is authorized user

        //        if (auth == false)
        //        {
        //            ErrorMessage = string.Format("** User authentication failed!");
        //            HttpError err = new HttpError(ErrorMessage);
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //        }

        //        #region check nullable validation

        //        if (string.IsNullOrEmpty(CIM.ColumnName))
        //        {
        //            ErrorMessage = "Please enter column name";
        //            HttpError err = new HttpError(ErrorMessage);
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //        }


        //        #endregion
        //        int status = CIM.Status == true ? 1 : 0;
        //        db.Database.ExecuteSqlCommand(@"update crm_ticketviewsetting set " + CIM.ColumnName + "=" + status + " where BranchID =" + branchID + "  and CompanyID = " + companyID + "");
        //        SuccessMessage = "Status changed successfully";
        //    }
        //    catch (Exception ex)
        //    {

        //        ExceptionLogging.SendExcepToDB(ex);
        //        ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
        //    }

        //    if (!string.IsNullOrEmpty(ErrorMessage))
        //    {
        //        HttpError err = new HttpError(ErrorMessage);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
        //    }
        //}

        //api/CRM_TicketsViewMaster/ChangeFieldsSequnce
    }
}
