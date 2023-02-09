using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TpTipoCuentum
    {
        public TpTipoCuentum()
        {
            TpCuenta = new HashSet<TpCuentum>();
        }

        public int Id { get; set; }
        public string? TipoCuenta { get; set; }

        public virtual ICollection<TpCuentum> TpCuenta { get; set; }
    }
}
