using MyPlace.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPlace.Domain;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MyPlace.Web.Helpers
{
    public static class HelpMe
    {
        public static System.Drawing.Image ResizeMeByHeight(this Image oldImage, int targetSize)
        {
            using (oldImage)
            {
                Size newSize = CalculateDimensions(oldImage.Size, targetSize);

                using (Bitmap newImage = new Bitmap(newSize.Width, newSize.Height))
                {
                    newImage.SetResolution(oldImage.HorizontalResolution, oldImage.VerticalResolution);
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), newSize));
                        System.Drawing.Image img = (Image)newImage.Clone();
                        return img ;
                    }
                }

            }
        }

        private static Size CalculateDimensions(Size oldSize, int targetSize)
        {
            Size newSize = new Size();
            //if (oldSize.Width > oldSize.Height)
            //{
            //    newSize.Width = targetSize;
            //    newSize.Height = (int)(oldSize.Height * (float)targetSize / (float)oldSize.Width);
            //}
            //else
            //{
                newSize.Width = (int)(oldSize.Width * (float)targetSize / (float)oldSize.Height);
                newSize.Height = targetSize;
            //}
            return newSize;
        }

        public static string SaveIt(this Image image, string giveMeRelativePath, string giveMeFileName="Default.jpg")
        {
            var fileName = giveMeFileName;// Path.GetFileName(image.FileName);
            var relativepath = giveMeRelativePath;
            var dir =HttpContext.Current.Server.MapPath(relativepath);
            var physicalPath = Path.Combine(dir, fileName);
            relativepath = (relativepath + "/" + fileName).TrimStart('~');
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            image.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            return relativepath;
        }
        public static string SaveIt(this HttpPostedFileBase imageToProcess, string giveMeRelativePath, string giveMeFileName = "Default")
        {
            if (giveMeFileName == "default")
                giveMeFileName = imageToProcess.FileName;
            else giveMeFileName = giveMeFileName + Path.GetExtension(imageToProcess.FileName);

           
            var fileName = giveMeFileName;// Path.GetFileName(image.FileName);
            var relativepath = giveMeRelativePath;
            var dir = HttpContext.Current.Server.MapPath(relativepath);
            var physicalPath = Path.Combine(dir, fileName);
            relativepath = (relativepath + "/" + fileName).TrimStart('~');
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                Image image = Image.FromStream(imageToProcess.InputStream);
                image.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {

                imageToProcess.SaveAs(physicalPath);
            }
            

            return relativepath;
        }

        public static string ProcessMe(this HttpPostedFileBase ImageToProcess,string PathToSave,int ReHeightTo,string fileName="default")
        {
            string result = "";
            if (fileName == "default")
                fileName = ImageToProcess.FileName;
            else fileName= fileName + Path.GetExtension(ImageToProcess.FileName);

            try
            {
                result= Image.FromStream(ImageToProcess.InputStream).ResizeMeByHeight(ReHeightTo).SaveIt(PathToSave,fileName );
            }
            catch (Exception)
            {
                throw new Exception("oh oh error");
               
            }
            return result;
        }

        public static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
       
    }
    public static class Connection
    {
        public static string Constring = "DefaultConnection";
    }
}