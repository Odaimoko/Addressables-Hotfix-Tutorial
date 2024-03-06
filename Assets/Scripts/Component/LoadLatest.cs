using UnityEngine;
using UnityEngine.UI;

namespace Imk.AddrTutorial
{
    /// <summary>
    /// Reload the scene using the latest catalog.
    /// </summary>
    public class LoadLatest : MonoBehaviour
    {
        private Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(OnLoadLatest);
        }


        private void OnLoadLatest()
        {
            AddrHelper.OnRetriveLatest?.Invoke();
        }
    }
}