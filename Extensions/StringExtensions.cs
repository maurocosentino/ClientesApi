using System;

namespace ClienteApi.Utils;

public static class StringExtensions
{
    public static string Clean(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;
            
        return input.Trim();
    }
}
