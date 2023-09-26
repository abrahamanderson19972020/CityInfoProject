using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfoAPI.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        public readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
             _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;  
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFiles(string fileId)
        {
            var filePath = @"C:\Users\Bruker\Downloads\test.pdf";
            if(!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            if(!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));
        }
    }
}
