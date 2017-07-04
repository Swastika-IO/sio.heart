using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsMenuMenu
    {
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public string Specificulture { get; set; }

        public virtual TtsMenu TtsMenu { get; set; }
        public virtual TtsMenu TtsMenuNavigation { get; set; }
    }
}
