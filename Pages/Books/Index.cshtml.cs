using Microsoft.AspNetCore.Mvc; // <-- ВОТ ЭТА СТРОКА
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_denisyuk.Models;
using wt_lab8_8razor_postgresql_denisyuk.Data;

namespace wt_lab8_8razor_postgresql_denisyuk.Pages.Books;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Book> Books { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public async Task OnGetAsync()
    {
        var query = _context.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            query = query.Where(b => b.Title.Contains(Search));
        }

        Books = await query.OrderBy(b => b.Id).ToListAsync();
    }
}