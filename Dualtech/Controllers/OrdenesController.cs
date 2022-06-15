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
    public class OrdenesController : ControllerBase
    {
        private readonly DualtechContext _context;

        public OrdenesController(DualtechContext context)
        {
            _context = context;
        }

        // GET: api/Ordenes
        [HttpGet]
        [Route("~/api/[controller]/getAll")]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrden()
        {
         
            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = await _context.Orden.ToListAsync(),
                //Detalles = await _context.DetalleOrden.FindAsync(Orden.OrdenId)
            });
        }

        // GET: api/Ordenes/5
        [HttpGet]
        [Route("~/api/[controller]/getById/{Id}")]
        public async Task<ActionResult<Orden>> GetOrden(long id)
        {
            var orden = await _context.Orden.FindAsync(id);

            if (orden == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "La orden no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = orden
            });
        }

        // PUT: api/Ordenes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("~/api/[controller]/update/{Id}")]
        public async Task<IActionResult> PutOrden(long id, Orden orden)
        {
            if (id != orden.OrdenId)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "La orden no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }
            
            _context.Entry(orden).State = EntityState.Modified;

            try
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (!ClienteExists(orden.ClienteId))
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "El cliente ingresado no existe",
                            Errors = e.Message,
                            data = "[]"
                        });
                    }
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!OrdenExists(id))
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "La orden no existe",
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
                Message = "Se actualizo la orden",
                Errors = "[]",
                data = orden
            });
        }

        // POST: api/Ordenes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("~/api/[controller]/create")]
        public async Task<ActionResult<Orden>> PostOrden(Orden orden)
        {
            try
            {
                orden.OrdenId = 0;
                orden.Subtotal = 0;
                orden.Impuesto = 0;
                orden.Total = 0;

                _context.Orden.Add(orden);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Cliente creado con exito",
                    Errors = "[]",
                    data = orden
                });
            }
            catch (DbUpdateException e)
            {
                if (!ClienteExists(orden.ClienteId))
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "El cliente ingresado no existe",
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
            
        }

        // DELETE: api/Ordenes/5
        [HttpDelete]
        [Route("~/api/[controller]/delete/{Id}")]
        public async Task<ActionResult<Orden>> DeleteOrden(long id)
        {
            var orden = await _context.Orden.FindAsync(id);
            if (orden == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "La orden no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.Orden.Remove(orden);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Orden eliminado con exito",
                Errors = "[]",
                data = orden
            });
        }

        private bool OrdenExists(long id)
        {
            return _context.Orden.Any(e => e.OrdenId == id);
        }

        private bool ClienteExists(long id)
        {
            return _context.Cliente.Any(e => e.ClienteId == id);
        }
    }
}
