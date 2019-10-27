using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Framewerk.UI.Components
{
    public class PointerElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public class OnPointerChangeEvent : UnityEvent<bool, Vector2>
        {

        }

        public OnPointerChangeEvent OnPointerChanged
        {
            get { return _onPointerChanged; }
        }

        private readonly OnPointerChangeEvent _onPointerChanged = new OnPointerChangeEvent();

        public void OnPointerDown(PointerEventData eventData)
        {
            _onPointerChanged.Invoke(true, eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _onPointerChanged.Invoke(false, eventData.position);
        }
    }
}