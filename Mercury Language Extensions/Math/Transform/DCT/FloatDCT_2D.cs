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

namespace Mercury.Language.Math.Transform.DCT
{
    /// <summary>
    /// Computes 2D Discrete Cosine Transform (DCT) of single precision datad The
    /// sizes of both dimensions can be arbitrary numbersd This is a parallel
    /// implementation of split-radix and mixed-radix algorithms optimized for SMP
    /// systemsd <br>
    /// <br>
    /// Part of the code is derived from General Purpose FFT Package written by Takuya Ooura
    /// (http://www.kurims.kyoto-u.ac.jp/~ooura/fft.html)
    /// 
    /// @author Piotr Wendykier (piotr.wendykier@gmail.com)
    /// 
    /// </summary>
    public class FloatDCT_2D
    {
        private int rows;

        private int columns;

        private float[] t;

        private FloatDCT_1D dctColumns, dctRows;

        private int nt;

        private int oldNthreads;

        private Boolean isPowerOfTwo = false;

        private Boolean useThreads = false;

        /// <summary>
        /// Creates new instance of FloatDCT_2D.
        /// 
        /// </summary>
        /// <param Name="rows"></param>
        ///            number of rows
        /// <param Name="columns"></param>
        ///            number of columns
        public FloatDCT_2D(int rows, int columns)
        {
            if (rows <= 1 || columns <= 1)
            {
                throw new ArgumentException("rows and columns must be greater than 1");
            }
            this.rows = rows;
            this.columns = columns;
            if (rows * columns >= Core.THREADS_BEGIN_N_2D)
            {
                this.useThreads = true;
            }
            if (rows.IsPowerOf2() && columns.IsPowerOf2())
            {
                isPowerOfTwo = true;
                oldNthreads = Process.GetCurrentProcess().Threads.Count;
                nt = 4 * oldNthreads * rows;
                if (columns == 2 * oldNthreads)
                {
                    nt >>= 1;
                }
                else if (columns < 2 * oldNthreads)
                {
                    nt >>= 2;
                }
                t = new float[nt];
            }
            dctColumns = new FloatDCT_1D(columns);
            if (columns == rows)
            {
                dctRows = dctColumns;
            }
            else
            {
                dctRows = new FloatDCT_1D(rows);
            }
        }

        /// <summary>
        /// Computes 2D forward DCT (DCT-II) leaving the result in <code>a</code>.
        /// The data is stored in 1D array in row-major order.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Forward(float[] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = 4 * nthreads * rows;
                    if (columns == 2 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 2 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt2d_subth(-1, a, isScale);
                    ddxt2d0_subth(-1, a, isScale);
                }
                else
                {
                    ddxt2d_sub(-1, a, isScale);
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Forward(a, i * columns, isScale);
                    }
                }
            }
            else
            {
                if ((nthreads > 1) && useThreads && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int r = firstRow; r < lastRow; r++)
                            {
                                dctColumns.Forward(a, r * columns, isScale);
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
                    p = columns / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstColumn = l * p;
                        int lastColumn = (l == (nthreads - 1)) ? columns : firstColumn + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            float[] temp = new float[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r * columns + c];
                                }
                                dctRows.Forward(temp, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    a[r * columns + c] = temp[r];
                                }
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
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Forward(a, i * columns, isScale);
                    }
                    float[] temp = new float[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r * columns + c];
                        }
                        dctRows.Forward(temp, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r * columns + c] = temp[r];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D forward DCT (DCT-II) leaving the result in <code>a</code>.
        /// The data is stored in 2D array.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Forward(float[][] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = 4 * nthreads * rows;
                    if (columns == 2 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 2 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt2d_subth(-1, a, isScale);
                    ddxt2d0_subth(-1, a, isScale);
                }
                else
                {
                    ddxt2d_sub(-1, a, isScale);
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Forward(a[i], isScale);
                    }
                }
            }
            else
            {
                if ((nthreads > 1) && useThreads && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int i = firstRow; i < lastRow; i++)
                            {
                                dctColumns.Forward(a[i], isScale);
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
                    p = columns / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstColumn = l * p;
                        int lastColumn = (l == (nthreads - 1)) ? columns : firstColumn + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            float[] temp = new float[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r][c];
                                }
                                dctRows.Forward(temp, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    a[r][c] = temp[r];
                                }
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
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Forward(a[i], isScale);
                    }
                    float[] temp = new float[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r][c];
                        }
                        dctRows.Forward(temp, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r][c] = temp[r];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D inverse DCT (DCT-III) leaving the result in <code>a</code>.
        /// The data is stored in 1D array in row-major order.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(float[] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = 4 * nthreads * rows;
                    if (columns == 2 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 2 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt2d_subth(1, a, isScale);
                    ddxt2d0_subth(1, a, isScale);
                }
                else
                {
                    ddxt2d_sub(1, a, isScale);
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Inverse(a, i * columns, isScale);
                    }
                }
            }
            else
            {
                if ((nthreads > 1) && useThreads && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int i = firstRow; i < lastRow; i++)
                            {
                                dctColumns.Inverse(a, i * columns, isScale);
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
                    p = columns / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstColumn = l * p;
                        int lastColumn = (l == (nthreads - 1)) ? columns : firstColumn + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            float[] temp = new float[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r * columns + c];
                                }
                                dctRows.Inverse(temp, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    a[r * columns + c] = temp[r];
                                }
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
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Inverse(a, i * columns, isScale);
                    }
                    float[] temp = new float[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r * columns + c];
                        }
                        dctRows.Inverse(temp, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r * columns + c] = temp[r];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D inverse DCT (DCT-III) leaving the result in <code>a</code>.
        /// The data is stored in 2D array.
        /// 
        /// </summary>
        /// <param Name="a"></param>
        ///            data to transform
        /// <param Name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(float[][] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = 4 * nthreads * rows;
                    if (columns == 2 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 2 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt2d_subth(1, a, isScale);
                    ddxt2d0_subth(1, a, isScale);
                }
                else
                {
                    ddxt2d_sub(1, a, isScale);
                    for (int i = 0; i < rows; i++)
                    {
                        dctColumns.Inverse(a[i], isScale);
                    }
                }
            }
            else
            {
                if ((nthreads > 1) && useThreads && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int i = firstRow; i < lastRow; i++)
                            {
                                dctColumns.Inverse(a[i], isScale);
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
                    p = columns / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstColumn = l * p;
                        int lastColumn = (l == (nthreads - 1)) ? columns : firstColumn + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            float[] temp = new float[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r][c];
                                }
                                dctRows.Inverse(temp, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    a[r][c] = temp[r];
                                }
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
                    for (int r = 0; r < rows; r++)
                    {
                        dctColumns.Inverse(a[r], isScale);
                    }
                    float[] temp = new float[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r][c];
                        }
                        dctRows.Inverse(temp, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r][c] = temp[r];
                        }
                    }
                }
            }
        }

        #region Private Method
        private void ddxt2d_subth(int isgn, float[] a, Boolean isScale)
        {
            int nthread = Process.GetCurrentProcess().Threads.Count;
            int nt = 4 * rows;
            if (columns == 2 * nthread)
            {
                nt >>= 1;
            }
            else if (columns < 2 * nthread)
            {
                nthread = columns;
                nt >>= 2;
            }
            int nthreads = nthread;
            Task[] taskArray = new Task[nthread];

            for (int i = 0; i < nthread; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx1, idx2;
                    if (columns > 2 * nthreads)
                    {
                        if (isgn == -1)
                        {
                            for (int c = 4 * n0; c < columns; c += 4 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + rows + r;
                                    t[startt + r] = a[idx1];
                                    t[idx2] = a[idx1 + 1];
                                    t[idx2 + rows] = a[idx1 + 2];
                                    t[idx2 + 2 * rows] = a[idx1 + 3];
                                }
                                dctRows.Forward(t, startt, isScale);
                                dctRows.Forward(t, startt + rows, isScale);
                                dctRows.Forward(t, startt + 2 * rows, isScale);
                                dctRows.Forward(t, startt + 3 * rows, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + rows + r;
                                    a[idx1] = t[startt + r];
                                    a[idx1 + 1] = t[idx2];
                                    a[idx1 + 2] = t[idx2 + rows];
                                    a[idx1 + 3] = t[idx2 + 2 * rows];
                                }
                            }
                        }
                        else
                        {
                            for (int c = 4 * n0; c < columns; c += 4 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + rows + r;
                                    t[startt + r] = a[idx1];
                                    t[idx2] = a[idx1 + 1];
                                    t[idx2 + rows] = a[idx1 + 2];
                                    t[idx2 + 2 * rows] = a[idx1 + 3];
                                }
                                dctRows.Inverse(t, startt, isScale);
                                dctRows.Inverse(t, startt + rows, isScale);
                                dctRows.Inverse(t, startt + 2 * rows, isScale);
                                dctRows.Inverse(t, startt + 3 * rows, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + rows + r;
                                    a[idx1] = t[startt + r];
                                    a[idx1 + 1] = t[idx2];
                                    a[idx1 + 2] = t[idx2 + rows];
                                    a[idx1 + 3] = t[idx2 + 2 * rows];
                                }
                            }
                        }
                    }
                    else if (columns == 2 * nthreads)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + 2 * n0;
                            idx2 = startt + r;
                            t[idx2] = a[idx1];
                            t[idx2 + rows] = a[idx1 + 1];
                        }
                        if (isgn == -1)
                        {
                            dctRows.Forward(t, startt, isScale);
                            dctRows.Forward(t, startt + rows, isScale);
                        }
                        else
                        {
                            dctRows.Inverse(t, startt, isScale);
                            dctRows.Inverse(t, startt + rows, isScale);
                        }
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + 2 * n0;
                            idx2 = startt + r;
                            a[idx1] = t[idx2];
                            a[idx1 + 1] = t[idx2 + rows];
                        }
                    }
                    else if (columns == nthreads)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            t[startt + r] = a[r * columns + n0];
                        }
                        if (isgn == -1)
                        {
                            dctRows.Forward(t, startt, isScale);
                        }
                        else
                        {
                            dctRows.Inverse(t, startt, isScale);
                        }
                        for (int r = 0; r < rows; r++)
                        {
                            a[r * columns + n0] = t[startt + r];
                        }
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

        private void ddxt2d_subth(int isgn, float[][] a, Boolean isScale)
        {
            int nthread = Process.GetCurrentProcess().Threads.Count;
            int nt = 4 * rows;
            if (columns == 2 * nthread)
            {
                nt >>= 1;
            }
            else if (columns < 2 * nthread)
            {
                nthread = columns;
                nt >>= 2;
            }
            int nthreads = nthread;
            Task[] taskArray = new Task[nthread];

            for (int i = 0; i < nthread; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx2;
                    if (columns > 2 * nthreads)
                    {
                        if (isgn == -1)
                        {
                            for (int c = 4 * n0; c < columns; c += 4 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + rows + r;
                                    t[startt + r] = a[r][c];
                                    t[idx2] = a[r][c + 1];
                                    t[idx2 + rows] = a[r][c + 2];
                                    t[idx2 + 2 * rows] = a[r][c + 3];
                                }
                                dctRows.Forward(t, startt, isScale);
                                dctRows.Forward(t, startt + rows, isScale);
                                dctRows.Forward(t, startt + 2 * rows, isScale);
                                dctRows.Forward(t, startt + 3 * rows, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + rows + r;
                                    a[r][c] = t[startt + r];
                                    a[r][c + 1] = t[idx2];
                                    a[r][c + 2] = t[idx2 + rows];
                                    a[r][c + 3] = t[idx2 + 2 * rows];
                                }
                            }
                        }
                        else
                        {
                            for (int c = 4 * n0; c < columns; c += 4 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + rows + r;
                                    t[startt + r] = a[r][c];
                                    t[idx2] = a[r][c + 1];
                                    t[idx2 + rows] = a[r][c + 2];
                                    t[idx2 + 2 * rows] = a[r][c + 3];
                                }
                                dctRows.Inverse(t, startt, isScale);
                                dctRows.Inverse(t, startt + rows, isScale);
                                dctRows.Inverse(t, startt + 2 * rows, isScale);
                                dctRows.Inverse(t, startt + 3 * rows, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + rows + r;
                                    a[r][c] = t[startt + r];
                                    a[r][c + 1] = t[idx2];
                                    a[r][c + 2] = t[idx2 + rows];
                                    a[r][c + 3] = t[idx2 + 2 * rows];
                                }
                            }
                        }
                    }
                    else if (columns == 2 * nthreads)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = startt + r;
                            t[idx2] = a[r][2 * n0];
                            t[idx2 + rows] = a[r][2 * n0 + 1];
                        }
                        if (isgn == -1)
                        {
                            dctRows.Forward(t, startt, isScale);
                            dctRows.Forward(t, startt + rows, isScale);
                        }
                        else
                        {
                            dctRows.Inverse(t, startt, isScale);
                            dctRows.Inverse(t, startt + rows, isScale);
                        }
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = startt + r;
                            a[r][2 * n0] = t[idx2];
                            a[r][2 * n0 + 1] = t[idx2 + rows];
                        }
                    }
                    else if (columns == nthreads)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            t[startt + r] = a[r][n0];
                        }
                        if (isgn == -1)
                        {
                            dctRows.Forward(t, startt, isScale);
                        }
                        else
                        {
                            dctRows.Inverse(t, startt, isScale);
                        }
                        for (int r = 0; r < rows; r++)
                        {
                            a[r][n0] = t[startt + r];
                        }
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

        private void ddxt2d0_subth(int isgn, float[] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;

            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    if (isgn == -1)
                    {
                        for (int r = n0; r < rows; r += nthreads)
                        {
                            dctColumns.Forward(a, r * columns, isScale);
                        }
                    }
                    else
                    {
                        for (int r = n0; r < rows; r += nthreads)
                        {
                            dctColumns.Inverse(a, r * columns, isScale);
                        }
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

        private void ddxt2d0_subth(int isgn, float[][] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;

            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    if (isgn == -1)
                    {
                        for (int r = n0; r < rows; r += nthreads)
                        {
                            dctColumns.Forward(a[r], isScale);
                        }
                    }
                    else
                    {
                        for (int r = n0; r < rows; r += nthreads)
                        {
                            dctColumns.Inverse(a[r], isScale);
                        }
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

        private void ddxt2d_sub(int isgn, float[] a, Boolean isScale)
        {
            int idx1, idx2;

            if (columns > 2)
            {
                if (isgn == -1)
                {
                    for (int c = 0; c < columns; c += 4)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = rows + r;
                            t[r] = a[idx1];
                            t[idx2] = a[idx1 + 1];
                            t[idx2 + rows] = a[idx1 + 2];
                            t[idx2 + 2 * rows] = a[idx1 + 3];
                        }
                        dctRows.Forward(t, 0, isScale);
                        dctRows.Forward(t, rows, isScale);
                        dctRows.Forward(t, 2 * rows, isScale);
                        dctRows.Forward(t, 3 * rows, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = rows + r;
                            a[idx1] = t[r];
                            a[idx1 + 1] = t[idx2];
                            a[idx1 + 2] = t[idx2 + rows];
                            a[idx1 + 3] = t[idx2 + 2 * rows];
                        }
                    }
                }
                else
                {
                    for (int c = 0; c < columns; c += 4)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = rows + r;
                            t[r] = a[idx1];
                            t[idx2] = a[idx1 + 1];
                            t[idx2 + rows] = a[idx1 + 2];
                            t[idx2 + 2 * rows] = a[idx1 + 3];
                        }
                        dctRows.Inverse(t, 0, isScale);
                        dctRows.Inverse(t, rows, isScale);
                        dctRows.Inverse(t, 2 * rows, isScale);
                        dctRows.Inverse(t, 3 * rows, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = rows + r;
                            a[idx1] = t[r];
                            a[idx1 + 1] = t[idx2];
                            a[idx1 + 2] = t[idx2 + rows];
                            a[idx1 + 3] = t[idx2 + 2 * rows];
                        }
                    }
                }
            }
            else if (columns == 2)
            {
                for (int r = 0; r < rows; r++)
                {
                    idx1 = r * columns;
                    t[r] = a[idx1];
                    t[rows + r] = a[idx1 + 1];
                }
                if (isgn == -1)
                {
                    dctRows.Forward(t, 0, isScale);
                    dctRows.Forward(t, rows, isScale);
                }
                else
                {
                    dctRows.Inverse(t, 0, isScale);
                    dctRows.Inverse(t, rows, isScale);
                }
                for (int r = 0; r < rows; r++)
                {
                    idx1 = r * columns;
                    a[idx1] = t[r];
                    a[idx1 + 1] = t[rows + r];
                }
            }
        }

        private void ddxt2d_sub(int isgn, float[][] a, Boolean isScale)
        {
            int idx2;

            if (columns > 2)
            {
                if (isgn == -1)
                {
                    for (int c = 0; c < columns; c += 4)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = rows + r;
                            t[r] = a[r][c];
                            t[idx2] = a[r][c + 1];
                            t[idx2 + rows] = a[r][c + 2];
                            t[idx2 + 2 * rows] = a[r][c + 3];
                        }
                        dctRows.Forward(t, 0, isScale);
                        dctRows.Forward(t, rows, isScale);
                        dctRows.Forward(t, 2 * rows, isScale);
                        dctRows.Forward(t, 3 * rows, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = rows + r;
                            a[r][c] = t[r];
                            a[r][c + 1] = t[idx2];
                            a[r][c + 2] = t[idx2 + rows];
                            a[r][c + 3] = t[idx2 + 2 * rows];
                        }
                    }
                }
                else
                {
                    for (int c = 0; c < columns; c += 4)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = rows + r;
                            t[r] = a[r][c];
                            t[idx2] = a[r][c + 1];
                            t[idx2 + rows] = a[r][c + 2];
                            t[idx2 + 2 * rows] = a[r][c + 3];
                        }
                        dctRows.Inverse(t, 0, isScale);
                        dctRows.Inverse(t, rows, isScale);
                        dctRows.Inverse(t, 2 * rows, isScale);
                        dctRows.Inverse(t, 3 * rows, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = rows + r;
                            a[r][c] = t[r];
                            a[r][c + 1] = t[idx2];
                            a[r][c + 2] = t[idx2 + rows];
                            a[r][c + 3] = t[idx2 + 2 * rows];
                        }
                    }
                }
            }
            else if (columns == 2)
            {
                for (int r = 0; r < rows; r++)
                {
                    t[r] = a[r][0];
                    t[rows + r] = a[r][1];
                }
                if (isgn == -1)
                {
                    dctRows.Forward(t, 0, isScale);
                    dctRows.Forward(t, rows, isScale);
                }
                else
                {
                    dctRows.Inverse(t, 0, isScale);
                    dctRows.Inverse(t, rows, isScale);
                }
                for (int r = 0; r < rows; r++)
                {
                    a[r][0] = t[r];
                    a[r][1] = t[rows + r];
                }
            }
        }
#endregion
    }
}
