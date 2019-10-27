using System;
using System.Collections.Generic;
using Framewerk.UI.Components;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framewerk.UI
{
    public class ExtendedMediator : Mediator
    {
        // Handlers
        private Dictionary<GameObject, UnityAction> ButtonHandlers = new Dictionary<GameObject, UnityAction>();
        private Dictionary<GameObject, UnityAction<bool>> ToggleHandlers = new Dictionary<GameObject, UnityAction<bool>>();
        private Dictionary<GameObject, UnityAction<int>> DropdownHandlers = new Dictionary<GameObject, UnityAction<int>>();
        private Dictionary<GameObject, UnityAction<bool, Vector2>> PointerHandlers = new Dictionary<GameObject, UnityAction<bool, Vector2>>();
        private Dictionary<GameObject, UnityAction<float>> SliderHandlers = new Dictionary<GameObject, UnityAction<float>>();
        private Dictionary<GameObject, UnityAction<string>> InputHandlers = new Dictionary<GameObject, UnityAction<string>>();
        private Dictionary<GameObject, Action<DragData>> DragHandlers = new Dictionary<GameObject, Action<DragData>>();

        public override void OnRemove()
        {
            RemoveListeners();
            base.OnRemove();
        }

        #region RemovingListeners

        protected void RemoveListeners()
        {
            RemoveButtonListeners();
            RemoveToggleListeners();
            RemoveDropdownListeners();
            RemoveSliderListeners();
            RemoveInputListeners();
            RemovePointerListeners();
            RemoveDragListeners();
        }

        protected void RemoveSliderListeners()
        {
            foreach (var pair in SliderHandlers)
            {
                var s = pair.Key.GetComponent<Slider>();
                s.onValueChanged.RemoveListener(pair.Value);
            }
            SliderHandlers.Clear();
        }

        protected void RemoveButtonListeners()
        {
            foreach (var pair in ButtonHandlers)
            {
                var b = pair.Key.GetComponent<Button>();
                b.onClick.RemoveListener(pair.Value);
            }
            ButtonHandlers.Clear();
        }

        protected void RemoveToggleListeners()
        {
            foreach (var pair in ToggleHandlers)
            {
                var b = pair.Key.GetComponent<Toggle>();
                b.onValueChanged.RemoveListener(pair.Value);
            }
            ToggleHandlers.Clear();
        }

        protected void RemoveDropdownListeners()
        {
            foreach (var pair in DropdownHandlers)
            {
                var b = pair.Key.GetComponent<TMP_Dropdown>();
                b.onValueChanged.RemoveListener(pair.Value);
            }
            ToggleHandlers.Clear();
        }

        protected void RemoveInputListeners()
        {
            foreach (var pair in InputHandlers)
            {
                //TODO:separate
                var b = pair.Key.GetComponent<InputField>();
                b?.onValueChanged.RemoveListener(pair.Value);
                
                var i = pair.Key.GetComponent<TMP_InputField>();
                i?.onValueChanged.RemoveListener(pair.Value);
            }
            InputHandlers.Clear();
        }

        protected void RemovePointerListeners()
        {
            foreach (var pair in PointerHandlers)
            {
                var m = pair.Key.GetComponent<PointerElement>();
                m.OnPointerChanged.RemoveListener(pair.Value);
            }
            PointerHandlers.Clear();
        }

        protected void RemoveDragListeners()
        {
            foreach (var pair in DragHandlers)
            {
                var m = pair.Key.GetComponent<DragElement>();
                m.DragChangedSignal.RemoveListener(pair.Value);
            }
            DragHandlers.Clear();
        }

        #endregion

        #region Adding Listeners

        protected void AddButtonListener(Button button, Action func)
        {
            UnityAction internalAction = () => { func(); };
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(internalAction);
            ButtonHandlers[button.gameObject] = internalAction;
        }

        protected void AddSliderListener(Slider slider, Action<float> func)
        {
            UnityAction<float> internalAction = (val) => { func(val); };

            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(internalAction);
            SliderHandlers[slider.gameObject] = internalAction;
        }

        protected void AddToggleListener(Toggle toggle, Action<bool> func)
        {
            UnityAction<bool> internalAction = (val) => { func(val); };

            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(internalAction);
            ToggleHandlers[toggle.gameObject] = internalAction;
        }

        protected void AddDropdownListener(TMP_Dropdown dropdown, Action<int> func)
        {
            UnityAction<int> internalAction = (val) => { func(val); };

            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener(internalAction);
            DropdownHandlers[dropdown.gameObject] = internalAction;
        }

        protected void AddInputListener(InputField input, Action<string> func)
        {
            UnityAction<string> internalAction = (val) => { func(val); };

            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener(internalAction);
            InputHandlers[input.gameObject] = internalAction;
        }
        
        protected void AddInputListener(TMP_InputField input, Action<string> func)
        {
            UnityAction<string> internalAction = (val) => { func(val); };

            input.onEndEdit.RemoveAllListeners();
            input.onEndEdit.AddListener(internalAction);
            InputHandlers[input.gameObject] = internalAction;
        }
        
        protected void AddPointerListener(PointerElement pointerElement, Action<bool, Vector2> func)
        {
            UnityAction<bool, Vector2> internalAction = (state, pos) => { func(state, pos); };

            pointerElement.OnPointerChanged.RemoveAllListeners();
            pointerElement.OnPointerChanged.AddListener(internalAction);
            PointerHandlers[pointerElement.gameObject] = internalAction;
        }

        protected void AddDragListener(DragElement dragElement, Action<DragData> handler)
        {
            dragElement.DragChangedSignal.RemoveAllListeners();
            dragElement.DragChangedSignal.AddListener(handler);

            DragHandlers[dragElement.gameObject] = handler;
        }

        #endregion
    }
}