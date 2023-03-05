using MySql.Data.MySqlClient;
using Svam.EF;
using Svam.UtilityManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Svam.Models
{
    public class ExceptionLogging
    {
        private static String exepurl;
        static MySqlConnection con;
        private static void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["_ConnectionString"].ToString();
            con = new MySqlConnection(constr);
            con.Open();
        }

        public static void SendExcepToDB(Exception exdb)
        {
            niscrmEntities db = new niscrmEntities();

            //connection();
            exepurl = HttpContext.Current.Request.Url.AbsolutePath;
            //MySqlCommand com = new MySqlCommand("CRM_ExceptionLogging", con);
            //com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("P_ExceptionMsg", exdb.Message.ToString());
            //com.Parameters.AddWithValue("P_ExceptionType", exdb.GetType().Name.ToString());
            //com.Parameters.AddWithValue("P_ExceptionURL", exepurl);
            //com.Parameters.AddWithValue("P_ExceptionSource", exdb.StackTrace.ToString());
            //com.ExecuteNonQuery();
            var exp = new crm_exceptionlogging
            {
                ExceptionMsg = exdb.Message.ToString(),
                ExceptionType = exdb.GetType().Name.ToString(),
                ExceptionSource= exdb.StackTrace.ToString(),
                ExceptionURL= exepurl,
                Logdate=Constant.GetBharatTime()
            };
            db.crm_exceptionlogging.Add(exp);
            db.SaveChanges();
        }  
    }
}