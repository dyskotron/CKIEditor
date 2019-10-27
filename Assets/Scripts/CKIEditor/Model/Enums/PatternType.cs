namespace CKIEditor.Model
{
    public enum PatternType
    {
        P3,
        CK,
        Sel
    }
    
    public static class PatternTypeExtensions
    {
        public static PatternType FromString(this PatternType myEnum, string value)
        {
            switch (value)
            {
                case "CK":
                    return PatternType.CK;
                case "P3":
                    return PatternType.P3;
                case "Sel":
                default:
                    return PatternType.Sel;
                    
            }
        }
    }
}