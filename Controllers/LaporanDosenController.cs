using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace ProjekTeFa.Controllers
{
    public class LaporanDosenController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public LaporanDosenController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult DataCustomer()
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Customer.Where(q => q.status == "Aktif").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult TransaksiBerlangsung()
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Where(q => q.status == "Menunggu Konfirmasi" || q.status == "Terkonfirmasi" || q.status == "RoundChecked" || q.status == "Dikerjakan" || q.status == "Selesai Dikerjakan").ToList();
            return View(data);
        }

        public IActionResult RiwayatTransaksi()
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking).Include(q => q.Grup).Include(q => q.Booking.Customer).Where(q => q.status == "Final").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailBooking(int id)
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");

            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking.Customer).Include(q => q.Booking).Where(q => q.id_pkb == id).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailCustomer(int id)
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");

            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking.Customer).Include(q => q.Booking).Where(q => q.id_pkb == id).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailRC(int id, int idbo)
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.id_booking == idbo).FirstOrDefault();
            var datarc = _applicationDbContext.tbl_RoundChecking.Where(q => q.id_booking == idbo).FirstOrDefault();
            ViewBag.data = data;
            ViewBag.datarc = datarc;
            var data2 = _applicationDbContext.tbl_PKB.Include(q => q.Booking.Customer).Include(q => q.Booking).Where(q => q.id_pkb == id).ToList();
            return View();
        }
    }
}
