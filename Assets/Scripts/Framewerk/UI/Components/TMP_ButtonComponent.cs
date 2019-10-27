using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Framewerk.UI.Components
{
    public class TMP_ButtonComponent : MonoBehaviour, IButtonComponent
    {
        public TMP_Text Label;
        
        public Button Button => _button;
        [SerializeField] private Button _button;
        
        public void SetLabel(string label)
        {
            Label.text = label;
        }
    }
}