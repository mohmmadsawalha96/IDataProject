using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
namespace Entity
{
   public static class Thumbnail
    {
        /// <summary>
        /// The function returns a Bitmap object of the changed thumbnail image which you can save on the disk.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="thumbWidth"></param>
        /// <param name="thumbHeight"></param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(string filePath, int thumbWidth, int thumbHeight)
        {
            System.Drawing.Bitmap bitmapOut = null;
            try
            {
                Bitmap loBMP = new Bitmap(filePath);
                ImageFormat loFormat = loBMP.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                //*** If the image is smaller than a thumbnail just return it
                if (loBMP.Width < thumbWidth && loBMP.Height < thumbHeight)
                    return loBMP;

                if (loBMP.Width > loBMP.Height)
                {
                    lnRatio = (decimal)thumbWidth / loBMP.Width;
                    lnNewWidth = thumbWidth;
                    decimal lnTemp = loBMP.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)thumbHeight / loBMP.Height;
                    lnNewHeight = thumbHeight;
                    decimal lnTemp = loBMP.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bitmapOut = new Bitmap(lnNewWidth, lnNewHeight);
                Graphics graphics = Graphics.FromImage(bitmapOut);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
                graphics.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);
                loBMP.Dispose();
            }
            catch
            {
                return null;
            }

            return bitmapOut;
        }
    }
}
