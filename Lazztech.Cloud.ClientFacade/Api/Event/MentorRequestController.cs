using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal;
using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Domain;
using Lazztech.Events.Dto.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Controllers.Event
{
    [Route("api/Event/[controller]")]
    public class MentorRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMentorRequestConductor _conductor;
        private readonly ISmsService _sms;

        public MentorRequestController(ApplicationDbContext applicationDbContext, IMentorRequestConductor conductor, ISmsService sms)
        {
            _context = applicationDbContext;
            _conductor = conductor;
            _sms = sms;
        }

        //MentorRequest
        [HttpGet]
        public IActionResult Get()
        {
             return new JsonResult(new { message = "Your request has been sent! Please wait for your reply." });
        }

        //MentorRequest
        [HttpPost]
        public void Post(string uniqueRequesteeId, string teamName, string teamLocation, int mentorId)
        {
            _conductor.SubmitRequest(uniqueRequesteeId, teamName, teamLocation, mentorId);
        }

        #region Depricated Team Login/Logout
        //[HttpPost]
        //public ActionResult TeamLogin(int teamPin)
        //{
        //    var cookie = Request.Cookies[StaticStrings.eventUserIdCookieName];

        //    //CHECK IF A TEAM BY THAT PIN NUMBER EXSISTS
        //    if (_db.Single<Team>(x => x.PinNumber == teamPin) != null)
        //    {
        //        Team team = _db.Single<Team>(x => x.PinNumber == teamPin);

        //        if (cookie == null)
        //        {
        //            //var option = new CookieOptions();
        //            //option.Expires = DateTime.UtcNow.AddDays(3);
        //            //Response.Cookies[StaticStrings.eventUserIdCookieName].Value = team.Name;
        //            //Response.Cookies.Append("string", team.Name, option);
        //            Response.Cookies.Append(StaticStrings.eventUserIdCookieName, team.Name, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(3) });

        //            //Response.Cookies[StaticStrings.eventUserIdCookieName].Expires = DateTime.UtcNow.AddDays(3);
        //        }
        //        if (cookie != null && cookie.ToString() == "")
        //        {
        //            //Response.Cookies[StaticStrings.eventUserIdCookieName].Value = team.Name;
        //            //Response.Cookies[StaticStrings.eventUserIdCookieName].Expires = DateTime.UtcNow.AddDays(3);
        //            Response.Cookies.Append(StaticStrings.eventUserIdCookieName, team.Name, new CookieOptions() { Expires = DateTime.UtcNow.AddDays(3) });
        //        }
        //        //if (cookie != null && cookie.Value == "")
        //        //{
        //        //    Response.Cookies[StaticStrings.eventUserIdCookieName].Value = team.Name;
        //        //    Response.Cookies[StaticStrings.eventUserIdCookieName].Expires = DateTime.UtcNow.AddDays(3);
        //        //}
        //    }
        //    //return Redirect("");
        //    return RedirectToPage("/Events/Event/Index");
        //}

        //public ActionResult LogOut()
        //{
        //    HttpCookie cookie = Request.Cookies[StaticStrings.eventUserIdCookieName];
        //    if (cookie != null)
        //    {
        //        Response.Cookies[StaticStrings.eventUserIdCookieName].Value = "";
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return RedirectToAction("Index");
        //}
        #endregion
    }
}