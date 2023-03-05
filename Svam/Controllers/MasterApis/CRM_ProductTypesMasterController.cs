using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Text.RegularExpressions;
using System.Web.Http;
using Traders.Models;


namespace Svam.Controllers.MasterApis
{
    public class CRM_ProductTypesMasterController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities();
        #endregion

        //api/CRM_ProductTypesMaster/GetProductTypeList?CompanyID=307&BranchID=184&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage GetProductTypeList(string CompanyID, string BranchID, string Token)
        {
            string ErrorMessage = string.Empty;


            var productTypes = new List<ProductTypeApiModel>();
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

                var data = db.crm_producttypetbl.Where(em => em.BranchID == branchID && em.CompanyID == companyID).ToList();
                if (data != null && data.Count > 0)
                {
                    productTypes = (from d in data
                                   select new ProductTypeApiModel
                                   {
                                       Id = d.Id,
                                       ProductTypeName = d.ProductTypeName,
                                       Status = (bool)d.Status,
                                       BranchID = d.BranchID,
                                       CompanyID = d.CompanyID
                                   }
                        ).ToList();
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
                return Request.CreateResponse(HttpStatusCode.OK, productTypes);
            }
        }


        //api/CRM_ProductTypesMaster/AddUpdateProductType
        [HttpPost]
        public HttpResponseMessage AddUpdateProductType([FromBody]JToken postData, HttpRequestMessage request)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {

                ProductTypeApiModel CIM = JsonConvert.DeserializeObject<ProductTypeApiModel>(postData.ToString());

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

                if (string.IsNullOrEmpty(CIM.ProductTypeName))
                {
                    ErrorMessage = "Please enter product type name";
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }


                #endregion
                if (CIM.Id > 0)
                {
                    var GetData = db.crm_producttypetbl.Where(em => em.Id == CIM.Id && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (GetData != null)
                    {
                        GetData.ProductTypeName = CIM.ProductTypeName;
                        db.SaveChanges();
                        SuccessMessage = "Product type updated successfully";
                    }
                    else
                    {
                        ErrorMessage = "** Somthing went wrong, while reading data, Please check the GET Data Parameters **";
                    }
                }
                else
                {
                    var checkExist = db.crm_producttypetbl.Where(em => em.ProductTypeName.ToLower() == CIM.ProductTypeName.ToLower() && em.BranchID == branchID && em.CompanyID == companyID).FirstOrDefault();
                    if (checkExist == null)
                    {
                        crm_producttypetbl lst = new crm_producttypetbl();
                        lst.ProductTypeName = CIM.ProductTypeName;
                        lst.BranchID = branchID;
                        lst.CompanyID = companyID;
                        lst.Status = true;
                        db.crm_producttypetbl.Add(lst);
                        if (db.SaveChanges() > 0)
                        {
                            SuccessMessage = "Product type added successfully";
                        }
                    }
                    else
                    {
                        ErrorMessage = "This Product type name is already available";
                    }
                }

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

        //api/CRM_ProductTypesMaster/GetProductTypeById?productTypeId=852
        [HttpGet]
        public HttpResponseMessage GetProductTypeById(int productTypeId)
        {
            string ErrorMessage = string.Empty;


            var PTM = new ProductTypeApiModel();
            try
            {
                var data = db.crm_producttypetbl.Where(em => em.Id == productTypeId).FirstOrDefault();
                if (data != null)
                {
                    PTM.Id = data.Id;
                    PTM.ProductTypeName = data.ProductTypeName;
                    PTM.Status = Convert.ToBoolean(data.Status);
                    PTM.CompanyID = data.CompanyID;
                    PTM.BranchID = data.BranchID;
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
                return Request.CreateResponse(HttpStatusCode.OK, PTM);
            }
        }

        //api/CRM_ProductTypesMaster/ChangeProductTypeStatusById?productTypeId=852&CompanyID=307&BranchID=184&Status=true&Token=VwFdB3OPEwOoHnr15a5qgg==
        [HttpGet]
        public HttpResponseMessage ChangeProductTypeStatusById(int productTypeId, string CompanyID, string BranchID, bool Status, string Token)
        {
            string ErrorMessage = string.Empty;
            string SuccessMessage = string.Empty;

            try
            {
                int branchID = Convert.ToInt32(BranchID);
                int companyID = Convert.ToInt32(CompanyID);

                //string Token = string.Empty;

                //var re = Request;
                //var headers = re.Headers;

                //if (headers.Contains("Token"))
                //{
                //    Token = headers.GetValues("Token").First();
                //}
                var auth = Utility.TokenVerify(companyID, Token);//verify token for is authorized user

                if (auth == false)
                {
                    ErrorMessage = string.Format("** User authentication failed!");
                    HttpError err = new HttpError(ErrorMessage);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, err);
                }

                var data = db.crm_producttypetbl.Where(em => em.Id == productTypeId).FirstOrDefault();
                if (data != null)
                {
                    data.Status = Status == true ? true : false;
                    db.SaveChanges();

                    SuccessMessage = "Status changed successfully";
                }
                else
                {
                    ErrorMessage = string.Format("** Somthing went wrong, while reading data, Please check the GET Data Parameters **");
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
                return Request.CreateResponse(HttpStatusCode.OK, SuccessMessage);
            }
        }
    }
}
