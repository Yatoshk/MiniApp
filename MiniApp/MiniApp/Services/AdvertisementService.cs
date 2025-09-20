using MiniApp.Data;

namespace MiniApp;

public class AdvertisementService : IAdvertisementService
{
    private readonly Dictionary<string, List<string>> _advertisements = new Dictionary<string, List<string>>();
    private BTree? _tree;

    public async Task UploadFile(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        while (await reader.ReadLineAsync() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(':', 2);
            if (parts.Length != 2)
                continue; 

            var platformName = parts[0].Trim();
            var locationPaths = parts[1].Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x) && x.Contains('/'))
                .ToList();

            if (locationPaths.Count == 0)
                continue;

            _advertisements.Add(platformName, locationPaths);
        }
        
        _tree = new BTree(_advertisements);
        _advertisements.Clear();
    }

    public Task<List<string>> SearchPlatform(string location)
    {
        if(_tree == null)
            throw new Exception("Upload file has not been initialized");
        
        return Task.FromResult(_tree.SearchPlatformsByLocation(location));
    }
}