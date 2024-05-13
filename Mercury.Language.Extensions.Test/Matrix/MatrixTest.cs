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
// http://www.apache.org/licenses/LICENSE-2.0
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
using NUnit.Framework;
using Mercury.Language.Math.LinearAlgebra;
using Mercury.Test.Utility;

namespace Mercury.Language.Extensions.Test.Matrix
{
    /// <summary>
    /// MatrixTest Description
    /// </summary>
    public class MatrixTest
    {
        [Test]
        public void QRDecompositionTest()
        {
            Double[,] data = new double[7, 7]{{ 1, -0.37796447300922725, 0.14285714285714288, -0.0539949247156039, 0.020408163265306128, -0.0077135606736577012, 0.002915451895043733}, { 1, -0.37796447300922725, 0.14285714285714288, -0.0539949247156039, 0.020408163265306128, -0.0077135606736577012, 0.002915451895043733}, { 1 , -0.37796447300922725 , 0.14285714285714288 , -0.0539949247156039 , 0.020408163265306128 , -0.0077135606736577012 , 0.002915451895043733 }, { 1 , 2.2677868380553634 , 5.1428571428571432 , 11.66290373857044 , 26.448979591836736 , 59.98064779836227 , 136.02332361516036 }, { 1 , -0.3779644730092272 , 0.14285714285714282 , -0.053994924715603874 , 0.020408163265306117 , -0.0077135606736576951 , 0.0029154518950437304 }, { 1 , -0.3779644730092272 , 0.14285714285714282 , -0.053994924715603874 , 0.020408163265306117 , -0.0077135606736576951 , 0.0029154518950437304 }, { 1 , -0.3779644730092272 , 0.14285714285714282 , -0.053994924715603874 , 0.020408163265306117 , -0.0077135606736576951 , 0.0029154518950437304}};

            Double[,] expResult = new double[7, 7] { { -0.3779644730092271, -0.15430334996209194, 0.6156422468452809, 0.5241854657277955, -0.4237305213840062, -1.0490621510628939E-17, 0.0 }, { -0.3779644730092272, -0.1543033499620918, 0.3628193245154657, -0.8307776171474742, -0.10725680369206846, -5.710283924813574E-18, -1.1331166295920983E-17 }, { -0.3779644730092272, -0.1543033499620919, 0.18839730905203267, 0.16967408879682605, 0.8769552388042696, -1.9187450873933407E-17, -1.1410778796148377E-18 }, { -0.3779644730092272, 0.9258200997725514, 0.0, -2.7755575615628914E-17, 2.7755575615628914E-17, -7.29643436711711E-18, 2.980164663996309E-18 }, { -0.3779644730092272, -0.15430334996209188, -0.3889529601375932, 0.045639354207617513, -0.11532263790939817, -0.5773502691896257, -0.5773502691896257 }, { -0.3779644730092272, -0.15430334996209188, -0.3889529601375932, 0.045639354207617513, -0.11532263790939828, 0.7886751345948129, -0.2113248654051871 }, { -0.3779644730092272, -0.15430334996209188, -0.3889529601375932, 0.045639354207617513, -0.11532263790939828, -0.2113248654051871, 0.7886751345948129 } };

            QRDecomposition qr = new QRDecomposition(data);

            var q = qr.GetQ();

            for(int i = 0; i< 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    Assert2.AreEqual(expResult[i, j], q[i, j]);
                }
            }
        }
    }
}
