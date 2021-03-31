using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LineCounter.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        [HttpGet]
        public IActionResult IndexAsync()
        {
            return View();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload()
        {
            var endpoint = new Uri(Configuration["Blob:serviceUri"]);
            var client = new BlobServiceClient(endpoint, new DefaultAzureCredential());
            var containerClient = client.GetBlobContainerClient("images");

            foreach (var file in HttpContext.Request.Form.Files)
            {
                var stream = file.OpenReadStream();
                await containerClient.CreateIfNotExistsAsync();
                await containerClient.UploadBlobAsync(file.FileName, stream);
            }

            return Ok();
        }
    }
}
