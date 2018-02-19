using DBHELPER;
using Entity;
using System;
using System.Data;

namespace DAL
{
    public class UserDAL
    {
        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserInfo AuthenticateUser(string email, string password)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    DbHelper.AddParameter("@paramEmail", email);
                    DbHelper.AddParameter("@paramPassword", password);
                    IDataReader reader = DbHelper.FillDataReader("SP_Users_AuthenticateUser");

                    if (reader.Read())
                    {
                        UserInfo userInfo = new UserInfo();
                        userInfo.UserID = Convert.ToInt32(reader["UserID"]);
                        userInfo.UserFirstName = reader["FirstName"].ToString();
                        userInfo.UserLastName = reader["LastName"].ToString();
                        userInfo.UserEmail = reader["Email"].ToString();
                        return userInfo;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="Fname"></param>
        /// <param name="Lname"></param>
        /// <returns></returns>
        public static DALMessage Register(string email, string password, string Fname, string Lname)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    DbHelper.AddParameter("@paramEmail", email);
                    DbHelper.AddParameter("@paramFirstName", Fname);
                    DbHelper.AddParameter("@paramLastName", Lname);
                    DbHelper.AddParameter("@paramPassword", password);
                    if (DbHelper.ExecuteNonQuery("SP_Users_Register"))
                    {

                        return DALMessage.Success;

                    }
                    else
                    {
                        throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/UserDAL/Register)  :SP_Users_Register ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

}
