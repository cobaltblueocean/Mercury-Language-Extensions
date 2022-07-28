﻿// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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
using Cern.Jet.Random.Engine;

namespace Mercury.Language.Math.Random
{
    /// <summary>
    /// MersenneTwister Description
    /// </summary>
    [Serializable]
    public class MersenneTwister: RandomGenerator
    {
        Cern.Jet.Random.Engine.RandomEngine _random;

        public MersenneTwister()
        {
            _random = new Cern.Jet.Random.Engine.MersenneTwister();
        }

        public MersenneTwister(int seed)
        {
            _random = new Cern.Jet.Random.Engine.MersenneTwister(seed);
        }

        public MersenneTwister(int[] seed)
        {
            _random = new Cern.Jet.Random.Engine.MersenneTwister(seed[0]);
        }

        public double Next()
        {
            return _random.NextDouble();
        }

        public override double NextDouble()
        {
            return _random.NextDouble();
        }

        public override int NextInt()
        {
            return _random.NextInt32();
        }

        public override long NextLong()
        {
            return _random.NextLong();
        }

        public override void NextBytes(byte[] bytes)
        {
            _random.NextBytes(bytes);
        }
    }
}
