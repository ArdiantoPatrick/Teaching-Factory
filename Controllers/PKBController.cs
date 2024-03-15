using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class PKBController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public PKBController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult PKB()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.status == "RoundChecked").ToList();
            return View(data);
        }

        public IActionResult PKBSelesai()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_PKB.Include(q => q.Booking).Include(q => q.Grup).Include(q => q.Booking.Customer).Where(q => q.status == "Pending" && q.id_grup != null || q.status == "Selesai" && q.id_grup != null).ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult PKBCreate(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.id_booking == id).FirstOrDefault();
            ViewBag.data = data;
            ViewBag.listgrup = GetListGrup();
            ViewBag.listtempat = GetListTempat();
            return View();
        }

        private List<SelectListItem> GetListGrup()
        {
            List<SelectListItem> listGrup = new List<SelectListItem>();
            var grup = _applicationDbContext.tbl_Grup.Select(Grup => Grup).Where(G => G.status == "Tersedia");

            listGrup = grup.Select(g => new SelectListItem()
            {
                Value = g.id_grup.ToString(),
                Text = g.nama_grup.ToString()
            }).ToList();

            var defItem = new SelectListItem()
            {  
                Text = "-- Pilih Grup --"
            };

            listGrup.Insert(0, defItem);

            return listGrup;
        }

        private List<SelectListItem> GetListTempat()
        {
            List<SelectListItem> listTempatKerja = new List<SelectListItem>();
            var tempatkerja = _applicationDbContext.tbl_TempatKerja.Select(TempatKerja => TempatKerja).Where(G => G.status == "Tersedia");

            listTempatKerja = tempatkerja.Select(g => new SelectListItem()
            {
                Value = g.id_tempatkerja.ToString(),
                Text = g.keterangan_tempat.ToString()
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "-- Pilih Tempat Kerja --"
            };

            listTempatKerja.Insert(0, defItem);

            return listTempatKerja;
        }


        [HttpPost]
        public IActionResult PKBCreate(PKB pkb)
        {
            var idbooking = pkb.id_booking;
            var idtempatkerja = pkb.id_tempatkerja;
            var idgrup = pkb.id_grup;
            var mekanisme = pkb.mekanisme_pengerjaan;
            var harga = pkb.harga;
            var status = "Sukses";
            var createddate = DateTime.Now;
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
            var modifieddate = DateTime.Now;

            var pkb1 = new PKB
            {
                id_booking = idbooking,
                id_tempatkerja = idtempatkerja,
                id_grup = idgrup,
                mekanisme_pengerjaan = mekanisme,
                harga = harga,
                created_by = HttpContext.Session.GetInt32("idprodi"),
                created_date = createddate,
                status = status,
            };
            _applicationDbContext.tbl_PKB.Add(pkb1);
            _applicationDbContext.SaveChanges();

            SqlCommand cmd = new SqlCommand("sp_UpdatePKB", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_tempatkerja", idtempatkerja);
            cmd.Parameters.AddWithValue("@id_booking", idbooking);
            cmd.Parameters.AddWithValue("@modified_by", modifiedby);
            cmd.Parameters.AddWithValue("@modified_date", modifieddate);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            TempData["Notifikasi"] = "PKB Berhasil Dibuat !!";
            return RedirectToAction("PKB");
        }

        public IActionResult ConfirmPKB(int id)
        {
            PKB pkb = _applicationDbContext.tbl_PKB.Find(id);

            if (pkb != null)
            {
                TempData["IsConfirm"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Booking Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("PKBSelesai");
        }

        public IActionResult ConfirmedPKB(int id/*, int idbo*/)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePKBSelesai", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pkb", id);
                //cmd.Parameters.AddWithValue("@id_booking", idbo);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("PKBSelesai");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("PKBSelesai");
            }
        }

        public IActionResult FollupConfirm(int id , int idbo)
        {
            PKB pkb = _applicationDbContext.tbl_PKB.Find(id);

            if (pkb != null)
            {
                TempData["FollupConfirm"] = "True";
                TempData["ID"] = id;
                TempData["IDBO"] = idbo;
            }
            else
            {
                TempData["Warning"] = "Transaksi Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("PKBSelesai");
        }

        public IActionResult FollupConfirmed(int id, int idbo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePKBFollup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_pkb", id);
                cmd.Parameters.AddWithValue("@id_booking", idbo);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("PKBSelesai");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("PKBSelesai");
            }
        }

    }
}
