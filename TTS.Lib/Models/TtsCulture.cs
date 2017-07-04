using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsCulture
    {
        public TtsCulture()
        {
            TtsArticle = new HashSet<TtsArticle>();
            TtsBanner = new HashSet<TtsBanner>();
            TtsMenu = new HashSet<TtsMenu>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Lcid { get; set; }
        public string Alias { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<TtsArticle> TtsArticle { get; set; }
        public virtual ICollection<TtsBanner> TtsBanner { get; set; }
        public virtual ICollection<TtsMenu> TtsMenu { get; set; }
    }
}
