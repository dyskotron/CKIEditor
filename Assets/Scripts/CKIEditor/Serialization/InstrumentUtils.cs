using System;
using CKIEditor.Model;
using SimpleJSON;

namespace CKIEditor.Serialization
{
    public static class InstrumentUtils
    {
        public static TrackControlType TrackControlFromJson(JSONNode json)
        {
            return TrackControlFromString(json[JsonKeys.TRACK_CONTROL]);
        }
        
        public static TrackControlType TrackControlFromString(string value)
        {
            switch(value)   
            {
                case "pgm":
                    return TrackControlType.Program;
                case "quant%":
                    return TrackControlType.Quant;
                case "note%":
                    return TrackControlType.NotePerc;
                case "noteC":
                    return TrackControlType.NoteC;
                case "velo%":
                    return TrackControlType.VeloPerc;
                case "veloC":
                    return TrackControlType.VeloC;
                case "leng%":
                    return TrackControlType.LengPerc;
                case "tbase":
                    return TrackControlType.Tbase;
                case "xpos":
                    return TrackControlType.Xpos;
                case "octave":
                    return TrackControlType.Octave;
                case "knob1":
                    return TrackControlType.Knob1;
                case "knob2":
                    return TrackControlType.Knob2;
                case "fts-R":
                    return TrackControlType.FtsR;
                case "fts-S":
                    return TrackControlType.FtsS;
                case "reich":
                    return TrackControlType.Reich;
                default:
                    throw new ArgumentOutOfRangeException("TrackControlType", value, "Unsupported Value");
            } 
        }
    }
}