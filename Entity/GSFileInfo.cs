
using System;
namespace Entity
{
    public class GSFileInfo
    {
        #region Constructors
        public GSFileInfo(string name, string path, int id, FileType type, string ParentPath)
        {
            this.ID = id;

            this.Name = name;

            this.Path = path;

            this.Type = type;

            this.ParentPath = ParentPath;

        }
        public GSFileInfo()
        {

        }
        #endregion

        #region DataInfo
        private string largeThumbnail;
        private string smallThumbnail;
        bool disposed = false;
        public int counter { set; get; }
        public int ID { set; get; }
        public string Name { set; get; }
        public string Path { set; get; }
        public FileType Type { set; get; }
        public string ParentPath { set; get; }
        public string MD5 { set; get; }
        public double Size { set; get; }
        public int HashID { set; get; }
        public int userID { set; get; }
        public string LargeThumbnail
        {
            set { largeThumbnail = value; }
            get { return "Uploads/LargeThumbnail/" + largeThumbnail; }
        }
        public string SmallThumbnail
        {
            set { smallThumbnail = value; }
            get { return "Uploads/SmallThumbnail/" + smallThumbnail; }
        }
        public string Extension { set; get; }
        public string fileNewName { set; get; }
        #endregion



    }
}
