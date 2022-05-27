using System.Collections.Immutable;
using BundleMaker.Core.Bundles;
using BundleMaker.Core.Utils;

namespace BundleMaker.Core;
public class I18N {
    public ImmutableDictionary<string, string> Language2Name = new Dictionary<string, string> {
        { "en", "English" }
    }.ToImmutableDictionary();
    private Bundle? Bundle {
        get;
        set;
    }
    public async Task ReadBundleFromAsync(string text) {
        var bundleContent = new Dictionary<string, string>();
        await BundleReader.ReadBundleIntoAsync(text, bundleContent);
        Bundle = new() { Pairs = bundleContent };
    }
    public async Task AppendBundleFromAsync(string text) {
        var bundleContent = new Dictionary<string, string>();
        await BundleReader.ReadBundleIntoAsync(text, bundleContent);
        Bundle = Bundle ??= new();
        Bundle.Pairs.AddAll(bundleContent);
    }
    public void ReadBundleFrom(string text) {
        var bundleContent = new Dictionary<string, string>();
        BundleReader.ReadBundleInto(text, bundleContent);
        Bundle = new() { Pairs = bundleContent };
    }
    public void AppendBundleFrom(string text) {
        var bundleContent = new Dictionary<string, string>();
        BundleReader.ReadBundleInto(text, bundleContent);
        Bundle = Bundle ??= new();
        Bundle.Pairs.AddAll(bundleContent);
    }
    public string Get(string key) => Bundle!.Get(key);
    public string this[string key] {
        get => Bundle![key];
    }
    public bool Has(string key) => Bundle!.Has(key);
    public string Format(string key, params object[] args) => Bundle!.Format(key, args);
}

public static class I18NHelper {
    public static string Bundle(this string key) => CORE.I18N[key];
    public static string Bundle(this string key, params object[] args) => CORE.I18N.Format(key, args);
}