namespace MiniApp.Data;

public class Advertisement(string location, List<string> platforms)
{
    public string Location { get; } = location;
    public List<string> Platforms { get; } = platforms ?? new List<string>();
}