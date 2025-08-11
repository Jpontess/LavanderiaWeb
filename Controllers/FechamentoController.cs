using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLavApp.Data;
using WebLavApp.Models;
using System.Globalization;
using System.Threading.Tasks;
using System.Data;

namespace WebLavApp.Controllers;

public class FechamentoController : Controller
{
    private readonly AppDbContext _context;

    public FechamentoController(AppDbContext context)
    {
        _context = context;
    }


 

   
        
    
}