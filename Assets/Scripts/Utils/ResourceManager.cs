using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class ResourceManager
    {
        private static Dictionary<string, Sprite> _dictionaryCache = new Dictionary<string, Sprite>();

        public static Dictionary<string, Sprite> DictionaryCache
        {
            get { return _dictionaryCache; }
            set { _dictionaryCache = value; }
        }
        
        /// <summary>
        /// Get file from given location
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Sprite Get(string path)
        {
            lock (DictionaryCache)
            {
                Sprite sprite = null;
                try
                {
                    DictionaryCache.TryGetValue(path, out sprite);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured while loading resource from dictionary " + e.Message);
                }
                if (sprite == null)
                {
                    try
                    {
                        sprite = Resources.Load<Sprite>(path);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error occured while loading resource from file " + e.Message);
                    }
                    try
                    {
                        DictionaryCache.Add(path, sprite);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error occured while adding resource to dictionary " + e.Message);
                    }
                }
                return sprite;
            }
        }

    }
}
