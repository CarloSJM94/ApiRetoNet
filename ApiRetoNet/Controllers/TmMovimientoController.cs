using ApiRetoNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApiRetoNet.Controllers
{
    [Route("api/movimientos")]
    [ApiController]
    public class TmMovimientoController : ControllerBase
    {
        private readonly DBRetoNetContext _context;

        public TmMovimientoController(DBRetoNetContext context)
        {
            _context = context;
        }

        [HttpPost("aplicarMovimiento")]
        public async Task<ActionResult> PostTmMovimiento(decimal valor, int identificacion, int numeroCuenta, int tipoMovimiento)
        {
            try
            {
                var tpCuenta = await _context.TpCuenta.FindAsync(identificacion, numeroCuenta);
                var tpTipoMovimiento = await _context.TpTipoMovimientos.FindAsync(tipoMovimiento);
                var Exitoso = true;

                if (tpCuenta == null)
                    return Ok("La informacion suministrada de la cuenta no existe");

                if (tpTipoMovimiento == null)
                    return Ok("El tipo de movimiento especificado no existe");

                if (tpTipoMovimiento.TipoMovimiento == "Ingreso")
                {
                    tpCuenta.Saldo += valor;

                    if (GuardarMovimiento(valor, identificacion, numeroCuenta, tipoMovimiento, tpCuenta, Exitoso) <= 0)
                        return StatusCode(500, "Se presentó un error al momento de guardar el movimiento.");
                }
                else
                {
                    if (tpCuenta.Saldo - valor >= 0)
                        tpCuenta.Saldo -= valor;
                    else
                        Exitoso = false;

                    if (GuardarMovimiento(valor, identificacion, numeroCuenta, tipoMovimiento, tpCuenta, Exitoso) <= 0)
                        return StatusCode(500, "Se presentó un error al momento de guardar el movimiento.");

                    if (!Exitoso)
                        return Conflict(string.Format("Saldo no disponible - Valor solicitado: {0} - Saldo Actual {1}", valor, tpCuenta.Saldo));
                }

                return Ok(string.Format("Movimiento aplicado; su nuevo saldo es: {0}", tpCuenta.Saldo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error aplicando el movimiento a la cuenta especificada.", Exception = ex.Message });
            }
        }

        [HttpGet("obtenerEstadoCuenta")]
        public async Task<ActionResult> GetEstadoCuenta(int identificacion, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var RptEstadoCuenta = new List<TpCuentum>();

                var Cuentas = _context.TpCuenta.Where(b => b.IdentificacionPersona.Equals(identificacion)).ToList();

                if (Cuentas == null)
                    return Ok("La información suministrada para obtener el estado de cuenta no arrojo ningún resultado.");

                foreach (var c in Cuentas)
                {
                    TpCuentum MovimientosCuenta = new TpCuentum();

                    MovimientosCuenta = c;

                    var TipoCuenta = await _context.TpTipoCuenta.FindAsync(c.IdTipoCuenta);

                    var Movimientos = _context.TmMovimientos.Where(b => b.NumeroCuenta.Equals(c.NumeroCuenta))
                                                            .Where(b => b.FechaMovimiento >= fechaInicio)
                                                            .Where(b => b.FechaMovimiento <= fechaFin).ToList();

                    if (Movimientos == null)
                        break;

                    MovimientosCuenta.IdTipoCuentaNavigation = TipoCuenta;
                    MovimientosCuenta.TmMovimientos = Movimientos;

                    RptEstadoCuenta.Add(MovimientosCuenta);
                }

                if (RptEstadoCuenta == null || RptEstadoCuenta.Count() <= 0)
                    return Ok("No existen movimientos en las cuentas relacionadas a la información suministrada.");

                return Ok(RptEstadoCuenta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ExceptionModel { Message = "Ocurrió un error al obtener la información de la cuenta.", Exception = ex.Message });
            }
        }

        private int GuardarMovimiento(decimal valor, int identificacion, int numeroCuenta, int tipoMovimiento, TpCuentum tpCuenta, bool existoso)
        {
            TmMovimiento tmMovimiento = new TmMovimiento
            {
                NumeroCuenta = numeroCuenta,
                IdTpTipoMovimiento = tipoMovimiento,
                Valor = valor,
                Saldo = tpCuenta.Saldo,
                FechaMovimiento = DateTime.Now,
                Exitoso = existoso
            };

            if(existoso)
                _context.Entry(tpCuenta).State = EntityState.Modified;

            _context.TmMovimientos.Add(tmMovimiento);

            return _context.SaveChangesAsync().Result;
        }
    }
}
