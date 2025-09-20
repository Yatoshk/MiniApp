using Microsoft.AspNetCore.Mvc;

namespace MiniApp;

[ApiController]
[Route("api/[controller]")]
public class AdvertisementController(IAdvertisementService advertisementService) : Controller 
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadAdvertisementFile(IFormFile file)
    {
        try
        {
            if (file.Length == 0)
                return BadRequest("No file uploaded");

            if (Path.GetExtension(file.FileName).ToLower() != ".txt")
                return BadRequest("Only .txt files are supported");
            
            await advertisementService.UploadFile(file);
            
            return Ok("File uploaded successfully");
        }
        catch (Exception _)
        {
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAdvertisingPlatforms([FromQuery] string location)
    {
        try
        {
            if (string.IsNullOrEmpty(location))
                return BadRequest("Location parameter is required");

            var platforms = await advertisementService.SearchPlatform(location);
            return Ok(new { Location = location, Platforms = platforms });
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}