using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsBanner
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public string Url { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedBy { get; set; }

        public virtual TtsCulture SpecificultureNavigation { get; set; }
    }
}
