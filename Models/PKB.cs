using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekTeFa.Models
{
    public class PKB
    {
        [Key]
        public int id_pkb { get; set; }
        public string mekanisme_pengerjaan { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public string harga { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string? status { get; set; }

        [ForeignKey("Booking")]
        public int? id_booking { get; set; }
        public Booking? Booking { get; set; }

        [ForeignKey("TempatKerja")]
        public int? id_tempatkerja { get; set; }
        public TempatKerja? TempatKerja { get; set; }

        [ForeignKey("Grup")]
        public int? id_grup { get; set; }
        public Grup? Grup { get; set; }
    }
}
