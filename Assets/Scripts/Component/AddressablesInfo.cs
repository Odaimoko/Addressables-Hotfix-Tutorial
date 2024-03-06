using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// Show some basic information about Addressables's locations.
    /// </summary>
    public class AddressablesInfo : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            CatalogCacheDir = $"{Application.persistentDataPath}/com.unity.addressables/";
            _text = GetComponent<TextMeshProUGUI>();
            Addressables.InitializeAsync(true).Completed += handle =>
            {
                var locations = Addressables.ResourceLocators;
                var sb = locations.Aggregate("", (current, locator) => current + $"\n\t{locator.LocatorId}: {locator.GetType().Name}");
                _text.text = sb;
                _text.text = "AddressablesInfo\n" + $"RuntimePath: {Addressables.RuntimePath}\n" +
                             $"Local Catalog: {Addressables.RuntimePath}/catalog.json\n" + // AddressablesImpl.InitializeAsync()
                             $"Remote Catalog: {CatalogCacheDir}\n" + // AddressablesImpl.kCacheDataFolder
                             $"PlayerBuildDataPath: {Addressables.PlayerBuildDataPath}\n" +
                             $"StreamingAssets: {Application.streamingAssetsPath}\n" +
                             $"ResourceLocators:" + sb + "\n";
            };
        }

        public static string CatalogCacheDir;
    }
}