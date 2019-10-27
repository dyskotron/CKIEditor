using System;
using System.Collections.Generic;
using Framewerk.Managers;
using UnityEngine;

namespace CKIEditor.Model
{
    public class DefinitionSO<TEnum> : ScriptableObject where TEnum : Enum
    {
        [HideInInspector]
        public TEnum Id;
    }

    public abstract class ScriptableObjectDefintions<TScriptable, TEnum> where TScriptable : DefinitionSO<TEnum> where TEnum : Enum
    {
        [Inject] public IAssetManager AssetManager { get; set; }
        
        protected abstract string Path { get; }

        private Dictionary<TEnum, TScriptable> _definitionById = new Dictionary<TEnum, TScriptable>();
        private TScriptable[] _allDefinitions;
        
        [PostConstruct]
        public void PostConstruct()
        {
            foreach (var enumValue in Enum.GetValues(typeof(TEnum)))
            {
                //so Enums can contain None (= -1) value
                if((int)enumValue < 0)
                    continue;
                
                var name = Enum.GetName(typeof(TEnum), enumValue);
                TEnum value = (TEnum) enumValue;
                
                var actionCardDefinition = AssetManager.GetAsset<TScriptable>(Path + name);
                if (actionCardDefinition != null)
                {
                    actionCardDefinition.Id = value;
                    _definitionById[value] = actionCardDefinition;                    
                }
                else
                    Debug.LogErrorFormat("<color=\"yellow\">{0}.Init - Failed to load definition with id: {1}</color>", this, name);
            }  
            
            _allDefinitions = new TScriptable[_definitionById.Values.Count];
            _definitionById.Values.CopyTo(_allDefinitions, 0);
        }

        public TScriptable[] GetAllDefinitions()
        {
            return  _allDefinitions;
        }

        public TScriptable GetDefinitionById(TEnum id)
        {
            if (_definitionById.ContainsKey(id))
                return _definitionById[id];

            Debug.LogErrorFormat("<color=\"red\">{0}.GetDefinitionById - Missing definition with id: {1}</color>", this, id);
            return null;

        }
    }
}