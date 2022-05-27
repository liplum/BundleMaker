namespace BundleMaker;
public partial class ProjectPage : ContentPage {
    public const string Domain = "Project";

    public ProjectPage() {
        InitializeComponent();
        BuildUI();
    }
    public Grid Outer;
    public void BuildUI() {
        /* for (var i = 0; i < 5; i++) {
             var cell = new ProjectCell {
                 Page = this,
                 Project = new()
             };
             cell.BuildUI();
             Body.Add(cell);
         }*/
        var forAdd = new ProjectCell {
            Page = this
        };
        forAdd.BuildUI();
        Body.Add(forAdd);
    }

    public void NewProjectInto(ProjectCell cell) {
        cell.Project = new();
    }

    public void RemoveProject(ProjectCell cell) {
        Body.Remove(cell);
    }
}

public class ProjectCell : ContentView {
    public ProjectPage Page {
        get;
        init;
    }
    public Project Project {
        get;
        set;
    }
    public static string Bundle(string key) {
        return CORE.I18N[$"{ProjectPage.Domain}.Cell.{key}"];
    }

    public void BuildUI() {
        Content = null;
        if (Project is null) {
            // Fake project to hint user to add a new one
            var addButton = new Button {
                Text = Bundle("Add"),
            };
            addButton.Clicked += (_, _) => { Page.NewProjectInto(this); };
            Content = addButton;
        } else {
            var deleteButton = new SwipeItem {
                Text = Bundle("Remove")
            };
            deleteButton.Invoked += (_, _) => { Page.RemoveProject(this); };
            Content = new SwipeView {
                LeftItems = new(new[] {
                    deleteButton
                }),
                Content = new Label {
                    Text = "Test Label"
                }
            };
        }
    }
}