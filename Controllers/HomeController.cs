using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ProjekTeFa.Models;
using ProjekTeFa.Data;

namespace ProjekTeFa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            //var data = _applicationDbContext.tbl_Mekanik.Where(q => q.status == "Aktif").ToList();
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        public IActionResult Faq()
        {
            return View();
        }
        public IActionResult LoginCustomer()
        {
            return View();
        }

        public IActionResult LoginMekanik()
        {
            return View();
        }

        public IActionResult LoginProdi()
        {
            return View();
        }

        public IActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]

        public IActionResult RegisterCustomer(Customer customer)
        {
            var usr = _applicationDbContext.tbl_Customer.Where(u => u.username == customer.username && u.status == "Aktif").FirstOrDefault();
            if (usr != null)
            {
                TempData["Warning"] = "Username Sudah Terpakai !!";
                return View(customer);
            }
            else
            {
                var nama = customer.nama_customer;
                var notelp = customer.notelp_customer;
                var alamat = customer.alamat;
                var username = customer.username;
                var password = customer.password;
                var createddate = DateTime.Now;
                var status = "Aktif";

                var customer1 = new Customer
                {
                    nama_customer = nama,
                    notelp_customer = notelp,
                    alamat = alamat,
                    username = username,
                    password = password,
                    created_date = createddate,
                    status = status,
                };

                _applicationDbContext.tbl_Customer.Add(customer1);
                _applicationDbContext.SaveChanges();
                TempData["Notifikasi"] = "Akun berhasil dibuat!!";
                return RedirectToAction("LoginCustomer");
            }
        }

        [HttpPost]
        
        public IActionResult LoginCustomer(Customer customer)
        {
            var usr = _applicationDbContext.tbl_Customer.Where(u => u.username == customer.username && u.password == customer.password && u.status == "Aktif").FirstOrDefault();
            if (usr != null)
            {
                HttpContext.Session.SetInt32("idcustomer", usr.id_customer);
                HttpContext.Session.SetString("namacustomer", usr.nama_customer);
                return RedirectToAction("Customer", "Dashboard");
            }
            else
            {
                TempData["Warning"] = "Username atau Password Tidak Valid !!";
                return View(customer);
            }
        }


        [HttpPost]

        public IActionResult LoginMekanik(Grup grup)
        {
            var usr = _applicationDbContext.tbl_Grup.Where(u => u.username == grup.username && u.password == grup.password && u.status == "Tersedia" || u.username == grup.username && u.password == grup.password && u.status == "Tidak Tersedia").FirstOrDefault();
            if (usr != null)
            {
                HttpContext.Session.SetInt32("idmekanik", usr.id_grup);
                HttpContext.Session.SetString("namamekanik", usr.nama_grup);
                return RedirectToAction("Mekanik", "Dashboard");
            }
            else
            {
                TempData["Warning"] = "Username atau Password Tidak Valid !!";
                return View(grup);
            }
        }


        [HttpPost]

        public IActionResult LoginProdi(Prodi prodi)
        {
            var usr = _applicationDbContext.tbl_Prodi.Where(u => u.npk == prodi.npk && u.password == prodi.password && u.status == "Aktif").FirstOrDefault();
            if (usr != null)
            {
                if (usr.role == "Service Advisor")
                {
                    HttpContext.Session.SetInt32("idprodi", usr.id_prodi);
                    HttpContext.Session.SetString("namaprodi", usr.nama);
                    return RedirectToAction("ServiceAdvisor", "Dashboard");
                }
                else if (usr.role == "DosenInstruktur")
                {
                    HttpContext.Session.SetInt32("idprodi", usr.id_prodi) ;
                    HttpContext.Session.SetString("namaprodi", usr.nama);
                    return RedirectToAction("DosenInstruktur", "Dashboard");
                }else if (usr.role == "Admin")
                {
                    HttpContext.Session.SetInt32("idprodi", usr.id_prodi);
                    HttpContext.Session.SetString("namaprodi", usr.nama);
                    return RedirectToAction("Admin", "Dashboard");
                }
                return View(prodi);
            }
            else
            {
                TempData["Warning"] = "NPK atau Password Tidak Valid !!";
                return View(prodi);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            //TempData["Notifikasi"] = "Berhasil Logout !!";
            return RedirectToAction("Index", "Home");
        }
    }
}