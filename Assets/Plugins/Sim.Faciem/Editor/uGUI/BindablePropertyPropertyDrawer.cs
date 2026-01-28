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

            var bindingIcon = new VisualElement
            {
                style =
                {
                    backgroundImage = new StyleBackground(EditorGUIUtility.IconContent("Binding").image as Texture2D),
                    position = Position.Absolute,
                    left = -12,
                    top = 2,
                    width = 16,
                    height = 16,
                    unityBackgroundImageTintColor = new  Color(87/255f, 133/255f, 217/255f, 1),
                    display = DisplayStyle.None
                }
            };
            root.Add(bindingIcon);
            
            root.schedule.Execute(() => bindingIcon.style.display = property.boxedValue is IBindableProperty
                {
                    BindingInfo: { IsDefault: true }
                }
                    ? DisplayStyle.None
                    : DisplayStyle.Flex)
                .Every(200);
                
            root.Add(valueField);
            
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