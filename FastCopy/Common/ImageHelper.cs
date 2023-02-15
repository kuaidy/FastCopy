using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FastCopy.Common
{
    public class ImageHelper
    {
        /// <summary>
        /// 获取文件的图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static BitmapImage GetFileIcon(string fileName)
        {
            try
            {
                if (Directory.Exists(fileName))
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/folder.png",UriKind.RelativeOrAbsolute));

                    return bitmapImage;
                }
                else if (File.Exists(fileName))
                {
                    Icon icon = Icon.ExtractAssociatedIcon(fileName);
                    Bitmap bitmap = icon.ToBitmap();
                    BitmapImage image = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        image.Freeze();
                    }
                    return image;
                }
                else
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/text.png", UriKind.RelativeOrAbsolute));
                    return bitmapImage;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
