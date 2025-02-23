﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Windows.UI
{
	[StructLayout(LayoutKind.Explicit)]
	public partial struct Color : IFormattable
	{
		/// <summary>
		/// Alias individual fields to avoid bitshifting and GetHashCode / compare costs
		/// </summary>
		[FieldOffset(0)]
		private uint _color;

		//
		// This memory layout assumes that the system uses little-endianness.
		//
		[FieldOffset(3)]
		private byte _a;
		[FieldOffset(2)]
		private byte _r;
		[FieldOffset(1)]
		private byte _g;
		[FieldOffset(0)]
		private byte _b;

		public byte A { get => _a; set => _a = value; }

		public byte B { get => _b; set => _b = value; }

		public byte G { get => _g; set => _g = value; }

		public byte R { get => _r; set => _r = value; }

		internal bool IsTransparent => A == 0;

		public static Color FromArgb(byte a, byte r, byte g, byte b) => new Color(a, r, g, b);

		private Color(byte a, byte r, byte g, byte b)
		{
			_color = 0; // Required for field initialization rules in C#
			_b = b;
			_g = g;
			_r = r;
			_a = a;
		}

		public override bool Equals(object o) => o is Color color && Equals(color);

		public bool Equals(Color color) =>
			color._color == _color;

		public override int GetHashCode() => (int)_color;

		public override string ToString() => ToString(null, null);

		public static bool operator ==(Color color1, Color color2) => color1.Equals(color2);

		public static bool operator !=(Color color1, Color color2) => !color1.Equals(color2);

		/// <summary>
		/// Returns value indicating color's luminance.
		/// Values lower than 0.5 mean dark color, above 0.5 light color.
		/// </summary>
		internal double Luminance => (0.299 * R + 0.587 * G + 0.114 * B) / 255;

		internal Color WithOpacity(double opacity) => new Color((byte)(A * opacity), R, G, B);

		internal uint AsUInt32() => _color;

		string IFormattable.ToString(string format, IFormatProvider formatProvider) => ToString(format, formatProvider);

		private string ToString(string format, IFormatProvider formatProvider) => string.Format(formatProvider, "#{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
	}
}
