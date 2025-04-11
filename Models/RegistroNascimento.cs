using System;

namespace DeMaria.Models
{
    public class RegistroNascimento
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public int RegistradoId { get; set; }
        public Pessoa Registrado { get; set; } = null!;
    }
} 