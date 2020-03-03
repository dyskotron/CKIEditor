using System.Collections.Generic;
using CKIEditor.Model.Defs;

namespace CKIEditor.Serialization
{
    public interface IInstrumentsParser
    {
        List<InstrumentDef> ParseInstruments(string json);
        string SerializeInstruments(List<InstrumentDef> instruments);
    }
}