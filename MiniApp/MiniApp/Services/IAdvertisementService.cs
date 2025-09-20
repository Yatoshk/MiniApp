namespace MiniApp;

public interface IAdvertisementService
{
    Task UploadFile(IFormFile file);
    
    Task<List<string>> SearchPlatform(string location);
}