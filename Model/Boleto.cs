using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuth.Model
{
    public class Boleto
    {
         public string Id { get; set; }
    public string NossoNumero { get; set; }
    public decimal Valor { get; set; }
    public string Status { get; set; }
    public DateTime Vencimento { get; set; }
    public DateTime DataEmissao { get; set; }
    public string LinhaDigitavel { get; set; }
    public string UrlBoleto { get; set; }
    }
}