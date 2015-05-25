using System;
using System.Linq.Expressions;
using System.Reflection;

namespace InventoryQuest
{
    /// <summary>
    ///     Set long/short name for this target
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public class NameAttribute : Attribute
    {
        private string _LongName = "NO_NAME";
        private string _ShortName = "NO_NAME";

        public NameAttribute()
        {
        }

        /// <summary>
        ///     Create name attribute with given <see cref="longName" /> as long and short name.
        /// </summary>
        public NameAttribute(string longName)
        {
            LongName = ShortName = longName;
        }

        /// <summary>
        ///     Create name attribute with given long and short name.
        /// </summary>
        public NameAttribute(string longName, string shortName)
        {
            LongName = longName;
            ShortName = shortName;

            GetNameAttribute(() => LongName);
        }

        /// <summary>
        ///     long name of this member.
        /// </summary>
        public string LongName
        {
            get { return _LongName; }
            private set { _LongName = value; }
        }

        /// <summary>
        ///     Short name of this member.
        /// </summary>
        public string ShortName
        {
            get { return _ShortName; }
            private set { _ShortName = value; }
        }

        /// <summary>
        ///     Returns <see cref="T:NameAttribute" /> from given member. If there is no <see cref="NameAttribute" /> in given
        ///     member, new
        ///     will be created with member name as long and short name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="member">Member, to get attribute from.</param>
        public static NameAttribute GetNameAttribute<T>(Expression<Func<T>> member)
        {
            var me = member.Body as MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("member body must be of type MemberExpression");
            }

            return GetNameAttribute(me.Member);
        }

        /// <summary>
        ///     Returns <see cref="T:NameAttribute" /> from given member. If there is no <see cref="NameAttribute" /> in given
        ///     member, new
        ///     will be created with member name as long and short name.
        /// </summary>
        /// <param name="member">Member, to get attribute from.</param>
        public static NameAttribute GetNameAttribute(MemberInfo member)
        {
            NameAttribute attrib = member.GetCustomAttributes(typeof(NameAttribute), false)[0] as NameAttribute;
            if (attrib == null)
            {
                // If there is no NameAttribute for this member, create one with default name.
                return new NameAttribute(member.Name);
            }

            return attrib;
        }
    }
}