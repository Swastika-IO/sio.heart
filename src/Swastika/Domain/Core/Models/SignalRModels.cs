using System;

namespace Swastika.Domain.Core.Models
{
    /// <summary>
    ///
    /// </summary>
    public class SignalRClient
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the nick.
        /// </summary>
        /// <value>
        /// The name of the nick.
        /// </value>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets the connection identifier.
        /// </summary>
        /// <value>
        /// The connection identifier.
        /// </value>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Gets or sets the joined date.
        /// </summary>
        /// <value>
        /// The joined date.
        /// </value>
        public DateTime JoinedDate { get; set; }
    }
}