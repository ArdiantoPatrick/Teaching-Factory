using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class MasterTempatKerjaController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public MasterTempatKerjaController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index(TempatKerja tempatkerja)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_TempatKerja.Where(q => q.status == "Tersedia" || q.status == "Tidak Tersedia").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            return View();
        }

        [HttpPost]
        public IActionResult Create(TempatKerja tempatkerja)
        {
            var keterangan = tempatkerja.keterangan_tempat;
            var jenis = tempatkerja.jenis_tempat;
            var status = "Tersedia";
            var createddate = DateTime.Now;

            //if (ModelState.IsValid)
            //{
            var tempatkerja1 = new TempatKerja
            {
                keterangan_tempat = keterangan,
                jenis_tempat = jenis,
                status = status,
                created_by = HttpContext.Session.GetInt32("idprodi"),
                created_date = createddate,
            };

            _applicationDbContext.tbl_TempatKerja.Add(tempatkerja1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Data Berhasil Ditambahkan !!";
            return RedirectToAction("Index");
            //}
            //else
            //{
            //    TempData["Warning"] = "Data Tidak Berhasil Ditambahkan !!";
            //    return RedirectToAction("Index");
            //}
        }

        public IActionResult Update(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            TempatKerja tempatkerja = _applicationDbContext.tbl_TempatKerja.Find(id);
            return View(tempatkerja);
        }

        [HttpPost]
        public IActionResult Update(TempatKerja tempatkerja)
        {
            var idtempat = tempatkerja.id_tempatkerja;
            var keterangan = tempatkerja.keterangan_tempat;
            var jenis = tempatkerja.jenis_tempat;
            var status = tempatkerja.status;
            var createdby = tempatkerja.created_by;
            var createdate = tempatkerja.created_date;
            var modifieddate = DateTime.Now;

            var tempatkerja1 = new TempatKerja
            {
                id_tempatkerja = idtempat,
                keterangan_tempat = keterangan,
                jenis_tempat = jenis,
                status = status,
                created_by = createdby,
                created_date = createdate,
                modified_by = HttpContext.Session.GetInt32("idprodi"),
                modified_date = modifieddate,
            };

            //if (ModelState.IsValid)
            //{
            _applicationDbContext.tbl_TempatKerja.Update(tempatkerja1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Data Berhasil Diubah !!";
            return RedirectToAction("Index");
            //else
            //{
            //    TempData["Notifikasi"] = "Data Tidak Berhasil Diubah !";
            //    return RedirectToAction("Index");
            //}

        }

        //public IActionResult Delete(int id, TempatKerja tempatkerja)
        //{

        //    tempatkerja.status = "Deleted";
        //    var tempatkerjaawal = (from m in _applicationDbContext.TempatKerjas where (m.id_tempatkerja.Equals(id.ToString()))
        //                           select m);
        //    tempatkerjaawal. = tempatkerja.status;

        //    _applicationDbContext(tempatkerjaawal).CurrentValues.SetValues(tempatkerja).First(); ;
        //    _applicationDbContext.SaveChanges();  
        //    return RedirectToAction("Index");
        //}

        public IActionResult Delete(int id)
        {
            TempatKerja tempatkerja = _applicationDbContext.tbl_TempatKerja.Find(id);

            if (tempatkerja != null)
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
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteTempatKerja", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_tempatkerja", id);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);
                con.Open();
                cmd.ExecuteNonQuery();
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
