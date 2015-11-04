using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;

        /// <summary>
        /// Returns the Instance of this singleton.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (_instance == null)
                    {
                        Debug.LogError("An _instance of " + typeof(T) +
                           " is needed in the scene, but there is none.");
                    }
                }

                return _instance;
            }
        }
    }
}
