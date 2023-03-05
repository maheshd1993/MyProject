using Svam.Models;
using Svam.UtilityManager;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;

namespace Svam.Controllers.MasterApis
{
    public class CRM_GetEmailSetupController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_GetEmailSetup?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage Get(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;

           
            var CESM = new CRMEmailSettingModel();
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

                var getEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == companyID && em.BranchCode == branchID).FirstOrDefault();
                if (getEmailSetting != null)
                {
                    CESM.EmailSettingID = getEmailSetting.EmailSettingID;
                    CESM.EmailAddress = getEmailSetting.EmailAddress;
                    CESM.Port = getEmailSetting.Port;
                    CESM.SMTPHost = getEmailSetting.SMTPHost;
                    CESM.UserName = getEmailSetting.UserName;
                    CESM.Password = getEmailSetting.Password;
                    CESM.SSL = getEmailSetting.SSL;
                    CESM.CCEmail = getEmailSetting.CCEmail;
                    CESM.DisplayName = getEmailSetting.DisplayName;
                }
                else
                {
                    ErrorMessage = string.Format("No record found");
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
                return Request.CreateResponse(HttpStatusCode.OK, CESM);
            }
        }
    }
}
