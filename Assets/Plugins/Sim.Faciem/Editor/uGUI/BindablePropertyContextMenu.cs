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
                    EditorUtility.SetDirty(bindingInfo.serializedObject.targetObject);
                    EditorWindow.focusedWindow?.Repaint();
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
                    var manipulationItemName = bindableProperty.BindingInfo.IsDefault
                        ? "Add Binding"
                        : "Edit Binding";
                    
                    menu.AddItem(new GUIContent(manipulationItemName), false, () =>
                    {
                        s_lastProperty = parentProperty;
                        s_ignoreSelf = true;
                        s_bindingManipulationProvider.BindableProperty.Value = bindableProperty;
                        s_bindingManipulationProvider.BindableProperty.ForceNotify();
                        s_ignoreSelf = false;
                        var bindingWindow = EditorWindow.GetWindow<BindingWindow.BindingWindow>();
                        bindingWindow.Show();
                    });

                    if (!bindableProperty.BindingInfo.IsDefault)
                    {
                        menu.AddItem(new GUIContent("Remove Binding"), false, () =>
                        {
                            var bindingInfo = parentProperty.FindPropertyRelative(nameof(IBindableProperty.BindingInfo));
                            bindingInfo.boxedValue = default(SimBindingInfo);
                            bindingInfo.serializedObject.ApplyModifiedProperties();
                            EditorUtility.SetDirty(bindingInfo.serializedObject.targetObject);
                            EditorWindow.focusedWindow?.Repaint();
                        });
                    }
                }
            }
        }
    }
}