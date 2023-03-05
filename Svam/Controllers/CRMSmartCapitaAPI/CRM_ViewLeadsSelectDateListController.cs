using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Traders.Models;
using Svam.EF;
using Svam.Models.ApiModel;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRM_ViewLeadsSelectDateListController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_ViewLeadsSelectDateList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;
            List<SelectDateTypesDTO> typeList = new List<SelectDateTypesDTO>();

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

                //check dynamic fields data according to company and branch id
                var data = db.crm_viewleadsetting.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                var GetFormData = db.crm_customizedformfieldtextname.Where(em => em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();

                string extracol9Name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol9TextName) ? GetFormData.ExtraCol9TextName : "Additional 9";
                string extracol10Name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol10TextName) ? GetFormData.ExtraCol10TextName : "Additional 10";
                string extracol18Name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol18TextName) ? GetFormData.ExtraCol18TextName : "Additional 18";
                string extracol19Name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol19TextName) ? GetFormData.ExtraCol19TextName : "Additional 19";
                string extracol20Name = GetFormData != null && !string.IsNullOrEmpty(GetFormData.ExtraCol20TextName) ? GetFormData.ExtraCol20TextName : "Additional 20";

                typeList.Add(new SelectDateTypesDTO() { Value = "all", Text = "Select Date" });
                typeList.Add(new SelectDateTypesDTO() { Value = "Created Date", Text = "Created Date" });
                typeList.Add(new SelectDateTypesDTO() { Value = "Modified Date", Text = "Modified Date" });
                typeList.Add(new SelectDateTypesDTO() { Value = "Followup Date", Text = "Followup Date" });
                typeList.Add(new SelectDateTypesDTO() { Value = "AssignDate", Text = "Assigned Date" });
                typeList.Add(new SelectDateTypesDTO() { Value = "DOB", Text = "Date of Birth" });
                typeList.Add(new SelectDateTypesDTO() { Value = "MarriageAnniversary", Text = "Marriage Anniversary" });
                typeList.Add(new SelectDateTypesDTO() { Value = "ExpectedDate", Text = "Expected Closing Date" });

                if(data!=null && data.IsExtraCol9Filter)
                {
                    typeList.Add(new SelectDateTypesDTO() { Value = "ExtCol9Date", Text = extracol9Name });
                }
                if(data != null && data.IsExtraCol10Filter)
                {
                    typeList.Add(new SelectDateTypesDTO() { Value = "ExtCol10Date", Text = extracol10Name });
                }
                if (data != null && data.IsExtraCol18Filter)
                {
                    typeList.Add(new SelectDateTypesDTO() { Value = "ExtCol18Date", Text = extracol18Name });
                }
                if (data != null && data.IsExtraCol19Filter)
                {
                    typeList.Add(new SelectDateTypesDTO() { Value = "ExtCol19Date", Text = extracol19Name });
                }
                if (data != null && data.IsExtraCol20Filter)
                {
                    typeList.Add(new SelectDateTypesDTO() { Value = "ExtCol20Date", Text = extracol20Name });
                }
                
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
                return Request.CreateResponse(HttpStatusCode.OK, typeList);
            }
        }
    }
}
