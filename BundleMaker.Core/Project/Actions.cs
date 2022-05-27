using BundleMaker.Core.Bundles;
using BundleMaker.Core.Services;

namespace BundleMaker.Core.Project;
public class Action {
    public bool CanUndo {
        get;
        protected set;
    } = false;
    public virtual void Do(IBundleService service) { }
    public virtual void Undo(IBundleService service) { }
}

public class ChangeAction : Action {
    public ChangeAction() {
        CanUndo = true;
    }
    public BundleLocator Locator { get; set; }
    public string Old { get; set; } = string.Empty;
    public string New { get; set; } = string.Empty;
    public override void Do(IBundleService service) {
        service.ChangeContent(Locator, New);
    }
    public override void Undo(IBundleService service) {
        service.ChangeContent(Locator, Old);
    }
}

public class MoveAction : Action {
    public MoveAction() {
        CanUndo = true;
    }
    public LineLocator From { get; set; }
    public LineLocator To { get; set; }
    public override void Do(IBundleService service) {
        service.MoveLine(From, To);
    }
    public override void Undo(IBundleService service) {
        service.MoveLine(To, From);
    }
}

public class RenameAction : Action {
    public RenameAction() {
        CanUndo = true;
    }
    public BundleLocator Old { get; set; }
    public BundleLocator New { get; set; }
    public override void Do(IBundleService service) {
        service.RenameKey(Old, New);
    }
    public override void Undo(IBundleService service) {
        service.RenameKey(New, Old);
    }
}

public class AddAction : Action {
    public AddAction() {
        CanUndo = true;
    }
    public BundleLocator Key { get; set; }
    public LineLocator InsertedLine { get; set; }
    public override void Do(IBundleService service) {
        service.AddKey(Key, InsertedLine);
    }
    public override void Undo(IBundleService service) {
        service.RemoveKey(Key, InsertedLine);
    }
}

public class RemoveAction : Action {
    public RemoveAction() {
        CanUndo = true;
    }
    public BundleLocator Key { get; set; }
    public LineLocator RemovedLine { get; set; }
    public override void Do(IBundleService service) {
        service.RemoveKey(Key, RemovedLine);
    }
    public override void Undo(IBundleService service) {
        service.AddKey(Key, RemovedLine);
    }
}