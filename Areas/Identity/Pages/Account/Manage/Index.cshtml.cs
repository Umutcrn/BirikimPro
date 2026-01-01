using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BirikimPro.Data;

namespace BirikimPro.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string Email { get; set; }
        public int HedefSayisi { get; set; }
        public decimal ToplamBirikim { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            Email = user?.Email ?? "";

            var hedefler = await _context.Hedefler
                .Where(h => h.UserId == user.Id)
                .ToListAsync();

            HedefSayisi = hedefler.Count;
            ToplamBirikim = hedefler.Sum(h => h.Biriken);
        }
    }
}
