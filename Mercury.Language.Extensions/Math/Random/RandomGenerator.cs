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
using Mercury.Language.Exceptions;

namespace Mercury.Language.Math.Random
{
    /// <summary>
    /// Abstract class implementing the {@link  RandomGenerator} interface.
    /// Default implementations for all methods other than {@link #NextDouble()} and
    /// {@link #SetSeed(long)} are provided.
    /// <p>
    /// All data generation methods are based on {@code code NextDouble()}.
    /// Concrete implementations <strong>must</strong> override
    /// this method and <strong>should</strong> provide better / more
    /// performant implementations of the other methods if the underlying PRNG
    /// supplies them.</p>
    /// 
    /// @since 1.1
    /// @version $Revision: 990655 $ $Date: 2010-08-29 23:49:40 +0200 (dimd 29 août 2010) $
    /// </summary>
    public class RandomGenerator : IRandomGenerator
    {

        private System.Random _random;

        /// <summary>
        /// Cached random normal valued  The default implementation for
        /// {@link #nextGaussian} generates pairs of values and this field caches the
        /// second value so that the full algorithm is not executed for every
        /// activationd  The value {@code Double.NaN} signals that there is
        /// no cached valued  Use {@link #clear} to clear the cached value.
        /// </summary>
        private double cachedNormalDeviate = Double.NaN;

        /// <summary>
        /// Construct a RandomGenerator.
        /// </summary>
        public RandomGenerator()
        {
            _random = new System.Random();
        }

        /// <summary>
        /// Clears the cache used by the default implementation of
        /// {@link #nextGaussian}d Implemementations that do not override the
        /// default implementation of {@code nextGaussian} should call this
        /// method in the implementation of {@link #SetSeed(long)}
        /// </summary>
        public void Clear()
        {
            cachedNormalDeviate = Double.NaN;
        }

        /// <summary>{@inheritDoc} */
        public void SetSeed(int seed)
        {
            SetSeed((long)seed);
        }

        /// <summary>{@inheritDoc} */
        public void SetSeed(int[] seed)
        {
            // the following number is the largest prime that fits in 32 bits (it is 2^32 - 5)
            long prime = 4294967291L;

            long combined = 0L;
            foreach (int s in seed)
            {
                combined = combined * prime + s;
            }
            SetSeed(combined);
        }

        /// <summary>
        /// Sets the seed of the underyling random number generator using a
        /// {@code long} seedd  Sequences of values generated starting with the
        /// same seeds should be identical.
        /// <p>
        /// Implementations that do not override the default implementation of
        /// {@code nextGaussian} should include a call to {@link #clear} in the
        /// implementation of this method.</p>
        /// 
        /// </summary>
        /// <param Name="seed">the seed value</param>
        public virtual void SetSeed(long seed)
        {
            _random = new System.Random((int)seed);
        }

        /// <summary>
        /// Generates random bytes and places them into a user-supplied
        /// byte arrayd  The number of random bytes produced is equal to
        /// the Length of the byte array.
        /// <p>
        /// The default implementation fills the array with bytes extracted from
        /// random integers generated using {@link #nextInt}.</p>
        /// 
        /// </summary>
        /// <param Name="bytes">the non-null byte array in which to put the</param>
        /// random bytes
        public virtual void NextBytes(byte[] bytes)
        {
            int bytesOut = 0;
            while (bytesOut < bytes.Length)
            {
                int randInt = NextInt();
                for (int i = 0; i < 3; i++)
                {
                    if (i > 0)
                    {
                        randInt = randInt >> 8;
                    }
                    bytes[bytesOut++] = (byte)randInt;
                    if (bytesOut == bytes.Length)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed {@code int}
        /// value from this random number generator's sequence.
        /// All 2<font size="-1"><sup>32</sup></font> possible {@code int} values
        /// should be produced with  (approximately) equal probability.
        /// <p>
        /// The default implementation provided here returns
        /// <pre>
        /// <code>(int)(NextDouble() * Int32.MaxValue)</code>
        /// </pre></p>
        /// 
        /// </summary>
        /// <returns>the next pseudorandom, uniformly distributed {@code int}</returns>
        ///  value from this random number generator's sequence
        public virtual int NextInt()
        {
            return (int)(NextDouble() * Int32.MaxValue);
        }

        /// <summary>
        /// Returns a pseudorandom, uniformly distributed {@code int} value
        /// between 0 (inclusive) and the specified value (exclusive), drawn from
        /// this random number generator's sequence.
        /// <p>
        /// The default implementation returns
        /// <pre>
        /// <code>(int)(NextDouble() * n</code>
        /// </pre></p>
        /// 
        /// </summary>
        /// <param Name="n">the bound on the random number to be returnedd  Must be</param>
        /// positive.
        /// <returns> a pseudorandom, uniformly distributed {@code int}</returns>
        /// value between 0 (inclusive) and n (exclusive).
        /// <exception cref="NotStrictlyPositiveException">if {@code n <= 0}d </exception>
        public virtual int NextInt(int n)
        {
            if (n <= 0)
            {
                throw new NotStrictlyPositiveException(n);
            }
            int result = (int)(NextDouble() * n);
            return result < n ? result : n - 1;
        }

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed {@code long}
        /// value from this random number generator's sequenced  All
        /// 2<font size="-1"><sup>64</sup></font> possible {@code long} values
        /// should be produced with (approximately) equal probability.
        /// <p>
        /// The default implementation returns
        /// <pre>
        /// <code>(long)(NextDouble() * Long.MaxValue)</code>
        /// </pre></p>
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed {@code long} *value from this random number generator's sequence</returns>

        public virtual long NextLong()
        {
            return (long)(NextDouble() * long.MaxValue);
        }

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed
        /// {@code Boolean} value from this random number generator's
        /// sequence.
        /// <p>
        /// The default implementation returns
        /// <pre>
        /// <code>NextDouble() <= 0.5</code>
        /// </pre></p>
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed</returns>
        /// {@code Boolean} value from this random number generator's
        /// sequence
        public virtual Boolean NextBoolean()
        {
            return NextDouble() <= 0.5;
        }

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed {@code float}
        /// value between {@code 0.0} and {@code 1.0} from this random
        /// number generator's sequence.
        /// <p>
        /// The default implementation returns
        /// <pre>
        /// <code>(float) NextDouble() </code>
        /// </pre></p>
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed {@code float}</returns>
        /// value between {@code 0.0} and {@code 1.0} from this
        /// random number generator's sequence
        public virtual float NextFloat()
        {
            return (float)NextDouble();
        }

        /// <summary>
        /// Returns the next pseudorandom, uniformly distributed
        /// {@code double} value between {@code 0.0} and
        /// {@code 1.0} from this random number generator's sequence.
        /// <p>
        /// This method provides the underlying source of random data used by the
        /// other methods.</p>
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, uniformly distributed</returns>
        ///  {@code double} value between {@code 0.0} and
        ///  {@code 1.0} from this random number generator's sequence
        public virtual double NextDouble()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// Returns the next pseudorandom, Gaussian ("normally") distributed
        /// {@code double} value with mean {@code 0.0} and standard
        /// deviation {@code 1.0} from this random number generator's sequence.
        /// <p>
        /// The default implementation uses the <em>Polar Method</em>
        /// due to G.E.Pd Box, M.Ed Muller and Gd Marsaglia, as described in
        /// Dd Knuth, <u>The Art of Computer Programming</u>, 3.4.1C.</p>
        /// <p>
        /// The algorithm generates a pair of independent random valuesd  One of
        /// these is cached for reuse, so the full algorithm is not executed on each
        /// activationd  Implementations that do not override this method should
        /// make sure to call {@link #clear} to clear the cached value in the
        /// implementation of {@link #SetSeed(long)}.</p>
        /// 
        /// </summary>
        /// <returns> the next pseudorandom, Gaussian ("normally") distributed</returns>
        /// {@code double} value with mean {@code 0.0} and
        /// standard deviation {@code 1.0} from this random number
        ///  generator's sequence
        public virtual double NextGaussian()
        {
            if (!Double.IsNaN(cachedNormalDeviate))
            {
                double dev = cachedNormalDeviate;
                cachedNormalDeviate = Double.NaN;
                return dev;
            }
            double v1 = 0;
            double v2 = 0;
            double s = 1;
            while (s >= 1)
            {
                v1 = 2 * NextDouble() - 1;
                v2 = 2 * NextDouble() - 1;
                s = v1 * v1 + v2 * v2;
            }
            if (s != 0)
            {
                s = System.Math.Sqrt(-2 * System.Math.Log(s) / s);
            }
            cachedNormalDeviate = v2 * s;
            return v1 * s;
        }
    }
}
