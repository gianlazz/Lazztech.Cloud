﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.ObsidianPresences.Vision.Microservice.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lazztech.ObsidianPresences.ClientFacade.Pages.Vision
{
    public class UploadModel : PageModel
    {
        public Person Person { get; set; }
        public string Base64Photo { get; set; }
        public Snapshot Snapshot { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer.PhotePath = Customer.Phote.FileName;
            await UploadPhoto();
            _db.Customers.Add(Customer);
            await _db.SaveChangesAsync();

            Message = "New customer created successfully!";

            return RedirectToPage("./Index");
        }

        private async Task UploadPhoto()
        {
            var uploadsDirectoryPath = Path.Combine(_environment.WebRootPath, "Uploads");
            var uploadedfilePath = Path.Combine(uploadsDirectoryPath, Customer.Phote.FileName);

            using (var fileStream = new FileStream(uploadedfilePath, FileMode.Create))
            {
                await Customer.Phote.CopyToAsync(fileStream);
            }
        }
    }
}