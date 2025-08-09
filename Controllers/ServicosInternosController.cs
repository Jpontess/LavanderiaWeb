using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLavApp.Data;
using WebLavApp.Models;

namespace WebLavApp.Controllers;

public class ServicosInternosController : Controller
{
    private readonly AppDbContext _context;

    public ServicosInternosController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? filtroNome, string? filtroDepartamento, DateTime? filtroDataInicio, DateTime? filtroDataFim)
    {

        var ServicosQuery = _context.ServicoInternos.AsQueryable();

        // Aplicar filtro
        if (!string.IsNullOrEmpty(filtroNome))
        {
            ServicosQuery = ServicosQuery.Where(s => s.Nome!.Contains(filtroNome));
        }
        if (!string.IsNullOrEmpty(filtroDepartamento))
        {
            ServicosQuery = ServicosQuery.Where(s => s.Departamento!.Contains(filtroDepartamento));
        }
        if (filtroDataInicio.HasValue)
        {
            ServicosQuery = ServicosQuery.Where(s => s.Data >= filtroDataInicio.Value);
        }
        if (filtroDataFim.HasValue)
        {
            ServicosQuery = ServicosQuery.Where(s => s.Data <= filtroDataFim.Value);
        }

        //obter departamento distintos para dropdown
        ViewBag.Departamentos = await _context.ServicoInternos
        .Select(s => s.Departamento)
        .Distinct()
        .OrderBy(d => d)
        .ToListAsync();

        //manter os filtros na viewData para manter o estado
        ViewData["FiltroNome"] = filtroNome;
        ViewData["FiltroDepartamento"] = filtroDepartamento;
        ViewData["FiltroDataInicio"] = filtroDataInicio?.ToString("yyyy-MM-dd");
        ViewData["FiltroDataFim"] = filtroDataFim?.ToString("yyyy-MM-dd");

        return View(await ServicosQuery.ToListAsync());
    }


    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Departamento,Material,Cor,Quantidade")] ServicoInterno servicoInterno)
    {
        if (ModelState.IsValid)
        {
            _context.Add(servicoInterno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(servicoInterno);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicosIntenos = await _context.ServicoInternos.FindAsync(id);
        if (servicosIntenos == null)
        {
            return NotFound();
        }

        ViewBag.Departamentos = new List<string>
        {
            "Administração",
            "Relacinamento",
            "Compras",
            "RH",
            "Alimentação",
            "BPS",
            "Modalidades",
            "Refeitório",
            "Eventos",
            "Enfermaria"
        };
        return View(servicosIntenos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Departamento,Material,Cor,Quantidade")] ServicoInterno servico)
    {
        if (id != servico.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicosInternosExiste(servico.Id))
                {
                    return NotFound();
                }
                throw;
            }

        }
        return View(servico);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var servicos = await _context.ServicoInternos.FirstOrDefaultAsync(s => s.Id == id);
        if (servicos == null)
        {
            return NotFound();
        }
        return View(servicos);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Delete(int id)
    {
        var servico = await _context.ServicoInternos.FindAsync(id);
        if (servico == null)
        {
            return NotFound();
        }
        _context.ServicoInternos.Remove(servico);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var servicos = await _context.ServicoInternos.FindAsync(id);
        if (servicos == null)
        {
            return NotFound();
        }

        return View(servicos);
    }

    private bool ServicosInternosExiste(int id)
    {
        return _context.ServicoInternos.Any(s => s.Id == id);
    }


}