using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Voice
{
    public class CallModel : PageModel
    {
        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter phone number: 5555555555")]
        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public VoiceUpload VoiceUpload { get; set; }

        private readonly IRepository _db;
        private readonly ICallService _call;

        public CallModel(IRepository repository, ICallService call)
        {
            _db = repository;
            _call = call;
        }

        public void OnGet(Guid? Id)
        {
            VoiceUpload = _db.Single<VoiceUpload>(x => x.Id == Id);
            if (VoiceUpload != null)
            {

            }
        }

        public async Task<IActionResult> OnPost()
        {
            string domainName = Request.HttpContext.Request.GetDisplayUrl().Replace(Request.Path, String.Empty);
            var url = "http://demo.kevinwhinnery.com/audio/zelda.mp3";
            //await _call.PreRecordedCall(PhoneNumber, domainName + "/" + VoiceUpload.FilePath);
            await _call.PreRecordedCall(PhoneNumber, url);


            return Page();
        }
    }
}