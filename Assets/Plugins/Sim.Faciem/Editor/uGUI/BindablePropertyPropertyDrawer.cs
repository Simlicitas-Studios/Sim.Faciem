using System.Collections.Generic;
using Plugins.Sim.Faciem.Shared;
using Sim.Faciem.Controls;
using Sim.Faciem.uGUI.Editor.Internal;
using Unity.Properties;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sim.Faciem.uGUI.Editor
{
    [CustomPropertyDrawer(typeof(BindableProperty<>))]
    public class BindablePropertyPropertyDrawer : PropertyDrawer
    {
        private readonly Color _bindingAccentColor = new(87/255f, 133/255f, 217/255f, 1);
        private static DebugPropertyVisitor s_DebugPropertyVisitor = new();
        
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
                    unityBackgroundImageTintColor = _bindingAccentColor,
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
            
            if (Application.isPlaying)
            {
                var path = new PropertyPath(property.propertyPath);
                s_DebugPropertyVisitor.Path = path;
                var target = property.serializedObject.targetObject;
                if (PropertyContainer.TryAccept(s_DebugPropertyVisitor, ref target))
                {
                    if (s_DebugPropertyVisitor.Value is IRuntimeBindableProperty { RuntimeBindingInfo: not null } runtimeBindableProperty)
                    {
                        valueField.SetEnabled(false);
                        
                        var runtimeInfoContainer = new VisualElement
                        {
                            style =
                            {
                                fontSize = 13,
                                marginTop = 4,
                                paddingBottom = 8,
                                paddingTop = 8,
                                paddingLeft = 8,
                                paddingRight = 8,
                                backgroundColor = new Color(100/255f, 100/255f, 100/255f, 1),
                                borderBottomColor = _bindingAccentColor,
                                borderTopColor = _bindingAccentColor,
                                borderLeftColor = _bindingAccentColor,
                                borderRightColor = _bindingAccentColor,
                                borderBottomWidth = 2,
                                borderTopWidth = 2,
                                borderLeftWidth = 2,
                                borderRightWidth = 2,
                                borderBottomLeftRadius = 2,
                                borderTopLeftRadius = 2,
                                borderBottomRightRadius = 2,
                                borderTopRightRadius = 2,
                            }
                        };
             
                        var row1Container = new VisualElement
                        {
                            style =
                            {
                                flexDirection = FlexDirection.Row,
                                marginBottom = 4
                            }
                        };
                        row1Container.Add(new Label("Bound to: ")
                        {
                            style =
                            {
                                minWidth = 100
                            }
                        });
                        var dataSourceLink = new HyperLinkLabel
                        {
                            LinkIds = new List<string>{ "1" },
                            InstanceIds = new List<int>{ runtimeBindableProperty.BindingInfo.DataSource.GetInstanceID() },
                            focusable = true,
                            LinkHoverColor = _bindingAccentColor,
                            LinkColor = Color.white,
                            pickingMode = PickingMode.Position,
                            style =
                            {
                                flexGrow = 1
                            }
                        };
                        dataSourceLink.text =
                            $"<link=\"1\">{runtimeBindableProperty.BindingInfo.DataSource.name} - {runtimeBindableProperty.BindingInfo.DataSource.GetType().Name}<link>";
                        row1Container.Add(dataSourceLink);
                        
                        runtimeInfoContainer.Add(row1Container);

                        var row2Container = new VisualElement
                        {
                            style =
                            {
                                flexDirection = FlexDirection.Row
                            }
                        };
                        row2Container.Add(new Label("Path: ")
                        {
                            style =
                            {
                                minWidth = 100
                            }
                        });
                        row2Container.Add(new Label(runtimeBindableProperty.BindingInfo.PropertyPath.ToString())
                        {
                            style =
                            {
                                flexGrow = 1
                            }
                        });
                        runtimeInfoContainer.Add(row2Container);
                        
                        root.Add(runtimeInfoContainer);
                    }   
                }
                s_DebugPropertyVisitor.Reset();
            }
            
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