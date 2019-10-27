using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Framewerk.UI.Components
{
    public enum DragState
    {
        Start,
        Move,
        End
    }

    /// <summary>
    /// DragPosition - dispatches coordinates of current touch relative to rect transform pivot
    /// DragPositionRelative - dispatches coordinates of current touch relative to begin touch point
    /// NormalizedPosition - dispatches normalized coordinates of current touch relative to rect transform pivot normalized to 0 - 1 when inside toucharea
    /// NormalizedPositionRelative - dispatches normalized coordinates of current touch relative to begin touch point normalized to 0 - 1 when inside toucharea
    ///
    /// Normalized positions are not clamped, so when going out of element, or with relative values starting elsewhere than
    /// </summary>
    public struct DragData
    {
        public DragState DragState;
        public Vector2 DragPosition;
        public Vector2 DragPositionRelative;
        public Vector2 NormalizedPosition;
        public Vector2 NormalizedPositionRelative;
    }

    public class DragElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Signal<DragData> DragChangedSignal = new Signal<DragData>();
        
        /// scales normalized position.
        /// e.g when we have pivot point in rect center we should preferably get values in range -0.5 to 0.5
        /// if we would rather prefer values -1 to 1 we could achieve that easily by setting scale to 2.
        public float NormalizedPositionScale = 1;

        public bool IsFloating = false;
        
        public bool RestrictFloatingByParentBounds;
        
        private RectTransform _rectTransform;
        private Rect _rect;
        private Vector2 _startPosition;
        private Vector2 _startPositionNormalized;
        
        public float MagnitudeNormalised;
        public Vector2 DragPositionRelative;
        public Vector2 DragPosition;
        public Vector2 DragPositionRelativeScaled;
        public Vector2 DragPositionScaled;
        
        private RectTransform _parentRect;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentRect = transform.parent.GetComponent<RectTransform>();
            
            _rect = _rectTransform.rect;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Store drag begin positions
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.pressPosition, eventData.pressEventCamera, out Vector2 pressPosition);
            _startPosition = pressPosition;
            _startPositionNormalized = pressPosition / _rect.size * NormalizedPositionScale;
            
            ProcessDrag(DragState.Start, eventData);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            ProcessDrag(DragState.Move, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ProcessDrag(DragState.End, eventData);
        }

        private void ProcessDrag(DragState dragState, PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, 
                                                                    eventData.position, 
                                                                    eventData.pressEventCamera, 
                                                                    out Vector2 dragPosition);
            
            var dragPositionNormalized = dragPosition / _rect.size * NormalizedPositionScale;
            
            //debug 
            DragPositionScaled = dragPositionNormalized;
            DragPositionRelativeScaled = dragPositionNormalized - _startPositionNormalized;
            MagnitudeNormalised = dragPositionNormalized.magnitude;
            
            //FLOATING
            //criteria for floating are now based on circle(distance from pivot)
            if (IsFloating && Mathf.Abs(dragPositionNormalized.magnitude) > 1)
            {  
                //move touch area
                var origPosition = _rectTransform.localPosition;
                var adjust = (dragPositionNormalized - dragPositionNormalized.normalized) * _rect.size / NormalizedPositionScale;
                var newPosition = origPosition + new Vector3(adjust.x, adjust.y, 0f);

                //Clamp new position by parent bounds
                if (RestrictFloatingByParentBounds)
                {
                    var parentRect = _parentRect.rect;
                    
                    newPosition.x = Mathf.Clamp(newPosition.x, parentRect.xMin, parentRect.xMax);
                    newPosition.y = Mathf.Clamp(newPosition.y, parentRect.yMin, parentRect.yMax);
                }
                
                _rectTransform.localPosition = newPosition;  
            }
            
            
            var dragData = new DragData { DragState = dragState,
                                          DragPosition = dragPosition, 
                                          DragPositionRelative = dragPosition - _startPosition, 
                                          NormalizedPosition = dragPositionNormalized, 
                                          NormalizedPositionRelative = dragPositionNormalized - _startPositionNormalized };
            
            DragChangedSignal.Dispatch(dragData);

            DragPositionRelative = dragPosition - _startPosition;
            DragPosition = dragPosition;
        }
    }
}