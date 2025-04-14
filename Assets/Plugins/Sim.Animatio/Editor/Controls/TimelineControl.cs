using System.Collections.Generic;
using System.Linq;
using Plugins.Sim.Faciem.Shared;
using R3;
using Sim.Animatio.Editor.Controls;
using Unity.Properties;
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
        private readonly VisualElement _root;
        private readonly VisualElement _selectionIndicator;
        private readonly ScrollView _rootScrollView;
        private DisposableBagHolder _disposables;
        private DisposableBag _keyframeDisplayDisposables;

        private List<VisualElement> _selectedKeyFrames = new();
        private TimelineItemData _data;
        private ScrollView _keyFrameContainer;
        private VisualElement _ticksContainer;

        [CreateProperty, UxmlAttribute]
        public TimelineItemData Items
        {
            get => _data;
            set
            {
                _data = value;
                UpdateKeyframeDisplay();
            }
        }

        public TimelineControl()
        {
            // Load UXML and USS
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                    "Assets/Plugins/Sim.Animatio/Editor/Controls/TimelineControlView.uxml");
            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Assets/Plugins/Sim.Animatio/Editor/Controls/TimelineControlSyles.uss");

            _root = visualTree.CloneTree();
            _root.styleSheets.Add(styleSheet);
            _root.style.flexGrow = 1;
            Add(_root);

            _disposables = _root.RegisterDisposableBag();
            _disposables.Add(_root.ObserveEvent<PointerDownEvent>()
                .Subscribe(_ => ResetCurrentKeyFrameSelection()));
            
            _selectionIndicator = _root.Q<VisualElement>("SelectionIndicator");
            _rootScrollView = _root.Q<ScrollView>("RootScrollView");
            
            _ticksContainer = _root.Q<VisualElement>("KeyFrameTimeLine");
            
            var currentFrameDragManipulator = new CurrentFrameDragManipulator();
            _ticksContainer.AddManipulator(currentFrameDragManipulator);
            _disposables.Add(currentFrameDragManipulator
                .MovedToFrame
                .Subscribe(MoveSelection));
            
            // Single item test
            _keyFrameContainer = _root.Q<ScrollView>("KeyFrameContainer");
            _keyFrameContainer.contentContainer.AddManipulator(new KeyFrameDragDropManipulator(_selectedKeyFrames));
            var verticalScroller = _keyFrameContainer.verticalScroller;
            verticalScroller.AddToClassList("tick-vertical-scroller");
            _rootScrollView.contentViewport.Add(verticalScroller);
        }

        private void UpdateKeyframeDisplay()
        {
            _keyframeDisplayDisposables.Dispose();
            _keyframeDisplayDisposables = new DisposableBag();
            
            VisualElement firstFrameTick = null;
            _ticksContainer.Clear();
            
            for (var i = 0; i < _data.TotalFrames; i++)
            {
                var tick = new VisualElement();
                firstFrameTick ??= tick;
                tick.AddToClassList("tick-header");
                
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

                _ticksContainer.Add(tick);
            }

            schedule.Execute(() => MoveSelection(firstFrameTick))
                .ExecuteLater(100);
            
            _keyFrameContainer.contentContainer.Clear();
            
            for (var x = 0; x < _data.Items.Count; x++)
            {
                var valueRow = new VisualElement
                {
                    name = "keyframe-row-" + x
                };
                valueRow.AddToClassList("tick-value-row");
                
                var item = _data.Items[x];
                
                for (var i = 0; i < _data.TotalFrames; i++)
                {
                    var tickValueContainer = new VisualElement
                    {
                        name = $"tick-value-container_{i}",
                    };
                    tickValueContainer.AddToClassList("tick-value-container");
                    var keyFrame = new VisualElement
                    {
                        name = $"KeyFrame_{i}"
                    };
                    keyFrame.AddToClassList("tick-value");

                    if (item.KeyFrames.Contains(i))
                    {
                        keyFrame.AddToClassList("tick-value-keyframe");
                    }

                    _disposables.Add(
                        keyFrame.ObserveEvent<PointerDownEvent>()
                            .Subscribe(evt =>
                            {
                                if (!evt.ctrlKey && !_selectedKeyFrames.Contains(keyFrame))
                                {
                                    ResetCurrentKeyFrameSelection();
                                }

                                SetKeyFrameSelection(keyFrame, true);
                                _selectedKeyFrames.Add(keyFrame);
                            }));
                    
                    tickValueContainer.Add(keyFrame);
                    valueRow.Add(tickValueContainer);
                }
                _keyFrameContainer.contentContainer.Add(valueRow);
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
