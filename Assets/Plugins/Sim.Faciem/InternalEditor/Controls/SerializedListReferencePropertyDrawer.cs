using Sim.Faciem.ListBinding;
using UnityEditor;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.InternalEditor.Controls
{
    [CustomPropertyDrawer(typeof(SerializedListReference))]
    public class SerializedListReferencePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new Label("Item Source Binding");
        }
    }
}