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
/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreementsd  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the Licensed  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math.Random
{
    /// <summary>
    /// Interface extracted from <code>java.Utility.Random</code>d  This interface is
    /// implemented by {@link AbstractRandomGenerator}.
    /// 
    /// @since 1.1
    /// @version $Revision: 949750 $ $Date: 2010-05-31 16:06:04 +0200 (lund 31 mai 2010) $
    /// </summary>
    public interface IRandomGenerator
    {

        /// <summary>
        /// Sets the seed of the underlying random number generator using an
        /// <code>int</code> seed.
        /// <p>Sequences of values generated starting with the same seeds
        /// should be identical.
        /// </p>
        /// </summary>
        /// <param Name="seed">the seed value</param>
        void SetSeed(int seed);

        /// <summary>
        /// Sets the seed of the underlying random number generator using an
        /// <code>int</code> array seed.
        /// <p>Sequences of values generated starting with the same seeds
        /// should be identical.
        /// </p>
        /// </summary>
        /// <param Name="seed">the seed value</param>
        void SetSeed(int[] seed);

        /// <summary>
        /// Sets the seed of the underlying random number generator using a
        /// <code>long</code> seed.
        /// <p>Sequences of values generated starting with the same seeds
        /// should be identical.
        /// </p>
        /// </summary>
        /// <param Name="seed">the seed value</param>
        void SetSeed(long seed);

        /// <summary>
        /// Generates random bytes and places them into a user-supplied
        /// byte arrayd  The number of random bytes produced is equal to
        /// the Length of the byte array.
        /// 
        /// </summary>
        /// <param Name="bytes">the non-null byte array in which to put the</param>
        /// random bytes
        void NextBytes(byte[] bytes);

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed <code>int</code>
        /// value from this random number generator's sequence.
        /// All 2<font size="-1"><sup>32</sup></font> possible <i>int</i> values
        /// should be produced with  (approximately) equal probability.
        /// 
        /// </summary>
        /// <returns>the next pseudorandom, uniformly distributed <code>int</code></returns>
        ///  value from this random number generator's sequence
        int NextInt();

        /// <summary>
        /// Returns a pseudorandom, uniformly distributed <i>int</i> value
        /// between 0 (inclusive) and the specified value (exclusive), drawn from
        /// this random number generator's sequence.
        /// 
        /// </summary>
        /// <param Name="n">the bound on the random number to be returnedd  Must be</param>
        /// positive.
        /// <returns> a pseudorandom, uniformly distributed <i>int</i></returns>
        /// value between 0 (inclusive) and n (exclusive).
        /// <exception cref="ArgumentException"> if n is not positived </exception>
        int NextInt(int n);

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed <code>long</code>
        /// value from this random number generator's sequenced  All
        /// 2<font size="-1"><sup>64</sup></font> possible <i>long</i> values
        /// should be produced with (approximately) equal probability.
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed <code>long</code> *value from this random number generator's sequence</returns>

        long NextLong();

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed
        /// <code>Boolean</code> value from this random number generator's
        /// sequence.
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed</returns>
        /// <code>Boolean</code> value from this random number generator's
        /// sequence
        Boolean NextBoolean();

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed <code>float</code>
        /// value between <code>0.0</code> and <code>1.0</code> from this random
        /// number generator's sequence.
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed <code>float</code></returns>
        /// value between <code>0.0</code> and <code>1.0</code> from this
        /// random number generator's sequence
        float NextFloat();

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed
        /// <code>double</code> value between <code>0.0</code> and
        /// <code>1.0</code> from this random number generator's sequence.
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed</returns>
        ///  <code>double</code> value between <code>0.0</code> and
        ///  <code>1.0</code> from this random number generator's sequence
        double NextDouble();

        /// <summary>
        /// Returns the next pseudorandom, Gaussian ("normally") distributed
        /// <code>double</code> value with mean <code>0.0</code> and standard
        /// deviation <code>1.0</code> from this random number generator's sequence.
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, Gaussian ("normally") distributed</returns>
        /// <code>double</code> value with mean <code>0.0</code> and
        /// standard deviation <code>1.0</code> from this random number
        ///  generator's sequence
        double NextGaussian();
    }
}
