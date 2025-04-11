using System;
using System.ComponentModel.DataAnnotations;

namespace DeMaria.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }

        [StringLength(100)]
        public string? NomePai { get; set; }

        [StringLength(100)]
        public string? NomeMae { get; set; }

        public DateTime? DataNascimentoPai { get; set; }
        public DateTime? DataNascimentoMae { get; set; }

        [StringLength(11)] // Only numbers: 12345678901
        public string? CpfPai { get; set; }

        [StringLength(11)] // Only numbers: 12345678901
        public string? CpfMae { get; set; }
    }
} 