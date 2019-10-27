namespace CKIEditor.Model.Defs
{
    public struct Note
    {
        public int Id => _id;
        public string Name => _name;

        private int _id;
        private string _name;

        public Note(int id)
        {
            _id = id;
            _name = NoteStringHelper.GetNoteName(id);
        }

        public Note(string name)
        {
            _id = NoteStringHelper.GetNoteId(name);
            _name = name;
        }
        
        public Note(int noteIndex, int octaveIndex)
        {
            _id = NoteStringHelper.GetNoteId(noteIndex, octaveIndex);
            _name = NoteStringHelper.GetNoteName(noteIndex, octaveIndex);
        }
    }
}