using Microsoft.AspNetCore.Mvc;
using PDF.Database;
using PDF.Excel;
using PDF.Models;
using System;
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

                return File(ImagemByteDados, "text/csv", $"PedidosxNfs_{DateTime.Now.ToShortDateString().Replace("/", "_")}.csv");
            }
        }

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
