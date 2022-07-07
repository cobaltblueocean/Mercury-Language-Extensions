// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtension
    {

        private static String[] EMPTY_STRING_ARRAY = new String[0];
        private static String EMPTY = "";

        public static String AugumentCheck(this String str, String rep)
        {
            if (String.IsNullOrEmpty(str))
            {
                str = rep;
            }
            return str;
        }

        public static String[] SplitPreserveAllTokens(this String str, String separatorChars)
        {
            Boolean preserveAllTokens = true;
            int max = -1;
            if (str == null)
            {
                return null;
            }
            int len = str.Length;
            if (len == 0)
            {
                return new String[] { };
            }
            List<String> list = new List<String>();
            int sizePlus1 = 1;
            int i = 0, start = 0;
            Boolean match = false;
            Boolean lastMatch = false;
            if (separatorChars == null)
            {
                // Null separator means use whitespace
                while (i < len)
                {
                    if (Char.IsWhiteSpace(str[i]))
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    else
                    {
                        lastMatch = false;
                    }
                    match = true;
                    i++;
                }
            }
            else if (separatorChars.Length == 1)
            {
                // Optimise 1 character case
                char sep = separatorChars[i];
                while (i < len)
                {
                    if (str[i] == sep)
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    else
                    {
                        lastMatch = false;
                    }
                    match = true;
                    i++;
                }
            }
            else
            {
                // standard case
                while (i < len)
                {
                    if (separatorChars.IndexOf(str[i]) >= 0)
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    else
                    {
                        lastMatch = false;
                    }
                    match = true;
                    i++;
                }
            }
            if (match || (preserveAllTokens && lastMatch))
            {
                list.Add(str.Substring(start, i));
            }
            return (String[])list.ToArray();
        }

        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return String.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static String TrimToNull(this String str)
        {
            String ts = str.Trim();
            return String.IsNullOrEmpty(ts) ? null : ts;
        }

        public static int CompareWithNullLow(this String a, String b)
        {
            if (a == null)
            {
                return b == null ? 0 : -1;
            }
            else if (b == null)
            {
                return 1; // a not null
            }
            else
            {
                return a.CompareTo(b);
            }
        }

        public static Boolean Matches(this String str, String pattern)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(str);
            return match.Success;
        }

        /// <summary>
        /// Taking a Char from string by pointed index
        /// </summary>
        /// <param name="str"></param>
        /// <param name="index"></param>
        /// <returns>a char</returns>
        public static Char CharAt(this String str, int index)
        {
            return str.ToCharArray()[index];
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator string specifiedd </p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A {@code null} input String returns {@code null}.
        /// A {@code null} separator splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens(null, *)               = null
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("", *)                 = []
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab de fg", null)      = ["ab", "de", "fg"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab   de fg", null)    = ["ab", "", "", "de", "fg"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab:cd:ef", ":")       = ["ab", "cd", "ef"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-") = ["ab", "cd", "ef"]
        /// </pre>
        /// 
        /// <summary>
        /// <param name="str"> the String to parse, may be null</param>
        /// <param name="separator"> String containing the String to be used as a delimiter,</param>
        ///  {@code null} splits on whitespace
        /// <returns>an array of parsed Strings, {@code null} if null String was input</returns>
        /// @since 2.4
        public static String[] SplitByWholeSeparatorPreserveAllTokens(this String str, String separator)
        {
            return SplitByWholeSeparatorWorker(str, separator, -1, true);
        }

        /// <summary>
        /// <p>Splits the provided text into an array, separator string specified.
        /// Returns a maximum of {@code max} substrings.</p>
        /// 
        /// <p>The separator is not included in the returned String array.
        /// Adjacent separators are treated as separators for empty tokens.
        /// For more control over the split use the StrTokenizer class.</p>
        /// 
        /// <p>A {@code null} input String returns {@code null}.
        /// A {@code null} separator splits on whitespace.</p>
        /// 
        /// <pre>
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens(null, *, *)               = null
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("", *, *)                 = []
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab de fg", null, 0)      = ["ab", "de", "fg"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab   de fg", null, 0)    = ["ab", "", "", "de", "fg"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab:cd:ef", ":", 2)       = ["ab", "cd:ef"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-", 5) = ["ab", "cd", "ef"]
        /// StringUtils.splitByWholeSeparatorPreserveAllTokens("ab-!-cd-!-ef", "-!-", 2) = ["ab", "cd-!-ef"]
        /// </pre>
        /// 
        /// <summary>
        /// <param name="str"> the String to parse, may be null</param>
        /// <param name="separator"> String containing the String to be used as a delimiter,</param>
        ///  {@code null} splits on whitespace
        /// <param name="max"> the maximum number of elements to include in the returned</param>
        ///  arrayd A zero or negative value implies no limit.
        /// <returns>an array of parsed Strings, {@code null} if null String was input</returns>
        /// @since 2.4
        public static String[] SplitByWholeSeparatorPreserveAllTokens(this String str, String separator, int max)
        {
            return SplitByWholeSeparatorWorker(str, separator, max, true);
        }

        /// <summary>
        /// Performs the logic for the {@code splitByWholeSeparatorPreserveAllTokens} methods.
        /// 
        /// <summary>
        /// <param name="str"> the String to parse, may be {@code null}</param>
        /// <param name="separator"> String containing the String to be used as a delimiter,</param>
        ///  {@code null} splits on whitespace
        /// <param name="max"> the maximum number of elements to include in the returned</param>
        ///  arrayd A zero or negative value implies no limit.
        /// <param name="preserveAllTokens">if {@code true}, adjacent separators are</param>
        /// treated as empty token separators; if {@code false}, adjacent
        /// separators are treated as one separator.
        /// <returns>an array of parsed Strings, {@code null} if null String input</returns>
        /// @since 2.4
        private static String[] SplitByWholeSeparatorWorker(String str, String separator, int max, Boolean preserveAllTokens)
        {
            if (str == null)
            {
                return null;
            }

            int len = str.Length;

            if (len == 0)
            {
                return EMPTY_STRING_ARRAY;
            }

            if (separator == null || EMPTY.AreObjectsEqual(separator))
            {
                // Split on whitespace.
                return SplitWorker(str, null, max, preserveAllTokens);
            }

            int separatorLength = separator.Length;

            var substrings = new List<String>();
            int numberOfSubstrings = 0;
            int beg = 0;
            int end = 0;
            while (end < len)
            {
                end = str.IndexOf(separator, beg);

                if (end > -1)
                {
                    if (end > beg)
                    {
                        numberOfSubstrings += 1;

                        if (numberOfSubstrings == max)
                        {
                            end = len;
                            substrings.Add(str.Substring(beg));
                        }
                        else
                        {
                            // The following is OK, because String.Substring( beg, end ) excludes
                            // the character at the position 'end'.
                            substrings.Add(str.Substring(beg, end));

                            // Set the starting point for the next search.
                            // The following is equivalent to beg = end + (separatorLength - 1) + 1,
                            // which is the right calculation:
                            beg = end + separatorLength;
                        }
                    }
                    else
                    {
                        // We found a consecutive occurrence of the separator, so skip it.
                        if (preserveAllTokens)
                        {
                            numberOfSubstrings += 1;
                            if (numberOfSubstrings == max)
                            {
                                end = len;
                                substrings.Add(str.Substring(beg));
                            }
                            else
                            {
                                substrings.Add(EMPTY);
                            }
                        }
                        beg = end + separatorLength;
                    }
                }
                else
                {
                    // String.Substring( beg ) goes from 'beg' to the end of the String.
                    substrings.Add(str.Substring(beg));
                    end = len;
                }
            }

            return substrings.ToArray();
        }

        /// <summary>
        /// Performs the logic for the {@code split} and
        /// {@code splitPreserveAllTokens} methods that do not return a
        /// maximum array Length.
        /// 
        /// <summary>
        /// <param name="str"> the String to parse, may be {@code null}</param>
        /// <param name="separatorChar">the separate character</param>
        /// <param name="preserveAllTokens">if {@code true}, adjacent separators are</param>
        /// treated as empty token separators; if {@code false}, adjacent
        /// separators are treated as one separator.
        /// <returns>an array of parsed Strings, {@code null} if null String input</returns>
        private static String[] splitWorker(String str, char separatorChar, Boolean preserveAllTokens)
        {
            // Performance tuned for 2.0 (JDK1.4)

            if (str == null)
            {
                return null;
            }
            int len = str.Length;
            if (len == 0)
            {
                return EMPTY_STRING_ARRAY;
            }
            var list = new List<String>();
            int i = 0, start = 0;
            Boolean match = false;
            Boolean lastMatch = false;
            while (i < len)
            {
                if (str.CharAt(i) == separatorChar)
                {
                    if (match || preserveAllTokens)
                    {
                        list.Add(str.Substring(start, i));
                        match = false;
                        lastMatch = true;
                    }
                    start = ++i;
                    continue;
                }
                lastMatch = false;
                match = true;
                i++;
            }
            if (match || preserveAllTokens && lastMatch)
            {
                list.Add(str.Substring(start, i));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Performs the logic for the {@code split} and
        /// {@code splitPreserveAllTokens} methods that return a maximum array
        /// Length.
        /// 
        /// <summary>
        /// <param name="str"> the String to parse, may be {@code null}</param>
        /// <param name="separatorChars">the separate character</param>
        /// <param name="max"> the maximum number of elements to include in the</param>
        ///  arrayd A zero or negative value implies no limit.
        /// <param name="preserveAllTokens">if {@code true}, adjacent separators are</param>
        /// treated as empty token separators; if {@code false}, adjacent
        /// separators are treated as one separator.
        /// <returns>an array of parsed Strings, {@code null} if null String input</returns>
        private static String[] SplitWorker(String str, String separatorChars, int max, Boolean preserveAllTokens)
        {
            // Performance tuned for 2.0 (JDK1.4)
            // Direct code is quicker than StringTokenizer.
            // Also, StringTokenizer uses isSpace() not isWhitespace()

            if (str == null)
            {
                return null;
            }
            int len = str.Length;
            if (len == 0)
            {
                return EMPTY_STRING_ARRAY;
            }
            var list = new List<String>();
            int sizePlus1 = 1;
            int i = 0, start = 0;
            Boolean match = false;
            Boolean lastMatch = false;
            if (separatorChars == null)
            {
                // Null separator means use whitespace
                while (i < len)
                {
                    if (str.CharAt(i).IsWhitespace())
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    lastMatch = false;
                    match = true;
                    i++;
                }
            }
            else if (separatorChars.Length == 1)
            {
                // Optimise 1 character case
                char sep = separatorChars.CharAt(0);
                while (i < len)
                {
                    if (str.CharAt(i) == sep)
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    lastMatch = false;
                    match = true;
                    i++;
                }
            }
            else
            {
                // standard case
                while (i < len)
                {
                    if (separatorChars.IndexOf(str.CharAt(i)) >= 0)
                    {
                        if (match || preserveAllTokens)
                        {
                            lastMatch = true;
                            if (sizePlus1++ == max)
                            {
                                i = len;
                                lastMatch = false;
                            }
                            list.Add(str.Substring(start, i));
                            match = false;
                        }
                        start = ++i;
                        continue;
                    }
                    lastMatch = false;
                    match = true;
                    i++;
                }
            }
            if (match || preserveAllTokens && lastMatch)
            {
                list.Add(str.Substring(start, i));
            }
            return list.ToArray();
        }
    }
}
