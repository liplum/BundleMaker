using BundleMaker.Core;

namespace BundleMaker;
public class I18NExtension : IMarkupExtension<string> {
    public string Key { get; set; } = "";
    string IMarkupExtension<string>.ProvideValue(IServiceProvider serviceProvider) {
        return CORE.I18N[Key];
    }
    public object ProvideValue(IServiceProvider serviceProvider) {
        return ((IMarkupExtension<string>)this).ProvideValue(serviceProvider);
    }
}