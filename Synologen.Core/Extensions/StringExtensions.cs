using System;
using System.Linq;
using System.Text.RegularExpressions;
using Spinit.Extensions;

namespace Spinit.Wpc.Synologen.Core.Extensions
{
	public static class StringExtensions
	{
		public static string AppendUrl(this string value, string urlToAppend)
		{
			return string.Concat(value.TrimEnd('/'), "/", urlToAppend.TrimStart('/'));
		}

		public static string FormatPersonalIdNumber(this string value)
		{
			if(value.Length != 12)
			{
				throw new ArgumentException("Invalid length (personal Id number length needs to be 12 digits long)", "value");
			}
			return value.Insert(8, "-");
		}

		public static byte[] ToByteArray(this string hexString)
		{
			if(String.IsNullOrEmpty(hexString)) return new byte[0];
			return Enumerable.Range(0, hexString.Length)
				.Where(x => 0 == x % 2)
				.Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
				.ToArray();
		}

		public static string RegexReplace(this string input, string pattern, string replacement)
		{
			if(input == null) return null;
			if(input == string.Empty) return string.Empty;
			return Regex.Replace(input, pattern, replacement);
		}

		public static string RegexRemove(this string input, string pattern)
		{
			return input.RegexReplace(pattern, String.Empty);
		}

		public static bool ContainsAny(this string value, params string[] comparisonValues)
		{
			return comparisonValues.Any(value.Contains);
		}

		public static int? ToNullableInt(this string value)
		{
			if (String.IsNullOrEmpty(value)) return null;
			return value.ToInt();
		}

		///// <summary>
		///// Replaces tokens in format string with given replacements
		///// Ex: "{Token1}def{Token2}.ReplaceWith(new { Token1 = "abc", Token2 = "ghi"}) would return the string "abcdefghi"
		///// </summary>
		///// <param name="format">Format that may or may not contain tokens</param>
		///// <param name="tokenReplacements">Anonymous object containing tokens to replace matching tokens in format</param>
		///// <returns></returns>
		//public static string ReplaceWith(this string format, object tokenReplacements)
		//{
		//    var output = format;
		//    var tokens = Regex.Matches(format, "{[A-�0-9]+?}");
		//    foreach (var tokenMatch in tokens)
		//    {
		//        var token = tokenMatch.ToString();
		//        var tokenName = token.Trim(new[] {'{', '}'});
		//        var matchingProperty = tokenReplacements.GetAnonymousPropertyValue<object>(tokenName);
		//        if(!matchingProperty.FoundProperty) continue;
		//        var replacement = matchingProperty.With(x => x.PropertyValue).Return(x => x.ToString(), string.Empty);
		//        output = output.Replace(token, replacement);
		//    }
		//    return output;
		//}
	}
}