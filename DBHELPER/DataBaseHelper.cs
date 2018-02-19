//using MySql.Data.MySqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DBHELPER
{

    public class DataBaseHelper : IDisposable
    {
        #region Variables
        MySqlConnection mySqlConnection;
        MySqlCommand mySqlcommand;
        bool disposed = false ;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public DataBaseHelper()
        {
            mySqlConnection = new MySqlConnection();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["imgdropConnectionString"].ToString();
            mySqlConnection.ConnectionString = connectionString;
            OpenConnection();
        }
        #endregion

        #region Public Methods
        #region DBmethods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public void AddParameter(string parameterName, object value)
        {
            mySqlcommand.Parameters.AddWithValue(parameterName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string name)
        {
            mySqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
            mySqlcommand.CommandText = name;
            return mySqlcommand.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataSet ExcuteQuery(string name)
        {
            mySqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
            mySqlcommand.CommandText = name;
            DataSet dataset = new DataSet();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(mySqlcommand);
            dataAdapter.Fill(dataset);
            return dataset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public IDataReader FillDataReader(string name)
        {
            mySqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
            mySqlcommand.CommandText = name;
            IDataReader reader = mySqlcommand.ExecuteReader();

            return reader;

        }
        #endregion

        #region DisposeMethod
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(Boolean disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    CloseConnection();
                }
            }
            disposed = true;
        }


        /// <summary>
        /// OverRide From Idispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #endregion

        #region Private Methods


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool OpenConnection()
        {
            try
            {

                mySqlConnection.Open();
                mySqlcommand = new MySqlCommand() { Connection = mySqlConnection };
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        throw new Exception("Cannot connect to server. Contact administrator !");

                    case 1045:
                        throw new Exception("Invalid username/password. Please check your connection string and try again !");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CloseConnection()
        {
            try
            {
                mySqlConnection.Close();

                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.InnerException == null)
                    throw new Exception(ex.Message);
                throw new Exception(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ~DataBaseHelper()
        {
            Dispose(false);
        }

        #endregion

    }
}


