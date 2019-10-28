namespace CKIEditor.Model.Defs
{
    public struct Note
    {
        public int Id => _id;
        public string Name => _name;
        public int NoteIndex => _noteIndex;
        public int OctaveIndex => _octaveIndex;

        private int _id;
        private string _name;
        private int _noteIndex;
        private int _octaveIndex;

        public Note(int id)
        {
            _id = id;
            _name = NoteStringHelper.GetNoteName(id);
            _noteIndex = NoteStringHelper.GetNoteIndex(id);
            _octaveIndex = NoteStringHelper.GetOctaveIndex(id);
        }

        public Note(string name)
        {
            _id = NoteStringHelper.GetNoteId(name);
            _name = name;
            _noteIndex = NoteStringHelper.GetNoteIndex(_id);
            _octaveIndex = NoteStringHelper.GetOctaveIndex(_id);
        }
        
        public Note(int noteIndex, int octaveIndex)
        {
            _id = NoteStringHelper.GetNoteId(noteIndex, octaveIndex);
            _name = NoteStringHelper.GetNoteName(noteIndex, octaveIndex);
            _noteIndex = noteIndex;
            _octaveIndex = octaveIndex;
        }
    }
}