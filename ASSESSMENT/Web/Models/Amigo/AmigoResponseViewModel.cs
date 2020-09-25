using System;

namespace Web.Models.Amigo
{
    public class AmigoResponseViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    
    }
}
