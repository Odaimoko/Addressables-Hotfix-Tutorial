using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// Loading successive images from Addressables.
    /// On bundle change, the current handle will be released, so that the next load will be from the new bundle.
    /// </summary>
    public class AddrLoadImage : MonoBehaviour
    {
        private Button _btn;
        private Image _image;

        int _index = 0;
        [SerializeField] private int indexStart = 1;
        [SerializeField] string imgPathFormat = "Assets/TutorialExamples/OriginalAssets/ori_{0}.psd";
        private TextMeshProUGUI _text;

        private AsyncOperationHandle<Sprite> _curHandle;

        private void Awake()
        {
            Addressables.InitializeAsync();
            _index = indexStart - 1;
            AddrHelper.OnPreBundleChanged += ReleaseCurHandle;
        }

        private void OnDestroy()
        {
            AddrHelper.OnPreBundleChanged -= ReleaseCurHandle;
            ReleaseCurHandle();
        }

        // Start is called before the first frame update
        void Start()
        {
            _btn = transform.Find("Button").GetComponent<Button>();
            _btn.onClick.AddListener(OnClick);
            _image = transform.Find("Image").GetComponent<Image>();
            _text = transform.Find("ImageName").GetComponent<TextMeshProUGUI>();
        }

        private void OnClick()
        {
            LoadNext();
        }

        private async void LoadNext()
        {
            _index++;
            var path = string.Format(imgPathFormat, _index);
            if (!AddrHelper.AddressableResourceExists<Sprite>(path))
            {
                _index = indexStart;
                path = string.Format(imgPathFormat, _index);
                Debug.Log("Resetting index to 1");
            }

            var handle = Addressables.LoadAssetAsync<Sprite>(path);

            var sp = await handle.Task;
            ReleaseCurHandle();
            _curHandle = handle;
            _image.sprite = sp;
            _text.text = Path.GetFileNameWithoutExtension(path);
        }

        private void ReleaseCurHandle()
        {
            if (_curHandle.IsValid())
            {
                Addressables.Release(_curHandle);
            }

            _curHandle = default;
        }
    }
}