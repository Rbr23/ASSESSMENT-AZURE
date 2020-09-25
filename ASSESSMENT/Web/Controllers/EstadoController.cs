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

namespace Web.Controllers
{
    public class EstadoController : Controller
    {
        private readonly string _UriAPI = "https://localhost:44318/";

        // GET: PaisController
        public ActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "api/Estados");

            var response = client.Get<List<EstadoViewModel>>(request);

            return View(response.Data);
        }

        // GET: PaisController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(_UriAPI + "api/Estados/" + id, DataFormat.Json);

            var response = client.Get<EstadoViewModel>(request);

            return View(response.Data);
        }

        // GET: PaisController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaisController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstadoViewModel estadoViewModel, IFormFile foto)
        {
            var urlFoto = UploadFotoPessoa(foto, estadoViewModel.Nome);
            estadoViewModel.UrlFoto = urlFoto;

            try
            {
                if (ModelState.IsValid == false)
                    return View(estadoViewModel);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "api/Estados", DataFormat.Json);

                request.AddJsonBody(estadoViewModel);

                var response = await client.PostAsync<EstadoViewModel>(request);

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
            var request = new RestRequest(_UriAPI + "api/Estados/" + id, DataFormat.Json);

            var response = client.Get<EstadoViewModel>(request);

            return View(response.Data);
        }

        // POST: PaisController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, EstadoViewModel estadoViewModel, IFormFile foto)
        {
            if (foto != null)
            {
                var urlFoto = UploadFotoPessoa(foto, estadoViewModel.Nome);
                estadoViewModel.UrlFoto = urlFoto;
            }

            try
            {
                if (ModelState.IsValid == false)
                    return View(estadoViewModel);

                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "api/Estados/" + id, DataFormat.Json);

                request.AddJsonBody(estadoViewModel);

                var response = client.Put<EstadoViewModel>(request);

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
            var request = new RestRequest(_UriAPI + "api/Estados/" + id, DataFormat.Json);

            var response = client.Get<EstadoViewModel>(request);

            return View(response.Data);
        }

        // POST: PaisController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, EstadoViewModel estadoViewModel)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(_UriAPI + "api/Estados/" + id, DataFormat.Json);

                request.AddJsonBody(estadoViewModel);

                var response = client.Delete<EstadoViewModel>(request);

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
            var container = blobClient.GetContainerReference("fotos-estados");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(NomePais);
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();

            return destinoDaImagemNaNuvem;
        }
    }
}
