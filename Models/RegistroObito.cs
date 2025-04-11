using System;

namespace DeMaria.Models
{
    public class RegistroObito
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime DataObito { get; set; }
        public int FalecidoId { get; set; }
        public Pessoa Falecido { get; set; } = null!;
    }
} 