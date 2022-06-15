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
    public class ClientesController : ControllerBase
    {
        private readonly DualtechContext _context;

        public ClientesController(DualtechContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        [Route("~/api/[controller]/getAll")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = await _context.Cliente.ToListAsync()
            });
        }

        // GET: api/Clientes/5
        [HttpGet]
        [Route("~/api/[controller]/getById/{Id}")]
        public async Task<ActionResult<Cliente>> GetCliente(long id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "El cliente no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "",
                Errors = "[]",
                data = cliente
            });
        }

        // PUT: api/Clientes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("~/api/[controller]/update/{Id}")]
        public async Task<IActionResult> PutCliente(long id, Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "El cliente no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException e)
                {
                    if (ClienteIdentidadExists(cliente.Identidad))
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "La identidad del cliente no puede ser igual al de otro",
                            Errors = e.Message,
                            data = "[]"
                        });
                    }
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!ClienteExists(id))
                {
                    return Ok(new
                    {
                        Success = false,
                        Message = "El cliente no existe",
                        Errors = e.Message ,
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
                data = cliente
            });
        }

        // POST: api/Clientes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("~/api/[controller]/create")]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            try
            {
                cliente.ClienteId = 0;
                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "Cliente creado con exito",
                    Errors = "[]",
                    data = cliente
                });

            }catch (DbUpdateException e){
                if (ClienteIdentidadExists(cliente.Identidad)) { 
                    return Ok(new
                    {
                        Success = false,
                        Message = "La identidad del cliente no puede ser igual al de otro",
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

        // DELETE: api/Clientes/5
        [HttpDelete]
        [Route("~/api/[controller]/delete/{Id}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(long id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "El cliente no existe",
                    Errors = "[]",
                    data = "[]"
                });
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Cliente Eliminado con exito",
                Errors = "[]",
                data = cliente
            });
        }

        private bool ClienteExists(long id)
        {
            return _context.Cliente.Any(e => e.ClienteId == id);
        }

        private bool ClienteIdentidadExists(string id)
        {
            return _context.Cliente.Any(e => e.Identidad == id);
        }
    }
}
