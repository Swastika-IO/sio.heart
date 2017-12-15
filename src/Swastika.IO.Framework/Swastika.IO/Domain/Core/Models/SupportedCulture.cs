using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Domain.Core.Models
{
    public class SupportedCulture
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Lcid { get; set; }
        public string Alias { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public bool IsSupported { get; set; }
    }
}
