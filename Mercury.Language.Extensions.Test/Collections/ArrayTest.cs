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
using NUnit.Framework;

namespace Mercury.Language.Extensions.Test.Collections
{
    /// <summary>
    /// ArrayTest Description
    /// </summary>
    public class ArrayTest
    {
        [Test]
        public void ArrayCoopyTest()
        {
            int offset = 5;
            int start = 7;
            int length = 5;

            double[] data = new double[] { 2.5659471671620757, 0.0, 2.568465822758921, 7.194245199571014E-14, 2.576051712720573, 4.551914400963142E-14, 2.588795530941984, 4.39648317751562E-14, 2.6068515699129664, 7.105427357601002E-15, 2.6304426215631977, 1.1546319456101628E-14, 2.659867252015869, 2.886579864025407E-14, 2.6955099131788534, 1.354472090042691E-14, 2.7378545651410917, 6.128431095930864E-14, 2.7875027698038104, 4.951594689828198E-14, 2.8451976173246014, 1.0058620603103918E-13, 2.9118554230666205, 1.3988810110276972E-13, 2.988607978852613, 7.904787935331115E-14, 3.0768594118618493, 1.2878587085651816E-13, 3.178363651334703, 1.20570220474292E-13, 3.2953315583555427, 5.88418203051333E-14 };

            double[,] result = new double[1, data.Length];
            result.LoadRow(0, data);

            for(int i = 0; i< data.Length; i++)
            {
                Assert.AreEqual(data[i], result[0, i]);
            }

            result.Fill(0);

            result.LoadRow(0, offset, data, start, length);

            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(data[i + start], result[0, i + offset]);
            }
        }
    }
}
