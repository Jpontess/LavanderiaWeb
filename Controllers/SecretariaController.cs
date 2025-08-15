using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebLavApp.Data;
using WebLavApp.Models;

namespace WebLavApp.Controllers;

public class SecretariaController : Controller
{
    public readonly AppDbContext _context;

    public SecretariaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? filtroNome, string? filtroSecretaria, DateTime? filtroDataInicio, DateTime? filtroDataFim)
    {
        var SecretariaQuery = _context.Secretarias.AsQueryable();

        // Aplicar filtro
        if (!string.IsNullOrEmpty(filtroNome))
        {
            SecretariaQuery = SecretariaQuery.Where(s => s.Nome!.Contains(filtroNome));
        }
        if (!string.IsNullOrEmpty(filtroSecretaria))
        {
            SecretariaQuery = SecretariaQuery.Where(s => s.Secretarias!.Contains(filtroSecretaria));
        }
        if (filtroDataInicio.HasValue)
        {
            SecretariaQuery = SecretariaQuery.Where(s => s.Data >= filtroDataInicio.Value);
        }
        if (filtroDataFim.HasValue)
        {
            SecretariaQuery = SecretariaQuery.Where(s => s.Data <= filtroDataFim.Value);
        }

        //obter departamento distintos para dropdown
        ViewBag.Secretarias = await _context.Secretarias
        .Select(s => s.Secretarias)
        .Distinct()
        .OrderBy(d => d)
        .ToListAsync();

        //manter os filtros na viewData para manter o estado
        ViewData["FiltroNome"] = filtroNome;
        ViewData["filtroSecretaria"] = filtroSecretaria;
        ViewData["FiltroDataInicio"] = filtroDataInicio?.ToString("yyyy-MM-dd");
        ViewData["FiltroDataFim"] = filtroDataFim?.ToString("yyyy-MM-dd");

        return View(await SecretariaQuery.ToListAsync());
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Secretarias,Material,Cor,Quantidade")] Secretaria secretaria)
    {
        if (ModelState.IsValid)
        {
            _context.Add(secretaria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(secretaria);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var secretaria = await _context.Secretarias.FindAsync(id);
        if (secretaria == null)
        {
            return NotFound();
        }
        ViewBag.Secretarias = new List<string>
        {
            "Secretaria da mulher",
            "Maturidade",
            "Promoção Social",
            "Cultura"
        };

        return View(secretaria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Secretarias,Material,Cor,Quantidade")] Secretaria secretaria)
    {
        if (id != secretaria.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(secretaria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!secretariaExiste(secretaria.Id))
                {
                    return NotFound();
                }
                throw;
            }
        }
        return View(secretaria);
    }

    public bool secretariaExiste(int id)
    {
        return _context.Secretarias.Any(s => s.Id == id);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var secretaria = await _context.Secretarias.FirstOrDefaultAsync(s => s.Id == id);

        if (secretaria == null)
        {
            return NotFound();
        }

        return View(secretaria);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var secretaria = await _context.Secretarias.FindAsync(id);
        if (secretaria == null)
        {
            return NotFound();
        }
        _context.Secretarias.Remove(secretaria);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var secretaria = await _context.Secretarias.FindAsync(id);
        if (secretaria == null)
        {
            return NotFound();
        }


        ViewBag.Secretarias = new List<string>
        {
            "Secretaria da mulher",
            "Maturidade",
            "Promoção Social",
            "Cultura"
        };

        return View(secretaria);
    }


    public IActionResult Fechamento(int? data)
    {
        var context = _context.Secretarias.AsQueryable();

        if (data > 0)
        {
            context = context.Where(s => s.Data.Month == data.Value);
        }

        var agrupar = context
        .GroupBy(s => s.Secretarias)
        .Select(g => new WebLavApp.Models.Secretaria
        {
            Secretarias = g.Key,
            Data = g.Min(x => x.Data),
            Quantidade = g.Sum(x => x.Quantidade)
        })
        .OrderBy(s => s.Secretarias)
        .ToList();

        ViewBag.Total = context.Sum(s => s.Quantidade);
        ViewData["mes"] = data?.ToString() ?? "";

        return View(agrupar);
    }
}