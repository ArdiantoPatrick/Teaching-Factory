using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class RoundCheckingController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public RoundCheckingController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult RoundChecking()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");

            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.status == "Terkonfirmasi").ToList();
            return View(data);
        }


        [HttpGet]
        public IActionResult RoundCheckingCreate(int id)
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Booking.Include(q => q.Customer).Where(q => q.id_booking == id).FirstOrDefault();
            ViewBag.data = data;
            return View();
        }

        [HttpPost]
        public IActionResult RoundCheckingCreate(RoundChecking rc)
        {
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
            var modifieddate = DateTime.Now;
            var idbooking = rc.id_booking;
            var status = "Selesai";

            var roundchecking = new RoundChecking
            {
                id_booking = idbooking,
                tanggal = rc.tanggal,
                waktu = rc.waktu,
                vin = rc.vin,
                nama_sa = rc.nama_sa,
                deskripsi_service = rc.deskripsi_service,
                perkiraan_biaya = rc.perkiraan_biaya,
                declines_sparetire = rc.declines_sparetire,
                declines_glovebox = rc.declines_glovebox,
                e_horn_operation = rc.e_horn_operation,
                e_lights = rc.e_lights,
                e_washer_operation = rc.e_washer_operation,
                e_winshield = rc.e_winshield,
                e_fuel_tank_cap = rc.e_fuel_tank_cap,
                i_combination_meter = rc.i_combination_meter,
                i_cabin_air_filter = rc.i_cabin_air_filter,
                i_parking_brake = rc.i_parking_brake,
                i_floor = rc.i_floor,
                uh_air_filter = rc.uh_air_filter,
                uh_battery_condition = rc.uh_battery_condition,
                uh_battery_state = rc.uh_battery_state,
                uh_cooling_system = rc.uh_cooling_system,
                uh_drive_belts = rc.uh_drive_belts,
                uh_hoses= rc.uh_hoses,
                uh_radiator_core = rc.uh_radiator_core,
                f_windshield_washer = rc.f_windshield_washer,
                f_coolant = rc.f_coolant,
                f_power_steering = rc.f_power_steering,
                f_brake_reservoir = rc.f_brake_reservoir,
                f_clutch_reservoir = rc.f_clutch_reservoir,
                f_transmission = rc.f_transmission,
                f_differential = rc.f_differential,
                f_transfercase = rc.f_transfercase,
                uv_propeller = rc.uv_propeller,
                uv_drive_shaft = rc.uv_drive_shaft,
                uv_axle_hub = rc.uv_axle_hub,
                uv_steering = rc.uv_steering,
                uv_suspension = rc.uv_suspension,
                uv_fluid_leaks = rc.uv_fluid_leaks,
                uv_exhaust_system = rc.uv_exhaust_system,
                uv_fuel_connections = rc.uv_fuel_connections,
                t_tread_depth = rc.t_tread_depth,
                t_tire_damage = rc.t_tire_damage,
                t_rims = rc.t_rims,
                t_rotated = rc.t_rotated,
                b_brake_lining = rc.b_brake_lining,
                b_hoses = rc.b_hoses,
                b_discs = rc.b_discs,
                komentar = rc.komentar,
                status = status,
            };
            _applicationDbContext.tbl_RoundChecking.Add(roundchecking);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Round Checking Berhasil Dilakukan !!";

            SqlCommand cmd = new SqlCommand("sp_UpdateRoundCheck", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_booking", idbooking);
            cmd.Parameters.AddWithValue("@modified_by", modifiedby);
            cmd.Parameters.AddWithValue("@modified_date", modifieddate);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("RoundChecking");
        }

        public IActionResult Lewati(int id)
        {
            Booking booking = _applicationDbContext.tbl_Booking.Find(id);

            if (booking != null)
            {
                TempData["IsPassed"] = "True";
                TempData["ID"] = id;
            }
            else
            {
                TempData["Warning"] = "Round Checking Tidak Dapat Dilewati !!";
            }
            return RedirectToAction("RoundChecking");
        }

        public IActionResult LewatiConfirmed(int id)
        {
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
            var modifieddate = DateTime.Now;
            try
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateRoundCheck", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_booking", id);
                cmd.Parameters.AddWithValue("@modified_by", modifiedby);
                cmd.Parameters.AddWithValue("@modified_date", modifieddate);

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

        public IActionResult DeleteRC(int id)
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
            var modifiedby = HttpContext.Session.GetInt32("idprodi");
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
    }
}
