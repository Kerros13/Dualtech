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
    public class DetalleOrdensController : ControllerBase
    {
        private readonly DualtechContext _context;

        public DetalleOrdensController(DualtechContext context)
        {
            _context = context;
        }

        // GET: api/DetalleOrdens
        [HttpGet]
        [Route("~/api/[controller]/getAll")]
        public async Task<ActionResult<IEnumerable<DetalleOrden>>> GetDetalleOrden()
        {
            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = await _context.DetalleOrden.ToListAsync()
            });
        }

        // GET: api/DetalleOrdens/5
        [HttpGet]
        [Route("~/api/[controller]/getById/{Id}")]
        public async Task<ActionResult<DetalleOrden>> GetDetalleOrden(long id)
        {
            var detalleOrden = await _context.DetalleOrden.FindAsync(id);

            if (detalleOrden == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Los detalles no existen",
                    Errors = "[]",
                    data = "[]"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = detalleOrden
            });
        }

        // PUT: api/DetalleOrdens/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("~/api/[controller]/update/{Id}")]
        public async Task<IActionResult> PutDetalleOrden(long id, DetalleOrden detalleOrden)
        {
            if (id != detalleOrden.DetalleOrdenid)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Los detalles no existen",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.Entry(detalleOrden).State = EntityState.Modified;
            var producto = await _context.Producto.FindAsync(detalleOrden.ProductoId);
            var cantidad = Convert.ToInt64(detalleOrden.Cantidad);
            var impuetos = Decimal.Multiply(detalleOrden.Cantidad, producto.Precio);
            var imp = 0.15m;
            impuetos = Decimal.Multiply(impuetos, imp);
            var subtotal = Decimal.Multiply(detalleOrden.Cantidad, producto.Precio);
            var total = impuetos + subtotal;

            detalleOrden.DetalleOrdenid = 0;
            detalleOrden.Impuestos = impuetos;
            detalleOrden.Subtotal = subtotal;
            detalleOrden.Total = total;

            try
            {
                try
                {
                    if(cantidad <= producto.Existencia) 
                    {
                        
                        await _context.SaveChangesAsync();
                        producto.Existencia = producto.Existencia - cantidad;
                        _context.Entry(producto).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "No hay suficiente existencia de este producto",
                            Errors = "[]",
                            data = producto
                        });
                    }
                }
                catch (DbUpdateException e)
                {
                    if (!ProductoExists(detalleOrden.ProductoId))
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "El producto ingresado no existe",
                            Errors = e.Message,
                            data = "[]"
                        });
                    }else if(!OrdenExists(detalleOrden.OrdenId)){
                        return Ok(new
                        {
                            Success = false,
                            Message = "La orden ingresado no existe",
                            Errors = e.Message,
                            data = "[]"
                        });
                    }
                }

            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!DetalleOrdenExists(id))
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "Los detalles no existe",
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
                Message = "Se actualizo la informacion del cliente",
                Errors = "[]",
                data = detalleOrden
            });
        }

        // POST: api/DetalleOrdens
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("~/api/[controller]/create")]
        public async Task<ActionResult<DetalleOrden>> PostDetalleOrden(DetalleOrden detalleOrden)
        {
            try
            {
                try 
                {
                    var producto = await _context.Producto.FindAsync(detalleOrden.ProductoId);
                    var orden = await _context.Orden.FindAsync(detalleOrden.OrdenId);
                    var cantidad = Convert.ToInt64(detalleOrden.Cantidad);
                    if (cantidad <= producto.Existencia)
                    {
                        var impuetos = Decimal.Multiply(detalleOrden.Cantidad, producto.Precio);
                        var imp = 0.15m;
                        impuetos = Decimal.Multiply(impuetos, imp);
                        var subtotal = Decimal.Multiply(detalleOrden.Cantidad, producto.Precio);
                        var total = impuetos + subtotal;

                        detalleOrden.DetalleOrdenid = 0;
                        detalleOrden.Impuestos = impuetos;
                        detalleOrden.Subtotal = subtotal;
                        detalleOrden.Total = total;


                        _context.DetalleOrden.Add(detalleOrden);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "No hay suficiente existencia de este producto",
                            Errors = "[]",
                            data = producto
                        });
                    }
                    return Ok(new
                    {
                        Success = true,
                        Message = "Detalles creados con exito",
                        Errors = "[]",
                        data = detalleOrden
                    });
                }
                catch (DbUpdateException e)
                {
                    if (!ProductoExists(detalleOrden.ProductoId))
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "El producto ingresado no existe",
                            Errors = e.Message,
                            data = "[]"
                        });
                    }
                    else if (!OrdenExists(detalleOrden.OrdenId))
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "La orden ingresado no existe",
                            Errors = e.Message,
                            data = "[]"
                        });
                    }
                }
            }
            catch (DbUpdateException e)
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

        // DELETE: api/DetalleOrdens/5
        [HttpDelete]
        [Route("~/api/[controller]/delete/{Id}")]
        public async Task<ActionResult<DetalleOrden>> DeleteDetalleOrden(long id)
        {
            var detalleOrden = await _context.DetalleOrden.FindAsync(id);
            if (detalleOrden == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Los detalles no existen",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.DetalleOrden.Remove(detalleOrden);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Detalles eliminados con exito",
                Errors = "[]",
                data = detalleOrden
            });
        }

        private bool DetalleOrdenExists(long id)
        {
            return _context.DetalleOrden.Any(e => e.DetalleOrdenid == id);
        }

        private bool OrdenExists(long id)
        {
            return _context.Orden.Any(e => e.OrdenId == id);
        }

        private bool ProductoExists(long id)
        {
            return _context.Producto.Any(e => e.ProductoId == id);
        }

        private bool DetalleProductoExists(long id)
        {
            return _context.DetalleOrden.Any(e => e.ProductoId == id);
        }

    }
}
