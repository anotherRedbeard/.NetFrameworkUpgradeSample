using Microsoft.AspNetCore.Mvc;

namespace eShopLegacyMVCCore.Controllers
{
    public class ASPNetController : Controller
    {
        public IActionResult Index()
        {
            return Ok("Hello World from upgrade!!");
        }
    }
}
