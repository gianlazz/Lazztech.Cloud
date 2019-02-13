using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Lazztech.Events.Dal.Dao;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Lazztech.Cloud.ClientFacade.Pages.Admin.Voice
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [Required]
        public IFormFile Audio { get; set; }
        [BindProperty]
        public AudioUpload NewUpload { get; set; }
        [BindProperty]
        public List<AudioUpload> Uploads { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public IndexModel(ApplicationDbContext applicationDbContext, IFileService fileService)
        {
            _context = applicationDbContext;
            _fileService = fileService;
        }

        public async Task OnGetAsync()
        {
            Uploads = await _context.AudioUploads.ToListAsync();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Attach(NewUpload).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await Upload();
            await _context.SaveChangesAsync();

            return Page();
        }

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