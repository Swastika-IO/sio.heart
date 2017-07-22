using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Swastika.Common.Helper
{
    public class ImageHelper
    {

        public static Image CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }
        public static void ResizeStream(int imageSize, Stream fileStream, string outputPath)
        {
            try
            {
                var image = Image.FromStream(fileStream);
                int thumbnailSize = imageSize;
                int newWidth, newHeight;

                if (image.Width > image.Height)
                {
                    newWidth = thumbnailSize;
                    newHeight = image.Height * thumbnailSize / image.Width;
                }
                else
                {
                    newWidth = image.Width * thumbnailSize / image.Height;
                    newHeight = thumbnailSize;
                }

                var thumbnailBitmap = new Bitmap(newWidth, newHeight);

                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbnailGraph.DrawImage(image, imageRectangle);

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        thumbnailBitmap.Save(memory, image.RawFormat);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }

                //thumbnailBitmap.Save(outputPath, image.RawFormat);
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            finally
            {

            }
        }

        public static string ResizeImage(Image img, string outputPath)
        {
            const int size = 150;
            const int quality = 75;
            string[] supports = new string[] { "", ".jpg", ".png", ".jpeg" };
            //int limit_size = 5 * 1024 * 1024;  //limit size 5MB
            string file_name = string.Empty;
            string file_dir = string.Empty;
            using (var image = new Bitmap(img))
            {
                string fileExt = GetFilenameExtension(image.RawFormat);
                //int file_size = img.;
                string guid = Guid.NewGuid().ToString("N");
                file_name = (guid + fileExt).Trim();
                outputPath = Path.Combine(outputPath, file_name);
                if (AllowEtension(fileExt, supports))// && file_size <= limit_size)
                {
                    //Save original file.

                    int width, height;
                    if (image.Width > image.Height)
                    {
                        width = size;
                        height = Convert.ToInt32(image.Height * size / (double)image.Width);
                    }
                    else
                    {
                        width = Convert.ToInt32(image.Width * size / (double)image.Height);
                        height = size;
                    }
                    var resized = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.DrawImage(image, 0, 0, width, height);

                        using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            var qualityParamId = System.Drawing.Imaging.Encoder.Quality;
                            var encoderParameters = new EncoderParameters(1);
                            encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                            var codec = ImageCodecInfo.GetImageDecoders()
                                .FirstOrDefault(cd => cd.FormatID == ImageFormat.Jpeg.Guid);
                            image.Save(outputPath, codec, encoderParameters);
                        }
                    }

                }
                return file_name;
            }
        }
        public static bool AllowEtension(string file_ext, string[] exts)
        {
            bool support = false;
            foreach (string ext in exts)
            {
                if (ext == file_ext)
                {
                    support = true;
                    break;
                }
            }
            return support;
        }
        public static string GetFilenameExtension(ImageFormat format)
        {
            
            var encoders = ImageCodecInfo.GetImageEncoders();
            var ec = encoders.FirstOrDefault(x => x.FormatID == format.Guid);
            if (ec != null)
            {

                string exts = ec.FilenameExtension;
                return exts.Split(';').First().Replace("*", "").ToLower();

            }
            else
            { return string.Empty; }
        }
        public static void ResizeStream(Image image, string outputPath, int imageSize = 1200)
        {
            try
            {
                int thumbnailSize = imageSize;
                int newWidth, newHeight;

                if (image.Width > image.Height)
                {
                    newWidth = thumbnailSize;
                    newHeight = image.Height * thumbnailSize / image.Width;
                }
                else
                {
                    newWidth = image.Width * thumbnailSize / image.Height;
                    newHeight = thumbnailSize;
                }

                var thumbnailBitmap = new Bitmap(newWidth, newHeight);

                var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbnailGraph.DrawImage(image, imageRectangle);

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        thumbnailBitmap.Save(memory, image.RawFormat);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }

                //thumbnailBitmap.Save(outputPath, image.RawFormat);
                thumbnailGraph.Dispose();
                thumbnailBitmap.Dispose();
                image.Dispose();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            finally
            {

            }
        }
        public static Image GetResizedImage(String path, float width, float height, bool isCrop)
        {
            return GetResizedImage(path, width, height, isCrop, false);
        }

        public static Image GetResizedImage(String path, float width, float height, bool isCrop, bool isVertical)
        {
            var image = (Image)new Bitmap(path);

            int thumbnailSize = (int)width;
            int newWidth, newHeight;


            if (image.Width > image.Height)
            {
                newWidth = thumbnailSize;
                newHeight = image.Height * thumbnailSize / image.Width;

                if (isVertical)
                {
                    if (newHeight < height)
                    {
                        newHeight = thumbnailSize;
                        newWidth = image.Width * thumbnailSize / image.Height;
                    }
                }

            }
            else
            {
                newWidth = thumbnailSize;
                newHeight = (image.Height * thumbnailSize) / image.Width; ;
            }

            System.IO.MemoryStream outStream = new System.IO.MemoryStream();

            var thumbnailBitmap = new Bitmap(newWidth, newHeight);

            var thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
            thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            thumbnailGraph.CompositingMode = CompositingMode.SourceCopy;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);

            thumbnailGraph.DrawImage(image, imageRectangle);
            thumbnailBitmap.Save(outStream, GetImageFormat(path));

            byte[] buffer = outStream.ToArray();
            MemoryStream ms = new MemoryStream(buffer);
            Image returnImage = Image.FromStream(ms);

            if (isCrop)
            {


                Bitmap bmp = returnImage as Bitmap;
                height = height > bmp.Height ? bmp.Height : height;
                width = width > bmp.Width ? bmp.Width : width;
                Rectangle cropRect = new Rectangle(0, 0, (int)width, (int)height);
                // Check if it is a bitmap:
                if (bmp == null)
                    throw new ArgumentException("No valid bitmap");

                // Crop the image:
                Bitmap cropBmp = bmp.Clone(cropRect, bmp.PixelFormat);

                // Release the resources:
                returnImage.Dispose();

                return cropBmp;
            }

            return returnImage;
        }

        public static string GetContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }

        public static ImageFormat GetImageFormat(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }
            return ImageFormat.Jpeg;
        }
    }
}
