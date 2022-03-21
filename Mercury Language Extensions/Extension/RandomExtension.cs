// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by System.Extension Inc.
//
// Copyright (C) 2012 - present by System.Extension Inc. and the System.Extension group of companies
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

namespace System
{
    /// <summary>
    /// RandomExtension Description
    /// </summary>
    public static class RandomExtension
    {
        public static double[] NextVector(this Random r, int length)
        {
            double[] rand = new double[length];

            for (int i = 0; i< length; i++)
            {
                rand[i] = r.Next();
            }

            return rand;
        }

        public static double[] GetVector(this Random r, int dimension)
        {
            // ArgumentChecker.NotNegative(dimension, "dimension");
            double[] result = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                result[i] = r.Next();
            }
            return result;
        }

        public static List<double[]> GetVectors(this Random r, int dimension, int n)
        {
            if (dimension < 0)
            {
                throw new ArgumentException(Mercury.Language.LocalizedResources.Instance().RANDOM_DIMENSION_MUST_BE_GREATER_THAN_ZERO);
            }
            if (n < 0)
            {
                throw new ArgumentException(Mercury.Language.LocalizedResources.Instance().RANDOM_NUMBER_OF_VALUES_MUST_BE_GREATER_THAN_ZERO);
            }
            List<double[]> result = new List<double[]>(n);
            double[] x;
            for (int i = 0; i < n; i++)
            {
                x = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    x[j] = r.Next();
                }
                result.Add(x);
            }
            return result;
        }
    }
}
