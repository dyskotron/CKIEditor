using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetUsageFinder
{
    [Serializable]
    internal sealed class InSceneDependency : DependencyBase
    {
        [SerializeField] private string _scenePath;

        public string ScenePath
        {
            get { return _scenePath; }
        }

        public InSceneDependency(Object target, string scenePath)
        {
            Target = new SearchTarget(target, scenePath);
            _scenePath = scenePath;

            var name = target is Component ? target.GetType().Name : target.name;

            TabContent = new GUIContent
            {
                text = name,
                image = AssetPreview.GetMiniTypeThumbnail(Target.Target.GetType()) ?? AssetPreview.GetMiniThumbnail(Target.Target)
            };

            Update();
        }

        public override void Update()
        {
            Dependencies = Group(FindDependencies.InScenePro(Target)).ToArray();
        }


        public override DependencyBase Nest(Object o)
        {
            return new InSceneDependency(o, _scenePath) {Parent = this};
        }
    }
}