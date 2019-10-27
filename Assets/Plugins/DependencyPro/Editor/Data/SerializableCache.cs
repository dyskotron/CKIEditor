using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace AssetUsageFinder
{
    [Serializable]
    public class SerializableCache
    {
        public SerializableCache(int version)
        {
            Version = version;
        }

        public int Version;
        internal Dictionary<string, AssetDependencies> Dict = new Dictionary<string, AssetDependencies>(StringComparer.InvariantCultureIgnoreCase);
        private static AssetDependencies RebuildCache(string path)
        {
            var dependencyPaths = AssetDatabase.GetDependencies(path, true);
            var guids = dependencyPaths.Select(p => AssetDatabase.AssetPathToGUID(p)).ToArray();
            return new AssetDependencies
            {
                DependencyGuids = guids,
                DependencyHash = AssetDatabase.GetAssetDependencyHash(path),
            };
        }

        internal AssetDependencies Get(string path, string guid)
        {
            AssetDependencies res;
            if (Dict.TryGetValue(guid, out res))
            {
                var assetDependencyHash = AssetDatabase.GetAssetDependencyHash(path);
                if (!assetDependencyHash.isValid || res.DependencyHash == assetDependencyHash)
                    return res;

                res = RebuildCache(path);
                Dict[guid] = res;
                return res;
            }

            res = RebuildCache(path);
            Dict.Add(guid, res);
            return res;
        }

        public bool Valid(int ver)
        {
            return Dict != null && ver == Version;
        }
    }
}