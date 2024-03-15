using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekTeFa.Models
{
    public class Mekanik
    {
        [Key]
        public int id_mekanik { get; set; }
        public string nim_mekanik { get; set; }
        public string nama_mekanik { get; set; }
        public string email_mekanik { get; set; }
        public string notelp_mekanik { get; set; }
        public string alamat_mekanik { get; set; }
        public int? created_by { get; set; }
        public int? modified_by { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string? status { get; set; }

        [ForeignKey("Grup")]
        public int? id_grup { get; set; }
        public Grup? Grup { get; set; }
    }
}
