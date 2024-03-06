using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// Controls the check and update of the catalog, and the download of the bundles.
    /// </summary>
    public class AddrCatalogUpdater : MonoBehaviour
    {
        Button _checkUpdateBtn;
        private Button _downloadCatalogBtn;
        private Button _downloadBundleBtn;
        private List<object> _updateKeys = new List<object>();
        private TextMeshProUGUI _updateIndicator;

        private void Awake()
        {
            _checkUpdateBtn = transform.Find("CheckUpdateButton").GetComponent<Button>();
            _checkUpdateBtn.onClick.AddListener(OnCheckUpdate);
            _downloadCatalogBtn = transform.Find("UpdateCatalogButton").GetComponent<Button>();
            _downloadCatalogBtn.onClick.AddListener(OnDownloadCatalog);
            _downloadBundleBtn = transform.Find("UpdateButton").GetComponent<Button>();
            _downloadBundleBtn.onClick.AddListener(OnDownloadBundlePressed);
            _updateIndicator = transform.Find("UpdateIndicatorText").GetComponent<TextMeshProUGUI>();
            _updateIndicator.text = null;
            _downloadBundleBtn.gameObject.SetActive(false);
            _downloadCatalogBtn.gameObject.SetActive(false);
        }

        private void OnCheckUpdate()
        {
            OnCheckUpdateImpl();
        }

        private async void OnCheckUpdateImpl()
        {
            var needUpdate = await AddrHelper.UpdateCatalog();
            if (needUpdate)
            {
                _updateIndicator.text = $"Catalog needs update.";
                _downloadCatalogBtn.gameObject.SetActive(true);
            }
            else
            {
                _updateIndicator.text = "Catalog is up to date";
                _downloadCatalogBtn.gameObject.SetActive(false);
                _downloadBundleBtn.gameObject.SetActive(false);
            }
        }

        private void OnDownloadCatalog()
        {
            OnDownloadCatalogImpl();
        }

        private async void OnDownloadCatalogImpl()
        {
            AddrHelper.OnPreBundleChanged?.Invoke(); // After Catalog update, the code may load new resources from the new bundle. So we need to release the current handles.
            var needUpdate = await AddrHelper.UpdateCatalog(_updateKeys, true);
            if (needUpdate)
            {
                var size = await AddrHelper.GetDownloadSize(_updateKeys);
                _updateIndicator.text = $"Catalog needs update. BundleSize: {size} Bytes.\nKeys: {_updateKeys.Count}";
                _downloadBundleBtn.gameObject.SetActive(true);
            }
            else
            {
                _downloadBundleBtn.gameObject.SetActive(false);
                _updateIndicator.text = "Catalog is up to date";
            }
        }

        private async void OnDownloadBundlePressed()
        {
            AddrHelper.OnPreBundleChanged?.Invoke();
            await AddrHelper.DownAssetImpl(_updateKeys);
        }
    }
}