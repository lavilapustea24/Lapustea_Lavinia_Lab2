
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lapustea_Lavinia_Lab2.Models;

namespace Lapustea_Lavinia_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Lapustea_Lavinia_Lab2Context _context;

    public EditModel(Lapustea_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing
            .FirstOrDefaultAsync(m => m.ID == id);

        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

        LoadDropdowns();

        return Page();
    }

    private void LoadDropdowns()
    {
        var bookList = _context.Book
            .Include(b => b.Authors)
            .Select(x => new
            {
                x.ID,
                BookFullName = x.Title + " - " +
                               x.Authors.LastName + " " +
                               x.Authors.FirstName
            })
            .ToList();

        ViewData["BookID"] = new SelectList(
            bookList, "ID", "BookFullName", Borrowing.BookID);

        ViewData["MemberID"] = new SelectList(
            _context.Member.ToList(),
            "ID", "FullName", Borrowing.MemberID);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return Page();
        }

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
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

    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}
