namespace BundleMaker.Core.Bundles;
public class Bundle {
    public IDictionary<string, string> Pairs {
        get;
        init;
    } = new Dictionary<string, string>();
    public string Get(string key) =>
        Pairs.TryGetValue(key, out var value) ? value : $"???{key}???";
    public string this[string key] {
        get => Get(key);
    }
}

public class BundleBuilder {
    public List<ILine> Lines {
        get;
        init;
    }
}

public struct BundleLocator {
    public string BundleName;
    public string Key;
}

public struct LineLocator {
    public string BundleName;
    public int LineNumber;
}