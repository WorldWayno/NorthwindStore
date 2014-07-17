using System.Web.Mvc;

namespace Northwind.Api.Controllers
{
    /// <summary>
    ///  Swagger
    /// </summary>
    public class HelpController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}