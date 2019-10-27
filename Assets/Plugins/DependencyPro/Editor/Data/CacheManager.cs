using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace AssetUsageFinder
{
    public class CacheManager:IDisposable
    {
        private const int CacheVersion = 3;
         
        private static string Root
        {
            get { return Application.temporaryCachePath; }
        }

        private static string Path
        {
            get { return string.Format("{0}/AssetUsageFinder.cache", Root); }
        }

        private HashSet<string> _used;
        private SerializableCache _cache;
        private static SerializableCache _cacheCache;

        private static SerializableCache Deserialize()
        {
            if (_cacheCache != null && _cacheCache.Valid(CacheVersion))
                return _cacheCache;

            using (var file = File.Open(Path, FileMode.OpenOrCreate))
            {
                var bf = new BinaryFormatter();
                try
                {
                    var serializedCache = (SerializableCache) bf.Deserialize(file);
                    return serializedCache.Valid(CacheVersion) 
                        ? serializedCache 
                        : new SerializableCache(CacheVersion);
                }
                catch (Exception)
                {
                    return new SerializableCache(CacheVersion);
                }
            }
        }

        private static void Serialize(SerializableCache cache)
        {
            using (var file = File.Open(Path, FileMode.OpenOrCreate))
            {
                var bf = new BinaryFormatter();
                if (cache == null)
                    cache = new SerializableCache(CacheVersion);
                bf.Serialize(file, cache);
            }
        }

        public CacheManager()
        {
            _used = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            _cache = Deserialize();
            _cacheCache = _cache;
        }

        public void Dispose()
        {
            Serialize(_cache);
        }

        internal AssetDependencies Get(string path, string guid)
        {
            _used.Add(guid);
            return _cache.Get(path, guid);
        }
    }
}