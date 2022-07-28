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

namespace Mercury.Language.Math.Random
{
    /// <summary>
    /// Generate random vectors isotropically located on the surface of a sphere.
    /// 
    /// @since 2.1
    /// @version $Revision: 990655 $ $Date: 2010-08-29 23:49:40 +0200 (dimd 29 août 2010) $
    /// </summary>
    public class UnitSphereRandomVectorGenerator : IRandomVectorGenerator
    {
        /// <summary>
        /// RNG used for generating the individual components of the vectors.
        /// </summary>
        private RandomGenerator rand;
        /// <summary>
        /// Space dimension.
        /// </summary>
        private int dimension;

        /// <summary>
        /// </summary>
        /// <param Name="dimension">Space dimension.</param>
        /// <param Name="rand">RNG for the individual components of the vectors.</param>
        public UnitSphereRandomVectorGenerator(int dimension, RandomGenerator rand)
        {
            this.dimension = dimension;
            this.rand = rand;
        }
        /// <summary>
        /// Create an object that will use a default RNG ({@link MersenneTwister}),
        /// in order to generate the individual components.
        /// 
        /// </summary>
        /// <param Name="dimension">Space dimension.</param>
        public UnitSphereRandomVectorGenerator(int dimension) : this(dimension, new MersenneTwister())
        {

        }

        /// <summary>{@inheritDoc} */
        public double[] NextVector()
        {

            double[] v = new double[dimension];

            double normSq;
            do
            {
                normSq = 0;
                AutoParallel.AutoParallelFor(0, dimension, (i) =>
                {
                    double comp = 2 * rand.NextDouble() - 1;
                    v[i] = comp;
                    normSq += comp * comp;
                });
            } while (normSq > 1);

            double f = 1 / System.Math.Sqrt(normSq);
            AutoParallel.AutoParallelFor(0, dimension, (i) =>
            {
                v[i] *= f;
            });

            return v;
        }
    }
}
