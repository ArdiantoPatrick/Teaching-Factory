using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Linq;

namespace ProjekTeFa.Controllers
{
    public class BookingCustomerController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public BookingCustomerController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.idcust = HttpContext.Session.GetInt32("idcustomer");    
            ViewBag.namacust = HttpContext.Session.GetString("namacustomer");
            var idcust = HttpContext.Session.GetInt32("idcustomer");
            var data = _applicationDbContext.tbl_Booking.Where(q => q.id_customer == idcust && q.status == "Menunggu Konfirmasi" || q.id_customer == idcust && q.status == "Terkonfirmasi" || q.id_customer == idcust && q.status == "RoundChecked" || q.id_customer == idcust && q.status == "Dikerjakan" || q.id_customer == idcust && q.status == "Selesai Dikerjakan").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Riwayat()
        {
            ViewBag.idcust = HttpContext.Session.GetInt32("idcustomer");
            ViewBag.namacust = HttpContext.Session.GetString("namacustomer");
            var idcust = HttpContext.Session.GetInt32("idcustomer");
            var data = _applicationDbContext.tbl_Booking.Where(q => q.id_customer == idcust && q.status == "Selesai" || q.id_customer == idcust && q.status == "Tidak Disetujui").ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.idcust = HttpContext.Session.GetInt32("idcustomer");
            ViewBag.namacust = HttpContext.Session.GetString("namacustomer");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            var tanggal = booking.rencana_service_tanggal;
            var waktu = booking.rencana_service_waktu;
            var jenis = booking.jenis_kendaraan;
            var nomorpol = booking.nomor_polisi; 
            var merk = booking.merk_kendaraan;
            var tahun = booking.tahun_pembuatan;
            var warna = booking.warna;
            var odometer = booking.odometer;
            var keluhan = booking.keluhan;
            var status = "Menunggu Konfirmasi";
            var created = DateTime.Now;

            var booking1 = new Booking
            {
                id_customer = HttpContext.Session.GetInt32("idcustomer"),
                rencana_service_tanggal = tanggal,
                rencana_service_waktu = waktu,
                jenis_kendaraan = jenis,
                nomor_polisi = nomorpol,
                merk_kendaraan = merk,
                tahun_pembuatan = tahun,
                warna = warna,
                odometer = odometer,
                keluhan = keluhan,
                status = status,
                created_date = created
            };
            _applicationDbContext.tbl_Booking.Add(booking1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Booking Berhasil Dilakukan !!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DetailBooking(int id)
        {
            ViewBag.idcust = HttpContext.Session.GetInt32("idcustomer");
            ViewBag.namacust = HttpContext.Session.GetString("namacustomer");

            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.id_booking == id).ToList();
            //Booking booking = _applicationDbContext.tbl_Booking.Find(id);
            return View(data);
        }
    }
}
