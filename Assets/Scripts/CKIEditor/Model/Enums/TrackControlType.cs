using System;

namespace CKIEditor.Model
{
    public enum TrackControlType
    {
        Program,
        Quant,
        NotePerc,
        NoteC,
        VeloPerc,
        VeloC,
        LengPerc,
        Tbase,
        Xpos,
        Octave,
        Knob1,
        Knob2,
        FtsR,
        FtsS,
        Reich
        
    }

    public static class TrackControlTypeExtensions
    {
        public static string ToDefString(this TrackControlType  trackControl)
        {
            switch (trackControl)
            {
                case TrackControlType.Program:
                    return "pgm";
                case TrackControlType.Quant:
                    return "quant%";
                case TrackControlType.NotePerc:
                    return "note%";
                case TrackControlType.NoteC:
                    return "noteC";
                case TrackControlType.VeloPerc:
                    return "velo%";
                case TrackControlType.VeloC:
                    return "veloC";
                case TrackControlType.LengPerc:
                    return "leng%";
                case TrackControlType.Tbase:
                    return "tbase";
                case TrackControlType.Xpos:
                    return "xpos";
                case TrackControlType.Octave:
                    return "octave";
                case TrackControlType.Knob1:
                    return "knob1";
                case TrackControlType.Knob2:
                    return "knob2";
                case TrackControlType.FtsR:
                    return "fts-R";
                case TrackControlType.FtsS:
                    return "fts-S";
                case TrackControlType.Reich:
                    return "reich";
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackControl), trackControl, null);
            }    
        }
    }
}