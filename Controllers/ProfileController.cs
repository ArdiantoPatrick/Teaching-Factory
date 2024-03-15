using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;

namespace ProjekTeFa.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private static string conStr = "Server=ASTRASRV\\SQLEXPRESS;Database=TeachingFactoryTO;User Id=sa;Password=12345;TrustServerCertificate=True";
        SqlConnection con = new SqlConnection(conStr);
        public ProfileController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult ProfileSA()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            Prodi serviceadvisor = _applicationDbContext.tbl_Prodi.Find(ViewBag.idsa);
            return View(serviceadvisor);
        }

        [HttpPost]
        public IActionResult ProfileSA(Prodi serviceadvisor)
        {
            var id = serviceadvisor.id_prodi;
            var npk = serviceadvisor.npk;
            var password = serviceadvisor.password;
            var nama = serviceadvisor.nama;
            var email = serviceadvisor.email;
            var notelp = serviceadvisor.notelp;
            var alamat = serviceadvisor.alamat;
            var role = serviceadvisor.role;
            var status = serviceadvisor.status;
            var createddate = serviceadvisor.created_date;
            var modifieddate = DateTime.Now;

            var serviceadvisor1 = new Prodi
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

            _applicationDbContext.tbl_Prodi.Update(serviceadvisor1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Profile Berhasil Diubah !!";
            return RedirectToAction("ServiceAdvisor", "Dashboard");
        }

        public IActionResult ProfileCustomer()
        {
            ViewBag.idcust = HttpContext.Session.GetInt32("idcustomer");
            ViewBag.namacust = HttpContext.Session.GetString("namacustomer");
            Customer customer = _applicationDbContext.tbl_Customer.Find(ViewBag.idcust);
            return View(customer);
        }

        [HttpPost]
        public IActionResult ProfileCustomer(Customer customer)
        {
            var id = customer.id_customer;
            var username = customer.username;
            var password = customer.password;
            var nama = customer.nama_customer;
            var notelp = customer.notelp_customer;
            var alamat = customer.alamat;
            var createddate = customer.created_date;
            var status = customer.status;
            var modifieddate = DateTime.Now;

            var customer1 = new Customer
            {
                id_customer = id,
                username = username,    
                password = password,
                nama_customer = nama,
                notelp_customer = notelp,
                alamat = alamat,
                created_date = createddate,
                modified_date = modifieddate,
                status = status
            };

            _applicationDbContext.tbl_Customer.Update(customer1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Profile Berhasil Diubah !!";
            return RedirectToAction("Customer", "Dashboard");
        }

        public IActionResult ProfileMekanik()
        {
            ViewBag.idmekanik = HttpContext.Session.GetInt32("idmekanik");
            ViewBag.namamekanik = HttpContext.Session.GetString("namamekanik");
            //var idmekanik = HttpContext.Session.GetInt32("idmekanik");
            //var data = _applicationDbContext.tbl_Mekanik.Where(q => q.id_grup == idmekanik && q.status == "Aktif").ToList();
            //ViewBag.mekanik = data;
            Grup grup = _applicationDbContext.tbl_Grup.Find(ViewBag.idmekanik);
            return View(grup);
        }

        [HttpPost]
        public IActionResult ProfileMekanik(Grup grup)
        {
            var id = grup.id_grup;
            var nama = grup.nama_grup;
            var username = grup.username;
            var password = grup.password;
            var createdby = grup.created_by;
            var createddate = grup.created_date;
            var modifiedby = grup.modified_by;
            var status = grup.status;
            var modifieddate = DateTime.Now;

            var grup1 = new Grup
            {
                id_grup = id,
                nama_grup = nama,
                username = username,
                password = password,    
                created_by = createdby,
                created_date = createddate,
                modified_by = modifiedby,
                modified_date = modifieddate,
                status = status
            };

            _applicationDbContext.tbl_Grup.Update(grup1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Profile Berhasil Diubah !!";
            return RedirectToAction("Mekanik", "Dashboard");
        }

        public IActionResult ProfileDosenInstruktur()
        {
            ViewBag.iddosen = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namadosen = HttpContext.Session.GetString("namaprodi");
            Prodi doseninstruktur = _applicationDbContext.tbl_Prodi.Find(ViewBag.iddosen);
            return View(doseninstruktur);
        }

        [HttpPost]
        public IActionResult ProfileDosenInstruktur(Prodi doseninstruktur)
        {
            var id = doseninstruktur.id_prodi;
            var npk = doseninstruktur.npk;
            var password = doseninstruktur.password;
            var nama = doseninstruktur.nama;
            var email = doseninstruktur.email;
            var notelp = doseninstruktur.notelp;
            var alamat = doseninstruktur.alamat;
            var role = doseninstruktur.role;
            var status = doseninstruktur.status;
            var createddate = doseninstruktur.created_date;
            var modifieddate = DateTime.Now;

            var doseninstruktur1 = new Prodi
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

            _applicationDbContext.tbl_Prodi.Update(doseninstruktur1);
            _applicationDbContext.SaveChanges();
            TempData["Notifikasi"] = "Profile Berhasil Diubah !!";
            return RedirectToAction("DosenInstruktur", "Dashboard");
        }
    }
}
