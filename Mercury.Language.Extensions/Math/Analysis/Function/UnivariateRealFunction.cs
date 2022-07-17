// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma.Analytics.Math.Function Inc.
//
// Copyright (C) 2012 - present by OpenGamma.Analytics.Math.Function Inc. and the OpenGamma.Analytics.Math.Function group of companies
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math;

namespace Mercury.Language.Math.Analysis.Function
{
    /// <summary>
    /// UnivariateFunction Description
    /// </summary>
    public class UnivariateRealFunction : IUnivariateRealFunction
    {
        public Func<Double?, Double?> function;
        private double _paramValue;

        public double ParamValue
        {
            get { return _paramValue; }
        }

        public UnivariateRealFunction()
        {

        }

        public UnivariateRealFunction(Func<Double?, Double?> f)
        {
            this.function = f;
        }

        public double Value(double x)
        {
            this._paramValue = x;

            return function(x).Value;
        }
    }
}
