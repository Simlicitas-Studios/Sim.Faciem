using UnityEditor;
using UnityEngine.UIElements;

namespace Sim.Animatio.Editor.Controls
{
    [CustomPropertyDrawer(typeof(TimelineItemData))]
    public class TimelineItemListPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new Label("Item List");
        }
    }
}