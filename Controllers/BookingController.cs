using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public BookingController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Booking()  
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.status == "Menunggu Konfirmasi").ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Booking(int id, DateTime tanggal, DateTime waktu)
        {
            //var id = booking.BookingCreate.id_booking;
            //var tanggal = booking.BookingCreate.rencana_service_tanggal;
            //var waktu = booking.BookingCreate.rencana_service_waktu;
            var modifieddate = DateTime.Now;
            var modifiedby = HttpContext.Session.GetString("namaprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateBooking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_booking", id);
                cmd.Parameters.AddWithValue("@tanggal", tanggal);
                cmd.Parameters.AddWithValue("@waktu", waktu);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                TempData["Notifikasi"] = "Aktual Service Berhasil Ditetapkan !!";
                return RedirectToAction("Booking");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("Booking");
            }
        }

        public IActionResult DeleteBooking(int id)
        {
            Booking booking = _applicationDbContext.tbl_Booking.Find(id);

            if (booking != null)
            {
                TempData["IsDelete"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Booking Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("Booking");
        }

        public IActionResult DeleteRoundCheck(int id)
        {
            Booking booking = _applicationDbContext.tbl_Booking.Find(id);

            if (booking != null)
            {
                TempData["IsDelete"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Booking Tidak Bisa Dihapus !!";
            }
            return RedirectToAction("RoundChecking");
        }

        public IActionResult DeleteConfirmedRC(int id)
        {
            var modifieddate = DateTime.Now;
            var modifiedby = HttpContext.Session.GetString("namaprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteBooking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_booking", id);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("RoundChecking");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("RoundChecking");
            }
        }

        public IActionResult DeleteConfirmed(int id)
        {
            var modifieddate = DateTime.Now;
            var modifiedby = HttpContext.Session.GetString("namaprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteBooking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_booking", id);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Booking");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("Booking");
            }
        }

        public IActionResult Confirm(int id)
        {
            Booking booking = _applicationDbContext.tbl_Booking.Find(id);

            if (booking != null)
            {
                TempData["IsConfirm"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Booking Tidak Bisa Dikonfirmasi !!";
            }
            return RedirectToAction("Booking");
        }

        public IActionResult ConfirmBooking(int id)
        {
            var modifieddate = DateTime.Now;
            var modifiedby = HttpContext.Session.GetString("namaprodi");
            try
            {
                SqlCommand cmd = new SqlCommand("sp_SetujuBooking", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_booking", id);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Booking");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return RedirectToAction("Booking");
            }
        }
            
        [HttpGet]
        public IActionResult DetailBooking(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");

            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.id_booking == id).ToList();
            //Booking booking = _applicationDbContext.tbl_Booking.Find(id);
            return View(data);
        }
    }
}
