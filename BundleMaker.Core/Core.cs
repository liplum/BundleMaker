global using JSON = Newtonsoft.Json.JsonSerializer;
using System.Diagnostics.CodeAnalysis;

namespace BundleMaker.Core;
[SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class CORE {
    [NotNull]
    public static I18N I18N;
    [NotNull]
    public static Setting Setting;
    [NotNull]
    public static JSON Json;
}
