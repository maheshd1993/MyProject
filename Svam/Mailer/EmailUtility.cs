using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography.X509Certificates;
using Svam.EF;
using System.IO;
using Svam.Models;

namespace Traders.Mailer
{
    public class EmailUtility
    {
        public static bool SendEmail(string mailto, string subject, string Message, string cc, Int32 CompanyID, Int32 BranchID)
        {
            bool EmailSent = false;
            try
            {
                using (System.Net.Mail.SmtpClient s = new System.Net.Mail.SmtpClient())
                {
                    niscrmEntities db = new niscrmEntities();
                    var getEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                    System.Net.NetworkCredential NetworkCredential = new System.Net.NetworkCredential();
                    NetworkCredential.Password = getEmailSetting.Password;
                    NetworkCredential.UserName = getEmailSetting.EmailAddress;
                    System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage(getEmailSetting.EmailAddress, getEmailSetting.DisplayName);
                    mailmsg.Body = Message;
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailmsg.Body, null, "text/html");
                    mailmsg.AlternateViews.Add(htmlView);
                    mailmsg.Subject = subject;
                    mailmsg.IsBodyHtml = true;
                    mailmsg.To.Add(mailto);
                    if (getEmailSetting.CCEmail != null && getEmailSetting.CCEmail != "")
                    {
                        mailmsg.CC.Add(getEmailSetting.CCEmail);
                    }
                    mailmsg.From = new System.Net.Mail.MailAddress(getEmailSetting.EmailAddress, getEmailSetting.DisplayName);
                    s.UseDefaultCredentials = false;
                    s.Credentials = NetworkCredential;
                    s.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    s.EnableSsl = Convert.ToBoolean(getEmailSetting.SSL);
                    s.Port = Convert.ToInt32(getEmailSetting.Port);
                    s.Host = getEmailSetting.SMTPHost;
                    s.Send(mailmsg);
                    mailmsg = null;
                    s.Dispose();
                    mailmsg.Dispose();
                }
            }
            catch
            {
                EmailSent = false;
            }

            return EmailSent;
        }

        public static bool SendMailAttachment(string mailto, string subject, string Message, string FileName, string Provider)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.From = new MailAddress(WebConfigurationManager.AppSettings["SMTPuser"], WebConfigurationManager.AppSettings["SMTPUserName"].ToString());
                mail.To.Add(mailto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = Message;
                //attach file
                if (Provider == "UPS")
                {
                    string[] arrayFile = FileName.Split(';');
                    mail.Attachments.Add(new Attachment(arrayFile[0]));
                    mail.Attachments.Add(new Attachment(arrayFile[1]));
                }
                else
                {
                    string[] arrayFile = FileName.Split(';');
                    mail.Attachments.Add(new Attachment(arrayFile[0]));
                }

                SmtpClient smtp = new SmtpClient(WebConfigurationManager.AppSettings["SMTPserver"], 25);
                smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["SMTPuser"], WebConfigurationManager.AppSettings["SMTPpassword"]);
                smtp.EnableSsl = true;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) 
                { return true; };
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CRMSendEmailAttachment(string mailto, string subject, string Message, HttpPostedFileBase fileUploader,Int32 CompanyID,Int32 BranchID,out string mgs)
        {
            //String ShowMsg = String.Empty;
            //bool EmailSent = false;
            try
            {
                niscrmEntities db = new niscrmEntities();
                var getEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(getEmailSetting.SMTPHost, Convert.ToInt32(getEmailSetting.Port));
                mail.From = new MailAddress(getEmailSetting.EmailAddress, getEmailSetting.DisplayName);
                mail.To.Add(mailto);
                mail.Subject = subject;
                mail.Body = Message;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                mail.AlternateViews.Add(htmlView);
                mail.IsBodyHtml = true;
                if (getEmailSetting.CCEmail != string.Empty)
                {
                    mail.CC.Add(getEmailSetting.CCEmail);
                }
                if (fileUploader !=null)
                {
                    string fileName = Path.GetFileName(fileUploader.FileName);
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(fileUploader.InputStream, fileName);
                    mail.Attachments.Add(attachment);
                }
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                SmtpServer.UseDefaultCredentials = true;
                //SmtpServer.Port = Convert.ToInt32(getEmailSetting.Port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(getEmailSetting.EmailAddress, getEmailSetting.Password);
                SmtpServer.EnableSsl = Convert.ToBoolean(getEmailSetting.SSL);
                SmtpServer.Send(mail); 
                SmtpServer.Dispose();
                mail.Dispose();
                //EmailSent = true;
                mgs = "True";
            }
            catch(Exception ex)
            {
                mgs = ex.Message;               
                //EmailSent = false;
            }
            return mgs;
        }

        public static string sCRMSendEmailAttachment(string mailto, string subject, string Message, string fileUploader, Int32 CompanyID, Int32 BranchID, out string mgs)
        {
            //String ShowMsg = String.Empty;
            //bool EmailSent = false;
            try
            {
                niscrmEntities db = new niscrmEntities();
                var getEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(getEmailSetting.SMTPHost, Convert.ToInt32(getEmailSetting.Port));
                mail.From = new MailAddress(getEmailSetting.EmailAddress, getEmailSetting.DisplayName);
                mail.To.Add(mailto);
                mail.Subject = subject;
                mail.Body = Message;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mail.Body, null, "text/html");
                mail.AlternateViews.Add(htmlView);
                mail.IsBodyHtml = true;
                if (getEmailSetting.CCEmail != string.Empty)
                {
                    mail.CC.Add(getEmailSetting.CCEmail);
                }
                if (fileUploader != null)
                {
                    string fileName = Path.GetFileName(fileUploader);

                    System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                    contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    contentType.Name = fileName;
                   
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(System.Web.HttpContext.Current.Server.MapPath(fileUploader), contentType);
                    mail.Attachments.Add(attachment);
                }
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.OnSuccess;
                SmtpServer.UseDefaultCredentials = true;
                //SmtpServer.Port = Convert.ToInt32(getEmailSetting.Port);
                SmtpServer.Credentials = new System.Net.NetworkCredential(getEmailSetting.EmailAddress, getEmailSetting.Password);
                SmtpServer.EnableSsl = Convert.ToBoolean(getEmailSetting.SSL);
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                //EmailSent = true;
                mgs = "True";
            }
            catch (Exception ex)
            {
                mgs = ex.Message;
                //EmailSent = false;
            }
            return mgs;
        }

        public static bool SendTicketEmailToCustomer(string mailto, string subject, string Message, int CompanyID, int BranchID)
        {
            bool EmailSent = true;
            try
            {
                using (System.Net.Mail.SmtpClient s = new System.Net.Mail.SmtpClient())
                {
                    string mailfrom = string.Empty;
                    string userName= string.Empty;
                    string pasword= string.Empty;
                    string displayName = string.Empty;
                    string host= string.Empty;
                    int port = 587;
                    bool ssl = true;
                    if(CompanyID==296)
                    {
                         mailfrom = "info@smartcapita.com";
                         userName = "AKIATR34ERRAV53POMOR";
                         pasword = "BHmh2uFTDbpGlCtLfLhAuXO/j5R4BNQTwBiNxzYvqgIC";
                         displayName = "Smart Capita";
                         host = "email-smtp.ap-south-1.amazonaws.com";
                         port = 587;                       
                    }
                    else
                    {
                        niscrmEntities db = new niscrmEntities();
                        var getEmailSetting = db.crm_emailsetting.Where(em => em.CompanyID == CompanyID && em.BranchCode == BranchID).FirstOrDefault();
                        if(getEmailSetting!=null)
                        {
                            mailfrom = getEmailSetting.EmailAddress;
                            userName = getEmailSetting.UserName;
                            pasword =  getEmailSetting.Password;
                            displayName = !string.IsNullOrEmpty(getEmailSetting.DisplayName) ? getEmailSetting.DisplayName : getEmailSetting.EmailAddress;
                            host = getEmailSetting.SMTPHost;
                            port = getEmailSetting.Port==null ? 587 : Convert.ToInt32(getEmailSetting.Port);
                            ssl = getEmailSetting.SSL==null ? false : Convert.ToBoolean(getEmailSetting.SSL);
                        }
                    }

                    if(!string.IsNullOrEmpty(mailfrom) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pasword) && !string.IsNullOrEmpty(host) && port>0)
                    {
                        System.Net.NetworkCredential NetworkCredential = new System.Net.NetworkCredential();

                        NetworkCredential.UserName = userName;
                        NetworkCredential.Password = pasword;

                        System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage(mailfrom, mailto);
                        mailmsg.Body = Message;
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailmsg.Body, null, "text/html");
                        mailmsg.AlternateViews.Add(htmlView);
                        mailmsg.Subject = subject;
                        mailmsg.IsBodyHtml = true;
                        mailmsg.To.Add(mailto);
                        //if (getEmailSetting.CCEmail != null && getEmailSetting.CCEmail != "")
                        //{
                        //    mailmsg.CC.Add(getEmailSetting.CCEmail);
                        //}
                        mailmsg.From = new System.Net.Mail.MailAddress(mailfrom, displayName);
                        s.UseDefaultCredentials = false;
                        s.Credentials = NetworkCredential;
                        s.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        s.EnableSsl = ssl;
                        s.Port = port;
                        s.Host = host;
                        s.Send(mailmsg);
                        s.Dispose();
                        mailmsg.Dispose();
                    }
                    else
                    {
                        EmailSent = false;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendExcepToDB(ex);
                EmailSent = false;
            }

            return EmailSent;
        }

        public static bool DeleteleadSendEmailOTP(string mailto, string subject, string Message, int CompanyID, int BranchID)
        {
            bool EmailSent = true;
            try
            {
                using (System.Net.Mail.SmtpClient s = new System.Net.Mail.SmtpClient())
                {
                    string mailfrom = string.Empty;
                    string userName = string.Empty;
                    string pasword = string.Empty;
                    string displayName = string.Empty;
                    string host = string.Empty;
                    int port = 587;
                    bool ssl = true;
                    mailfrom = "info@smartcapita.com";
                    userName = "AKIATR34ERRAZWOKRMMP";
                    pasword = "BPaiIgS5HRhShZDw6sXA6gjAxhMA4f10T7BhA9U/hIkW";
                    displayName = "Smart Capita";
                    host = "email-smtp.ap-south-1.amazonaws.com";
                    port = 587;
                    if (!string.IsNullOrEmpty(mailfrom) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pasword) && !string.IsNullOrEmpty(host) && port > 0)
                    {
                        System.Net.NetworkCredential NetworkCredential = new System.Net.NetworkCredential();

                        NetworkCredential.UserName = userName;
                        NetworkCredential.Password = pasword;

                        System.Net.Mail.MailMessage mailmsg = new System.Net.Mail.MailMessage(mailfrom, mailto);
                        mailmsg.Body = Message;
                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailmsg.Body, null, "text/html");
                        mailmsg.AlternateViews.Add(htmlView);
                        mailmsg.Subject = subject;
                        mailmsg.IsBodyHtml = true;
                        mailmsg.To.Add(mailto);
                        mailmsg.From = new System.Net.Mail.MailAddress(mailfrom, displayName);
                        s.UseDefaultCredentials = false;
                        s.Credentials = NetworkCredential;
                        s.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        s.EnableSsl = ssl;
                        s.Port = port;
                        s.Host = host;
                        s.Send(mailmsg);
                        s.Dispose();
                    }
                    else
                    {
                        EmailSent = false;
                    }
                }
            }
            catch (Exception ex)
            {
                EmailSent = false;
            }
            return EmailSent;
        }

    }
}