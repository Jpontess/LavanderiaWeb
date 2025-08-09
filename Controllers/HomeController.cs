using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLavApp.Data;
using WebLavApp.Models;

namespace WebLavApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public  IActionResult Index()
        {
        
            return View();
        }
    }
}
