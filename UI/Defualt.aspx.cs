using DAL;
using Entity;
using Entity.json;
using System;
using System.Collections.Specialized;
namespace IData
{
    public partial class Home : System.Web.UI.Page
    {
        #region Variables
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Events
        #endregion

        #region Methods

        #region Public
        #endregion

        #region Private
        #endregion

        #endregion

        #region WebMethods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="parentPath"></param>
        /// <param name="md5"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static bool UpdateFile(int userID, string parentPath, string md5, int id)
        {


            UserInfo userInfo = new UserInfo();
            userInfo.UserID = userID;
            userInfo.FilesInfo.ID = id;
            userInfo.FilesInfo.ParentPath = parentPath;
            userInfo.FilesInfo.MD5 = md5;
            if (FilesDAL.UpdateFile(userInfo) == DALMessage.Success)
            {
                return true;
            }
            else
            {
                return false;
            }





        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="parentPath"></param>
        /// <param name="isLevelList"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static string GetDirctoryInfo(int userID, string parentPath, bool isLevelList)
        {

            return JSONHelper.ToJson(FilesDAL.GetDirctoryInfo(userID, parentPath, isLevelList));

        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="Name"></param>
        /// <param name="fileNewName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static bool RenameFolder(int userID, string Name, string fileNewName, int ID)
        {
            try
            {
                UserInfo userInfo = new UserInfo();
                userInfo.UserID = userID;
                userInfo.FilesInfo.ID = ID;
                userInfo.FilesInfo.Name = Name;
                userInfo.FilesInfo.fileNewName = fileNewName;
                if (FilesDAL.RenameFolder(userInfo) == DALMessage.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="id"></param>
        /// <param name="parentPath"></param>
        /// <param name="fileName"></param>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <param name="md5"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static bool DeleteFile(int userID, int id, string parentPath, string fileName, int type, string path = "", string md5 = null, string extension = null)
        {



            UserInfo userInfo = new UserInfo();
            userInfo.UserID = userID;
            userInfo.FilesInfo.Path = path;
            userInfo.FilesInfo.Type = (FileType)type;
            userInfo.FilesInfo.ParentPath = parentPath;
            userInfo.FilesInfo.Name = fileName;
            userInfo.FilesInfo.ID = id;
            userInfo.FilesInfo.MD5 = md5;
            userInfo.FilesInfo.Extension = extension;
            if (FilesDAL.DeleteFile(userInfo) == DALMessage.Success)
            {
                return true;
            }
            else
            {
                return false;
            }



        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="parentPath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [System.Web.Services.WebMethod]
        public static bool AddFolder(int userID, string parentPath, string folderName)
        {

            UserInfo userInfo = new UserInfo();
            userInfo.UserID = userID;
            userInfo.FilesInfo.ParentPath = parentPath;
            userInfo.FilesInfo.Name = folderName;
            if (FilesDAL.InsertFolder(userInfo) == DALMessage.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }




        #endregion

    }
}

