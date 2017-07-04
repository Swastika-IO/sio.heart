using System;
using System.Collections.Generic;

namespace TTS.Lib.Models
{
    public partial class TtsComment
    {
        public Guid CommentId { get; set; }
        public int? ArticleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string EditedBy { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public bool? IsView { get; set; }
    }
}
