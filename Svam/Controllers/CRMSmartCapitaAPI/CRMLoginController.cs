using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;
using Svam._Class;
using System;
using Svam.Models;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using Svam.UtilityManager;
using Svam.Models.DTO;
using Traders.Models;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMLoginController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities(); 
        #endregion
       
        #region Get Check Login Crediential
        /// <summary>
        ///  Check Login Crediential, 
        ///  Also the Subscription of User
        ///  if Subscription is ok, it will login and save the attendnce of user of particular day
        ///  GET api/CRMLogin/?userName=arunaw@123&password=admin
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string userName, string password)
        {
            string message = string.Empty;
            CRMUserModel CRMUM = new CRMUserModel();
            try
            {
                #region password decryption

                string loginPwd = "";
                //var userData = new List<UserCredential
                var userData = (from ur in db.crm_usertbl
                                where (ur.UserName == userName ||
                                ur.Email == userName) && ur.Status == true
                                select new UserCredential
                                {
                                    UserName = ur.UserName,
                                    Email = ur.Email,
                                    Password = ur.Password,
                                    KeyVersion = ur.KeyVersion
                                }).ToList();
                //string query = @"select UserName,Email,Password,KeyVersion from crm_usertbl where username='" + userName + "'or Email='" + userName + "'and Status=1";
                //var userData = db.Database.SqlQuery<UserCredential>(query).ToList();

                if (userData != null && userData.Count > 0)
                {
                    for (int i = 0; i < userData.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(userData[i].KeyVersion))
                        {
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string encPwd = EncriptAES.EncryptString(password, key, iv1);

                            if ((userData[i].UserName.ToLower() == userName.ToLower() || userData[i].Email.ToLower() == userName.ToLower()) && userData[i].Password == encPwd)
                            {
                                loginPwd = encPwd;

                                break;
                            }
                            //else if (userData[i].Email == userName && userData[i].Password == encPwd)
                            //{
                            //    loginPwd = encPwd;
                            //    break;
                            //}
                        }
                        else
                        {
                            if ((userData[i].UserName.ToLower() == userName.ToLower() || userData[i].Email.ToLower() == userName.ToLower()) && userData[i].Password == password)
                            {
                                loginPwd = password;
                                break;
                            }
                            //else if (userData[i].Email == userName && userData[i].Password == password)
                            //{
                            //    loginPwd = password;
                            //    break;
                            //}
                        }
                    }
                }
                #endregion
                crm_usertbl u = new crm_usertbl();
                u = db.crm_usertbl.Where(em => (em.UserName == userName || em.Email == userName) && em.Password == loginPwd).FirstOrDefault();
                if (u == null)
                {
                    message = string.Format("** Username and Password does not exits,Please contact to administrator **");
                }
                else
                {
                    if (u.Status == true)
                    {
                        
                        CRMUM.Id = u.Id;
                        CRMUM.UserName = Convert.ToString(u.UserName);
                        CRMUM.Fname = Convert.ToString(u.Fname);
                        CRMUM.Lname = Convert.ToString(u.Lname);
                        CRMUM.BranchID = Convert.ToInt32(u.BranchID);
                        CRMUM.CompanyID = Convert.ToInt32(u.CompanyID);
                        CRMUM.ProfileName = u.ProfileName;
                        CRMUM.TimeZone = u.TimeZone;
                        CRMUM.Email = u.Email;

                        string qry = @"select cp.Country as CountryID,cntry.country_name as CountryName from company_profile cp
                                       join acc_countries cntry on cp.Country=cntry.id where cp.ID=" + u.CompanyID;
                        var country = db.Database.SqlQuery<ManageCountryModel>(qry).FirstOrDefault();

                        if(country!=null)
                        {
                            CRMUM.CountryID = country.CountryID;
                            CRMUM.CountryName = country.CountryName;
                        }

                        CRMUM.DateFormat= Constant.DateFormatForApi(Convert.ToInt32(u.CompanyID));//get date format

                        if (!string.IsNullOrEmpty(u.KeyVersion))
                        {
                            #region password decryption
                            byte[] iv1;
                            byte[] key = EncriptAES.getdcriptkey(out iv1);
                            string decryptPwd = EncriptAES.DecryptString(u.Password, key, iv1);
                            #endregion
                            CRMUM.Password = decryptPwd;
                        }
                        else
                        {
                         CRMUM.Password = u.Password;
                        }
                       
                        CRMUM.Token = Utility.TokenGenerator(Convert.ToInt32(u.CompanyID));//generate token for access all service

                        DateTime dt  = Constant.GetimeForApi(Convert.ToInt32(u.CompanyID));

                        #region GET Expire start and End Date format in 'yyyy-MM-dd'
                        string base64StartDate = EncodeDecodeForBase64.DecodeBase64(u.StartDate);
                        string base64EndDate = EncodeDecodeForBase64.DecodeBase64(u.EndDate);
                        DateTime ExpireStarDate = DateTime.ParseExact(base64StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        DateTime ExpireEndDate = DateTime.ParseExact(base64EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        #endregion

                        if (u.ProfileName == "SuperAdmin" &&( u.IsActive == true || u.IsActive == false) && ( u.IsExpired == true || u.IsExpired == false) && ExpireStarDate.Date <= dt.Date && ExpireEndDate.Date <= dt.Date)
                        {
                           message = string.Format("** Subscription is expired, Please contact to administrator.!"); 
                            
                        }
                        else if (u.ProfileName != "SuperAdmin" && (u.IsActive == true || u.IsActive == false) && (u.IsExpired == true || u.IsExpired == false) && ExpireStarDate.Date <= dt.Date && ExpireEndDate.Date <= dt.Date)
                        {
                            message = string.Format("** Subscription is expired, Please contact to administrator.!");                            
                        }                
                    }
                    else
                    {
                        message = string.Format("** Username or e-mail is InActive, Please contact to administrator.!");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
                var errMsg = "** Something went wrong.!";
                HttpError err = new HttpError(errMsg);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }

            if (!string.IsNullOrEmpty(message))
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.OK, err);
            }
            else
            {
                #region Get-MAC-Address and IP Address
                var mac = GetMACAddress();
                if (mac != string.Empty)
                {
                    mac = Regex.Replace(mac, ".{2}", "$0-").TrimEnd('-');
                }
                string userIpAddress = HttpContext.Current.Request.UserHostAddress;
                #endregion

                //DateTime utcTime = DateTime.UtcNow;
                //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                DateTime localTime = Constant.GetimeForApi(Convert.ToInt32(CRMUM.CompanyID));
                var time = localTime.ToString("hh:mm:ss tt");
                var Date = localTime.ToString("dd/MM/yyyy");
                var CheckExistAttand = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == CRMUM.Id && em.L_In_Date == Date && em.BranchID == CRMUM.BranchID && em.CompanyID == CRMUM.CompanyID).FirstOrDefault();
                if (CheckExistAttand == null)
                {
                    crm_tbl_nipl_emp_attendance LogAtt = new crm_tbl_nipl_emp_attendance();
                    LogAtt.EmpId = CRMUM.Id;
                    LogAtt.L_In_Date = Date;
                    LogAtt.L_In_Time = time;
                    LogAtt.Status = "P";
                    LogAtt.IPAddress = userIpAddress;
                    LogAtt.MacAddress = mac;
                    LogAtt.LogTimeZone = CRMUM.TimeZone;
                    LogAtt.Working_Late_Night = false;
                    LogAtt.Extra_working = false;
                    LogAtt.CompanyID = CRMUM.CompanyID;
                    LogAtt.BranchID = CRMUM.BranchID;
                    db.crm_tbl_nipl_emp_attendance.Add(LogAtt);
                    db.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, CRMUM);
            }           
        }
        #endregion

        #region Get Mac Address
        /// <summary>
        /// Get MAC Address
        /// </summary>
        /// <returns></returns>
        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        } 
        #endregion
    }
}