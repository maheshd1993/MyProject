using Newtonsoft.Json;
using Svam.EF;
using Svam.Models;
using Svam.Models.ApiModel;
using Svam.Models.FBApiModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;


namespace Svam.Controllers.FacebookApi
{
    public class webhooksController : ApiController
    {
        niscrmEntities db = new niscrmEntities();

       
        [HttpGet]
        public HttpResponseMessage Get()
        {
           //SmartCapita Page ID: 625832474544280
            string token = "EAAQPIAriFpIBAJ8o4LrHPKDtyPdhzM3Lz6Bb6yijZAUmrl0CzXdCK02Ipsi68R18g4VBZCYbAe5aTd4b4N8f5LnA3iENkipT6k2iZCAfbuGDwrKarj6RDfTyN5lDfZCvrcROPTVoYZBTuANGOA1odVbsE0m8RwijWKQpTwvTksgZDZD";
           // string token = "EAAQPIAriFpIBACaZAxwp9SnMVtYdSDEQVMZBwNnZAtK7dQ9vTZCUnqHK8vezjXxBAydNI1jzm31BCEWawnRZABjivWVsJNif3m4ZBp6QlG4E70VwZBF5AtxDwnezZBiEddBTnZAaE7bOKK8GixppwfiQ1lbZAxReUdRqda0s1W72Qy0nckPGe9lQCbNT7uQRfYlmPSRYyZCy0NqcnHqpZAz6EnhooNGcPEcixyB4DpccZBlt0Vhrvt9CwSZBWJ2oGT4RaNVygZD";
           string verifyToken = System.Web.HttpContext.Current.Request.QueryString["hub.verify_token"];
           if(token==verifyToken)
           {
              var challenge = System.Web.HttpContext.Current.Request.QueryString["hub.challenge"];
               //var response = new HttpResponseMessage(HttpStatusCode.OK)
               //{
               //    Content = new StringContent(System.Web.HttpContext.Current.Request.QueryString["hub.challenge"])
               //};
               //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
               challenge = string.Format("webhook-{0}", challenge); //"webhook-" + challenge;
               return Request.CreateResponse(HttpStatusCode.OK,challenge);
                //return response;
                //var response = new HttpResponseMessage(HttpStatusCode.OK)
                //{
                //    Content = new StringContent(HttpContext.Current.Request.QueryString["hub.challenge"])
                //};
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                //return response;
            }
           else
           {
               return new HttpResponseMessage(HttpStatusCode.BadRequest);
           }    
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] JsonData data)
        {
           try
           {
               var entry = data.Entry.FirstOrDefault();
               //Get change
               var change = entry?.Changes.FirstOrDefault();
               if (change == null) return new HttpResponseMessage(HttpStatusCode.BadRequest);
               //Get lead Id
               var leadId = change.Value.LeadGenId;
               //Generate user access token here https://developers.facebook.com/tools/accesstoken/
               const string token = "EAAQPIAriFpIBAJ8o4LrHPKDtyPdhzM3Lz6Bb6yijZAUmrl0CzXdCK02Ipsi68R18g4VBZCYbAe5aTd4b4N8f5LnA3iENkipT6k2iZCAfbuGDwrKarj6RDfTyN5lDfZCvrcROPTVoYZBTuANGOA1odVbsE0m8RwijWKQpTwvTksgZDZD";
               var leadUrl = $"https://graph.facebook.com/v14.0/{change.Value.LeadGenId}?access_token={token}";
               var formUrl = $"https://graph.facebook.com/v14.0/{change.Value.FormId}?access_token={token}";

               using (var httpClientLead = new HttpClient())
               {
                   var response =await httpClientLead.GetStringAsync(formUrl);
                   if(!string.IsNullOrEmpty(response))
                   {
                       var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(response);
                       //jsonObjLead.Name contains the lead ad name

                       //If response is valid get the field data
                       using (var httpClientFields = new HttpClient())
                       {
                           var responseFields = await httpClientFields.GetStringAsync(leadUrl);
                           if (!string.IsNullOrEmpty(responseFields))
                           {
                               var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);
                               List<LeadModel> leads = new List<LeadModel>();
                               LeadModel obj = new LeadModel();
                               foreach (var t in jsonObjFields.FieldData)
                               {

                                   if (t.Name == "full_name")
                                   {
                                       obj.Customer = t.Values.FirstOrDefault();
                                   }
                                     
                                    //if (t.Name == "last_name")
                                    //{
                                    //    lastName = t.Values.FirstOrDefault();
                                    //}
                                    else if (t.Name == "email")
                                   {
                                       obj.Email = t.Values.FirstOrDefault();
                                   }
                                   else if (t.Name == "phone_number")
                                   {
                                       obj.Mobno = t.Values.FirstOrDefault();
                                   }
                                   else if (t.Name == "city")
                                   {
                                       obj.City = t.Values.FirstOrDefault();
                                   }
                                   else if (t.Name == "state")
                                   {
                                       obj.State = t.Values.FirstOrDefault();
                                   }
                                   else if (t.Name == "country")
                                   {
                                       obj.Country = t.Values.FirstOrDefault();
                                   }
                                   else
                                   {

                                   }
                               }
                               leads.Add(obj);
                               #region save leads in db
                               if (leads != null && leads.Count > 0)
                               {
                                   int BranchID = 173;
                                   int CompanyID = 296;
                                   var getleadSourceStatus = db.crm_leadsource_tbl.Where(em => em.LeadsourceName == "Facebook" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                   var getStatus = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

                                   foreach (var item in leads)
                                   {
                                       string Mobno = item.Mobno;
                                       string customer = item.Customer;
                                       string email = item.Email;
                                       string city = string.Empty;
                                       string state = string.Empty;
                                       string country = string.Empty;

                                       if (Convert.ToString(Mobno).Contains("+91"))
                                       {
                                           Mobno = Mobno.Replace("+91", "");
                                       }

                                       //bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");

                                       if (!string.IsNullOrEmpty(Mobno) && !string.IsNullOrEmpty(customer))
                                       {
                                           var Getexists = db.crm_createleadstbl.Where(em => em.MobileNo.Replace("+91", "").Trim() == Mobno.Trim() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                           if (Getexists == null)
                                           {
                                               var getState = new com_state();
                                               var getCity = new com_city();
                                               var GetCountry = new acc_countries();
                                               if (!string.IsNullOrEmpty(country))
                                               {
                                                    GetCountry =  db.acc_countries.Where(em => em.country_name.ToLower() == country.ToLower()).FirstOrDefault();
                                               }
                                               if (!string.IsNullOrEmpty(state))
                                               {
                                                   if(GetCountry!=null && GetCountry.id==1)
                                                   {
                                                       getState = db.com_state.Where(em => em.State.Substring(3).ToLower() == state.ToLower()).FirstOrDefault();
                                                   }
                                                   else
                                                   {
                                                       getState = db.com_state.Where(em => em.State.ToLower() == state.ToLower()).FirstOrDefault();
                                                   }
                                               }

                                               if (!string.IsNullOrEmpty(city))
                                               {
                                                   getCity = db.com_city.Where(em => em.City.ToLower() == city.ToLower()).FirstOrDefault();
                                               }
                                               crm_createleadstbl CL = new crm_createleadstbl();
                                               CL.LeadOwner = 50;
                                               CL.MobileNo = Mobno.Trim();
                                               CL.Customer = customer.Trim();
                                               CL.EmailId = email;
                                               CL.LeadStatusID = getStatus != null ? getStatus.Id : 0;
                                               CL.LeadStatus = getStatus != null ? getStatus.LeadStatusName : string.Empty;
                                               CL.LeadSourceID = getleadSourceStatus == null ? 0 : getleadSourceStatus.Id;
                                               CL.LeadResource = getleadSourceStatus == null ? string.Empty : getleadSourceStatus.LeadsourceName;
                                               CL.FollowDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                               CL.date = DateTime.Now.ToString("dd/MM/yyyy");
                                               CL.Createddate = DateTime.Now;
                                               CL.Status = true;
                                               CL.CountryID = GetCountry != null ? GetCountry.id : 0;
                                               CL.StateID = getState != null ? getState.ID : 0;
                                               CL.CityID = getCity != null ? getCity.ID : 0;
                                               CL.LeadsType = "Facebook";
                                               CL.CompanyID = CompanyID;
                                               CL.BranchID = BranchID;
                                               db.crm_createleadstbl.Add(CL);
                                               db.SaveChanges();
                                           }

                                       }
                                   }
                               }
                               #endregion
                           }
                       }
                   }
               }
               return new HttpResponseMessage(HttpStatusCode.OK);
           }
           catch (Exception ex)
           {
               ExceptionLogging.SendExcepToDB(ex);
               return new HttpResponseMessage(HttpStatusCode.BadRequest);
           }
        }




        // #region Get Request  

        // [HttpGet]
        // public HttpResponseMessage Get()
        // {
            // //SmartCapita Page ID: 625832474544280
            // //LeadAdsTest app AccessToken=EAANOmFX2UV4BAD8IXlvLC73f70W6SEaBKXoSENYxevZB2dSf1uzNvZAMZCs0w82x3plAGvvFgL6EyEdWIxcVAyeOZAagZAwdKZAJEunTW9FmbY3vqO5mkHD2gQ4Pav1ZAUlAdPaLyuAMbO4oNXAgR7cPl6V522cMZBNT8yelgijZAk6oPXf00zSvHq7KAVbeD7tcZD
            // string token = "689545360";
            // string verifyToken = System.Web.HttpContext.Current.Request.QueryString["hub.verify_token"];
            // if (token == verifyToken)
            // {
                // var challenge = System.Web.HttpContext.Current.Request.QueryString["hub.challenge"];
                // //var response = new HttpResponseMessage(HttpStatusCode.OK)
                // //{
                // //    Content = new StringContent(System.Web.HttpContext.Current.Request.QueryString["hub.challenge"])
                // //};
                // //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                // challenge = string.Format("webhook-{0}", challenge); //"webhook-" + challenge;
                // return Request.CreateResponse(HttpStatusCode.OK, challenge);
                // //return response;
            // }
            // else
            // {
                // return new HttpResponseMessage(HttpStatusCode.BadRequest);
            // }
        // }
         
        // #endregion Get Request


        // #region post Request  
        // [HttpPost]
        // public async Task<HttpResponseMessage> Post([FromBody] JsonData data)
        // {


            // try
            // {
                // var entry = data.Entry.FirstOrDefault();
                // var change = entry?.Changes.FirstOrDefault();
                // if (change == null)
                // {
                    // return new HttpResponseMessage(HttpStatusCode.BadRequest);
                // }

                // /// generate user access token here https://developers.facebook.com/tools/accesstoken/

                // const string token = "abcxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
                // //  const string apptoken = "1144465609458953|lZXz6ERBKW_DMtXkIT_Ahi96JH8";
                // // const string usertoken = "EAAQQ4qocBQkBAGZBz57OXHZBmDb1a5M1XZC1uO7GmEi0K4OXtvZAtQP70jwdmEF1CjD7IH3Q5YZBruERL1EMqaFAypuOVUtSIV4q1UK0KfOqQKNTmATb8GYyQZCJC7zXsIAZAuH0mhIpaG3aOAEPhW7l7VrZC8zIyLoQ8GJZBcwSl5ZC1tvV5faPtVb3fATRoy8esZD";

                // var leadUrl = $"https://graph.facebook.com/v2.10/{change.Value.LeadGenId}?access_token={token}";
                // var formUrl = $"https://graph.facebook.com/v2.10/{change.Value.FormId}?access_token={token}";

                // using (var httpClientLead = new HttpClient())
                // {
                    // var response = await httpClientLead.GetStringAsync(formUrl);
                    // if (!string.IsNullOrEmpty(response))
                    // {
                        // var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(response);
                        // ///jsonObjLead.name  contains the lead ad name 

                        // ////if response is valid get the field data
                        // using (var httpClientFields = new HttpClient())
                        // {
                            // var responseFields = await httpClientFields.GetStringAsync(leadUrl);
                            // if (!string.IsNullOrEmpty(responseFields))
                            // {
                                // var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);
                                // LeadModel obj = new LeadModel();
                                // //// lead obj will use according to requirement
                                // List<LeadModel> leads = new List<LeadModel>();
                                // foreach (var t in jsonObjFields.FieldData)
                                // {
                                    // if (t.Name == "full_name")
                                    // {
                                        // obj.FullName = t.Values.FirstOrDefault();
                                    // }
                                    // if (t.Name == "first_name")
                                    // {
                                        // obj.FirstName = t.Values.FirstOrDefault();
                                    // }
                                    // if (t.Name == "last_name")
                                    // {
                                        // obj.LastName = t.Values.FirstOrDefault();

                                    // }
                                    // if (t.Name == "email")
                                    // {
                                        // obj.Email = t.Values.FirstOrDefault();

                                    // }
                                    // if (t.Name == "phone_number")
                                    // {
                                        // obj.Mobno = t.Values.FirstOrDefault();
                                    // }
                                    // if (t.Name == "work_phone_number")
                                    // {
                                        // obj.WhatsAppNo = t.Values.FirstOrDefault();
                                    // }
                                    // if (t.Name == "State")
                                    // {
                                        // obj.State = t.Values.FirstOrDefault();
                                    // }
                                    // if (t.Name == "hometown")
                                    // {
                                        // obj.City = t.Values.FirstOrDefault();
                                    // }
                                // }
                                // obj.createdOn = DateTime.Now;
                                // leads.Add(obj);
                                // #region save leads in db
                                // if (leads != null && leads.Count > 0)
                                // {
                                    // int BranchID = 173;

                                    // int CompanyID = 296;
                                    // var getleadSourceStatus = db.crm_leadsource_tbl.Where(em => em.LeadsourceName == "Facebook" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                    // var getStatus = db.crm_leadstatus_tbl.Where(em => em.LeadStatusName == "Open" && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();

                                    // foreach (var item in leads)
                                    // {
                                        // string Mobno = item.Mobno;
                                        // string customer = item.Customer;
                                        // string email = item.Email;
                                        // string city = string.Empty;
                                        // string state = string.Empty;
                                        // string country = string.Empty;

                                        // if (Convert.ToString(Mobno).Contains("+91"))
                                        // {
                                            // Mobno = Mobno.Replace("+91", "");
                                        // }

                                        // //bool numeronly = Regex.IsMatch(Mobno.Replace("+91", ""), @"^[0][1-9]\d{9}$|^[1-9]\d{9}$");

                                        // if (!string.IsNullOrEmpty(Mobno) && !string.IsNullOrEmpty(customer))
                                        // {
                                            // var Getexists = db.crm_createleadstbl.Where(em => em.MobileNo.Replace("+91", "").Trim() == Mobno.Trim() && em.BranchID == BranchID && em.CompanyID == CompanyID).FirstOrDefault();
                                            // if (Getexists == null)
                                            // {
                                                // var getState = new com_state();
                                                // var getCity = new com_city();
                                                // var GetCountry = new acc_countries();
                                                // if (!string.IsNullOrEmpty(country))
                                                // {
                                                    // GetCountry = db.acc_countries.Where(em => em.country_name.ToLower() == country.ToLower()).FirstOrDefault();
                                                // }
                                                // if (!string.IsNullOrEmpty(state))
                                                // {
                                                    // if (GetCountry != null && GetCountry.id == 1)
                                                    // {
                                                        // getState = db.com_state.Where(em => em.State.Substring(3).ToLower() == state.ToLower()).FirstOrDefault();
                                                    // }
                                                    // else
                                                    // {
                                                        // getState = db.com_state.Where(em => em.State.ToLower() == state.ToLower()).FirstOrDefault();
                                                    // }
                                                // }

                                                // if (!string.IsNullOrEmpty(city))
                                                // {
                                                    // getCity = db.com_city.Where(em => em.City.ToLower() == city.ToLower()).FirstOrDefault();
                                                // }
                                                // crm_createleadstbl CL = new crm_createleadstbl();
                                                // CL.LeadOwner = 50;
                                                // CL.MobileNo = Mobno.Trim();
                                                // CL.Customer = customer.Trim();
                                                // CL.EmailId = email;
                                                // CL.LeadStatusID = getStatus != null ? getStatus.Id : 0;
                                                // CL.LeadStatus = getStatus != null ? getStatus.LeadStatusName : string.Empty;
                                                // CL.LeadSourceID = getleadSourceStatus == null ? 0 : getleadSourceStatus.Id;
                                                // CL.LeadResource = getleadSourceStatus == null ? string.Empty : getleadSourceStatus.LeadsourceName;
                                                // CL.FollowDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                // CL.date = DateTime.Now.ToString("dd/MM/yyyy");
                                                // CL.Createddate = DateTime.Now;
                                                // CL.Status = true;
                                                // CL.CountryID = GetCountry != null ? GetCountry.id : 0;
                                                // CL.StateID = getState != null ? getState.ID : 0;
                                                // CL.CityID = getCity != null ? getCity.ID : 0;
                                                // CL.LeadsType = "Facebook";
                                                // CL.CompanyID = CompanyID;
                                                // CL.BranchID = BranchID;
                                                // db.crm_createleadstbl.Add(CL);
                                                // db.SaveChanges();
                                            // }

                                        // }
                                    // }
                                // }
                                // #endregion
                            // }
                        // }

                    // }
                // }

                // return new HttpResponseMessage(HttpStatusCode.OK);
            // }
            // catch (Exception ex)
            // {
                // return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // }
        // }
        // #endregion post Request  


    }
}
