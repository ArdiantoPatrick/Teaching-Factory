using System.ComponentModel.DataAnnotations;

namespace ProjekTeFa.Models
{
    public class Grup
    {
        [Key]
        public int id_grup { get; set; }    
        public string nama_grup { get; set; }
        public string username  {get; set; }
        public string password { get; set; }
        public int? created_by { get; set; }
        public int? modified_by { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string? status { get; set; }
    }
}
