using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_ViewTicketController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        [HttpGet]
        public HttpResponseMessage Get(int CompanyID, int BranchID, string ProfileName, int? LoginUserID, string Token, string ErrorTypeName, string UrgencyName, string SearchUserID, string SearchFromDate, string SearchToDate, string SearchTerm, string DateType, string TicketStatusName)
        {
            
            String ErrorMessage = String.Empty;
            
            //string Token = string.Empty;
            //var re = Request;
            //var headers = re.Headers;

            //if (headers.Contains("Token"))
            //{
            //    Token = headers.GetValues("Token").First();
            //}
            var auth = Utility.TokenVerify(CompanyID, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            List<ViewTicketApiModel> TicketList = new List<ViewTicketApiModel>();
            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);

                int UID = 0;
                
                if (!string.IsNullOrEmpty(ProfileName) && ProfileName.Trim('"') == "SuperAdmin")
                {

                    if (!string.IsNullOrEmpty(SearchUserID))
                    {
                        UID = Convert.ToInt32(SearchUserID);                       
                    }
                }
                else
                {                    
                    UID = Convert.ToInt32(LoginUserID);
                    if (!string.IsNullOrEmpty(SearchUserID) && SearchUserID != "0")
                    {
                        UID = Convert.ToInt32(SearchUserID);
                    }
                }
                var DateFormat = Constant.DateFormatForApi(companyID);//get date format by company id
                var dd = Constant.GetimeForApi(companyID);
                DateTime MonthStartDate = new DateTime(dd.Year, dd.Month, 1);
                DateTime MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
                var MStartDate = MonthStartDate.ToString("dd/MM/yyyy");
                var MEndDate = MonthEndDate.ToString("dd/MM/yyyy");

                #region check date format 
                if (!string.IsNullOrEmpty(SearchFromDate) && !string.IsNullOrEmpty(SearchToDate))
                {

                    if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                    {                       
                        var fmDate = Convert.ToDateTime(SearchFromDate);
                        var tDate = Convert.ToDateTime(SearchToDate);

                        MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                        MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                    }
                    else
                    {
                        MStartDate = SearchFromDate;
                        MEndDate = SearchToDate;
                    }
                }
                #endregion
                //check dynamic fields data according to company and branch id
                var data = db.crm_ticketviewsetting.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();
                var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == branchID && em.CompanyId == companyID).FirstOrDefault();

               
                DataTable dtTicket = new DataTable();
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                  
                    dtTicket = DataAccessLayer.GetDataTable("call CRM_GetTicketListBySearchText(" + CompanyID + "," + BranchID + ",'" + SearchTerm + "')");
                }
                else if ((string.IsNullOrEmpty(DateType) || DateType == "all" || DateType == "cDate") && string.IsNullOrEmpty(SearchTerm))
                {
                    dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketList(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
                }
                else if (DateType == "mDate" && string.IsNullOrEmpty(SearchTerm))
                {
                    dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketListByModifiedDate(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
                }
                //else
                //{
                //    dtTicket = DataAccessLayer.GetDataTable("call CRM_TicketList(" + CompanyID + "," + BranchID + ",'" + MStartDate + "','" + MEndDate + "'," + UID + ")");
                //}

                if (dtTicket.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTicket.Rows.Count; i++)
                    {
                        ViewTicketApiModel oticket = new ViewTicketApiModel();
                        oticket.TicketID = Convert.ToInt32(dtTicket.Rows[i]["TicketID"]);
                        oticket.TicketNo = Convert.ToString(dtTicket.Rows[i]["TicketNo"]);
                        //oticket.CustomerID = dtTicket.Rows[i]["CustomerID"] == DBNull.Value ? Convert.ToInt32(0) : Convert.ToInt32(dtTicket.Rows[i]["CustomerID"]);
                        oticket.CustomerName = dtTicket.Rows[i]["CustomerName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CustomerName"]);
                        oticket.EmailID = Convert.ToString(dtTicket.Rows[i]["EmailID"]);
                        oticket.PhoneNumber = Convert.ToString(dtTicket.Rows[i]["PhoneNumber"]);
                        //oticket.ErrorTypeID = Convert.ToInt32(dtTicket.Rows[i]["ErrrorID"]);
                        oticket.ErrorTypeName = Convert.ToString(dtTicket.Rows[i]["ErrorName"]);
                        //oticket.UrgencyID = Convert.ToInt32(dtTicket.Rows[i]["urgencyID"]);
                        oticket.UrgencyName = Convert.ToString(dtTicket.Rows[i]["urgencyName"]);
                        //oticket.TicketStatusID = Convert.ToInt32(dtTicket.Rows[i]["StatusId"]);
                        //oticket.ProductTypeID = Convert.ToInt32(dtTicket.Rows[i]["ProductTypeID"]);
                        oticket.ProductTypeName = Convert.ToString(dtTicket.Rows[i]["ProductTypeName"]);
                        oticket.TicketSubject = Convert.ToString(dtTicket.Rows[i]["subject"]);
                        oticket.TicketStatusName = dtTicket.Rows[i]["StatusId"] == null ? string.Empty : Convert.ToInt32(dtTicket.Rows[i]["StatusId"]) == 1 ? "Open" : "Closed";
                        oticket.CreatedBy = dtTicket.Rows[i]["CreatedByName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CreatedByName"]);
                        //oticket.AssignedBy = dtTicket.Rows[i]["AssignedBy"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dtTicket.Rows[i]["AssignedBy"]);
                        oticket.AssignedByName = dtTicket.Rows[i]["AssignedByName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedByName"]);
                        //oticket.AssignedTo = dtTicket.Rows[i]["AssignedTo"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dtTicket.Rows[i]["AssignedTo"]);
                        oticket.AssignedToName = dtTicket.Rows[i]["AssignedToName"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["AssignedToName"]);
                        oticket.CreatedDate = dtTicket.Rows[i]["CreatedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["CreatedDate"]);
                        oticket.ModifiedDate = dtTicket.Rows[i]["ModifiedDate"] == DBNull.Value ? String.Empty : Convert.ToString(dtTicket.Rows[i]["ModifiedDate"]);
                        oticket.ExtraCol1 = Convert.ToString(dtTicket.Rows[i]["ExtraCol1"]);
                        oticket.ExtraCol2 = Convert.ToString(dtTicket.Rows[i]["ExtraCol2"]);
                        oticket.ExtraCol3 = Convert.ToString(dtTicket.Rows[i]["ExtraCol3"]);
                        oticket.ExtraCol4 = Convert.ToString(dtTicket.Rows[i]["ExtraCol4"]);
                        oticket.ExtraCol5 = Convert.ToString(dtTicket.Rows[i]["ExtraCol5"]);
                        oticket.ExtraCol6 = Convert.ToString(dtTicket.Rows[i]["ExtraCol6"]);
                        oticket.ExtraCol7 = Convert.ToDecimal(dtTicket.Rows[i]["ExtraCol7"]);
                        oticket.ExtraCol8 = Convert.ToDecimal(dtTicket.Rows[i]["ExtraCol8"]);
                        oticket.ExtraCol9 = dtTicket.Rows[i]["ExtraCol9"] == DBNull.Value ? string.Empty : Convert.ToString(dtTicket.Rows[i]["ExtraCol9"]);
                        oticket.ExtraCol10 = dtTicket.Rows[i]["ExtraCol10"] == DBNull.Value ? string.Empty : Convert.ToString(dtTicket.Rows[i]["ExtraCol10"]);
                        oticket.ExtraCol11 = Convert.ToInt32(dtTicket.Rows[i]["ExtraCol11"]);
                        oticket.ExtraCol12 = Convert.ToInt32(dtTicket.Rows[i]["ExtraCol12"]);

                        //add dynamic field values
                        oticket.IsTicketNo = true;
                        oticket.TicketNoLabel = "Ticket No.";

                        oticket.CustomerNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";
                        oticket.IsCustomerName = true;

                        oticket.EmailIDLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";
                        oticket.IsEmailID= (data == null || (data != null && data.IsEmailID)?true:false);

                        oticket.PhoneNumberLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";
                        oticket.IsPhoneNumber = true;

                        oticket.ProductTypeNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";
                        oticket.IsProductTypeName = data != null && data.IsProductTypeID ? true : false;

                        oticket.ErrorTypeNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";
                        oticket.IsErrorTypeName = (data == null || (data != null && data.IsErrorTypeID) ? true : false);

                        oticket.UrgencyNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";
                        oticket.IsUrgencyName= (data == null || (data != null && data.IsUrgencyID) ? true : false);

                        oticket.TicketStatusNameLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";
                        oticket.IsTicketStatusName= (data == null || (data != null && data.IsStatusID) ? true : false);

                        oticket.TicketSubjectLabel = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";
                        oticket.IsTicketSubject= data != null && data.Issubject ? true : false;

                        oticket.CreatedByLabel = "Created By";
                        oticket.IsCreatedBy= (data == null || (data != null && data.IsCreatedBy) ? true : false);

                        oticket.CreatedDateLabel = "Created Date";
                        oticket.IsCreatedDate = (data == null || (data != null && data.IsCreatedDate) ? true : false);

                        oticket.AssignedToNameLabel = "Assigned To";
                        oticket.IsAssignedToName = (data == null || (data != null && data.IsAssignedTo) ? true : false);

                        oticket.AssignedByNameLabel = "Assigned By";
                        oticket.IsAssignedByName = (data == null || (data != null && data.IsAssignedBy) ? true : false);

                        oticket.ModifiedDateLabel = "Modified Date";
                        oticket.IsModifiedDate = (data == null || (data != null && data.IsModifiedDate) ? true : false);


                        oticket.ExtraCol1Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text :"Additional 1";
                        oticket.IsExtraCol1 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? data.IsExtraCol1 : false;
                       
                        oticket.ExtraCol2Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : "Additional 2";
                        oticket.IsExtraCol2 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? data.IsExtraCol2 : false;
                       
                        oticket.ExtraCol3Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : "Additional 3";
                        oticket.IsExtraCol3 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? data.IsExtraCol3 : false;
                       
                        oticket.ExtraCol4Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : "Additional 4";
                        oticket.IsExtraCol4 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? data.IsExtraCol4 : false;
                        
                        oticket.ExtraCol5Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text :"Additional 5";
                        oticket.IsExtraCol5 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? data.IsExtraCol5 : false;
                        
                        oticket.ExtraCol6Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : "Additional 6";
                        oticket.IsExtraCol6 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? data.ISExtraCol6 : false;
                        
                        oticket.ExtraCol7Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : "Additional 7";
                        oticket.IsExtraCol7 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? data.ISExtraCol7 : false;
                       
                        oticket.ExtraCol8Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : "Additional 8";
                        oticket.IsExtraCol8 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? data.ISExtraCol8 : false;
                       
                        oticket.ExtraCol9Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : "Additional 9";
                        oticket.IsExtraCol9 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? data.IsExtraCol9 : false;
                        
                        oticket.ExtraCol10Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : "Additional 10";
                        oticket.IsExtraCol10 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? data.IsExtraCol10 : false;
                      
                        oticket.ExtraCol11Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : "Additional 11";
                        oticket.IsExtraCol11 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? data.IsExtraCol11 : false;
                       
                        oticket.ExtraCol12Label = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : "Additional 12";
                        oticket.IsExtraCol12 = GetFormData != null && data != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? data.IsExtraCol12 : false;

                        TicketList.Add(oticket);
                    }

                    string urgencyName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";
                    string statusName = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";
                    string errName  = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";

                    if (!string.IsNullOrWhiteSpace(UrgencyName) && UrgencyName!= String.Format("Select {0}", urgencyName))
                    {
                        TicketList = TicketList.Where(em => em.UrgencyName == UrgencyName).ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(ErrorTypeName)&& ErrorTypeName!= String.Format("Select {0}", errName))
                    {
                        TicketList = TicketList.Where(em => em.ErrorTypeName == ErrorTypeName).ToList();
                    }
                    if (!string.IsNullOrEmpty(TicketStatusName) && TicketStatusName!= String.Format("Select {0}", statusName))
                    {
                        TicketList = TicketList.Where(em => em.TicketStatusName == TicketStatusName).ToList();
                    }                    
                }
               
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            
            if (TicketList.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, TicketList);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "No record found");
            }
        }
    }
}
