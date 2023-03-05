using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System;

namespace Svam.UtilityManager
{
    public class Utility
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath)  
        {  
            DataTable dt = new DataTable();  
            using (StreamReader sr = new StreamReader(strFilePath))  
            {  
                string[] headers = sr.ReadLine().Split(',');  
                foreach (string header in headers)  
                {  
                    dt.Columns.Add(header);  
                }  
  
                while (!sr.EndOfStream)  
                {  
                    string[] rows = sr.ReadLine().Split(',');  
                    if (rows.Length > 1)  
                    {  
                        DataRow dr = dt.NewRow();  
                        for (int i = 0; i < headers.Length; i++)  
                        {  
                            dr[i] = rows[i].Trim();  
                        }  
                        dt.Rows.Add(dr);  
                    }  
                } 
            } 
            return dt;  
        }

         public static DataTable ConvertXSLXOrXLStoDataTable(string strFilePath, string connString)
         {
             OleDbConnection oledbConn = new OleDbConnection(connString);
             DataTable dt = new DataTable();
             try
             {
                 oledbConn.Open();
                 using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                 {
                     OleDbDataAdapter oleda = new OleDbDataAdapter();
                     oleda.SelectCommand = cmd;
                     DataSet ds = new DataSet();
                     oleda.Fill(ds);

                     dt = ds.Tables[0];
                 }
             }
             catch(Exception ex)
             {
                 throw ex;
             }
             finally
             {
                 oledbConn.Close();
             }
             return dt;
         } 

        public static string TokenGenerator(int companyId)
        {
            string token = "";
            byte[] iv1;
            byte[] key = EncriptAES.getdcriptkey(out iv1);
            string encPwd = EncriptAES.EncryptString(companyId.ToString(), key, iv1);
            token = encPwd;
            return token;
        }

        public static bool TokenVerify(int companyId,string token)
        {
            bool isExist= false;
            byte[] iv1;
            byte[] key = EncriptAES.getdcriptkey(out iv1);
            string encPwd = EncriptAES.EncryptString(companyId.ToString(), key, iv1);
            if(encPwd==token)
            {
                isExist = true;
            }           
            return isExist;
        }

        public static bool DeleteFile(string folder, string fileName)
        {
            string physicalPath = System.Web.HttpContext.Current.Server.MapPath("~/" + folder + "/" + fileName);
            System.IO.FileInfo file = new System.IO.FileInfo(physicalPath);
            if (file.Exists)
            {
                file.Delete();
                return true;
            }
            return false;
        }

        
        public static string SendSMS(string message, string mobile, string api, string user, string pass, string sender)
        {
            string result = string.Empty;
            using (var web = new System.Net.WebClient())
            {
                try
                {
                   
                    
                    //string url = "https://prpsms.co.in/API/SendMsg.aspx?uname=20201136&pass=Q9o99eLe&send=SMSINF&dest="+mobile+"&msg="+sms+"&priority=1";
                    string url = api + "uname=" + user + "&pass=" + pass + "&send=" + sender + "&dest=" + mobile + "&msg=" + message + "&priority=1";
                   return result = web.DownloadString(url);
                    //MessageBox.Show("SMS sent sucessfully..");
                }
                catch (Exception ex)
                {
                    return ex.Message;
                    //MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}