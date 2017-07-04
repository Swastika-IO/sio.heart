using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsMenuArticle
    {
        public int ArticleId { get; set; }
        public int MenuId { get; set; }
        public string Specificulture { get; set; }

        public virtual TtsArticle TtsArticle { get; set; }
        public virtual TtsMenu TtsMenu { get; set; }
    }
}
