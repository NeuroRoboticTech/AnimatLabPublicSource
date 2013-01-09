// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Text;
using System.Drawing;
using System.Globalization;
using System.ComponentModel;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Collection of static helper methods to convert between types.
	/// </summary>
    public sealed class ConversionHelper
    {
		// Improve performance by caching the converters and type objects, 
		// rather than keep recreating them each time a conversion is required
		private static SizeConverter _sc = new SizeConverter();
		private static PointConverter _pc = new PointConverter();
		private static DecimalConverter _dc = new DecimalConverter();
		private static Type _stringType = Type.GetType("System.String");

		// Prevent instance from being created.
		private ConversionHelper() {}

		/// <summary>
		/// Convert a Size instance into a string representation.
		/// </summary>
		/// <param name="size">Size instance to be converted.</param>
		/// <returns>String representation of the Size instance.</returns>
		public static string SizeToString(Size size)
		{
			return (string)_sc.ConvertTo(null, CultureInfo.InvariantCulture, size, _stringType);
		}

		/// <summary>
		/// Convert a string representation to a Size instance.
		/// </summary>
		/// <param name="str">String representation of the Size instance.</param>
		/// <returns>Size instance created from string representation.</returns>
		public static Size StringToSize(string str)
		{
			return (Size)_sc.ConvertFrom(null, CultureInfo.InvariantCulture, str);
		}

		/// <summary>
		/// Convert a Point instance into a string representation.
		/// </summary>
		/// <param name="point">Point instance to be converted.</param>
		/// <returns>String representation of the Point instance.</returns>
		public static string PointToString(Point point)
		{
			return (string)_pc.ConvertTo(null, CultureInfo.InvariantCulture, point, _stringType);
		}

		/// <summary>
		/// Convert a string representation to a Point instance.
		/// </summary>
		/// <param name="str">String representation of the Point instance.</param>
		/// <returns>Point instance created from string representation.</returns>
		public static Point StringToPoint(string str)
		{
			return (Point)_pc.ConvertFrom(null, CultureInfo.InvariantCulture, str);
		}

		/// <summary>
		/// Convert a Decimal value into a string representation.
		/// </summary>
		/// <param name="value">Decimal value to be converted.</param>
		/// <returns>String representation of the Decimal instance.</returns>
		public static string DecimalToString(Decimal value)
		{
			return (string)_dc.ConvertTo(null, CultureInfo.InvariantCulture, value, _stringType);
		}

		/// <summary>
		/// Convert a string representation to a Decimal instance.
		/// </summary>
		/// <param name="str">String representation of the Decimal instance.</param>
		/// <returns>Decimal instance created from string representation.</returns>
		public static Decimal StringToDecimal(string str)
		{
			try
			{
				return (Decimal)_dc.ConvertFrom(null, CultureInfo.InvariantCulture, str);
			}
			catch(NotSupportedException)
			{
				// Failed to convert using invariant culture, try again using local culture.
				// This is done because the use of the invariant was added as a fix and so 
				// old configurations will have culture specific values.
				return (Decimal)_dc.ConvertFrom(null, CultureInfo.CurrentCulture, str);
			}
		}
	}
}
