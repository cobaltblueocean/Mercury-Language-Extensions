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
using Mercury.Language.Math;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;
using Mercury.Language;
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Language.Math.Random;
using Mercury.Language.Math.Matrix;
using MathNet.Numerics.LinearAlgebra;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Interpolating function that  :  the
    /// <a href="http://www.dudziak.com/microsphere.php">Microsphere Projection</a>.
    /// 
    /// @version $Revision: 990655 $ $Date: 2010-08-29 23:49:40 +0200 (dimd 29 août 2010) $
    /// </summary>
    public class MicrosphereInterpolatingFunction : IMultivariateRealFunction
    {
        /// <summary>
        /// Space dimension.
        /// </summary>
        private int dimension;
        /// <summary>
        /// Internal accounting data for the interpolation algorithm.
        /// Each element of the list corresponds to one surface element of
        /// the microsphere.
        /// </summary>
        private List<MicrosphereSurfaceElement> microsphere;
        /// <summary>
        /// Exponent used in the power law that computes the weights of the
        /// sample data.
        /// </summary>
        private double brightnessExponent;
        /// <summary>
        /// Sample data.
        /// </summary>
        private Dictionary<Vector<Double>, Double?> samples;

        public double[] ParamValue => throw new NotImplementedException();

        /// <summary>
        /// Class for storing the accounting data needed to perform the
        /// microsphere projection.
        /// </summary>
        private class MicrosphereSurfaceElement
        {

            /// <summary>Normal vector characterizing a surface elementd */
            private Vector<Double> _normal;

            /// <summary>Illumination received from the brightest sampled */
            private double brightestIllumination;

            /// <summary>Brightest sampled */
            private KeyValuePair<Vector<Double>, Double?> brightestSample;

            private KeyValuePair<Vector<Double>, Double?> _NULL = new KeyValuePair<Vector<Double>, Double?>(null, null);

            /// <summary>
            /// </summary>
            /// <param Name="n">Normal vector characterizing a surface element</param>
            /// of the microsphere.
            public MicrosphereSurfaceElement(double[] n)
            {
                _normal = MathNetMatrixUtility.CreateRealVector(n);
            }

            /// <summary>
            /// Return the normal vector.
            /// </summary>
            /// <returns>the normal vector</returns>
            public Vector<Double> Normal
            {
                get { return _normal; }
            }

            /// <summary>
            /// Reset "illumination" and "sampleIndex".
            /// </summary>
            public void Reset()
            {
                brightestIllumination = 0;
                brightestSample = _NULL;
            }

            /// <summary>
            /// Store the illumination and index of the brightest sample.
            /// </summary>
            /// <param Name="illuminationFromSample">illumination received from sample</param>
            /// <param Name="sample">current sample illuminating the element</param>
            public void Store(double illuminationFromSample, KeyValuePair<Vector<Double>, Double?> sample)
            {
                if (illuminationFromSample > this.brightestIllumination)
                {
                    this.brightestIllumination = illuminationFromSample;
                    this.brightestSample = sample;
                }
            }

            /// <summary>
            /// Get the illumination of the element.
            /// </summary>
            /// <returns>the illumination.</returns>
            public double Illumination()
            {
                return brightestIllumination;
            }

            /// <summary>
            /// Get the sample illuminating the element the most.
            /// </summary>
            /// <returns>the sample.</returns>
            public KeyValuePair<Vector<Double>, Double?> Sample()
            {
                return brightestSample;
            }
        }

        /// <summary>
        /// </summary>
        /// <param Name="xval">the arguments for the interpolation points.</param>
        /// {@code xval[i][0]} is the first component of interpolation point
        /// {@code i}, {@code xval[i][1]} is the second component, and so on
        /// until {@code xval[i][d-1]}, the last component of that interpolation
        /// point (where {@code dimension} is thus the dimension of the sampled
        /// space).
        /// <param Name="yval">the values for the interpolation points</param>
        /// <param Name="brightnessExponent">Brightness dimming factor.</param>
        /// <param Name="microsphereElements">Number of surface elements of the</param>
        /// microsphere.
        /// <param Name="rand">Unit vector generator for creating the microsphere.</param>
        /// <exception cref="DimensionMismatchException">if the lengths of {@code yval} and </exception>
        /// {@code xval} (equal to {@code n}, the number of interpolation points)
        /// do not match, or the the arrays {@code xval[0]} [] {@code xval[n]},
        /// have lengths different from {@code dimension}.
        /// <exception cref="DataNotFoundException">if there are no data (xval null or zero Length) </exception>
        public MicrosphereInterpolatingFunction(double[][] xval,
                                                double[] yval,
                                                int brightnessExponent,
                                                int microsphereElements,
                                                UnitSphereRandomVectorGenerator rand)
        {
            if (xval.Length == 0 || xval[0] == null)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().NO_DATA);
            }

            if (xval.Length != yval.Length)
            {
                throw new DimensionMismatchException(xval.Length, yval.Length);
            }

            dimension = xval.GetLength(1);
            this.brightnessExponent = brightnessExponent;

            // Copy data samples.
            samples = new Dictionary<Vector<Double>, Double?>(yval.Length);
            AutoParallel.AutoParallelFor(0, xval.Length, (i) =>
            {
                double[] xvalI = xval[i];
                if (xvalI.Length != dimension)
                {
                    throw new DimensionMismatchException(xvalI.Length, dimension);
                }

                samples.AddOrUpdate(MathNetMatrixUtility.CreateRealVector(xvalI), yval[i]);
            });

            microsphere = new List<MicrosphereSurfaceElement>(microsphereElements);
            // Generate the microsphere, assuming that a fairly large number of
            // randomly generated normals will represent a sphere.
            AutoParallel.AutoParallelFor(0, microsphereElements, (i) =>
            {
                microsphere.Add(new MicrosphereSurfaceElement(rand.NextVector()));
            });

        }

        /// <summary>
        /// </summary>
        /// <param Name="point">Interpolation point.</param>
        /// <returns>the interpolated value.</returns>
        public double Value(double[] point)
        {

            Vector<Double> p = MathNetMatrixUtility.CreateRealVector(point);

            // Reset.
            AutoParallel.AutoParallelForEach(microsphere, (md) =>
            {
                md.Reset();
            });

            // Compute contribution of each sample points to the microsphere elements illumination
            foreach (var sd in samples)
            {

                // Vector between interpolation point and current sample point.
                Vector<Double> diff = sd.Key.Subtract(p);
                double diffNorm = diff.Norm();

                if (System.Math.Abs(diffNorm) < QuickMath.Ulp(1d))
                {
                    // No need to interpolate, as the interpolation point is
                    // actually (very close to) one of the sampled points.
                    return sd.Value.Value;
                }

                foreach (MicrosphereSurfaceElement md in microsphere)
                {
                    double w = System.Math.Pow(diffNorm, -brightnessExponent);
                    md.Store(cosAngle(diff, md.Normal) * w, sd);
                }
            }

            // Interpolation calculation.
            double value = 0;
            double totalWeight = 0;
            AutoParallel.AutoParallelForEach(microsphere, (md) =>
            {
                double iV = md.Illumination();
                KeyValuePair<Vector<Double>, Double?> sd = md.Sample();
                if (sd.Key != null && sd.Value != null)
                {
                    value += iV * sd.Value.Value;
                    totalWeight += iV;
                }
            });

            return value / totalWeight;
        }

        /// <summary>
        /// Compute the cosine of the angle between 2 vectors.
        /// 
        /// </summary>
        /// <param Name="v">Vector.</param>
        /// <param Name="w">Vector.</param>
        /// <returns>cosine of the angle</returns>
        private double cosAngle(Vector<Double> v, Vector<Double> w)
        {
            return v.DotProduct(w) / (v.Norm() * w.Norm());
        }
    }
}
