// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by The Noda Time Authors.
// Modified to use for System.Text.Json serialization.
//
// Copyright 2012 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
// https://github.com/nodatime/nodatime.serialization
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Buffers;
using System.Buffers.Text;
using NodaTime.Text;
using NodaTime.Utility;
namespace Mercury.Language.Serialization
{
    /// <summary>
    /// A JSON converter for types which can be represented by a single string value, parsed or formatted
    /// from an <see cref="IPattern{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type to convert to/from JSON.</typeparam>
    public sealed class NodaPatternConverter<T> : NodaConverterBase<T>
    {
        private readonly IPattern<T> pattern;
        private readonly Action<T> validator;

        /// <summary>
        /// Creates a new instance with a pattern and no validator.
        /// </summary>
        /// <param name="pattern">The pattern to use for parsing and formatting.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null.</exception>
        public NodaPatternConverter(IPattern<T> pattern) : this(pattern, null)
        {
        }

        /// <summary>
        /// Creates a new instance with a pattern and an optional validator. The validator will be called before each
        /// value is written, and may throw an exception to indicate that the value cannot be serialized.
        /// </summary>
        /// <param name="pattern">The pattern to use for parsing and formatting.</param>
        /// <param name="validator">The validator to call before writing values. May be null, indicating that no validation is required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> is null.</exception>
        public NodaPatternConverter(IPattern<T> pattern, Action<T> validator)
        {
            Preconditions.CheckNotNull(pattern, nameof(pattern));
            this.pattern = pattern;
            this.validator = validator;
        }

        /// <summary>
        /// Implemented by concrete subclasses, this performs the final conversion from a non-null JSON value to
        /// a value of type T.
        /// </summary>
        /// <param name="reader">The JSON reader to pull data from</param>
        /// <param name="serializer">The serializer to use for nested serialization</param>
        /// <returns>The deserialized value of type T.</returns>
        protected override T ReadJsonImpl(Utf8JsonReader reader, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new InvalidNodaDataException(
                    $"Unexpected token parsing {typeof(T).Name}. Expected String, got {reader.TokenType}.");
            }
            string text = reader.GetString();
            return pattern.Parse(text).Value;
        }

        /// <summary>
        /// Writes the formatted value to the writer.
        /// </summary>
        /// <param name="writer">The writer to write JSON data to</param>
        /// <param name="value">The value to serializer</param>
        /// <param name="serializer">The serializer to use for nested serialization</param>
        protected override void WriteJsonImpl(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            validator?.Invoke(value);
            writer.WriteStringValue(pattern.Format(value));
        }
    }
}
