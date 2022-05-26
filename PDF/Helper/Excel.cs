using OperadoresRelatorio.Models;
using PDF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using PDF.Until;

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
                        var d = dados.ElementAt(i).DataPedido.ToString("dd/MM/yyyy");
                        var e = dados.ElementAt(i).Qtd_Atendida_ret;
                        var f = dados.ElementAt(i).Qtd_Atendida_Not;
                        var g = dados.ElementAt(i).NumeroNotaPedido;
                        var h = dados.ElementAt(i).Observacao;
                        line = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", a, b, c, d, e, f, g, h);

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

        public static void CreateCSVPedido(string filePath, IEnumerable<Pedido> dados1, IEnumerable<Pedido> dados2)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("Pedidos sem arquivo de NOTA FISCAL (D-2) ate o momento");
                    sw.WriteLine();

                    var line = string.Format("Data;Numero do Pedido;CNPJ Cliente;CNPJ / CDD;Nome do Arquivo");

                    sw.WriteLine(line);

                    for (int i = 0; i < dados1.Count(); i++)
                    {
                        var a = dados1.ElementAt(i).DtHrPedido;
                        var b = dados1.ElementAt(i).PedidoSemNota;
                        var c = dados1.ElementAt(i).FarmaciaCNPJ.toStringCnpj();
                        var d = dados1.ElementAt(i).CNPJ.toStringCnpj() + " - " + dados1.ElementAt(i).NomeCDD;
                        var e = dados1.ElementAt(i).ArquivoSemNota;
                        line = string.Format("{0};{1};{2};{3};{4}", a, b, c, d, e);

                        sw.WriteLine(line);
                        sw.Flush();
                    }
                    sw.WriteLine(dados1.Count() + " registros encontrados");


                    sw.WriteLine();
                    sw.WriteLine();

                    sw.WriteLine("Pedidos sem arquivo de RETORNO (D-2) ate o momento");
                    sw.WriteLine();
                    line = string.Format("Data;Numero do Pedido;CNPJ Cliente;CNPJ / CDD;Nome do Arquivo");

                    sw.WriteLine(line);

                    for (int i = 0; i < dados2.Count(); i++)
                    {
                        var a = dados2.ElementAt(i).DtHRPedido;
                        var b = dados2.ElementAt(i).PedidoEnviado;
                        var c = dados2.ElementAt(i).CNPJ.toStringCnpj();
                        var d = dados2.ElementAt(i).CNPJCDD.toStringCnpj() + " - " + dados2.ElementAt(i).NomeCDD;
                        var e = dados2.ElementAt(i).ArquivoSEMRETORNO;
                        line = string.Format("{0};{1};{2};{3};{4}", a, b, c, d, e);

                        sw.WriteLine(line);
                        sw.Flush();
                    }
                    sw.WriteLine(dados2.Count() + " registros encontrados");

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