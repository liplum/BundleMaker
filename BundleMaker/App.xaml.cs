using BundleMaker.Core.Utils;
using Newtonsoft.Json;

namespace BundleMaker;
public partial class App : Application {
    public App() {
        InitializeComponent();
        LoadAssets();
        MainPage = new AppShell();
    }
    private readonly string _settingPath = FileSystem.AppDataDirectory.Sub("app_settings.json");
    private const string BundleFolder = "Bundles";

    private void LoadAssets() {
        CORE.Json = new() {
            Formatting = Formatting.Indented
        };
        LoadSettings();
        LoadBundles();
    }

    private void LoadSettings() {
        if (!File.Exists(_settingPath)) {
            var setting = new Setting();
            using var fileStream = new StreamWriter(_settingPath);
            using var jsonWriter = new JsonTextWriter(fileStream);
            CORE.Json.Serialize(jsonWriter, setting);
            CORE.Setting = setting;
        } else {
            using var fileStream = new StreamReader(_settingPath);
            using var jsonReader = new JsonTextReader(fileStream);
            CORE.Setting = CORE.Json.Deserialize<Setting>(jsonReader) ?? new Setting();
        }
        CORE.Setting.SettingChangedEvent += setting => {
            using var fileStream = new StreamWriter(_settingPath);
            using var jsonWriter = new JsonTextWriter(fileStream);
            CORE.Json.Serialize(jsonWriter, setting);
        };
    }
    private void LoadBundles() {
        var setting = CORE.Setting;
        var name = setting.Language + ".properties";
        var path = BundleFolder.Sub(name);
        var bundle = path.LoadAssetAsString();
        var i18N = new I18N();
        i18N.ReadBundleFrom(bundle);
        CORE.I18N = i18N;
    }
}