using System.Collections.Generic;
using CKIEditor.Model.Defs;
using UnityEngine;

namespace CKIEditor.Model
{
    public interface IInstrumentsModel
    {
        InstrumentDef GetEditedInstrument();
        void SelectEditedInstrument(int id);
        void AddInstruments(List<InstrumentDef> instruments);
        int AddInstrument(InstrumentDef instrument);
        List<InstrumentDef> GetAllInstruments();
        InstrumentDef GetInstrumentById(int id);
        InstrumentDef GetInstrumentByName(string name);
    }
    
    public class InstrumentsModel : IInstrumentsModel
    {
        private int _editedInstrumentId = 0;
        private List<InstrumentDef> _instruments = new List<InstrumentDef>();
        private Dictionary<int, InstrumentDef> _instrumentById = new Dictionary<int, InstrumentDef>();

        public InstrumentDef GetEditedInstrument()
        {
            _instrumentById.TryGetValue(_editedInstrumentId, out InstrumentDef instrumentDef);
            return instrumentDef;
        }

        public void SelectEditedInstrument(int id)
        {
            if(!_instrumentById.ContainsKey(id))
                Debug.LogError($"<color=\"aqua\">InstrumentsModel.SelectEditedInstrument() : No instrument with id:{id} </color>");

            _editedInstrumentId = id;
        }

        public void AddInstruments(List<InstrumentDef> instruments)
        {
            _instruments.AddRange(instruments);

            var id = _instrumentById.Values.Count;
            foreach (var instrumentDef in instruments)
            {   
                instrumentDef.Id = id;
                _instrumentById[id] = instrumentDef;
                id++;
            }
        }
        
        public int AddInstrument(InstrumentDef instrument)
        {
            var id = _instrumentById.Count; 
            
            _instruments.Add(instrument);
            _instrumentById[id] = instrument;

            return id;
        }
        
        public List<InstrumentDef> GetAllInstruments()
        {
            return _instruments;
        }

        public InstrumentDef GetInstrumentById(int id)
        {
            return _instrumentById[id];
        }

        public InstrumentDef GetInstrumentByName(string name)
        {
            foreach (var instrumentDef in _instruments)
            {
                if (instrumentDef.Name == name)
                    return instrumentDef;
            }

            return null;
        }
    }
}