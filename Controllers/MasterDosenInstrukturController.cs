using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;
using System.Reflection;

namespace ProjekTeFa.Controllers
{
    public class MasterDosenInstrukturController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public MasterDosenInstrukturController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.idadm = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namaadm = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Prodi.Where(q => q.status == "Aktif" && q.role == "DosenInstruktur").ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.idadm = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namaadm = HttpContext.Session.GetString("namaprodi");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Prodi prodi)
        {
            var data = _applicationDbContext.tbl_Prodi.Where(q => q.status == "Aktif" && q.npk == prodi.npk).FirstOrDefault();
            if (data != null)
            {
                TempData["Warning"] = "NIM / NPK Sudah Terpakai !!";
                return View(prodi);
            }
            else
            {
                var npk = prodi.npk;
                var password = prodi.password;
                var nama = prodi.nama;
                var email = prodi.email;
                var notelp = prodi.notelp;
                var alamat = prodi.alamat;
                var role = "DosenInstruktur";
                var status = "Aktif";
                var createddate = DateTime.Now;

                var prodi1 = new Prodi
                {
                    npk = npk,
                    password = password,
                    nama = nama,
                    email = email,
                    notelp = notelp,
                    alamat = alamat,
                    role = role,
                    status = status,
                    created_date = createddate,
                };
                //if (ModelState.IsValid)
                //{
                _applicationDbContext.tbl_Prodi.Add(prodi1);
                _applicationDbContext.SaveChanges();
                TempData["Notifikasi"] = "Data Berhasil Ditambahkan !!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Update(Prodi prodi)
        {
            var id = prodi.id_prodi;
            var npk = prodi.npk;
            var password = prodi.password;
            var nama = prodi.nama;
            var email = prodi.email;
            var notelp = prodi.notelp;
            var alamat = prodi.alamat;
            var role = prodi.role;
            var status = prodi.status;
            var createddate = prodi.created_date;
            var modifieddate = DateTime.Now;

            var prodi1 = new Prodi
            {
                id_prodi = id,
                npk = npk,
                password = password,
                nama = nama,
                email = email,
                notelp = notelp,
                alamat = alamat,
                role = role,
                status = status,
                created_date = createddate, 
                modified_date = modifieddate
            };

            //if (ModelState.IsValid)
            //{
            _applicationDbContext.tbl_Prodi.Update(prodi1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Data Berhasil Diubah !!";
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            ViewBag.idadm = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namaadm = HttpContext.Session.GetString("namaprodi");
            Prodi doseninstruktur = _applicationDbContext.tbl_Prodi.Find(id);
            return View(doseninstruktur);
        }

        public IActionResult Delete(int id)
        {
            Prodi prodi = _applicationDbContext.tbl_Prodi.Find(id);

            if (prodi != null)
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
            ViewBag.idadm = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namaadm = HttpContext.Session.GetString("namaprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteProdi", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_prodi", id);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
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
