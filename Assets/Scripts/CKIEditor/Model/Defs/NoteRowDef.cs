using Framewerk.UI.List;

namespace CKIEditor.Model.Defs
{
    public class NoteRowDef : IListItemDataProvider
    {
        public Note Note{ get; private set; }
        public string Label { get; private set; }
        public bool AlwaysShow { get; private set; }
        
        public void SetNote(Note value)
        {
            Note = value;
        }
        
        public void SetLabel(string value)
        {
            Label = value;
        }

        public void SetAlwaysShow(bool value)
        {
            AlwaysShow = value;
        }

        public override string ToString()
        {
            return  $"NoteRowDef id: {Note.Id} note: {Note.Name} label{Label}";
        }
    }
}