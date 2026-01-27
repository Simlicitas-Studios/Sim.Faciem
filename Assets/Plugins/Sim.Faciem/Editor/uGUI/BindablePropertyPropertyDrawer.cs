using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sim.Faciem.uGUI.Editor
{
    [CustomPropertyDrawer(typeof(BindableProperty<>))]
    public class BindablePropertyPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var valueProperty = property.FindPropertyRelative("_value");
            
            var root = new VisualElement();

            var valueField = new PropertyField(valueProperty)
            {
                label = property.displayName
            };
                
            root.Add(valueField);

            root.RegisterCallback<ContextClickEvent>(evt =>
            {
                var menu = new GenericMenu();
                menu.AddItem(
                    new GUIContent("Add Binding…"),
                    false,
                    () => AddBinding(property)
                );

                menu.AddItem(
                    new GUIContent("Clear Binding"),
                    false,
                    () => ClearBinding(property)
                );

                menu.ShowAsContext();

                evt.StopPropagation();
            });
            
            return root;
        }
        
        private void AddBinding(SerializedProperty property)
        {
            Debug.Log($"Add binding for {property.propertyPath}");
            // Open your binding popup here
        }

        private void ClearBinding(SerializedProperty property)
        {
            Debug.Log($"Clear binding for {property.propertyPath}");
            // Remove serialized binding info here
        }
    }
}