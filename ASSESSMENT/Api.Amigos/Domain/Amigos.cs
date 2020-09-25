using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Amigos.Domain
{
    public class Amigos
    {
        public Guid Id { get; set; }
        public string UrlFoto { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataAniversario { get; set; }
        public virtual List<Amigos> Amigo { get; set; } = new List<Amigos>();
        public Guid Id_PaisOrigiem { get; set; }
        public Guid Id_EstadoOrigem { get; set; }
    }
}
