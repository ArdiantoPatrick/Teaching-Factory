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
    public class MasterMekanikController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public MasterMekanikController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index(Mekanik mekanik)
        {
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Mekanik.Include(q => q.Grup).Where(q => q.status == "Aktif").ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            ViewBag.listgrup = GetListGrup();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Mekanik mekanik)
        {
            var nama = mekanik.nama_mekanik;
            var nim = mekanik.nim_mekanik;
            var alamat = mekanik.alamat_mekanik;
            var email = mekanik.email_mekanik;
            var notelp = mekanik.notelp_mekanik;
            var idgrup = mekanik.id_grup;
            var status = "Aktif";
            var createddate = DateTime.Now;

            var mekanik1 = new Mekanik
            {
                id_grup = idgrup,
                nama_mekanik = nama,
                nim_mekanik = nim,
                email_mekanik = email,
                notelp_mekanik = notelp,
                alamat_mekanik = alamat,
                status = status,
                created_by = HttpContext.Session.GetInt32("idprodi"),
                created_date = createddate,
            };
            //if (ModelState.IsValid)
            //{
            _applicationDbContext.tbl_Mekanik.Add(mekanik1);
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


        private List<SelectListItem> GetListGrup()
        {
            List<SelectListItem> listGrup = new List<SelectListItem>();
            var grup = _applicationDbContext.tbl_Grup.Select(Grup => Grup).Where(G => G.status == "Tersedia" || G.status == "Tidak Tersedia");

            listGrup = grup.Select(g => new SelectListItem()
            {
                Value = g.id_grup.ToString(),
                Text = g.nama_grup.ToString()
            }).ToList();

            var defItem = new SelectListItem()
            {   
                Value = "",    
                Text = "-- Pilih Grup --"
            };

            listGrup.Insert(0, defItem);

            return listGrup;
        }

        //private List<SelectListItem> GetListGrup2()
        //{
        //    List<SelectListItem> listGrup = new List<SelectListItem>();
        //    var grup = _applicationDbContext.tbl_Grup.Select(Grup => Grup);

        //    listGrup = grup.Select(g => new SelectListItem()
        //    {
        //        Value = g.id_grup.ToString(),
        //        Text = g.nama_grup.ToString()
        //    }).ToList();

        //    return listGrup;
        //}
        public IActionResult Update(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            Mekanik mekanik = _applicationDbContext.tbl_Mekanik.Find(id);
            ViewBag.listgrup = GetListGrup();
            return View(mekanik);
        }

        [HttpPost]
        public IActionResult Update(Mekanik mekanik)
        {
            var idmekanik = mekanik.id_mekanik;
            var idgrup = mekanik.id_grup;
            var nim = mekanik.nim_mekanik;
            var nama = mekanik.nama_mekanik;
            var email = mekanik.email_mekanik;
            var notelp = mekanik.notelp_mekanik;
            var alamat = mekanik.alamat_mekanik;
            var status = mekanik.status;
            var createdby = mekanik.created_by;
            var createdate = mekanik.created_date;
            var modifieddate = DateTime.Now;

            var mekanik1 = new Mekanik
            {
                id_mekanik = idmekanik,
                id_grup = idgrup,
                nim_mekanik = nim,
                nama_mekanik = nama,
                email_mekanik = email,
                notelp_mekanik = notelp,
                alamat_mekanik = alamat,
                status = status,
                created_by = createdby,
                created_date = createdate,
                modified_by = HttpContext.Session.GetInt32("idprodi"),
                modified_date = modifieddate
            };

            //if (ModelState.IsValid)
            //{
            _applicationDbContext.tbl_Mekanik.Update(mekanik1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Data Berhasil Diubah !!";
            return RedirectToAction("Index");
            //}
            //else
            //{
            //    TempData["Notifikasi"] = "Data Tidak Berhasil Diubah !";
            //    return RedirectToAction("Index");
            //}
            //}
        }

        public IActionResult Delete(int id)
        {
            Mekanik mekanik = _applicationDbContext.tbl_Mekanik.Find(id);

            if (mekanik != null)
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
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteMekanik", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_mekanik", id);
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
