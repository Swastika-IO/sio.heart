using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsArticle
    {
        public TtsArticle()
        {
            TtsMenuArticle = new HashSet<TtsMenuArticle>();
        }

        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Menus { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }
        public int? Views { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string BriefContent { get; set; }
        public string FullContent { get; set; }
        public string Sename { get; set; }
        public string Seotitle { get; set; }
        public string Seodescription { get; set; }
        public string Seokeywords { get; set; }
        public string Source { get; set; }
        public int? Position { get; set; }
        public int? MenuId { get; set; }
        public int? Index { get; set; }
        public bool IsVisible { get; set; }
        public bool? Hot { get; set; }
        public int Type { get; set; }

        public virtual ICollection<TtsMenuArticle> TtsMenuArticle { get; set; }
        public virtual TtsCulture SpecificultureNavigation { get; set; }
    }
}
