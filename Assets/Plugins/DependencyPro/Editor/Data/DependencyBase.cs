using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetUsageFinder
{
    [Serializable]
    internal abstract class DependencyBase
    {
        public SearchTarget Target;
        public ResultRow[] Dependencies;
        public GUIContent TabContent;
        public DependencyBase Parent;

        public List<DependencyBase> Parents()
        {
            var res = new List<DependencyBase>();
            for (var cur = this; cur != null; cur = cur.Parent)
                res.Add(cur);
            return res;
        }

        public IEnumerable<ResultRow> Group(IEnumerable<ResultRow> inScenePro)
        {
            ResultRow cur = null;
            var res = new List<ResultRow>();
            foreach (var source in inScenePro.OrderBy(t => t.Main.GetInstanceID()))
            {
                if (cur == null || cur.Main != source.Main)
                {
                    cur = source;
                    res.Add(cur);
                }
                else
                {
                    cur.Properties.AddRange(source.Properties);
                }
            }
            return res;
        }

        public abstract void Update();
        public abstract DependencyBase Nest(Object o);
    }
}