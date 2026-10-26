using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lapustea_Lavinia_Lab2.Models;

namespace Lapustea_Lavinia_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Lapustea_Lavinia_Lab2Context _context;

    public IndexModel(Lapustea_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;

    public int? SelectedCategoryID { get; set; }

    public async Task OnGetAsync(int? id)
    {
        Category = await _context.Category
            .Include(c => c.BookCategories)
                .ThenInclude(bc => bc.Book)
                    .ThenInclude(b => b.Authors)
            .ToListAsync();

        SelectedCategoryID = id;
    }
}