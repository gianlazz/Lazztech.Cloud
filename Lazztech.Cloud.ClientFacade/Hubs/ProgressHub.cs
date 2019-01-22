using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Hubs
{
    public class ProgressHub : Hub
    {
        //This is for debugging the being added to a group
        public static ConcurrentDictionary<string, string> GroupsForDebugging = new ConcurrentDictionary<string, string>();

        public Task<string> GetConnectionId()
        {
            return Task.Run(() =>
            {
                return Context.ConnectionId;
            });
        }

        public override Task OnConnectedAsync()
        {
            string usersUniqueIDCookie = null;
            if (Context.GetHttpContext().Request.Cookies.ContainsKey(StaticStrings.eventUserIdCookieName))
                usersUniqueIDCookie = Context.GetHttpContext().Request.Cookies[StaticStrings.eventUserIdCookieName];
            if (usersUniqueIDCookie != null)
            {
                GroupsForDebugging.TryAdd(Context.ConnectionId, usersUniqueIDCookie);
                //Groups.Add(Context.ConnectionId, name);
                //Groups.Add(Context.ConnectionId, cookie.Value);
                Groups.AddToGroupAsync(Context.ConnectionId, usersUniqueIDCookie);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            GroupsForDebugging.TryRemove(Context.ConnectionId, out string uniqueUserId);
            return base.OnDisconnectedAsync(exception);
        }

        public void ThrowException()
        {
            throw new Exception();
        }

        public void UpdateTeamOfMentorRequest(string teamName, bool accepted, string message = null)
        {
            var percentage = (20 * 100) / 100;

            //PUSHING DATA TO ALL CLIENTS

            //Clients.Group(teamName).requestUpdate(message, percentage);
            Clients.Group(teamName).SendAsync("requestUpdate", message, percentage);
        }
    }
}