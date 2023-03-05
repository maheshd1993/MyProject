using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_TicketFieldDynamicTextController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion
        // GET api/CRM_TicketFieldDynamicText?companyId=307&branchId=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        public HttpResponseMessage Get(int companyId, int branchId, string Token)
        {
            var model = new List<CreateTicketDynamicFieldsAPIModel>();
            string ErrorMessage = string.Empty;
            var auth = Utility.TokenVerify(companyId, Token);//verify token for is authorized user

            if (auth == false)
            {
                ErrorMessage = string.Format("** User authentication failed!");
                HttpError err = new HttpError(ErrorMessage);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            var GetData = new CreateTicketDynamicFieldsAPIModel();

            try
            {
               
                var dataBool = db.crm_ticketcreatesetting.Where(em => em.BranchID == branchId && em.CompanyId == companyId).FirstOrDefault();
                var GetFormData = db.crm_ticketfieldnamecustomized.Where(em => em.BranchID == branchId && em.CompanyId == companyId).FirstOrDefault();

                GetData.IsName = true;
                GetData.IsNameRequired = true;
                GetData.Name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.NameText) ? GetFormData.NameText : "Customer Name";

                GetData.IsEmailID = dataBool == null ? true : dataBool.IsEmailID;
                GetData.IsEmailIDRequired= dataBool == null ? true : dataBool.IsEmailIDRequired;
                GetData.EmailID = GetFormData != null && !string.IsNullOrEmpty(GetFormData.EmailIDText) ? GetFormData.EmailIDText : "Email Address";

                GetData.IsPhoneNumber = true;
                GetData.IsPhoneNumberRequired = true;
                GetData.PhoneNumber = GetFormData != null && !string.IsNullOrEmpty(GetFormData.PhoneNumberText) ? GetFormData.PhoneNumberText : "Phone Number";

                GetData.IsProductTypeID = true;
                GetData.IsProductTypeIDRequired = true;
                GetData.ProductTypeID = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ProductTypeIDText) ? GetFormData.ProductTypeIDText : "Product Type";

                GetData.IsErrorTypeID = true;
                GetData.IsErrorTypeIDRequired = true;
                GetData.ErrorTypeID = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ErrorTypeIDText) ? GetFormData.ErrorTypeIDText : "Error Type";

                GetData.IsUrgencyID = true;
                GetData.IsUrgencyIDRequired = true;
                GetData.UrgencyID = GetFormData != null && !string.IsNullOrEmpty(GetFormData.UrgencyIDText) ? GetFormData.UrgencyIDText : "Urgency Type";

                GetData.IsStatusID = true;
                GetData.IsStatusIDRequired = true;
                GetData.StatusID = GetFormData != null && !string.IsNullOrEmpty(GetFormData.StatusIDText) ? GetFormData.StatusIDText : "Ticket Status";

                GetData.Issubject = true;
                GetData.IssubjectRequired = true;
                GetData.subject = GetFormData != null && !string.IsNullOrEmpty(GetFormData.subjectText) ? GetFormData.subjectText : "Ticket Subject";

                GetData.IsExtraCol1 = (dataBool != null && dataBool.IsExtraCol1) ? true : false;
                GetData.IsExtraCol1Required = (dataBool != null && dataBool.IsExtraCol1Required) ? true : false;
                GetData.ExtraCol1 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol1Text) ? GetFormData.ExtraCol1Text : "Additional 1";

                GetData.IsExtraCol2 = (dataBool != null && dataBool.IsExtraCol2) ? true : false;
                GetData.IsExtraCol2Required = (dataBool != null && dataBool.IsExtraCol2Required) ? true : false;
                GetData.ExtraCol2 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol2Text) ? GetFormData.ExtraCol2Text : "Additional 2";

                GetData.IsExtraCol3 = (dataBool != null && dataBool.IsExtraCol3) ? true : false;
                GetData.IsExtraCol3Required = (dataBool != null && dataBool.IsExtraCol3Required) ? true : false;
                GetData.ExtraCol3 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol3Text) ? GetFormData.ExtraCol3Text : "Additional 3";

                GetData.IsExtraCol4 = (dataBool != null && dataBool.IsExtraCol4) ? true : false;
                GetData.IsExtraCol4Required = (dataBool != null && dataBool.IsExtraCol4Required) ? true : false;
                GetData.ExtraCol4 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol4Text) ? GetFormData.ExtraCol4Text : "Additional 4";

                GetData.IsExtraCol5 = (dataBool != null && dataBool.IsExtraCol5) ? true : false;
                GetData.IsExtraCol5Required = (dataBool != null && dataBool.IsExtraCol5Required) ? true : false;
                GetData.ExtraCol5 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol5Text) ? GetFormData.ExtraCol5Text : "Additional 5";

                GetData.IsExtraCol6 = (dataBool != null && dataBool.ISExtraCol6) ? true : false;
                GetData.IsExtraCol6Required = (dataBool != null && dataBool.ISExtraCol6Required) ? true : false;
                GetData.ExtraCol6 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol6Text) ? GetFormData.ExtraCol6Text : "Additional 6";

                GetData.IsExtraCol7 = (dataBool != null && dataBool.ISExtraCol7) ? true : false;
                GetData.IsExtraCol7Required = (dataBool != null && dataBool.ISExtraCol7Required) ? true : false;
                GetData.ExtraCol7 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol7Text) ? GetFormData.ExtraCol7Text : "Additional 7";

                GetData.IsExtraCol8 = (dataBool != null && dataBool.ISExtraCol8) ? true : false;
                GetData.IsExtraCol8Required = (dataBool != null && dataBool.ISExtraCol8Required) ? true : false;
                GetData.ExtraCol8 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol8Text) ? GetFormData.ExtraCol8Text : "Additional 8";

                GetData.IsExtraCol9 = (dataBool != null && dataBool.IsExtraCol9) ? true : false;
                GetData.IsExtraCol9Required = (dataBool != null && dataBool.IsExtraCol9Required) ? true : false;
                GetData.ExtraCol9 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9Text) ? GetFormData.ExtraCol9Text : "Additional 9";

                GetData.IsExtraCol10 = (dataBool != null && dataBool.IsExtraCol10) ? true : false;
                GetData.IsExtraCol10Required = (dataBool != null && dataBool.IsExtraCol10Required) ? true : false;
                GetData.ExtraCol10 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10Text) ? GetFormData.ExtraCol10Text : "Additional 10";

                GetData.IsExtraCol11 = (dataBool != null && dataBool.IsExtraCol11) ? true : false;
                GetData.IsExtraCol11Required = (dataBool != null && dataBool.IsExtraCol11Required) ? true : false;
                GetData.ExtraCol11 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol11Text) ? GetFormData.ExtraCol11Text : "Additional 11";

                GetData.IsExtraCol12 = (dataBool != null && dataBool.IsExtraCol12) ? true : false;
                GetData.IsExtraCol12Required = (dataBool != null && dataBool.IsExtraCol12Required) ? true : false;
                GetData.ExtraCol12 = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol12Text) ? GetFormData.ExtraCol12Text : "Additional 12";

               

                model.Add(GetData);



            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format("Somthing went wrong, while reading data, Please check the GET Data Format");
                ExceptionLogging.SendExcepToDB(ex);
                HttpError err = new HttpError(ErrorMessage);

                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            return Request.CreateResponse(HttpStatusCode.OK, model);

        }
    }
}
