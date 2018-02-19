using DAL;
using Entity;
using System;
using System.Collections.Generic;
namespace BAL
{
    public class UsersBAL
    {
        #region Variables
        #endregion

        #region Constructors
        public UsersBAL()
        {

        }
        #endregion

        #region public Methods
        public static UserInfo AuthenticateUser(string email, string password)
        {
            try
            {

                UserInfo userInfo = new UserInfo();
                userInfo = UserDAL.AuthenticateUser(email, password);
                if (userInfo != null)
                {
                    return userInfo;
                }
                else
                {
                    throw new GSException(ErrorCode.AuthenticationFailed, "There is no such User");
                }
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (AuthenticateUser) : Mesage " + ex.Message);
            }
            #endregion
        }
    }
}






