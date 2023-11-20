using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProdCa.DAO;
using ProdCa.Models;

namespace ProdCa.Controllers
{
    public class ProductoController : Controller
    {
        private readonly MyContext _context;

        public static string mensaje = "";

        public ProductoController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            MyViewModel myModel = new MyViewModel()
            {
                newProduct = new Producto(),
                ListaProductos = _context.Products.Include(a => a.AsociacionPorProducto).ToList()
            };

            return View(myModel);
        }

        [HttpGet("products")]
        [HttpPost("products/add")]
        public IActionResult AgregarProducto(Producto nuevoProducto)
        {
            int coincidencia = _context.Products.Include(a => a.AsociacionPorProducto).Where(n => n.Nombre == nuevoProducto.Nombre).Count();

            if (coincidencia == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(nuevoProducto);
                    _context.SaveChanges();

                    mensaje = "";
                    ViewBag.Error = mensaje;

                    return RedirectToAction("Index", "Producto");
                }
                else
                {
                    mensaje = "Todos los campos del producto son requeridos";
                    ViewBag.Error = mensaje;
                }
            }
            else
            {
                mensaje = "El producto ya existe en la lista";
                ViewBag.Error = mensaje;
            }

            MyViewModel myModel = new MyViewModel()
            {
                newProduct = new Producto(),
                ListaProductos = _context.Products.Include(a => a.AsociacionPorProducto).ToList()
            };

            return View("Index", myModel);
        }

        [Route("products/{ProdId}")]
        public IActionResult AsociarCategoria(int ProdId)
        {
            var listaNombre = _context.Associations.Include(p => p.Producto).Where(x => x.ProductoId == ProdId).Include(c => c.Categoria).Select(n => n.Categoria.Nombre).ToList();

            var listaNombreExcluido = _context.Categories.Select(x => x.Nombre).ToList();

            MyViewModel myModel = new MyViewModel()
            {
                ListaCategoriaPorProducto = _context.Associations.Include(p => p.Producto).Where(x => x.ProductoId == ProdId).Include(c => c.Categoria).ToList(),
                ListaCategoriaExcluida = listaNombreExcluido.Except(listaNombre)
            };

            ViewBag.ProductoId = ProdId;

            ViewBag.Titulo = _context.Products.Where(n => n.ProductoId == ProdId).Select(i => i.Nombre).FirstOrDefault();

            return View(myModel);
        }

        [HttpGet]
        [HttpPost]
        [Route("products/{ProdId}/add")]
        public IActionResult IncluirCategoria(string NombreCategoria, int ProdId)
        {
            Asociacion listaAsociacion = new Asociacion();

            int CatId = _context.Categories.Where(n => n.Nombre == NombreCategoria).Select(i => i.CategoriaId).FirstOrDefault();

            listaAsociacion.ProductoId = ProdId;
            listaAsociacion.CategoriaId = CatId;

            if (ModelState.IsValid)
            {
                _context.Add(listaAsociacion);
                _context.SaveChanges();

                return RedirectToAction("AsociarCategoria", "Producto", new { ProdId });
            }

            return View();
        }

        private bool ProductoExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductoId == id)).GetValueOrDefault();
        }
    }
}