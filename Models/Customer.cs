using System.ComponentModel.DataAnnotations;

namespace ProjekTeFa.Models
{
    public class Customer
    {
        [Key]
        public int id_customer { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nama_customer { get; set; }
        public string notelp_customer { get; set; }
        public string alamat { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string status { get; set; }
    }
}
