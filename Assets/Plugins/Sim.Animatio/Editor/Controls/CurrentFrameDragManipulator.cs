using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace Sim.Animatio.Editor.Controls
{
    public class CurrentFrameDragManipulator : PointerManipulator
    {
        private readonly Subject<VisualElement> _onMoveToSubject = new();
        private bool _isDragging;
        private List<VisualElement> _dropZones;

        public Observable<VisualElement> MovedToFrame => _onMoveToSubject;

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
            if (evt.button != 0)
            {
                return;
            }
            
            _dropZones = target.Query<VisualElement>(className: "tick-header").ToList();
          
            _isDragging = true;
            target.CapturePointer(evt.pointerId);
            evt.StopPropagation();
            
            EvaluateFrameIndicatorPosition(evt.position.x);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging)
            {
                return;
            }

            EvaluateFrameIndicatorPosition(evt.position.x);
        }

        private void EvaluateFrameIndicatorPosition(float x)
        {
            var pos = new Vector2(x, target.worldBound.position.y);

            var maybeKeyFrameContainer = _dropZones.FirstOrDefault(dropZone => dropZone.worldBound.Contains(pos));

            if (maybeKeyFrameContainer == null)
            {
                return;
            }
           
            _onMoveToSubject.OnNext(maybeKeyFrameContainer);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_isDragging) return;

            _isDragging = false;
            target.ReleasePointer(evt.pointerId);
            evt.StopPropagation();
        }
    }
}