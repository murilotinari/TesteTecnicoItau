using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteTecnicoItau.Domain.Models
{
    public class CotacaoAtivoDto(decimal cotacaoAtivo)
    {
        public decimal CotacaoAtivo { get; set; } = cotacaoAtivo;
    }
}
