using UnityEditor;

namespace Container.Editor
{
    [CustomPropertyDrawer(typeof(String2StringDictionary))]
    public class String2StringDictionaryEditor : SerializableDictionaryPropertyDrawer
    {
    }
}