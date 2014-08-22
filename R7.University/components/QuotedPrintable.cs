//
// QuotedPrintable.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

// http://sourceforge.net/apps/mediawiki/syncmldotnet/index.php?title=Quoted_Printable

namespace R7.University
{
	/// <summary>
	/// Provide encoding and decoding of Quoted-Printable.
	/// </summary>
	public class QuotedPrintable
	{
		private QuotedPrintable ()
		{
		}

		/// <summary>
		/// // so including the = connection, the length will be 76
		/// </summary>
		private const int RFC_1521_MAX_CHARS_PER_LINE = 75;

		/// <summary>
		/// Return quoted printable string with 76 characters per line.
		/// </summary>
		/// <param name="textToEncode"></param>
		/// <returns></returns>
		public static string Encode (string textToEncode)
		{
			if (textToEncode == null)
				throw new ArgumentNullException ();

			return Encode (textToEncode, RFC_1521_MAX_CHARS_PER_LINE);
		}

		private static string Encode (string textToEncode, int charsPerLine)
		{
			if (textToEncode == null)
				throw new ArgumentNullException ();

			if (charsPerLine <= 0)
				throw new ArgumentOutOfRangeException ();

			return FormatEncodedString (EncodeString (textToEncode), charsPerLine);
		}

		/// <summary>
		/// Return quoted printable string, all in one line.
		/// </summary>
		/// <param name="textToEncode"></param>
		/// <returns></returns>
		public static string EncodeString (string textToEncode)
		{
			if (textToEncode == null)
				throw new ArgumentNullException ();

			byte[] bytes = Encoding.UTF8.GetBytes (textToEncode);
			StringBuilder builder = new StringBuilder ();
			foreach (byte b in bytes)
			{
				if (b != 0)
				if ((b < 32) || (b > 126))
					builder.Append (String.Format ("={0}", b.ToString ("X2")));
				else
				{
					switch (b)
					{
						case 13:
							builder.Append ("=0D");
							break;
						case 10:
							builder.Append ("=0A");
							break;
						case 61:
							builder.Append ("=3D");
							break;
						default:
							builder.Append (Convert.ToChar (b));
							break;
					}
				}
			}

			return builder.ToString ();
		}

		private static string FormatEncodedString (string qpstr, int maxcharlen)
		{
			if (qpstr == null)
				throw new ArgumentNullException ();

			StringBuilder builder = new StringBuilder ();
			char[] charArray = qpstr.ToCharArray ();
			int i = 0;
			foreach (char c in charArray)
			{
				builder.Append (c);
				i++;
				if (i == maxcharlen)
				{
					builder.AppendLine ("=");
					i = 0;
				}
			}

			return builder.ToString ();
		}

		static string HexDecoderEvaluator (Match m)
		{
			if (String.IsNullOrEmpty (m.Value))
				return null;

			CaptureCollection captures = m.Groups [3].Captures;
			byte[] bytes = new byte[captures.Count];

			for (int i = 0; i < captures.Count; i++)
			{
				bytes [i] = Convert.ToByte (captures [i].Value, 16);
			}

			return UTF8Encoding.UTF8.GetString (bytes);
		}

		static string HexDecoder (string line)
		{
			if (line == null)
				throw new ArgumentNullException ();

			Regex re = new Regex ("((\\=([0-9A-F][0-9A-F]))*)", RegexOptions.IgnoreCase);
			return re.Replace (line, new MatchEvaluator (HexDecoderEvaluator));
		}


		public static string Decode (string encodedText)
		{
			if (encodedText == null)
				throw new ArgumentNullException ();

			using (StringReader sr = new StringReader (encodedText))
			{
				StringBuilder builder = new StringBuilder ();
				string line;
				while ((line = sr.ReadLine ()) != null)
				{
					if (line.EndsWith ("="))
						builder.Append (line.Substring (0, line.Length - 1));
					else
						builder.Append (line);
				}

				return HexDecoder (builder.ToString ());
			}
		}


	}
}

