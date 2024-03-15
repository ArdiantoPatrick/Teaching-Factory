using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjekTeFa.Data;
using ProjekTeFa.Models;
using System.Data;

namespace ProjekTeFa.Controllers
{
    public class MasterKendaraanController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.idsa = HttpContext.Session.GetInt32("idprodi");
            ViewBag.namasa = HttpContext.Session.GetString("namaprodi");
            return View();
        }
    }
}
