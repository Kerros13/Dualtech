using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dualtech.Data;
using Dualtech.Modelos;

namespace Dualtech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DualtechContext _context;

        public ProductosController(DualtechContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [HttpGet]
        [Route("~/api/[controller]/getAll")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducto()
        {
            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = await _context.Producto.ToListAsync()
            });
        }

        // GET: api/Productos/5
        [HttpGet]
        [Route("~/api/[controller]/getById/{Id}")]
        public async Task<ActionResult<Producto>> GetProducto(long id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "El producto no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = producto
            });
        }

        // PUT: api/Productos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("~/api/[controller]/update/{Id}")]
        public async Task<IActionResult> PutProducto(long id, Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "El producto no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ProductoExists(id))
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "El producto no existe",
                        Errors = e.Message,
                        data = "[]"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "",
                        Errors = e.Message,
                        data = "[]"
                    });
                }
            }

            return Ok(new
            {
                Success = true,
                Message = "Se actualizo la informacion del producto",
                Errors = "[]",
                data = producto
            });
        }

        // POST: api/Productos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("~/api/[controller]/create")]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            try
            {
                producto.ProductoId = 0;
                _context.Producto.Add(producto);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Producto creado con exito",
                    Errors = "[]",
                    data = producto
                });
            }
            catch(DbUpdateException e)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "",
                    Errors = e.Message,
                    data = "[]"
                });
            }
            
        }

        // DELETE: api/Productos/5
        [HttpDelete]
        [Route("~/api/[controller]/delete/{Id}")]
        public async Task<ActionResult<Producto>> DeleteProducto(long id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "El producto no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Producto eliminado con exito",
                Errors = "[]",
                data = producto

            });
        }

        private bool ProductoExists(long id)
        {
            return _context.Producto.Any(e => e.ProductoId == id);
        }
    }
}
