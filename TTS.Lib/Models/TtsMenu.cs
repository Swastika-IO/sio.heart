using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsMenu
    {
        public TtsMenu()
        {
            TtsMenuArticle = new HashSet<TtsMenuArticle>();
            TtsMenuMenuTtsMenu = new HashSet<TtsMenuMenu>();
            TtsMenuMenuTtsMenuNavigation = new HashSet<TtsMenuMenu>();
        }

        public int MenuId { get; set; }
        public string Specificulture { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Views { get; set; }
        public int? Position { get; set; }
        public bool IsDeleted { get; set; }
        public string Sename { get; set; }
        public string Seotitle { get; set; }
        public string Seodescription { get; set; }
        public string Seokeywords { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? Level { get; set; }
        public int? ParentMenuId { get; set; }

        public virtual ICollection<TtsMenuArticle> TtsMenuArticle { get; set; }
        public virtual ICollection<TtsMenuMenu> TtsMenuMenuTtsMenu { get; set; }
        public virtual ICollection<TtsMenuMenu> TtsMenuMenuTtsMenuNavigation { get; set; }
        public virtual TtsCulture SpecificultureNavigation { get; set; }
    }
}
