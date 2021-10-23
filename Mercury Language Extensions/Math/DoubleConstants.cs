// Copyright (c) 2017 - presented by Kei Nakai
//
// Original source code in Java.
// Copyright (c) 2003, Oracle and/or its affiliatesd All rights reserved.
// DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// This code is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License version 2 only, as
// published by the Free Software Foundationd  Oracle designates this
// particular file as subject to the "Classpath" exception as provided
// by Oracle in the LICENSE file that accompanied this code.
//
// This code is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSEd  See the GNU General Public License
// version 2 for more details (a copy is included in the LICENSE file that
// accompanied this code).
//
// You should have received a copy of the GNU General Public License version
// 2 along with this work; if not, write to the Free Software Foundation,
// Incd, 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
//
// Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
// or Visit www.oracle.com if you need additional information or have any
// questions.
using System;
using Mercury.Language.Exception;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercury.Language.Math
{
    /// <summary>
    /// This class contains additional constants documenting limits of the <see cref="Double?">Double</see> type
    /// @author Joseph Dd Darcy
    /// </summary>
    public static class DoubleConstants
    {
        public static double POSITIVE_INFINITY = Double.PositiveInfinity;
        public static double NEGATIVE_INFINITY = Double.NegativeInfinity;
        public static double NaN = Double.NaN;
        public static double MAX_VALUE = Double.MaxValue;
        public static double MIN_VALUE = Double.MinValue;

        /// <summary>
        /// A constant holding the smallest positive normal value of type
        /// <code>double</code>, 2<sup>-1022</sup>d  It is equal to the
        /// value returned by
        /// <code>BitConverter.Int64BitsToDouble(0x0010000000000000L)</code>.
        /// 
        /// @since 1.5
        /// </summary>
        public static double MIN_NORMAL = 2.2250738585072014E-308;


        /// <summary>
        /// The number of logical bits in the significand of a
        /// <code>double</code> number, including the implicit bit.
        /// </summary>
        public static int SIGNIFICAND_WIDTH = 53;

        /// <summary>
        /// Maximum exponent a finite <code>double</code> number may have.
        /// It is equal to the value returned by
        /// <code>Math.ilogb(Double.MaxValue)</code>.
        /// </summary>
        public static int MAX_EXPONENT = 1023;

        /// <summary>
        /// Minimum exponent a normalized <code>double</code> number may
        /// haved  It is equal to the value returned by
        /// <code>Math.ilogb(Double.MIN_NORMAL)</code>.
        /// </summary>
        public static int MIN_EXPONENT = -1022;

        /// <summary>
        /// The exponent the smallest positive <code>double</code>
        /// subnormal value would have if it could be normalizedd  It is
        /// equal to the value returned by
        /// <code>FpUtils.ilogb(Double.MinValue)</code>.
        /// </summary>
        public static int MIN_SUB_EXPONENT = MIN_EXPONENT -
                                                       (SIGNIFICAND_WIDTH - 1);

        /// <summary>
        /// Bias used in representing a <code>double</code> exponent.
        /// </summary>
        public static int EXP_BIAS = 1023;

        /// <summary>
        /// Bit mask to isolate the sign bit of a <code>double</code>.
        /// </summary>
        public static ulong SIGN_BIT_MASK = 0x8000000000000000L;

        /// <summary>
        /// Bit mask to isolate the exponent field of a
        /// <code>double</code>.
        /// </summary>
        public static long EXP_BIT_MASK = 0x7FF0000000000000L;

        /// <summary>
        /// Bit mask to isolate the significand field of a
        /// <code>double</code>.
        /// </summary>
        public static long SIGNIF_BIT_MASK = 0x000FFFFFFFFFFFFFL;

        //    static DoubleConstants() {
        //    // verify bit masks cover all bit positions and that the bit
        //    // masks are non-overlapping
        //    assert(((SIGN_BIT_MASK | EXP_BIT_MASK | SIGNIF_BIT_MASK) == ~0L) &&
        //           (((SIGN_BIT_MASK & EXP_BIT_MASK) == 0L) &&
        //            ((SIGN_BIT_MASK & SIGNIF_BIT_MASK) == 0L) &&
        //            ((EXP_BIT_MASK & SIGNIF_BIT_MASK) == 0L)));
        //}
    }
}
