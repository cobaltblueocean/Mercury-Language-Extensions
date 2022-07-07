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
    /// Object to represent Values linked to a reference for which the Values can be added or multiplied by a constant.
    /// Used for different sensitivities (parallel Curve sensitivity,..d)d The objects stored as a HashMap(T, Double?).
    /// <summary>
    /// <typeparam name="T">The reference object.</typeparam>
    public class ReferenceAmount<T>
    {

        /// <summary>
        /// The data stored as a mapd Not null.
        /// <summary>
        private Dictionary<T, Double?> _data;

        public Dictionary<T, Double?> Data
        {
            get
            {
                return _data;
            }
            set
            {
                this._data.Clear();
                this._data.AddOrUpdateAll(value);
            }
        }

        /// <summary>
        /// Constructord Create an empty map.
        /// <summary>
        public ReferenceAmount()
        {
            _data = new Dictionary<T, Double?>();
        }

        /// <summary>
        /// Constructor from an existing mapd The map is used in the new object (no new map is created).
        /// <summary>
        /// <param name="map">The map.</param>
        private ReferenceAmount(Dictionary<T, Double?> map)
        {
            _data = map;
        }

        /// <summary>
        /// Gets the underlying map.
        /// <summary>
        /// <returns>The map.</returns>
        public Dictionary<T, Double?> GetDictionary()
        {
            return _data;
        }

        /// <summary>
        /// Add a value to the objectd The existing object is modifiedd If the point is not in the existing points of the
        /// object, it is put in the map.
        /// If a point is already in the existing points of the object, the value is added to the existing value.
        /// <summary>
        /// <param name="point">The surface point.</param>
        /// <param name="value">The associated value.</param>
        public void Add(T point, Double? value)
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
        /// Create a new object containing the points of the initial object plus the points of another object.
        /// If two points <T> are identical, the Values are added.
        /// <summary>
        /// <param name="other">The other ReferenceAmount.</param>
        /// <returns>The total.</returns>
        public ReferenceAmount<T> Plus(ReferenceAmount<T> other)
        {
            var plusMap = new Dictionary<T, Double?>(_data);
            ReferenceAmount<T> plus = new ReferenceAmount<T>(plusMap);
            foreach (var p in other.Data)
            {
                plus.Add(p.Key, p.Value);
            }
            return plus;
        }

        /// <summary>
        /// Create a new object containing the point of the initial object with the all Values multiplied by a given factor.
        /// <summary>
        /// <param name="factor">The multiplicative factor.</param>
        /// <returns>The multiplied surface.</returns>
        public ReferenceAmount<T> MultiplyBy(double factor)
        {
            var multiplied = new Dictionary<T, Double?>();
            foreach (T p in _data.Keys)
            {
                multiplied.AddOrUpdate(p, _data[p] * factor);
            }
            return new ReferenceAmount<T>(multiplied);
        }
    }
}
