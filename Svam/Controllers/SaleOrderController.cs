using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Svam.EF;
using Svam.Models;
using Svam.Models.DTO;
using Svam.Models.ViewModel;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Traders.Models;

namespace Svam.Controllers
{
    public class SaleOrderController : Controller
    {
        niscrmEntities db = new niscrmEntities();

        public async Task<ActionResult> CreateSaleOrder()
        {
            //try
            //{
            //    int BranchID = Convert.ToInt32(Session["BranchID"]);
            //    int CompanyID = Convert.ToInt32(Session["CompanyID"]);


            //    int UserID = Convert.ToInt32(Session["UID"]);
            //    string pwd = "";
            //    if (Convert.ToString(Session["UserType"]) == "SuperAdmin")
            //    {
            //        if (Session["T_LoginId"] == null)
            //        {
            //            var data = await db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefaultAsync();

            //            if (data.t_LoginId == null)//check if t_login id is null then save it to table
            //            {
            //                int Id = 0;
            //                string query = @"select id,user_password from t_login where company_id=" + CompanyID + " and branch='" + BranchID.ToString() + "' and userrole='3' order by id asc limit 1";
            //                Id = await db.Database.SqlQuery<int>(query).FirstOrDefaultAsync();
            //                data.t_LoginId = Id;
            //                await db.SaveChangesAsync();
            //                Session["T_LoginId"] = Id;
            //                var t_qury1 = await db.t_login.Where(em => em.id == Id).FirstOrDefaultAsync();//used for service
            //                if (!string.IsNullOrEmpty(t_qury1.KeyVersion))
            //                {
            //                    #region password decryption
            //                    byte[] iv1;
            //                    byte[] key = EncriptAES.getdcriptkey(out iv1);
            //                    string decryptPwd = EncriptAES.DecryptString(t_qury1.user_password, key, iv1);
            //                    Session["T_pwd"] = decryptPwd;
            //                    #endregion
            //                }
            //                Session["trole_name"] = t_qury1.role_name;
            //                Session["tId"] = t_qury1.id;
            //                Session["tUser_id"] = t_qury1.user_id;
            //                Session["trole_id"] = t_qury1.role_id;
            //                Session["temp_code"] = t_qury1.Emp_code;
            //                Session["tcompany_id"] = t_qury1.company_id;
            //                Session["tbranch_id"] = t_qury1.branch;
            //                Session["tcustomerid"] = t_qury1.customer_id;
            //                Session["t_crm_token"] = t_qury1.crm_token;
            //                //Session["t_crm_token_cr_time"] = t_qury1.crm_token_cr_time;

            //            }
            //            else
            //            {
            //                Session["T_LoginId"] = data.t_LoginId;
            //                int t_loginid = Convert.ToInt32(Session["T_LoginId"]);
            //                var t_qury = await db.t_login.Where(em => em.id == t_loginid).FirstOrDefaultAsync();//used for service
            //                if (!string.IsNullOrEmpty(t_qury.KeyVersion))
            //                {
            //                    #region password decryption

            //                    byte[] iv2;
            //                    byte[] key1 = EncriptAES.getdcriptkey(out iv2);
            //                    string decryptPwd = EncriptAES.DecryptString(t_qury.user_password, key1, iv2);
            //                    Session["T_pwd"] = decryptPwd;
            //                    TempData["T_pwd"] = decryptPwd;
            //                    #endregion

            //                }
            //                else
            //                {
            //                    Session["T_pwd"] = t_qury.user_password;
            //                }

            //                string id = t_qury.user_id;
            //                string pwd1 =Convert.ToString(Session["T_pwd"]);
            //                Session["trole_name"] = t_qury.role_name;
            //                Session["tId"] = t_qury.id;
            //                Session["tUser_id"] = t_qury.user_id;
            //                TempData["tUser_id"] = t_qury.user_id;
            //                Session["trole_id"] = t_qury.role_id;
            //                Session["temp_code"] = t_qury.Emp_code;
            //                Session["tcompany_id"] = t_qury.company_id;
            //                Session["tbranch_id"] = t_qury.branch;
            //                Session["tcustomerid"] = t_qury.customer_id;
            //                Session["tloginfrmwhr"] = "weblogin";
            //                Session["t_crm_token"] = t_qury.crm_token;
            //                //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

            //                var request = (HttpWebRequest)WebRequest.Create("https://viren.nicoleinfosoftdemo.com/login/crm");

            //                var postData = "login-username=" + Uri.EscapeDataString(id);
            //                postData += "&login-password=" + Uri.EscapeDataString(pwd1);
            //                var data1 = Encoding.ASCII.GetBytes(postData);

            //                request.Method = "POST";
            //                request.ContentType = "application/x-www-form-urlencoded";
            //                request.ContentLength = data1.Length;

            //                using (var stream = request.GetRequestStream())
            //                {
            //                    stream.Write(data1, 0, data1.Length);
            //                }

            //                var response = (HttpWebResponse)request.GetResponse();

            //                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //                string jsonString = responseString;
            //                dynamic myObject = JValue.Parse(jsonString);
            //                var crmtoken = "";
            //                foreach (dynamic questions in myObject)
            //                {
            //                    if(questions.Name== "crm_token")
            //                        crmtoken =((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
            //                }

            //                var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

            //                Session["t_crm_token"] = crmtoken;
            //                //Session["t_crm_token_cr_time"] = tlogin.crm_token_cr_time;

            //                //return responseString;

            //            }
            //        }
            //        else
            //        {
            //            int Id1 = 0;
            //            var data12 = await db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefaultAsync();
            //            Id1 = Convert.ToInt32(data12.t_LoginId);
            //            var t_qury1 = await db.t_login.Where(em => em.id == Id1).FirstOrDefaultAsync();//used for service
            //            if (!string.IsNullOrEmpty(t_qury1.KeyVersion))
            //            {
            //                #region password decryption
            //                byte[] iv1;
            //                byte[] key = EncriptAES.getdcriptkey(out iv1);
            //                string decryptPwd = EncriptAES.DecryptString(t_qury1.user_password, key, iv1);
            //                Session["T_pwd"] = decryptPwd;
            //                #endregion
            //            }

            //            string id = t_qury1.user_id;
            //            string pwd1 = Convert.ToString(Session["T_pwd"]);
            //            Session["trole_name"] = t_qury1.role_name;
            //            Session["tId"] = t_qury1.id;
            //            Session["tUser_id"] = t_qury1.user_id;
            //            Session["trole_id"] = t_qury1.role_id;
            //            Session["temp_code"] = t_qury1.Emp_code;
            //            Session["tcompany_id"] = t_qury1.company_id;
            //            Session["tbranch_id"] = t_qury1.branch;
            //            Session["tcustomerid"] = t_qury1.customer_id;
            //            Session["t_crm_token"] = t_qury1.crm_token;
            //            //Session["t_crm_token_cr_time"] = t_qury1.crm_token_cr_time;


            //            var request = (HttpWebRequest)WebRequest.Create("https://viren.nicoleinfosoftdemo.com/login/crm");

            //            var postData = "login-username=" + Uri.EscapeDataString(id);
            //            postData += "&login-password=" + Uri.EscapeDataString(pwd1);
            //            var data1 = Encoding.ASCII.GetBytes(postData);

            //            request.Method = "POST";
            //            request.ContentType = "application/x-www-form-urlencoded";
            //            request.ContentLength = data1.Length;

            //            using (var stream = request.GetRequestStream())
            //            {
            //                stream.Write(data1, 0, data1.Length);
            //            }

            //            var response = (HttpWebResponse)request.GetResponse();

            //            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //            string jsonString = responseString;
            //            dynamic myObject = JValue.Parse(jsonString);
            //            var crmtoken = "";
            //            foreach (dynamic questions in myObject)
            //            {
            //                if (questions.Name == "crm_token")
            //                    crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
            //            }

            //            var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

            //            Session["t_crm_token"] = crmtoken;
            //            //Session["t_crm_token_cr_time"] = tlogin.crm_token_cr_time;
            //        }

            //    }
            //    else
            //    {
            //        if (Session["T_LoginId"] == null)
            //        {
            //            var data = await db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefaultAsync();

            //            if (data.t_LoginId == null)//check if t_login id is null then save it to table
            //            {
            //                var dt = Constant.GetBharatTime();
            //                string uniqUserId = data.Fname + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

            //                #region Password encryption
            //                string VersionKey = "";

            //                VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
            //                                                                                                           //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
            //                byte[] iv1;
            //                byte[] key = EncriptAES.getdcriptkey(out iv1);
            //                string ecncryptPwd = EncriptAES.EncryptString(uniqUserId, key, iv1);
            //                #endregion

            //                #region get syncid from t_login
            //                string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
            //                int syncid = await db.Database.SqlQuery<int>(query).FirstOrDefaultAsync();
            //                string synId = "O" + (syncid + 1);
            //                #endregion
            //                var tl = new t_login
            //                {
            //                    user_id = uniqUserId,
            //                    user_password = ecncryptPwd,//save encrypted password
            //                    role_id = 7,
            //                    role_name = "CRM",
            //                    UserRole = 7,
            //                    Emp_code = "CRM",
            //                    branch = BranchID.ToString(),
            //                    company_id = CompanyID,
            //                    Subscription_StartDate = data.StartDate,
            //                    Subscription_EndDate = data.EndDate,
            //                    PaymentStatus = "no",
            //                    CreatedDate = dt,
            //                    IsActive = "yes",
            //                    flag = "N",
            //                    SyncID = synId,
            //                    mastercompany = 0,
            //                    KeyVersion = VersionKey//save latest key version
            //                };
            //                db.t_login.Add(tl);
            //                await db.SaveChangesAsync();
            //                Session["T_LoginId"] = tl.id;
            //                if (!string.IsNullOrEmpty(tl.KeyVersion))// used for services
            //                {
            //                    #region password decryption
            //                    byte[] iv2;
            //                    byte[] key1 = EncriptAES.getdcriptkey(out iv2);
            //                    string decryptPwd = EncriptAES.DecryptString(tl.user_password, key, iv1);
            //                    Session["T_pwd"] = decryptPwd;
            //                    #endregion

            //                }
            //            }
            //            else
            //            {
            //                Session["T_LoginId"] = data.t_LoginId;
            //                int t_loginid = Convert.ToInt32(Session["T_LoginId"]);
            //                var t_qury = await db.t_login.Where(em => em.id == t_loginid).FirstOrDefaultAsync();//used for service
            //                if (!string.IsNullOrEmpty(t_qury.KeyVersion))
            //                {
            //                    #region password decryption
            //                    byte[] iv1;
            //                    byte[] key = EncriptAES.getdcriptkey(out iv1);
            //                    string decryptPwd = EncriptAES.DecryptString(t_qury.user_password, key, iv1);
            //                    Session["T_pwd"] = decryptPwd;
            //                    #endregion

            //                }
            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendExcepToDB(ex);
            //    TempData["alert"] = "Something went wrong!";
            //}
            //return View();
            CRMQutationsaleModel QSM = new CRMQutationsaleModel();
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string emailid = Convert.ToString(Session["UserEmail"]);
            var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
            if (comptype.Count > 0)
            {
                string Orglist = @"SELECT ID, Organization FROM company_profile WHERE ID='" + CompanyID + "'";
                var getOrg = db.Database.SqlQuery<CRMQutationsaleModel>(Orglist).OrderBy(a => a.Organization).ToList();

                string Orglist2 = @"SELECT ID, Organization FROM company_profile WHERE mastercompany='" + CompanyID + "'";
                var getOrg2 = db.Database.SqlQuery<CRMQutationsaleModel>(Orglist2).OrderBy(a => a.Organization).ToList();

                var newlist = getOrg.Concat(getOrg2);
                QSM.joinlist = getOrg.Concat(getOrg2).ToList();
            }
            else
            {
                int UserID = Convert.ToInt32(Session["UID"]);
                string pwd = "";
                if (Session["T_LoginId"] == null)
                {
                    var data = db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefault();
                    Session["T_LoginId"] = data.t_LoginId;
                    int t_loginid = Convert.ToInt32(Session["T_LoginId"]);
                    var t_qury = db.t_login.Where(em => em.id == t_loginid).FirstOrDefault();//used for service
                    if (t_qury != null)
                    {
                        if (!string.IsNullOrEmpty(t_qury.KeyVersion))
                        {
                            #region password decryption

                            byte[] iv2;
                            byte[] key1 = EncriptAES.getdcriptkey(out iv2);
                            string decryptPwd = EncriptAES.DecryptString(t_qury.user_password, key1, iv2);
                            Session["T_pwd"] = decryptPwd;
                            TempData["T_pwd"] = decryptPwd;
                            #endregion

                        }
                        else
                        {
                            Session["T_pwd"] = t_qury.user_password;
                        }

                        string id = t_qury.user_id;
                        string pwd1 = Convert.ToString(Session["T_pwd"]);
                        Session["trole_name"] = t_qury.role_name;
                        Session["tId"] = t_qury.id;
                        Session["tUser_id"] = t_qury.user_id;
                        TempData["tUser_id"] = t_qury.user_id;
                        Session["trole_id"] = t_qury.role_id;
                        Session["temp_code"] = t_qury.Emp_code;
                        Session["tcompany_id"] = t_qury.company_id;
                        Session["tbranch_id"] = t_qury.branch;
                        Session["tcustomerid"] = t_qury.customer_id;
                        Session["tloginfrmwhr"] = "weblogin";
                        Session["t_crm_token"] = t_qury.crm_token;
                        //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

                        var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                        var postData = "login-username=" + Uri.EscapeDataString(id);
                        postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                        var data1 = Encoding.ASCII.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data1.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data1, 0, data1.Length);
                        }

                        var response = (HttpWebResponse)request.GetResponse();

                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        string jsonString = responseString;
                        dynamic myObject = JValue.Parse(jsonString);
                        var crmtoken = "";
                        foreach (dynamic questions in myObject)
                        {
                            if (questions.Name == "crm_token")
                                crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                        }

                        var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

                        Session["t_crm_token"] = crmtoken;

                        return Redirect("/SaleOrder/CreateSaleOrder1");
                    }
                    else
                    {
                        var data1 = db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefault();
                        if (data1.t_LoginId == null)//check if t_login id is null then save it to table
                        {
                            var dt = Constant.GetBharatTime();
                            string uniqUserId = data1.Fname + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                            #region Password encryption
                            string VersionKey = "";

                            VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                                                                                                       //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string ecncryptPwd = EncriptAES.EncryptString(uniqUserId, key, iv1);
                            #endregion

                            #region get syncid from t_login
                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                            string synId = "O" + (syncid + 1);
                            #endregion
                            //var tl = new t_login
                            //{
                            t_login tl = new t_login();
                            tl.user_id = uniqUserId;
                            tl.user_password = ecncryptPwd;//save encrypted password
                            tl.role_id = 7;
                            tl.role_name = "Admin";
                            tl.UserRole = 7;
                            tl.Emp_code = "CRM";
                            tl.branch = BranchID.ToString();
                            tl.company_id = CompanyID;
                            tl.Subscription_StartDate = data.StartDate;
                            tl.Subscription_EndDate = data.EndDate;
                            tl.PaymentStatus = "no";
                            tl.CreatedDate = dt;
                            tl.IsActive = "yes";
                            tl.flag = "N";
                            tl.SyncID = synId;
                            tl.mastercompany = 0;
                            //tl.user_type = "crm_user";
                            tl.customer_id = "";
                            tl.permissions = "";
                            tl.KeyVersion = VersionKey;//save latest key version
                                                       //};
                            db.t_login.Add(tl);
                            db.SaveChanges();
                            string t_usereturnValue = tl.user_id;
                            var get_tlogindata = db.t_login.Where(em => em.company_id == CompanyID && em.user_id == t_usereturnValue).FirstOrDefault();

                            if (get_tlogindata != null)
                            {
                                var add_tloginid = db.crm_usertbl.Where(a => a.Email == emailid && a.CompanyID == CompanyID).FirstOrDefault();
                                if (add_tloginid != null)
                                {
                                    add_tloginid.t_LoginId = get_tlogindata.id;
                                    db.SaveChanges();
                                }
                            }

                            Session["T_LoginId"] = get_tlogindata.id;
                            if (!string.IsNullOrEmpty(get_tlogindata.KeyVersion))// used for services
                            {
                                #region password decryption
                                byte[] iv2;
                                byte[] key1 = EncriptAES.getdcriptkey(out iv2);
                                string decryptPwd = EncriptAES.DecryptString(get_tlogindata.user_password, key, iv1);
                                Session["T_pwd"] = decryptPwd;
                                #endregion

                            }
                            string id = get_tlogindata.user_id;
                            string pwd1 = Convert.ToString(Session["T_pwd"]);
                            Session["trole_name"] = get_tlogindata.role_name;
                            Session["tId"] = get_tlogindata.id;
                            Session["tUser_id"] = get_tlogindata.user_id;
                            TempData["tUser_id"] = get_tlogindata.user_id;
                            Session["trole_id"] = get_tlogindata.role_id;
                            Session["temp_code"] = get_tlogindata.Emp_code;
                            Session["tcompany_id"] = get_tlogindata.company_id;
                            Session["tbranch_id"] = get_tlogindata.branch;
                            Session["tcustomerid"] = get_tlogindata.customer_id;
                            Session["tloginfrmwhr"] = "weblogin";
                            Session["t_crm_token"] = get_tlogindata.crm_token;
                            //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

                            var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                            var postData = "login-username=" + Uri.EscapeDataString(id);
                            postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                            var data121 = Encoding.ASCII.GetBytes(postData);

                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.ContentLength = data121.Length;

                            using (var stream = request.GetRequestStream())
                            {
                                stream.Write(data121, 0, data121.Length);
                            }

                            var response = (HttpWebResponse)request.GetResponse();

                            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                            string jsonString = responseString;
                            dynamic myObject = JValue.Parse(jsonString);
                            var crmtoken = "";
                            foreach (dynamic questions in myObject)
                            {
                                if (questions.Name == "crm_token")
                                    crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                            }

                            //var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

                            Session["t_crm_token"] = crmtoken;
                            return Redirect("/Quotation/CreateQuotation1");


                        }
                    }

                }
                else
                {
                    int t_loginid = Convert.ToInt32(Session["T_LoginId"]);
                    var t_qury = db.t_login.Where(em => em.id == t_loginid).FirstOrDefault();//used for service
                    if (!string.IsNullOrEmpty(t_qury.KeyVersion))
                    {
                        #region password decryption

                        byte[] iv2;
                        byte[] key1 = EncriptAES.getdcriptkey(out iv2);
                        string decryptPwd = EncriptAES.DecryptString(t_qury.user_password, key1, iv2);
                        Session["T_pwd"] = decryptPwd;
                        TempData["T_pwd"] = decryptPwd;
                        #endregion

                    }
                    else
                    {
                        Session["T_pwd"] = t_qury.user_password;
                    }

                    string id = t_qury.user_id;
                    string pwd1 = Convert.ToString(Session["T_pwd"]);
                    Session["trole_name"] = t_qury.role_name;
                    Session["tId"] = t_qury.id;
                    Session["tUser_id"] = t_qury.user_id;
                    TempData["tUser_id"] = t_qury.user_id;
                    Session["trole_id"] = t_qury.role_id;
                    Session["temp_code"] = t_qury.Emp_code;
                    Session["tcompany_id"] = t_qury.company_id;
                    Session["tbranch_id"] = t_qury.branch;
                    Session["tcustomerid"] = t_qury.customer_id;
                    Session["tloginfrmwhr"] = "weblogin";
                    Session["t_crm_token"] = t_qury.crm_token;
                    //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

                    var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                    var postData = "login-username=" + Uri.EscapeDataString(id);
                    postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                    var data1 = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data1.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data1, 0, data1.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    string jsonString = responseString;
                    dynamic myObject = JValue.Parse(jsonString);
                    var crmtoken = "";
                    foreach (dynamic questions in myObject)
                    {
                        if (questions.Name == "crm_token")
                            crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                    }

                    var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

                    Session["t_crm_token"] = crmtoken;

                    return Redirect("/SaleOrder/CreateSaleOrder1");
                }
            }
            return PartialView("_SaleOrderDropDownModule", QSM);
        }
        public ActionResult CreateSaleOrder1()
        {
            return View();
        }
        public async Task<ActionResult> GetCompanydetail(string id, string CG)
        {
            int ID = Convert.ToInt32(id);
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            int UserID = Convert.ToInt32(Session["UID"]);
            string emailid = Convert.ToString(Session["UserEmail"]);
            var data = await db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefaultAsync();
            var crmtoken = "";
            if (CompanyID == 2682)
            {
                string crm_username = data.UserName;
                string Customergropid = data.Customergroupid;

                var dt1 = Constant.GetBharatTime();
                string uniqUserId1 = data.UserName + "" + Guid.NewGuid().ToString("n").Substring(0, 5);

                string username = uniqUserId1.Substring(0, 5);
                var t_qury1 = await db.t_login.Where(em => em.company_id == ID && em.role_name == "Admin" && em.user_id.Substring(0, 5) == uniqUserId1.Substring(0, 5)).FirstOrDefaultAsync();//used for service
                if (t_qury1 != null)
                {
                    if (!string.IsNullOrEmpty(t_qury1.KeyVersion))
                    {
                        #region password decryption
                        byte[] iv1;
                        byte[] key = EncriptAES.getdcriptkey(out iv1);
                        string decryptPwd = EncriptAES.DecryptString(t_qury1.user_password, key, iv1);
                        Session["T_pwd"] = decryptPwd;
                        #endregion
                    }
                    else
                    {
                        Session["T_pwd"] = t_qury1.user_password;
                    }
                    Session["trole_name"] = t_qury1.role_name;
                    Session["tId"] = t_qury1.id;
                    Session["tUser_id"] = t_qury1.user_id;
                    Session["trole_id"] = t_qury1.role_id;
                    Session["temp_code"] = t_qury1.Emp_code;
                    Session["tcompany_id"] = t_qury1.company_id;
                    Session["tbranch_id"] = t_qury1.branch;
                    Session["tcustomerid"] = t_qury1.customer_id;
                    Session["t_crm_token"] = t_qury1.crm_token;

                    string id1 = t_qury1.user_id;
                    string pwd1 = Convert.ToString(Session["T_pwd"]);
                    var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                    var postData = "login-username=" + Uri.EscapeDataString(id1);
                    postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                    var data1 = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data1.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data1, 0, data1.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    string jsonString = responseString;
                    dynamic myObject = JValue.Parse(jsonString);
                    foreach (dynamic questions in myObject)
                    {
                        if (questions.Name == "crm_token")
                            crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                    }
                    Session["t_crm_token"] = crmtoken;
                    //t_qury1.customer_groups = CG;
                    //t_qury1.customer_groups = data.Customergroupid;
                    //t_qury1.user_type = "crm_user";
                    //db.t_login.Add(t_qury1);
                    await db.SaveChangesAsync();

                }
                else
                {
                    TempData["alert"] = "Something went wrong!";
                    return Redirect("/SaleOrder/CreateSaleOrder");
                }
            }
            else if (CompanyID == 2603)
            {
                string crm_username = data.UserName;
                string Customergropid = data.Customergroupid;
                var t_qury1 = await db.t_login.Where(em => em.company_id == ID && em.role_name == "Admin").FirstOrDefaultAsync();//used for service
                if (t_qury1 != null)
                {
                    if (!string.IsNullOrEmpty(t_qury1.KeyVersion))
                    {
                        #region password decryption
                        byte[] iv1;
                        byte[] key = EncriptAES.getdcriptkey(out iv1);
                        string decryptPwd = EncriptAES.DecryptString(t_qury1.user_password, key, iv1);
                        Session["T_pwd"] = decryptPwd;
                        #endregion
                    }
                    else
                    {
                        Session["T_pwd"] = t_qury1.user_password;
                    }
                    Session["trole_name"] = t_qury1.role_name;
                    Session["tId"] = t_qury1.id;
                    Session["tUser_id"] = t_qury1.user_id;
                    Session["trole_id"] = t_qury1.role_id;
                    Session["temp_code"] = t_qury1.Emp_code;
                    Session["tcompany_id"] = t_qury1.company_id;
                    Session["tbranch_id"] = t_qury1.branch;
                    Session["tcustomerid"] = t_qury1.customer_id;
                    Session["t_crm_token"] = t_qury1.crm_token;

                    string id1 = t_qury1.user_id;
                    string pwd1 = Convert.ToString(Session["T_pwd"]);
                    var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                    var postData = "login-username=" + Uri.EscapeDataString(id1);
                    postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                    var data1 = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data1.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data1, 0, data1.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    string jsonString = responseString;
                    dynamic myObject = JValue.Parse(jsonString);
                    foreach (dynamic questions in myObject)
                    {
                        if (questions.Name == "crm_token")
                            crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                    }
                    Session["t_crm_token"] = crmtoken;
                    //t_qury1.customer_groups = Customergropid;
                    //t_qury1.user_type = "crm_user";
                    await db.SaveChangesAsync();
                }
            }

            else if (CompanyID == 1153)
            {
                string crm_username = data.UserName;
                string Customergropid = data.Customergroupid;
                var t_qury1 = await db.t_login.Where(em => em.company_id == ID && em.role_name == "Admin").FirstOrDefaultAsync();//used for service
                if (t_qury1 != null)
                {
                    if (!string.IsNullOrEmpty(t_qury1.KeyVersion))
                    {
                        #region password decryption
                        byte[] iv1;
                        byte[] key = EncriptAES.getdcriptkey(out iv1);
                        string decryptPwd = EncriptAES.DecryptString(t_qury1.user_password, key, iv1);
                        Session["T_pwd"] = decryptPwd;
                        #endregion
                    }
                    else
                    {
                        Session["T_pwd"] = t_qury1.user_password;
                    }
                    Session["trole_name"] = t_qury1.role_name;
                    Session["tId"] = t_qury1.id;
                    Session["tUser_id"] = t_qury1.user_id;
                    Session["trole_id"] = t_qury1.role_id;
                    Session["temp_code"] = t_qury1.Emp_code;
                    Session["tcompany_id"] = t_qury1.company_id;
                    Session["tbranch_id"] = t_qury1.branch;
                    Session["tcustomerid"] = t_qury1.customer_id;
                    Session["t_crm_token"] = t_qury1.crm_token;

                    string id1 = t_qury1.user_id;
                    string pwd1 = Convert.ToString(Session["T_pwd"]);
                    var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                    var postData = "login-username=" + Uri.EscapeDataString(id1);
                    postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                    var data1 = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data1.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data1, 0, data1.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    string jsonString = responseString;
                    dynamic myObject = JValue.Parse(jsonString);
                    foreach (dynamic questions in myObject)
                    {
                        if (questions.Name == "crm_token")
                            crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                    }
                    Session["t_crm_token"] = crmtoken;
                    //t_qury1.customer_groups = data.Customergroupid;
                    //t_qury1.user_type = "crm_user";
                    await db.SaveChangesAsync();
                }
            }

            else if (CompanyID == 2644)
            {
                string crm_username = data.UserName;
                string Customergropid = data.Customergroupid;
                var t_qury1 = await db.t_login.Where(em => em.company_id == ID && em.role_name == "Admin").FirstOrDefaultAsync();//used for service
                if (t_qury1 != null)
                {
                    if (!string.IsNullOrEmpty(t_qury1.KeyVersion))
                    {
                        #region password decryption
                        byte[] iv1;
                        byte[] key = EncriptAES.getdcriptkey(out iv1);
                        string decryptPwd = EncriptAES.DecryptString(t_qury1.user_password, key, iv1);
                        Session["T_pwd"] = decryptPwd;
                        #endregion
                    }
                    else
                    {
                        Session["T_pwd"] = t_qury1.user_password;
                    }
                    Session["trole_name"] = t_qury1.role_name;
                    Session["tId"] = t_qury1.id;
                    Session["tUser_id"] = t_qury1.user_id;
                    Session["trole_id"] = t_qury1.role_id;
                    Session["temp_code"] = t_qury1.Emp_code;
                    Session["tcompany_id"] = t_qury1.company_id;
                    Session["tbranch_id"] = t_qury1.branch;
                    Session["tcustomerid"] = t_qury1.customer_id;
                    Session["t_crm_token"] = t_qury1.crm_token;

                    string id1 = t_qury1.user_id;
                    string pwd1 = Convert.ToString(Session["T_pwd"]);
                    var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                    var postData = "login-username=" + Uri.EscapeDataString(id1);
                    postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                    var data1 = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data1.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data1, 0, data1.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    string jsonString = responseString;
                    dynamic myObject = JValue.Parse(jsonString);
                    foreach (dynamic questions in myObject)
                    {
                        if (questions.Name == "crm_token")
                            crmtoken = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                    }
                    Session["t_crm_token"] = crmtoken;
                    //t_qury1.customer_groups = Customergropid;
                    //t_qury1.user_type = "crm_user";
                    await db.SaveChangesAsync();
                }
            }

            else
            {
                if (Session["T_LoginId"] == null)
                {
                    var data12 = db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefault();
                    Session["T_LoginId"] = data12.t_LoginId;
                    int t_loginid = Convert.ToInt32(Session["T_LoginId"]);
                    var t_qury = db.t_login.Where(em => em.id == t_loginid).FirstOrDefault();//used for service
                    if (t_qury != null)
                    {
                        if (!string.IsNullOrEmpty(t_qury.KeyVersion))
                        {
                            #region password decryption

                            byte[] iv2;
                            byte[] key1 = EncriptAES.getdcriptkey(out iv2);
                            string decryptPwd = EncriptAES.DecryptString(t_qury.user_password, key1, iv2);
                            Session["T_pwd"] = decryptPwd;
                            TempData["T_pwd"] = decryptPwd;
                            #endregion

                        }
                        else
                        {
                            Session["T_pwd"] = t_qury.user_password;
                        }

                        string id12 = t_qury.user_id;
                        string pwd1 = Convert.ToString(Session["T_pwd"]);
                        Session["trole_name"] = t_qury.role_name;
                        Session["tId"] = t_qury.id;
                        Session["tUser_id"] = t_qury.user_id;
                        TempData["tUser_id"] = t_qury.user_id;
                        Session["trole_id"] = t_qury.role_id;
                        Session["temp_code"] = t_qury.Emp_code;
                        Session["tcompany_id"] = t_qury.company_id;
                        Session["tbranch_id"] = t_qury.branch;
                        Session["tcustomerid"] = t_qury.customer_id;
                        Session["tloginfrmwhr"] = "weblogin";
                        Session["t_crm_token"] = t_qury.crm_token;
                        //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

                        var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                        var postData = "login-username=" + Uri.EscapeDataString(id12);
                        postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                        var data1 = Encoding.ASCII.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data1.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data1, 0, data1.Length);
                        }

                        var response = (HttpWebResponse)request.GetResponse();

                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        string jsonString = responseString;
                        dynamic myObject = JValue.Parse(jsonString);
                        var crmtoken12 = "";
                        foreach (dynamic questions in myObject)
                        {
                            if (questions.Name == "crm_token")
                                crmtoken12 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                        }

                        var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

                        Session["t_crm_token"] = crmtoken12;
                        return Redirect("/SaleOrder/CreateSaleOrder1");
                    }
                    else
                    {
                        var data1 = db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefault();
                        if (data1.t_LoginId == null)//check if t_login id is null then save it to table
                        {
                            var dt = Constant.GetBharatTime();
                            string uniqUserId = data1.Fname + "" + Guid.NewGuid().ToString("n").Substring(0, 5);//get user uniq use id

                            #region Password encryption
                            string VersionKey = "";

                            VersionKey = db.masterkeyversions.OrderByDescending(a => a.Id).FirstOrDefault().keyversion;//get latest key version
                                                                                                                       //string UserPassword = Guid.NewGuid().ToString("n").Substring(0, 5);
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string ecncryptPwd = EncriptAES.EncryptString(uniqUserId, key, iv1);
                            #endregion

                            #region get syncid from t_login
                            string query = @"SELECT MAX(CAST(SUBSTR(TRIM(SyncID),2) AS UNSIGNED))as SyncID FROM t_login";
                            int syncid = db.Database.SqlQuery<int>(query).FirstOrDefault();
                            string synId = "O" + (syncid + 1);
                            #endregion
                            //var tl = new t_login
                            //{
                            t_login tl = new t_login();
                            tl.user_id = uniqUserId;
                            tl.user_password = ecncryptPwd;//save encrypted password
                            tl.role_id = 7;
                            tl.role_name = "Admin";
                            tl.UserRole = 7;
                            tl.Emp_code = "CRM";
                            tl.branch = BranchID.ToString();
                            tl.company_id = CompanyID;
                            tl.Subscription_StartDate = data.StartDate;
                            tl.Subscription_EndDate = data.EndDate;
                            tl.PaymentStatus = "no";
                            tl.CreatedDate = dt;
                            tl.IsActive = "yes";
                            tl.flag = "N";
                            tl.SyncID = synId;
                            tl.mastercompany = 0;
                            //tl.user_type = "crm_user";
                            tl.customer_id = "";
                            tl.permissions = "";
                            tl.KeyVersion = VersionKey;//save latest key version
                                                       //};
                            db.t_login.Add(tl);
                            db.SaveChanges();
                            string t_usereturnValue = tl.user_id;
                            var get_tlogindata = db.t_login.Where(em => em.company_id == CompanyID && em.user_id == t_usereturnValue).FirstOrDefault();

                            if (get_tlogindata != null)
                            {
                                var add_tloginid = db.crm_usertbl.Where(a => a.Email == emailid && a.CompanyID == CompanyID).FirstOrDefault();
                                if (add_tloginid != null)
                                {
                                    add_tloginid.t_LoginId = get_tlogindata.id;
                                    db.SaveChanges();
                                }
                            }

                            Session["T_LoginId"] = get_tlogindata.id;
                            if (!string.IsNullOrEmpty(get_tlogindata.KeyVersion))// used for services
                            {
                                #region password decryption
                                byte[] iv2;
                                byte[] key1 = EncriptAES.getdcriptkey(out iv2);
                                string decryptPwd = EncriptAES.DecryptString(get_tlogindata.user_password, key, iv1);
                                Session["T_pwd"] = decryptPwd;
                                #endregion

                            }
                            string id123 = get_tlogindata.user_id;
                            string pwd1 = Convert.ToString(Session["T_pwd"]);
                            Session["trole_name"] = get_tlogindata.role_name;
                            Session["tId"] = get_tlogindata.id;
                            Session["tUser_id"] = get_tlogindata.user_id;
                            TempData["tUser_id"] = get_tlogindata.user_id;
                            Session["trole_id"] = get_tlogindata.role_id;
                            Session["temp_code"] = get_tlogindata.Emp_code;
                            Session["tcompany_id"] = get_tlogindata.company_id;
                            Session["tbranch_id"] = get_tlogindata.branch;
                            Session["tcustomerid"] = get_tlogindata.customer_id;
                            Session["tloginfrmwhr"] = "weblogin";
                            Session["t_crm_token"] = get_tlogindata.crm_token;
                            //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

                            var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                            var postData = "login-username=" + Uri.EscapeDataString(id123);
                            postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                            var data121 = Encoding.ASCII.GetBytes(postData);

                            request.Method = "POST";
                            request.ContentType = "application/x-www-form-urlencoded";
                            request.ContentLength = data121.Length;

                            using (var stream = request.GetRequestStream())
                            {
                                stream.Write(data121, 0, data121.Length);
                            }

                            var response = (HttpWebResponse)request.GetResponse();

                            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                            string jsonString = responseString;
                            dynamic myObject = JValue.Parse(jsonString);
                            var crmtoken123 = "";
                            foreach (dynamic questions in myObject)
                            {
                                if (questions.Name == "crm_token")
                                    crmtoken123 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                            }

                            //var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

                            Session["t_crm_token"] = crmtoken123;
                            return Redirect("/SaleOrder/CreateSaleOrder1");


                        }
                    }

                }
                else
                {
                    int t_loginid = Convert.ToInt32(Session["T_LoginId"]);
                    var t_qury = db.t_login.Where(em => em.id == t_loginid).FirstOrDefault();//used for service
                    if (t_qury != null)
                    {
                        if (!string.IsNullOrEmpty(t_qury.KeyVersion))
                        {
                            #region password decryption

                            byte[] iv2;
                            byte[] key1 = EncriptAES.getdcriptkey(out iv2);
                            string decryptPwd = EncriptAES.DecryptString(t_qury.user_password, key1, iv2);
                            Session["T_pwd"] = decryptPwd;
                            TempData["T_pwd"] = decryptPwd;
                            #endregion

                        }
                        else
                        {
                            Session["T_pwd"] = t_qury.user_password;
                        }

                        string id12 = t_qury.user_id;
                        string pwd1 = Convert.ToString(Session["T_pwd"]);
                        Session["trole_name"] = t_qury.role_name;
                        Session["tId"] = t_qury.id;
                        Session["tUser_id"] = t_qury.user_id;
                        TempData["tUser_id"] = t_qury.user_id;
                        Session["trole_id"] = t_qury.role_id;
                        Session["temp_code"] = t_qury.Emp_code;
                        Session["tcompany_id"] = t_qury.company_id;
                        Session["tbranch_id"] = t_qury.branch;
                        Session["tcustomerid"] = t_qury.customer_id;
                        Session["tloginfrmwhr"] = "weblogin";
                        Session["t_crm_token"] = t_qury.crm_token;
                        //Session["t_crm_token_cr_time"] = t_qury.crm_token_cr_time;

                        var request = (HttpWebRequest)WebRequest.Create("https://www.smartcapita.com/login/crm");

                        var postData = "login-username=" + Uri.EscapeDataString(id12);
                        postData += "&login-password=" + Uri.EscapeDataString(pwd1);
                        var data1 = Encoding.ASCII.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data1.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data1, 0, data1.Length);
                        }

                        var response = (HttpWebResponse)request.GetResponse();

                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        string jsonString = responseString;
                        dynamic myObject = JValue.Parse(jsonString);
                        var crmtoken12 = "";
                        foreach (dynamic questions in myObject)
                        {
                            if (questions.Name == "crm_token")
                                crmtoken12 = ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)questions).Value).Value.ToString();
                        }

                        var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

                        Session["t_crm_token"] = crmtoken12;
                    }
                }
            }
            return Json(crmtoken, JsonRequestBehavior.AllowGet);


        }
        public ActionResult SaleOrderReport(string FromDate, string ToDate, string CustomerName)
        {
            var SOR = new SaleOrderVM();
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            // var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/
            try
            {
                if (Session["UID"] != null)
                {
                    #region DateTime.......
                    string DateFormat = Constant.DateFormat();//get date format by company id
                    SOR.DateFormat = DateFormat;
                    Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                    var CurrentDate = Constant.GetBharatTime();
                    DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                    DateTime MonthendDate = new DateTime(monthStartDate.Year, monthStartDate.Month, CurrentDate.Day);

                    var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
                    var MEndDate = MonthendDate.ToString("dd/MM/yyyy");

                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            SOR.FromDate = FromDate;
                            SOR.ToDate = ToDate;

                            var fmDate = DateTime.ParseExact(FromDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
                            var tDate = DateTime.ParseExact(ToDate, DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(ToDate);

                            MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
                            MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

                        }
                        else
                        {
                            TempData["Fromdate"] = FromDate;
                            TempData["Todate"] = ToDate;
                            MStartDate = FromDate;
                            MEndDate = ToDate;
                        }
                    }
                    else
                    {
                        if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
                        {
                            SOR.FromDate = monthStartDate.ToString(DateFormat);
                            SOR.ToDate = MonthendDate.ToString(DateFormat);
                        }
                        else
                        {
                            SOR.FromDate = MStartDate;
                            SOR.ToDate = MEndDate;
                        }
                    }

                    #endregion

                    #region customer list
                    SOR.CustomerName = CustomerName;
                    var getCustomerList = db.customersgroupdetails.ToList();
                    var getcustomers = db.customers.Where(em => em.CompanyID == CompanyID.ToString()).ToList();
                    //var getCustomerList = db.customers.Where(em => em.CompanyID == CompanyID.ToString() && em.BranchCode == BranchID).ToList();
                    //string query = "SELECT distinct CustomerName,CustomerID FROM Customers where CompanyID = " + CompanyID + " and BranchCode=" + BranchID + "";
                    //var getCustomerList = db.Database.SqlQuery<CustomerListModel>(query).ToList();
                    //if (getCustomerList.Count > 0)
                    //{
                    //    SOR.CustomerList = new SelectList(getCustomerList, "CustomerName", "CustomerName", CustomerName);
                    //}
                    if (getCustomerList.Count > 0)
                    {

                        var CustList = (from item in getCustomerList
                                        join od in getcustomers on item.CustomerID equals od.CustomerID
                                        where item.CompanyId == Convert.ToInt16(CompanyID)
                                        select new QuotationModel
                                        {
                                            CustomerID = od.ID,
                                            CustomerName = od.CustomerName
                                        }).ToList();
                        SOR.CustomerList = new SelectList(CustList, "CustomerName", "CustomerName", CustomerName);

                    }
                    #endregion

                    DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetSaleOrderReport('" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + CustomerName + "')");
                    if (GetReport.Rows.Count > 0)
                    {
                        SOR.SaleReport = (from dr in GetReport.AsEnumerable()
                                          select new SaleOrderReportDTO()
                                          {
                                              BillNo = Convert.ToString(dr["BillNo"]),
                                              BillDate = dr["BillDate"] == DBNull.Value ? string.Empty : string.Format("{0:" + DateFormat + "}", Convert.ToDateTime(dr["BillDate"])),
                                              CustomerName = Convert.ToString(dr["Customer"]),
                                              QuotationNo = Convert.ToString(dr["Quotation"]),
                                              PurchasePrice = dr["PurchasePrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PurchasePrice"]),
                                              SalePrice = dr["SalePrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["SalePrice"]),
                                              Recivable = dr["Recivable"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Recivable"]),
                                              Delivery = dr["Delivery"] == DBNull.Value ? string.Empty : string.Format("{0:" + DateFormat + "}", Convert.ToDateTime(dr["Delivery"]))
                                          }).ToList();

                    }
                }
                else
                {
                    return Redirect("/home/login");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(SOR);
        }

        public ActionResult SaleOrderInvoice(string BillNo)
        {
            var SOR = new SaleOrderVM();
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string DateFormat = Constant.DateFormat();//get date format by company id
            var taxtype = db.taxtypemasters.Where(a => a.companyID == CompanyID && a.BranchCode == BranchID).FirstOrDefault();//get company tax type
            if (taxtype != null)
            {
                SOR.TaxTypeName = taxtype.TaxType;
            }

            if (SOR.TaxTypeName.ToLower() == "total")
            {
                decimal TotalTax = 0;

                string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_SOtaxdetails where GRNNo='" + BillNo + "' and CompanyID = " + CompanyID + " and BranchCode =" + BranchID + "";
                TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                SOR.TotalTax = TotalTax;
            }


            //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            //var CurrentDate = Constant.GetBharatTime();

            #region get company detail
            DataTable GetCompanyDetail = DataAccessLayer.GetDataTable(" call SpCompanyDetail(" + CompanyID + "," + BranchID + ")");
            if (GetCompanyDetail.Rows.Count > 0)
            {
                var data = (from dr in GetCompanyDetail.AsEnumerable()
                            select new SaleOrderVM()
                            {
                                RegistrationNo = Convert.ToString(dr["RegistrationNo"]),
                                CompanyEmail = Convert.ToString(dr["Email"]),
                                CompanyMobileNo = Convert.ToString(dr["Phoneno"]),
                                CompanyAlternateNo = Convert.ToString(dr["AlternateNo"]),
                                Organization = Convert.ToString(dr["Organization"]),
                                CompanyAddress = Convert.ToString(dr["Address"])
                            }).FirstOrDefault();

                SOR.RegistrationNo = data.RegistrationNo;
                SOR.CompanyEmail = data.CompanyEmail;
                SOR.CompanyMobileNo = data.CompanyMobileNo;
                SOR.CompanyAlternateNo = data.CompanyAlternateNo;
                SOR.Organization = data.Organization;
                SOR.CompanyAddress = data.CompanyAddress;
            }
            #endregion

            #region get sale order item detail
            DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetDataSaleOrder('" + BillNo + "'," + CompanyID + "," + BranchID + ")");
            if (GetDataSaleOrder.Rows.Count > 0)
            {
                SOR.SaleReport = new List<SaleOrderReportDTO>();
                decimal Gt = 0;
                for (int i = 0; i < GetDataSaleOrder.Rows.Count; i++)
                {
                    var rec = new SaleOrderReportDTO();
                    decimal cgst_per = 0;
                    decimal cgstAmt = 0;
                    decimal igst_per = 0;
                    decimal igstAmt = 0;
                    rec.BillNo = Convert.ToString(GetDataSaleOrder.Rows[i]["BillNo"]);
                    rec.ItemName = Convert.ToString(GetDataSaleOrder.Rows[i]["Particular"]);
                    rec.CustomerName = Convert.ToString(GetDataSaleOrder.Rows[i]["Customer_Name"]);
                    rec.Address = Convert.ToString(GetDataSaleOrder.Rows[i]["Address"]);
                    rec.MobileNo = Convert.ToString(GetDataSaleOrder.Rows[i]["MobileNo"]);
                    rec.POS = Convert.ToString(GetDataSaleOrder.Rows[i]["POS"]);
                    rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
                    rec.Qty = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["qty"]);
                    rec.Price = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Price"]);
                    rec.OurPrice = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Ourprice"]);
                    rec.RefNo = Convert.ToString(GetDataSaleOrder.Rows[i]["RefNo"]);
                    rec.GSTPer = GetDataSaleOrder.Rows[i]["gstper"] == DBNull.Value ? 0 : Convert.ToDecimal(GetDataSaleOrder.Rows[i]["gstper"]);
                    rec.GSTAmount = GetDataSaleOrder.Rows[i]["gstamt"] == DBNull.Value ? 0 : Convert.ToDecimal(GetDataSaleOrder.Rows[i]["gstamt"]);
                    rec.Delivery = GetDataSaleOrder.Rows[i]["Delivery"] == DBNull.Value ? string.Empty : string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(GetDataSaleOrder.Rows[i]["Delivery"]));

                    if (SOR.TaxTypeName.ToLower() == "total")
                    {
                        decimal total = rec.Qty * rec.OurPrice;
                        rec.Total = Math.Round((total), 2);
                        Gt = Math.Round((Gt + total), 2);
                    }
                    else
                    {
                        decimal total = rec.Qty * rec.Price;
                        rec.Total = Math.Round(total, 2);//total

                        rec.NetTaxable = Math.Round(total, 2);//net taxable
                        if (rec.RefNo == "CGST+UGST" || rec.RefNo == "CGST+SGST")
                        {
                            cgst_per = rec.GSTPer / 2;
                            cgstAmt = rec.GSTAmount / 2;

                            rec.CGSTPer = rec.GSTPer / 2;
                            rec.CGSTAmount = rec.GSTAmount / 2;

                        }
                        else if (rec.RefNo == "IGST")
                        {
                            igst_per = rec.GSTPer;
                            igstAmt = rec.GSTAmount;
                            rec.IGSTPer = rec.GSTPer;
                            rec.IGSTAmount = rec.GSTAmount;
                        }

                        Gt = Math.Round((Gt + total + cgstAmt + cgstAmt + igstAmt), 2);
                    }


                    SOR.SaleReport.Add(rec);
                }

                SOR.OrderNo = SOR.SaleReport[0].BillNo;
                SOR.PlaceOfSupply = SOR.SaleReport[0].POS;
                SOR.CustomerName = SOR.SaleReport[0].CustomerName;
                SOR.CustomerMobileNo = SOR.SaleReport[0].MobileNo;
                SOR.CustomerAddress = SOR.SaleReport[0].Address;
                SOR.DeliveryDate = SOR.SaleReport[0].Delivery;

                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    SOR.NetAmount = Gt + SOR.TotalTax;//net amount with total tax
                    decimal rounded = Math.Round(Gt + SOR.TotalTax);
                    SOR.RoundOff = Math.Round((rounded - (Gt + SOR.TotalTax)), 2);//round off//
                }
                else
                {
                    SOR.NetAmount = Gt;//net amount
                    decimal rounded = Math.Round(Gt);
                    SOR.RoundOff = Math.Round((rounded - Gt), 2);//round off//
                }

                SOR.AmountInWord = NumberToWord(Convert.ToInt32(SOR.NetAmount));
            }
            #endregion
            return PartialView("_SaleOrderReportDetail", SOR);
        }



        #region number to word
        private string NumberToWord(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWord(Math.Abs(number));

            string words = "";
            if ((number / 1000000000) > 0)
            {
                words += NumberToWord(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 10000000) > 0)
            {
                words += NumberToWord(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWord(number / 1000000) + " Million ";
                number %= 1000000;
            }


            if ((number / 100000) > 0)
            {
                words += NumberToWord(number / 100000) + " Lakh ";
                number %= 100000;
            }


            if ((number / 1000) > 0)
            {
                words += NumberToWord(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWord(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        #endregion

        public string Getresponcetext(string id, string pwd)
        {
            //string responseText = String.Empty;
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://shouldipost.nicoleinfosoftdemo.com/login");
            //string postData = "{\"login-username\":\'" + id + "'\", \"login-password\":\'" + pwd + "'\"}";
            //request.Method = "POST";
            //request.ContentLength = postData.Length;
            //using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
            //{
            //    sw.Write(postData);
            //}
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            //{
            //    responseText = sr.ReadToEnd();
            //}
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            var request = (HttpWebRequest)WebRequest.Create("https://shouldipost.nicoleinfosoftdemo.com/login/crm");

            var postData = "login-username=" + Uri.EscapeDataString(id);
            postData += "&login-password=" + Uri.EscapeDataString(pwd);
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var tlogin = db.t_login.Where(a => a.company_id == CompanyID).FirstOrDefault();

            Session["t_crm_token"] = tlogin.crm_token;
            Session["t_crm_token_cr_time"] = tlogin.crm_token_cr_time;

            return responseString;

        }
    }
}
