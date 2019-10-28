using System;
using System.Collections.Generic;
using System.IO;
using CKIEditor.Model;
using CKIEditor.Model.Defs;
using Crosstales.FB;
using SimpleJSON;
using UnityEngine;

namespace CKIEditor.Serialization
{
    public interface IInstrumentsParser
    {
        List<InstrumentDef> ParseInstruments(string json);
        InstrumentDef ParseInstrument(string name, JSONNode json);
        string BuildCkiFile(List<InstrumentDef> instruments);
    }

    public class InstrumentsParser : IInstrumentsParser
    {
        private static int TRACK_VALUE_INT_POS = 5;
        private static int CC_DEF_INT_POS = 3;


        public List<InstrumentDef> ParseInstruments(string json)
        {
            var instrumentList = new List<InstrumentDef>();

            var jsonNode = JSON.Parse(json);
            var rootNode = jsonNode[JsonKeys.INSTRUMENT_DATA];

            foreach (string instrumentKey in rootNode.Keys)
            {
                instrumentList.Add(ParseInstrument(instrumentKey, rootNode[instrumentKey]));
            }

            return instrumentList;
        }

        public InstrumentDef ParseInstrument(string name, JSONNode json)
        {
            var instrumentDef = new InstrumentDef();

            var defaultPatternString = json[JsonKeys.DEFAULT_PATT];
            Enum.TryParse(defaultPatternString, true, out PatternType defaultPattern);

            // GLOBAL
            instrumentDef.Name = name;
            instrumentDef.MidiPort = json[JsonKeys.MIDI_PORT];
            instrumentDef.MidiChannel = json[JsonKeys.MIDI_CHAN]; 
            
            if(json[JsonKeys.DEFAULT_NOTE] != "off")
                instrumentDef.DefaultNote = new Note((string)json[JsonKeys.DEFAULT_NOTE]);
            
            instrumentDef.DefaultPattern = defaultPattern;
            instrumentDef.Multi = json[JsonKeys.MULTI];
            instrumentDef.PolySpread = json[JsonKeys.POLY_SPREAD];
            instrumentDef.NoFts = json[JsonKeys.NO_FTS];
            instrumentDef.NoXpose = json[JsonKeys.NO_XPOSE];

            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() : =================={name}==================</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - MidiPort:{instrumentDef.MidiPort}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - MidiChannel:{instrumentDef.MidiChannel}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - DefaultNote:{instrumentDef.DefaultNote}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - DefaultPattern:{instrumentDef.DefaultPattern}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - Multi:{instrumentDef.Multi}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - PolySpread:{instrumentDef.PolySpread}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - NoFts:{instrumentDef.NoFts}</color>");
            Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() GLOBAL - NoXpose:{instrumentDef.NoXpose}</color>");

            //TRACK VALUES
            var trackValuesJson = json[JsonKeys.TRACK_VALUES];
            foreach (string trackValueKey in trackValuesJson.Keys)
            {
                var slotIndex = int.Parse(trackValueKey.Substring(TRACK_VALUE_INT_POS));
                var trackValueDef = ParseTrackValueDef(slotIndex, trackValuesJson[trackValueKey]);
                if (trackValueDef != null)
                {
                    instrumentDef.TrackValues[trackValueDef.SlotIndex] = trackValueDef;

                    if (trackValueDef.Type == TrackValueType.MidiCC)
                        Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() TRACK VALUE - Label: {trackValueDef.Label}, CC: {trackValueDef.MidiCC}</color>");
                    else if (trackValueDef.Type == TrackValueType.TrackControl)
                        Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() TRACK VALUE - TrackControl: {trackValueDef.TrackControl}</color>");
                }
            }

            //CC DEFS
            var ccDefsJson = json[JsonKeys.CC_DEFS];
            foreach (string ccDefKey in ccDefsJson.Keys)
            {
                var ccNum = int.Parse(ccDefKey.Substring(CC_DEF_INT_POS));
                var ccDefJson = ccDefsJson[ccDefKey];
                var ccDef = new CcDef(ccNum);

                ccDef.SetLabel(ccDefJson[JsonKeys.LABEL]);
                ccDef.SetMinValue(ccDefJson[JsonKeys.MIN_VAL]);
                
                if(ccDefJson[JsonKeys.MAX_VAL] != null)
                    ccDef.SetMaxValue(ccDefJson[JsonKeys.MAX_VAL]);
                
                ccDef.SetStartValue(ccDefJson[JsonKeys.START_VAL]);

                instrumentDef.CcDefs[ccNum] = ccDef;

                Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() CC DEF - Label: {ccDef.Label}, ccNum: {ccDef.CcNum}</color>");
            }

            //NOTE ROWS
            var noteRowsJson = json[JsonKeys.ROW_DEFS];
            foreach (string noteDefKey in noteRowsJson.Keys)
            {
                var noteRowJson = noteRowsJson[noteDefKey];
                var noteRow = new NoteRowDef();
                noteRow.SetNote(new Note(noteDefKey));
                noteRow.SetLabel(noteRowJson[JsonKeys.LABEL]);
                noteRow.SetAlwaysShow(noteRowJson[JsonKeys.ALWAYS_SHOW]);

                instrumentDef.NoteRowDefs[noteRow.Note.Id] = noteRow;

                Debug.LogWarning($"<color=\"aqua\">InitAppCommand.ParseInstrument() {instrumentDef.Name} - NOTE ROW DEF - Note: {noteRow.Note.Name}, Label: {noteRow.Label}, AlwaysShow: {noteRow.AlwaysShow}, </color>");
            }

            return instrumentDef;
        }

        private TrackValueDef ParseTrackValueDef(int index, JSONNode json)
        {
            TrackValueType trackValueType = TrackValueType.Empty;

            if (json[JsonKeys.MIDI_CC] != null)
                trackValueType = TrackValueType.MidiCC;
            else if (json[JsonKeys.TRACK_CONTROL] != null)
                trackValueType = TrackValueType.TrackControl;

            if (trackValueType == TrackValueType.Empty)
                return null;

            var trackValueDef = new TrackValueDef();
            trackValueDef.SlotIndex = index;
            trackValueDef.Type = trackValueType;

            switch (trackValueType)
            {
                case TrackValueType.MidiCC:
                    trackValueDef.MidiCC = json[JsonKeys.MIDI_CC];
                    trackValueDef.Label = json[JsonKeys.LABEL];
                    break;
                case TrackValueType.TrackControl:
                    trackValueDef.TrackControl = InstrumentUtils.TrackControlFromJson(json);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return trackValueDef;
        }

        public string BuildCkiFile(List<InstrumentDef> instruments)
        {
            var instrumentData = new JSONObject();

            foreach (var instrument in instruments)
            {
                var instrumentJson = new JSONObject();
                instrumentJson.Add(JsonKeys.MIDI_PORT, new JSONNumber(instrument.MidiPort));
                instrumentJson.Add(JsonKeys.MIDI_CHAN, new JSONNumber(instrument.MidiChannel));
                instrumentJson.Add(JsonKeys.DEFAULT_NOTE, new JSONString(instrument.DefaultNote.Name));
                instrumentJson.Add(JsonKeys.DEFAULT_PATT, new JSONString(instrument.DefaultPattern.ToString()));
                instrumentJson.Add(JsonKeys.POLY_SPREAD, new JSONBool(instrument.PolySpread));
                instrumentJson.Add(JsonKeys.NO_XPOSE, new JSONBool(instrument.NoXpose));
                instrumentJson.Add(JsonKeys.NO_FTS, new JSONBool(instrument.NoFts));
                instrumentJson.Add(JsonKeys.MULTI, new JSONBool(instrument.Multi));

                //track values
                var trackValuesJson = new JSONObject();
                foreach (var trackValuePair in instrument.TrackValues)
                {
                    var trackValue = trackValuePair.Value;
                    var trackValueId = trackValuePair.Key;

                    var trackValueJson = new JSONObject();
                    
                    //value
                    switch (trackValue.Type)
                    {
                        case TrackValueType.Empty:
                            //do not serialize empty track value.
                            continue;
                        case TrackValueType.MidiCC:
                            trackValueJson.Add(JsonKeys.MIDI_CC, new JSONNumber(trackValue.MidiCC));
                            break;
                        case TrackValueType.TrackControl:
                            trackValueJson.Add(JsonKeys.TRACK_CONTROL, new JSONString(trackValue.TrackControl.ToDefString()));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    //label
                    if(trackValue.Label != null)
                        trackValueJson.Add(JsonKeys.LABEL, new JSONString(trackValue.Label));
                    
                    trackValuesJson.Add("slot_" + trackValueId, trackValueJson);
                }

                instrumentJson.Add(JsonKeys.TRACK_VALUES, trackValuesJson);
                
                //cc defs
                var ccDefsJson = new JSONObject();
                foreach (var CcDef in instrument.CcDefs.Values)
                {
                    var ccDefJson = new JSONObject();
                    
                    ccDefJson.Add(JsonKeys.LABEL, new JSONString(CcDef.Label));
                    ccDefJson.Add(JsonKeys.MIN_VAL, new JSONNumber(CcDef.MinValue));
                    ccDefJson.Add(JsonKeys.MAX_VAL, new JSONNumber(CcDef.MaxValue));
                    ccDefJson.Add(JsonKeys.START_VAL, new JSONNumber(CcDef.MaxValue));
                    ccDefsJson.Add("CC_" + CcDef.CcNum, ccDefJson);
                } 
                
                instrumentJson.Add(JsonKeys.CC_DEFS, ccDefsJson);

                //row defs
                var rowDefsJson = new JSONObject();
                foreach (var rowDef in instrument.NoteRowDefs.Values)
                {
                    var rowDefJson = new JSONObject();
                    rowDefJson.Add(JsonKeys.LABEL, new JSONString(rowDef.Label));
                    rowDefJson.Add(JsonKeys.ALWAYS_SHOW, new JSONBool(rowDef.AlwaysShow));
                    rowDefsJson.Add(rowDef.Note.Name, rowDefJson);
                }

                instrumentJson.Add(JsonKeys.ROW_DEFS, rowDefsJson);

                instrumentData.Add(instrument.Name, instrumentJson);
            }
            
            var ckiJson = new JSONObject();
            ckiJson.Add(JsonKeys.INSTRUMENT_DATA, instrumentData);

            return ckiJson.ToString();
        }
    }
}