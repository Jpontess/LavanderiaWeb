using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLavApp.Data;
using WebLavApp.Models;

namespace WebLavApp.Controllers;

public class ModalidadesController : Controller
{
    public readonly AppDbContext _context;

    public ModalidadesController(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index(string? filtroNome, string? filtroModalidade, DateTime? filtroDataInicio, DateTime? filtroDataFim)
    {
        var ModalidadesQuery = _context.Modalidades.AsQueryable();

        // Aplicar filtro
        if (!string.IsNullOrEmpty(filtroNome))
        {
            ModalidadesQuery = ModalidadesQuery.Where(s => s.Nome!.Contains(filtroNome));
        }
        if (!string.IsNullOrEmpty(filtroModalidade))
        {
            ModalidadesQuery = ModalidadesQuery.Where(s => s.Modalidade!.Contains(filtroModalidade));
        }
        if (filtroDataInicio.HasValue)
        {
            ModalidadesQuery = ModalidadesQuery.Where(s => s.Data >= filtroDataInicio.Value);
        }
        if (filtroDataFim.HasValue)
        {
            ModalidadesQuery = ModalidadesQuery.Where(s => s.Data <= filtroDataFim.Value);
        }

        //obter Modalidade distintos para dropdown
        ViewBag.Modalidades = await _context.Modalidades
        .Select(s => s.Modalidade)
        .Distinct()
        .OrderBy(d => d)
        .ToListAsync();

        //manter os filtros na viewData para manter o estado
        ViewData["FiltroNome"] = filtroNome;
        ViewData["FiltroModalidade"] = filtroModalidade;
        ViewData["FiltroDataInicio"] = filtroDataInicio?.ToString("yyyy-MM-dd");
        ViewData["FiltroDataFim"] = filtroDataFim?.ToString("yyyy-MM-dd");

        return View(await ModalidadesQuery.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Modalidade,Material,Cor,Quantidade")] Modalidades modalidades)
    {
        if (ModelState.IsValid)
        {
            _context.Add(modalidades);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(modalidades);
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var modalidade = await _context.Modalidades.FindAsync(id);
        if (modalidade == null)
        {
            return NotFound();
        }

        ViewBag.Modalidade = new List<string>
        {
            "Futsal",
            "Vôlei",
            "Handebol",
            "Basquete",
            "Futebol Vida Saudável",
            "Futsal Vida Saudável",
            "Atletismo",
            "Tênis",
            "Skate",
            "Artes Marciais",
            "Ginástica"
        };

        return View(modalidade);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int? id, [Bind("Id,Nome,Modalidade,Material,Cor,Quantidade")] Modalidades modalidades)
    {
        if (id != modalidades.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(modalidades);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!modalidadesExiste(modalidades.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        return View(modalidades);
    }

    public bool modalidadesExiste(int id)
    {
        return _context.Modalidades.Any(c => c.Id == id);
    }

    [HttpGet]
    public async Task<IActionResult> Deletar(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var modalidades = await _context.Modalidades.FirstOrDefaultAsync(m => m.Id == id);
        if (modalidades == null)
        {
            return NotFound();
        }

        return View(modalidades);
    }


    [HttpPost, ActionName("Deletar")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deletar(int id)
    {
        var modalidades = await _context.Modalidades.FindAsync(id);
        if (modalidades == null)
        {
            return NotFound();
        }
        _context.Modalidades.Remove(modalidades);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Detalhes(int id)
    {
        var modalidades = await _context.Modalidades.FindAsync(id);
        if (modalidades == null)
        {
            return NotFound();
        }
        return View(modalidades);
    }
    
    public IActionResult Fechamento()
    {
        var somaPorModalidade = _context.Modalidades
       .GroupBy(m => m.Modalidade)
       .Select(g => new
       {
           Modalidade = g.Key,
           Quantidade = g.Sum(m => m.Quantidade)
       }).ToList();

        ViewBag.TotalGeral = somaPorModalidade.Sum(item => item.Quantidade);
        ViewBag.Total = somaPorModalidade;
        return View();
    }
}