//
//  CultureHelper.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Globalization;
using System.Text.RegularExpressions;

namespace R7.University.Utilities
{
    public static class CultureHelper
    {
        // TODO: Move to the base library
        public static int GetPlural (int n, CultureInfo culture)
        {
            // http://localization-guide.readthedocs.org/en/latest/l10n/pluralforms.html

            var lang = culture.TwoLetterISOLanguageName;
            if (lang == "ru") {
                // nplurals=3
                return (n % 10 == 1 && n % 100 != 11 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2);
            }

            if (lang == "pl") {
                // nplurals=3
                return (n == 1 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2);
            }

            if (lang == "zh" || lang == "ja" || lang == "ko") {
                // nplurals=1
                return 0;
            }

            if (lang == "ar") {
                // nplurals=6
                return (n == 0 ? 0 : n == 1 ? 1 : n == 2 ? 2 : n % 100 >= 3 && n % 100 <= 10 ? 3 : n % 100 >= 11 ? 4 : 5);
            }

            if (lang == "fr" || lang == "tr" ||  lang == "fa") {
                // nplurals=2
                return (n > 1) ? 1 : 0;
            }

            // nplurals=2 for en, es, it, pt, hi, bn, ta, ur, ml, nl, ...
            return (n != 1) ? 1 : 0;
        }

        // TODO: Add support for other languages?

        #region Russian transliteration table

        private static string [,] ruTranslitTable =
        {
            // apply some filename rules
            { @"[^0-9^a-z^а-я^A-Z^А-Я^\-^ё^Ё]", "_" },
            { @"_+", "_" },
            { @"\-+", "-" },
            { @"\A[_\-]+", "" },
            { @"[_\-]+\z", "" },
            { @"([_\-])[_\-]+", "$1" },
            // custom rules
            { @"\bх", "kh" },
            { @"\bХ", "Kh" },
            { @"\Bый", "y" },
            { "ье", "iye" },
            { "ья", "iya" },
            { "ью", "iyu" },
            // main rules
            { "а", "a" },
            { "б", "b" },
            { "в", "v" },
            { "г", "g" },
            { "д", "d" },
            { "е", "e" },
            { "ё", "yo" },
            { "ж", "zh" },
            { "з", "z" },
            { "и", "i" },
            { "й", "y" },
            { "к", "k" },
            { "л", "l" },
            { "м", "m" },
            { "н", "n" },
            { "о", "o" },
            { "п", "p" },
            { "р", "r" },
            { "с", "s" },
            { "т", "t" },
            { "у", "u" },
            { "ф", "f" },
            { "х", "h" },
            { "ц", "c" },
            { "ч", "ch" },
            { "ш", "sh" },
            { "щ", "sch" },
            { "ъ", "" },
            { "ы", "y" },
            { "ь", "" },
            { "э", "e" },
            { "ю", "yu" },
            { "я", "ya" },
            { "А", "A" },
            { "Б", "B" },
            { "В", "V" },
            { "Г", "G" },
            { "Д", "D" },
            { "Е", "E" },
            { "Ё", "YO" },
            { "Ж", "Zh" },
            { "З", "Z" },
            { "И", "I" },
            { "Й", "Y" },
            { "К", "K" },
            { "Л", "L" },
            { "М", "M" },
            { "Н", "N" },
            { "О", "O" },
            { "П", "P" },
            { "Р", "R" },
            { "С", "S" },
            { "Т", "T" },
            { "У", "U" },
            { "Ф", "F" },
            { "Х", "H" },
            { "Ц", "C" },
            { "Ч", "Ch" },
            { "Ш", "Sh" },
            { "Щ", "Sch" },
            { "Ъ", "" },
            { "Ы", "Y" },
            { "Ь", "" },
            { "Э", "E" },
            { "Ю", "YU" },
            { "Я", "YA" }
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
