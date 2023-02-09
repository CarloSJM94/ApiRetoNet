using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TpPersona
    {
        public int Identificacion { get; set; }
        public int? IdGenero { get; set; }
        public int? Edad { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }

        public virtual TpGenero? IdGeneroNavigation { get; set; }
        public virtual TpCliente? TpCliente { get; set; }
    }
}
