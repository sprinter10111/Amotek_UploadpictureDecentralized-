using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.IO;
using Ipfs;
using Ipfs.Http;



namespace img_save.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ImageController : Controller
    {        
        [HttpPost("uploadImage")]        
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file provided.");
            }    
            

            var ipfs = new IpfsClient();
            using (var stream = imageFile.OpenReadStream())
            {
                var result = await ipfs.FileSystem.AddAsync(stream);
                return StatusCode(201, result.Id.Hash.ToString());
            }
        }
        [HttpPost("DownloadImage")]
        public async Task<IActionResult> DownloadImage(string ipfsHash)
        {
            string savePath = @"C:\Users\siema\Desktop\amotek_technis_oef\random testje\ConsoleApp1\ConsoleApp1\image.jpg";
            var ipfs = new IpfsClient("http://127.0.0.1:8080");
            using (var stream = await ipfs.FileSystem.ReadFileAsync(ipfsHash))
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    await System.IO.File.WriteAllBytesAsync(savePath, fileBytes);
                }
            }
            return Ok();
        }





    }
}
