using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Estado;
using Web.Models.Pais;

namespace Web.Models.Amigo
{
    public class CreateAmigoViewModel
    {
        
        
            public string UrlFoto { get; set; }
            public string Nome { get; set; }
            public string SobreNome { get; set; }
            public string Email { get; set; }
            public string Telefone { get; set; }
            public DateTime DataAniversario { get; set; }
            public PaisViewModel PaisOrigiem { get; set; }
            public EstadoViewModel EstadoOrigem { get; set; }
        
    }
}
