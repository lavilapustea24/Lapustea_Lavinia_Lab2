using Lapustea_Lavinia_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lapustea_Lavinia_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Lapustea_Lavinia_Lab2Context _context;

    public EditModel(Lapustea_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }


        var book = await _context.Book
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);

        if (book is null)
        {
            return NotFound();
        }
        Book = book;

        PopulateAssignedCategoryData(_context, Book);

        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID",
"PublisherName");
        ViewData["AuthorsID"] = new SelectList(
    _context.Set<Authors>(), "ID", "LastName", Book.AuthorsID);
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.


    public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
    {
        var bookToUpdate = await _context.Book
            .Include(b => b.Authors)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.ID == Book.ID);

        if (bookToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            b => b.Title,
            b => b.AuthorsID,
            b => b.Price,
            b => b.PublishingDate,
            b => b.PublisherID))
        {
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        UpdateBookCategories(_context, selectedCategories, bookToUpdate);
        PopulateAssignedCategoryData(_context, bookToUpdate);

        ViewData["PublisherID"] = new SelectList(
            _context.Set<Publisher>(), "ID", "PublisherName",
            bookToUpdate.PublisherID);

        ViewData["AuthorsID"] = new SelectList(
            _context.Set<Authors>(), "ID", "LastName",
            bookToUpdate.AuthorsID);

        Book = bookToUpdate;
        return Page();
    }


    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
