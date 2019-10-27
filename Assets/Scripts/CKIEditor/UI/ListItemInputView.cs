using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CKIEditor.UI
{
    public class ListItemInputView : View
    {
        public TMP_InputField Input;
        [SerializeField]
        private Button _button;

        protected override void Start()
        {
            base.Start();
            _button.onClick.AddListener(ButtonOnclickHandler);
            Input.enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _button.onClick.RemoveListener(ButtonOnclickHandler);
        }
        
        private void ButtonOnclickHandler()
        {
            Input.enabled = true;
            Input.Select();    
        }
    }
}