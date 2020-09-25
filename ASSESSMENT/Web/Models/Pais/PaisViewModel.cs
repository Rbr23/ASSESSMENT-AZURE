using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Estado;

namespace Web.Models.Pais
{
    public class PaisViewModel
    {
        public Guid Id { get; set; }
        public string UrlFoto { get; set; }
        public string Nome { get; set; }
        public virtual List<EstadoViewModel> Estados { get; set; } = new List<EstadoViewModel>();
    }
}
