using Lazztech.Events.Dto.Interfaces;
using Lazztech.Events.Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lazztech.Cloud.ClientFacade.Pages.Events.Admin.Judges
{
    public class EditModel : PageModel
    {
        //private readonly Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext _context;

        //public EditModel(Lazztech.Cloud.ClientFacade.Data.ApplicationDbContext context)
        //{
        //    _context = context;
        //}
        private IRepository _repo = Startup.DbRepo;

        [BindProperty]
        public Judge Judge { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Judge = await _context.Judge.FirstOrDefaultAsync(m => m.Id == id);
            Judge = _repo.All<Judge>().FirstOrDefault(m => m.Id == id);

            if (Judge == null)
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

            //_context.Attach(Judge).State = EntityState.Modified;

            try
            {
                _repo.Delete<Judge>(x => x.Id == Judge.Id);
                _repo.Add<Judge>(Judge);
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JudgeExists(Judge.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool JudgeExists(Guid id)
        {
            return _repo.All<Judge>().Any(x => x.Id == id);
            //return _context.Judge.Any(e => e.Id == id);
        }
    }
}