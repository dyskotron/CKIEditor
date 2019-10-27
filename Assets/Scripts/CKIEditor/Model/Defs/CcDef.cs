using System;
using Framewerk.UI.List;
using UnityEngine;

namespace CKIEditor.Model.Defs
{
    public class CcDef : IListItemDataProvider
    {
        public const int MIN_CC_VALUE = 0;
        public const int MAX_CC_VALUE = 127;
        
        public int CcNum{ get; private set; }
        public string Label { get; private set; }
        public int MinValue{ get; private set; }
        public int MaxValue{ get; private set; }
        public int StartValue{ get; private set; }

        public CcDef(int ccNum)
        {
            SetCcNum(ccNum);
            StartValue = MinValue = MIN_CC_VALUE;
            MaxValue = MAX_CC_VALUE;
        }

        public void SetCcNum(int value)
        {
            if (value != Mathf.Clamp(value, MIN_CC_VALUE, MAX_CC_VALUE))
            {
                throw new ArgumentOutOfRangeException("CcNum" ,value, "Allowed range is 0 - 127");
            }
            
            CcNum = value;
        }
        
        public void SetLabel(string value)
        {
            //TODO: restrict chars - //[-A-Za-z0-9()#. $@!&]
            Label = value.Substring(0,Math.Min(value.Length, CkiConsts.CC_NAME_CHARACTER_LIMIT));
        }

        public void SetMinValue(int value)
        {
            MinValue = Mathf.Clamp(value, MIN_CC_VALUE, MaxValue);
            
            MaxValue = Mathf.Clamp(MaxValue, MinValue, MAX_CC_VALUE);
            StartValue = Mathf.Clamp(StartValue, MinValue, MaxValue);
        }
        
        public void SetMaxValue(int value)
        {
            MaxValue = Mathf.Clamp(value, MinValue, MAX_CC_VALUE);
            
            MinValue = Mathf.Clamp(MinValue, MIN_CC_VALUE, MaxValue);
            StartValue = Mathf.Clamp(StartValue, MinValue, MaxValue);
        }
        
        public void SetStartValue(int value)
        {
            StartValue = Mathf.Clamp(value, MinValue, MaxValue);
        }
    }
}