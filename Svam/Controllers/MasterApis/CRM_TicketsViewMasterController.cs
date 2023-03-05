using AutoMapper;
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.MasterApis
{
    public class CRM_TicketsViewMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_TicketsViewMaster/GetTicketsViewList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetTicketsViewList(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;
            
            var fieldsList = new List<TicketsViewMasterApiModel>();

            try
            {
                Int32 branchID = Convert.ToInt32(BranchID);
                Int32 companyID = Convert.ToInt32(CompanyID);

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
                var model  = new ViewTecketSettingDTO();
                var GetData =  db.crm_ticketviewsetting.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();
                var GetFormData =  db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();

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
                    data.BranchID = branchID;
                    data.CompanyId = companyID;
                    data.IsName = true;
                    data.IsPhoneNumber = true;
                    db.crm_ticketviewsetting.Add(data);
                    int i =  db.SaveChanges();
                    if (i > 0)
                    {
                        model = Mapper.Map<ViewTecketSettingDTO>(data);
                    }
                }

                #region fields names list for show row wise
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = "Ticket No.", ColumnName = "TicketNo", IsActive = true,IsFilter=true,IsActiveButtonAction=false,IsFilterButtonAction=false });
                
                string NameText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = NameText, ColumnName = "IsName", IsActive = true, IsFilter = true, IsActiveButtonAction = false, IsFilterButtonAction = false });
               
                string EmailIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = EmailIDText, ColumnName = "IsEmailID", IsActive = model.IsEmailID, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string PhoneNumberText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = PhoneNumberText, ColumnName = "IsPhoneNumber", IsActive = true, IsFilter = true, IsActiveButtonAction = false, IsFilterButtonAction = false });
                
                string ErrorTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = ErrorTypeIDText, ColumnName = "IsErrorTypeID", IsActive = model.IsErrorTypeID, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string UrgencyIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = UrgencyIDText, ColumnName = "IsUrgencyID", IsActive = model.IsUrgencyID, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string StatusIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = StatusIDText, ColumnName = "IsStatusID", IsActive = model.IsStatusID, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = "Created By", ColumnName = "IsCreatedBy", IsActive = model.IsCreatedBy, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = "Created Date", ColumnName = "IsCreatedDate", IsActive = model.IsCreatedDate, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = "Assigned To", ColumnName = "IsAssignedTo", IsActive = model.IsAssignedTo, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = "Assigned By", ColumnName = "IsAssignedBy", IsActive = model.IsAssignedBy, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = "Modified Date", ColumnName = "IsModifiedDate", IsActive = model.IsModifiedDate, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string subjectText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = subjectText, ColumnName = "Issubject", IsActive = model.Issubject, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });

                string ProductTypeIDText = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";
                fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = ProductTypeIDText, ColumnName = "IsProductTypeID", IsActive = model.IsProductTypeID, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });


                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text))
                {                  
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol1Text, ColumnName = "IsExtraCol1", IsActive = model.IsExtraCol1, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol2Text, ColumnName = "IsExtraCol2", IsActive = model.IsExtraCol2, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol3Text, ColumnName = "IsExtraCol3", IsActive = model.IsExtraCol3, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol4Text, ColumnName = "IsExtraCol4", IsActive = model.IsExtraCol4, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol5Text, ColumnName = "IsExtraCol5", IsActive = model.IsExtraCol5, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol6Text, ColumnName = "ISExtraCol6", IsActive = model.ISExtraCol6, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol7Text, ColumnName = "ISExtraCol7", IsActive = model.ISExtraCol7, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol8Text, ColumnName = "ISExtraCol8", IsActive = model.ISExtraCol8, IsFilter = true, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol9Text, ColumnName = "IsExtraCol9", IsActive = model.IsExtraCol9, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol10Text, ColumnName = "IsExtraCol10", IsActive = model.IsExtraCol10, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol11Text, ColumnName = "IsExtraCol11", IsActive = model.IsExtraCol11, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }

                if (GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text))
                {
                    fieldsList.Add(new TicketsViewMasterApiModel { FieldLabel = GetFormData.ExtraCol12Text, ColumnName = "IsExtraCol12", IsActive = model.IsExtraCol12, IsFilter = false, IsActiveButtonAction = true, IsFilterButtonAction = false });
                }
                              
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

        //api/CRM_TicketsViewMaster/ChangeStatus
        [HttpPost]
        public HttpResponseMessage ChangeStatus([FromBody]JToken postData, HttpRequestMessage request) 
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                UpdateTicketsViewPostDTO CIM = JsonConvert.DeserializeObject<UpdateTicketsViewPostDTO>(postData.ToString());

                int branchID = Convert.ToInt32(CIM.BranchID);
                int companyID = Convert.ToInt32(CIM.CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, CIM.Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                #region check nullable validation

                if (string.IsNullOrEmpty(CIM.ColumnName))
                {
                    ErrorMessage = "Please enter column name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                int status = CIM.Status == true ? 1 : 0;
                db.Database.ExecuteSqlCommand(@"update crm_ticketviewsetting set " + CIM.ColumnName + "="+status+" where BranchID =" + branchID + "  and CompanyID = " + companyID + "");
                SuccessMessage = "Status changed successfully";
            }
            catch (Exception ex)
            {

                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }
    }

}
