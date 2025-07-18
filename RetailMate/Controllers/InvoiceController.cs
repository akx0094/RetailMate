using Microsoft.AspNetCore.Mvc;
using RetailMate.Models; // Ensure this namespace is correct for your Invoice model
using System.Text.Json;
using System.Linq; // Required for .Where().Select().ToList()
using System.Collections.Generic; // Required for List<Claim> if Authorize was active
using System.Diagnostics; // Required for Debug.WriteLine
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]

namespace RetailMate.Controllers // Adjust namespace to match your project
{
    [Authorize] // This attribute protects all actions in this controller, requiring authentication.
    public class InvoiceController : Controller
    {
        public IActionResult Create()
        {
            return View(new Invoice());
        }

        [HttpPost]
        public IActionResult Create(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                // invoice.BillNumber = $"BILL-{DateTime.Now.Ticks}"; // Removed as per your provided code
                invoice.Date = invoice.Date == default ? DateTime.Now : invoice.Date;

                // Store invoice as JSON in TempData
                TempData["Invoice"] = JsonSerializer.Serialize(invoice);

                return RedirectToAction("InvoicePdf");
            }
            else // This 'else' block is important to ensure debug output only runs when model is NOT valid
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToList();

                System.Diagnostics.Debug.WriteLine("Model Errors:");
                foreach (var err in errors)
                {
                    foreach (var e in err.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Property: {err.Key} - Error: {e.ErrorMessage}");
                    }
                }
            }

            return View(invoice); // Return the view with validation errors if ModelState is not valid
        }

        public IActionResult InvoicePdf()
        {
            if (TempData["Invoice"] == null)
                return RedirectToAction("Create"); // Redirect back if no invoice data

            // Deserialize back to Invoice model
            TempData.Keep("Invoice"); // Keep data for subsequent refreshes if needed
            var json = TempData["Invoice"] as string;
            var invoice = JsonSerializer.Deserialize<Invoice>(json);

            return View(invoice);
        }
    }
}
