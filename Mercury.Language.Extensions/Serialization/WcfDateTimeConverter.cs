// Copyright (c) 2017 - presented by Kei Nakai
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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Buffers;
using System.Buffers.Text;

namespace Mercury.Language.Serialization
{
    /// <summary>
    /// WcfDateTimeConverter Description
    /// </summary>
    public class WcfDateTimeConverter : JsonConverter<DateTime>
    {
        private const char backslash = '\u005c';

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            if (reader.TokenType == JsonTokenType.String)
            {
                ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                if (Utf8Parser.TryParse(span, out DateTime datetime, out int bytesConsumed) && span.Length == bytesConsumed)
                    return datetime;

                var dateString = reader.GetString().Replace("\"\\\\/Date(", "").Replace("\"\\/Date(", "").Replace("/Date(", "").Replace(")\\\\/\"", "").Replace(")\\/\"", "").Replace(")/", "");
                try
                {
                    DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    long milliseconds = 0;
                    int offset = 0;

                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;
                    var localTimeOffset = (timeZoneInfo.GetUtcOffset(DateTime.Now).Hours * 100);

                    String[] data = dateString.Split("-");

                    // This means the value is given local datetime
                    if (dateString.Contains("-"))
                    {
                        data = dateString.Split("-");
                        int.TryParse(data[1], out offset);
                        offset = offset * -1;
                    }
                    else if (dateString.Contains("+"))
                    {
                        data = dateString.Split("+");
                        int.TryParse(data[1], out offset);
                    }
                    else
                        data = new string[] { dateString };

                    var diffHours = (localTimeOffset - offset) / 100;

                    long.TryParse(data[0], out milliseconds);

                    return Jan1st1970.AddMilliseconds(milliseconds).AddHours(diffHours).ToLocalTime();
                }
                finally
                {
                    // nothing
                }
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            string offset = "";
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;

            if (value.Kind != DateTimeKind.Utc)
            {
                if ((timeZoneInfo.GetUtcOffset(value).Hours * 100) > 0)
                    offset = "+" + (timeZoneInfo.GetUtcOffset(value).Hours * 100).ToString("D4");
                else if ((timeZoneInfo.GetUtcOffset(value).Hours * 100) < 0)
                    offset = (timeZoneInfo.GetUtcOffset(value).Hours * 100).ToString("D4");
            }

            var returnValue = @"\/Date(" + value.CurrentTimeMillis() + offset + @")\/";

            writer.WriteStringValue(returnValue);
        }
    }
}
