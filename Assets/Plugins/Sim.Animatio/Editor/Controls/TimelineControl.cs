using System.Collections.Generic;
using Plugins.Sim.Faciem.Shared;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// ReSharper disable once CheckNamespace
namespace Sim.Animatio
{
    [UxmlElement]
    public partial class TimelineControl : VisualElement
    {
        private VisualElement _selectionIndicator;
        private ScrollView _rootScrollView;

        private const int TickCount = 100;  // Number of ticks to display

        private const int ValueRows = 10;

        private List<VisualElement> _selectedKeyFrames = new();
        
        public TimelineControl()
        {
            // Load UXML and USS
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/Plugins/Sim.Animatio/Editor/Controls/TimelineControlView.uxml");
            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Assets/Plugins/Sim.Animatio/Editor/Controls/TimelineControlSyles.uss");

            var root = visualTree.CloneTree();
            root.styleSheets.Add(styleSheet);
            root.style.flexGrow = 1;
            Add(root);

            var disposables = root.RegisterDisposableBag();
            
            _selectionIndicator = root.Q<VisualElement>("SelectionIndicator");
            
            var ticksContainer = root.Q<VisualElement>("KeyFrameTimeLine");

            for (var i = 0; i < TickCount; i++)
            {
                var tick = new VisualElement();

                disposables.Add(
                    tick
                        .ObserveEvent<PointerDownEvent>()
                        .AsUnitObservable()
                        .Merge(tick.ObserveEvent<PointerMoveEvent>()
                            .Where(_ => Mouse.current?.leftButton.isPressed ?? false)
                            .AsUnitObservable())
                        .Subscribe(_ => MoveSelection(tick)));
                
                if (i % 5 == 0)
                {
                    tick.AddToClassList("tick-header-big");
                    var label = new Label($"{i}");
                    label.AddToClassList("tick-header-big__text");
                    tick.Add(label);
                }
                else
                {
                    tick.AddToClassList("tick-header-small"); 
                }

                ticksContainer.Add(tick);
            }
            
            _rootScrollView = root.Q<ScrollView>("RootScrollView");
            
            // Single item test
            var keyFrameContainer = root.Q<ScrollView>("KeyFrameContainer");
            var verticalScroller = keyFrameContainer.verticalScroller;
            verticalScroller.AddToClassList("tick-vertical-scroller");
            _rootScrollView.contentViewport.Add(verticalScroller);
            
            for (var x = 0; x < ValueRows; x++)
            {
                var valueRow = new VisualElement();
                valueRow.AddToClassList("tick-value-row");
                for (var i = 0; i < TickCount; i++)
                {
                    var tickValueContainer = new VisualElement();
                    tickValueContainer.AddToClassList("tick-value-container");
                    var keyFrame = new VisualElement
                    {
                        name = $"KeyFrame_{i}"
                    };
                    keyFrame.AddToClassList("tick-value");

                    if (i % 5 == 0)
                    {
                        keyFrame.AddToClassList("tick-value-keyframe");
                    }

                    disposables.Add(
                        keyFrame.ObserveEvent<PointerDownEvent>()
                            .Subscribe(evt =>
                            {
                                if (!evt.ctrlKey)
                                {
                                    ResetCurrentKeyFrameSelection();
                                }

                                SetKeyFrameSelection(keyFrame, true);
                                _selectedKeyFrames.Add(keyFrame);

                            }));

                    _ = new KeyFrameDragDropManipulator(valueRow, keyFrame);
                    
                    tickValueContainer.Add(keyFrame);
                    valueRow.Add(tickValueContainer);
                }
                keyFrameContainer.contentContainer.Add(valueRow);
            }

        }

        private void ResetCurrentKeyFrameSelection()
        {
            foreach (var keyFrame in _selectedKeyFrames)
            {
                SetKeyFrameSelection(keyFrame, false);
            }
            _selectedKeyFrames.Clear();
        }

        private void SetKeyFrameSelection(VisualElement keyFrame, bool isSelected)
        {
            keyFrame.EnableInClassList("tick-value-keyframe-selected", isSelected);
        }
        

        private void MoveSelection(VisualElement selectedValue)
        {
            var childWorldPos = selectedValue.worldBound.position;
            var parentLocalPos = _rootScrollView.contentContainer.WorldToLocal(childWorldPos);
            _selectionIndicator.style.left = parentLocalPos.x + selectedValue.resolvedStyle.width / 2;
        }
    }
}
