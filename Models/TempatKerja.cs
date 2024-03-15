using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekTeFa.Models
{
    public class TempatKerja
    {   
        [Key]
        public int id_tempatkerja { get; set; }
        public string keterangan_tempat { get; set; }
        public string jenis_tempat { get; set; }
        public int? created_by { get; set; }
        public int? modified_by { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string status { get; set; }
    }
}
