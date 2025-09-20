using MiniApp.Data;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public void TestBTree_CorrectLocation()
    {
        var advertisements = new Dictionary<string, List<string>>
        {
            ["Яндекс.Директ"] = ["/ru"],
            ["Ревдинский рабочий"] = ["/ru/svrd/revda", "/ru/svrd/pervik"],
            ["Газета уральских москвичей"] = ["/ru/msk", "/ru/permobl", "/ru/chelobl"],
            ["Крутая реклама"] = ["/ru/svrd"]
        };

        
        var tree = new BTree(advertisements);
        var result = tree.SearchPlatformsByLocation("/ru");
        
        Assert.Single(result);
        Assert.Contains("Яндекс.Директ", result);
        
        result = tree.SearchPlatformsByLocation("/ru/svrd/revda");
        
        Assert.Equal(3, result.Count);
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Ревдинский рабочий", result);
        Assert.Contains("Крутая реклама", result);
        
        result = tree.SearchPlatformsByLocation("/ru/svrd");
        
        Assert.Equal(2, result.Count);
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Крутая реклама", result);
        
        result = tree.SearchPlatformsByLocation("/ru/chelobl");
        
        Assert.Equal(2, result.Count);
        Assert.Contains("Яндекс.Директ", result);
        Assert.Contains("Газета уральских москвичей", result);
        
        
    }
    
    [Fact]
    public void TestBTree_CorrectLocation_notFound()
    {
        var advertisements = new Dictionary<string, List<string>>
        {
            ["Яндекс.Директ"] = ["/ru"],
            ["Ревдинский рабочий"] = ["/ru/svrd/revda", "/ru/svrd/pervik"],
            ["Газета уральских москвичей"] = ["/ru/msk", "/ru/permobl", "/ru/chelobl"],
            ["Крутая реклама"] = ["/ru/svrd"]
        };

        
        var tree = new BTree(advertisements);
        var result = tree.SearchPlatformsByLocation(" ");
        
        Assert.Equal([], result);
    }
}