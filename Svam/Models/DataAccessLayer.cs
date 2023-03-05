using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;

namespace Traders.Models
{
    public class DataAccessLayer
    {
        static string Con = ConfigurationManager.ConnectionStrings["_ConnectionString"].ToString();
        public static DataTable GetDataTable(string SPName)
        {
            MySqlConnection SqlCon = new MySqlConnection(Con);
            MySqlCommand SqlCmd = new MySqlCommand(SPName);
            SqlCmd.Connection = SqlCon;
            MySqlDataAdapter da = new MySqlDataAdapter(SqlCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            SqlCon.Close();
            SqlCon.Dispose();
            return dt;
        }
    }
}