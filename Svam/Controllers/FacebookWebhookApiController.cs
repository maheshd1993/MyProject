using Newtonsoft.Json;
using Svam.EF;
using Svam.Models.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using static Svam.Models.CRMLeadModel;

namespace Svam.Controllers
{

    //#region facebook reviews classes
    //public class JsonData
    //{
    //    [JsonProperty("entry")]
    //    public List<Entry> Entry { get; set; }

    //    [JsonProperty("object")]
    //    public string Object { get; set; }
    //}

    //public class Entry
    //{
    //    [JsonProperty("changes")]

    //    public List<Change> Changes { get; set; }

    //    [JsonProperty("id")]

    //    public string Id { get; set; }

    //    [JsonProperty("time")]

    //    public int Time { get; set; }
    //}

    //public class Change
    //{
    //    [JsonProperty("field")]

    //    public string Field { get; set; }

    //    [JsonProperty("value")]

    //    public Value Value { get; set; }

    //}

    //public class Value
    //{
    //    [JsonProperty("ad_id")]

    //    public string AdId { get; set; }

    //    [JsonProperty("form_id")]

    //    public string FormId { get; set; }

    //    [JsonProperty("leadgen_id")]

    //    public string LeadGenId { get; set; }

    //    [JsonProperty("created_time")]

    //    public int CreatedTime { get; set; }

    //    [JsonProperty("page_id")]

    //    public string PageId { get; set; }

    //    [JsonProperty("adgroup_id")]

    //    public string AdGroup_Id { get; set; }
    //}

    //public class LeadData
    //{
    //    [JsonProperty("created_time")]

    //    public string CreatedTime { get; set; }

    //    [JsonProperty("id")]

    //    public string Id { get; set; }

    //    [JsonProperty("field_data")]

    //    public List<FieldData> FieldData { get; set; }
    //}

    //public class FieldData
    //{
    //    [JsonProperty("name")]

    //    public string Name { get; set; }

    //    [JsonProperty("values")]

    //    public List<string> Values { get; set; }
    //}

    //public class LeadFormData
    //{
    //    [JsonProperty("id")]

    //    public string Id { get; set; }

    //    [JsonProperty("leadgen_export_csv_url")]

    //    public string CsvExportUrl { get; set; }

    //    [JsonProperty("locale")]

    //    public string Locale { get; set; }

    //    [JsonProperty("name")]

    //    public string Name { get; set; }

    //    [JsonProperty("status")]

    //    public string Status { get; set; }
    //}

    //#endregion facebook reviews classes



    //#region facebook APi Integration classes
    //public class FacebookWebhooksControllers : ApiController
    //{
    //    #region Get Request  
    //    [System.Web.Http.HttpGet]
    //    public HttpResponseMessage FacebookWebhooksGet()
    //    {
    //        var response = new HttpResponseMessage(HttpStatusCode.OK)
    //        {
    //            Content = new StringContent(System.Web.HttpContext.Current.Request.QueryString["hub.challenge"])
    //        };
    //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
    //        return response;
    //    }
    //    #endregion Get Request


    //    #region post Request  
    //    [System.Web.Http.HttpPost]
    //    public async Task<HttpResponseMessage> FacebookWebhooksPostmethod([FromBody] JsonData data)
    //    {


    //        try
    //        {
    //            var entry = data.Entry.FirstOrDefault();
    //            var change = entry?.Changes.FirstOrDefault();
    //            if (change == null)
    //            {
    //                return new HttpResponseMessage(HttpStatusCode.BadRequest);
    //            }

    //            /// generate user access token here https://developers.facebook.com/tools/accesstoken/

    //            const string token = "abcxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
    //          //  const string apptoken = "1144465609458953|lZXz6ERBKW_DMtXkIT_Ahi96JH8";
    //           // const string usertoken = "EAAQQ4qocBQkBAGZBz57OXHZBmDb1a5M1XZC1uO7GmEi0K4OXtvZAtQP70jwdmEF1CjD7IH3Q5YZBruERL1EMqaFAypuOVUtSIV4q1UK0KfOqQKNTmATb8GYyQZCJC7zXsIAZAuH0mhIpaG3aOAEPhW7l7VrZC8zIyLoQ8GJZBcwSl5ZC1tvV5faPtVb3fATRoy8esZD";

    //            var leadUrl = $"https://graph.facebook.com/v2.10/{change.Value.LeadGenId}?access_token={token}";
    //            var formUrl = $"https://graph.facebook.com/v2.10/{change.Value.FormId}?access_token={token}";

    //            using (var httpClientLead = new HttpClient())
    //            {
    //                var response = await httpClientLead.GetStringAsync(formUrl);
    //                if (!string.IsNullOrEmpty(response))
    //                {
    //                    var jsonObjLead = JsonConvert.DeserializeObject<LeadFormData>(response);
    //                    ///jsonObjLead.name  contains the lead ad name 

    //                    ////if response is valid get the field data
    //                    using (var httpClientFields = new HttpClient())
    //                    {
    //                        var responseFields = await httpClientFields.GetStringAsync(leadUrl);
    //                        if (!string.IsNullOrEmpty(responseFields))
    //                        {
    //                            var jsonObjFields = JsonConvert.DeserializeObject<LeadData>(responseFields);
    //                            LeadModel obj = new LeadModel();
    //                            //// lead obj will use according to requirement
    //                           List<LeadModel> leads = new List<LeadModel>();
    //                            foreach (var t in jsonObjFields.FieldData)
    //                            {
    //                                if (t.Name == "full_name")
    //                                {
    //                                    obj.FullName = t.Values.FirstOrDefault();
    //                                }
    //                                if (t.Name == "first_name")
    //                                {
    //                                    obj.FirstName = t.Values.FirstOrDefault();
    //                                }
    //                                if (t.Name == "last_name")
    //                                {
    //                                    obj.LastName = t.Values.FirstOrDefault();

    //                                }
    //                                if (t.Name == "email")
    //                                {
    //                                    obj.Email = t.Values.FirstOrDefault();

    //                                }
    //                                if (t.Name == "phone_number")
    //                                {
    //                                    obj.Mobno = t.Values.FirstOrDefault();
    //                                }
    //                                if (t.Name == "work_phone_number")
    //                                {
    //                                    obj.WhatsAppNo = t.Values.FirstOrDefault();
    //                                }
    //                                if (t.Name == "State")
    //                                {
    //                                    obj.State = t.Values.FirstOrDefault();
    //                                }
    //                                if (t.Name == "Country")
    //                                {
    //                                    obj.Country = t.Values.FirstOrDefault();
    //                                }
    //                            }
    //                             obj.createdOn = DateTime.Now;
    //                            leads.Add(obj);
    //                        }
    //                    }

    //                }
    //            }
    //            crm_createleadstbl CL = new crm_createleadstbl();

    //            return new HttpResponseMessage(HttpStatusCode.OK);
    //        }
    //        catch (Exception ex)
    //        {
    //            return new HttpResponseMessage(HttpStatusCode.BadRequest);

    //        }
    //    }
    //    #endregion post Request  
      
        
        
        
    //}

    //#endregion facebook APi Integration classes



    //[HttpPost]
    //public IActionResult Index()
    //{
    //    var payloadString = GetRequestBody().Result.ToString();
    //   // var signature = Request.Headers[webhookSettings.Value.XeroSignature].FirstOrDefault();
    //    //TestResponse(payloadString, signature);

    // //   if (!VerifySignature(payloadString, signature))
    // //   {
    //        // Webhook signature invalid, reject payload
    //   //     return Unauthorized(string.Empty);
    //  //  }

    //    // Valid signature, enqueue payload to queue and start asynchronous processing of payload
    //   // payloadQueue.Enqueue(JsonConvert.DeserializeObject<XeroPayload>(payloadString));
    //  //  _ProcessPayloadWorker.RunWorkerAsync();

    //    return Ok(string.Empty);
    //}

    //private async Task<string> GetRequestBody()
    //{
    //    using (var reader = new StreamReader(Request.Body))
    //    {
    //        return await reader.ReadToEndAsync();
    //    }
    //}









    //string fbAppAccess_Token = ConfigurationManager.AppSettings["fbApp_Token"];
    //string fbAPP_ID = ConfigurationManager.AppSettings["fbAPP_ID"];
    //string fbApp_secret = ConfigurationManager.AppSettings["fbApp_secret"];


    //#region Check Fb Authorize

    ////Facebook App Authorization
    //private bool IsValidFacebookApp(Int64 SMID, string AppID, string accessToken)
    //{
    //    try
    //    {
    //        string FeedRequestUrl = string.Concat("https://graph.facebook.com/" + AppID);
    //        HttpWebRequest feedRequest = (HttpWebRequest)WebRequest.Create(FeedRequestUrl);
    //        feedRequest.Method = "GET";
    //        feedRequest.Accept = "application/json";
    //        feedRequest.ContentType = "application/json; charset=utf-8";
    //        WebResponse feedResponse = (HttpWebResponse)feedRequest.GetResponse();
    //        using (feedResponse)
    //        {
    //            using (var reader = new StreamReader(feedResponse.GetResponseStream()))
    //            {
    //                var data = reader.ReadToEnd();
    //                if (data != null)
    //                {
    //                    var result = IsValidFacebookAccessToken(SMID, accessToken, false);

    //                    return result;
    //                }
    //                else
    //                {   //error
    //                    return false;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return false;
    //    }
    //}
    //#endregion

    //#region Post Comment on Review

    ///// Post Comment on facebook page review using api
    //private string FacebookCommentOnReview(string ReviewID, string Comment, string accessToken)
    //{
    //    try
    //    {
    //        //accessToken = "EAAecdBkoxHMBALglrsNKMvPK4ohy3fb3Df3pL44JxD8VCNguVshCESVHhMPNN1ZA7gYPFwqMIKwFWJuyBUZB6aqT6f0QXHlccFRjmrJzs568NNSRG66Jvok4JZAieqUAxuDtZAQLzsu4xC3o5rHwbJ3R45XSk6eBWiL0ByoZB6yyotglj5At5QNKt4vOeld8mO3cZCeWMucgZDZD";
    //        string FeedRequestUrl = string.Concat("https://graph.facebook.com/v5.0/" + ReviewID + "/comments?message=" + Comment + "&access_token=" + accessToken);

    //        HttpWebRequest feedRequest = (HttpWebRequest)WebRequest.Create(FeedRequestUrl);
    //        feedRequest.Method = "POST";
    //        feedRequest.Accept = "application/json";
    //        feedRequest.ContentType = "application/json; charset=utf-8";
    //        WebResponse feedResponse = (HttpWebResponse)feedRequest.GetResponse();
    //        using (feedResponse)
    //        {
    //            using (var reader = new StreamReader(feedResponse.GetResponseStream()))
    //            {
    //                var result = reader.ReadToEnd();
    //                return result;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return string.Empty;
    //    }
    //}

    //#endregion


    //#region Facebook AccessToken verify & Get Reviews & Get locations
    //public async Task<string> GetFacebookLocations(long SMID, string fbAppID, string fbAppSecret, string accessToken, bool IsEmail)
    //{
    //    try
    //    {
    //        string locations = "";
    //        if (IsValidFacebookAccessToken(SMID, accessToken, IsEmail))
    //        {
    //            locations = GetFacebookPageLocations(accessToken);
    //        }
    //        else
    //        {
    //            accessToken = await FacebookGetRefreshToken(SMID, fbAppID, fbAppSecret, accessToken);
    //            locations = GetFacebookPageLocations(accessToken);
    //        }

    //        return locations;
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    return null;
    //}

    //private string GetFacebookPageLocations(string accessToken)
    //{
    //    try
    //    {
    //        FacebookClient fbClient = new FacebookClient();
    //        Dictionary<string, object> fbParams = new Dictionary<string, object>();
    //        fbParams["access_token"] = accessToken;
    //        JsonObject publishedResponse = fbClient.Get("me/accounts?fields=access_token,name,id,single_line_address&limit=2000", fbParams) as JsonObject;

    //        return publishedResponse.ToString();

    //    }
    //    catch (Exception ex)
    //    {
    //        var result = ex.GetType().Name;
    //        return ex.GetType().Name;
    //    }
    //}

    ///// Get new access_token
    //public string GetFacebookRefreshTokenCallback(string fbAppID, string fbAppSecret, string token)
    //{
    //    string currentAccessToken = token;
    //    FacebookClient fbClient = new FacebookClient();
    //    Dictionary<string, object> fbParams = new Dictionary<string, object>();
    //    fbParams["client_id"] = fbAppID;
    //    fbParams["grant_type"] = "fb_exchange_token";
    //    fbParams["client_secret"] = fbAppSecret;
    //    fbParams["fb_exchange_token"] = currentAccessToken;
    //    JsonObject publishedResponse = fbClient.Get("/oauth/access_token", fbParams) as JsonObject;
    //    return publishedResponse["access_token"].ToString();
    //}

    ///// Get facebook page reviews from fb api
    //public string FacebookPageReviewsCallBack(string accessToken)
    //{
    //    try
    //    {
    //        string FeedRequestUrl = string.Concat("https://graph.facebook.com/v7.0/me/ratings?fields=open_graph_story{id,message,start_time,data{recommendation_type,review_text},comments},reviewer,recommendation_type,rating,created_time&limit=2000&access_token=" + accessToken);
    //        // string FeedRequestUrl = string.Concat("https://graph.facebook.com/v3.2/me/ratings?fields=open_graph_story{id,message,start_time,data{recommendation_type,review_text},comments},reviewer&limit=2000&access_token=EAALwjEnZB4xQBAA1xwQZAJcd6BFcG1XVhwq370i8ASZA72ZAW4sANgB5lW4AcLVLTiMv4qepZC2Of9cUotw069FzSKE8hVPK90UVgQoDkpwofeTYsYVPGkmYhBvYQF2P50W9lC2CA8pAMj9ibbi3dkiTwy2ZACl0vyicysxjQwP7sLDdK5Ey5p");

    //        HttpWebRequest feedRequest = (HttpWebRequest)WebRequest.Create(FeedRequestUrl);
    //        feedRequest.Method = "GET";
    //        feedRequest.Accept = "application/json";
    //        feedRequest.ContentType = "application/json; charset=utf-8";
    //        WebResponse feedResponse = (HttpWebResponse)feedRequest.GetResponse();
    //        using (feedResponse)
    //        {
    //            using (var reader = new StreamReader(feedResponse.GetResponseStream()))
    //            {
    //                var result = reader.ReadToEnd();
    //                return result;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    return string.Empty;
    //}

    //public bool IsValidFacebookAccessToken(long SMID, string AccessToken, bool IsEmail)
    //{
    //    try
    //    {
    //        string Url = "https://graph.facebook.com/debug_token?";
    //        StringBuilder UrlBuilder = new StringBuilder(Url);
    //        UrlBuilder.Append("input_token=" + AccessToken);
    //        UrlBuilder.Append("&&access_token=" + AccessToken);
    //        HttpWebRequest request = WebRequest.Create(UrlBuilder.ToString()) as HttpWebRequest;
    //        request.Method = "GET";
    //        request.Accept = "application/json";
    //        request.ContentType = "application/json; charset=utf-8";
    //        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
    //        {
    //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
    //            {
    //                var vals = reader.ReadToEnd();
    //                JavaScriptSerializer js = new JavaScriptSerializer();
    //                RootObject ObjRootObject = js.Deserialize<RootObject>(vals);
    //                bool is_valid = ObjRootObject.data.is_valid;
    //                var data_access_expires_at = ObjRootObject.data.data_access_expires_at;
    //                var expireAt = ObjRootObject.data.expires_at;
    //                reader.Close();
    //                response.Close();
    //                //convert in datetime
    //                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
    //                dtDateTime = dtDateTime.AddSeconds(data_access_expires_at).ToLocalTime();

    //                UpdateSocialMediaAccessTokenExpiryDate(SMID, dtDateTime);

    //                var expiry = dtDateTime.AddDays(-2);
    //                var CurrentDatetime = DateTime.Now;

    //                if (CurrentDatetime > expiry)
    //                {
    //                    UpdateRefreshToken(SMID);
    //                    if (IsEmail == false)
    //                    {
    //                        var data = GetExpiredTokenDetail(SMID);
    //                    }
    //                    return false;
    //                }
    //                else
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return false;
    //    }
    //}

    //private bool IsValidFacebookPageAccessToken(string AccessToken)
    //{
    //    try
    //    {
    //        string Url = "https://graph.facebook.com/debug_token?";
    //        StringBuilder UrlBuilder = new StringBuilder(Url);
    //        UrlBuilder.Append("input_token=" + AccessToken);
    //        UrlBuilder.Append("&&access_token=" + AccessToken);
    //        HttpWebRequest request = WebRequest.Create(UrlBuilder.ToString()) as HttpWebRequest;
    //        request.Method = "GET";
    //        request.Accept = "application/json";
    //        request.ContentType = "application/json; charset=utf-8";
    //        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
    //        {
    //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
    //            {
    //                var vals = reader.ReadToEnd();
    //                JavaScriptSerializer js = new JavaScriptSerializer();
    //                RootObject ObjRootObject = js.Deserialize<RootObject>(vals);
    //                bool is_valid = ObjRootObject.data.is_valid;
    //                var data_access_expires_at = ObjRootObject.data.data_access_expires_at;
    //                var expireAt = ObjRootObject.data.expires_at;
    //                reader.Close();
    //                response.Close();
    //                return true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        return false;
    //    }
    //}


    ///// Get new access_token
    //public string FacebookCallback(string token)
    //{
    //    string currentAccessToken = token;
    //    FacebookClient fbClient = new FacebookClient();
    //    Dictionary<string, object> fbParams = new Dictionary<string, object>();
    //    fbParams["client_id"] = fbAPP_ID;
    //    fbParams["grant_type"] = "fb_exchange_token";
    //    fbParams["client_secret"] = fbApp_secret;
    //    fbParams["fb_exchange_token"] = currentAccessToken;
    //    JsonObject publishedResponse = fbClient.Get("/oauth/access_token", fbParams) as JsonObject;
    //    return publishedResponse["access_token"].ToString();
    //}
    //#endregion


    //#region FB Token Detail classes
    //public class RootObject
    //{
    //    public RootData data { get; set; }
    //}
    //public class RootData
    //{
    //    public string app_id { get; set; }
    //    public string type { get; set; }
    //    public string application { get; set; }
    //    public int data_access_expires_at { get; set; }
    //    public int expires_at { get; set; }
    //    public bool is_valid { get; set; }
    //    public int issued_at { get; set; }
    //    public string profile_id { get; set; }
    //    public List<string> scopes { get; set; }
    //    public string user_id { get; set; }
    //}
    //#endregion

    //#region facebook reviews classes
    //public class facebookModal
    //{
    //    public FBRatings ratings { get; set; }
    //}
    //public class ImageData
    //{
    //    public int height { get; set; }
    //    public bool is_silhouette { get; set; }
    //    public string url { get; set; }
    //    public int width { get; set; }
    //}
    //public class Picture
    //{
    //    public ImageData data { get; set; }
    //}
    //public class Reviewer
    //{
    //    public string first_name { get; set; }
    //    public string last_name { get; set; }
    //    public Picture picture { get; set; }
    //    public string id { get; set; }

    //    public string name { get; set; }

    //}
    //public class Seller
    //{
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public string type { get; set; }
    //    public string url { get; set; }
    //}
    //public class Data2
    //{
    //    public string recommendation_type { get; set; }
    //    public string review_text { get; set; }
    //    public Seller seller { get; set; }
    //    public bool has_review_update { get; set; }
    //    public string review_update_time { get; set; }
    //    public rating rating { get; set; }
    //}
    //public class OpenGraphStory
    //{
    //    public string id { get; set; }
    //    public string message { get; set; }
    //    public DateTime start_time { get; set; }
    //    public string type { get; set; }
    //    public Data2 data { get; set; }
    //    public comments comments { get; set; }

    //}
    //public class comments
    //{
    //    public List<FbCommentsData> data { get; set; }
    //    //  public Data Data { get; set; }

    //}
    //public class From
    //{
    //    public string name { get; set; }
    //    public string id { get; set; }
    //}
    //public class FbCommentsData
    //{
    //    public DateTime created_time { get; set; }
    //    public From from { get; set; }
    //    public string message { get; set; }
    //    public string id { get; set; }
    //}
    //public class rating
    //{
    //    public string value { get; set; }
    //    public string scale { get; set; }

    //}
    //public class Datum
    //{
    //    public OpenGraphStory open_graph_story { get; set; }
    //    public Reviewer Reviewer { get; set; }
    //    public string recommendation_type { get; set; }
    //    public DateTime created_time { get; set; }
    //    public int? rating { get; set; }

    //}
    //public class FBRatings
    //{
    //    public List<Datum> data { get; set; }
    //}
    //#endregion

    //#region facebook locations/pages classes
    //public class FbLocations
    //{
    //    public string single_line_address { get; set; }
    //    public FBLocationAddress location { get; set; }
    //    public string access_token { get; set; }
    //    public string name { get; set; }
    //    public string id { get; set; }

    //    public long ReviewCount { get; set; }
    //}

    //public class FacebookLocations
    //{
    //    public List<FbLocations> data { get; set; }
    //}


    //public class FBLocationAddress
    //{
    //    public string city { get; set; }
    //    public string country { get; set; }
    //    public double latitude { get; set; }
    //    public double longitude { get; set; }
    //    public string street { get; set; }
    //    public string zip { get; set; }
    //}
    //#endregion



}
