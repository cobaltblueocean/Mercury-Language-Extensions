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

namespace Mercury.Language.Math.Optimization.Linear
{
    /// <summary>
    /// Types of relationships between two cells in a Solver {@link LinearConstraint}.
    /// @version $Revision: 1003886 $ $Date: 2010-10-02 23:04:44 +0200 (sam. 02 oct. 2010) $
    /// @since 2.0
    /// </summary>
    public sealed class Relationship
    {

        /// <summary>
        /// Equality relationship.
        /// /// </summary>
        public static readonly Relationship EQ = new Relationship("=");

        /// <summary>
        /// Lesser than or equal relationship.
        /// /// </summary>
        public static readonly Relationship LEQ = new Relationship("<=");

        /// <summary>
        /// Greater than or equal relationship.
        /// /// </summary>
        public static readonly Relationship GEQ = new Relationship(">=");

        /// <summary>
        /// Display string for the relationship.
        /// /// </summary>
        private String stringValue;

        /// <summary>Simple constructor.
        /// </summary>
        /// <param name="stringValue">display string for the relationship</param>
        private Relationship(String stringValue)
        {
            this.stringValue = stringValue;
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        //@Override
        public override String ToString()
        {
            return stringValue;
        }

        /// <summary>
        /// Get the relationship obtained when multiplying all coefficients by -1.
        /// </summary>
        /// <returns>relationship obtained when multiplying all coefficients by -1</returns>
        public Relationship OppositeRelationship()
        {
            if (this == LEQ)
                return GEQ;
            else if (this == GEQ)
                return LEQ;
            else
                return EQ;
        }
    }
}
