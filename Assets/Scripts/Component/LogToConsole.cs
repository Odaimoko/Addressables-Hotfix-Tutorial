using UnityEngine;
using UnityEngine.UI;

namespace Imk.AddrTutorial
{
    public class LogToConsole : MonoBehaviour
    {
        Button _btn;

        private void Awake()
        {
            _btn = GetComponent<Button>();
            
            _btn.onClick.AddListener(OnLogToConsole);
        }

        private void OnLogToConsole()
        {
            Debug.Log("Remote Bundles in current catalog:");
            AddrHelper.LogRemoteBundlesInCurrentCatalog();
        }
    }
}