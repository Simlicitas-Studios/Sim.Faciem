using Sim.Faciem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.Editor
{
    [CustomEditor(typeof(ViewIdAsset))]
    public class ViewIdAssetEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root  = new VisualElement();
        
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            
            return root;
        }
    }
}
