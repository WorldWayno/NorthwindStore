using System.Web.Mvc;

namespace Northwind.Api.Controllers
{
    /// <summary>
    ///  Swagger
    /// </summary>
    public class HelpController : Controller
    {
        private string _apiDocUri;
        public HelpController()
        {
         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var host = Request.ServerVariables["HTTP_HOST"];
            var pathInfo = Request.ApplicationPath;
            ViewBag.ApiDocUri = string.Format("http://{0}{1}/swagger/api-docs", host, pathInfo);
            return View();
        }
    }
}