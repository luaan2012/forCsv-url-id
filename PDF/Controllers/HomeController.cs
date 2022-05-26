using Microsoft.AspNetCore.Mvc;
using OperadoresRelatorio.Models;
using PDF.Database;
using PDF.Excel;
using PDF.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PDF.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConvert _db;

        public HomeController(DbConvert db)
        {
            _db = db;
        }        
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult>DownloadCSV(int operadorLogistico_id = 0)
        {
            using (null)
            {
                var dados = await _db.ProcMultipleAsync<Dados>("P_ScriptCase_RelatorioPedidosxNotas", new
                {
                    OperadorLogistico_Grupo_id = operadorLogistico_id
                });

                var caminhoArquivo = Environment.CurrentDirectory + @"\Downloads\PedidosxNfs.csv";
                ExcelService.CreateExcel(caminhoArquivo, dados);

                byte[] ImagemByteDados = System.IO.File.ReadAllBytes(caminhoArquivo);

                return Json(ImagemByteDados);
            }
        }

        public async Task<IActionResult> DownloadCSVAnterior(int operadorLogistico_id = 0)
        {
            using (null)
            {
                var dados = await _db.ProcMultipleAsync<Dados>("P_ScriptCase_RelatorioPedidosxNotas_MesAnterior", new
                {
                    OperadorLogistico_Grupo_id = operadorLogistico_id
                });

                var caminhoArquivo = Environment.CurrentDirectory + @"\Downloads\PedidosxNfsAnterior.csv";
                ExcelService.CreateExcel(caminhoArquivo, dados);

                byte[] ImagemByteDados = System.IO.File.ReadAllBytes(caminhoArquivo);

                return Json(ImagemByteDados);
            }
        }

        public async Task<IActionResult> DownloadCSVPedido(int operadorLogistico_id = 0, int operadorLogistico_grupo_id = 0, int diasAte = 2, int origem = 0, int enviado = 0)
        {
            using (null)
            {
                origem = (origem == 1) ? 7 : origem;

                var dados1 = await _db.ProcMultipleAsync<Pedido>("P_ADM_Operadores_ListaNotas", new
                {
                    Padrao = 1,
                    OperadorLogistico_Grupo_id = operadorLogistico_grupo_id,
                    OperadorLogistico_id = operadorLogistico_id,
                    DiasAte = diasAte,
                    Origem = origem,
                    Enviado = enviado
                });

                var dados2 = await _db.ProcMultipleAsync<Pedido>("P_ADM_Operadores_ListaRetornos", new
                {
                    Padrao = 1,
                    OperadorLogistico_Grupo_id = operadorLogistico_grupo_id,
                    Origem = origem,
                });

                var caminhoArquivo = Environment.CurrentDirectory + @"\Downloads\PedidosSemNota.csv";
                ExcelService.CreateCSVPedido(caminhoArquivo, dados1, dados2);

                byte[] ImagemByteDados = System.IO.File.ReadAllBytes(caminhoArquivo);

                return Json(ImagemByteDados);
            }
        }

        //public async Task<IActionResult> DownloadCSVAnterior(int operadorLogistico_id = 0)
        //{
        //    using (null)
        //    {
        //        var dados = await _db.ProcMultipleAsync<Dados>("P_ScriptCase_RelatorioPedidosxNotas_MesAnterior", new
        //        {
        //            OperadorLogistico_Grupo_id = operadorLogistico_id
        //        });

        //        var caminhoArquivo = Environment.CurrentDirectory + @"\Downloads\PedidosxNfsAnterior.csv";
        //        ExcelService.CreateExcel(caminhoArquivo, dados);

        //        byte[] ImagemByteDados = System.IO.File.ReadAllBytes(caminhoArquivo);

        //        return File(ImagemByteDados, "text/csv", $"PedidosMesAnterior_{DateTime.Now.ToShortDateString().Replace("/", "_")}.csv");
        //    }
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
