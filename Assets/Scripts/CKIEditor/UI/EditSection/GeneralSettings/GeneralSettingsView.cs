using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.UI;

namespace CKIEditor.UI.EditSection.General
{
    public class GeneralSettingsView : View
    {
        public TMP_InputField InstrumentNameInput;
        public TMP_Dropdown MidiPortDropdown;
        public TMP_Dropdown MidiChannelDropdown;
        public TMP_Dropdown DefaultNoteDropdown;
        public TMP_Dropdown DefaultNoteOctaveDropdown;
        public TMP_Dropdown DefaultPatternDropdown;
        public Toggle MultiToggle;
        public Toggle PolySpreadToggle;
        public Toggle NoTransposeToggle;
        public Toggle NoFtsToggle;
    }
}