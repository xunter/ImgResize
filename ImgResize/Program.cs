using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace ImgResize
{
    class Program
    {
		//Usage: ImgResize.exe %SRC_DIR_PATH% %DEST_DIR_PATH% %ZOOM_PERCENT_FLOAT% %EXTENSION%
		//Example: ImgResize.exe c:\images c:\small_images 0.7 .jpg
        static void Main(string[] args)
        {
            try
            {
                var fromDirPath = args[0];
                var toDirPath = args[1];
                float zoom = (float)Convert.ToDouble(args[2]);
                var ext = args[3];

                Console.WriteLine("Zoom: {0}", zoom);
                
                var searchPattern = String.Format("*.{0}", ext);
                var dirInfo = new DirectoryInfo(fromDirPath);
                foreach (var file in dirInfo.GetFiles(searchPattern))
                {
                    using (var image = Bitmap.FromFile(file.FullName))
                    {
                        var originalWidth = image.Width;
                        var originalHeight = image.Height;
                        var thumbWidth = (int)Math.Truncate((double)originalWidth * zoom);
                        var thumbHeight = (int)Math.Truncate((double)originalHeight * zoom);
                        var thumbFileName = Path.Combine(toDirPath, file.Name);
                        /*using (var thumbImage = image.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero))
                        {
                            thumbImage.Save(thumbFileName);  
                        }*/
                        using (var newImage = new Bitmap(thumbWidth, thumbHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                        {
                            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                            using (var g = Graphics.FromImage(newImage))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                //g.ScaleTransform(zoom, zoom, System.Drawing.Drawing2D.MatrixOrder.Append);
                                g.DrawImage(image, new Rectangle(0,0,thumbWidth, thumbHeight), new Rectangle(0,0,image.Width, image.Height), GraphicsUnit.Pixel);
                            }
                            newImage.Save(thumbFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }
}
