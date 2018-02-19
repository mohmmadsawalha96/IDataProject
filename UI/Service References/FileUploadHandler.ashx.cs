using System;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using Entity;
using Entity.json;
using System.Collections.Generic;

namespace IData
{

    public class FileUploadHandler : IHttpHandler
    {
        #region Variables
        List<UserInfo> userinfoList = new List<UserInfo>();
        #endregion

        #region Process methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext content)
        {
            List<GSFileInfo> userInfoList = new List<GSFileInfo>();
            try
            {

                if (content.Request.Files.Count > 0)
                {

                    HttpFileCollection files = content.Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFile file = files[i];
                        GSFileInfo fileInfo = new GSFileInfo();
                        using (MemoryStream streamCopy = new MemoryStream())
                        {
                            byte[] fileDataArr = null;

                            content.Request.Files[i].InputStream.CopyTo(streamCopy);

                            string[] parentPath = content.Request.Headers.GetValues("parentPath");

                            fileDataArr = streamCopy.ToArray();

                            if (file.ContentType.Contains("image"))
                            {

                                fileInfo.Type = FileType.File;
                            }
                            else
                            {
                                fileInfo.Type = FileType.Folder;
                            }

                            fileInfo.MD5 = GetMD5(fileDataArr);
                            fileInfo.Size = files[i].ContentLength;
                            fileInfo.Name = Path.GetFileNameWithoutExtension(file.FileName);
                            fileInfo.ParentPath = parentPath[0];
                            fileInfo.Extension = (files[i].ContentType.Substring(files[i].ContentType.IndexOf('/'))).Replace("/", ".");
                            fileInfo.userID = Convert.ToInt32(content.Request.Headers.GetValues("UserID"));
                            {

                                string ServerPath = content.Server.MapPath("~/uploads/" + fileInfo.MD5 + fileInfo.Extension);
                                file.SaveAs(ServerPath);
                                fileInfo.Path = "Uploads/" + fileInfo.MD5 + fileInfo.Extension;
                                System.Drawing.Bitmap largeThumbnail = Thumbnail.CreateThumbnail(ServerPath, 170, 170);
                                largeThumbnail.Save(content.Server.MapPath("~/uploads/largeThumbnail/" + fileInfo.MD5 + fileInfo.Extension));
                                fileInfo.LargeThumbnail = fileInfo.MD5 + fileInfo.Extension;
                                fileInfo.SmallThumbnail = fileInfo.MD5 + fileInfo.Extension;

                            }


                            userInfoList.Add(fileInfo);
                        }
                    }
                    content.Response.Write(JSONHelper.ToJson(userInfoList));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetMD5(Byte[] file)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    string hash = BitConverter.ToString(md5.ComputeHash(file)).Replace("-", "");

                    return hash;

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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }


        #endregion
    }
}