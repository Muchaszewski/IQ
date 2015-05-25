namespace InventoryQuest
{
    public static class StringExtensions
    {
        public static string TrimStart(this string str, string valueToTrim)
        {
            if (valueToTrim.Length > str.Length)
            {
                return str;
            }

            for (var i = 0; i < valueToTrim.Length; i++)
            {
                if (str[i] != valueToTrim[i])
                {
                    return str;
                }
            }

            return str.Substring(valueToTrim.Length);
        }
    }
}