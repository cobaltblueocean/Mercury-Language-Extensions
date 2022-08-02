// Copyright (c) 2017 - presented by Kei Nakai
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

namespace Mercury.Language.Math.LinearAlgebra
{
    /// <summary>
    /// Interface defining a visitor for matrix entries.
    /// 
    /// </summary>
    /// <see cref="DefaultRealMatrixPreservingVisitor"></see>
    /// @version $Revision: 1073158 $ $Date: 2011-02-21 22:46:52 +0100 (lun. 21 févr. 2011) $
    /// @since 2.0
    public interface IRealMatrixPreservingVisitor
    {

        /// <summary>
        /// Start visiting a matrix.
        /// <p>This method is called once before any entry of the matrix is visited.</p>
        /// </summary>
        /// <param name="rows">number of rows of the matrix</param>
        /// <param name="columns">number of columns of the matrix</param>
        /// <param name="startRow">Initial row index</param>
        /// <param name="endRow">Final row index (inclusive)</param>
        /// <param name="startColumn">Initial column index</param>
        /// <param name="endColumn">Final column index (inclusive)</param>
        double Start(int rows, int columns, int startRow, int endRow, int startColumn, int endColumn);

        /// <summary>
        /// Visit one matrix entry.
        /// </summary>
        /// <param name="row">row index of the entry</param>
        /// <param name="column">column index of the entry</param>
        /// <param name="value">current value of the entry</param>
        /// <exception cref="MatrixVisitorException">if something wrong occurs </exception>
        double Visit(int row, int column, double value);

        /// <summary>
        /// End visiting a matrix.
        /// <p>This method is called once after all entries of the matrix have been visited.</p>
        /// </summary>
        /// <returns>the value that the <code>walkInXxxOrder</code> must return</returns>
        double End();
    }
}
