using System.Collections.Generic;
using Framewerk.UI.List;

namespace CKIEditor.Model.Defs
{
    public class InstrumentDef : IListItemDataProvider
    {
        public static string DEFAULT_NAME = "New instrument";
        
        public int Id;
        public string Name = DEFAULT_NAME;
        public int MidiPort = 1; // 1 - 5 midi 6 - 12 usb (usb1 - usb6)
        public int MidiChannel = 1;
        
        //TODO: off + C0 - G10
        public Note DefaultNote;
        public PatternType DefaultPattern = PatternType.Sel;
        public bool Multi;
        public bool PolySpread;
        public bool NoXpose;
        public bool NoFts;
        public Dictionary<int, TrackValueDef> TrackValues = new Dictionary<int, TrackValueDef>();
        public Dictionary<int, CcDef> CcDefs = new Dictionary<int, CcDef>();
        public Dictionary<int, NoteRowDef> NoteRowDefs = new Dictionary<int, NoteRowDef>();

        public InstrumentDef()
        {
            for (var i = 1; i < CkiConsts.TRACK_VALUES_PER_SCREEN * 20 + 1; i++)
            {
                TrackValues[i] = new TrackValueDef {SlotIndex = i, Type = TrackValueType.Empty};
            }
        }
    }
}