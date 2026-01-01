using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BirikimPro.Data;

namespace BirikimPro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /* ===== ANA GÖSTERİLENLER ===== */
        public Hedef? AktifHedef { get; set; }

        public decimal ToplamBirikim { get; set; }

        public int AktifHedefSayisi { get; set; }

        public int TamamlananHedefSayisi { get; set; }

        public int OrtalamaIlerleme { get; set; }

        public string Motivasyon { get; set; } = "";

        public decimal BuAyBirikim { get; set; }


        private readonly string[] Motivasyonlar =
        {
            "Bugün vazgeçmezsen, yarın teşekkür edersin.",
            "Az ama sürekli biriktiren kazanır.",
            "Hedefine sandığından daha yakınsın.",
            "Birikim bir alışkanlıktır.",
            "Paranı sen yönetmezsen, o seni yönetir."
        };

        public async Task OnGetAsync()
        {
            if (!User.Identity!.IsAuthenticated)
                return;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            var hedefler = await _context.Hedefler
                .Where(h => h.UserId == user.Id)
                .ToListAsync();

            /* ===== İSTATİSTİKLER ===== */
            AktifHedefSayisi = hedefler.Count(h => h.Biriken < h.Fiyat);
            TamamlananHedefSayisi = hedefler.Count(h => h.Biriken >= h.Fiyat);
            ToplamBirikim = hedefler.Sum(h => h.Biriken);

            if (hedefler.Count > 0)
            {
                OrtalamaIlerleme = (int)Math.Round(
                    hedefler.Average(h =>
                        h.Fiyat > 0
                            ? Math.Min(100, (h.Biriken * 100) / h.Fiyat)
                            : 0
                    )
                );
            }
            else
            {
                OrtalamaIlerleme = 0;
            }

            /* ===== AKTİF HEDEF (EN SON EKLENEN) ===== */
            AktifHedef = hedefler
                .OrderByDescending(h => h.Id)
                .FirstOrDefault();

            /* ===== MOTİVASYON ===== */
            var rnd = new Random();
            Motivasyon = Motivasyonlar[rnd.Next(Motivasyonlar.Length)];

            var ayBaslangici = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            BuAyBirikim = hedefler.Sum(h => h.Biriken);
        }

        /* ===== HIZLI BİRİKİM ===== */
        public async Task<IActionResult> OnPostHizliEkleAsync(decimal tutar)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToPage();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToPage();

            var hedef = await _context.Hedefler
                .Where(h => h.UserId == user.Id && h.Biriken < h.Fiyat)
                .OrderByDescending(h => h.Id)
                .FirstOrDefaultAsync();

            if (hedef != null)
            {
                hedef.Biriken += tutar;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
