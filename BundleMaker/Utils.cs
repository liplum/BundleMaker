namespace BundleMaker; 
public static class Utils {
    public static async Task<string> LoadAssetAsStringAsync(this string path) {
        await using var stream = await FileSystem.OpenAppPackageFileAsync(path);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
    public static string LoadAssetAsString(this string path) {
        using var stream = FileSystem.OpenAppPackageFileAsync(path).Result;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}