using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using CKIEditor.UI.EditSection.General;

namespace CKIEditor.UI.EditSection.GeneralSettings
{
    public class GeneralSettingsMediator : EditInstrumentSectionMediator
    {
        [Inject] public IOptionsModel OptionsModel { get; set; }
        
        [Inject] public InstrumentGeneralSettingsChangedSignal InstrumentGeneralSettingsChangedSignal { get; set; }
        
        [Inject] public GeneralSettingsView View { get; set; }
        
        public override void OnRegister()
        {
            View.MidiPortDropdown.options = OptionsModel.GetMidiPortOptions();
            View.MidiChannelDropdown.options = OptionsModel.GetMidiChannelOptions();
            View.DefaultNoteDropdown.options = OptionsModel.GetNoteOptions();
            View.DefaultNoteOctaveDropdown.options = OptionsModel.GetOctaveOptions();
            View.DefaultPatternDropdown.options = OptionsModel.GetPatternOptions();
            
            AddInputListener(View.InstrumentNameInput, InputHandler);
            
            AddDropdownListener(View.MidiPortDropdown, DropDownHandler);
            AddDropdownListener(View.MidiChannelDropdown, DropDownHandler);
            AddDropdownListener(View.DefaultNoteDropdown, DropDownHandler);
            AddDropdownListener(View.DefaultNoteOctaveDropdown, DropDownHandler);
            AddDropdownListener(View.DefaultPatternDropdown, DropDownHandler);
            
            AddToggleListener(View.MultiToggle, ToggleHandler);
            AddToggleListener(View.NoFtsToggle, ToggleHandler);
            AddToggleListener(View.NoTransposeToggle, ToggleHandler);
            AddToggleListener(View.PolySpreadToggle, ToggleHandler);
            
            base.OnRegister();
        }

        protected override void ShowInstrumentData(InstrumentDef instrumentDef)
        {
            var inst = InstrumentsModel.GetEditedInstrument();
            if(inst == null)
                return;

            View.InstrumentNameInput.text = inst.Name;
            View.MidiPortDropdown.value = inst.MidiPort - 1;
            View.MidiChannelDropdown.value = inst.MidiChannel - 1;
            View.DefaultNoteDropdown.value = NoteStringHelper.GetNoteIndex(inst.DefaultNote);
            View.DefaultNoteOctaveDropdown.value = NoteStringHelper.GetOctaveIndex(inst.DefaultNote);
            View.DefaultPatternDropdown.value = (int)inst.DefaultPattern;
            
            View.DefaultPatternDropdown.value = (int)inst.DefaultPattern;
            View.MultiToggle.isOn = inst.Multi;
            View.PolySpreadToggle.isOn = inst.PolySpread;
            View.NoTransposeToggle.isOn = inst.NoXpose;
            View.NoFtsToggle.isOn = inst.NoFts;

            if (inst.Name == InstrumentDef.DEFAULT_NAME)
                View.InstrumentNameInput.Select();    
        }
        
        private void UpdateInstrument()
        {
            var inst = InstrumentsModel.GetEditedInstrument();
            if(inst == null)
                return;  
            
            inst.Name = View.InstrumentNameInput.text;
            inst.MidiPort = View.MidiPortDropdown.value + 1;
            inst.MidiChannel = View.MidiChannelDropdown.value + 1;
            
            var noteId = NoteStringHelper.GetNoteId(View.DefaultNoteDropdown.value, View.DefaultNoteOctaveDropdown.value);
            inst.DefaultNote = NoteStringHelper.GetNoteName(noteId);
            
            inst.DefaultPattern =  (PatternType)View.DefaultPatternDropdown.value;
            
            inst.Multi = View.MultiToggle.isOn;
            inst.PolySpread = View.PolySpreadToggle.isOn;
            inst.NoXpose = View.NoTransposeToggle.isOn;
            inst.NoFts  = View.NoFtsToggle.isOn;

            InstrumentGeneralSettingsChangedSignal.Dispatch();
        }
        
        private void InputHandler(string value)
        {
            UpdateInstrument();
        }
        
        private void DropDownHandler(int value)
        {
            UpdateInstrument();    
        }
        
        private void ToggleHandler(bool value)
        {
            UpdateInstrument();    
        }
    }
}