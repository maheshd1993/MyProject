using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Svam
{
    public class DataUtility
    {
        #region All Constructors
        /// <summary>
        /// This default or no argument constructor.
        /// </summary>
        public DataUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// This parametrized constructor
        /// </summary>
        /// <param name="Connection">string</param>
        public DataUtility(string Connection)
        {
            this._mCon = new MySqlConnection(Connection);
        }
        #endregion

        #region All Propreties
        private MySqlConnection _mCon;

        public MySqlConnection mCon
        {
            get { return _mCon; }
            set { _mCon = value; }
        }

        private MySqlCommand _mDataCom;

        public MySqlCommand mDataCom
        { 
            get { return _mDataCom; }
            set { _mDataCom = value; }
        }

        private MySqlDataAdapter _mDa;

        public MySqlDataAdapter mDa
        {
            get
            {
                if (_mDa == null)
                {
                    _mDa = new MySqlDataAdapter();
                }
                return _mDa;
            }
            set { _mDa = value; }
        }

        private DataTable _DataTable;

        public DataTable DataTable
        {
            get
            {
                if (_DataTable == null)
                {
                    _DataTable = new DataTable();
                }
                return _DataTable;
            }
            set { _DataTable = value; }
        }

        private DataSet _DataSet;

        public DataSet DataSet
        {
            get
            {
                if (_DataSet == null)
                {
                    _DataSet = new DataSet();
                }
                return _DataSet;
            }
            set { _DataSet = value; }
        }


        #endregion

        #region All private methods
        /// <summary>
        ///  1. Initialize Connection object with parameterize constructor.
        ///  2. Initialize Command object wiht default or no argument constructor.
        ///  3. Set active connectin with Command object.
        /// </summary>
        /// 
        private void OpenConnection()
        {
            // Check Connection object for null.
            if (_mCon == null)
            {
                _mCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["_ConnectionString"].ConnectionString);
            }
            // Check Connection State.

            _mCon.Close();
            _mCon.Open();

            // Initialize Command object.
            _mDataCom = new MySqlCommand();

            // Set active connection with Command object.
            _mDataCom.Connection = _mCon;


        }
        /// <summary>
        /// This method  is used for close the connection.
        /// </summary>
        private void CloseConnection()
        {
            // Check Connection is open.

            _mCon.Close();

        }
        /// <summary>
        /// This method is used for Dispose the connection object.
        /// </summary>
        private void DisposeConnection()
        {
            if (_mCon != null)
            {
                _mCon.Dispose();
                // Initialize Connection object with null.
                _mCon = null;
            }
        }
        #endregion

        #region All public methods
        /// <summary>
        /// This method is used to execute DML  using SQL as text.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>int, no of rows affected</returns>
        public int ExecuteSql(string strSql)
        {
            // Open the connection.
            OpenConnection();
            // Set Command object properties.

            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            // Execute the method.
            int intResult = _mDataCom.ExecuteNonQuery();
            // Close the connection.
            CloseConnection();
            // Release the resources.
            DisposeConnection();
            return intResult;
        }


        /// <summary>
        /// This method is used to execute DML using parameterized SQL query.
        /// Passing MySqlParameter array and SQL query.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSql">string</param>
        /// <returns>int, no of rows affected</returns>
        public int ExecuteSql(MySqlParameter[] arrParam, string strSql)
        {
            OpenConnection();
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            int intResult = _mDataCom.ExecuteNonQuery();
            CloseConnection();
            DisposeConnection();
            return intResult;
        }

        /// <summary>
        /// This method is used to execute DML using stored procedure.
        /// Passing MySqlParameter array and procedure name.  
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSPName">string</param>
        /// <returns>string, no of records affected</returns>
        public string ExecuteSqlSPS(MySqlParameter[] arrParam, string strSPName)
        {
            OpenConnection();
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }

            _mDataCom.ExecuteNonQuery();
            string strResult = (_mDataCom.Parameters["@strResult"].Value.ToString());
            CloseConnection();
            DisposeConnection();
            return strResult;
        }

        /// <summary>
        /// This method is used to execute DML using stored procedure.
        /// Passing MySqlParameter array and procedure name.  
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSPName">string</param>
        /// <returns>int, no of records affected</returns>
        public int ExecuteSqlSP(MySqlParameter[] arrParam, string strSPName)
        {
            OpenConnection();
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }

            _mDataCom.ExecuteNonQuery();
            int intResult = Int32.Parse(_mDataCom.Parameters["@intResult"].Value.ToString());
            CloseConnection();
            DisposeConnection();
            return intResult;
        }

        /// <summary>
        /// This is to validate a record if its already there in the table.
        /// If record exist return true else return false.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>bool</returns>
        public bool IsExist(string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            // Execute the method
            int intResult = (int)_mDataCom.ExecuteScalar(); // typecasting because ExecuteScalar return object.
            CloseConnection();
            DisposeConnection();

            // Check the result.
            if (intResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// This method is used to return frist column of the selected record.
        /// Pass SQL query as text.
        /// Return object so type cast it.
        /// Created Date : 31/10/2005
        /// Created By   : Dipak Sinha.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>object</returns>
        public object GetScalar(string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            // Execute the method
            object intResult = _mDataCom.ExecuteScalar();
            CloseConnection();
            DisposeConnection();
            return intResult;
        }

        /// <summary>
        /// This method check whether record already exist in the table.
        /// Passing MySqlParameter array and SQL query as text.
        /// If exist return true else return false.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSql">string</param>
        /// <returns>bool</returns>
        public bool IsExist(MySqlParameter[] arrParam, string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;

            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Execute the method
            int intResult = (int)_mDataCom.ExecuteScalar(); // typecasting because ExecuteScalar return object.
            CloseConnection();
            DisposeConnection();

            // Check the result.
            if (intResult > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable GetDataTable(string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;

            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;

            // Initialize DataTable object.
            _DataTable = new DataTable();
            _mDa.Fill(_DataTable);
            CloseConnection();
            DisposeConnection();
            return _DataTable;
        }

        /// <summary>
        /// This is to read data in Disconnected mode using parameterized SQL.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSql">string</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(MySqlParameter[] arrParam, string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataTable object.
            _DataTable = new DataTable();
            _mDa.Fill(_DataTable);
            CloseConnection();
            DisposeConnection();
            return _DataTable;
        }
        /// <summary>
        /// This is to read data using Disconnected mode using stored procedure.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSPName">string</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableSP(MySqlParameter[] arrParam, string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure; ;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataTable object.
            _DataTable = new DataTable();
            _mDa.Fill(_DataTable);
            CloseConnection();
            DisposeConnection();
            return _DataTable;
        }
        /// <summary>
        /// This is to read data using Disconnected mode using stored procedure.
        /// Passing procedure name as parameter.
        /// </summary>
        /// <param name="strSPName">string stored procedure name</param>
        /// <returns>DataTable object</returns>
        public DataTable GetDataTableSP(string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataTable object.
            _DataTable = new DataTable();
            _mDa.Fill(_DataTable);
            CloseConnection();
            DisposeConnection();
            return _DataTable;
        }
        /// <summary>
        /// This is to read data in Disconnected mode using SQL as text.
        /// </summary>
        /// <param name="strSql">string </param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            // Initailize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataSet object.
            _DataSet = new DataSet();
            _mDa.Fill(_DataSet);
            CloseConnection();
            DisposeConnection();
            return _DataSet;
        }
        /// <summary>
        /// This is to read data in Disconnected mode using parameterized SQL as text.
        /// </summary>
        /// <param name="arrParam">MySqlParameter[]</param>
        /// <param name="strSql">string</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(MySqlParameter[] arrParam, string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataSet object.
            _DataSet = new DataSet();
            _mDa.Fill(_DataSet);
            CloseConnection();
            DisposeConnection();
            return _DataSet;
        }

        /// <summary>
        /// This is to read data in Disconnected mode using stored procedure.
        /// Passing stored procedure name.
        /// </summary>
        /// <param name="strSPName">string stored procedure name</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetSP(string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataSet object.
            _DataSet = new DataSet();
            _mDa.Fill(_DataSet);
            CloseConnection();
            DisposeConnection();
            return _DataSet;
        }

        /// <summary>
        /// This is to read data in Disconnected mode using parameterized stored procedure.
        /// Passing MySqlParameter collection stored procedure name.
        /// </summary>
        /// <param name="arrParam">MySqlParameter[]</param>
        /// <param name="strSPName">string</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetSP(MySqlParameter[] arrParam, string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataSet object.
            _DataSet = new DataSet();
            _mDa.Fill(_DataSet);
            CloseConnection();
            DisposeConnection();
            return _DataSet;
        }

        /// <summary>
        /// This is to read data using Connected mode using SQL as text.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>MySqlDataReader</returns>
        public MySqlDataReader GetDataReader(string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;

            // Create MySqlDataReader object.
            MySqlDataReader dReader;
            dReader = _mDataCom.ExecuteReader(CommandBehavior.CloseConnection);
            return dReader;
        }
        /// <summary>
        /// This is to read data using Connected mode using parameterized SQL.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSql">string</param>
        /// <returns>MySqlDataReader</returns>
        public MySqlDataReader GetDataReader(MySqlParameter[] arrParam, string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;

            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Create MySqlDataReader object.
            MySqlDataReader dReader;
            dReader = _mDataCom.ExecuteReader(CommandBehavior.CloseConnection);
            return dReader;
        }

        /// <summary>
        /// This is to read data in Connected mode using stored procedure.
        /// Passing MySqlParameter and procedure name as parameter.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSql">string</param>
        /// <returns>MySqlDataReader</returns>
        public MySqlDataReader GetDataReaderSP(MySqlParameter[] arrParam, string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;

            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Create MySqlDataReader object.
            MySqlDataReader dReader;
            dReader = _mDataCom.ExecuteReader(CommandBehavior.CloseConnection);
            return dReader;
        }
        /// <summary>
        /// This is to read data in Connected mode using stored procedure.
        /// Passing procedure name as parameter.
        /// </summary>
        /// <param name="strSPName">string stored procedure name</param>
        /// <returns>MySqlDataReader object</returns>
        public MySqlDataReader GetDataReaderSP(string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            // Create MySqlDataReader object.
            MySqlDataReader dReader;
            dReader = _mDataCom.ExecuteReader(CommandBehavior.CloseConnection);
            return dReader;
        }

        /// <summary>
        /// This is to read Single data using Connected mode using SQL as text.
        /// </summary>
        /// <param name="strSql">string</param>
        /// <returns>Object</returns>
        public object GetScaler(MySqlParameter[] arrParam, string strSql)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.Parameters.Clear();
            _mDataCom.CommandType = CommandType.Text;
            _mDataCom.CommandText = strSql;
            _mDataCom.CommandTimeout = 21600;
            _mDataCom.Parameters.Add(arrParam[0]);

            // Create MySqlDataReader object.
            //MySqlDataReader dReader;
            Object oObject;
            oObject = _mDataCom.ExecuteScalar();
            return oObject;
        }
        public object GetScalerSP(MySqlParameter[] arrParam, string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.Parameters.Clear();
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;
            _mDataCom.Parameters.Add(arrParam[0]);

            // Create MySqlDataReader object.
            //MySqlDataReader dReader;
            Object oObject;
            oObject = _mDataCom.ExecuteScalar();
            return oObject;
        }
        public object GetScalerSP(string strSPName)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.Parameters.Clear();
            _mDataCom.CommandType = CommandType.StoredProcedure;
            _mDataCom.CommandText = strSPName;
            _mDataCom.CommandTimeout = 21600;

            // Create MySqlDataReader object.
            //MySqlDataReader dReader;
            Object oObject;
            oObject = _mDataCom.ExecuteScalar();
            return oObject;
        }

        /// <summary>
        /// This is to read data using Disconnected mode using stored procedure.
        /// </summary>
        /// <param name="arrParam">MySqlParameter</param>
        /// <param name="strSPName">string</param>
        /// <returns>DataTable</returns>
        public DataTable GetMaster(MySqlParameter[] arrParam)
        {
            OpenConnection();
            // Set Command object properties.
            _mDataCom.CommandType = CommandType.StoredProcedure; ;
            _mDataCom.CommandText = "USP_GetMaster";
            _mDataCom.CommandTimeout = 21600;
            for (int i = 0; i < arrParam.Length; i++)
            {
                _mDataCom.Parameters.Add(arrParam[i]);
            }
            // Initialize MySqlDataAdapter object.
            _mDa = new MySqlDataAdapter();
            // Set the Command object in DataAdapter.
            _mDa.SelectCommand = _mDataCom;
            // Initialize DataTable object.
            _DataTable = new DataTable();
            _mDa.Fill(_DataTable);
            CloseConnection();
            DisposeConnection();
            return _DataTable;
        }
        #endregion
    }
}