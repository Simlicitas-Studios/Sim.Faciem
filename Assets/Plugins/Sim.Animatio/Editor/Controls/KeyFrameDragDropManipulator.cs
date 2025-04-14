using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sim.Animatio.Editor.Controls
{
    public class KeyFrameDragDropManipulator : PointerManipulator
    {
        private readonly List<VisualElement> _selectedElements;
        private bool _isDragging;
        private readonly Dictionary<VisualElement, VisualElement> _moveTargets = new();
        private Vector2 _startMousePos;

        private List<VisualElement> _dropZones;
        
        public KeyFrameDragDropManipulator(List<VisualElement> selected)
        {
            _selectedElements = selected;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || _selectedElements.Count == 0)
            {
                return;
            }
            
            _dropZones = target.Query<VisualElement>(className: "tick-value").ToList();
            
            _isDragging = true;
            target.CapturePointer(evt.pointerId);

            _startMousePos = evt.position;
            _moveTargets.Clear();

            foreach (var selectedKeyframe in _selectedElements)
            {
                selectedKeyframe.RemoveFromClassList("tick-value-keyframe");
                selectedKeyframe.AddToClassList("tick-value-keyframe-preview");
                _moveTargets[selectedKeyframe] = selectedKeyframe;
            }

            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging) return;
            
            
            var xDelta = _startMousePos.x - evt.position.x;
            
            foreach (var selectedKeyframe in _selectedElements)
            {
                var pos = new Vector2(selectedKeyframe.worldBound.position.x - xDelta, selectedKeyframe.worldBound.position.y);

                var maybeKeyFrameContainer = _dropZones.FirstOrDefault(dropZone => dropZone.worldBound.Contains(pos));

                if (maybeKeyFrameContainer == null)
                {
                    return;
                }
                
                if (_moveTargets.TryGetValue(selectedKeyframe, out var currentTarget)
                    && currentTarget != maybeKeyFrameContainer)
                {
                    currentTarget.RemoveFromClassList("tick-value-keyframe-preview");
                }
            
                maybeKeyFrameContainer.AddToClassList("tick-value-keyframe-preview");
                _moveTargets[selectedKeyframe] = maybeKeyFrameContainer;
            }
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_isDragging) return;

            _isDragging = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();

            foreach (var selectedKeyFrame in _selectedElements)
            {
                selectedKeyFrame.AddToClassList("tick-value-keyframe");
            
                if (_moveTargets.TryGetValue(selectedKeyFrame, out var currentTarget))
                {
                    currentTarget.RemoveFromClassList("tick-value-keyframe-preview");
                }
            }
            
        }
    }
}
