//
// TextUtils.cs
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
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace R7.University
{
    public static class TextUtils
    {
        // TODO: Add support for other languages

        #region Russian transliteration table

        private static string [,] ruTranslitTable = {
            // apply some filename rules
            {@"[^0-9^a-z^а-я^A-Z^А-Я^\-^ё^Ё]", "_"},
            {@"_+", "_"},
            {@"\-+", "-"},
            {@"\A[_\-]+", ""},
            {@"[_\-]+\z", ""},
            {@"([_\-])[_\-]+", "$1"},
            // custom rules
            {@"\bх", "kh" },
            {@"\bХ", "Kh" },
            {@"\Bый", "y"},
            {"ье", "iye" },
            {"ья", "iya" },
            {"ью", "iyu" },
            // main rules
            {"а", "a"}, 
            {"б", "b"}, 
            {"в", "v"},
            {"г", "g"},
            {"д", "d"},
            {"е", "e"},
            {"ё", "yo"},
            {"ж", "zh"},
            {"з", "z"},
            {"и", "i"},
            {"й", "y"},
            {"к", "k"},
            {"л", "l"},
            {"м", "m"},
            {"н", "n"},
            {"о", "o"},
            {"п", "p"},
            {"р", "r"},
            {"с", "s"},
            {"т", "t"},
            {"у", "u"},
            {"ф", "f"},
            {"х", "h"},
            {"ц", "c"},
            {"ч", "ch"},
            {"ш", "sh"},
            {"щ", "sch"},
            {"ъ", ""},
            {"ы", "y"},
            {"ь", ""},
            {"э", "e"},
            {"ю", "yu"},
            {"я", "ya"},
            {"А", "A"},
            {"Б", "B"},
            {"В", "V"},
            {"Г", "G"},
            {"Д", "D"},
            {"Е", "E"},
            {"Ё", "YO"},
            {"Ж", "Zh"},
            {"З", "Z"},
            {"И", "I"},
            {"Й", "Y"},
            {"К", "K"},
            {"Л", "L"},
            {"М", "M"},
            {"Н", "N"},
            {"О", "O"},
            {"П", "P"},
            {"Р", "R"},
            {"С", "S"},
            {"Т", "T"},
            {"У", "U"},
            {"Ф", "F"},
            {"Х", "H"},
            {"Ц", "C"},
            {"Ч", "Ch"},
            {"Ш", "Sh"},
            {"Щ", "Sch"},
            {"Ъ", ""},
            {"Ы", "Y"},
            {"Ь", ""},
            {"Э", "E"},
            {"Ю", "YU"},
            {"Я", "YA"}
        };

        public static string [,] RuTranslitTable
        {
            get { return ruTranslitTable; }
        }

        #endregion

        public static string Transliterate (string s, string [,] translitTable)
        {
            if (translitTable != null)
                for (var i = 0; i < translitTable.GetLength (0); i++)
                    s = Regex.Replace (s, translitTable [i, 0], translitTable [i, 1]);

            return s;
        }
    }
}

