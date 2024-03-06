using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// Clear all the asset bundle caches.
    /// </summary>
    public class AddrCacheClear : MonoBehaviour
    {
        Button _clearCacheBtn;
        TextMeshProUGUI _allCacheText;

        private void Awake()
        {
            _clearCacheBtn = transform.Find("ClearBundleCache").GetComponent<Button>();
            _clearCacheBtn.onClick.AddListener(OnClearCache);
            _allCacheText = transform.Find("AllCaches").GetComponent<TextMeshProUGUI>();
            List<string> cachePaths = new();
            Caching.GetAllCachePaths(cachePaths);
            _allCacheText.text = "All Cache Paths: " + string.Join("\n", cachePaths);
        }

        private async void OnClearCache()
        {
            AddrHelper.OnPreBundleChanged?.Invoke();
            await AddrHelper.ClearAllCache();
        }
    }
}