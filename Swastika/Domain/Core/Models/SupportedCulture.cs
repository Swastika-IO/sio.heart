using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.Domain.Core.Models
{
    public class SupportedCulture
    {
        public string Specificulture { get; set; }
        public string Icon { get; set; }
        public bool IsSupported { get; set; }
    }
}
