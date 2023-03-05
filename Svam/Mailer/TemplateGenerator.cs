using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace Traders.Mailer
{
    public class TemplateGenerator
    {
        //Aipartner Mailer Formate......................................
        static string HREMail = "aaradhanachauhan@nicoleinfosoft.com";
        #region New-Order
        public static string ExtraWorkingTemplate(string UserName, string UserEmail, string Date, string LoginTime, string LogoutTime, Int32 CompanyID, Int32 BranchID)
        {
            string htmlTemplate = HttpContext.Current.Server.MapPath("~/MailTemplate/ExtraWorking.html");
            StreamReader sReader = new StreamReader(htmlTemplate);
            string htmlBody = sReader.ReadToEnd();
            htmlBody = htmlBody.Replace("##UserName##", UserName);
            htmlBody = htmlBody.Replace("##Date##", Date);
            htmlBody = htmlBody.Replace("##UserEmail##", UserEmail);
            htmlBody = htmlBody.Replace("##LoginTime##", LoginTime);
            htmlBody = htmlBody.Replace("##LogOutTime##", LogoutTime);
            EmailUtility.SendEmail(HREMail, "Extra working on non-working day", htmlBody, "stiwari@nicoleinfosoft.com", CompanyID, BranchID);
            return htmlBody;
        }
        public static string LateNightWorkingTemplate(string UserName,string UserEmail,string Date,string LogoutTime,Int32 CompanyID, Int32 BranchID)
        {
            string htmlTemplate = HttpContext.Current.Server.MapPath("~/MailTemplate/LateNightWorking.html");
            StreamReader sReader = new StreamReader(htmlTemplate);
            string htmlBody = sReader.ReadToEnd(); 
            htmlBody = htmlBody.Replace("##UserName##", UserName);
            htmlBody = htmlBody.Replace("##UserEmail##", UserEmail);
            htmlBody = htmlBody.Replace("##Date##", Date);
            htmlBody = htmlBody.Replace("##Time##", LogoutTime);
            EmailUtility.SendEmail(HREMail, "Late Night Working", htmlBody, "stiwari@nicoleinfosoft.com", CompanyID, BranchID);
            return htmlBody;
        }
        #endregion        
    }
}