using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Swastika.Common.Helper
{
    public class PaginationModel<T>
    {
       
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
        public PaginationModel()
        {
            PageIndex = 0;
            PageSize = 0;
            TotalItems = 0;
            TotalPage = 1;
            Items = new List<T>();
        }
    }
    public class FileViewModel
    {
        public string FullPath
        {
            get
            {
                string fullPath = string.Format(Constants.StringTemplates.FileFolder, FileFolder);
                return string.Format(@"/{0}/{1}.{2}", fullPath.Replace(@"wwwroot/", string.Empty), Filename, Extension);
            }
            set { }
        }
        public string FileFolder { get; set; }
        [Required]
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string Content { get; set; }
        public string FileStream { get; set; }
    }

    public class TemplateViewModel
    {        
        public string FileFolder { get; set; }
        [Required]
        public string Filename { get; set; }
        public string Extension { get; set; }
        public string Content { get; set; }
        public string FileStream { get; set; }
    }
}
