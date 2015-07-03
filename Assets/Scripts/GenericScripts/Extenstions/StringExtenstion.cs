using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public static class StringExtenstion
{
    public static string SplitCamelCase(this string input)
    {
        return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.None).Trim();
    }
}
