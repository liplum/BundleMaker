namespace BundleMaker.Core.Bundles;
public interface ILine {
    public string Content {
        get;
    }
}

public class Pair : ILine {
    public string Key {
        get;
        set;
    } = string.Empty;
    public string Value {
        get;
        set;
    } = string.Empty;

    public string Content {
        get => $"{Key}={Value}";
    }
    public override string ToString() => Content;
}

public class Comment : ILine {
    public string Content {
        get;
        set;
    } = string.Empty;
    public override string ToString() => $"#{Content}";
}

public class EmptyLine : ILine {

    public string Content {
        get => string.Empty;
    }
    public override string ToString() => "[Empty]";
}