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

/**
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
using Mercury.Language.Exceptions;
using Mercury.Language.Log;
using Mercury.Language;

namespace Mercury.Language.Math.Optimization.Fitting
{

    /// <summary>
    /// Guesses the parameters ({@code a}, {@code b}, {@code c}, and {@code d})
    /// of a {@link ParametricGaussianFunction} based on the specified observed
    /// points.
    /// 
    /// @since 2.2
    /// @version $Revision: 983921 $ $Date: 2010-08-10 12:46:06 +0200 (mard 10 août 2010) $
    /// </summary>

    public class GaussianParametersGuesser
    {

        /// <summary>Observed pointsd */
        private WeightedObservedPoint[] observations;

        /// <summary>Resulting guessed parametersd */
        private double[] parameters;

        /// <summary>
        /// Constructs instance with the specified observed points.
        /// 
        /// </summary>
        /// <param Name="observations">observed points upon which should base guess</param>
        public GaussianParametersGuesser(WeightedObservedPoint[] observations)
        {
            if (observations == null)
            {
                throw new ArgumentNullException(LocalizedResources.Instance().INPUT_ARRAY);
            }
            if (observations.Length < 3)
            {
                throw new NumberIsTooSmallException(observations.Length, 3, true);
            }
            this.observations = observations.CloneExact();
        }

        /// <summary>
        /// Guesses the parameters based on the observed points.
        /// 
        /// </summary>
        /// <returns>guessed parameters array <code>{a, b, c, d}</code></returns>
        public double[] guess()
        {
            if (parameters == null)
            {
                parameters = basicGuess(observations);
            }
            return parameters.CloneExact();
        }

        /// <summary>
        /// Guesses the parameters based on the specified observed points.
        /// 
        /// </summary>
        /// <param Name="points">observed points upon which should base guess</param>
        /// 
        /// <returns>guessed parameters array <code>{a, b, c, d}</code></returns>
        private double[] basicGuess(WeightedObservedPoint[] points)
        {
            Array.Sort(points, createWeightedObservedPointComparator());
            double[] _params = new double[4];

            int minYIdx = findMinY(points);
            _params[0] = points[minYIdx].Y;

            int maxYIdx = findMaxY(points);
            _params[1] = points[maxYIdx].Y;
            _params[2] = points[maxYIdx].X;

            double fwhmApprox;
            try
            {
                double halfY = _params[0] + ((_params[1] - _params[0]) / 2.0);
                double fwhmX1 = interpolateXAtY(points, maxYIdx, -1, halfY);
                double fwhmX2 = interpolateXAtY(points, maxYIdx, +1, halfY);
                fwhmApprox = fwhmX2 - fwhmX1;
            }
            catch (IndexOutOfRangeException e)
            {
                Logger.Information(e.Message);
                fwhmApprox = points[points.Length - 1].X - points[0].X;
            }
            _params[3] = fwhmApprox / (2.0 * System.Math.Sqrt(2.0 * System.Math.Log(2.0)));

            return _params;
        }

        /// <summary>
        /// Finds index of point in specified points with the smallest Y.
        /// 
        /// </summary>
        /// <param Name="points">points to search</param>
        /// 
        /// <returns>index in specified points array</returns>
        private int findMinY(WeightedObservedPoint[] points)
        {
            int minYIdx = 0;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].Y < points[minYIdx].Y)
                {
                    minYIdx = i;
                }
            }
            return minYIdx;
        }

        /// <summary>
        /// Finds index of point in specified points with the largest Y.
        /// 
        /// </summary>
        /// <param Name="points">points to search</param>
        /// 
        /// <returns>index in specified points array</returns>
        private int findMaxY(WeightedObservedPoint[] points)
        {
            int maxYIdx = 0;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i].Y > points[maxYIdx].Y)
                {
                    maxYIdx = i;
                }
            }
            return maxYIdx;
        }

        /// <summary>
        /// Interpolates using the specified points to determine X at the specified
        /// Y.
        /// 
        /// </summary>
        /// <param Name="points">points to use for interpolation</param>
        /// <param Name="startIdx">index within points from which to start search for</param>
        ///        interpolation bounds points
        /// <param Name="idxStep">index step for search for interpolation bounds points</param>
        /// <param Name="y">Y value for which X should be determined</param>
        /// 
        /// <returns>value of X at the specified Y</returns>
        /// 
        /// <exception cref="ArgumentException">if idxStep is 0 </exception>
        /// <exception cref="OutOfRangeException">if specified <code>y</code> is not within the </exception>
        ///         range of the specified <code>points</code>
        private double interpolateXAtY(WeightedObservedPoint[] points, int startIdx, int idxStep, double y)
        {
            if (idxStep == 0)
            {
                throw new ZeroOperationException();
            }
            WeightedObservedPoint[] twoPoints = getInterpolationPointsForY(points, startIdx, idxStep, y);
            WeightedObservedPoint pointA = twoPoints[0];
            WeightedObservedPoint pointB = twoPoints[1];
            if (pointA.Y == y)
            {
                return pointA.X;
            }
            if (pointB.Y == y)
            {
                return pointB.X;
            }
            return pointA.X +
                   (((y - pointA.Y) * (pointB.X - pointA.X)) / (pointB.Y - pointA.Y));
        }

        /// <summary>
        /// Gets the two bounding interpolation points from the specified points
        /// suitable for determining X at the specified Y.
        /// 
        /// </summary>
        /// <param Name="points">points to use for interpolation</param>
        /// <param Name="startIdx">index within points from which to start search for</param>
        ///        interpolation bounds points
        /// <param Name="idxStep">index step for search for interpolation bounds points</param>
        /// <param Name="y">Y value for which X should be determined</param>
        /// 
        /// <returns>array containing two points suitable for determining X at the</returns>
        ///         specified Y
        /// 
        /// <exception cref="ArgumentException">if idxStep is 0 </exception>
        /// <exception cref="OutOfRangeException">if specified <code>y</code> is not within the </exception>
        ///         range of the specified <code>points</code>
        private WeightedObservedPoint[] getInterpolationPointsForY(WeightedObservedPoint[] points, int startIdx, int idxStep, double y)

        {
            if (idxStep == 0)
            {
                throw new ZeroOperationException();
            }
            for (int i = startIdx;
                 (idxStep < 0) ? (i + idxStep >= 0) : (i + idxStep < points.Length);
                  i += idxStep)
            {
                if (isBetween(y, points[i].Y, points[i + idxStep].Y))
                {
                    return (idxStep < 0) ?
                           new WeightedObservedPoint[] { points[i + idxStep], points[i] } :
                           new WeightedObservedPoint[] { points[i], points[i + idxStep] };
                }
            }

            double minY = Double.PositiveInfinity;
            double maxY = Double.NegativeInfinity;
            foreach (WeightedObservedPoint point in points)
            {
                minY = System.Math.Min(minY, point.Y);
                maxY = System.Math.Max(maxY, point.Y);
            }
            throw new IndexOutOfRangeException("y: " + y + ", minY: " + minY + ", maxY: " + maxY);

        }

        /// <summary>
        /// Determines whether a value is between two other values.
        /// 
        /// </summary>
        /// <param Name="value">value to determine whether is between <code>boundary1</code></param>
        ///        and <code>boundary2</code>
        /// <param Name="boundary1">one end of the range</param>
        /// <param Name="boundary2">other end of the range</param>
        /// 
        /// <returns>true if <code>value</code> is between <code>boundary1</code> and</returns>
        ///         <code>boundary2</code> (inclusive); false otherwise
        private Boolean isBetween(double value, double boundary1, double boundary2)
        {
            return (value >= boundary1 && value <= boundary2) ||
                   (value >= boundary2 && value <= boundary1);
        }

        /// <summary>
        /// Factory method creating <code>IComparer</code> for comparing
        /// <code>WeightedObservedPoint</code> instances.
        /// 
        /// </summary>
        /// <returns>new <code>IComparer</code> instance</returns>
        private IComparer<WeightedObservedPoint> createWeightedObservedPointComparator()
        {
            return new WeightedObservedPointComparer();
        }

        private class WeightedObservedPointComparer : IComparer<WeightedObservedPoint>
        {
            public int Compare(WeightedObservedPoint p1, WeightedObservedPoint p2)
            {
                if (p1 == null && p2 == null)
                {
                    return 0;
                }
                if (p1 == null)
                {
                    return -1;
                }
                if (p2 == null)
                {
                    return 1;
                }
                if (p1.X < p2.X)
                {
                    return -1;
                }
                if (p1.X > p2.X)
                {
                    return 1;
                }
                if (p1.Y < p2.Y)
                {
                    return -1;
                }
                if (p1.Y > p2.Y)
                {
                    return 1;
                }
                if (p1.Weight < p2.Weight)
                {
                    return -1;
                }
                if (p1.Weight > p2.Weight)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}
