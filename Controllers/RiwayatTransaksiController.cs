using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class RiwayatTransaksiController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public RiwayatTransaksiController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult RiwayatTransaksi()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking).Include(q => q.Grup).Include(q => q.Booking.Customer).Where(q => q.status == "Final").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailBooking(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");

            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking.Customer).Include(q => q.Booking).Where(q => q.id_pkb == id).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailCustomer(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");

            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking.Customer).Include(q => q.Booking).Where(q => q.id_pkb == id).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailRC(int id, int idbo)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.id_booking == idbo).FirstOrDefault();
            var datarc = _applicationDbContext.tbl_RoundChecking.Where(q => q.id_booking == idbo).FirstOrDefault();
            ViewBag.data = data;
            ViewBag.datarc = datarc;
            var data2 = _applicationDbContext.tbl_PKB.Include(q => q.Booking.Customer).Include(q => q.Booking).Where(q => q.id_pkb == id).ToList();
            return View();
        }
    }
}
