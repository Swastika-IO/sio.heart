using System;
using System.Collections.Generic;
using System.Text;

namespace Swastika.IO.Domain.Core.Models
{
    public class SignalRClient
    {
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string ConnectionId { get; set; }
        public DateTime JoinedDate { get; set; }
    }
}
