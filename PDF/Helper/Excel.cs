using Linq.Csv;
using PDF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PDF.Excel
{
    public class ExcelService: IDisposable
    {
        public static string ReadExcelAsCsv(string filePath)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheets.Worksheet(1);
                try
                {
                    var table = worksheet.RangeUsed().AsTable().AsNativeDataTable();

                    var result = new StringBuilder();

                    foreach (DataRow row in table.Rows)
                    {
                        result.Append(string.Join(';', row.ItemArray.Select(x => x.ToString().Trim())) + Environment.NewLine);
                    }

                    return result.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static void CreateExcel(string filePath, IEnumerable<Dados> dados)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    var line = string.Format("PedidoEnviado;Barra;Qtd_Pedido;DataPedido;Qtd_Atendida;Qtd_Atendida_Not;NumeroNotaPedido;Observacao");

                    sw.WriteLine(line);

                    for (int i = 0; i < dados.Count(); i++)
                    {
                        var a = dados.ElementAt(i).PedidoEnviado;
                        var b = dados.ElementAt(i).Barra;
                        var c = dados.ElementAt(i).Qtd_Pedido;
                        var d = dados.ElementAt(i).DataPedido;
                        var e = dados.ElementAt(i).Qtd_Atendida.ToString();
                        var f = dados.ElementAt(i).Qtd_Atendida_Not;
                        var g = dados.ElementAt(i).NumeroNotaPedido;
                        var h = dados.ElementAt(i).Observacao;
                        line = string.Format("{0};{1};{2};{3};{4};{5};{6}", a, b, c, d, e, f, g, h);

                        sw.WriteLine(line);
                        sw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro");
                Console.WriteLine(ex.Message);
            }
        }
        public void Dispose()
        {            
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}