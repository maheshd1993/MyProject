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
    [NoCache]
    public class QuotationController : Controller
    {
        niscrmEntities db = new niscrmEntities();
        public ActionResult CreateQuotation()
        {
            CRMQutationsaleModel QSM = new CRMQutationsaleModel();
            try
            {

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
                            return Redirect("/Quotation/CreateQuotation1");
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

                        return Redirect("/Quotation/CreateQuotation1");
                    }

                }
            }
            catch (Exception ex)
            {

            }


            return PartialView("_QuatationDropDownModule1", QSM);
        }

        public ActionResult CreateQuotation1()
        {
            return View();
        }

        public JsonResult BindcomgrpModule(int? pid)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);

            List<Addcustomegrop> data = db.customersgroups.Where(em => em.CompanyId == pid)
                                    .Select(em => new Addcustomegrop
                                    {
                                        ID = em.ID,
                                        CustGroupName = em.CustGroupName
                                    }).OrderBy(a => a.CustGroupName).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
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
                    return Redirect("/Quotation/CreateQuotation");
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
                        return Redirect("/Quotation/CreateQuotation1");
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
                            return Redirect("/Quotation/CreateQuotation1");


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
        //public ActionResult getmultiplecompany()
        //{
        //    //CRMQutationsaleModel QSM = new CRMQutationsaleModel();
        //    //int BranchID = Convert.ToInt32(Session["BranchID"]);
        //    //int CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //    //string emailid = Convert.ToString(Session["UserEmail"]);
        //    //var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
        //    //if (comptype.Count > 0)
        //    //{
        //    //    string Orglist = @"select ID, Organization from company_profile Where mastercompany = '" + CompanyID + "'";
        //    //    var getOrg = db.Database.SqlQuery<CRMQutationsaleModel>(Orglist).OrderBy(a => a.Organization).ToList();
        //    //    if (getOrg.Count > 0)
        //    //    {
        //    //        QSM.OrgList = getOrg;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    //string Orglist = @"select ID, Organization from company_profile Where mastercompany = '" + CompanyID + "'";


        //    //}


        //    //return PartialView("_QuatationDropDownModule", QSM);
        //}

        [HttpGet]
        public ActionResult EditQuotation(int? QuotationID)
        {
            QuotationModel QM = new QuotationModel();
            try
            {
                int uid = Convert.ToInt32(Session["UID"]);
                Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                String CompanyID = Convert.ToString(Session["CompanyID"]);
                Int32 iCompanyID = Convert.ToInt32(Session["CompanyID"]);
                QM.CompanyID = iCompanyID;
                QM.BranchID = BranchID;
                var dd = Constant.GetBharatTime();

                QM.BillDate = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", dd));
                QM.DeliveryDate = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", dd));
                var taxtype = db.taxtypemasters.Where(a => a.companyID == iCompanyID && a.BranchCode == BranchID).FirstOrDefault();
                if (taxtype != null)
                {
                    QM.TaxType = taxtype.TaxType;
                    Session["taxType"] = taxtype.TaxType;
                }

                string query = @"select TaxCode,TaxName,Sum(TaxPer) as TaxPer,TaxMethod 
                            from taxmaster tm where Applicable_Sale = 1 and tm.companyid = " + iCompanyID + " and tm.BranchCode=" + BranchID + " group by TaxMethod";
                var taxperdata = db.Database.SqlQuery<TaxPerMaster>(query).ToList();
                QM.TaxDetails = taxperdata;

                var getCustomerList = db.customers.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).ToList();
                if (getCustomerList.Count > 0)
                {
                    //List<QuotationModel> CustList = new List<QuotationModel>();
                    var CustList = (from item in getCustomerList
                                    select new QuotationModel
                                    {
                                        CustomerID = item.ID,
                                        CustomerName = item.CustomerName
                                    }).ToList();
                    QM.oCustomerList = CustList;
                }


                var GetStateList = db.com_state.Where(em => em.Country == "1").ToList();
                if (GetStateList.Count > 0)
                {
                    //List<QuotationModel> cList = new List<QuotationModel>();
                    var cList = (from item in GetStateList
                                 select new QuotationModel
                                 {
                                     StateID = item.ID,
                                     StateName = item.State
                                 }).ToList();
                    QM.oStateList = cList;
                }

                string[] slTypes ={
                    "GST",
                    "Exempt",
                    "Nil Rated",
                    "Zero Rated",
                    "Non GST"};
                QM.saleTypes = new List<string>(slTypes);

                //string query=@"select company_type from company_profile where ID=" + iCompanyID + "";
                //var ctype = db.Database.SqlQuery<string>(query).FirstOrDefault();
                //if (ctype.ToLower() == "registered")
                //{
                //    string[] slTypes ={
                //    "GST",
                //    "Exempt",
                //    "Nil Rated",
                //    "Zero Rated",
                //    "Non GST"};
                //    QM.saleTypes = new List<string>(slTypes);                    
                //}
                //else
                //{
                //    string[] slTypes ={"Composition"};
                //    QM.saleTypes = new List<string>(slTypes);                   
                //}

                //var CreditLimit = db.customercrdrs.Where(em => em.CompanyId == iCompanyID && em.BranchCode == BranchID).FirstOrDefault();
                //if (CreditLimit != null)
                //{
                //    QM.CreditLimit = CreditLimit.Cr;
                //    QM.Dues = CreditLimit.Balance;
                //}
                //else
                //{
                //    QM.CreditLimit = 0.00M;
                //    QM.Dues = 0.00M;
                //}

                //var customerDues = db.customercrdrs.Where(em => em.CompanyId == iCompanyID && em.BranchCode == BranchID).FirstOrDefault();
                //if (CreditLimit != null)
                //{
                //    QM.Dues = customerDues.Balance;
                //}
                //else
                //{
                //    QM.Dues = 0.00M;
                //}
                QM.BillNumber = GetBillNo(BranchID, iCompanyID);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(QM);
        }

        [HttpPost]
        public ActionResult EditQuotation(List<QuotationModel> QuotationList)
        {
            String mgs = String.Empty;
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            decimal? TotalAmount = 0;
            string pincode = string.Empty;
            string RefNo = string.Empty;
            string QuotNo = string.Empty;
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (QuotationList.Count > 0)
                    {
                        TotalAmount = QuotationList.Select(a => a.FinalTotalAmount).FirstOrDefault();
                        pincode = QuotationList.Select(a => a.Pincode).FirstOrDefault();
                        QuotNo = QuotationList.Select(a => a.BillNumber).FirstOrDefault();
                        DataTable GetCompanyDetail = DataAccessLayer.GetDataTable(" call CRM_CompanyBranchDetail(" + CompanyID + "," + BranchID + ")");
                        if (GetCompanyDetail.Rows.Count > 0)
                        {
                            var data = (from dr in GetCompanyDetail.AsEnumerable()
                                        select new QuotationReportVM()
                                        {
                                            //BranchName = dr["BranchName"].ToString(),
                                            //RegistrationNo = dr["RegistrationNo"].ToString(),
                                            //CompanyMobileNo = dr["Phoneno"].ToString(),
                                            //CompanyEmail = Convert.ToString(dr["Email"]),
                                            //CompanyAddress = dr["Address"].ToString(),
                                            StateId = Convert.ToInt32(dr["StateId"]),
                                            StateName = Convert.ToString(dr["State"])
                                        }).FirstOrDefault();
                            string customerStateName = QuotationList.Select(a => a.StateName).FirstOrDefault();
                            if (customerStateName == data.StateName)
                            {
                                RefNo = "CGST+SGST";
                            }
                            else if (customerStateName == "35-Andaman & Nicobar Islands")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else if (customerStateName == "04-Chandigarh")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else if (customerStateName == "26-Dadra & Nagar Haveli")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else if (customerStateName == "25-Daman & Diu")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else if (customerStateName == "31-Lakshdweep")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else if (customerStateName == "34-Pondicherry")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else if (customerStateName == "01-Jammu & Kashmir")
                            {
                                RefNo = "CGST+UGST";
                            }
                            else
                            {
                                RefNo = "IGST";
                            }
                        }
                    }

                    foreach (var item in QuotationList)
                    {
                        int LastSyncID = 0;
                        decimal perUnitPrice = 0;
                        DataTable dtLastSync = DataAccessLayer.GetDataTable("call CRM_QuotationLastSyncID('" + CompanyID + "'," + BranchID + ")");
                        if (Convert.ToInt32(dtLastSync.Rows[0]["SyncID"] == DBNull.Value ? 0 : dtLastSync.Rows[0]["SyncID"]) > 0)
                        {
                            LastSyncID = Convert.ToInt32(dtLastSync.Rows[0]["SyncID"]) + 1;
                        }
                        else
                        {
                            LastSyncID = 1;
                        }

                        tbl_quotation quo = new tbl_quotation();
                        quo.QuoNo = item.BillNumber;
                        quo.customers_ID = Convert.ToInt32(item.CustomerID);
                        quo.QuoDt = Convert.ToDateTime(item.BillDate);
                        quo.CustoNm = item.CustomerName;
                        quo.MobileNo = item.MobileNumber;
                        quo.Address = item.Address;
                        quo.TotalQty = Convert.ToInt32(item.TotalQty);
                        quo.Rmk = string.Empty;
                        quo.RefNo = RefNo;
                        quo.gstper = item.GstPercent;
                        quo.gstamt = item.GstAmount;
                        quo.Shift = item.StateName;
                        quo.Particular = item.ItemName;
                        quo.SKU = item.ItemCode;
                        quo.Color = item.ColorName;
                        quo.Size = item.SizeName;
                        if (item.GstPercent > 0)
                        {
                            if (quo.TotalQty > 1)
                            {
                                perUnitPrice = Math.Round(Convert.ToDecimal(item.AfterAmountDiscount / quo.TotalQty), 2);
                                decimal perGstAMt = Math.Round(Convert.ToDecimal((perUnitPrice * item.GstPercent) / (item.GstPercent + 100)), 2);
                                decimal UnitPrice = perUnitPrice - perGstAMt;
                                quo.Price = UnitPrice;
                            }
                            else
                            {
                                perUnitPrice = item.AfterAmountDiscount ?? 0;
                                decimal perGstAMt = Math.Round(Convert.ToDecimal((perUnitPrice * item.GstPercent) / (item.GstPercent + 100)), 2);
                                decimal UnitPrice = perUnitPrice - perGstAMt;
                                quo.Price = UnitPrice;
                            }
                        }
                        else
                        {
                            quo.Price = item.OurPrice;
                        }
                        quo.Unit = item.UnitName;
                        quo.Ourprice = item.OurPrice;
                        quo.mrp = item.MRP;
                        quo.DisPer = item.DiscountPercent;
                        quo.DisAmt = item.DiscountAmount;
                        quo.totalamt = item.AfterAmountDiscount;
                        quo.challanamstcolumn10 = item.AfterAmountDiscount;
                        quo.GrandTotal = item.GrandTotal;
                        quo.Receivableamt = item.Receivable;
                        quo.BalanceAmt = item.BalanceAmount;
                        quo.CreatedDate = Constant.GetBharatTime();
                        quo.CompanyID = Convert.ToString(CompanyID);
                        quo.BranchCode = BranchID;
                        quo.CreatedBy = Convert.ToString(Session["UserName"]);
                        quo.flag = "N";
                        quo.SyncID = "O" + LastSyncID;
                        db.tbl_quotation.Add(quo);
                    }
                    db.SaveChanges();

                    var taxType = Convert.ToString(Session["taxType"]);
                    if (!string.IsNullOrEmpty(taxType) && taxType == "total" && TotalAmount > 0)
                    {
                        string query = @"select TaxCode,TaxName,Sum(TaxPer) as TaxPer,TaxMethod 
                            from taxmaster tm where Applicable_Sale = 1 and tm.companyid = " + CompanyID + " and tm.BranchCode=" + BranchID + " ;";
                        var taxperdata = db.Database.SqlQuery<TaxPerMaster>(query).ToList();

                        if (taxperdata != null && taxperdata.Count > 0)
                        {
                            decimal? taxAMT = 0;
                            foreach (var item in taxperdata)
                            {
                                int LastSyncID = 0;
                                DataTable dtLastSync = DataAccessLayer.GetDataTable("call CRM_Sal_QoutLastSyncID('" + CompanyID + "'," + BranchID + ")");
                                if (Convert.ToInt32(dtLastSync.Rows[0]["SyncId"] == DBNull.Value ? 0 : dtLastSync.Rows[0]["SyncId"]) > 0)
                                {
                                    LastSyncID = Convert.ToInt32(dtLastSync.Rows[0]["SyncId"]) + 1;
                                }
                                else
                                {
                                    LastSyncID = 1;
                                }
                                if (!string.IsNullOrEmpty(pincode))
                                {
                                    if (item.TaxPer > 0)
                                    {
                                        taxAMT = Math.Round(Convert.ToDecimal(TotalAmount * item.TaxPer / 100), 2);
                                    }
                                }
                                else
                                {
                                    if (item.TaxPer > 0 && item.TaxMethod == "fixed")
                                    {
                                        taxAMT = Math.Round(Convert.ToDecimal(TotalAmount * item.TaxPer / 100), 2);
                                    }
                                }

                                var txDetail = new sal_quotaxdetails
                                {
                                    GRNNo = QuotNo,
                                    TaxCode = item.TaxCode,
                                    TaxName = item.TaxName,
                                    TaxPer = item.TaxPer,
                                    TaxAmt = taxAMT,
                                    CompanyID = CompanyID,
                                    BranchCode = BranchID,
                                    flag = "N",
                                    SyncId = "O" + LastSyncID
                                };
                                db.sal_quotaxdetails.Add(txDetail);
                            }
                            db.SaveChanges();
                        }
                    }
                    trans.Commit();
                    mgs = "Quotation add successful";

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    mgs = "Something went wrong,Please try again!";
                    ExceptionLogging.SendExcepToDB(ex);
                }
            }

            return Json(mgs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageQuotation(string FromDate, string ToDate, int CustomerId = 0)
        {

            CRMQutationsaleModel QSM = new CRMQutationsaleModel();
            try
            {
                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                string emailid = Convert.ToString(Session["UserEmail"]);
                var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                if (comptype.Count > 0)
                {
                    string Orglist = @"SELECT ID, Organization FROM company_profile WHERE ID = '" + CompanyID + "'";
                    var getOrg = db.Database.SqlQuery<CRMQutationsaleModel>(Orglist).OrderBy(a => a.Organization).ToList();

                    string Orglist2 = @"SELECT ID, Organization FROM company_profile WHERE mastercompany='" + CompanyID + "'";
                    var getOrg2 = db.Database.SqlQuery<CRMQutationsaleModel>(Orglist2).OrderBy(a => a.Organization).ToList();

                    var newlist = getOrg.Concat(getOrg2);
                    QSM.joinlist = getOrg.Concat(getOrg2).ToList();
                }
                else
                {
                    var QR = new QuotationReportVM();
                    var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/

                    if (Session["UID"] != null)
                    {
                        #region DateTime.......
                        string DateFormat = Constant.DateFormat();//get date format by company id
                        QR.DateFormat = DateFormat;
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
                                QR.FromDate = FromDate;
                                QR.ToDate = ToDate;

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
                                QR.FromDate = monthStartDate.ToString(DateFormat);
                                QR.ToDate = MonthendDate.ToString(DateFormat);
                            }
                            else
                            {
                                QR.FromDate = MStartDate;
                                QR.ToDate = MEndDate;
                            }
                        }

                        #endregion

                        #region customer list
                        QR.CustomerId = CustomerId;
                        var getCustomerList = db.customers.Where(em => em.CompanyID == CompanyID.ToString() && em.BranchCode == BranchID).ToList();
                        if (getCustomerList.Count > 0)
                        {

                            //var CustList = (from item in getCustomerList
                            //                select new QuotationModel
                            //                {
                            //                    CustomerID = item.ID,
                            //                    CustomerName = item.CustomerName
                            //                }).ToList();
                            QR.CustomerList = new SelectList(getCustomerList, "ID", "CustomerName", CustomerId);
                        }

                        #endregion

                        if (CustomerId != 0)
                        {
                            DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReportmultiplecompany('" + user + "','" + MStartDate + "','" + MEndDate + "'," + CustomerId + ")");
                            if (GetReport.Rows.Count > 0)
                            {
                                QR.QtReport = (from dr in GetReport.AsEnumerable()
                                               select new QuotationDTO()
                                               {
                                                   QuotationNo = Convert.ToString(dr["QuoNo"]),
                                                   CustomerName = Convert.ToString(dr["CustoNm"]),
                                                   MobileNo = Convert.ToString(dr["MobileNo"]),
                                                   TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                                   QuotationDate = Convert.ToDateTime(dr["QuoDt"])
                                               }).ToList();

                            }
                        }
                        else
                        {
                            DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReport('" + user + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + CustomerId + ")");
                            if (GetReport.Rows.Count > 0)
                            {
                                QR.QtReport = (from dr in GetReport.AsEnumerable()
                                               select new QuotationDTO()
                                               {
                                                   QuotationNo = Convert.ToString(dr["QuoNo"]),
                                                   CustomerName = Convert.ToString(dr["CustoNm"]),
                                                   MobileNo = Convert.ToString(dr["MobileNo"]),
                                                   TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                                   QuotationDate = Convert.ToDateTime(dr["QuoDt"])
                                               }).ToList();

                            }
                        }
                    }
                    else
                    {
                        return Redirect("/home/login");
                    }
                    return Redirect("/Quotation/ManageQuotation1");
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ManageQuatationDropDownModule", QSM);
            //var QR = new QuotationReportVM();
            //Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            //Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            //var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/
            //try
            //{
            //    if (Session["UID"] != null)
            //    {
            //        #region DateTime.......
            //        string DateFormat = Constant.DateFormat();//get date format by company id
            //        QR.DateFormat = DateFormat;
            //        Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
            //        var CurrentDate = Constant.GetBharatTime();
            //        DateTime monthStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            //        DateTime MonthendDate = new DateTime(monthStartDate.Year, monthStartDate.Month, CurrentDate.Day);

            //        var MStartDate = monthStartDate.ToString("dd/MM/yyyy");
            //        var MEndDate = MonthendDate.ToString("dd/MM/yyyy");
            //        if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
            //        {
            //            if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
            //            {
            //                QR.FromDate = FromDate;
            //                QR.ToDate = ToDate;

            //                var fmDate = DateTime.ParseExact(FromDate, DateFormat, CultureInfo.InvariantCulture); //Convert.ToDateTime(FromDate);
            //                var tDate = DateTime.ParseExact(ToDate, DateFormat, CultureInfo.InvariantCulture);// Convert.ToDateTime(ToDate);

            //                MStartDate = String.Format("{0:dd/MM/yyyy}", fmDate);//convert to dd/MM/yyyy format for stored procedure
            //                MEndDate = String.Format("{0:dd/MM/yyyy}", tDate);//convert to dd/MM/yyyy format for stored procedure

            //            }
            //            else
            //            {
            //                TempData["Fromdate"] = FromDate;
            //                TempData["Todate"] = ToDate;
            //                MStartDate = FromDate;
            //                MEndDate = ToDate;
            //            }
            //        }
            //        else
            //        {
            //            if (DateFormat != "dd/MM/yyyy")//check if date format not dd/MM/yyyy format then convert it
            //            {
            //                QR.FromDate = monthStartDate.ToString(DateFormat);
            //                QR.ToDate = MonthendDate.ToString(DateFormat);
            //            }
            //            else
            //            {
            //                QR.FromDate = MStartDate;
            //                QR.ToDate = MEndDate;
            //            }
            //        }

            //        #endregion

            //        #region customer list
            //        QR.CustomerId = CustomerId;
            //        var getCustomerList = db.customers.Where(em => em.CompanyID == CompanyID.ToString() && em.BranchCode == BranchID).ToList();
            //        if (getCustomerList.Count > 0)
            //        {

            //            //var CustList = (from item in getCustomerList
            //            //                select new QuotationModel
            //            //                {
            //            //                    CustomerID = item.ID,
            //            //                    CustomerName = item.CustomerName
            //            //                }).ToList();
            //            QR.CustomerList = new SelectList(getCustomerList, "ID", "CustomerName", CustomerId);
            //        }

            //        #endregion

            //        DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReport('" + user + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + CustomerId + ")");
            //        if (GetReport.Rows.Count > 0)
            //        {
            //            QR.QtReport = (from dr in GetReport.AsEnumerable()
            //                           select new QuotationDTO()
            //                           {
            //                               QuotationNo = Convert.ToString(dr["QuoNo"]),
            //                               CustomerName = Convert.ToString(dr["CustoNm"]),
            //                               MobileNo = Convert.ToString(dr["MobileNo"]),
            //                               TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
            //                               QuotationDate = Convert.ToDateTime(dr["QuoDt"])
            //                           }).ToList();

            //        }
            //    }
            //    else
            //    {
            //        return Redirect("/home/login");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionLogging.SendExcepToDB(ex);
            //}
            //return View(QR);
        }

        public ActionResult GetManageQuotationCompanydetail(int companyid)
        {
            string FromDate = "";
            string ToDate = "";
            int CustomerId = 0;
            var QR = new QuotationReportVM();
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/
            try
            {
                if (Session["UID"] != null)
                {
                    #region DateTime.......
                    string DateFormat = Constant.DateFormat();//get date format by company id
                    QR.DateFormat = DateFormat;
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
                            QR.FromDate = FromDate;
                            QR.ToDate = ToDate;

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
                            QR.FromDate = monthStartDate.ToString(DateFormat);
                            QR.ToDate = MonthendDate.ToString(DateFormat);
                        }
                        else
                        {
                            QR.FromDate = MStartDate;
                            QR.ToDate = MEndDate;
                        }
                    }

                    #endregion

                    #region customer list
                    QR.CustomerId = CustomerId;
                    QR.companyid = Convert.ToInt32(companyid);
                    var getCustomerList = db.customers.Where(em => em.CompanyID == CompanyID.ToString() && em.BranchCode == BranchID).ToList();
                    if (getCustomerList.Count > 0)
                    {

                        //var CustList = (from item in getCustomerList
                        //                select new QuotationModel
                        //                {
                        //                    CustomerID = item.ID,
                        //                    CustomerName = item.CustomerName
                        //                }).ToList();
                        QR.CustomerList = new SelectList(getCustomerList, "ID", "CustomerName", CustomerId);
                    }

                    #endregion

                    DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReport('" + user + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + companyid + "," + CustomerId + ")");
                    if (GetReport.Rows.Count > 0)
                    {
                        QR.QtReport = (from dr in GetReport.AsEnumerable()
                                       select new QuotationDTO()
                                       {
                                           QuotationNo = Convert.ToString(dr["QuoNo"]),
                                           CustomerName = Convert.ToString(dr["CustoNm"]),
                                           MobileNo = Convert.ToString(dr["MobileNo"]),
                                           TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                           QuotationDate = Convert.ToDateTime(dr["QuoDt"]),
                                           companyId = Convert.ToInt32(dr["CompanyID"]),
                                           Createdby = Convert.ToString(dr["CreatedBy"])
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
            return Json(companyid, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ManageQuotation1(string FromDate, string ToDate, int CustomerId = 0, int Q_filterstatus = 0)
        {
            var QR = new QuotationReportVM();
            try
            {
                string companyid = Request.QueryString["comid"];
                if (companyid == null)
                {
                    companyid = Convert.ToString(Session["CompanyIDs"]);
                }
                if (companyid != null)
                {
                    Session["CompanyIDs"] = companyid;
                    int branchid = 0;
                    var qttbl = db.tbl_quotation.Where(em => em.CompanyID == companyid).FirstOrDefault();
                    if (qttbl != null)
                    {
                        branchid = Convert.ToInt32(qttbl.BranchCode);
                    }
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/

                    if (Session["UID"] != null)
                    {
                        #region DateTime.......
                        string DateFormat = Constant.DateFormat();//get date format by company id
                        QR.DateFormat = DateFormat;
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
                                QR.FromDate = FromDate;
                                QR.ToDate = ToDate;

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
                                QR.FromDate = monthStartDate.ToString(DateFormat);
                                QR.ToDate = MonthendDate.ToString(DateFormat);
                            }
                            else
                            {
                                QR.FromDate = MStartDate;
                                QR.ToDate = MEndDate;
                            }
                        }

                        #endregion

                        #region customer list
                        QR.CustomerId = CustomerId;
                        //int UserID = Convert.ToInt32(Session["UID"]);
                        //var data =  db.crm_usertbl.Where(a => a.Id == UserID).FirstOrDefault();
                        ////var getcustomersGroup = "select Customergroupid from crm_usertbl where UserName='" + Convert.ToString(Session["UserName"]) + "' ";
                        ////var getCustomerList = db.customersgroupdetails.Where(em => em.CompanyId == Convert.ToInt32(companyid) && em.CustomerGroupId == Convert.ToInt32(getcustomersGroup)).ToList();

                        var getCustomerList = db.customersgroupdetails.ToList();
                        var getcustomers = db.customers.Where(em => em.CompanyID == companyid.ToString()).ToList();
                        if (getCustomerList.Count > 0)
                        {

                            var CustList = (from item in getCustomerList
                                            join od in getcustomers on item.CustomerID equals od.CustomerID
                                            where od.CompanyID== companyid
                                            select new QuotationModel
                                            {
                                                CustomerID = od.ID,
                                                CustomerName = od.CustomerName
                                            }).ToList();
                            QR.CustomerList = new SelectList(CustList, "CustomerID", "CustomerName", CustomerId);
                        }

                        #endregion

                        DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReport('" + user + "','" + MStartDate + "','" + MEndDate + "'," + branchid + "," + companyid + "," + CustomerId + ")");
                        if (GetReport.Rows.Count > 0)
                        {
                            QR.QtReport = (from dr in GetReport.AsEnumerable()
                                           select new QuotationDTO()
                                           {
                                               QuotationNo = Convert.ToString(dr["QuoNo"]),
                                               CustomerName = Convert.ToString(dr["CustoNm"]),
                                               MobileNo = Convert.ToString(dr["MobileNo"]),
                                               TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                               QuotationDate = Convert.ToDateTime(dr["QuoDt"]),
                                               companyId = Convert.ToInt32(dr["CompanyID"]),
                                               Createdby = Convert.ToString(dr["CreatedBy"]),
                                               status = Convert.ToInt32(dr["status"])
                                           }).ToList();

                        }
                    }

                }
                else if (Q_filterstatus != 0)
                {
                    int branchid = 0;
                    var qttbl = db.tbl_quotation.Where(em => em.CompanyID == companyid).FirstOrDefault();
                    if (qttbl != null)
                    {
                        branchid = Convert.ToInt32(qttbl.BranchCode);
                    }
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/

                    if (Session["UID"] != null)
                    {
                        #region DateTime.......
                        string DateFormat = Constant.DateFormat();//get date format by company id
                        QR.DateFormat = DateFormat;
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
                                QR.FromDate = FromDate;
                                QR.ToDate = ToDate;

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
                                QR.FromDate = monthStartDate.ToString(DateFormat);
                                QR.ToDate = MonthendDate.ToString(DateFormat);
                            }
                            else
                            {
                                QR.FromDate = MStartDate;
                                QR.ToDate = MEndDate;
                            }
                        }

                        #endregion

                        #region customer list
                        QR.CustomerId = CustomerId;
                        var getCustomerList = db.customers.Where(em => em.CompanyID == companyid.ToString()).ToList();
                        if (getCustomerList.Count > 0)
                        {

                            //var CustList = (from item in getCustomerList
                            //                select new QuotationModel
                            //                {
                            //                    CustomerID = item.ID,
                            //                    CustomerName = item.CustomerName
                            //                }).ToList();
                            QR.CustomerList = new SelectList(getCustomerList, "ID", "CustomerName", CustomerId);
                        }

                        #endregion

                        DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReport1('" + user + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + ",'" + Q_filterstatus + "')");
                        if (GetReport.Rows.Count > 0)
                        {
                            QR.QtReport = (from dr in GetReport.AsEnumerable()
                                           select new QuotationDTO()
                                           {
                                               QuotationNo = Convert.ToString(dr["QuoNo"]),
                                               CustomerName = Convert.ToString(dr["CustoNm"]),
                                               MobileNo = Convert.ToString(dr["MobileNo"]),
                                               TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                               QuotationDate = Convert.ToDateTime(dr["QuoDt"]),
                                               companyId = Convert.ToInt32(dr["CompanyID"]),
                                               Createdby = Convert.ToString(dr["CreatedBy"]),
                                               status = Convert.ToInt32(dr["status"])
                                           }).ToList();

                        }
                    }
                }
                else
                {
                    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
                    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
                    var user = string.Empty;/*Convert.ToString(Session["UserType"]) == "SuperAdmin"?"": Convert.ToString(Session["UserName"]);*/
                    var getCompanyId = db.customers.Where(em => em.CrmCustomerID == CustomerId && em.BranchCode == BranchID).FirstOrDefault();
                    if (Session["UID"] != null)
                    {
                        #region DateTime.......
                        string DateFormat = Constant.DateFormat();//get date format by company id
                        QR.DateFormat = DateFormat;
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
                                QR.FromDate = FromDate;
                                QR.ToDate = ToDate;

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
                                QR.FromDate = monthStartDate.ToString(DateFormat);
                                QR.ToDate = MonthendDate.ToString(DateFormat);
                            }
                            else
                            {
                                QR.FromDate = MStartDate;
                                QR.ToDate = MEndDate;
                            }
                        }

                        #endregion

                        #region customer list
                        QR.CustomerId = CustomerId;
                        var getCustomerList = db.customers.Where(em => em.CompanyID == CompanyID.ToString() && em.BranchCode == BranchID).ToList();
                        if (getCustomerList.Count > 0)
                        {

                            //var CustList = (from item in getCustomerList
                            //                select new QuotationModel
                            //                {
                            //                    CustomerID = item.ID,
                            //                    CustomerName = item.CustomerName
                            //                }).ToList();
                            QR.CustomerList = new SelectList(getCustomerList, "ID", "CustomerName", CustomerId);
                        }

                        #endregion

                        if (CustomerId != 0)
                        {
                            DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReportmultiplecompany('" + user + "','" + MStartDate + "','" + MEndDate + "'," + CustomerId + ")");
                            if (GetReport.Rows.Count > 0)
                            {
                                QR.QtReport = (from dr in GetReport.AsEnumerable()
                                               select new QuotationDTO()
                                               {
                                                   QuotationNo = Convert.ToString(dr["QuoNo"]),
                                                   CustomerName = Convert.ToString(dr["CustoNm"]),
                                                   MobileNo = Convert.ToString(dr["MobileNo"]),
                                                   TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                                   QuotationDate = Convert.ToDateTime(dr["QuoDt"]),
                                                   companyId = Convert.ToInt32(dr["CompanyID"]),
                                                   Createdby = Convert.ToString(dr["CreatedBy"]),
                                                   status = Convert.ToInt32(dr["status"])
                                               }).ToList();

                            }
                        }
                        else
                        {
                            DataTable GetReport = DataAccessLayer.GetDataTable(" call CRM_GetQuotationReport('" + user + "','" + MStartDate + "','" + MEndDate + "'," + BranchID + "," + CompanyID + "," + CustomerId + ")");
                            if (GetReport.Rows.Count > 0)
                            {
                                QR.QtReport = (from dr in GetReport.AsEnumerable()
                                               select new QuotationDTO()
                                               {
                                                   QuotationNo = Convert.ToString(dr["QuoNo"]),
                                                   CustomerName = Convert.ToString(dr["CustoNm"]),
                                                   MobileNo = Convert.ToString(dr["MobileNo"]),
                                                   TotalQuantity = Convert.ToInt32(dr["TotalQty"]),
                                                   QuotationDate = Convert.ToDateTime(dr["QuoDt"]),
                                                   //companyId = Convert.ToInt32(dr["CompanyID"]),
                                                   Createdby = Convert.ToString(dr["CreatedBy"]),
                                                   status = Convert.ToInt32(dr["status"])
                                               }).ToList();

                            }
                        }
                    }
                    else
                    {
                        return Redirect("/home/login");
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(QR);

        }

        [HttpGet]
        public ActionResult GetItemList(Int32? BranchID, Int32? CompanyID)
        {
            List<QuotationModel> oItemList = new List<QuotationModel>();
            var getItemList = db.inv_itemsku.Where(em => em.CompanyId == CompanyID && em.BranchCode == BranchID).ToList();
            if (getItemList.Count > 0)
            {
                foreach (var item in getItemList)
                {
                    QuotationModel QM = new QuotationModel();
                    QM.ItemID = item.SKU;
                    QM.ItemName = item.ItemName;
                    oItemList.Add(QM);
                }
            }
            return Json(oItemList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Itemdetail(Int32? BranchID, Int32? CompanyID, String SKU)
        {
            QuotationModel QM = new QuotationModel();
            //var taxType = Convert.ToString(Session["taxType"]);
            DataTable getItemdetail = DataAccessLayer.GetDataTable("call CRM_ItemCurrentStock(" + CompanyID + "," + BranchID + ",'" + SKU + "')");
            if (getItemdetail.Rows.Count > 0)
            {
                if (Convert.ToDecimal(getItemdetail.Rows[0]["curstk"]) > 0)
                {
                    QM.CurrentStock = Convert.ToDecimal(getItemdetail.Rows[0]["curstk"]);
                }
                else
                {
                    QM.CurrentStock = 0.0M;
                }
                QM.ItemCode = getItemdetail.Rows[0]["SKU"] == null ? String.Empty : Convert.ToString(getItemdetail.Rows[0]["SKU"]);
                QM.UnitName = getItemdetail.Rows[0]["UnitID"] == null ? String.Empty : Convert.ToString(getItemdetail.Rows[0]["UnitID"]);
                QM.AltUnitName = string.IsNullOrWhiteSpace(Convert.ToString(getItemdetail.Rows[0]["AlternateUnit"])) ? QM.UnitName : Convert.ToString(getItemdetail.Rows[0]["AlternateUnit"]);
                QM.GstPercent = getItemdetail.Rows[0]["gstper"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["gstper"]);
                QM.ColorID = getItemdetail.Rows[0]["ColorID"] == null ? String.Empty : Convert.ToString(getItemdetail.Rows[0]["ColorID"]);
                QM.SizeID = getItemdetail.Rows[0]["SizeID"] == null ? String.Empty : Convert.ToString(getItemdetail.Rows[0]["SizeID"]);
                //QM.OurPrice = getItemdetail.Rows[0]["Rate"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["Rate"]);
                QM.OurPrice = getItemdetail.Rows[0]["saleprice_includedgst"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["saleprice_includedgst"]);
                QM.MRP = getItemdetail.Rows[0]["MRP"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["MRP"]);
                QM.Qty = 1;
                QM.TotalQty = 1;
                QM.DiscountType = getItemdetail.Rows[0]["DiscountType"] == null ? String.Empty : Convert.ToString(getItemdetail.Rows[0]["DiscountType"]);

                if (!string.IsNullOrEmpty(QM.DiscountType))
                {
                    if (QM.DiscountType == "amt")
                    {
                        QM.DiscountPercent = Math.Round(Convert.ToDecimal(((QM.MRP - QM.OurPrice) / QM.MRP) * 100), 2);
                        QM.DiscountAmount = getItemdetail.Rows[0]["mrpdiscunt"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["mrpdiscunt"]);
                    }
                    else if (QM.DiscountType == "per")
                    {
                        QM.DiscountPercent = getItemdetail.Rows[0]["mrpdiscunt"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["mrpdiscunt"]);
                        QM.DiscountAmount = getItemdetail.Rows[0]["mrpdiscunt"] == DBNull.Value ? 0 : ((QM.MRP / 100) * Convert.ToDecimal(getItemdetail.Rows[0]["mrpdiscunt"]));//calculate discount amount on mrp
                    }
                }
                else
                {
                    QM.DiscountPercent = getItemdetail.Rows[0]["mrpdiscunt"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["mrpdiscunt"]);
                    QM.DiscountAmount = getItemdetail.Rows[0]["mrpdiscunt"] == DBNull.Value ? 0 : ((QM.MRP / 100) * Convert.ToDecimal(getItemdetail.Rows[0]["mrpdiscunt"]));//calculate discount amount on mrp
                }


                QM.GstAmount = getItemdetail.Rows[0]["gstamt"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["gstamt"]);
                QM.AfterAmountDiscount = getItemdetail.Rows[0]["saleprice_includedgst"] == DBNull.Value ? 0 : Convert.ToDecimal(getItemdetail.Rows[0]["saleprice_includedgst"]) * 1;
            }
            return Json(QM, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetUnitList(Int32? BranchID, Int32? CompanyID)
        {
            String sCompanyID = Convert.ToString(CompanyID);
            List<QuotationModel> oUnitList = new List<QuotationModel>();
            var getItemList = db.com_measurementunits.Where(em => em.CompanyID == sCompanyID && em.BranchCode == BranchID).ToList();
            if (getItemList.Count > 0)
            {
                foreach (var item in getItemList)
                {
                    QuotationModel QM = new QuotationModel();
                    QM.UnitID = item.ID;
                    QM.UnitName = item.Unit;
                    oUnitList.Add(QM);
                }
            }
            return Json(oUnitList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAlternateUnit(Int32? BranchID, Int32? CompanyID)
        {
            List<QuotationModel> oUnitAlternateList = new List<QuotationModel>();
            DataTable getItemList = DataAccessLayer.GetDataTable("call crm_alternateunit(" + CompanyID + "," + BranchID + ")");
            if (getItemList.Rows.Count > 0)
            {
                for (int i = 0; i < getItemList.Rows.Count; i++)
                {
                    QuotationModel QM = new QuotationModel();
                    QM.AltUnitName = Convert.ToString(getItemList.Rows[i]["UnitID"]);
                    QM.AltUnitName = Convert.ToString(getItemList.Rows[i]["UnitName"]);
                    oUnitAlternateList.Add(QM);
                }
            }
            return Json(oUnitAlternateList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetColor(Int32? BranchID, Int32? CompanyID, string ColorId)
        {
            List<QuotationModel> oColorList = new List<QuotationModel>();

            if (!string.IsNullOrWhiteSpace(ColorId))
            {
                var splitedIds = ColorId.Split(',');

                foreach (var id in splitedIds)
                {
                    DataTable getItemList = DataAccessLayer.GetDataTable("call CRM_Color(" + CompanyID + "," + BranchID + ",'" + id + "')");
                    if (getItemList.Rows.Count > 0)
                    {
                        for (int i = 0; i < getItemList.Rows.Count; i++)
                        {
                            QuotationModel QM = new QuotationModel();
                            QM.ColorID = Convert.ToString(getItemList.Rows[i]["ColorID"]);
                            QM.ColorName = Convert.ToString(getItemList.Rows[i]["ColorName"]);
                            if (!oColorList.Any(a => a.ColorID == QM.ColorID && a.ColorName == QM.ColorName))
                                oColorList.Add(QM);
                        }
                    }
                }
            }

            return Json(oColorList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSize(Int32? BranchID, Int32? CompanyID, string SizeId)
        {
            List<QuotationModel> oSizeList = new List<QuotationModel>();

            if (!string.IsNullOrWhiteSpace(SizeId))
            {
                var splitedIds = SizeId.Split(',');

                foreach (var id in splitedIds)
                {
                    DataTable getItemList = DataAccessLayer.GetDataTable("call CRM_Size(" + CompanyID + "," + BranchID + ",'" + id + "')");

                    if (getItemList.Rows.Count > 0)
                    {
                        for (int i = 0; i < getItemList.Rows.Count; i++)
                        {
                            QuotationModel QM = new QuotationModel();
                            QM.SizeID = Convert.ToString(getItemList.Rows[i]["SizeID"]);
                            QM.SizeName = Convert.ToString(getItemList.Rows[i]["SizeName"]);
                            if (!oSizeList.Any(a => a.SizeID == QM.SizeID && a.SizeName == QM.SizeName))
                                oSizeList.Add(QM);
                        }
                    }

                    //var data = db.size_name.Where(a => a.Code == id && a.CompanyId == CompanyID.ToString() && a.BranchCode == BranchID.ToString()).FirstOrDefault();
                    //if(data!=null)
                    //{
                    //    QuotationModel QM = new QuotationModel();
                    //    QM.SizeID = data.Code;
                    //    QM.SizeName = data.Name;
                    //    oSizeList.Add(QM);
                    //}                   
                }
            }


            return Json(oSizeList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMobile(Int32? CustomerID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            String CompanyID = Convert.ToString(Session["CompanyID"]);
            var getCustomerMobile = db.customers.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID && em.ID == CustomerID).FirstOrDefault();
            return Json(getCustomerMobile.MobileNo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAddressList(Int32? CustomerID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            String CompanyID = Convert.ToString(Session["CompanyID"]);
            var getCustomerAddress = db.customers.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID && em.ID == CustomerID).FirstOrDefault();
            return Json(getCustomerAddress.BillingAddress, JsonRequestBehavior.AllowGet);
        }

        public string GetBillNo(Int32? BranchID, Int32? CompanyID)
        {
            string Billno = string.Empty;
            String BranchName = String.Empty;
            string Comname = "select upper(lpad(BranchName,4,0)) as BranchName from com_branches where OrgBranchCode = " + BranchID + "";
            DataTable DTDetail11 = DataAccessLayer.GetDataTable(Comname);
            if (!string.IsNullOrEmpty(Convert.ToString(DTDetail11.Rows[0]["BranchName"])))
            {
                BranchName = Convert.ToString(DTDetail11.Rows[0]["BranchName"]);
            }
            string Branches = "select count(QuoNo) as BranchCount from tbl_Quotation where CreatedBy='" + Convert.ToString(Session["UserName"]) + "' and BranchCode = " + BranchID + " and CompanyID = " + CompanyID + "";
            DataTable DTDetail111 = DataAccessLayer.GetDataTable(Branches);
            Int32? BranchCount = 0;
            if (!string.IsNullOrEmpty(Convert.ToString(DTDetail111.Rows[0]["BranchCount"])))
            {
                BranchCount = Convert.ToInt32(DTDetail111.Rows[0]["BranchCount"]);
            }

            Billno = BranchName + Convert.ToInt32(BranchID) + Convert.ToInt32(Session["UID"]) + "QOS" + (BranchCount + 1);
            return Billno;
        }

        public ActionResult GetCustomerDetails(Int32? CustomerID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            String CompanyID = Convert.ToString(Session["CompanyID"]);
            var cutomerData = new customer();
            var getCustomerMobile = db.customers.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID && em.ID == CustomerID).FirstOrDefault();
            if (getCustomerMobile != null)
            {
                cutomerData = getCustomerMobile;
            }
            return Json(cutomerData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Geteditor(int? id)
        {
            var msg = "";
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            crmtermcondition ITM = new crmtermcondition();
            try
            {
                if (id != 0)
                {
                    var getData = db.crm_termcondition.Where(em => em.Id == id && em.BranchId == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        ITM.TermCondition = getData.TermCondition;
                        ITM.Id = getData.Id;
                        ITM.orgId = getData.orgId;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return Json(ITM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TermCondition(int? id, int? orgID)
        {
            Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            string emailid = Convert.ToString(Session["UserEmail"]);
            crmtermcondition ITM = new crmtermcondition();
            var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
            if (comptype.Count > 0)
            {
                string Orglist = @"SELECT ID as orgId, Organization FROM company_profile WHERE ID = '" + CompanyID + "'";
                var getOrg = db.Database.SqlQuery<crmtermcondition>(Orglist).OrderBy(a => a.Organization).ToList();

                string Orglist2 = @"SELECT ID as orgId, Organization FROM company_profile WHERE mastercompany='" + CompanyID + "'";
                var getOrg2 = db.Database.SqlQuery<crmtermcondition>(Orglist2).OrderBy(a => a.Organization).ToList();

                var newlist = getOrg.Concat(getOrg2);
                ITM.joinlist = getOrg.Concat(getOrg2).ToList();
            }
            try
            {
                if (id != 0)
                {
                    var getData = db.crm_termcondition.Where(em => em.Id == id && em.BranchId == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        ITM.TermCondition = getData.TermCondition;
                        ITM.orgId = getData.orgId;
                        ITM.Organization = getData.orgname;
                        ITM.Id = getData.Id;
                    }
                }
                ViewBag.termconditionList = db.crm_termcondition.Where(em => em.BranchId == BranchID && em.CompanyId == CompanyID).ToList();

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return View(ITM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult TermCondition(crmtermcondition crtct, int? id, string TermCondition, int? orgID)
        {
            try
            {
                crmtermcondition ITM = new crmtermcondition();
                int BranchID = Convert.ToInt32(Session["BranchID"]);
                int CompanyID = Convert.ToInt32(Session["CompanyID"]);
                string emailid = Convert.ToString(Session["UserEmail"]);
                var comptype = db.company_profile.Where(e => e.mastercompany == CompanyID).ToList();
                if (comptype.Count > 0)
                {
                    string Orglist = @"SELECT ID as orgID, Organization FROM company_profile WHERE email='" + emailid + "'";
                    var getOrg = db.Database.SqlQuery<crmtermcondition>(Orglist).OrderBy(a => a.Organization).ToList();

                    string Orglist2 = @"SELECT ID as orgID, Organization FROM company_profile WHERE mastercompany='" + CompanyID + "'";
                    var getOrg2 = db.Database.SqlQuery<crmtermcondition>(Orglist2).OrderBy(a => a.Organization).ToList();

                    var newlist = getOrg.Concat(getOrg2);
                    ITM.joinlist = getOrg.Concat(getOrg2).ToList();
                }
                if (id == null)
                {

                    crm_termcondition ctc = new crm_termcondition();
                    ctc.TermCondition = crtct.TermCondition;
                    ctc.Status = true;
                    ctc.CompanyId = CompanyID;
                    ctc.BranchId = BranchID;
                    ctc.orgId = orgID;
                    var comname = db.company_profile.Where(a => a.mastercompany == CompanyID && a.ID == orgID).FirstOrDefault();
                    ctc.orgname = comname.Organization;
                    db.crm_termcondition.Add(ctc);
                    db.SaveChanges();
                    TempData["success"] = "Term and Condition added successfully";
                }
                else
                {
                    var getData = db.crm_termcondition.Where(em => em.Id == id && em.BranchId == BranchID && em.CompanyId == CompanyID).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.TermCondition = crtct.TermCondition;
                        getData.BranchId = BranchID;
                        getData.CompanyId = CompanyID;
                        getData.orgId = orgID;
                        var comname = db.company_profile.Where(a => a.ID == orgID).FirstOrDefault();
                        if (comname != null)
                        {
                            getData.orgname = comname.Organization;
                        }
                        //getData.Status = true;
                        db.SaveChanges();
                        TempData["success"] = "Term and Condition updated successfully";
                    }
                    else
                    {
                        TempData["alert"] = "Sorry no data found.";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Redirect("/Quotation/TermCondition");
        }

        public JsonResult UpdateQuotationstatus(string QuotationNo, Int32? QuotationStatus)
        {

            Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var msg = "";
            try
            {
                if (QuotationNo != null)
                {
                    var getData = db.tbl_quotation.Where(em => em.QuoNo == QuotationNo).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.status = QuotationStatus;
                        db.SaveChanges();
                        msg = "Status Update Successfully !!!";
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangetermStatus(int id)
        {
            var msg = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"update crm_termcondition set Status=case when Status=1 then 0 else 1 end where Id=" + id);
                msg = "ok";
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                ex.Message.ToString();
                msg = "err";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #region new code to show quotation invoice
        public ActionResult GetQuotationByQTNo(string QuotationNo)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == CompanyID && a.BranchCode == BranchID).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }

                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + CompanyID + " and BranchCode =" + BranchID + "";
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation('" + QuotationNo + "'," + CompanyID + "," + BranchID + ")");
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailByQuotNo", SOR);

        }

        public ActionResult GetQuotationByQTNo_p(string QuotationNo)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            string comid = "";
            int branchid = 0;
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == CompanyID && a.BranchCode == BranchID).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                    SOR.CreatedBy = qtbl.CreatedBy;
                    comid = qtbl.CompanyID;
                    branchid = Convert.ToInt32(qtbl.BranchCode);
                }
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + CompanyID + " and BranchCode =" + BranchID + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.com_branches.Where(e => e.OrganizationID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.Logo;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation('" + QuotationNo + "'," + comid + "," + branchid + ")");
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
                        rec.unit = Convert.ToString(GetDataSaleOrder.Rows[i]["unit"]);
                        rec.Qty = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["qty"]);
                        rec.Price = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["mrp"]);
                        rec.OurPrice = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Ourprice"]);
                        rec.RefNo = Convert.ToString(GetDataSaleOrder.Rows[i]["RefNo"]);
                        rec.DisPer = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["DisPer"]);
                        rec.GSTPer = GetDataSaleOrder.Rows[i]["gstper"] == DBNull.Value ? 0 : Convert.ToDecimal(GetDataSaleOrder.Rows[i]["gstper"]);
                        rec.GSTAmount = GetDataSaleOrder.Rows[i]["gstamt"] == DBNull.Value ? 0 : Convert.ToDecimal(GetDataSaleOrder.Rows[i]["gstamt"]);
                        rec.Delivery = GetDataSaleOrder.Rows[i]["Delivery"] == DBNull.Value ? string.Empty : string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(GetDataSaleOrder.Rows[i]["Delivery"]));
                        rec.CreatedBy = Convert.ToString(GetDataSaleOrder.Rows[i]["CreatedBy"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforPersonalclient", SOR);
        }

        public ActionResult GetQuotationByQTNo_ARIH(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            string comid = "";
            int branchid = 0;
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                    SOR.CreatedBy = qtbl.CreatedBy;
                    comid = qtbl.CompanyID;
                    branchid = Convert.ToInt32(qtbl.BranchCode);
                }
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.com_branches.Where(e => e.OrganizationID == multicomid).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.Logo;
                }
                //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                //var CurrentDate = Constant.GetBharatTime();

                #region get company detail
                DataTable GetCompanyDetail = DataAccessLayer.GetDataTable(" call SpCompanyDetail_smakit(" + multicomid + ")");
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
                                    CompanyAddress = Convert.ToString(dr["Address"]),
                                    PanNo = Convert.ToString(dr["PanNo"])
                                }).FirstOrDefault();

                    SOR.RegistrationNo = data.RegistrationNo;
                    SOR.CompanyEmail = data.CompanyEmail;
                    SOR.CompanyMobileNo = data.CompanyMobileNo;
                    SOR.CompanyAlternateNo = data.CompanyAlternateNo;
                    SOR.Organization = data.Organization;
                    SOR.CompanyAddress = data.CompanyAddress;
                    SOR.PanNo = data.PanNo;
                }
                #endregion
                var tblq = db.tbl_quotation.Where(e => e.QuoNo == QuotationNo).FirstOrDefault();
                if (tblq != null)
                {
                    SOR.customerId = Convert.ToInt32(tblq.customers_ID);
                }
                var cus = db.customers.Where(e => e.ID == SOR.customerId).FirstOrDefault();
                if (cus != null)
                {
                    SOR.customerpanNO = cus.AltMobileNo;
                }
                #region get sale order item detail
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_samkit('" + QuotationNo + "'," + multicomid + ")");
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
                        rec.unit = Convert.ToString(GetDataSaleOrder.Rows[i]["unit"]);
                        rec.Qty = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["qty"]);
                        rec.Price = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["mrp"]);
                        rec.OurPrice = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Ourprice"]);
                        rec.RefNo = Convert.ToString(GetDataSaleOrder.Rows[i]["RefNo"]);
                        rec.DisPer = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["DisPer"]);
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
                            decimal total = rec.Qty * rec.OurPrice;
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforARIH", SOR);
        }

        public ActionResult GetQuotationByQTNo_GOYM(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            string comid = "";
            int branchid = 0;
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                    SOR.CreatedBy = qtbl.CreatedBy;
                    comid = qtbl.CompanyID;
                    branchid = Convert.ToInt32(qtbl.BranchCode);
                }
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.com_branches.Where(e => e.OrganizationID == multicomid).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.Logo;
                }
                //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                //var CurrentDate = Constant.GetBharatTime();

                #region get company detail
                DataTable GetCompanyDetail = DataAccessLayer.GetDataTable(" call SpCompanyDetail_smakit(" + multicomid + ")");
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
                                    CompanyAddress = Convert.ToString(dr["Address"]),
                                    PanNo = Convert.ToString(dr["PanNo"])
                                }).FirstOrDefault();

                    SOR.RegistrationNo = data.RegistrationNo;
                    SOR.CompanyEmail = data.CompanyEmail;
                    SOR.CompanyMobileNo = data.CompanyMobileNo;
                    SOR.CompanyAlternateNo = data.CompanyAlternateNo;
                    SOR.Organization = data.Organization;
                    SOR.CompanyAddress = data.CompanyAddress;
                    SOR.PanNo = data.PanNo;
                }
                #endregion
                var tblq = db.tbl_quotation.Where(e => e.QuoNo == QuotationNo).FirstOrDefault();
                if (tblq != null)
                {
                    SOR.customerId = Convert.ToInt32(tblq.customers_ID);
                }
                var cus = db.customers.Where(e => e.ID == SOR.customerId).FirstOrDefault();
                if (cus != null)
                {
                    SOR.customerpanNO = cus.AltMobileNo;
                }
                #region get sale order item detail
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_samkit('" + QuotationNo + "'," + multicomid + ")");
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
                        rec.unit = Convert.ToString(GetDataSaleOrder.Rows[i]["unit"]);
                        rec.Qty = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["qty"]);
                        rec.Price = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["mrp"]);
                        rec.OurPrice = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Ourprice"]);
                        rec.RefNo = Convert.ToString(GetDataSaleOrder.Rows[i]["RefNo"]);
                        rec.DisPer = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["DisPer"]);
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
                            decimal total = rec.Qty * rec.OurPrice;
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforGOYM", SOR);
        }

        public ActionResult GetQuotationByQTNo_NEW5(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            string comid = "";
            int branchid = 0;
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                    SOR.CreatedBy = qtbl.CreatedBy;
                    comid = qtbl.CompanyID;
                    branchid = Convert.ToInt32(qtbl.BranchCode);
                }
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.com_branches.Where(e => e.OrganizationID == multicomid).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.Logo;
                }
                //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                //var CurrentDate = Constant.GetBharatTime();

                #region get company detail
                DataTable GetCompanyDetail = DataAccessLayer.GetDataTable(" call SpCompanyDetail_smakit(" + multicomid + ")");
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
                                    CompanyAddress = Convert.ToString(dr["Address"]),
                                    PanNo = Convert.ToString(dr["PanNo"])
                                }).FirstOrDefault();

                    SOR.RegistrationNo = data.RegistrationNo;
                    SOR.CompanyEmail = data.CompanyEmail;
                    SOR.CompanyMobileNo = data.CompanyMobileNo;
                    SOR.CompanyAlternateNo = data.CompanyAlternateNo;
                    SOR.Organization = data.Organization;
                    SOR.CompanyAddress = data.CompanyAddress;
                    SOR.PanNo = data.PanNo;
                }
                #endregion
                var tblq = db.tbl_quotation.Where(e => e.QuoNo == QuotationNo).FirstOrDefault();
                if (tblq != null)
                {
                    SOR.customerId = Convert.ToInt32(tblq.customers_ID);
                }
                var cus = db.customers.Where(e => e.ID == SOR.customerId).FirstOrDefault();
                if (cus != null)
                {
                    SOR.customerpanNO = cus.AltMobileNo;
                }
                #region get sale order item detail
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_samkit('" + QuotationNo + "'," + multicomid + ")");
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
                        rec.unit = Convert.ToString(GetDataSaleOrder.Rows[i]["unit"]);
                        rec.Qty = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["qty"]);
                        rec.Price = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["mrp"]);
                        rec.OurPrice = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Ourprice"]);
                        rec.RefNo = Convert.ToString(GetDataSaleOrder.Rows[i]["RefNo"]);
                        rec.DisPer = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["DisPer"]);
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
                            decimal total = rec.Qty * rec.OurPrice;
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforNEW5", SOR);
        }

        public ActionResult GetQuotationByQTNo_SNK4(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);

            string comid = "";
            int branchid = 0;
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                    SOR.CreatedBy = qtbl.CreatedBy;
                    comid = qtbl.CompanyID;
                    branchid = Convert.ToInt32(qtbl.BranchCode);
                }
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.com_branches.Where(e => e.OrganizationID == multicomid).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.Logo;
                }
                //Session["DpDateFormat"] = Constant.JsDateFormat(CompanyID);//get date picker date format by companyid
                //var CurrentDate = Constant.GetBharatTime();

                #region get company detail
                DataTable GetCompanyDetail = DataAccessLayer.GetDataTable(" call SpCompanyDetail_smakit(" + multicomid + ")");
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
                                    CompanyAddress = Convert.ToString(dr["Address"]),
                                    PanNo = Convert.ToString(dr["PanNo"])
                                }).FirstOrDefault();

                    SOR.RegistrationNo = data.RegistrationNo;
                    SOR.CompanyEmail = data.CompanyEmail;
                    SOR.CompanyMobileNo = data.CompanyMobileNo;
                    SOR.CompanyAlternateNo = data.CompanyAlternateNo;
                    SOR.Organization = data.Organization;
                    SOR.CompanyAddress = data.CompanyAddress;
                    SOR.PanNo = data.PanNo;
                }
                #endregion
                var tblq = db.tbl_quotation.Where(e => e.QuoNo == QuotationNo).FirstOrDefault();
                if (tblq != null)
                {
                    SOR.customerId = Convert.ToInt32(tblq.customers_ID);
                }
                var cus = db.customers.Where(e => e.ID == SOR.customerId).FirstOrDefault();
                if (cus != null)
                {
                    SOR.customerpanNO = cus.AltMobileNo;
                }
                #region get sale order item detail
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_samkit('" + QuotationNo + "'," + multicomid + ")");
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
                        rec.unit = Convert.ToString(GetDataSaleOrder.Rows[i]["unit"]);
                        rec.Price = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["mrp"]);
                        rec.OurPrice = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["Ourprice"]);
                        rec.RefNo = Convert.ToString(GetDataSaleOrder.Rows[i]["RefNo"]);
                        rec.DisPer = Convert.ToDecimal(GetDataSaleOrder.Rows[i]["DisPer"]);
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
                            decimal total = rec.Qty * rec.OurPrice;
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSNK4", SOR);
        }

        #endregion

        #region Different ViewQuotation format for shail company
        public ActionResult GetQuotationByQTNo_Sahilcompanysspower(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;

                }
                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}

                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == multicomid).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + multicomid + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanyLT", SOR);
        }

        public ActionResult GetQuotationByQTNo_SahilcompanyACE(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                }
                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + CompanyID + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + multicomid + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanyACE", SOR);
        }

        public ActionResult GetQuotationByQTNo_Sahilcompanysahilpower(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == CompanyID && a.BranchCode == BranchID).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.termcondition = qtbl.TermCondition;
                }

                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + CompanyID + " and BranchCode =" + BranchID + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + CompanyID + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanysahilpower", SOR);
        }

        public ActionResult GetQuotationByQTNo_Sahilcompanypowertrading(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                }
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + multicomid + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanypowertrading", SOR);
        }


        public ActionResult GetQuotationByQTNo_Sahilcompanyelectosupply(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                }
                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + " ";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + multicomid + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanyelectosupply", SOR);
        }

        public ActionResult GetQuotationByQTNo_Sahilcompanywolke9(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                }
                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + " ";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + multicomid + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanywolke9", SOR);
        }



        public ActionResult GetQuotationByQTNo_Sahilcompanyaceengineers(string QuotationNo, int multicomid)
        {
            int BranchID = Convert.ToInt32(Session["BranchID"]);
            int CompanyID = Convert.ToInt32(Session["CompanyID"]);
            var SOR = new SaleOrderVM();
            try
            {
                string DateFormat = Constant.DateFormat();//get date format by company id
                var taxtype = db.taxtypemasters.Where(a => a.companyID == multicomid).FirstOrDefault();//get company tax type
                if (taxtype != null)
                {
                    SOR.TaxTypeName = taxtype.TaxType;
                }
                var qtbl = db.tbl_quotation.Where(a => a.QuoNo == QuotationNo).FirstOrDefault();
                if (qtbl != null)
                {
                    SOR.QuoNo = qtbl.QuoNo;
                    SOR.QuoDt = qtbl.QuoDt;
                    SOR.CustomerName = qtbl.CustoNm;
                    SOR.termcondition = qtbl.TermCondition;
                }
                //var termcondition = db.crm_termcondition.Where(a => a.CompanyId == CompanyID && a.BranchId == BranchID && a.orgId == multicomid).FirstOrDefault();
                //if (termcondition != null)
                //{
                //    SOR.termcondition = termcondition.TermCondition;
                //}
                if (SOR.TaxTypeName.ToLower() == "total")
                {
                    decimal TotalTax = 0;

                    string qry = "select ifnull(sum(TaxAmt),0) TaxAmt from Sal_Quotaxdetails where GRNNo='" + QuotationNo + "' and CompanyID = " + multicomid + "";
                    TotalTax = db.Database.SqlQuery<decimal>(qry).FirstOrDefault();
                    SOR.TotalTax = TotalTax;
                }

                var comlogo = db.company_profile.Where(e => e.ID == CompanyID).FirstOrDefault();
                if (comlogo != null)
                {
                    SOR.imagepath = comlogo.FilePath;
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
                DataTable GetDataSaleOrder = DataAccessLayer.GetDataTable(" call GetData_Quotation_sahil('" + QuotationNo + "'," + multicomid + ")");
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
                        //rec.HSN = Convert.ToString(GetDataSaleOrder.Rows[i]["HSN"]);
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
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }
            return PartialView("_GetQuotationDetailsforSahilcompanyaceengineers", SOR);
        }
        #endregion

        //public ActionResult GetQuotationByQTNo(string QuotationNo)
        //{
        //    Int32 BranchID = Convert.ToInt32(Session["BranchID"]);
        //    Int32 CompanyID = Convert.ToInt32(Session["CompanyID"]);
        //    var  VCM = new QuotationReportVM();
        //    try
        //    {
        //        DataTable GetCompanyDetail  = DataAccessLayer.GetDataTable(" call CRM_CompanyBranchDetail(" + CompanyID + ","+ BranchID + ")");
        //        if (GetCompanyDetail.Rows.Count > 0)
        //        {
        //            VCM = (from dr in GetCompanyDetail.AsEnumerable()
        //                   select new QuotationReportVM()
        //                   {
        //                       Organization = dr["Organization"].ToString(),
        //                       BranchName = dr["BranchName"].ToString(),
        //                       RegistrationNo = dr["RegistrationNo"].ToString(),
        //                       CompanyMobileNo = dr["Phoneno"].ToString(),
        //                       CompanyEmail = Convert.ToString(dr["Email"]),
        //                       CompanyAddress = dr["Address"].ToString(),
        //                       StateId= Convert.ToInt32(dr["StateId"]),
        //                       StateName= Convert.ToString(dr["State"])
        //                   }).FirstOrDefault();
        //        }

        //        //decimal? CGST = 0;
        //        //decimal? SGST = 0;
        //        //decimal? IGST = 0;
        //        //decimal GST = 0;
        //        if(!string.IsNullOrEmpty(VCM.StateName))
        //        {
        //            var firstRecord = db.tbl_quotation.Where(tq => tq.QuoNo == QuotationNo && tq.CompanyID == CompanyID.ToString() && tq.BranchCode == BranchID).FirstOrDefault();
        //            if (firstRecord != null)
        //            {
        //                VCM.CustomerName = firstRecord.CustoNm;
        //                VCM.CustomerMobileNo = firstRecord.MobileNo;
        //                VCM.CustomerAddress = firstRecord.Address;
        //                VCM.QuotationNo = QuotationNo;
        //                VCM.QuotationDate = firstRecord.QuoDt.Value;
        //                VCM.PlaceOfSupply = firstRecord.Shift;
        //                VCM.TaxTypeName = firstRecord.RefNo;

        //                #region set textypename
        //                if (string.IsNullOrEmpty(firstRecord.RefNo))
        //                {
        //                    if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == VCM.StateName)
        //                    {
        //                        VCM.TaxTypeName = "CGST+SGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "35-Andaman & Nicobar Islands")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "04-Chandigarh")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "26-Dadra & Nagar Haveli")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "25-Daman & Diu")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "31-Lakshdweep")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "34-Pondicherry")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift == "01-Jammu & Kashmir")
        //                    {
        //                        VCM.TaxTypeName = "CGST+UGST";
        //                    }
        //                    else
        //                    {
        //                        VCM.TaxTypeName = "IGST";
        //                    }
        //                }
        //                #endregion

        //                #region set gst/igst percent
        //                //if (!string.IsNullOrEmpty(firstRecord.Shift) && firstRecord.Shift==VCM.StateName)
        //                //{
        //                //    CGST = 9;
        //                //    SGST = 9;
        //                //    GST = 18;
        //                //}
        //                //else
        //                //{
        //                //    IGST = 18;
        //                //    GST = 18;
        //                //}
        //                #endregion
        //            }
        //        }

        //        #region Get Item-List

        //        var qItemList = new List<QuotationItemDetail>();
        //        var data = db.tbl_quotation.Where(tq => tq.QuoNo == QuotationNo && tq.CompanyID == CompanyID.ToString() && tq.BranchCode == BranchID).ToList();
        //        foreach (var tq in data) 
        //        {
        //            var qi = new QuotationItemDetail();
        //            if (VCM.TaxTypeName == "IGST")
        //            {

        //                qi.IGSTPer = tq.gstper > 0 ? tq.gstper : 0;
        //                qi.IGSTAmount = tq.gstamt > 0 ? tq.gstamt : 0;
        //            }
        //            else
        //            {
        //                qi.CGSTPer = tq.gstper > 0 ? tq.gstper / 2 : 0;
        //                qi.SGSTPer = tq.gstper > 0 ? tq.gstper / 2 : 0;
        //                qi.CGSTAmount = tq.gstamt > 0 ? Math.Round(Convert.ToDecimal(tq.gstamt / 2), 2) : 0;
        //                qi.SGSTAmount = tq.gstamt > 0 ? Math.Round(Convert.ToDecimal(tq.gstamt / 2), 2) : 0;
        //            }
        //            qi.ItemName = tq.Particular;
        //            qi.Qty = tq.TotalQty;
        //            qi.UnitPrice = tq.Price;

        //            qi.NetTotalAmt = tq.totalamt;

        //            qi.Total = tq.totalamt;
        //            //qi.CGSTAmount = CGST > 0 ? Math.Round(Convert.ToDecimal((tq.totalamt * CGST) / 100), 2) : 0;
        //            //qi.SGSTAmount = SGST > 0 ? Math.Round(Convert.ToDecimal((tq.totalamt * SGST) / 100), 2) : 0;
        //            //qi.IGSTAmount = IGST > 0 ? Math.Round(Convert.ToDecimal((tq.totalamt * IGST) / 100), 2) : 0;
        //            //qi.Total = Math.Round(Convert.ToDecimal(tq.totalamt + ((tq.totalamt * GST) / 100)), 2);
        //            qItemList.Add(qi);

        //        }

        //        VCM.QtItemList = qItemList;

        //        if (VCM.QtItemList.Count>0)
        //        {
        //            VCM.GrandTotalAmt = VCM.QtItemList.Sum(a => a.Total);
        //            VCM.AmountInWord = NumberToWord(Convert.ToInt32(VCM.GrandTotalAmt));
        //        }

        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendExcepToDB(ex);
        //    }

        //    return PartialView("_QuoatationDetailInfoByQtNo", VCM);
        //}

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
        //public void setgsttype()
        // {
        //     string gsttype = "";
        //     string stID = "";
        //     try
        //     {
        //         var getbranch = db.com_branches.Where(em => em.OrgBranchCode == Authorised.Branch).FirstOrDefault();
        //         if (getbranch != null)
        //         {
        //             stID = getbranch.StateId.ToString();
        //             if (cbostate.SelectedValue.ToString() == getbranch.StateId)
        //             {
        //                 gsttype = "CGST+SGST";
        //             }
        //             else if (cbostate.Text == "35-Andaman & Nicobar Islands")
        //             {
        //                 gsttype = "CGST+UGST";
        //             }
        //             else if (cbostate.Text == "04-Chandigarh")
        //             {
        //                 gsttype = "CGST+UGST";
        //             }
        //             else if (cbostate.Text == "26-Dadra & Nagar Haveli")
        //             {
        //                 gsttype = "CGST+UGST";
        //             }
        //             else if (cbostate.Text == "25-Daman & Diu")
        //             {
        //                 gsttype = "CGST+UGST";
        //             }
        //             else if (cbostate.Text == "31-Lakshdweep")
        //             {
        //                 gsttype = "CGST+UGST";
        //             }
        //             else if (cbostate.Text == "34-Pondicherry")
        //             {
        //                 gsttype = "CGST+UGST";
        //             }
        //             else
        //             {
        //                 gsttype = "IGST";
        //             }
        //         }
        //     }
        //     catch (Exception)
        //     {
        //     }
        // }
    }
}
