using System;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;

namespace Swastika.Common
{
    public class CommonHelper
    {
        //private static string defaultImagePath = "http://placehold.it/200x200";
        public static string ParseJsonPropertyName(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return Char.ToLower(input[0]) + input.Substring(1);
            }
            else
            {
                return string.Empty;
            }
            
        }

        public static Stream LoadImage(string strImage64)
        {
            //data:image/gif;base64,
            //this image is a single pixel (black)
            try
            {
                string imgData = strImage64.Substring(strImage64.IndexOf(',') + 1);
                //byte[] bytes = Convert.FromBase64String(imgData);

                //Image image;
                //using (MemoryStream ms = new MemoryStream(bytes))
                //{
                //    image = Image.FromStream(ms);
                //}

                //return image;
                byte[] imageBytes = Convert.FromBase64String(imgData);
                // Convert byte[] to Image
                return new MemoryStream(imageBytes, 0, imageBytes.Length);
                //using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                //{
                //    Image image = Image.FromStream(ms, true);
                //    return image;
                //}
            }
            catch//(Exception ex)
            {
                return null;
            }
        }

        //public static string UploadPhoto(string fullPath, Image img)
        //{

        //    try
        //    {
        //        if (!Directory.Exists(fullPath))
        //        {
        //            Directory.CreateDirectory(fullPath);
        //        }

        //        if (img != null)
        //        {
        //            //string fileExt = GetFilenameExtension(img.RawFormat);
        //            //file_name = (guid + fileExt).Trim();
        //            //file_dir = filePath + file_name;
        //            //ImageResizer.ResizeStream(TTXConstants.Params.photoSize, img, file_dir);

        //            return ImageHelper.ResizeImage(img, fullPath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Empty;
        //    }
        //    return string.Empty;
        //}

        public static string UploadFile(
            string fullPath, FileInfo file)
        {

            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                if (file != null)
                {
                    //var fileName = ContentDispositionHeaderValue.Parse
                    //    (file.ContentDisposition).FileName.Trim('"');
                    string fileName = string.Format("{0}.{1}",
                        Guid.NewGuid().ToString("N"),
                        file.FullName.Split('.').Last());
                    using (var fileStream = new FileStream(Path.Combine(fullPath, fileName), FileMode.Create, FileAccess.ReadWrite))
                    {
                        file.CopyTo(Path.Combine(fullPath, fileName));
                        return fileName;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }

        }


        public static async System.Threading.Tasks.Task<string> GetWebResponseAsync(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                using (Stream resStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
        }

        public static string ReadFromFile(string filename)
        {
            string s = "";
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(file);
                s = sr.ReadToEnd();
                sr.Dispose();
                file.Dispose();
            }
            catch
            {
                s = "";
            }
            return s;
        }


        public static bool RemoveFile(string filePath)
        {
            bool result = false;
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }


        private static char[] _base62chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            .ToCharArray();

        private static Random _random = new Random();

        public static string GetBase62(int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(62)]);

            return sb.ToString();
        }
    }
    
}
