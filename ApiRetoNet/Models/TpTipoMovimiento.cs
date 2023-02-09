using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TpTipoMovimiento
    {
        public TpTipoMovimiento()
        {
            TmMovimientos = new HashSet<TmMovimiento>();
        }

        public int Id { get; set; }
        public string? TipoMovimiento { get; set; }

        public virtual ICollection<TmMovimiento> TmMovimientos { get; set; }
    }
}
