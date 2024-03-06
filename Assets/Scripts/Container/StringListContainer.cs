using System.Collections.Generic;
using MathieuLeBer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Container
{
    [CreateAssetMenu(fileName = "String List", menuName = "AddrTutorial/String List", order = 0)]
    public class StringListContainer : ScriptableObject
    {
        [SerializeField] List<string> _list = new List<string>();
        public List<string> List => _list;

        [SerializeField] String2StringDictionary updatableAddresses = new();


        public bool TryGetUpdatableAddress(string key, out string address)
        {
            return updatableAddresses.TryGetValue(key, out address);
        }

        public IEnumerable<string> AllKeys => updatableAddresses.Keys;
    }
}