using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI;

namespace CKIEditor.UI.NoteRows
{
    public class AddNoteDefMediator : ExtendedMediator
    {
        [Inject] public IOptionsModel OptionsModel { get; set; }

        [Inject] public AddNoteRowSignal AddNoteRowSignal { get; set; }

        [Inject] public AddNoteDefView View { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            
            View.NoteDropdown.options = OptionsModel.GetNoteOptions();
            View.OctaveDropdown.options = OptionsModel.GetOctaveOptions();
            
            AddButtonListener(View.SaveButton, SaveButtonListener);
        }

        private void SaveButtonListener()
        {
            var ccDef = new NoteRowDef();
            ccDef.SetNote(new Note(View.NoteDropdown.value, View.OctaveDropdown.value));
            ccDef.SetLabel(View.NameInput.text);
            ccDef.SetAlwaysShow(View.AlwaysShowToggle.isOn);

            AddNoteRowSignal.Dispatch(ccDef);
            ResetValues();
        }

        public void ResetValues()
        {
            View.NoteDropdown.value = 0;
            View.OctaveDropdown.value = 0;
            View.NameInput.text = "";
            View.AlwaysShowToggle.isOn = true;
        }
    }
}