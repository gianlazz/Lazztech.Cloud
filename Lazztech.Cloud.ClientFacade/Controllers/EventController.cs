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

namespace Lazztech.Cloud.ClientFacade.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMentorRequestConductor _conductor;
        private readonly ISmsService _sms;

        public EventController(ApplicationDbContext applicationDbContext, IMentorRequestConductor conductor, ISmsService sms)
        {
            _context = applicationDbContext;
            _conductor = conductor;
            _sms = sms;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> MentorRequest(string uniqueRequesteeId, string teamName, string teamLocation, int mentorId)
        {
            bool succeded = false;

            var mentor = await _context.Mentors.SingleOrDefaultAsync(x => x.MentorId == mentorId);
            if (mentor != null)
            {
                var message = EventStrings.OutBoundRequestSms(mentor.FirstName, teamName, teamLocation);
                var request = new MentorRequest()
                {
                    UniqueRequesteeId = uniqueRequesteeId,
                    Mentor = mentor,
                    OutboundSms = _sms.SendSms(mentor.PhoneNumber, message).MapToEntity()
                };
                succeded = _conductor.TryAddRequest(request);
                if (succeded)
                {
                    _context.MentorRequests.Add(request);
                    await _context.SaveChangesAsync();
                }
            }

            if (succeded)
                return RedirectToPage("/Event/Index", new { message = "Your request has been sent! Please wait for your reply." });
            else
                return RedirectToPage("/Event/Index", new { alert = "The mentor you selected is currently helping someone else! Please select another." });
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