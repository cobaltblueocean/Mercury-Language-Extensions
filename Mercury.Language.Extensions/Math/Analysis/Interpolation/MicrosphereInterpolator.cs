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
using Mercury.Language.Math.Analysis.Function;
using Mercury.Language.Exceptions;

using Mercury.Language.Math.Random;

namespace Mercury.Language.Math.Analysis.Interpolation
{
    /// <summary>
    /// Interpolator that  :  the algorithm described in
    /// <em>William Dudziak</em>'s
    /// <a href="http://www.dudziak.com/microsphere.pdf">MS thesis</a>.
    /// @since 2.1
    /// 
    /// @version $Revision: 980944 $ $Date: 2010-07-30 22:31:11 +0200 (vend 30 juild 2010) $
    /// </summary>
    public class MicrosphereInterpolator : IMultivariateRealInterpolator
    {

        /// <summary>
        /// Default number of surface elements that composes the microsphere.
        /// </summary>
        public static int DEFAULT_MICROSPHERE_ELEMENTS = 2000;

        /// <summary>
        /// Default exponent used the weights calculation.
        /// </summary>
        public static int DEFAULT_BRIGHTNESS_EXPONENT = 2;

        /// <summary>
        /// Number of surface elements of the microsphere.
        /// </summary>
        private int microsphereElements;

        /// <summary>
        /// Exponent used in the power law that computes the weights of the
        /// sample data.
        /// </summary>
        private int brightnessExponent;

        /// <summary>Create a microsphere interpolator with default settings.
        /// <p>Calling this constructor is equivalent to call {@link
        /// #MicrosphereInterpolator(int, int)
        /// MicrosphereInterpolator(MicrosphereInterpolator.DEFAULT_MICROSPHERE_ELEMENTS,
        /// MicrosphereInterpolator.DEFAULT_BRIGHTNESS_EXPONENT)}.</p>
        /// </summary>
        public MicrosphereInterpolator() : this(DEFAULT_MICROSPHERE_ELEMENTS, DEFAULT_BRIGHTNESS_EXPONENT)
        {

        }

        /// <summary>Create a microsphere interpolator.
        /// </summary>
        /// <param Name="microsphereElements">number of surface elements of the microsphere.</param>
        /// <param Name="brightnessExponent">exponent used in the power law that computes the</param>
        /// weights of the sample data.
        /// <exception cref="NotStrictlyPositiveException">if {@code microsphereElements <= 0} </exception>
        /// or {@code brightnessExponent < 0}.
        public MicrosphereInterpolator(int microsphereElements,
                                       int brightnessExponent)
        {
            MicropshereElements = (microsphereElements);
            BrightnessExponent = (brightnessExponent);
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public IMultivariateRealFunction Interpolate(double[][] xval,
                                                    double[] yval)
        {
            UnitSphereRandomVectorGenerator rand
                = new UnitSphereRandomVectorGenerator(xval.GetLength(1));
            return new MicrosphereInterpolatingFunction(xval, yval,
                                                        brightnessExponent,
                                                        microsphereElements,
                                                        rand);
        }

        /// <summary>
        /// Get/Set the brightness exponent.
        /// </summary>
        public int BrightnessExponent
        {
            get { return brightnessExponent; }
            set {
                if (value < 0)
                {
                    throw new NotStrictlyPositiveException(value);
                }
                brightnessExponent = value;
            }
        }

        /// <summary>
        /// Get/Set the number of microsphere elements.
        /// </summary>
        public int MicropshereElements
        {
            get { return microsphereElements; }
            set {
                if (value <= 0)
                {
                    throw new NotStrictlyPositiveException(value);
                }
                microsphereElements = value;
            }
        }
    }
}
