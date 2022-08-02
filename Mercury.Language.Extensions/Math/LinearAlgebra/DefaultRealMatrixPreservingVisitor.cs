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

namespace Mercury.Language.Math.LinearAlgebra
{
    /// <summary>
    /// Default implementation of the {@link RealMatrixPreservingVisitor} interface.
    /// <p>
    /// This class is a convenience to create custom visitors without defining all
    /// methodsd This class provides default implementations that do nothing.
    /// </p>
    /// 
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lund 21 févrd 2011) $
    /// @since 2.0
    /// </summary>
    public class DefaultRealMatrixPreservingVisitor : IRealMatrixPreservingVisitor
    {
        public Func<int, int, int, int, int, int, double> startfunc;
        public Func<int, int, double, double> visitfunc;
        public Func<double> endfunc;

        /// <summary>{@inheritDoc} */
        public double Start(int rows, int columns, int startRow, int endRow, int startColumn, int endColumn)
        {
            return startfunc(rows, columns, startRow, endRow, startColumn, endColumn);
        }

        /// <summary>{@inheritDoc} */
        public double Visit(int row, int column, double value)
        {
            return visitfunc(row, column, value);
        }

        /// <summary>{@inheritDoc} */
        public double End()
        {
            return endfunc();
        }
    }
}
