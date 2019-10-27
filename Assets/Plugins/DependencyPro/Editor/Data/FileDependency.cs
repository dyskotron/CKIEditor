using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetUsageFinder
{
    [Serializable]
    internal sealed class FileDependency : DependencyBase
    {
        public class Pair
        {
            public string NicifiedPath;
            public string Path;
        }
        public Pair[] ScenePaths;

        public FileDependency(Object target)
        {
            Target = new SearchTarget(target);
            Update();

            TabContent = new GUIContent
            {
                text = target.name,
                image = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(Target.Target))
            };
        }


        public override void Update()
        {
            var files = FindDependencies.FilesThatReference(Target);
            Dependencies = Group(files.Where(f => !(f.Target is SceneAsset)))
                .OrderBy(t => t.LabelContent.text, StringComparer.Ordinal)
                .ToArray();
//            ScenePaths = FindDependencies.ScenesThatContain(Target.Target).Select(p => new Pair { Path = p, NicifiedPath = p.Replace("Assets/", string.Empty) }).ToArray();
        }

        public override DependencyBase Nest(Object o)
        {
            return new FileDependency(o) {Parent = this};
        }
    }
}