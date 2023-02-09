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
    [Route("api/cuentas")]
    [ApiController]
    public class TpCuentumsController : ControllerBase
    {
        private readonly DBRetoNetContext _context;

        public TpCuentumsController(DBRetoNetContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TpCuentum>>> GetTpCuenta()
        {
            return await _context.TpCuenta.ToListAsync();
        }

        [HttpGet("{id}/{noCuenta}")]
        public async Task<ActionResult<TpCuentum>> GetTpCuentum(int id, int noCuenta)
        {
            try
            {
                var tpCuentum = await _context.TpCuenta.FindAsync(id, noCuenta);

                if (tpCuentum == null)
                    return Ok("La información de la cuenta suministrada para la búsqueda no arrojo ningún resultado.");

                return tpCuentum;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error al obtener la información de la cuenta.", Exception = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutTpCuentum(TpCuentum tpCuentum)
        {
            try
            {
                if (TpCuentumExists(tpCuentum.NumeroCuenta))
                {
                    _context.Entry(tpCuentum).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                    return Ok("La información suministrada para la actualización no arrojo ningún resultado.");

                var tpCuentaResult = await _context.TpCuenta.FindAsync(tpCuentum.IdentificacionPersona, tpCuentum.NumeroCuenta);

                return Ok(tpCuentaResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error al actualizar la información de la cuenta.", Exception = ex.Message });
            }

        }

        [HttpPost]
        public async Task<ActionResult<TpCuentum>> PostTpCuentum(TpCuentum tpCuentum)
        {
            try
            {
                if (TpCuentumExists(tpCuentum.IdentificacionPersona))
                    return Conflict("La información suministrada ya se encuentra relacionada a una cuenta existente.");

                _context.TpCuenta.Add(tpCuentum);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTpCuentum", new { id = tpCuentum.IdentificacionPersona, noCuenta = tpCuentum.NumeroCuenta }, tpCuentum);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error en el proceso de creación de la cuenta.", Exception = ex.Message });
            }
        }

        [HttpDelete("{id}/{noCuenta}")]
        public async Task<IActionResult> DeleteTpCuentum(int id, int noCuenta)
        {
            try
            {
                var tpCuentum = await _context.TpCuenta.FindAsync(id, noCuenta);

                if (tpCuentum == null)
                    return Ok("La información suministrada para la eliminación no arrojo ningún resultado.");

                if(TmMovimientoExists(noCuenta))
                    return Conflict("La Cuenta suministrada posee movimientos relacionadas, no se puede proceder con la eliminación.");

                _context.TpCuenta.Remove(tpCuentum);
                await _context.SaveChangesAsync();

                return Ok(string.Format("La cuenta numero: {0} ha sido eliminado correctamente", noCuenta));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error en el proceso de eliminación de la cuenta.", Exception = ex.Message });
            }
        }

        private bool TpCuentumExists(int numeroCuenta)
        {
            return _context.TpCuenta.Any(e => e.NumeroCuenta == numeroCuenta);
        }

        private bool TmMovimientoExists(int numeroCuenta)
        {
            return _context.TmMovimientos.Any(e => e.NumeroCuenta == numeroCuenta);
        }
    }
}
