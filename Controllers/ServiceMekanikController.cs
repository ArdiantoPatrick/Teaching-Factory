using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class ServiceMekanikController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public ServiceMekanikController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult ListService()
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking).Include(q => q.TempatKerja).Where(q => q.id_grup == null && q.status == "Sukses").ToList();
            return View(data);
        }

        public IActionResult AntrianService()
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            var id = HttpContext.Session.GetInt32("idmekanik");
            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking).Include(q => q.TempatKerja).Where(q => q.id_grup == id && q.status == "Sukses").ToList();
            return View(data);
        }

        public IActionResult RiwayatService()
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            var id = HttpContext.Session.GetInt32("idmekanik");
            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking).Include(q => q.TempatKerja).Where(q => q.id_grup == id && q.status == "Pending" || q.id_grup == id && q.status == "Selesai" || q.id_grup == id && q.status == "Final").ToList();
            return View(data);
        }

        public IActionResult DetailServiceBelum(int id)
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            var data = _applicationDbContext.tbl_PKB.Where(q => q.id_pkb == id).FirstOrDefault();
            ViewBag.data = data;
            return View();
        }

        public IActionResult DetailServiceSudah(int id)
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            var data = _applicationDbContext.tbl_PKB.Where(q => q.id_pkb == id).FirstOrDefault();
            ViewBag.data = data;
            return View();
        }

        public IActionResult Ambil(int id)
        {
            PKB pkb = _applicationDbContext.tbl_PKB.Find(id);

            if (pkb != null)
            {
                TempData["Ambil"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Data Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("ListService");
        }

        public IActionResult KonfirmasiAmbil(int id)
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_AmbilPKB", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pkb", id);
                cmd.Parameters.AddWithValue("@id_grup", HttpContext.Session.GetInt32("idmekanik"));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("ListService");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("ListService");
            }
        }

        public IActionResult Selesai(int id)
        {
            PKB pkb = _applicationDbContext.tbl_PKB.Find(id);

            if (pkb != null)
            {
                TempData["Selesai"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Data Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("AntrianService");
        }

        public IActionResult KonfirmasiSelesai(int id)
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_SelesaiPKB", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pkb", id);
                cmd.Parameters.AddWithValue("@id_grup", HttpContext.Session.GetInt32("idmekanik"));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("AntrianService");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("AntrianService");
            }
        }
    }
}
