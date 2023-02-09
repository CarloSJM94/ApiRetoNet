using System;
using System.Collections.Generic;

namespace ApiRetoNet.Models
{
    public partial class TpGenero
    {
        public TpGenero()
        {
            TpPersonas = new HashSet<TpPersona>();
        }

        public int Id { get; set; }
        public string? Genero { get; set; }

        public virtual ICollection<TpPersona> TpPersonas { get; set; }
    }
}
