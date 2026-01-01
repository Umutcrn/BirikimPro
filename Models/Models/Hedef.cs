using System.ComponentModel.DataAnnotations;

namespace BirikimPro.Models
{
    public class Hedef
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = "";

        [Required]
        public string UrunAdi { get; set; } = "";

        [Required]
        public decimal Fiyat { get; set; }

        public decimal Biriken { get; set; }
    }
}
