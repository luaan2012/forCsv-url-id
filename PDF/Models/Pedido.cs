using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OperadoresRelatorio.Models
{
    public class Pedido
    {
        public int PedidoSemNota { get; set; }
        public DateTime DtHrPedido { get; set; }
        public string FarmaciaCNPJ { get; set; }
        public string Distribuidora { get; set; }
        public string NomeCDD { get; set; }
        public string CNPJ { get; set; }
        public string ArquivoSemNota { get; set; }
        public int PedidoEnviado { get; set; }
        public DateTime DtHRPedido { get; set; }
        public string CNPJCDD { get; set; }
        public string ArquivoSEMRETORNO { get; set; }
        

    }
}
