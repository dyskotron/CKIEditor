using Framewerk.UI.List;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CKIEditor.UI.NoteRows
{
    public class NoteRowListItemView : ListItemView
    {
        public Button DeleteButton;
        
        public TMP_Dropdown NoteDropdown;
        public TMP_Dropdown OctaveDropdown;
        public TMP_InputField NameInput;
        public Toggle AlwaysShowToggle;  
        
        public Image BackgroundImage;
        
        [Header("Colors")] 
        public Color NormalColor;
        public Color SelectedColor;
    }
}