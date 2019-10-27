using Framewerk.UI.List;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CKIEditor.UI.TrackValues.CcList
{
    public class CcListItemView : ListItemView
    {
        public TMP_InputField CcInput;
        public TMP_InputField NameInput;
        public TMP_InputField StartInput;
        public TMP_InputField MinInput;
        public TMP_InputField MaxInput;
        
        public Button RemoveButton;
        public Button SaveButton;
        
        public Image BackgroundImage;

        [Header("Colors")] 
        public Color NormalColor;
        public Color SelectedColor;
    }
}