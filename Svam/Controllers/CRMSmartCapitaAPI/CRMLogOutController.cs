using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Svam.EF;
using System;
using Svam.Models;
using System.Net;
using Svam.UtilityManager;
using Svam.Models.DTO;

namespace Svam.Controllers.CRMSmartCapitaAPI
{
    public class CRMLogOutController : ApiController
    {
        #region Define Entity Class
        niscrmEntities db = new niscrmEntities(); 
        #endregion

        #region Get Check SignOut Crediential
        /// <summary>
        ///  //GET api/CRMLogOut/?userName=arunaw@123&password=admin
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string userName, string password)
        {
            string message = string.Empty;
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
                        //DateTime utcTime = DateTime.UtcNow;
                        //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        //DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
                        DateTime localTime = Constant.GetimeForApi(Convert.ToInt32(u.CompanyID));

                        var time = localTime.ToString("hh:mm:ss tt");
                        var Date = localTime.ToString("dd/MM/yyyy");
                        var GetAttandData = db.crm_tbl_nipl_emp_attendance.Where(em => em.EmpId == u.Id && em.L_In_Date == Date && em.BranchID == u.BranchID && em.CompanyID == u.CompanyID).FirstOrDefault();
                        if (GetAttandData != null)
                        {
                            var lgtime = GetAttandData.L_In_Date + " " + GetAttandData.L_In_Time;
                            DateTime logt = Convert.ToDateTime(lgtime);
                            DateTime dt = localTime;
                            TimeSpan duration = (dt - logt);
                            string dur = string.Format("{0:00}:{1:00}:{2:00}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
                            GetAttandData.L_Out_Date = Date;
                            GetAttandData.L_Out_Time = time;
                            GetAttandData.Duration = dur;
                            GetAttandData.BranchID = u.BranchID;
                            GetAttandData.CompanyID = u.CompanyID;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendExcepToDB(ex);
            }

            if (message != string.Empty)
            {
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, err);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Sign Out of CRM SmartCapita");
            }
        }
        #endregion
    }
}
