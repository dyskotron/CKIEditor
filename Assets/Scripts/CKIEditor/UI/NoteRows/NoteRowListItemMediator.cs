using CKIEditor.Controller;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Framewerk.UI.List;

namespace CKIEditor.UI.NoteRows
{
    public class NoteRowListItemMediator : ListItemMediator<NoteRowListItemView, NoteRowDef>
    {
        [Inject] public IOptionsModel OptionsModel { get; set; }
        
        [Inject] public DeleteNoteRowSignal DeleteNoteRowSignal { get; set; }
        [Inject] public EditNoteRowSignal EditNoteRowSignal { get; set; }

        public override void OnRegister()
        {
            View.NoteDropdown.options = OptionsModel.GetNoteOptions();
            View.OctaveDropdown.options = OptionsModel.GetOctaveOptions();

            AddUiListeners();
            
            base.OnRegister();
        }

        public override void SetData(NoteRowDef dataProvider, int index)
        {
            base.SetData(dataProvider, index);

            RemoveListeners();

            View.NameInput.text = dataProvider.Label;
            View.NoteDropdown.value = NoteStringHelper.GetNoteIndex(dataProvider.Note.Name);
            View.OctaveDropdown.value = NoteStringHelper.GetOctaveIndex(dataProvider.Note.Name);
            View.AlwaysShowToggle.isOn = dataProvider.AlwaysShow;

            AddUiListeners();
        }
        
        public override void SetSelected(bool selected)
        {
            base.SetSelected(selected);
            
            View.BackgroundImage.color = IsSelected ? View.SelectedColor : View.NormalColor;
        }

        private void NoteRowEdited()
        {
            var noteRow = new NoteRowDef();
            noteRow.SetLabel(View.NameInput.text);
            noteRow.SetAlwaysShow(View.AlwaysShowToggle.isOn);
            noteRow.SetNote(new Note(View.NoteDropdown.value, View.OctaveDropdown.value));
            
            EditNoteRowSignal.Dispatch(DataProvider.Note.Id, noteRow);
        }

        private void AddUiListeners()
        {
            AddInputListener(View.NameInput, NameInputHandler);
            AddToggleListener(View.AlwaysShowToggle, AlwaysShowToggleHandler);
            AddDropdownListener(View.NoteDropdown, DropDownHandler);
            AddDropdownListener(View.OctaveDropdown, DropDownHandler);
            
            AddButtonListener(View.DeleteButton, DeleteButtonHandler);    
        }
        
        private void DeleteButtonHandler()
        {
            DeleteNoteRowSignal.Dispatch(DataProvider.Note);
        }
        
        private void NameInputHandler(string value)
        {
            DataProvider.SetLabel(value);
            NoteRowEdited();
        }

        private void AlwaysShowToggleHandler(bool value)
        {
            DataProvider.SetAlwaysShow(value);
            NoteRowEdited();
        }

        private void DropDownHandler(int value)
        {
            DataProvider.SetNote(new Note(View.NoteDropdown.value, View.OctaveDropdown.value));
            NoteRowEdited();
        }
    }
}