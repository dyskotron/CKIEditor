using Framewerk.UI;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace CKIEditor.UI
{
    public class EditorScreenView : View
    {
        public Button QuitButton;
    }

    public class EditorScreenMediator : ExtendedMediator
    {
        [Inject] public EditorScreenView View { get; set; }

        public override void OnRegister()
        {
            AddButtonListener(View.QuitButton, QuitButtonListener);
            base.OnRegister();
        }

        private void QuitButtonListener()
        {
            Application.Quit();
        }
    }
}