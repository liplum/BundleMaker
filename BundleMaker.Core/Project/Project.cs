using BundleMaker.Core.Basic;

namespace BundleMaker.Core.Project;
public class ProjectMeta : Bindable {

}

public class Project : Bindable {
    private string _name = string.Empty;
    public string Name {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    public ProjectMeta Meta {
        get;
        set;
    }
}

public class ProjectList {

}