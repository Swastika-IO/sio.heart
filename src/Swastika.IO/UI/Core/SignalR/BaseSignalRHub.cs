using Microsoft.AspNetCore.SignalR;
using Swastika.IO.Domain.Core.Models;
using Swastika.IO.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Swastika.IO.UI.Core.SignalR
{
    public abstract class BaseSignalRHub : Hub
    {
        private static readonly List<SignalRClient> Users = new List<SignalRClient>();

        /// <summary>
        /// Fails the result.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="errorMsg">The error MSG.</param>
        void FailResult(dynamic objData, string errorMsg)//AccessTokenViewModel accessToken
        {
            string responseKey = "Failed";
            int status = 0;
            ApiResult<dynamic> result = new ApiResult<dynamic>()
            {
                ResponseKey = responseKey,
                Status = status,
                Data = objData,
                //authData = accessToken,
            };
            Clients.Client(Context.ConnectionId).InvokeAsync("receiveMessage", result);
        }

        public virtual void UpdatePlayerConnectionIdAsync(string playerId)
        {
            var player = Users.Find(p => p.UserId == playerId);
            if (player != null && player.ConnectionId != Context.ConnectionId)
            {               
                //Missing Update current groups user connId

                player.ConnectionId = Context.ConnectionId;
                //player.SaveModel();
            }
        }

        public virtual void UpdateGroupConnection()
        {
            var user = Users.Find(p => p.ConnectionId == Context.ConnectionId);
            if (user != null)
            {
                // Loop Group to Update UserId
                //player.ConnectionId = Context.ConnectionId;
            }
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the user
            Users.RemoveAll(u => u.ConnectionId == Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public override Task OnConnectedAsync()
        {
            UpdateGroupConnection();
            return base.OnConnectedAsync();
        }
    }
}
