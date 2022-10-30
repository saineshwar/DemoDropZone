using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace DemoDropZone.Controllers
{
    public class DemoUploadController : Controller
    {
        [HttpGet]
        public IActionResult UploadFiles()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile filedata)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Any())
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(file.FileName);
                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid().ToString("N"));
                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);
                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);
                        await using var target = new MemoryStream();
                        await file.CopyToAsync(target);
                        var physicalPath = $"{new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles")).Root}{$@"{fileName}"}";
                        string filePath = $"/UploadedFiles/{fileName}";
                        await using FileStream fs = System.IO.File.Create(physicalPath);
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                }

                return Json(new { status = true, Message = "Files Uploaded Successfully!" });
            }

            return View();
        }

    }
}
