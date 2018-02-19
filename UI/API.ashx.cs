using System;
using System.Web;
using BAL;
using Entity;
using Entity.json;
using DAL;
using System.Collections.Generic;

namespace IData
{

    public class API : IHttpHandler
    {
        #region Global Variables
        UserInfo userInfo = null;
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string action = GetHeaderValue(context, "Action");

                if (action == "RegisterReq")
                {
                    Register(context);
                    return;
                }


                string email = GetHeaderValue(context, "Email");
                string password = GetHeaderValue(context, "Password");

                userInfo = UsersBAL.AuthenticateUser(email, password);

                switch (action)
                {
                    case "GetUserInfo":
                        GetUserInfo(userInfo, context);
                        break;
                    case "Rename":
                        RenameFile(context);
                        break;
                    case "ListFiles":
                        ListFiles(context);
                        break;
                    case "RenameFolder":
                        RenameFolder(context);
                        break;
                    case "DeleteFile":
                        DeleteFile(context);
                        break;
                    case "AddFolder":
                        AddFolder(context);
                        break;
                }

            }
            catch (GSException ex)
            {
                context.Response.AddHeader("ErrorCode", ((int)ex.ErrorCode).ToString());
                context.Response.AddHeader("ErrorMessage", ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.AddHeader("ErrorCode", ((int)ErrorCode.InternalServerError).ToString());
                context.Response.AddHeader("ErrorMessage", "Internal Server Error (ProcessRequest) : Mesage " + ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region Private Methods 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void Register(HttpContext context)
        {
            try
            {
                string email = GetHeaderValue(context, "Email");
                string password = GetHeaderValue(context, "Password");
                string firstName;
                string lastName;
                firstName = context.Request["firstName"];
                lastName = context.Request["lastName"];

                if (UserDAL.Register(email, password, firstName, lastName) == DALMessage.Success)
                {
                    throw new GSException(ErrorCode.Success, "Successful register");
                }
                else
                {
                    throw new GSException(ErrorCode.FailedToRname, "Register Faild");
                }
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (Register) : Mesage " + ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="context"></param>
        private void GetUserInfo(UserInfo userInfo, HttpContext context)
        {
            try
            {
                string json = JSONHelper.ToJson(userInfo);
                context.Response.Write(json);
                throw new GSException(ErrorCode.Success, "Success Login");
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.FailedToSerialize, "Failed to Get User Info");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void RenameFile(HttpContext context)
        {
            try
            {
                GSFileInfo fileInfo = new GSFileInfo();
                fileInfo.Name = context.Request["FileName"];
                fileInfo.ParentPath = context.Request["ParentPath"];
                fileInfo.ID = Convert.ToInt32(context.Request["FileID"]);
                fileInfo.userID = userInfo.UserID;

                if (FilesDAL.UpdateFile(fileInfo) == DALMessage.Success)
                {
                    throw new GSException(ErrorCode.Success, "Success file was updated");
                }
                else
                {
                    throw new GSException(ErrorCode.FailedToRname, "Failed To Rname");
                }
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (RenameFile) : Mesage " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void ListFiles(HttpContext context)
        {

            try
            {
                List<GSFileInfo> files;
                string parentPath = GetHeaderValue(context, "parentPath");
                if (parentPath != null)
                {
                    files = FilesDAL.GetDirctoryInfo(userInfo.UserID, parentPath, true);
                }
                else
                {
                    files = FilesDAL.GetDirctoryInfo(userInfo.UserID, "", true);
                }


                if (files != null && files.Count > 0)
                {
                    string json = JSONHelper.ToJson(files);
                    context.Response.Write(json);
                    throw new GSException(ErrorCode.Success, "List Files Successfully");
                }
                else
                {
                    throw new GSException(ErrorCode.FailedToListFiles, "Failed To List Files ");
                }
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (ListFiles) : Mesage " + ex.Message);
            }


        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="allowNull"></param>
        /// <returns></returns>
        private string GetHeaderValue(HttpContext context, string key, bool allowNull = false)
        {
            try
            {
                string value = context.Request.Headers[key];
                if (string.IsNullOrEmpty(value) && !allowNull)
                {
                    throw new GSException(ErrorCode.BadGateway, "Missing Required Header Memeber: Please Send " + key);
                }
                else
                {
                    return value;
                }
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (GetHeaderValue) : Mesage " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="parentPath"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        protected void AddFolder(HttpContext context)
        {
            try
            {
                GSFileInfo fileInfo = new GSFileInfo();
                fileInfo.Name = context.Request["FolderName"];
                fileInfo.ParentPath = context.Request["ParentPath"];
                fileInfo.userID = userInfo.UserID;
                if (FilesDAL.AddFolder(fileInfo) == DALMessage.Success)
                {
                    throw new GSException(ErrorCode.Success, "Success Folder was Created");
                }
                else
                {
                    throw new GSException(ErrorCode.FailedToAddFolder, "Failed To Add Folder");
                }
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (AddFolder) : Mesage " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        protected void RenameFolder(HttpContext context)
        {
            try
            {
                GSFileInfo fileInfo = new GSFileInfo();
                fileInfo.Name = context.Request["FolderName"];
                fileInfo.ParentPath = context.Request["ParentPath"];
                fileInfo.userID = userInfo.UserID;

                if (FilesDAL.RenameFolder(fileInfo) == DALMessage.Success)
                {
                    throw new GSException(ErrorCode.Success, "Success was RenameFolder");
                }
                else
                {
                    throw new GSException(ErrorCode.FailedToRname, "Failed To Rname");
                }
            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (RenameFolder) : Mesage " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void DeleteFile(HttpContext context)
        {
            try
            {
                GSFileInfo fileInfo = new GSFileInfo();
                fileInfo.ID = Convert.ToInt32(context.Request["FileID"]);
                fileInfo.ParentPath = context.Request["ParentPath"];
                fileInfo.userID = userInfo.UserID;

                if (FilesDAL.DeleteFile(fileInfo) == DALMessage.Success)
                {
                    throw new GSException(ErrorCode.Success, "Success file was updated");
                }
                else
                {
                    throw new GSException(ErrorCode.FailedToDelete, "Failed To Delete File");
                }

            }
            catch (GSException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.InternalServerError, "Internal Server Error (DeleteFile) : Mesage " + ex.Message);
            }
        }
        #endregion

    }
}