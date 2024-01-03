using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurant_app_API.Helper;

namespace restaurant_app_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPut("upload")]
        public async Task<IActionResult> Upload(IFormFile file, string productId)
        {
            APIResponse response = new APIResponse(false, "", null);
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest();

                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images", "products", productId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(path, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                response.Data = "http://localhost:5072/images/products/" + productId + "/" + fileName;
                response.Message = "Upload successful";
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }

        }

        [HttpGet("get/{productId}")]
        public IActionResult Get(string productId)
        {
            APIResponse response = new APIResponse(false, "", null);
            try
            {
                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images", "products", productId);


                if (!Directory.Exists(path))
                {
                    response.Data = null;
                    response.Message = "Get successful";
                    response.Success = true;
                    return Ok(response);
                }

                response.Data = "http://localhost:5072/images/products/" + productId + "/" + path;
                response.Message = "Get successful";
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }

        }

        [HttpDelete("delete")]
        public IActionResult Delete(string productId, string fileName)
        {
            APIResponse response = new APIResponse(false, "", null);
            try
            {
                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images", "products", productId, fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                response.Message = "Delete successful";
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }

        }

        [HttpDelete("deleteAll")]
        public IActionResult DeleteAll(string productId)
        {
            APIResponse response = new APIResponse(false, "", null);
            try
            {
                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images", "products", productId);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                response.Message = "Delete successful";
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }

        }

        //multiple upload
        [HttpPut("uploadMultiple")]
        public async Task<IActionResult> UploadMultiple(IFormFile[] files, string productId)
        {
            APIResponse response = new APIResponse(false, "", null);
            try
            {
                if (files == null || files.Length == 0)
                    return BadRequest();

                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images", "products", productId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                response.Data = "http://localhost:5072/images/products/" + productId;
                response.Message = "Upload successful";
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }

        }

        //download file
        [HttpGet("download")]
        public async Task<IActionResult> Download(string productId, string fileName)
        {
            APIResponse response = new APIResponse(false, "", null);
            try
            {
                string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "images", "products", productId, fileName);
                if (!System.IO.File.Exists(path))
                {
                    response.Data = null;
                    response.Message = "File not found";
                    response.Success = false;
                    return BadRequest(response);
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;
                response.Data = memory;
                response.Message = "Download successful";
                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                return BadRequest(response);
            }

        }
    }
}
