using HackathonManager.Models;
using HackathonManager.PocoModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.ObsidianPresences.ClientFacade.Hubs
{
    public class ProgressHub : Hub
    {
        public void UpdateTeam(MentorRequest request, string message)
        {
            if (request.RequestAccepted)
            {
                Clients.Group(request.TeamName).SendAsync("RequestUpdate", request, "Mentor Accepted Request.");
            }
        }

        public Task<string> GetConnectionId()
        {
            return Task.Run(() =>
            {
                return Context.ConnectionId;
            });
        }

        public void AddConnectionToTeamGroup()
        {
            var teamCookieValue = Context.GetHttpContext().Request.Cookies["team"];

            if (!string.IsNullOrWhiteSpace(teamCookieValue))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, teamCookieValue);
            }

        }

        public void MessageTeam(Team team, string message)
        {
            Clients.Group(team.Name).SendAsync("message", message);
        }

        public static ConcurrentDictionary<string, Team> MyUsers = new ConcurrentDictionary<string, Team>();

        public override Task OnConnectedAsync()
        {
            string usersTeamCookie = null;
            if (Context.GetHttpContext().Request.Cookies.ContainsKey("team"))
                usersTeamCookie = Context.GetHttpContext().Request.Cookies["team"];
            if (usersTeamCookie != null)
            {
                MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = usersTeamCookie });
                //string name = Context.User.Identity.Name;

                //Groups.Add(Context.ConnectionId, name);
                Groups.AddToGroupAsync(Context.ConnectionId, usersTeamCookie);

                if (usersTeamCookie != null)
                {
                    MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = usersTeamCookie });
                    //string name = Context.User.Identity.Name;

                    //Groups.Add(Context.ConnectionId, name);

                    //Groups.Add(Context.ConnectionId, cookie.Value);
                    Groups.AddToGroupAsync(Context.ConnectionId, usersTeamCookie);
                }
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Team team;
            MyUsers.TryRemove(Context.ConnectionId, out team);
            return base.OnDisconnectedAsync(exception);
        }

        //public override Task OnReconnected()
        //{
        //    Cookie cookie = Context.Request.Cookies["team"];
        //    if (cookie != null)
        //    {
        //        MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = cookie.Value });
        //        //string name = Context.User.Identity.Name;

        //        //Groups.Add(Context.ConnectionId, name);
        //        Groups.Add(Context.ConnectionId, cookie.Name);

        //        if (cookie.Value != null)
        //        {
        //            MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = cookie.Value });
        //            //string name = Context.User.Identity.Name;

        //            //Groups.Add(Context.ConnectionId, name);
        //            Groups.Add(Context.ConnectionId, cookie.Name);
        //        }
        //    }

        //    return base.OnReconnected();
        //}

        public void UpdateTeamOfMentorRequest(string teamName, bool accepted, string message = null)
        {
            var percentage = (20 * 100) / 100;

            //PUSHING DATA TO ALL CLIENTS

            //Clients.Group(teamName).requestUpdate(message, percentage);
            Clients.Group(teamName).SendAsync("requestUpdate", message, percentage);
        }
    }
}
