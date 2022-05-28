using BundleMaker.Core;

namespace BundleMaker;
public class EditPage : ContentPage {
    public EditPage() {
        Content = new Button {
            Text = CORE.I18N["EditPage.Button.TestButton"]
        };
    }
}

public class ProjectPane : View {

}

public class LineEditArea : View {

}