using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace CKIEditor.UI.NoteRows
{
    public class AddNoteDefView : View
    {
        public TMP_Dropdown NoteDropdown;
        public TMP_Dropdown OctaveDropdown;
        public TMP_InputField NameInput;
        public Toggle AlwaysShowToggle;

        public Button SaveButton;
    }
}