#region Using
using Microsoft.AspNetCore.Mvc;
using MyVet.Web.Models;
using System.Diagnostics;
#endregion


namespace MyVet.Web.Controllers
{
    #region Permisos
    #endregion
    public class HomeController : Controller
    {
        #region Variables
        #endregion

        #region Constructor	
        #endregion

        #region Metodos	
       
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

    }
}
