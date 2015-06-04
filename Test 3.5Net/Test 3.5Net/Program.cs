using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Test_3._5Net
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                ETest.Test.GetAttributeOfType<Attrib>().Ok.ToString()
                );
            Console.ReadKey();
        }
    }

    enum ETest
    {
        [Attrib2(false)]
        [Attrib(true)]
        Test
    }

    static class Test
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
            object[] attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }

    class Attrib : Attribute
    {
        public bool Ok { get; set; }

        public Attrib(bool ok)
        {
            Ok = ok;
        }
    }
    class Attrib2 : Attribute
    {
        public bool Ok { get; set; }

        public Attrib2(bool ok)
        {
            Ok = ok;
        }
    }
}
