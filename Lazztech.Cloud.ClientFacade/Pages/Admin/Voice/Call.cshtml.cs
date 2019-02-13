using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;
using Lazztech.Events.Dto.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.Voice
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
        public AudioUpload AudioUpload { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly ICallService _call;

        public CallModel(ApplicationDbContext applicationDbContext, ICallService callService)
        {
            _context = applicationDbContext;
            _call = callService;
        }

        public async Task OnGet(int Id)
        {
            AudioUpload = await _context.AudioUploads.FirstOrDefaultAsync(x => x.AudioUploadId == Id);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _call.PreRecordedCall(PhoneNumber, AudioUpload.AudioUploadId.ToString());

            return Page();
        }
    }
}