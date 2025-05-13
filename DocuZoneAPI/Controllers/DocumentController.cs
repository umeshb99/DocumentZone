using DocuZoneAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace DocuZoneAPI.Controllers
{
    public class DocumentCotroller : Controller
    {
        private readonly IBlobService blobService;

        public IActionResult Index()
        {
            return View();
        }

        public DocumentCotroller(IBlobService blobService)
        {
            this.blobService = blobService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var url = await blobService.UploadAsync(file);
            return Ok(new { FileUrl = url });
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return BadRequest("Please Enter file name");

            var stream = await blobService.DownloadAsync(filename);
            return File(stream, "application/octet-stream", filename);
        }

        [HttpGet("List")]

        public async Task<IActionResult> List()
        {
            var files = await blobService.ListFilesAsync();
            return Ok(files);
        }
    }
}
