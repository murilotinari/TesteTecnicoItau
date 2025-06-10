using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteTecnicoItau.Domain.Models
{
    public class PrecoMedioDto(decimal precoMedio)
    {
        public decimal PrecoMedio { get; set; } = precoMedio;
    }
}
