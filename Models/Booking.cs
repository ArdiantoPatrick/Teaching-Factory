using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekTeFa.Models
{
    public class Booking
    {
        [Key]
        public int id_booking { get; set; }
        [DisplayFormat(DataFormatString = "{0:mm/MM/yyyy}")]
        public DateTime rencana_service_tanggal { get; set; }
        public TimeSpan rencana_service_waktu { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? aktual_service_tanggal { get; set; }
        public TimeSpan? aktual_service_waktu { get; set; }
        public string jenis_kendaraan { get; set; }
        public string nomor_polisi { get; set; }
        public string merk_kendaraan { get; set; }
        public int tahun_pembuatan { get; set; }
        public string warna { get; set; }
        public int odometer { get; set; }
        public string keluhan { get; set; }
        public string status { get; set; }
        public string? modified_by { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }

        [ForeignKey("Customer")]
        public int? id_customer { get; set; }
        public Customer? Customer { get; set; }
    }
}
