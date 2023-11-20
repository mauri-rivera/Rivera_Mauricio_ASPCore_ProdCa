using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProdCa.DAO;
using ProdCa.Models;

namespace ProdCa.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly MyContext _context;

        public static string mensaje = "";

        public CategoriaController(MyContext context)
        {
            _context = context;
        }

        [Route("categories")]
        public IActionResult Index()
        {
            MyViewModel myModel = new MyViewModel()
            {
                newCategory = new Categoria(),
                ListaCategorias = _context.Categories.Include(a => a.AsociacionPorCategoria).ToList()
            };

            return View(myModel);
        }

        [HttpGet]
        [HttpPost]
        [Route("categories/add")]
        public IActionResult AgregarCategoria(Categoria nuevaCategoria)
        {
            int coincidencia = _context.Categories.Include(a => a.AsociacionPorCategoria).Where(n => n.Nombre == nuevaCategoria.Nombre).Count();

            if (coincidencia == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nuevaCategoria);
                    _context.SaveChanges();

                    mensaje = "";
                    ViewBag.Error = mensaje;

                    return RedirectToAction("Index", "Categoria");
                }
                else
                {
                    mensaje = "La categoría es requerida";
                    ViewBag.Error = mensaje;
                }
            }
            else
            {
                mensaje = "La categoria ya existe en la lista";
                ViewBag.Error = mensaje;
            }

            MyViewModel myModel = new MyViewModel()
            {
                newCategory = new Categoria(),
                ListaCategorias = _context.Categories.Include(a => a.AsociacionPorCategoria).ToList()
            };

            return View("Index", myModel);
        }

        [Route("categories/{CtgId}")]
        public IActionResult AsociarProducto(int CtgId)
        {
            MyViewModel myModel = new MyViewModel()
            {
                ListaProductoPorCategoria = _context.Associations.Include(p => p.Categoria).Where(x => x.CategoriaId == CtgId).Include(c => c.Producto).ToList(),
                ListaProductoExcluida = _context.Products.Include(a => a.AsociacionPorProducto).ThenInclude(p => p.Categoria).Where(x => x.AsociacionPorProducto.Max(h => h.CategoriaId) != CtgId).ToList()
            };

            ViewBag.CategoriaId = CtgId;

            return View(myModel);
        }

        [HttpGet]
        [HttpPost]
        [Route("categories/{CtgId}/add")]
        public IActionResult IncluirProducto(string NombreProducto, int CtgId)
        {
            Asociacion listaAsociacion = new Asociacion();

            int ProId = _context.Products.Where(n => n.Nombre == NombreProducto).Select(i => i.ProductoId).FirstOrDefault();

            listaAsociacion.ProductoId = ProId;
            listaAsociacion.CategoriaId = CtgId;

            if (ModelState.IsValid)
            {
                _context.Add(listaAsociacion);
                _context.SaveChanges();

                return RedirectToAction("AsociarProducto", "Categoria", new { CtgId });
            }

            return View();
        }

        private bool CategoriaExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }
    }
}
