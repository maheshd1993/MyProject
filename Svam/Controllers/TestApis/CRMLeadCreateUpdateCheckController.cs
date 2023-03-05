using Newtonsoft.Json;
using Svam.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Svam.Controllers.TestApis
{
    public class CRMLeadCreateUpdateCheckController : ApiController
    {
        #region Post Insert and Update the Lead of the Login User
        /// <summary>
        /// Post Insert and Update the Lead of the Login User
        /// GET api/CRMLeadCreateUpdate
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="UserName"></param>
        /// <param name="CLM"></param>
        /// <param name="id"></param>
        /// <param name="CompanyID"></param>
        /// <param name="BranchID"></param>
        /// <param name="file"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<HttpResponseMessage> CheckPostData() 
        {
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
            var json = await filesReadToProvider.Contents[0].ReadAsStringAsync();
            var fileBytes = await filesReadToProvider.Contents[0].ReadAsByteArrayAsync();

            APILeadModel value = JsonConvert.DeserializeObject<APILeadModel>(json);

            
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;
           
                try
                {
                var localtime = DateTime.Now;
                #region Add-Lead-Description and Attachment file
                string FileFullName = string.Empty;
                string FileName = string.Empty;

                Dictionary<string, object> dict = new Dictionary<string, object>();
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            int MaxContentLength = 1024 * 1024 * 5; //Size = 5 MB  

                            IList<string> AllowedFileExtensions = new List<string> { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                            //var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var ext = Path.GetExtension(postedFile.FileName);
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {
                                var message = string.Format("Please Upload  the file of type as .txt,.doc,.docx,.pdf,.xls,.xlsx,.jpg,.jpeg,.png,.gif,.bmp.");
                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                            else if (postedFile.ContentLength > MaxContentLength)
                            {
                                var message = string.Format("Please Upload a file upto 5 mb.");
                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }
                            else
                            {
                                //var CLM = db.crm_createleadstbl.Where(em => em.Id == LeadID && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                                FileName = "Lead-Test-Check"+ localtime.ToString("ddMMyyyyhhmmss") + "";
                                FileFullName = FileName + extension;
                                var filePath = HttpContext.Current.Server.MapPath("~/LeadAttachment/" + FileFullName);
                                postedFile.SaveAs(filePath);
                            }
                        }
                    }
                }



                SuccessMessage = value.LeadID>0?Convert.ToString(value.LeadID):"0";
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
                #endregion

            }
            catch (Exception ex)
                {
                  
                    ExceptionLogging.SendExcepToDB(ex);
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the Post Data(Format) Parameters **");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }                   
        }
        #endregion
    }
}
