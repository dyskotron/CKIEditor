using System;
using System.Collections.Generic;
using CKIEditor.Model.Defs;
using UnityEngine;

namespace CKIEditor.Serialization
{
    public class PyramidInstrumentsParser : IInstrumentsParser
    {
        private const string NAME = "NAME";
        private const string OUT = "OUT";
        private const string CHANNEL = "CHANNEL";
        
        public List<InstrumentDef> ParseInstruments(string content)
        {   
            var instruments = new List<InstrumentDef>();
            InstrumentDef instrument = null;
            var lines = content.Split(new[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            var i = 0;
            while (i < lines.Length - 1)
            {
                string name;
                string value;

                if (!ParseLine(lines[i++], out name, out value))
                {
                    Debug.LogError($"<color=\"aqua\">PyramidInstrumentsParser.ParseInstruments() : Invalid line({i}) : {lines[i]}</color>");
                    return null;
                }

                if (name == NAME)
                {
                    //new instrument
                    instrument = new InstrumentDef();
                    instrument.Name = value;
                    
                    //Midi Port
                    ParseLine(lines[i++], out name, out value); //TODO check ParseLine return value
                    if(name != OUT)
                        Debug.LogError($"<color=\"aqua\">PyramidInstrumentsParser.ParseInstruments() : Instrument OUT expected on line({i}) : {lines[i]}</color>");

                    instrument.MidiPort = value == "A" ? 1 : 2;
                    
                    //Midi Channel
                    ParseLine(lines[i++], out name, out value); //TODO check ParseLine return value
                    if(name != CHANNEL)
                        Debug.LogError($"<color=\"aqua\">PyramidInstrumentsParser.ParseInstruments() : Instrument CHANNEL expected on line({i}) : {lines[i]}</color>");

                    instrument.MidiChannel = int.Parse(value);
                    
                    //Add to collection
                    instruments.Add(instrument);
                }

                if (instrument == null)
                {
                    Debug.LogError($"<color=\"aqua\">PyramidInstrumentsParser.ParseInstruments() : Instrument NAME expected on line({i}) : {lines[i]}</color>");
                    return null;
                }
                
                ParseLine(lines[i++], out name, out value); //TODO check ParseLine return value

                if (name[0] == 'N')
                {
                    Debug.LogWarning($"<color=\"aqua\">PyramidInstrumentsParser.ParseInstruments() : we got note{int.Parse(name.Substring(1))}</color>");
                    
                    var row = new NoteRowDef();
                    row.SetLabel(value);
                    row.SetNote(new Note(int.Parse(name.Substring(1))));
                    row.SetAlwaysShow(true);
                    
                    instrument.NoteRowDefs.Add(row.Note.Id,row);
                }
                else
                {
                    Debug.LogWarning($"<color=\"aqua\">PyramidInstrumentsParser.ParseInstruments() : we got CC{int.Parse(name)}</color>");

                    var ccNum = int.Parse(name);
                    var ccDef = new CcDef(ccNum);
                    ccDef.SetLabel(value);
                    
                    instrument.CcDefs[ccNum] = ccDef;
                }

            } 

            return instruments;
        }

        private bool ParseLine(string line, out string name, out string value)
        {
            var components = line.Split(':');

            if (components.Length < 2)
            {
                name = value = null;
                return false;
            }

            name = components[0].ToUpper();
            value = components[1].ToUpper();

            return true;
        }

        public string SerializeInstruments(List<InstrumentDef> instruments)
        {
            throw new NotImplementedException();
        }
    }
}