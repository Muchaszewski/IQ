using System;

namespace InventoryQuest.Utils
{
    /// <summary>
    ///     Change format of displayed stat
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayFormatAttribute : Attribute
    {
        /// <summary>
        ///     Change format of displayed stat
        /// </summary>
        /// <param name="format">eg format: "0.00"</param>
        public DisplayFormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; set; }
    }
}