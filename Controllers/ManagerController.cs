using Microsoft.AspNetCore.Mvc;

namespace property_rental_management.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
