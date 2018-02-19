using DBHELPER;
using Entity;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class FilesDAL
    {
        #region Variables
        #endregion

        #region Constructors
        public FilesDAL()
        {

        }
        #endregion

        #region Insert, Update And Delete Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static DALMessage InsertFile(GSFileInfo fileInfo)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    DbHelper.AddParameter("@paramMD5", fileInfo.MD5);
                    DbHelper.AddParameter("@paramName", fileInfo.Name);
                    DbHelper.AddParameter("@paramParentPath", fileInfo.ParentPath);
                    DbHelper.AddParameter("@paramType", fileInfo.Type);
                    DbHelper.AddParameter("@paramSize", fileInfo.Size);
                    DbHelper.AddParameter("@paramExtension", fileInfo.Extension);

                    if (DbHelper.ExecuteNonQuery("SP_Files_InsertFile"))
                    {
                        return DALMessage.Success;
                    }
                    else
                    {
                        throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/InsertFile)  :SP_Files_InsertFile ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/InsertFile) : Mesage " + ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static DALMessage AddFolder(GSFileInfo fileInfo)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    DbHelper.AddParameter("@paramUserID", fileInfo.userID);
                    DbHelper.AddParameter("@paramFolderName", fileInfo.Name);
                    DbHelper.AddParameter("@paramParentPath", fileInfo.ParentPath);

                    if (DbHelper.ExecuteNonQuery("SP_Files_AddFolder"))
                    {
                        return DALMessage.Success;
                    }
                    else
                    {
                        throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/InsertFolder)  :SP_Files_AddFolder ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/InsertFolder) : Mesage " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static DALMessage UpdateFile(GSFileInfo fileInfo)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {

                    DbHelper.AddParameter("@paramName", fileInfo.Name);
                    DbHelper.AddParameter("@paramParentPath", fileInfo.ParentPath);
                    DbHelper.AddParameter("@paramFileID", fileInfo.ID);
                    DbHelper.AddParameter("@paramUserID", fileInfo.userID);
                    if (DbHelper.ExecuteNonQuery("SP_Files_UpdateFile"))
                    {
                        return DALMessage.Success;
                    }
                    else
                    {
                        throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/UpdateFile)  :SP_Files_UpdateFile ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/UpdateFile) : Mesage " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DALMessage DeleteFile(GSFileInfo fileInfo)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    if (fileInfo.Type == FileType.File)
                    {

                        DbHelper.AddParameter("@paramuserID", fileInfo.userID);
                        DbHelper.AddParameter("@paramMD5", fileInfo.MD5);
                        DbHelper.AddParameter("@paramFileID", fileInfo.ID);
                        IDataReader reader = DbHelper.FillDataReader("SP_Files_DeleteFile");
                        if (reader.Read())
                        {

                            fileInfo.counter = Convert.ToInt32(reader["@tempCounter := Counter"]);

                            if (fileInfo.counter == 0)
                            {
                                string largeThumbnail = ("E:\\Idata\\UI\\Uploads\\largeThumbnail\\" + fileInfo.MD5 + fileInfo.Extension);
                                string smallThumbnail = ("E:\\Idata\\UI\\Uploads\\smallThumbnail\\" + fileInfo.MD5 + fileInfo.Extension);
                                string filePath = ("E:\\Idata\\UI\\Uploads\\" + fileInfo.MD5 + fileInfo.Extension);


                                try
                                {
                                    File.Delete(filePath);

                                }
                                catch (Exception ex)
                                {
                                    throw new GSException(ErrorCode.FailedToDeleteFromStorage, "Internal Server Error(BAL/FilesDAL/DeleteFile/filePath) : Failed to delete from Storage" + ex.Message);
                                }
                                try
                                {
                                    File.Delete(largeThumbnail);
                                }
                                catch (Exception ex)
                                {
                                    throw new GSException(ErrorCode.FailedToDeleteFromStorage, "Internal Server Error(BAL/FilesDAL/DeleteFile/largeThumbnail) : Failed to delete from Storage" + ex.Message);
                                }
                                try
                                {
                                    File.Delete(smallThumbnail);
                                }
                                catch (Exception ex)
                                {
                                    throw new GSException(ErrorCode.FailedToDeleteFromStorage, "Internal Server Error(BAL/FilesDAL/DeleteFile/smallThumbnail) : Failed to delete from Storage" + ex.Message);
                                }
                                return DALMessage.Success;
                            }
                            else
                            {
                                return DALMessage.Success;
                            }

                        }
                        else
                        {
                            throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/DeleteFile)  :Reader ");

                        }
                    }
                    throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/DeleteFile)  :This is not File Type ");
                }
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/DeleteFile)  :DeleteFile ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static DALMessage DeleteFolder(GSFileInfo fileInfo)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    DbHelper.AddParameter("@paramuserID", fileInfo.userID);
                    DbHelper.AddParameter("@paramParentPath", fileInfo.ParentPath);
                    DbHelper.AddParameter("@paramID", fileInfo.ID);
                    IDataReader reader = DbHelper.FillDataReader("SP_Files_DeleteFolder");
                    while (reader.Read())
                    {
                        fileInfo.counter = Convert.ToInt32(reader["Counter"]);
                        fileInfo.MD5 = reader["MD5"].ToString();

                        if (fileInfo.counter == 0)
                        {

                            string[] largeThumbnail = Directory.GetFiles("E:\\Idata\\UI\\Uploads\\largeThumbnail\\", fileInfo.MD5 + "*.*");
                            string[] smallThumbnail = Directory.GetFiles("E:\\Idata\\UI\\Uploads\\smallThumbnail\\", fileInfo.MD5 + "*.*");
                            string[] filePath = Directory.GetFiles("E:\\Idata\\UI\\Uploads\\", fileInfo.MD5 + "*.*");
                            foreach (string file in filePath)
                                try
                                {
                                    File.Delete(file);
                                }
                                catch { continue; };
                            foreach (string file in largeThumbnail)
                                try
                                {
                                    File.Delete(file);
                                }
                                catch { continue; };
                            foreach (string file in smallThumbnail)
                                try
                                {
                                    File.Delete(file);
                                }
                                catch { continue; };

                            return DALMessage.Success;
                        }
                        else
                        {
                            return DALMessage.Failed;
                        }
                    }
                    return DALMessage.Success;
                }
            }
            catch (Exception ex)
            {
                return DALMessage.Exception;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static DALMessage RenameFolder(GSFileInfo fileInfo)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {

                    DbHelper.AddParameter("@paramUserID", fileInfo.userID);
                    DbHelper.AddParameter("@paramFolderName", fileInfo.Name);
                    DbHelper.AddParameter("@paramNewName", fileInfo.fileNewName);
                    DbHelper.AddParameter("@paramFolderID", fileInfo.ID);

                    if (DbHelper.ExecuteNonQuery("SP_Files_RenameFolder"))
                    {

                        return DALMessage.Success;

                    }
                    else
                    {
                        throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/RenameFolder)  :SP_Files_RenameFolder ");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new GSException(ErrorCode.DataBaseError, "Internal Server Error (BAL/FilesDAL/RenameFolder) : Mesage " + ex.Message);
            }
        }


        #endregion

        #region Retrive Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<GSFileInfo> GetDirctoryInfo(int userID, string parentPath = "", bool isLevelList = true)
        {
            try
            {
                using (DataBaseHelper DbHelper = new DataBaseHelper())
                {
                    DbHelper.AddParameter("@paramParentPath", parentPath);
                    DbHelper.AddParameter("@paramUserID", userID);
                    IDataReader reader;
                    if (isLevelList)
                    {
                        reader = DbHelper.FillDataReader("SP_Files_GetDirctoryList");
                    }
                    else
                    {
                        reader = DbHelper.FillDataReader("SP_Files_GetDirctoryInfo");
                    }

                    List<GSFileInfo> userFilesList = new List<GSFileInfo>();
                    while (reader.Read())
                    {
                        GSFileInfo fileInfo = new GSFileInfo();
                        fileInfo.Name = reader["Name"].ToString();
                        fileInfo.ParentPath = reader["ParentPath"].ToString();
                        fileInfo.ID = Convert.ToInt32(reader["FileID"]);
                        fileInfo.Type = (FileType)Convert.ToInt32(reader["Type"]);
                        fileInfo.Size = Converter.ConvertBytesToKilobytes(Convert.ToInt32(reader["Size"]));

                        if (fileInfo.Type == FileType.File)
                        {
                            fileInfo.Extension = reader["Extension"].ToString();
                            fileInfo.Path = "Uploads/" + reader["MD5"].ToString() + reader["Extension"].ToString().Replace("/", ".");
                            fileInfo.LargeThumbnail = reader["MD5"].ToString() + reader["Extension"].ToString().Replace("/", ".");
                            fileInfo.SmallThumbnail = reader["MD5"].ToString() + reader["Extension"].ToString().Replace("/", ".");
                            fileInfo.HashID = Convert.ToInt32(reader["HashID"]);
                            fileInfo.MD5 = reader["MD5"].ToString();
                        }
                        userFilesList.Add(fileInfo);
                    }

                    return userFilesList;
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
