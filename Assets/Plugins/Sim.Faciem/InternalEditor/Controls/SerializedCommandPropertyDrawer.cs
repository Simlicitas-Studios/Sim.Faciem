using Sim.Faciem.CommandBinding;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Plugins.Sim.Faciem.InternalEditor.Controls
{
    [CustomPropertyDrawer(typeof(SerializedCommand))]
    public class SerializedCommandPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new Label("Command Binding");
        }
    }
}