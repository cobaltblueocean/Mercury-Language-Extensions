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

/* ***** BEGIN LICENSE BLOCK *****
* Version: MPL 1.1/GPL 2.0/LGPL 2.1
*
* The contents of this file are subject to the Mozilla Public License Version
* 1.1 (the "License"); you may not use this file except in compliance with
* the Licensed You may obtain a copy of the License at
* http://www.mozilla.org/MPL/
*
* Software distributed under the License is distributed on an "AS IS" basis,
* WITHOUT WARRANTY OF ANY KIND, either express or impliedd See the License
* for the specific language governing rights and limitations under the
* License.
*
* The Original Code is JTransforms.
*
* The Initial Developer of the Original Code is
* Piotr Wendykier, Emory University.
* Portions created by the Initial Developer are Copyright (C) 2007-2009
* the Initial Developerd All Rights Reserved.
*
* Alternatively, the contents of this file may be used under the terms of
* either the GNU General Public License Version 2 or later (the "GPL"), or
* the GNU Lesser General Public License Version 2.1 or later (the "LGPL"),
* in which case the provisions of the GPL or the LGPL are applicable instead
* of those aboved If you wish to allow use of your version of this file only
* under the terms of either the GPL or the LGPL, and not to allow others to
* use your version of this file under the terms of the MPL, indicate your
* decision by deleting the provisions above and replace them with the notice
* and other provisions required by the GPL or the LGPLd If you do not delete
* the provisions above, a recipient may use your version of this file under
* the terms of any one of the MPL, the GPL or the LGPL.
*
* ***** END LICENSE BLOCK ***** */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Log;
using Mercury.Language.Math.Transform.FFT;

namespace Mercury.Language.Math.Transform.DHT
{

    /// <summary>
    /// Computes 1D Discrete Hartley Transform (DHT) of real, single precision data.
    /// The size of the data can be an arbitrary numberd It uses FFT algorithmd This
    /// is a parallel implementation optimized for SMP systems.
    /// 
    /// @author Piotr Wendykier (piotr.wendykier@gmail.com)
    /// </summary>
    public class FloatDHT_1D
    {
        private int n;
        private FloatFFT_1D fft;

        /// <summary>
        /// Creates new instance of FloatDHT_1D.
        /// 
        /// </summary>
        /// <param Name="n"></param>
        ///            size of data
        public FloatDHT_1D(int n)
        {
            this.n = n;
            fft = new FloatFFT_1D(n);
        }

        /// <summary>
        /// Computes 1D real, forward DHT leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        public void Forward(float[] a)
        {
            Forward(a, 0);
        }

        /// <summary>
        /// Computes 1D real, forward DHT leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="offa"></param>
        ///            index of the first element in array <code>a</code>
        public void Forward(float[] a, int offa)
        {
            if (n == 1)
                return;
            fft.RealForward(a, offa);
            float[] b = new float[n];
            Array.Copy(a, offa, b, 0, n);
            int nd2 = n / 2;
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && (nd2 > TransformCore.THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                nthreads = 2;
                int k1 = nd2 / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = 1 + i * k1;
                    int lastIdx = (i == (nthreads - 1)) ? nd2 : firstIdx + k1;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        int idx1, idx2;
                        for (int i = firstIdx; i < lastIdx; i++)
                        {
                            idx1 = 2 * i;
                            idx2 = idx1 + 1;
                            a[offa + i] = b[idx1] - b[idx2];
                            a[offa + n - i] = b[idx1] + b[idx2];
                        }
                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                int idx1, idx2;
                for (int i = 1; i < nd2; i++)
                {
                    idx1 = 2 * i;
                    idx2 = idx1 + 1;
                    a[offa + i] = b[idx1] - b[idx2];
                    a[offa + n - i] = b[idx1] + b[idx2];
                }
            }
            if ((n % 2) == 0)
            {
                a[offa + nd2] = b[1];
            }
            else
            {
                a[offa + nd2] = b[n - 1] - b[1];
                a[offa + nd2 + 1] = b[n - 1] + b[1];
            }

        }

        /// <summary>
        /// Computes 1D real, inverse DHT leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(float[] a, Boolean isScale)
        {
            Inverse(a, 0, isScale);
        }

        /// <summary>
        /// Computes 1D real, inverse DHT leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="offa"></param>
        ///            index of the first element in array <code>a</code>
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(float[] a, int offa, Boolean isScale)
        {
            if (n == 1)
                return;
            Forward(a, offa);
            if (isScale)
            {
                scale(n, a, offa);
            }
        }

        private void scale(float m, float[] a, int offa)
        {
            float norm = (float)(1.0 / m);
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && (n >= TransformCore.THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                nthreads = 2;
                int k = n / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = offa + i * k;
                    int lastIdx = (i == (nthreads - 1)) ? offa + n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (int i = firstIdx; i < lastIdx; i++)
                        {
                            a[i] *= norm;
                        }
                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                int lastIdx = offa + n;
                for (int i = offa; i < lastIdx; i++)
                {
                    a[i] *= norm;
                }

            }
        }
    }
}
