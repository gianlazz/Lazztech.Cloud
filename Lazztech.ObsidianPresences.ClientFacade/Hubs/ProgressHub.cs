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
                Clients.Group(request.Team.Name).RequestUpdate(request, "Mentor Accepted Request.");
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
            Cookie cookie = Context.Request.Cookies["team"];
            //HttpCookie cookie = HttpContext.Request.Cookies.Get("team");
            //HttpCookie cookie = HttpContext.Current.Request.Cookies["team"];
            if (cookie != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, cookie.Value);
            }

        }

        public void MessageTeam(Team team, string message)
        {
            Clients.Group(team.Name).message(message);
        }

        //https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups
        //public Task JoinRoom(string roomName)
        //{
        //    return Groups.Add(Context.ConnectionId, roomName);
        //}

        //public Task LeaveRoom(string roomName)
        //{
        //    return Groups.Remove(Context.ConnectionId, roomName);
        //}

        public static ConcurrentDictionary<string, Team> MyUsers = new ConcurrentDictionary<string, Team>();

        public override Task OnConnected()
        {
            Cookie usersTeamCookie = null;
            if (Context.Request.Cookies.ContainsKey("team"))
                usersTeamCookie = Context.Request.Cookies["team"];
            if (usersTeamCookie != null)
            {
                MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = usersTeamCookie.Value });
                //string name = Context.User.Identity.Name;

                //Groups.Add(Context.ConnectionId, name);
                Groups.Add(Context.ConnectionId, usersTeamCookie.Value);

                if (usersTeamCookie.Value != null)
                {
                    MyUsers.TryAdd(Context.ConnectionId, new Team() { Name = usersTeamCookie.Value });
                    //string name = Context.User.Identity.Name;

                    //Groups.Add(Context.ConnectionId, name);

                    //Groups.Add(Context.ConnectionId, cookie.Value);
                    Groups.Add(Context.ConnectionId, usersTeamCookie.Value);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Team team;
            MyUsers.TryRemove(Context.ConnectionId, out team);
            return base.OnDisconnected(stopCalled);
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
    }
