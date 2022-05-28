using System.Collections.ObjectModel;
using BundleMaker.Core.Project;

namespace BundleMaker;
public partial class ProjectPage : ContentPage {
    public const string Domain = "Project";
    public ObservableCollection<Project> AllProjects {
        get;
    } = new();
    public ProjectPage() {
        InitializeComponent();
        BuildUi();
        BindingContext = this;
    }
    public void BuildUi() {
        NewProject();
        NewProjectButton.Clicked += (_, _) => { NewProject(); };
    }

    public void NewProject() {
        AllProjects.Add(new());
    }

    private void RemoveCurrentProject(object sender, EventArgs e) {

    }
}