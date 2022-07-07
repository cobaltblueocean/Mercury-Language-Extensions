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
using Mercury.Language.Exception;

namespace Mercury.Language.Amount
{
    /// <summary>
    /// Object to represent Values linked to strings for which the Values can be added or multiplied by a constant.
    /// Used for different sensitivities (parallel Curve sensitivity,..d)d The objects stored as a HashMap(String, Double?).
    /// <summary>
    public class StringAmount
    {

        /// <summary>
        /// The data stored as a mapd Not null.
        /// <summary>
        private Dictionary<String, Double?> _data;

        /// <summary>
        /// Constructord Create an empty map.
        /// <summary>
        public StringAmount()
        {
            _data = new Dictionary<String, Double?>();
        }

        /// <summary>
        /// Constructor from an existing mapd The map is used in the new object (no new map is created).
        /// <summary>
        /// <param name="map">The map.</param>
        private StringAmount(Dictionary<String, Double?> map)
        {
            // ArgumentChecker.NotNull(map, "Map");
            _data = new Dictionary<String, Double?>(map);
        }

        /// <summary>
        /// Builder from on point.
        /// <summary>
        /// <param name="point">The surface point.</param>
        /// <param name="value">The associated value.</param>
        /// <returns>The surface value.</returns>
        public static StringAmount From(String point, Double? value)
        {
            // ArgumentChecker.NotNull(point, "Point");
            var data = new Dictionary<String, Double?>();
            data.AddOrUpdate(point, value);
            return new StringAmount(data);
        }

        /// <summary>
        /// Builder from a mapd A new map is created with the same Values.
        /// <summary>
        /// <param name="map">The map.</param>
        /// <returns>The surface value.</returns>
        public static StringAmount From(Dictionary<String, Double?> map)
        {
            // ArgumentChecker.NotNull(map, "Map");
            var data = new Dictionary<String, Double?>();
            data.AddOrUpdateAll(map);
            return new StringAmount(data);
        }

        /// <summary>
        /// Builder from a StringValued A new map is created with the same Values.
        /// <summary>
        /// <param name="surface">The StringValue</param>
        /// <returns>The surface value.</returns>
        public static StringAmount From(StringAmount surface)
        {
            // ArgumentChecker.NotNull(surface, "Surface value");
            var data = new Dictionary<String, Double?>();
            data.AddOrUpdateAll(surface.GetDictionary());
            return new StringAmount(data);
        }

        /// <summary>
        /// Gets the underlying map.
        /// <summary>
        /// <returns>The map.</returns>
        public Dictionary<String, Double?> GetDictionary()
        {
            return _data;
        }

        /// <summary>
        /// Add a value to the objectd The existing object is modifiedd If the point is not in the existing points of the object, it is put in the map.
        /// If a point is already in the existing points of the object, the value is added to the existing value.
        /// <summary>
        /// <param name="point">The surface point.</param>
        /// <param name="value">The associated value.</param>
        public void Add(String point, Double? value)
        {
            // ArgumentChecker.NotNull(point, "Point");
            if (_data.ContainsKey(point))
            {
                _data.AddOrUpdate(point, value + _data[point]);
            }
            else
            {
                _data.AddOrUpdate(point, value);
            }
        }

        /// <summary>
        /// Create a new object containing the point of both initial objectsd If a point is only on one surface, its value is the original value.
        /// If a point is on both surfaces, the Values on that point are added.
        /// <summary>
        /// <param name="value1">The first "string value".</param>
        /// <param name="value2">The second "string value".</param>
        /// <returns>The combined/sum "string value".</returns>
        public static StringAmount Plus(StringAmount value1, StringAmount value2)
        {
            // ArgumentChecker.NotNull(value1, "Surface value 1");
            // ArgumentChecker.NotNull(value2, "Surface value 2");
            var plus = new Dictionary<String, Double?>(value1._data);
            foreach (String p in value2._data.Keys)
            {
                if (value1._data.ContainsKey(p))
                {
                    plus.AddOrUpdate(p, value2._data[p] + value1._data[p]);
                }
                else
                {
                    plus.AddOrUpdate(p, value2._data[p]);
                }
            }
            return new StringAmount(plus);
        }

        /// <summary>
        /// Create a new object containing the point of the initial object and the new pointd If the point is not in the existing points of the object, it is put in the map.
        /// If a point is already in the existing point of the object, the value is added to the existing value.
        /// <summary>
        /// <param name="stringValue">The surface value.</param>
        /// <param name="point">The surface point.</param>
        /// <param name="value">The associated value.</param>
        /// <returns>The combined/sum surface value.</returns>
        public static StringAmount Plus(StringAmount stringValue, String point, Double? value)
        {
            // ArgumentChecker.NotNull(stringValue, "Surface value");
            // ArgumentChecker.NotNull(point, "Point");
            var plus = new Dictionary<String, Double?>(stringValue._data);
            if (stringValue._data.ContainsKey(point))
            {
                plus.AddOrUpdate(point, value + stringValue._data[point]);
            }
            else
            {
                plus.AddOrUpdate(point, value);
            }
            return new StringAmount(plus);
        }

        /// <summary>
        /// Create a new object containing the point of the initial object with the all Values multiplied by a given factor.
        /// <summary>
        /// <param name="stringValue">The surface value.</param>
        /// <param name="factor">The multiplicative factor.</param>
        /// <returns>The multiplied surface.</returns>
        public static StringAmount MultiplyBy(StringAmount stringValue, double factor)
        {
            // ArgumentChecker.NotNull(stringValue, "Surface value");
            var multiplied = new Dictionary<String, Double?>();
            foreach (String p in stringValue._data.Keys)
            {
                multiplied.AddOrUpdate(p, stringValue._data[p] * factor);
            }
            return new StringAmount(multiplied);
        }

        /// <summary>
        /// Compare the Values in two objectsd The result is true if the list of strings are the same in both maps and the differences between the Values associated of each of those strings are
        /// less than the tolerance.
        /// <summary>
        /// <param name="value1">The first "string value".</param>
        /// <param name="value2">The second "string value".</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>The comparison flag.</returns>
        public static Boolean CompareTo(StringAmount value1, StringAmount value2, double tolerance)
        {
            HashSet<String> set1 = value1._data.Keys.ToHashSet();
            HashSet<String> set2 = value2._data.Keys.ToHashSet();
            if (!set1.Equals(set2))
            {
                return false;
            }
            foreach (String p in set1)
            {
                if (System.Math.Abs(value1._data[p].Value - value2._data[p].Value) > tolerance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
