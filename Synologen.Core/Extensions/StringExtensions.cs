namespace Spinit.Wpc.Synologen.Core.Extensions
{
	public static class StringExtensions
	{
		public static decimal ToDecimalOrDefault(this string textValue, decimal defaultValue)
		{
			decimal output;
			return decimal.TryParse(textValue, out output) ? output : defaultValue;
		}

		public static decimal ToDecimalOrDefault(this string textValue)
		{
			decimal output;
			return decimal.TryParse(textValue, out output) ? output : default(decimal);
		}

		public static decimal ToDecimal(this string textValue)
		{
			return decimal.Parse(textValue);
		}

		public static int ToIntOrDefault(this string textValue, int defaultValue)
		{
			int output;
			return int.TryParse(textValue, out output) ? output : defaultValue;	
		}

		public static int ToIntOrDefault(this string textValue)
		{
			int output;
			return int.TryParse(textValue, out output) ? output : default(int);
		}

		public static int ToInt(this string textValue)
		{
			return int.Parse(textValue);
		}
	}
}