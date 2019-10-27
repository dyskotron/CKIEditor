using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Framewerk.UI.Components
{
    public interface IButtonComponent
    {
        void SetLabel(string label);

        Button Button { get; }
    }

    public class ButtonComponent : MonoBehaviour, IButtonComponent
    {
        public Text Label;
        
        public Button Button => _button;
        [SerializeField] private Button _button;
        
        public void SetLabel(string label)
        {
            Label.text = label;
        }
    }
}