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
    public class MasterGrupController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public MasterGrupController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        //[HttpGet]
        public IActionResult Index()
        {
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Grup.Where(q => q.status == "Tersedia" || q.status == "Tidak Tersedia").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Grup grup)
        {
            var data = _applicationDbContext.tbl_Grup.Where(q => q.username == grup.username && q.status == "Tersedia" || q.username == grup.username && q.status == "Tidak Tersedia").FirstOrDefault();
            if (data != null)
            {
                TempData["Warning"] = "Username Sudah Terpakai !!";
                return View(grup);
            }
            else
            {
                var nama = grup.nama_grup;
                var username = grup.username;
                var password = grup.password;
                var status = "Tersedia";
                var createddate = DateTime.Now;

                var grup1 = new Grup
                {
                    nama_grup = nama,
                    username = username,
                    password = password,
                    status = status,
                    created_by = HttpContext.Session.GetInt32("idprodi"),
                    created_date = createddate,
                };
                _applicationDbContext.tbl_Grup.Add(grup1);
                _applicationDbContext.SaveChanges();
                TempData["Notifikasi"] = "Data Berhasil Ditambahkan !!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Update(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            Grup grup = _applicationDbContext.tbl_Grup.Find(id);
            return View(grup);
        }

        [HttpPost]
        public IActionResult Update(Grup grup)
        {
            var idgrup = grup.id_grup;
            var nama = grup.nama_grup;
            var username = grup.username;
            var password = grup.password;
            var status = grup.status;
            var createdby = grup.created_by;
            var createdate = grup.created_date;
            var modifieddate = DateTime.Now;

            var grup1 = new Grup
            {
                id_grup = idgrup,
                nama_grup = nama,
                username = username,
                password = password,
                status = status,
                created_by = createdby,
                created_date = createdate,
                modified_by = HttpContext.Session.GetInt32("idprodi"),
                modified_date = modifieddate
            };
            _applicationDbContext.tbl_Grup.Update(grup1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Data Berhasil Diubah !!";
            return RedirectToAction("Index");

        }

        public IActionResult Delete(int id)
        {
            Grup grup = _applicationDbContext.tbl_Grup.Find(id);

            if (grup != null)
            {
                TempData["IsDelete"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Data Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var modifieddate = DateTime.Now;
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteGrup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_grup", id);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);

                SqlCommand cmd2 = new SqlCommand("sp_UpdateMekanik", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@id_grup", id);
                con.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("Index");
            }
        }
    }
}
