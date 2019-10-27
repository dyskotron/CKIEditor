using Framewerk.UI.List;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CKIEditor.UI.MainMenu
{
    public class MainMenuItemView : ListItemView
    {
        public TMP_Text Label;
        public Image BackgroundImage;
        
        public Color BaseColor = Color.grey;
        public Color SelectedColor = Color.cyan;
    }
}