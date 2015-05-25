using System;
using System.ComponentModel;
using System.Reflection;

namespace InventoryQuest
{
    public static class AttributeHelper
    {
        /// <summary>
        ///     Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            Type type = enumVal.GetType();
            MemberInfo[] memInfo = type.GetMember(enumVal.ToString());
            object[] attributes = memInfo[0].GetCustomAttributes(typeof (T), false);
            return (attributes.Length > 0) ? (T) attributes[0] : null;
        }

        /// <summary>
        ///     Get an desctiption attribute
        /// </summary>
        /// <param name="value">Enum form witch description should be taken</param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof (DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }
    }
}