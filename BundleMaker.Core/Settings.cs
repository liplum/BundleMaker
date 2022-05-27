namespace BundleMaker;
public class Setting {
    public delegate void SettingChanged(Setting setting);

    public event SettingChanged SettingChangedEvent;
    protected virtual void OnSettingChanged() {
        SettingChangedEvent?.Invoke(this);
    }
    private string _language = "en";
    public string Language {
        get => _language;
        set {
            _language = value;
            OnSettingChanged();
        }
    }
}