using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Svam._Class
{
    public class WrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            return BuildApiResponse(request, response);
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            string errorMessage = null;
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;
                if (error != null)
                {
                    content = "";
                    errorMessage = error.Message;
                    errorMessage = string.Concat(errorMessage);
                }
            }
            else
            {
                if (response.StatusCode.Equals(400))
                {
                    errorMessage = "error";
                }
                else
                {
                    errorMessage = "success";
                    if (response.TryGetContentValue(out content))//facebook/user login failed
                    {
                        #region for facebook webhook api response
                        var webhk = content.ToString();
                        if (webhk.Contains("webhook-"))
                        {
                            int dashIndex = 0;
                            string challenge = string.Empty;

                            dashIndex = webhk.IndexOf('-');//get index of dash
                            challenge = dashIndex > 0 ? webhk.Substring(dashIndex + 1) : string.Empty;
                            if (!string.IsNullOrEmpty(challenge))
                            {
                                return request.CreateResponse(response.StatusCode,Convert.ToInt32(challenge));
                            }
                        }
                        
                        #endregion

                        #region for if user invalid and status code 200 then show message fail
                        HttpError error = content as HttpError;
                        string err1 = "** Username and Password does not exits,Please contact to administrator **";
                        string err2 = "** Subscription is expired, Please contact to administrator.!";
                        string err3 = "** Username or e-mail is InActive, Please contact to administrator.!";
                        if (error != null)
                        {
                            if (error.Message == err1 || error.Message == err2 || error.Message == err3)
                            {
                                errorMessage = "fail";
                            }
                        }
                        #endregion
                    }
                  
                }
            }

            var newResponse = request.CreateResponse(response.StatusCode, new ApiResponse(response.StatusCode, content, errorMessage));
            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }
            return newResponse;
        }
      
    }

    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public object Data { get; set; }
        public ApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            Data = result;
            Message = errorMessage;
        }
    }
}