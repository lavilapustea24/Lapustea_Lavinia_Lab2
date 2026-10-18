using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lapustea_Lavinia_Lab2.Models;

namespace Lapustea_Lavinia_Lab2.Pages.AuthorsPages;

public class CreateModel : PageModel
{
    private readonly Lapustea_Lavinia_Lab2Context _context;

    public CreateModel(Lapustea_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Authors Authors { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Authors.Add(Authors);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
