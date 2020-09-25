using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Web.Models.Amigo;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Web.Controllers
{
    public class AmigoController : Controller
    {
        private readonly string link = "https://localhost:44304/";

        // GET: AutorController
        public IActionResult Index()
        {
            var client = new RestClient();
            var request = new RestRequest(link + "api/Amigos");

            

            var response = client.Get<List<ListarAmigoViewModel>>(request);

            return View(response.Data);
        }

        // GET: AutorController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(link + "api/Amigos/" + id, DataFormat.Json);          

            var response = client.Get<AmigoViewModel>(request);

            var requestAmigos = new RestRequest(link + "api/Amigos/" + id + "/amigos", DataFormat.Json);
            response.Data.Amigos = client.Get<List<AmigoResponseViewModel>>(requestAmigos).Data;

            var requestTodosAmigos = new RestRequest(link + "api/Amigos", DataFormat.Json);
            response.Data.TodosAmigos = client.Get<List<AmigoResponseViewModel>>(requestTodosAmigos).Data;

            var amigo = response.Data.TodosAmigos.First(x => x.Id == id);
            response.Data.TodosAmigos.Remove(amigo);

            return View(response.Data);
        }

        public ActionResult DetailsPost(Guid id, Guid idAmigo)
        {
            var client = new RestClient();
            var request = new RestRequest(link + "api/Amigos/" + id + "/Amigos");

            request.AddJsonBody(idAmigo);

            var response = client.Post(request);

            return RedirectToAction(nameof(Details), new { id });
        }

        public ActionResult DetailsDelete(Guid id, Guid idAmigo)
        {
            var client = new RestClient();
            var request = new RestRequest(link + "api/Amigos/" + id + "/Amigos");

            request.AddJsonBody(idAmigo);

            var response = client.Delete(request);

            return RedirectToAction(nameof(Details), new { id });
        }



        // GET: AutorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AutorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAmigoViewModel formAutorViewModel)
        {
            formAutorViewModel.UrlFoto = "https://assessment123.blob.core.windows.net/azure/pngwing.com.png";
            try
            {
                if (ModelState.IsValid == false)
                    return View(formAutorViewModel);

                var client = new RestClient();
                var request = new RestRequest(link + "api/Amigos", DataFormat.Json);
               
                request.AddJsonBody(formAutorViewModel);

                var response = client.Post<CreateAmigoViewModel>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro, por favor tente mais tarde.");
                return View(formAutorViewModel);
            }
        }

        // GET: AutorController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(link + "api/Amigos/" + id, DataFormat.Json);
           
            var response = client.Get<EditarAmigoViewModel>(request);

            return View(response.Data);
        }

        // POST: AutorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, EditarAmigoViewModel editaramigoViewModel, IFormFile foto)
        {
            var urlFoto = UploadFotoPessoa(foto, editaramigoViewModel.Id);
            editaramigoViewModel.UrlFoto = urlFoto;

            try
            {
                if (ModelState.IsValid == false)
                    return View(editaramigoViewModel);

                var client = new RestClient();
                var request = new RestRequest(link + "api/Amigos/" + id, DataFormat.Json);
               
                request.AddJsonBody(editaramigoViewModel);

                var response = client.Put<EditarAmigoViewModel>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AutorController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var client = new RestClient();
            var request = new RestRequest(link + "api/Amigos/" + id, DataFormat.Json);
           
            var response = client.Get<AmigoViewModel>(request);

            return View(response.Data);
        }

        // POST: AutorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, AmigoViewModel autorViewModel)
        {
            try
            {
                var client = new RestClient();
                var request = new RestRequest(link + "api/Amigos/" + id, DataFormat.Json);
               
                request.AddJsonBody(autorViewModel);

                var response = client.Delete<CreateAmigoViewModel>(request);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private string UploadFotoPessoa(IFormFile urlFoto, Guid id)
        {
            

            var reader = urlFoto.OpenReadStream();
            var cloudStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=assessment123;AccountKey=k8CqZDNIW0SWQPEUzaHfNZUX4QP3n/qBK3hr61i4LG6rslvsDkvK6IjqjjNUYWygvwLt69CA1Hx6oVLzoDzDBg==;EndpointSuffix=core.windows.net");
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("fotos-amigos");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(id.ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();

            return destinoDaImagemNaNuvem;
        }
    }
}
