using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace ProjekTeFa.Models
{
    public class Prodi
    {
        [Key]
        public int id_prodi { get; set; }
        public string npk { get; set; }
        public string password { get; set; }
        public string nama { get; set; }
        public string email { get; set; }
        public string notelp { get; set; }
        public string alamat { get; set; }
        public string role { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string status { get; set; }

    }
}
