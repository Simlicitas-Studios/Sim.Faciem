using System;
using System.Linq;
using System.Reflection;
using Plugins.Sim.Faciem.Shared;
using R3;
using Sim.Faciem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Plugins.Sim.Faciem.Editor
{
    [CustomPropertyDrawer(typeof(MonoScriptReference))]
    public class MonoScriptField : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            var typeFilters = fieldInfo
                .GetCustomAttributes<MonoScriptReferenceFilter>()
                .ToList();

            var dynamicFilters = fieldInfo
                .GetCustomAttributes<MonoScriptReferenceDependentFilter>()
                .Select(filter => property.serializedObject.FindProperty(filter.FieldName)
                    .FindPropertyRelative(nameof(MonoScriptReference.Script)))
                .Where(prop => prop != null)
                .ToList();

            var disposables = root.RegisterDisposableBag();
            
            var viewModelProperty = property.FindPropertyRelative(nameof(MonoScriptReference.Script));

            var label = new Label(property.displayName);
            root.Add(label);

            var valuePart = new VisualElement
            {
                style = { marginLeft = 16 }
            };
            
            var objectField = new ObjectField
            {
                objectType = typeof(MonoScript)
            };
            objectField.BindProperty(viewModelProperty);
            
            var validationLabel = new Label
            {
                style =
                {
                    marginLeft = 4,
                    color = Color.red,
                    flexWrap = Wrap.Wrap,
                    textOverflow = TextOverflow.Clip
                },
                enableRichText = true
            };

                disposables.Add(
                    objectField.ObserveValueChanged()
                        .CombineLatest(Observable.Interval(TimeSpan.FromSeconds(1), UnityTimeProvider.UpdateIgnoreTimeScale).Prepend(Unit.Default), (obj, _ ) => obj)
                        .OfType<Object, MonoScript>()
                        .Subscribe(newValue =>
                        {
                            var dependentTypes = dynamicFilters
                                .Select(dependentProperty => dependentProperty.objectReferenceValue)
                                .OfType<MonoScript>()
                                .Select(monoScript => monoScript.GetClass())
                                .Where(type => type != null)
                                .ToList();
                            
                            
                            if (typeFilters.Count > 0 || dependentTypes.Count > 0)
                            {
                                var type = newValue.GetClass(); 
                                
                                var missingTypes = typeFilters
                                    .Select(typeFilter => typeFilter.TargetType)
                                    .Concat(dependentTypes)
                                    .Where(typeFilter => !typeFilter.IsAssignableFrom(type))
                                    .ToList();

                                if (missingTypes.Any())
                                {
                                    validationLabel.text =
                                        $"The passed type must inherit from the types:\n {string.Join("\n", missingTypes.Select(x => x.FullName))}";
                                }
                                else
                                {
                                    validationLabel.text = string.Empty;
                                }

                            }

                            root.panel.visualTree.MarkDirtyRepaint();
                        }));
            
            valuePart.Add(objectField);
            valuePart.Add(validationLabel);
            root.Add(valuePart);

            return root;
        }
    }
}