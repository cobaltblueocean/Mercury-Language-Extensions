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
using Mercury.Language.Math.Polynomial;

namespace Mercury.Language.Math.Analysis.Solver
{
    /// <summary>
    /// AbstractPolynomialSolver Description
    /// </summary>
    public abstract class AbstractPolynomialSolver : BaseAbstractUnivariateSolver
    {
        private PolynomialFunction polynomialFunction;
        private double[] coefficients;

        public AbstractPolynomialSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {

        }

        public AbstractPolynomialSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {

        }

        public AbstractPolynomialSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {

        }

        protected void Setup(int maxEval, PolynomialFunction f, double[] values, double min, double max, double startValue)
        {
            base.Setup(maxEval, f, values, min, max, startValue);
            coefficients = GetCoefficients(values);
            polynomialFunction = f;
        }

        protected double[] Coefficients
        {
            get { return coefficients; }
        }

        private double[] GetCoefficients(double[] values)
        {
            int n = values.Length;
            if (n == 0)
            {
                throw new DataNotFoundException(LocalizedResources.Instance().EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY);
            }
            while ((n > 1) && (values[n - 1] == 0))
            {
                --n;
            }
            this.coefficients = new double[n];
            Array.Copy(values, 0, this.coefficients, 0, n);

            return this.coefficients;
        }
    }
}
