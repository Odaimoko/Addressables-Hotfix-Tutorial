using Container;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// A component to display the seasonal event, indicating some asset bundle has been updated.
    /// The asset in use will be unloaded immediately after the operation is completed.
    /// </summary>
    public class SeasonalEvent : MonoBehaviour
    {
        static StringListContainer _addressCollection = null;

        TextMeshProUGUI _eventText = null;

        private void Awake()
        {
            Addressables.InitializeAsync();
            AddrHelper.OnRetriveLatest += UpdateSeasonalEvent;

            _eventText = transform.Find("EventsText").GetComponent<TextMeshProUGUI>();
        }

        private void OnDestroy()
        {
            AddrHelper.OnRetriveLatest -= UpdateSeasonalEvent;
        }

        private void UpdateSeasonalEvent()
        {
            Addressables.LoadAssetAsync<StringListContainer>(AddrConst.AddressCollectionKey).Completed += handle =>
            {
                _addressCollection = handle.Result; // DO not release the handle
                LoadEvents();
                foreach (var key in _addressCollection.AllKeys)
                {
                    if (_addressCollection.TryGetUpdatableAddress(key, out var address))
                    {
                        Debug.Log($"{key}: {address}");
                    }
                }

                Addressables.Release(handle);
            };
        }

        private async void LoadEvents()
        {
            if (!_addressCollection.TryGetUpdatableAddress(AddrConst._SeasonaleventKey, out var seasonalEventKey))
                return;
            var handle = Addressables.LoadAssetAsync<StringListContainer>(seasonalEventKey);
            var container = await handle.Task;
            if (container != null)
            {
                _eventText.text = string.Join("\n", container.List);
            }

            Addressables.Release(handle);
        }
    }
}