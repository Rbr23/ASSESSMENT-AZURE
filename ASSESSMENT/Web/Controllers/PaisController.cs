using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using RestSharp;
using Web.Models.Estado;
using Web.Models.Pais;

namespace Web.Controllers
{
    public class PaisController : Controller
    {
        private readonly string _UriAPI = "https://localhost:44318/";

        // GET: Controller
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "api/Pais");

            var response = client.Get<List<ListarPaisViewModel>>(request);

            return View(response.Data);
        }

        // GET: Controller/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "api/Pais/" + id, DataFormat.Json);

            var response = client.Get<PaisViewModel>(request);

            var requestEstados = new RestRequest(_UriAPI + "api/Pais/" + id + "/Estados", DataFormat.Json);
            response.Data.Estados = client.Get<List<EstadoViewModel>>(requestEstados).Data;

            return View(response.Data);
        }

        // GET: Controller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CriarPaisViewModel criarPaisViewModel, IFormFile foto)
        {
            var urlFoto = UploadFotoPessoa(foto, criarPaisViewModel.Nome);
            criarPaisViewModel.UrlFoto = urlFoto;

            try
            {
                if (ModelState.IsValid == false)
                    return View(criarPaisViewModel);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "api/Pais", DataFormat.Json);

                request.AddJsonBody(criarPaisViewModel);

                var response = await client.PostAsync<CriarPaisViewModel>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaisController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "api/Pais/" + id, DataFormat.Json);

            var response = client.Get<EditarPaisViewModel>(request);

            return View(response.Data);
        }

        // POST: PaisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, EditarPaisViewModel editarPaisViewModel, IFormFile foto)
        {
            if (foto != null)
            {
                var urlFoto = UploadFotoPessoa(foto, editarPaisViewModel.Nome);
                editarPaisViewModel.UrlFoto = urlFoto;
            }

            try
            {
                if (ModelState.IsValid == false)
                    return View(editarPaisViewModel);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "api/Pais/" + id, DataFormat.Json);

                request.AddJsonBody(editarPaisViewModel);

                var response = client.Put<EditarPaisViewModel>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaisController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "api/Pais/" + id, DataFormat.Json);

            var response = client.Get<PaisViewModel>(request);

            return View(response.Data);
        }

        // POST: PaisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, PaisViewModel paisViewModel)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "api/Pais/" + id, DataFormat.Json);

                request.AddJsonBody(paisViewModel);

                var response = client.Delete<PaisViewModel>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string UploadFotoPessoa(IFormFile urlFoto, string NomePais)
        {
            var reader = urlFoto.OpenReadStream();
            var cloudStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=redesocialinfnet;AccountKey=vZcfcl7NqHfcIQJXfUXZRe4U1ePNyU3D4A9mkbj4yoWNFWl/2f78zN9gDFfbg05n1tKvp6QlTTT5jRIVFtTqmg==;EndpointSuffix=core.windows.net");
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("fotos-paises");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(NomePais);
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();

            return destinoDaImagemNaNuvem;
        }
    }
}
