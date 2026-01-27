using Plugins.Sim.Faciem.Editor.DI;
using R3;
using UnityEditor;
using UnityEngine;

namespace Sim.Faciem.uGUI.Editor
{
    public static class BindablePropertyContextMenu
    {
        private static IBindingManipulationProvider s_bindingManipulationProvider;
        private static SerializedProperty s_lastProperty;
        private static bool s_ignoreSelf = false;
        
        [InitializeOnLoadMethod]
        public static void Init()
        {
            EditorApplication.contextualPropertyMenu += OnContextualPropertyMenu;
            s_bindingManipulationProvider = EditorInjector.Instance.ResolveInstance<IBindingManipulationProvider>();

            s_bindingManipulationProvider.BindableProperty
                .Where(_ => s_lastProperty != null && !s_ignoreSelf)
                .Subscribe(propertyChanged =>
                {
                    var bindingInfo = s_lastProperty.FindPropertyRelative(nameof(IBindableProperty.BindingInfo));
                    bindingInfo.boxedValue = propertyChanged.BindingInfo;
                    bindingInfo.serializedObject.ApplyModifiedProperties();
                });
        }

        private static void OnContextualPropertyMenu(GenericMenu menu, SerializedProperty property)
        {
            
            if (property.name == "_value")
            {
                var parentProperty = property.FindParentProperty();
                if (parentProperty != null && parentProperty.type == typeof(BindableProperty<>).Name
                    && parentProperty.boxedValue is IBindableProperty bindableProperty)
                {
                    menu.AddItem(new GUIContent("Add Binding"), false, () =>
                    {
                        s_lastProperty = parentProperty;
                        s_ignoreSelf = true;
                        s_bindingManipulationProvider.BindableProperty.Value = bindableProperty;
                        s_ignoreSelf = false;
                        var bindingWindow = ScriptableObject.CreateInstance<BindingWindow.BindingWindow>();
                        bindingWindow.ShowAuxWindow();
                    });
                }
            }
        }
    }
}