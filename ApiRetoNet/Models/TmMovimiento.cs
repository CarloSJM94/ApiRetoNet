using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TmMovimiento
    {
        public long Id { get; set; }
        public int? NumeroCuenta { get; set; }
        public int? IdTpTipoMovimiento { get; set; }
        public decimal? Valor { get; set; }
        public decimal? Saldo { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public bool? Exitoso { get; set; }

        public virtual TpTipoMovimiento? IdTpTipoMovimientoNavigation { get; set; }
        public virtual TpCuentum? NumeroCuentaNavigation { get; set; }
    }
}
