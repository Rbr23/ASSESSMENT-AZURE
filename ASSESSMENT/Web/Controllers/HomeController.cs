using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using Web.Models;
using Web.Models.Amigo;
using Web.Models.Estado;
using Web.Models.Home;
using Web.Models.Pais;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var client = new RestClient();

            var requestEstado = new RestRequest("https://localhost:44318/api/Estados");
            var responseEstado = client.Get<List<EstadoViewModel>>(requestEstado);


            var requestPais = new RestRequest("https://localhost:44318/api/Pais");
            var responsePais = client.Get<List<ListarPaisViewModel>>(requestPais);

            var requestAmigos = new RestRequest("https://localhost:44304/api/Amigos");
            var responseAmigos = client.Get<List<ListarAmigoViewModel>>(requestAmigos);

            var pgInicial = new HomeViewModel
            {
                QuantidadeAmigos = responseAmigos.Data.Count,
                QuantidadeEstados = responseEstado.Data.Count,
                QuantidadePaises = responsePais.Data.Count
            };

            return View(pgInicial);
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
