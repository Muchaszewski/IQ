using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    public static class OperationUtils
    {
        /*
         * Operation utils to make some of editor stuff work, but feel free to use it in your project!
         */

        /// <summary>
        /// Convert object of one type to it children
        /// <para>Very slow operation!</para>
        /// </summary>
        public static T1 ConvertFrom<T1, T2>(this T1 destinationObject, T2 parentObject)
            where T1 : class
            where T2 : class
        {
            PropertyInfo[] srcFields = parentObject.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            PropertyInfo[] destFields = destinationObject.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var property in srcFields)
            {
                var dest = destFields.FirstOrDefault(x => x.Name == property.Name);
                if (dest != null && dest.CanWrite)
                    dest.SetValue(destinationObject, property.GetValue(parentObject, null), null);
            }

            return destinationObject;
        }

        /// <summary>
        /// Copy component into component
        /// </summary>
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        /// <summary>
        /// Extend Add Component
        /// </summary>
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }
    }
}
