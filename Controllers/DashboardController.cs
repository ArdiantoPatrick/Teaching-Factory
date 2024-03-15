using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Linq;

namespace ProjekTeFa.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public DashboardController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult ServiceAdvisor()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            ViewBag.tempat = _applicationDbContext.tbl_TempatKerja.Where(q => q.status == "Tersedia" || q.status == "Tidak Tersedia").Count();
            ViewBag.mekanik = _applicationDbContext.tbl_Mekanik.Where(q => q.status == "Aktif").Count();
            ViewBag.customer = _applicationDbContext.tbl_Customer.Where(q => q.status == "Aktif").Count();
            ViewBag.grup = _applicationDbContext.tbl_Grup.Where(q => q.status == "Tersedia" || q.status == "Tidak Tersedia").Count();

            ViewBag.booking = _applicationDbContext.tbl_Booking.Where(q => q.status == "Menunggu Konfirmasi").Count();
            ViewBag.rc = _applicationDbContext.tbl_Booking.Where(q => q.status == "Terkonfirmasi").Count();
            ViewBag.pkb = _applicationDbContext.tbl_Booking.Where(q => q.status == "RoundChecked").Count();
            ViewBag.konfirmasi = _applicationDbContext.tbl_PKB.Where(q => q.status == "Pending" && q.id_grup != null || q.status == "Selesai" && q.id_grup != null).Count();
            return View();
        }

        public IActionResult Customer()
        {
            ViewBag.idcust = HttpContext.Session.GetInt32("idcustomer");
            ViewBag.namacust = HttpContext.Session.GetString("namacustomer");
            var idcust = HttpContext.Session.GetInt32("idcustomer");
            ViewBag.ongoing = _applicationDbContext.tbl_Booking.Where(q => q.id_customer == idcust && q.status == "Menunggu Konfirmasi" || q.id_customer == idcust && q.status == "Terkonfirmasi" || q.id_customer == idcust && q.status == "RoundChecked" || q.id_customer == idcust && q.status == "Dikerjakan" || q.id_customer == idcust && q.status == "Selesai Dikerjakan").Count();
            ViewBag.riwayat = _applicationDbContext.tbl_Booking.Where(q => q.id_customer == idcust && q.status == "Selesai" || q.id_customer == idcust && q.status == "Tidak Disetujui").Count();
            return View();
        }

        public IActionResult Mekanik()
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            var id = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.list = _applicationDbContext.tbl_PKB.Where(q => q.id_grup == null && q.status == "Sukses").Count();
            ViewBag.antrian = _applicationDbContext.tbl_PKB.Where(q => q.id_grup == id && q.status == "Sukses").Count();
            ViewBag.riwayat = _applicationDbContext.tbl_PKB.Where(q => q.id_grup == id && q.status == "Pending" || q.id_grup == id && q.status == "Selesai" || q.id_grup == id && q.status == "Final").Count();
            return View();
        }

        public IActionResult Admin()
        {
            ViewBag.idadm = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namaadm = HttpContext.Session.GetString("namaprodi");
            ViewBag.dosen = _applicationDbContext.tbl_Prodi.Where(q => q.status == "Aktif" && q.role == "DosenInstruktur").Count();
            ViewBag.sa = _applicationDbContext.tbl_Prodi.Where(q => q.status == "Aktif" && q.role == "Service Advisor").Count();
            return View();
        }

        public IActionResult DosenInstruktur()
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");
            ViewBag.customer = _applicationDbContext.tbl_Customer.Where(q => q.status == "Aktif").Count();
            ViewBag.transaksi = _applicationDbContext.tbl_Booking.Where(q => q.status == "Menunggu Konfirmasi" || q.status == "Terkonfirmasi" || q.status == "RoundChecked" || q.status == "Dikerjakan" || q.status == "Selesai Dikerjakan").Count();
            ViewBag.riwayat = _applicationDbContext.tbl_PKB.Where(q => q.status == "Final").Count();
            return View();
        }
    }
}
