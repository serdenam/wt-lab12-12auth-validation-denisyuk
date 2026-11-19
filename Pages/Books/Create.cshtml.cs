using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using wt_lab12_12auth_validation_denisyuk.Data;
using wt_lab12_12auth_validation_denisyuk.Models;

namespace wt_lab12_12auth_validation_denisyuk.Pages.Books;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    // ВАЖНО: Свойство теперь называется NewBook (как в твоем HTML)
    [BindProperty]
    public Book NewBook { get; set; } = new();

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Books.Add(NewBook);           // Добавляем NewBook
        await _context.SaveChangesAsync();     // Сохраняем в базу

        return RedirectToPage("./Index");
    }
}