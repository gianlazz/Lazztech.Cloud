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
        [Required]
        public IFormFile Photo { get; set; }
        [BindProperty]
        public Mentor Mentor { get; set; }

        private readonly IRepository _repo;
        private readonly IFileService _fileService;

        public EditModel(IRepository repository, IFileService fileService)
        {
            _repo = repository;
            _fileService = fileService;
        }

        public IActionResult OnGet(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Mentor = await _context.Mentor.FirstOrDefaultAsync(m => m.Id == id);
            Mentor = _repo.All<Mentor>().FirstOrDefault(m => m.Id == id);

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
            await UploadPhoto();
            _repo.Delete<Mentor>(x => x.Id == Mentor.Id);
            _repo.Add<Mentor>(Mentor);

            return RedirectToPage("./Index");
        }

        private bool MentorExists(Guid id)
        {
            return _repo.All<Mentor>().Any(x => x.Id == id);
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
                Mentor.Image = imagePath;
                _fileService.WriteAllBytes(imagePath, imageBytes);
            }
        }
    }
}