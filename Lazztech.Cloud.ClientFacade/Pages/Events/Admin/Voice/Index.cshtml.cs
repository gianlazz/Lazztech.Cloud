using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Voice
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [Required]
        public IFormFile Audio { get; set; }
        [BindProperty]
        public VoiceUpload NewUpload { get; set; }
        public List<VoiceUpload> Uploads { get; set; }

        private readonly IRepository _db;
        private readonly IFileService _fileService;

        public IndexModel(IRepository repository, IFileService fileService)
        {
            _db = repository;
            _fileService = fileService;
        }

        public void OnGet()
        {
            Uploads = _db.All<VoiceUpload>().ToList();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await Upload();
            _db.Add<VoiceUpload>(NewUpload);

            return Page();
        }

        //public IActionResult OnPostCall(Guid Id)
        //{
        //    RedirectToPage()
        //} 

        private async Task Upload()
        {
            using (var ms = new MemoryStream())
            {
                await Audio.CopyToAsync(ms);
                var extension = _fileService.GetExtension(Audio.FileName);
                var imageBytes = ms.ToArray();

                var directory = StaticStrings.dataDir;
                var fileName = NewUpload.FileName;
                var filePath = directory + fileName + extension;
                NewUpload.FilePath = filePath;
                if (_fileService.FileExists(filePath))
                    throw new Exception("Tried to save an audio file with an existing filename.");
                _fileService.WriteAllBytes(filePath, imageBytes);
            }
        }
    }
}