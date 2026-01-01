using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BirikimPro.Data;

namespace BirikimPro.Pages
{
    [Authorize]
    public class HedefimModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HedefimModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string UrunAdi { get; set; }

        [BindProperty]
        public decimal Fiyat { get; set; }

        public List<Hedef> Hedefler { get; set; } = new();

        public List<Hedef> TamamlananHedefler { get; set; } = new();


        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
           Hedefler = await _context.Hedefler
                .Where(h => h.UserId == user.Id && h.Biriken < h.Fiyat)
                .OrderByDescending(h => h.Id)
                .ToListAsync();

            TamamlananHedefler = await _context.Hedefler
                .Where(h => h.UserId == user.Id && h.Biriken >= h.Fiyat)
                .OrderByDescending(h => h.Id)
                .ToListAsync();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var hedef = new Hedef
            {
                UserId = user.Id,
                UrunAdi = UrunAdi,
                Fiyat = Fiyat,
                Biriken = 0
            };

            _context.Hedefler.Add(hedef);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddAsync(int id, decimal ekleme)
        {
            var hedef = await _context.Hedefler.FindAsync(id);
            if (hedef != null)
            {
                hedef.Biriken += ekleme;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var hedef = await _context.Hedefler.FindAsync(id);
            if (hedef != null)
            {
                _context.Hedefler.Remove(hedef);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostHizliEkleAsync(int id, decimal tutar)
        {
            var hedef = await _context.Hedefler.FindAsync(id);
            if (hedef != null)
            {
                hedef.Biriken += tutar;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
