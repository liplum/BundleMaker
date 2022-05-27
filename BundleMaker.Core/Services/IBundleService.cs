using BundleMaker.Core.Bundles;

namespace BundleMaker.Core.Services;
public interface IBundleService {
    public void ChangeContent(BundleLocator locator, string newContent);
    public void MoveLine(LineLocator oldLine, LineLocator newLine);
    public void RenameKey(BundleLocator oldKey, BundleLocator newKey);
    public void AddKey(BundleLocator newKey, LineLocator line);
    public void RemoveKey(BundleLocator removedKey, LineLocator line);
}