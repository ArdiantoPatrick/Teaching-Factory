using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjekTeFa.Data;

namespace ProjekTeFa.Controllers
{
    public class MasterCustomerController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public MasterCustomerController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.idsa = HttpContext.Session.GetString("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            var data = _applicationDbContext.tbl_Customer.Where(q => q.status == "Aktif").ToList();
            return View(data);
        }
    }
}
