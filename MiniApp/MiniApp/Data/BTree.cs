namespace MiniApp.Data;

public class BTreeNode(List<BTreeNode> children, Advertisement advertisement)
{
    public readonly List<BTreeNode> Children = children;
    public readonly Advertisement Advertisement = advertisement;
}

public class BTree
{
    private readonly BTreeNode _root;
    private readonly int _degree;

    public BTree(Dictionary<string, List<string>> advertisements, int degree = 3)
    {
        _degree = degree;
        _root = new BTreeNode(new List<BTreeNode>(), new Advertisement("", new List<string>()));
        
        foreach (var advertisement in advertisements)
        {
            var platformName = advertisement.Key;
            foreach (var location in advertisement.Value)
            {
                Insert(location, platformName);
            }
        }
    }

    private void Insert(string location, string platformName)
    {
        var pathParts = GetLocationParts(location);
        InsertRecursive(_root, pathParts, 0, platformName, location);
    }

    private void InsertRecursive(BTreeNode node, List<string> pathParts, int depth, string platformName, string fullLocation)
    {
        if (depth >= pathParts.Count)
            return;

        var currentPart = pathParts[depth];
        var existingChild = node.Children.FirstOrDefault(c => 
            GetLocationParts(c.Advertisement.Location).LastOrDefault() == currentPart);

        if (existingChild != null)
        {
            if (depth == pathParts.Count - 1 && existingChild.Advertisement.Location == fullLocation)
            {
                if (!existingChild.Advertisement.Platforms.Contains(platformName))
                {
                    existingChild.Advertisement.Platforms.Add(platformName);
                }
            }

            if (depth + 1 < pathParts.Count)
            {
                InsertRecursive(existingChild, pathParts, depth + 1, platformName, fullLocation);
            }
        }
        else
        {
            var fullPath = depth == 0 ? $"/{currentPart}" : $"{node.Advertisement.Location}/{currentPart}";
            var platforms = depth == pathParts.Count - 1 ? 
                new List<string> { platformName } : 
                new List<string>();
            
            var newNode = new BTreeNode(new List<BTreeNode>(), new Advertisement(fullPath, platforms));
            
            if (depth + 1 < pathParts.Count)
            {
                InsertRecursive(newNode, pathParts, depth + 1, platformName, fullLocation);
            }

            AddChild(node, newNode);

            if (node.Children.Count > _degree)
            {
                SplitNode(node);
            }
        }
    }

    private void AddChild(BTreeNode parent, BTreeNode child)
    {
        var index = parent.Children.FindIndex(c => 
            string.Compare(c.Advertisement.Location, child.Advertisement.Location, StringComparison.Ordinal) > 0);
        
        if (index == -1)
        {
            parent.Children.Add(child);
        }
        else
        {
            parent.Children.Insert(index, child);
        }
    }

    public List<string> SearchPlatformsByLocation(string location)
    {
        var result = new HashSet<string>();
        var pathParts = GetLocationParts(location);
        SearchRecursive(_root, pathParts, 0, result, location);
        return result.ToList();
    }

    private void SearchRecursive(BTreeNode node, List<string> pathParts, int depth, HashSet<string> result, string targetLocation)
    {
        if (node.Advertisement.Location == targetLocation)
        {
            foreach (var platform in node.Advertisement.Platforms)
            {
                result.Add(platform);
            }
        }

        if (depth >= pathParts.Count) return;

        var currentPart = pathParts[depth];
        var foundChild = node.Children.FirstOrDefault(c => 
            GetLocationParts(c.Advertisement.Location).LastOrDefault() == currentPart);

        if (foundChild != null)
        {
            SearchRecursive(foundChild, pathParts, depth + 1, result, targetLocation);
        }

        if (targetLocation.StartsWith(node.Advertisement.Location + "/") || 
            node.Advertisement.Location == targetLocation)
        {
            foreach (var platform in node.Advertisement.Platforms)
            {
                result.Add(platform);
            }
        }
    }

    private List<string> GetLocationParts(string location)
    {
        return location.Split('/')
            .Where(part => !string.IsNullOrEmpty(part))
            .ToList();
    }

    private void SplitNode(BTreeNode node)
    {
        if (node.Children.Count <= _degree) return;
    }
}