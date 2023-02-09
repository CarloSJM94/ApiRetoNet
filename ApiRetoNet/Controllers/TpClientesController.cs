using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRetoNet.Models;

namespace ApiRetoNet.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class TpClientesController : ControllerBase
    {
        private readonly DBRetoNetContext _context;

        public TpClientesController(DBRetoNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpCliente>>> GetTpClientes()
        {
            return await _context.TpClientes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TpCliente>> GetTpCliente(int id)
        {
            try
            {
                var tpCliente = await _context.TpClientes.FindAsync(id);
                var tpPersona = await _context.TpPersonas.FindAsync(id);

                if (tpCliente == null || tpPersona == null)
                    return Ok("La identificación suministrada para la búsqueda no arrojo ningún resultado.");
                else
                    tpCliente.IdentificacionPersonaNavigation = tpPersona;

                return tpCliente;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel{ Message = "Ocurrió un error al obtener la información del cliente.", Exception = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutTpCliente([FromBody] TpCliente tpCliente)
        {
            try
            {
                if (TpClienteExists(tpCliente.IdentificacionPersona))
                {
                    _context.Entry(tpCliente).State = EntityState.Modified;
                    _context.Entry(tpCliente.IdentificacionPersonaNavigation).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                else
                    return Ok("La identificación suministrada para la actualización no arrojo ningún resultado.");

                var tpClienteResult = await _context.TpClientes.FindAsync(tpCliente.IdentificacionPersona);

                return Ok(tpClienteResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error al actualizar la información del cliente.", Exception = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TpCliente>> PostTpCliente(TpCliente tpCliente)
        {
            try
            {
                if (TpClienteExists(tpCliente.IdentificacionPersona))
                    return Conflict("La información suministrada ya se encuentra relacionada a un usuario existente.");

                _context.TpClientes.Add(tpCliente);
                
                await _context.SaveChangesAsync();
                
                var tpClienteResult = await _context.TpClientes.FindAsync(tpCliente.IdentificacionPersona);
                
                return Ok(tpClienteResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error en el proceso de creación del cliente.", Exception = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTpCliente(int id)
        {
            try
            {
                var tpCliente = await _context.TpClientes.FindAsync(id);
                var tpPersona = await _context.TpPersonas.FindAsync(id);

                if (tpCliente == null || tpPersona == null)
                    return Ok("La identificación suministrada para la eliminación no arrojo ningún resultado.");

                if (TpCuentumExists(id))
                    return Conflict("El cliente suministrado posee cuentas relacionadas, no se puede proceder con la eliminación.");


                _context.TpClientes.Remove(tpCliente);
                _context.TpPersonas.Remove(tpPersona);

                await _context.SaveChangesAsync();

                return Ok(string.Format("Cliente con identificación: {0} ha sido eliminado correctamente", id));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error en el proceso de eliminación del cliente.", Exception = ex.Message });
            }
        }

        private bool TpClienteExists(int id)
        {
            return _context.TpClientes.Any(e => e.IdentificacionPersona == id);
        }

        private bool TpCuentumExists(int id)
        {
            return _context.TpCuenta.Any(e => e.IdentificacionPersona == id);
        }
    }
}
