using Sim.Faciem;
using UnityEditor;
using UnityEngine;

namespace Plugins.Sim.Faciem.Editor
{
    public static class ViewIdAssetFactory
    {
        
        [MenuItem("Assets/Create/Faciem/ViewId", false, 10)]
        public static void CreateViewIdAsset()
        {
            var viewIdAsset = ScriptableObject.CreateInstance<ViewIdAsset>();
            viewIdAsset.name = "ViewId";
            
            var currentPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            AssetDatabase.CreateAsset(viewIdAsset, currentPath + "/" +viewIdAsset.name + ".asset");
            
            var entry = AddressableHelper.CreateAssetEntry(viewIdAsset);
            entry.SetLabel(FaciemAddressables.ViewId, true, true);
        }
    }
}