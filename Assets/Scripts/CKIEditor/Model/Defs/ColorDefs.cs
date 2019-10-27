using UnityEngine;

namespace CKIEditor.Model.Defs
{
    public enum ColorName
    {
        Red,
        GreyDark,
        GreyLight,
        BackgroundDark,
        BackroundLight
    }
    
    [CreateAssetMenu(menuName = "CkiEditor/ColorDefinition")]
    public class ColorDefSO : DefinitionSO<ColorName>
    {
        public Color Color;
    }
    
    public class ColorDefs : ScriptableObjectDefintions<ColorDefSO, ColorName>
    {
        protected override string Path => "";
    }
}