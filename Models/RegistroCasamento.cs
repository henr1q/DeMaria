using System;

namespace DeMaria.Models
{
    public class RegistroCasamento
    {
        public int Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime DataCasamento { get; set; }
        public int Conjuge1Id { get; set; }
        public int Conjuge2Id { get; set; }
        public Pessoa Conjuge1 { get; set; } = null!;
        public Pessoa Conjuge2 { get; set; } = null!;
    }
} 