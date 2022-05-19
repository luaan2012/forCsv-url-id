using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDF.Models
{
    public class Dados
    {
        public string PedidoEnviado { get; set; }
        public string Barra { get; set; }
        public int Qtd_Pedido { get; set; }
        public string DataPedido { get; set; }
        public int Qtd_Atendida { get; set; }
        public string Observacao { get; set; }
        public string Qtd_Atendida_Not { get; set; }
        public string NumeroNotaPedido { get; set; }
    }
}
