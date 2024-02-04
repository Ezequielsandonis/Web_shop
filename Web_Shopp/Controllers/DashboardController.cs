using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Shop.Data;
namespace Web_Shop.Controllers
{
    [Authorize(Policy = "RequiredAdminOrStaff")]
    public class DashboardController : BaseController 
    {
        
        public DashboardController(ApplicationDbContext context) : base(context)
        {
            
        }

        public  IActionResult Index() { return View(); }

    }
}
