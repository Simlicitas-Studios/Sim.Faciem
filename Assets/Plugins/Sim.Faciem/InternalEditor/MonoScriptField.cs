﻿using System;
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
            var root = new VisualElement
            {
                style =
                {
                    flexGrow = 1,
                    alignItems = Align.Stretch,
                }
            };
            
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

            var label = new Label(property.displayName)
            {
                style = 
                {
                    width = Length.Percent(40),
                }
            };

            var valuePart = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                }
            };
            
            valuePart.Add(label);
            
            var objectField = new ObjectField
            {
                objectType = typeof(MonoScript),
                style =
                {
                    width = Length.Percent(60)
                }
            };
            objectField.BindProperty(viewModelProperty);
            
            var validationLabel = new Label
            {
                style =
                {
                    marginLeft = 4,
                    color = Color.red,
                    flexWrap = Wrap.Wrap,
                    textOverflow = TextOverflow.Clip,
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
                                    .Where(typeFilter => !IsAssignableTo(type, typeFilter))
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
            root.Add(valuePart);
            root.Add(validationLabel);

            return root;
        }

        private bool IsAssignableTo(Type type, Type filterType)
        {
            if (filterType.IsGenericType)
            {
                var targetType = type;

                do
                {
                    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == filterType)
                    {
                        return true;
                    }
                    targetType = targetType.BaseType;
                } while (targetType != null && targetType != typeof(object));
            }
            
            return filterType.IsAssignableFrom(type);
        }
    }
}