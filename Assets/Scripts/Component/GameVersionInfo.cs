using Container;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// A simple component to display the game version, indicating some asset bundle has been updated.
    /// The asset in use will be unloaded immediately after the operation is completed.
    /// </summary>
    public class GameVersionInfo : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            AddrHelper.OnRetriveLatest += LoadGameInfo;
        }

        private void OnDestroy()
        {
            AddrHelper.OnRetriveLatest -= LoadGameInfo;
        }

        private void LoadGameInfo()
        {
            Addressables.LoadAssetAsync<StringListContainer>(AddrConst.AddressCollectionKey).Completed += handle =>
            {
                var addressCollection = handle.Result;
                // ReSharper disable once Unity.NoNullPropagation
                if (addressCollection.TryGetUpdatableAddress("_gamever", out var v))
                {
                    _text.text = $"Game Version: {v}";
                    Debug.Log(_text.text);
                }

                Addressables.Release(handle);
            };
        }
    }
}