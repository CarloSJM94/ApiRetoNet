using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TpCliente
    {
        public TpCliente()
        {
            TpCuenta = new HashSet<TpCuentum>();
        }

        public int IdentificacionPersona { get; set; }
        public bool? EstadoCliente { get; set; }
        public string? Contraseña { get; set; }

        public virtual TpPersona IdentificacionPersonaNavigation { get; set; } = null!;
        public virtual ICollection<TpCuentum> TpCuenta { get; set; }
    }
}
