using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public IFormFile Photo { get; set; }
        [BindProperty]
        public Mentor Mentor { get; set; }

        private readonly IDalHelper _db;
        private readonly IFileService _fileService;

        public EditModel(IDalHelper dal, IFileService fileService)
        {
            _db = dal;
            _fileService = fileService;
        }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);
            Mentor = _db.All<Mentor>().FirstOrDefault(m => m.Id == id);

            if (Mentor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (!MentorExists(Mentor.Id))
            {
                return NotFound();
            }

            if (Photo != null)
            {
                await UploadPhoto();
                _db.Delete<Mentor>(x => x.Id == Mentor.Id);
                _db.Add<Mentor>(Mentor);
            }
            else
            {
                Mentor.Image = _db.All<Mentor>().FirstOrDefault(m => m.Id == Mentor.Id).Image;
                _db.Delete<Mentor>(x => x.Id == Mentor.Id);
                _db.Add<Mentor>(Mentor);
            }

            return RedirectToPage("./Index");
        }

        private bool MentorExists(Guid id)
        {
            return _db.All<Mentor>().Any(x => x.Id == id);
            //return _context.Mentor.Any(e => e.Id == id);
        }

        private async Task UploadPhoto()
        {
            using (var ms = new MemoryStream())
            {
                await Photo.CopyToAsync(ms);
                var extension = _fileService.GetExtension(Photo.FileName);
                var imageBytes = ms.ToArray();

                var directory = StaticStrings.dataDir;
                var fileName = Mentor.Id + extension;
                var imagePath = directory + fileName;

                if (_fileService.FileExists(imagePath))
                    _fileService.DeleteFile(imagePath);

                Mentor.Image = imagePath;
                _fileService.WriteAllBytes(imagePath, imageBytes);
            }
        }
    }
}