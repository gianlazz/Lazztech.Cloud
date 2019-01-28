using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Lazztech.Standard.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Mentors
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        [Required]
        public IFormFile Photo { get; set; }
        [BindProperty]
        public Mentor Mentor { get; set; }

        private readonly IRepository _repo;
        private readonly IFileService _fileService;

        public CreateModel(IRepository repository, IFileService fileService)
        {
            _repo = repository;
            _fileService = fileService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Mentor.Add(Mentor);
            //await _context.SaveChangesAsync();

            await UploadPhoto();
            _repo.Add<Mentor>(Mentor);

            return RedirectToPage("./Index");
        }

        private async Task UploadPhoto()
        {
            using (var ms = new MemoryStream())
            {
                await Photo.CopyToAsync(ms);
                var extension = _fileService.GetExtension(Photo.FileName);
                var imageBytes = ms.ToArray();

                var directory = @"C:\LazztechCloud\";
                var fileName = Mentor.Id + extension;
                var imagePath = directory + fileName;
                Mentor.Image = imagePath;
                _fileService.WriteAllBytes(imagePath, imageBytes);
            }
        }
    }
}