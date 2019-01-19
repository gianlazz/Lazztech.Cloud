﻿using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Judges
{
    public class CreateModel : PageModel
    {
        private readonly IRepository _repo;

        public CreateModel(IRepository repository)
        {
            _repo = repository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Judge Judge { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //_context.Judge.Add(Judge);
            //await _context.SaveChangesAsync();
            _repo.Add<Judge>(Judge);

            return RedirectToPage("./Index");
        }
    }
}