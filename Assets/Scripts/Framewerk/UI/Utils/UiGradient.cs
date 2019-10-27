using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framewerk.UI.Utils
{
    public enum GradientType {
        Horizontal,
        Vertical
    }
    
    public enum BlendMode {
        Override,
        Add,
        Multiply
    }
    
    /// Based on Unity UI Extensions Gradient effect (http://bit.ly/unityuiextensions)
    /// <summary>
    /// - UI Element wich creates its own mesh with gradient.
    /// - Adjustable vertex count for displaying complex gradients with more colors.
    /// - Uses Unity's Gradient class to define the colors
    /// 
    /// For more colors / uneven ratios set QuadCount parameter high enough but not higher than needed, less quads = better performance
    /// </summary>[AddComponentMenu("UI/UiGradient")]
    public class UiGradient : MaskableGraphic
    {
        [Header("Gradient Settings")]
        [SerializeField]
        private Gradient _effectGradient = new Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.white, 1) } };
        
        [SerializeField]
        private GradientType _gradientType;
        
        [SerializeField]
        private BlendMode _blendMode = BlendMode.Multiply;
        
        [Range(1, 100)]
        [SerializeField]
        private int _quadCount = 1;

        #region Properties
        
        public UnityEngine.Gradient EffectGradient {
            get { return _effectGradient; }
            set { _effectGradient = value; }
        }

        public GradientType GradientType {
            get { return _gradientType; }
            set { _gradientType = value; }
        }
        
        public BlendMode BlendMode {
            get { return _blendMode; }
            set { _blendMode = value; }
        }
        
        public int QuadCount {
            get { return _quadCount; }
            set { _quadCount = value; }
        }
        
        #endregion
        
        private int _ySize = 1;
        private int _xSize = 1;
        
        private UIVertex[] _vertexes;
        private int[] _indices;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (canvas == null)
            {
                base.OnPopulateMesh(vh); 
                return;
            }

            if (_gradientType == GradientType.Horizontal)
            {
                _xSize = _quadCount;
                _ySize = 1;
            }
            else
            {
                _xSize = 1;
                _ySize = _quadCount;    
            }

            vh.Clear();

            CalculateVertexes();
            CalculateIndices();
            
            vh.AddUIVertexStream(new List<UIVertex>(_vertexes), new List<int>(_indices));
        }

        private void CalculateVertexes()
        {
            _vertexes = new UIVertex[(_ySize + 1 )* (_xSize + 1)];
            
            var r = GetPixelAdjustedRect();

            var quadWidth = r.width / _xSize;
            var quadHeight = r.height / _ySize;
            float time;
            
            for (int y = 0, i = 0; y <= _ySize; y++)
            {
                for (var x = 0; x <= _xSize; x++, i++)
                {
                    _vertexes[i].position.x = r.x + x * quadWidth;
                    _vertexes[i].position.y = r.y + y * quadHeight;
                    
                    time = _gradientType == GradientType.Horizontal ? x / (float)_xSize : y / (float)_ySize;
                    _vertexes[i].color = BlendColor(color, _effectGradient.Evaluate(time));
                }   
            }
        }
        
        private void CalculateIndices()
        {
            _indices = new int[_ySize * _xSize * 6];
            
            for (int ti = 0, vi = 0, y = 0; y < _ySize; y++, vi++) {
                for (int x = 0; x < _xSize; x++, ti += 6, vi++)
                {
                    _indices[ti] = vi;
                    _indices[ti + 3] = _indices[ti + 2] = vi + 1;
                    _indices[ti + 4] = _indices[ti + 1] = vi + _xSize + 1;
                    _indices[ti + 5] = vi + _xSize + 2;
                }
            }
        }
        
        private Color BlendColor(Color colorA, Color colorB) {
            switch(BlendMode) {
                default: return colorB;
                case BlendMode.Add: return colorA + colorB;
                case BlendMode.Multiply: return colorA * colorB;
            }
        }
    }
}