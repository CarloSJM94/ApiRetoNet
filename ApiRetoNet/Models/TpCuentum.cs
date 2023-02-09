using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TpCuentum
    {
        public TpCuentum()
        {
            TmMovimientos = new HashSet<TmMovimiento>();
        }

        public int IdentificacionPersona { get; set; }
        public int NumeroCuenta { get; set; }
        public int? IdTipoCuenta { get; set; }
        public bool? EstadoCuenta { get; set; }
        public decimal? Saldo { get; set; }

        public virtual TpTipoCuentum? IdTipoCuentaNavigation { get; set; }
        public virtual TpCliente? IdentificacionPersonaNavigation { get; set; } = null!;
        public virtual ICollection<TmMovimiento> TmMovimientos { get; set; }
    }
}
