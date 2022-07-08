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
using Mercury.Language.Math.Transform.DCT;

namespace Mercury.Language.Math.Transform.DST
{
    /// <summary>
    /// Computes 1D Discrete Sine Transform (DST) of double precision datad The size
    /// of data can be an arbitrary numberd It uses DCT algorithmd This is a parallel
    /// implementation optimized for SMP systems.
    /// 
    /// @author Piotr Wendykier (piotr.wendykier@gmail.com)
    /// 
    /// </summary>
    public class DoubleDST_1D
    {
        private int n;

        private DoubleDCT_1D dct;

        /// <summary>
        /// Creates new instance of DoubleDST_1D.
        /// 
        /// </summary>
        /// <param Name="n"></param>
        ///            size of data
        public DoubleDST_1D(int n)
        {
            this.n = n;
            dct = new DoubleDCT_1D(n);
        }

        /// <summary>
        /// Computes 1D forward DST (DST-II) leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Forward(double[] a, Boolean isScale)
        {
            Forward(a, 0, isScale);
        }

        /// <summary>
        /// Computes 1D forward DST (DST-II) leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="offa"></param>
        ///            index of the first element in array <code>a</code>
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Forward(double[] a, int offa, Boolean isScale)
        {
            if (n == 1)
                return;
            double tmp;
            int nd2 = n / 2;
            int startIdx = 1 + offa;
            int stopIdx = offa + n;
            for (int i = startIdx; i < stopIdx; i += 2)
            {
                a[i] = -a[i];
            }
            dct.Forward(a, offa, isScale);
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && (nd2 > TransformCore.THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                nthreads = 2;
                int k = nd2 / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int j = 0; j < nthreads; j++)
                {
                    int firstIdx = j * k;
                    int lastIdx = (j == (nthreads - 1)) ? nd2 : firstIdx + k;
                    taskArray[j] = Task.Factory.StartNew(() =>
                    {
                        double tmp;
                        int idx0 = offa + n - 1;
                        int idx1;
                        int idx2;
                        for (int i = firstIdx; i < lastIdx; i++)
                        {
                            idx2 = offa + i;
                            tmp = a[idx2];
                            idx1 = idx0 - i;
                            a[idx2] = a[idx1];
                            a[idx1] = tmp;
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
                int idx0 = offa + n - 1;
                int idx1;
                int idx2;
                for (int i = 0; i < nd2; i++)
                {
                    idx2 = offa + i;
                    tmp = a[idx2];
                    idx1 = idx0 - i;
                    a[idx2] = a[idx1];
                    a[idx1] = tmp;
                }
            }
        }

        /// <summary>
        /// Computes 1D inverse DST (DST-III) leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(double[] a, Boolean isScale)
        {
            Inverse(a, 0, isScale);
        }

        /// <summary>
        /// Computes 1D inverse DST (DST-III) leaving the result in <code>a</code>.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="offa"></param>
        ///            index of the first element in array <code>a</code>
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(double[] a, int offa, Boolean isScale)
        {
            if (n == 1)
                return;
            double tmp;
            int nd2 = n / 2;
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && (nd2 > TransformCore.THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                nthreads = 2;
                int k = nd2 / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int j = 0; j < nthreads; j++)
                {
                    int firstIdx = j * k;
                    int lastIdx = (j == (nthreads - 1)) ? nd2 : firstIdx + k;
                    taskArray[j] = Task.Factory.StartNew(() =>
                    {
                        double tmp;
                        int idx0 = offa + n - 1;
                        int idx1, idx2;
                        for (int i = firstIdx; i < lastIdx; i++)
                        {
                            idx2 = offa + i;
                            tmp = a[idx2];
                            idx1 = idx0 - i;
                            a[idx2] = a[idx1];
                            a[idx1] = tmp;
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
                int idx0 = offa + n - 1;
                for (int i = 0; i < nd2; i++)
                {
                    tmp = a[offa + i];
                    a[offa + i] = a[idx0 - i];
                    a[idx0 - i] = tmp;
                }
            }
            dct.Inverse(a, offa, isScale);
            int startidx = 1 + offa;
            int stopidx = offa + n;
            for (int i = startidx; i < stopidx; i += 2)
            {
                a[i] = -a[i];
            }
        }
    }
}
