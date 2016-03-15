using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PeanutButter.Utils
{
    public static class StringExtensions
    {
        public static string RegexReplace(this string input, string pattern, string replaceWith)
        {
            var regex = new Regex(pattern);
            return regex.Replace(input, replaceWith);
        }

        public static string Or(this string input, string alternative)
        {
            return string.IsNullOrEmpty(input) ? alternative : input;
        }

        private static readonly string[] _truthy = {"yes", "y", "1", "true"};
        public static bool AsBoolean(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            return _truthy.Any(item => item == input.ToLower());
        }

        public static bool ContainsOneOf(this string src, params string[] search)
        {
            if (src == null)
                return false;
            src = src.ToLower();
            return search.Any(s => src.Contains(s.ToLower()));
        }

        public static bool StartsWithOneOf(this string src, params string[] search)
        {
            if (src == null)
                return false;
            src = src.ToLower();
            return search.Any(s => src.StartsWith(s.ToLower()));
        }

        public static byte[] AsBytes(this string src)
        {
            return src == null
                    ? null
                    : Encoding.UTF8.GetBytes(src);
        }

        public static bool IsInteger(this string src)
        {
            int value;
            return int.TryParse(src, out value);
        }

        public static int AsInteger(this string value)
        {
            int result;
            var interestingPart = GetLeadingIntegerCharsFrom(value ?? string.Empty);
            int.TryParse(interestingPart, out result);
            return result;
        }

        private static string GetLeadingIntegerCharsFrom(string value)
        {
            var collected = new List<string>();
            var intMarker = 0;
            value.ForEach(c =>
            {
                if (intMarker > 1)
                    return;
                var asString = c.ToString();
                if ("1234567890".Contains(asString))
                {
                    intMarker = 1;
                    collected.Add(asString);
                }
                else if (intMarker == 1)
                {
                    intMarker++;
                }
            });
            return collected.JoinWith(string.Empty);
        }
    }
}