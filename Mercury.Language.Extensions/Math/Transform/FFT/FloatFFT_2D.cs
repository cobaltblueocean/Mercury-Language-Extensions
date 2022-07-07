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

namespace Mercury.Language.Math.Transform.FFT
{
    /// <summary>
    /// Computes 2D Discrete Fourier Transform (DFT) of complex and real, single
    /// precision datad The sizes of both dimensions can be arbitrary numbersd This
    /// is a parallel implementation of split-radix and mixed-radix algorithms
    /// optimized for SMP systemsd <br>
    /// <br>
    /// Part of the code is derived from General Purpose FFT Package written by Takuya Ooura
    /// (http://www.kurims.kyoto-u.ac.jp/~ooura/fft.html)
    /// 
    /// @author Piotr Wendykier (piotr.wendykier@gmail.com)
    /// 
    /// </summary>
    public class FloatFFT_2D
    {
        private int rows;

        private int columns;

        private float[] t;

        private FloatFFT_1D fftColumns, fftRows;

        private int oldNthreads;

        private int nt;

        private Boolean isPowerOfTwo = false;

        private Boolean useThreads = false;

        /// <summary>
        /// Creates new instance of FloatFFT_2D.
        /// 
        /// </summary>
        /// <param name="rows"></param>
        ///            number of rows
        /// <param name="columns"></param>
        ///            number of columns
        public FloatFFT_2D(int rows, int columns)
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
                nt = 8 * oldNthreads * rows;
                if (2 * columns == 4 * oldNthreads)
                {
                    nt >>= 1;
                }
                else if (2 * columns < 4 * oldNthreads)
                {
                    nt >>= 2;
                }
                t = new float[nt];
            }
            fftRows = new FloatFFT_1D(rows);
            if (rows == columns)
            {
                fftColumns = fftRows;
            }
            else
            {
                fftColumns = new FloatFFT_1D(columns);
            }
        }

        /// <summary>
        /// Computes 2D forward DFT of complex data leaving the result in
        /// <code>a</code>d The data is stored in 1D array in row-major order.
        /// Complex number is stored as two float values in sequence: the real and
        /// imaginary part, i.ed the input array must be of size rows*2*columnsd The
        /// physical layout of the input data has to be as follows:<br>
        /// 
        /// <pre>
        /// a[k1*2*columns+2*k2] = Re[k1][k2],
        /// a[k1*2*columns+2*k2+1] = Im[k1][k2], 0&lt;=k1&lt;rows, 0&lt;=k2&lt;columns,
        /// </pre>
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void ComplexForward(float[] a)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                int oldn2 = columns;
                columns = 2 * columns;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(0, -1, a, true);
                    cdft2d_subth(-1, a, true);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.ComplexForward(a, r * columns);
                    }
                    cdft2d_sub(-1, a, true);
                }
                columns = oldn2;
            }
            else
            {
                int rowStride = 2 * columns;
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
                                fftColumns.ComplexForward(a, r * rowStride);
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
                            float[] temp = new float[2 * rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                int idx0 = 2 * c;
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx1 = 2 * r;
                                    int idx2 = r * rowStride + idx0;
                                    temp[idx1] = a[idx2];
                                    temp[idx1 + 1] = a[idx2 + 1];
                                }
                                fftRows.ComplexForward(temp);
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx1 = 2 * r;
                                    int idx2 = r * rowStride + idx0;
                                    a[idx2] = temp[idx1];
                                    a[idx2 + 1] = temp[idx1 + 1];
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
                        fftColumns.ComplexForward(a, r * rowStride);
                    }
                    float[] temp = new float[2 * rows];
                    for (int c = 0; c < columns; c++)
                    {
                        int idx0 = 2 * c;
                        for (int r = 0; r < rows; r++)
                        {
                            int idx1 = 2 * r;
                            int idx2 = r * rowStride + idx0;
                            temp[idx1] = a[idx2];
                            temp[idx1 + 1] = a[idx2 + 1];
                        }
                        fftRows.ComplexForward(temp);
                        for (int r = 0; r < rows; r++)
                        {
                            int idx1 = 2 * r;
                            int idx2 = r * rowStride + idx0;
                            a[idx2] = temp[idx1];
                            a[idx2 + 1] = temp[idx1 + 1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D forward DFT of complex data leaving the result in
        /// <code>a</code>d The data is stored in 2D arrayd Complex data is
        /// represented by 2 float values in sequence: the real and imaginary part,
        /// i.ed the input array must be of size rows by 2*columnsd The physical
        /// layout of the input data has to be as follows:<br>
        /// 
        /// <pre>
        /// a[k1][2*k2] = Re[k1][k2],
        /// a[k1][2*k2+1] = Im[k1][k2], 0&lt;=k1&lt;rows, 0&lt;=k2&lt;columns,
        /// </pre>
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void ComplexForward(float[][] a)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                int oldn2 = columns;
                columns = 2 * columns;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(0, -1, a, true);
                    cdft2d_subth(-1, a, true);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.ComplexForward(a[r]);
                    }
                    cdft2d_sub(-1, a, true);
                }
                columns = oldn2;
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
                                fftColumns.ComplexForward(a[r]);
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
                            float[] temp = new float[2 * rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                int idx1 = 2 * c;
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx2 = 2 * r;
                                    temp[idx2] = a[r][idx1];
                                    temp[idx2 + 1] = a[r][idx1 + 1];
                                }
                                fftRows.ComplexForward(temp);
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx2 = 2 * r;
                                    a[r][idx1] = temp[idx2];
                                    a[r][idx1 + 1] = temp[idx2 + 1];
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
                        fftColumns.ComplexForward(a[r]);
                    }
                    float[] temp = new float[2 * rows];
                    for (int c = 0; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        for (int r = 0; r < rows; r++)
                        {
                            int idx2 = 2 * r;
                            temp[idx2] = a[r][idx1];
                            temp[idx2 + 1] = a[r][idx1 + 1];
                        }
                        fftRows.ComplexForward(temp);
                        for (int r = 0; r < rows; r++)
                        {
                            int idx2 = 2 * r;
                            a[r][idx1] = temp[idx2];
                            a[r][idx1 + 1] = temp[idx2 + 1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D inverse DFT of complex data leaving the result in
        /// <code>a</code>d The data is stored in 1D array in row-major order.
        /// Complex number is stored as two float values in sequence: the real and
        /// imaginary part, i.ed the input array must be of size rows*2*columnsd The
        /// physical layout of the input data has to be as follows:<br>
        /// 
        /// <pre>
        /// a[k1*2*columns+2*k2] = Re[k1][k2],
        /// a[k1*2*columns+2*k2+1] = Im[k1][k2], 0&lt;=k1&lt;rows, 0&lt;=k2&lt;columns,
        /// </pre>
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// <param name="scale"></param>
        ///            if true then scaling is performed
        /// 
        public void ComplexInverse(float[] a, Boolean scale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                int oldn2 = columns;
                columns = 2 * columns;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(0, 1, a, scale);
                    cdft2d_subth(1, a, scale);
                }
                else
                {

                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.ComplexInverse(a, r * columns, scale);
                    }
                    cdft2d_sub(1, a, scale);
                }
                columns = oldn2;
            }
            else
            {
                int rowspan = 2 * columns;
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
                                fftColumns.ComplexInverse(a, r * rowspan, scale);
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
                            float[] temp = new float[2 * rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                int idx1 = 2 * c;
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx2 = 2 * r;
                                    int idx3 = r * rowspan + idx1;
                                    temp[idx2] = a[idx3];
                                    temp[idx2 + 1] = a[idx3 + 1];
                                }
                                fftRows.ComplexInverse(temp, scale);
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx2 = 2 * r;
                                    int idx3 = r * rowspan + idx1;
                                    a[idx3] = temp[idx2];
                                    a[idx3 + 1] = temp[idx2 + 1];
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
                        fftColumns.ComplexInverse(a, r * rowspan, scale);
                    }
                    float[] temp = new float[2 * rows];
                    for (int c = 0; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        for (int r = 0; r < rows; r++)
                        {
                            int idx2 = 2 * r;
                            int idx3 = r * rowspan + idx1;
                            temp[idx2] = a[idx3];
                            temp[idx2 + 1] = a[idx3 + 1];
                        }
                        fftRows.ComplexInverse(temp, scale);
                        for (int r = 0; r < rows; r++)
                        {
                            int idx2 = 2 * r;
                            int idx3 = r * rowspan + idx1;
                            a[idx3] = temp[idx2];
                            a[idx3 + 1] = temp[idx2 + 1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D inverse DFT of complex data leaving the result in
        /// <code>a</code>d The data is stored in 2D arrayd Complex data is
        /// represented by 2 float values in sequence: the real and imaginary part,
        /// i.ed the input array must be of size rows by 2*columnsd The physical
        /// layout of the input data has to be as follows:<br>
        /// 
        /// <pre>
        /// a[k1][2*k2] = Re[k1][k2],
        /// a[k1][2*k2+1] = Im[k1][k2], 0&lt;=k1&lt;rows, 0&lt;=k2&lt;columns,
        /// </pre>
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// <param name="scale"></param>
        ///            if true then scaling is performed
        /// 
        public void ComplexInverse(float[][] a, Boolean scale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                int oldn2 = columns;
                columns = 2 * columns;

                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(0, 1, a, scale);
                    cdft2d_subth(1, a, scale);
                }
                else
                {

                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.ComplexInverse(a[r], scale);
                    }
                    cdft2d_sub(1, a, scale);
                }
                columns = oldn2;
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
                                fftColumns.ComplexInverse(a[r], scale);
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
                            float[] temp = new float[2 * rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                int idx1 = 2 * c;
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx2 = 2 * r;
                                    temp[idx2] = a[r][idx1];
                                    temp[idx2 + 1] = a[r][idx1 + 1];
                                }
                                fftRows.ComplexInverse(temp, scale);
                                for (int r = 0; r < rows; r++)
                                {
                                    int idx2 = 2 * r;
                                    a[r][idx1] = temp[idx2];
                                    a[r][idx1 + 1] = temp[idx2 + 1];
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
                        fftColumns.ComplexInverse(a[r], scale);
                    }
                    float[] temp = new float[2 * rows];
                    for (int c = 0; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        for (int r = 0; r < rows; r++)
                        {
                            int idx2 = 2 * r;
                            temp[idx2] = a[r][idx1];
                            temp[idx2 + 1] = a[r][idx1 + 1];
                        }
                        fftRows.ComplexInverse(temp, scale);
                        for (int r = 0; r < rows; r++)
                        {
                            int idx2 = 2 * r;
                            a[r][idx1] = temp[idx2];
                            a[r][idx1 + 1] = temp[idx2 + 1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D forward DFT of real data leaving the result in <code>a</code>
        /// d This method only works when the sizes of both dimensions are
        /// power-of-two numbersd The physical layout of the output data is as
        /// follows:
        /// 
        /// <pre>
        /// a[k1*columns+2*k2] = Re[k1][k2] = Re[rows-k1][columns-k2],
        /// a[k1*columns+2*k2+1] = Im[k1][k2] = -Im[rows-k1][columns-k2],
        ///       0&lt;k1&lt;rows, 0&lt;k2&lt;columns/2,
        /// a[2*k2] = Re[0][k2] = Re[0][columns-k2],
        /// a[2*k2+1] = Im[0][k2] = -Im[0][columns-k2],
        ///       0&lt;k2&lt;columns/2,
        /// a[k1*columns] = Re[k1][0] = Re[rows-k1][0],
        /// a[k1*columns+1] = Im[k1][0] = -Im[rows-k1][0],
        /// a[(rows-k1)*columns+1] = Re[k1][columns/2] = Re[rows-k1][columns/2],
        /// a[(rows-k1)*columns] = -Im[k1][columns/2] = Im[rows-k1][columns/2],
        ///       0&lt;k1&lt;rows/2,
        /// a[0] = Re[0][0],
        /// a[1] = Re[0][columns/2],
        /// a[(rows/2)*columns] = Re[rows/2][0],
        /// a[(rows/2)*columns+1] = Re[rows/2][columns/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transformd The
        /// other half satisfies the symmetry conditiond If you want the full real
        /// forward transform, use <code>realForwardFull</code>d To get back the
        /// original data, use <code>realInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void RealForward(float[] a)
        {
            if (isPowerOfTwo == false)
            {
                throw new ArgumentException("rows and columns must be power of two numbers");
            }
            else
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(1, 1, a, true);
                    cdft2d_subth(-1, a, true);
                    rdft2d_sub(1, a);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealForward(a, r * columns);
                    }
                    cdft2d_sub(-1, a, true);
                    rdft2d_sub(1, a);
                }
            }
        }

        /// <summary>
        /// Computes 2D forward DFT of real data leaving the result in <code>a</code>
        /// d This method only works when the sizes of both dimensions are
        /// power-of-two numbersd The physical layout of the output data is as
        /// follows:
        /// 
        /// <pre>
        /// a[k1][2*k2] = Re[k1][k2] = Re[rows-k1][columns-k2],
        /// a[k1][2*k2+1] = Im[k1][k2] = -Im[rows-k1][columns-k2],
        ///       0&lt;k1&lt;rows, 0&lt;k2&lt;columns/2,
        /// a[0][2*k2] = Re[0][k2] = Re[0][columns-k2],
        /// a[0][2*k2+1] = Im[0][k2] = -Im[0][columns-k2],
        ///       0&lt;k2&lt;columns/2,
        /// a[k1][0] = Re[k1][0] = Re[rows-k1][0],
        /// a[k1][1] = Im[k1][0] = -Im[rows-k1][0],
        /// a[rows-k1][1] = Re[k1][columns/2] = Re[rows-k1][columns/2],
        /// a[rows-k1][0] = -Im[k1][columns/2] = Im[rows-k1][columns/2],
        ///       0&lt;k1&lt;rows/2,
        /// a[0][0] = Re[0][0],
        /// a[0][1] = Re[0][columns/2],
        /// a[rows/2][0] = Re[rows/2][0],
        /// a[rows/2][1] = Re[rows/2][columns/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transformd The
        /// other half satisfies the symmetry conditiond If you want the full real
        /// forward transform, use <code>realForwardFull</code>d To get back the
        /// original data, use <code>realInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void RealForward(float[][] a)
        {
            if (isPowerOfTwo == false)
            {
                throw new ArgumentException("rows and columns must be power of two numbers");
            }
            else
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(1, 1, a, true);
                    cdft2d_subth(-1, a, true);
                    rdft2d_sub(1, a);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealForward(a[r]);
                    }
                    cdft2d_sub(-1, a, true);
                    rdft2d_sub(1, a);
                }
            }
        }

        /// <summary>
        /// Computes 2D forward DFT of real data leaving the result in <code>a</code>
        /// d This method computes full real forward transform, i.ed you will get the
        /// same result as from <code>complexForward</code> called with all imaginary
        /// part equal 0d Because the result is stored in <code>a</code>, the input
        /// array must be of size rows*2*columns, with only the first rows*columns
        /// elements filled with real datad To get back the original data, use
        /// <code>complexInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void RealForwardFull(float[] a)
        {
            if (isPowerOfTwo)
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(1, 1, a, true);
                    cdft2d_subth(-1, a, true);
                    rdft2d_sub(1, a);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealForward(a, r * columns);
                    }
                    cdft2d_sub(-1, a, true);
                    rdft2d_sub(1, a);
                }
                fillSymmetric(a);
            }
            else
            {
                mixedRadixRealForwardFull(a);
            }
        }

        /// <summary>
        /// Computes 2D forward DFT of real data leaving the result in <code>a</code>
        /// d This method computes full real forward transform, i.ed you will get the
        /// same result as from <code>complexForward</code> called with all imaginary
        /// part equal 0d Because the result is stored in <code>a</code>, the input
        /// array must be of size rows by 2*columns, with only the first rows by
        /// columns elements filled with real datad To get back the original data,
        /// use <code>complexInverse</code> on the output of this method.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void RealForwardFull(float[][] a)
        {
            if (isPowerOfTwo)
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth1(1, 1, a, true);
                    cdft2d_subth(-1, a, true);
                    rdft2d_sub(1, a);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealForward(a[r]);
                    }
                    cdft2d_sub(-1, a, true);
                    rdft2d_sub(1, a);
                }
                fillSymmetric(a);
            }
            else
            {
                mixedRadixRealForwardFull(a);
            }
        }

        /// <summary>
        /// Computes 2D inverse DFT of real data leaving the result in <code>a</code>
        /// d This method only works when the sizes of both dimensions are
        /// power-of-two numbersd The physical layout of the input data has to be as
        /// follows:
        /// 
        /// <pre>
        /// a[k1*columns+2*k2] = Re[k1][k2] = Re[rows-k1][columns-k2],
        /// a[k1*columns+2*k2+1] = Im[k1][k2] = -Im[rows-k1][columns-k2],
        ///       0&lt;k1&lt;rows, 0&lt;k2&lt;columns/2,
        /// a[2*k2] = Re[0][k2] = Re[0][columns-k2],
        /// a[2*k2+1] = Im[0][k2] = -Im[0][columns-k2],
        ///       0&lt;k2&lt;columns/2,
        /// a[k1*columns] = Re[k1][0] = Re[rows-k1][0],
        /// a[k1*columns+1] = Im[k1][0] = -Im[rows-k1][0],
        /// a[(rows-k1)*columns+1] = Re[k1][columns/2] = Re[rows-k1][columns/2],
        /// a[(rows-k1)*columns] = -Im[k1][columns/2] = Im[rows-k1][columns/2],
        ///       0&lt;k1&lt;rows/2,
        /// a[0] = Re[0][0],
        /// a[1] = Re[0][columns/2],
        /// a[(rows/2)*columns] = Re[rows/2][0],
        /// a[(rows/2)*columns+1] = Re[rows/2][columns/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transformd The
        /// other half satisfies the symmetry conditiond If you want the full real
        /// inverse transform, use <code>realInverseFull</code>.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// 
        /// <param name="scale"></param>
        ///            if true then scaling is performed
        public void RealInverse(float[] a, Boolean scale)
        {
            if (isPowerOfTwo == false)
            {
                throw new ArgumentException("rows and columns must be power of two numbers");
            }
            else
            {
                int nthreads;
                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    rdft2d_sub(-1, a);
                    cdft2d_subth(1, a, scale);
                    xdft2d0_subth1(1, -1, a, scale);
                }
                else
                {
                    rdft2d_sub(-1, a);
                    cdft2d_sub(1, a, scale);
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealInverse(a, r * columns, scale);
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D inverse DFT of real data leaving the result in <code>a</code>
        /// d This method only works when the sizes of both dimensions are
        /// power-of-two numbersd The physical layout of the input data has to be as
        /// follows:
        /// 
        /// <pre>
        /// a[k1][2*k2] = Re[k1][k2] = Re[rows-k1][columns-k2],
        /// a[k1][2*k2+1] = Im[k1][k2] = -Im[rows-k1][columns-k2],
        ///       0&lt;k1&lt;rows, 0&lt;k2&lt;columns/2,
        /// a[0][2*k2] = Re[0][k2] = Re[0][columns-k2],
        /// a[0][2*k2+1] = Im[0][k2] = -Im[0][columns-k2],
        ///       0&lt;k2&lt;columns/2,
        /// a[k1][0] = Re[k1][0] = Re[rows-k1][0],
        /// a[k1][1] = Im[k1][0] = -Im[rows-k1][0],
        /// a[rows-k1][1] = Re[k1][columns/2] = Re[rows-k1][columns/2],
        /// a[rows-k1][0] = -Im[k1][columns/2] = Im[rows-k1][columns/2],
        ///       0&lt;k1&lt;rows/2,
        /// a[0][0] = Re[0][0],
        /// a[0][1] = Re[0][columns/2],
        /// a[rows/2][0] = Re[rows/2][0],
        /// a[rows/2][1] = Re[rows/2][columns/2]
        /// </pre>
        /// 
        /// This method computes only half of the elements of the real transformd The
        /// other half satisfies the symmetry conditiond If you want the full real
        /// inverse transform, use <code>realInverseFull</code>.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// 
        /// <param name="scale"></param>
        ///            if true then scaling is performed
        public void RealInverse(float[][] a, Boolean scale)
        {
            if (isPowerOfTwo == false)
            {
                throw new ArgumentException("rows and columns must be power of two numbers");
            }
            else
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    rdft2d_sub(-1, a);
                    cdft2d_subth(1, a, scale);
                    xdft2d0_subth1(1, -1, a, scale);
                }
                else
                {
                    rdft2d_sub(-1, a);
                    cdft2d_sub(1, a, scale);
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealInverse(a[r], scale);
                    }
                }
            }
        }

        /// <summary>
        /// Computes 2D inverse DFT of real data leaving the result in <code>a</code>
        /// d This method computes full real inverse transform, i.ed you will get the
        /// same result as from <code>complexInverse</code> called with all imaginary
        /// part equal 0d Because the result is stored in <code>a</code>, the input
        /// array must be of size rows*2*columns, with only the first rows*columns
        /// elements filled with real data.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// 
        /// <param name="scale"></param>
        ///            if true then scaling is performed
        public void RealInverseFull(float[] a, Boolean scale)
        {
            if (isPowerOfTwo)
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth2(1, -1, a, scale);
                    cdft2d_subth(1, a, scale);
                    rdft2d_sub(1, a);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealInverse2(a, r * columns, scale);
                    }
                    cdft2d_sub(1, a, scale);
                    rdft2d_sub(1, a);
                }
                fillSymmetric(a);
            }
            else
            {
                mixedRadixRealInverseFull(a, scale);
            }
        }

        /// <summary>
        /// Computes 2D inverse DFT of real data leaving the result in <code>a</code>
        /// d This method computes full real inverse transform, i.ed you will get the
        /// same result as from <code>complexInverse</code> called with all imaginary
        /// part equal 0d Because the result is stored in <code>a</code>, the input
        /// array must be of size rows by 2*columns, with only the first rows by
        /// columns elements filled with real data.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// 
        /// <param name="scale"></param>
        ///            if true then scaling is performed
        public void RealInverseFull(float[][] a, Boolean scale)
        {
            if (isPowerOfTwo)
            {
                int nthreads;

                nthreads = Process.GetCurrentProcess().Threads.Count;
                if (nthreads != oldNthreads)
                {
                    nt = 8 * nthreads * rows;
                    if (columns == 4 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 4 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new float[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    xdft2d0_subth2(1, -1, a, scale);
                    cdft2d_subth(1, a, scale);
                    rdft2d_sub(1, a);
                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        fftColumns.RealInverse2(a[r], 0, scale);
                    }
                    cdft2d_sub(1, a, scale);
                    rdft2d_sub(1, a);
                }
                fillSymmetric(a);
            }
            else
            {
                mixedRadixRealInverseFull(a, scale);
            }
        }

        private void mixedRadixRealForwardFull(float[][] a)
        {
            int n2d2 = columns / 2 + 1;
            float[][] temp = ArrayUtility.CreateJaggedArray<float>(n2d2, 2 * rows);

            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && useThreads && (rows >= nthreads) && (n2d2 - 2 >= nthreads))
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
                            fftColumns.RealForward(a[i]);
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


                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r][0]; //first column is always real
                }
                fftRows.RealForwardFull(temp[0]);

                p = (n2d2 - 2) / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstColumn = 1 + l * p;
                    int lastColumn = (l == (nthreads - 1)) ? n2d2 - 1 : firstColumn + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int c = firstColumn; c < lastColumn; c++)
                        {
                            int idx2 = 2 * c;
                            for (int r = 0; r < rows; r++)
                            {
                                int idx1 = 2 * r;
                                temp[c][idx1] = a[r][idx2];
                                temp[c][idx1 + 1] = a[r][idx2 + 1];
                            }
                            fftRows.ComplexForward(temp[c]);
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


                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r][1];
                        //imaginary part = 0;
                    }
                    fftRows.RealForwardFull(temp[n2d2 - 1]);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = n2d2 - 1;
                        temp[idx2][idx1] = a[r][2 * idx2];
                        temp[idx2][idx1 + 1] = a[r][1];
                    }
                    fftRows.ComplexForward(temp[n2d2 - 1]);

                }

                p = rows / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx1 = 2 * r;
                            for (int c = 0; c < n2d2; c++)
                            {
                                int idx2 = 2 * c;
                                a[r][idx2] = temp[c][idx1];
                                a[r][idx2 + 1] = temp[c][idx1 + 1];
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


                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = 1 + l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx3 = rows - r;
                            for (int c = n2d2; c < columns; c++)
                            {
                                int idx1 = 2 * c;
                                int idx2 = 2 * (columns - c);
                                a[0][idx1] = a[0][idx2];
                                a[0][idx1 + 1] = -a[0][idx2 + 1];
                                a[r][idx1] = a[idx3][idx2];
                                a[r][idx1 + 1] = -a[idx3][idx2 + 1];
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
                    fftColumns.RealForward(a[r]);
                }

                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r][0]; //first column is always real
                }
                fftRows.RealForwardFull(temp[0]);

                for (int c = 1; c < n2d2 - 1; c++)
                {
                    int idx2 = 2 * c;
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        temp[c][idx1] = a[r][idx2];
                        temp[c][idx1 + 1] = a[r][idx2 + 1];
                    }
                    fftRows.ComplexForward(temp[c]);
                }

                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r][1];
                        //imaginary part = 0;
                    }
                    fftRows.RealForwardFull(temp[n2d2 - 1]);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = n2d2 - 1;
                        temp[idx2][idx1] = a[r][2 * idx2];
                        temp[idx2][idx1 + 1] = a[r][1];
                    }
                    fftRows.ComplexForward(temp[n2d2 - 1]);

                }

                for (int r = 0; r < rows; r++)
                {
                    int idx1 = 2 * r;
                    for (int c = 0; c < n2d2; c++)
                    {
                        int idx2 = 2 * c;
                        a[r][idx2] = temp[c][idx1];
                        a[r][idx2 + 1] = temp[c][idx1 + 1];
                    }
                }

                //fill symmetric
                for (int r = 1; r < rows; r++)
                {
                    int idx3 = rows - r;
                    for (int c = n2d2; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        int idx2 = 2 * (columns - c);
                        a[0][idx1] = a[0][idx2];
                        a[0][idx1 + 1] = -a[0][idx2 + 1];
                        a[r][idx1] = a[idx3][idx2];
                        a[r][idx1 + 1] = -a[idx3][idx2 + 1];
                    }
                }
            }
        }

        private void mixedRadixRealForwardFull(float[] a)
        {
            int rowStride = 2 * columns;
            int n2d2 = columns / 2 + 1;
            float[][] temp = ArrayUtility.CreateJaggedArray<float>(n2d2, 2 * rows);

            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && useThreads && (rows >= nthreads) && (n2d2 - 2 >= nthreads))
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
                            fftColumns.RealForward(a, i * columns);
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


                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r * columns]; //first column is always real
                }
                fftRows.RealForwardFull(temp[0]);

                p = (n2d2 - 2) / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstColumn = 1 + l * p;
                    int lastColumn = (l == (nthreads - 1)) ? n2d2 - 1 : firstColumn + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int c = firstColumn; c < lastColumn; c++)
                        {
                            int idx0 = 2 * c;
                            for (int r = 0; r < rows; r++)
                            {
                                int idx1 = 2 * r;
                                int idx2 = r * columns + idx0;
                                temp[c][idx1] = a[idx2];
                                temp[c][idx1 + 1] = a[idx2 + 1];
                            }
                            fftRows.ComplexForward(temp[c]);
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


                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r * columns + 1];
                        //imaginary part = 0;
                    }
                    fftRows.RealForwardFull(temp[n2d2 - 1]);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = r * columns;
                        int idx3 = n2d2 - 1;
                        temp[idx3][idx1] = a[idx2 + 2 * idx3];
                        temp[idx3][idx1 + 1] = a[idx2 + 1];
                    }
                    fftRows.ComplexForward(temp[n2d2 - 1]);
                }

                p = rows / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx1 = 2 * r;
                            for (int c = 0; c < n2d2; c++)
                            {
                                int idx0 = 2 * c;
                                int idx2 = r * rowStride + idx0;
                                a[idx2] = temp[c][idx1];
                                a[idx2 + 1] = temp[c][idx1 + 1];
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


                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = 1 + l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx5 = r * rowStride;
                            int idx6 = (rows - r + 1) * rowStride;
                            for (int c = n2d2; c < columns; c++)
                            {
                                int idx1 = 2 * c;
                                int idx2 = 2 * (columns - c);
                                a[idx1] = a[idx2];
                                a[idx1 + 1] = -a[idx2 + 1];
                                int idx3 = idx5 + idx1;
                                int idx4 = idx6 - idx1;
                                a[idx3] = a[idx4];
                                a[idx3 + 1] = -a[idx4 + 1];
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
                    fftColumns.RealForward(a, r * columns);
                }
                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r * columns]; //first column is always real
                }
                fftRows.RealForwardFull(temp[0]);

                for (int c = 1; c < n2d2 - 1; c++)
                {
                    int idx0 = 2 * c;
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = r * columns + idx0;
                        temp[c][idx1] = a[idx2];
                        temp[c][idx1 + 1] = a[idx2 + 1];
                    }
                    fftRows.ComplexForward(temp[c]);
                }

                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r * columns + 1];
                        //imaginary part = 0;
                    }
                    fftRows.RealForwardFull(temp[n2d2 - 1]);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = r * columns;
                        int idx3 = n2d2 - 1;
                        temp[idx3][idx1] = a[idx2 + 2 * idx3];
                        temp[idx3][idx1 + 1] = a[idx2 + 1];
                    }
                    fftRows.ComplexForward(temp[n2d2 - 1]);
                }

                for (int r = 0; r < rows; r++)
                {
                    int idx1 = 2 * r;
                    for (int c = 0; c < n2d2; c++)
                    {
                        int idx0 = 2 * c;
                        int idx2 = r * rowStride + idx0;
                        a[idx2] = temp[c][idx1];
                        a[idx2 + 1] = temp[c][idx1 + 1];
                    }
                }

                //fill symmetric
                for (int r = 1; r < rows; r++)
                {
                    int idx5 = r * rowStride;
                    int idx6 = (rows - r + 1) * rowStride;
                    for (int c = n2d2; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        int idx2 = 2 * (columns - c);
                        a[idx1] = a[idx2];
                        a[idx1 + 1] = -a[idx2 + 1];
                        int idx3 = idx5 + idx1;
                        int idx4 = idx6 - idx1;
                        a[idx3] = a[idx4];
                        a[idx3 + 1] = -a[idx4 + 1];
                    }
                }
            }
        }

        private void mixedRadixRealInverseFull(float[][] a, Boolean scale)
        {
            int n2d2 = columns / 2 + 1;
            float[][] temp = ArrayUtility.CreateJaggedArray<float>(n2d2, 2 * rows);

            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && useThreads && (rows >= nthreads) && (n2d2 - 2 >= nthreads))
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
                            fftColumns.RealInverse2(a[i], 0, scale);
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


                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r][0]; //first column is always real
                }
                fftRows.RealInverseFull(temp[0], scale);

                p = (n2d2 - 2) / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstColumn = 1 + l * p;
                    int lastColumn = (l == (nthreads - 1)) ? n2d2 - 1 : firstColumn + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int c = firstColumn; c < lastColumn; c++)
                        {
                            int idx2 = 2 * c;
                            for (int r = 0; r < rows; r++)
                            {
                                int idx1 = 2 * r;
                                temp[c][idx1] = a[r][idx2];
                                temp[c][idx1 + 1] = a[r][idx2 + 1];
                            }
                            fftRows.ComplexInverse(temp[c], scale);
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


                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r][1];
                        //imaginary part = 0;
                    }
                    fftRows.RealInverseFull(temp[n2d2 - 1], scale);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = n2d2 - 1;
                        temp[idx2][idx1] = a[r][2 * idx2];
                        temp[idx2][idx1 + 1] = a[r][1];
                    }
                    fftRows.ComplexInverse(temp[n2d2 - 1], scale);

                }

                p = rows / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx1 = 2 * r;
                            for (int c = 0; c < n2d2; c++)
                            {
                                int idx2 = 2 * c;
                                a[r][idx2] = temp[c][idx1];
                                a[r][idx2 + 1] = temp[c][idx1 + 1];
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


                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = 1 + l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx3 = rows - r;
                            for (int c = n2d2; c < columns; c++)
                            {
                                int idx1 = 2 * c;
                                int idx2 = 2 * (columns - c);
                                a[0][idx1] = a[0][idx2];
                                a[0][idx1 + 1] = -a[0][idx2 + 1];
                                a[r][idx1] = a[idx3][idx2];
                                a[r][idx1 + 1] = -a[idx3][idx2 + 1];
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
                    fftColumns.RealInverse2(a[r], 0, scale);
                }

                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r][0]; //first column is always real
                }
                fftRows.RealInverseFull(temp[0], scale);

                for (int c = 1; c < n2d2 - 1; c++)
                {
                    int idx2 = 2 * c;
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        temp[c][idx1] = a[r][idx2];
                        temp[c][idx1 + 1] = a[r][idx2 + 1];
                    }
                    fftRows.ComplexInverse(temp[c], scale);
                }

                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r][1];
                        //imaginary part = 0;
                    }
                    fftRows.RealInverseFull(temp[n2d2 - 1], scale);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = n2d2 - 1;
                        temp[idx2][idx1] = a[r][2 * idx2];
                        temp[idx2][idx1 + 1] = a[r][1];
                    }
                    fftRows.ComplexInverse(temp[n2d2 - 1], scale);

                }

                for (int r = 0; r < rows; r++)
                {
                    int idx1 = 2 * r;
                    for (int c = 0; c < n2d2; c++)
                    {
                        int idx2 = 2 * c;
                        a[r][idx2] = temp[c][idx1];
                        a[r][idx2 + 1] = temp[c][idx1 + 1];
                    }
                }

                //fill symmetric
                for (int r = 1; r < rows; r++)
                {
                    int idx3 = rows - r;
                    for (int c = n2d2; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        int idx2 = 2 * (columns - c);
                        a[0][idx1] = a[0][idx2];
                        a[0][idx1 + 1] = -a[0][idx2 + 1];
                        a[r][idx1] = a[idx3][idx2];
                        a[r][idx1 + 1] = -a[idx3][idx2 + 1];
                    }
                }
            }
        }

        private void mixedRadixRealInverseFull(float[] a, Boolean scale)
        {
            int rowStride = 2 * columns;
            int n2d2 = columns / 2 + 1;
            float[][] temp = ArrayUtility.CreateJaggedArray<float>(n2d2, 2 * rows);

            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && useThreads && (rows >= nthreads) && (n2d2 - 2 >= nthreads))
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
                            fftColumns.RealInverse2(a, i * columns, scale);
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

                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r * columns]; //first column is always real
                }
                fftRows.RealInverseFull(temp[0], scale);

                p = (n2d2 - 2) / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstColumn = 1 + l * p;
                    int lastColumn = (l == (nthreads - 1)) ? n2d2 - 1 : firstColumn + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int c = firstColumn; c < lastColumn; c++)
                        {
                            int idx0 = 2 * c;
                            for (int r = 0; r < rows; r++)
                            {
                                int idx1 = 2 * r;
                                int idx2 = r * columns + idx0;
                                temp[c][idx1] = a[idx2];
                                temp[c][idx1 + 1] = a[idx2 + 1];
                            }
                            fftRows.ComplexInverse(temp[c], scale);
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


                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r * columns + 1];
                        //imaginary part = 0;
                    }
                    fftRows.RealInverseFull(temp[n2d2 - 1], scale);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = r * columns;
                        int idx3 = n2d2 - 1;
                        temp[idx3][idx1] = a[idx2 + 2 * idx3];
                        temp[idx3][idx1 + 1] = a[idx2 + 1];
                    }
                    fftRows.ComplexInverse(temp[n2d2 - 1], scale);
                }

                p = rows / nthreads;
                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx1 = 2 * r;
                            for (int c = 0; c < n2d2; c++)
                            {
                                int idx0 = 2 * c;
                                int idx2 = r * rowStride + idx0;
                                a[idx2] = temp[c][idx1];
                                a[idx2 + 1] = temp[c][idx1 + 1];
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


                for (int l = 0; l < nthreads; l++)
                {
                    int firstRow = 1 + l * p;
                    int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                    taskArray[l] = Task.Factory.StartNew(() =>
                    {
                        for (int r = firstRow; r < lastRow; r++)
                        {
                            int idx5 = r * rowStride;
                            int idx6 = (rows - r + 1) * rowStride;
                            for (int c = n2d2; c < columns; c++)
                            {
                                int idx1 = 2 * c;
                                int idx2 = 2 * (columns - c);
                                a[idx1] = a[idx2];
                                a[idx1 + 1] = -a[idx2 + 1];
                                int idx3 = idx5 + idx1;
                                int idx4 = idx6 - idx1;
                                a[idx3] = a[idx4];
                                a[idx3 + 1] = -a[idx4 + 1];
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
                    fftColumns.RealInverse2(a, r * columns, scale);
                }
                for (int r = 0; r < rows; r++)
                {
                    temp[0][r] = a[r * columns]; //first column is always real
                }
                fftRows.RealInverseFull(temp[0], scale);

                for (int c = 1; c < n2d2 - 1; c++)
                {
                    int idx0 = 2 * c;
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = r * columns + idx0;
                        temp[c][idx1] = a[idx2];
                        temp[c][idx1 + 1] = a[idx2 + 1];
                    }
                    fftRows.ComplexInverse(temp[c], scale);
                }

                if ((columns % 2) == 0)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        temp[n2d2 - 1][r] = a[r * columns + 1];
                        //imaginary part = 0;
                    }
                    fftRows.RealInverseFull(temp[n2d2 - 1], scale);

                }
                else
                {
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = 2 * r;
                        int idx2 = r * columns;
                        int idx3 = n2d2 - 1;
                        temp[idx3][idx1] = a[idx2 + 2 * idx3];
                        temp[idx3][idx1 + 1] = a[idx2 + 1];
                    }
                    fftRows.ComplexInverse(temp[n2d2 - 1], scale);
                }

                for (int r = 0; r < rows; r++)
                {
                    int idx1 = 2 * r;
                    for (int c = 0; c < n2d2; c++)
                    {
                        int idx0 = 2 * c;
                        int idx2 = r * rowStride + idx0;
                        a[idx2] = temp[c][idx1];
                        a[idx2 + 1] = temp[c][idx1 + 1];
                    }
                }

                //fill symmetric
                for (int r = 1; r < rows; r++)
                {
                    int idx5 = r * rowStride;
                    int idx6 = (rows - r + 1) * rowStride;
                    for (int c = n2d2; c < columns; c++)
                    {
                        int idx1 = 2 * c;
                        int idx2 = 2 * (columns - c);
                        a[idx1] = a[idx2];
                        a[idx1 + 1] = -a[idx2 + 1];
                        int idx3 = idx5 + idx1;
                        int idx4 = idx6 - idx1;
                        a[idx3] = a[idx4];
                        a[idx3 + 1] = -a[idx4 + 1];
                    }
                }
            }
        }

        private void rdft2d_sub(int isgn, float[] a)
        {
            int n1h, j;
            float xi;
            int idx1, idx2;

            n1h = rows >> 1;
            if (isgn < 0)
            {
                for (int i = 1; i < n1h; i++)
                {
                    j = rows - i;
                    idx1 = i * columns;
                    idx2 = j * columns;
                    xi = a[idx1] - a[idx2];
                    a[idx1] += a[idx2];
                    a[idx2] = xi;
                    xi = a[idx2 + 1] - a[idx1 + 1];
                    a[idx1 + 1] += a[idx2 + 1];
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int i = 1; i < n1h; i++)
                {
                    j = rows - i;
                    idx1 = i * columns;
                    idx2 = j * columns;
                    a[idx2] = 0.5f * (a[idx1] - a[idx2]);
                    a[idx1] -= a[idx2];
                    a[idx2 + 1] = 0.5f * (a[idx1 + 1] + a[idx2 + 1]);
                    a[idx1 + 1] -= a[idx2 + 1];
                }
            }
        }

        private void rdft2d_sub(int isgn, float[][] a)
        {
            int n1h, j;
            float xi;

            n1h = rows >> 1;
            if (isgn < 0)
            {
                for (int i = 1; i < n1h; i++)
                {
                    j = rows - i;
                    xi = a[i][0] - a[j][0];
                    a[i][0] += a[j][0];
                    a[j][0] = xi;
                    xi = a[j][1] - a[i][1];
                    a[i][1] += a[j][1];
                    a[j][1] = xi;
                }
            }
            else
            {
                for (int i = 1; i < n1h; i++)
                {
                    j = rows - i;
                    a[j][0] = 0.5f * (a[i][0] - a[j][0]);
                    a[i][0] -= a[j][0];
                    a[j][1] = 0.5f * (a[i][1] + a[j][1]);
                    a[i][1] -= a[j][1];
                }
            }
        }

        private void cdft2d_sub(int isgn, float[] a, Boolean scale)
        {
            int idx1, idx2, idx3, idx4, idx5;
            if (isgn == -1)
            {
                if (columns > 4)
                {
                    for (int c = 0; c < columns; c += 8)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            t[idx2] = a[idx1];
                            t[idx2 + 1] = a[idx1 + 1];
                            t[idx3] = a[idx1 + 2];
                            t[idx3 + 1] = a[idx1 + 3];
                            t[idx4] = a[idx1 + 4];
                            t[idx4 + 1] = a[idx1 + 5];
                            t[idx5] = a[idx1 + 6];
                            t[idx5 + 1] = a[idx1 + 7];
                        }
                        fftRows.ComplexForward(t, 0);
                        fftRows.ComplexForward(t, 2 * rows);
                        fftRows.ComplexForward(t, 4 * rows);
                        fftRows.ComplexForward(t, 6 * rows);
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            a[idx1] = t[idx2];
                            a[idx1 + 1] = t[idx2 + 1];
                            a[idx1 + 2] = t[idx3];
                            a[idx1 + 3] = t[idx3 + 1];
                            a[idx1 + 4] = t[idx4];
                            a[idx1 + 5] = t[idx4 + 1];
                            a[idx1 + 6] = t[idx5];
                            a[idx1 + 7] = t[idx5 + 1];
                        }
                    }
                }
                else if (columns == 4)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        t[idx2] = a[idx1];
                        t[idx2 + 1] = a[idx1 + 1];
                        t[idx3] = a[idx1 + 2];
                        t[idx3 + 1] = a[idx1 + 3];
                    }
                    fftRows.ComplexForward(t, 0);
                    fftRows.ComplexForward(t, 2 * rows);
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        a[idx1] = t[idx2];
                        a[idx1 + 1] = t[idx2 + 1];
                        a[idx1 + 2] = t[idx3];
                        a[idx1 + 3] = t[idx3 + 1];
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        t[idx2] = a[idx1];
                        t[idx2 + 1] = a[idx1 + 1];
                    }
                    fftRows.ComplexForward(t, 0);
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        a[idx1] = t[idx2];
                        a[idx1 + 1] = t[idx2 + 1];
                    }
                }
            }
            else
            {
                if (columns > 4)
                {
                    for (int c = 0; c < columns; c += 8)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            t[idx2] = a[idx1];
                            t[idx2 + 1] = a[idx1 + 1];
                            t[idx3] = a[idx1 + 2];
                            t[idx3 + 1] = a[idx1 + 3];
                            t[idx4] = a[idx1 + 4];
                            t[idx4 + 1] = a[idx1 + 5];
                            t[idx5] = a[idx1 + 6];
                            t[idx5 + 1] = a[idx1 + 7];
                        }
                        fftRows.ComplexInverse(t, 0, scale);
                        fftRows.ComplexInverse(t, 2 * rows, scale);
                        fftRows.ComplexInverse(t, 4 * rows, scale);
                        fftRows.ComplexInverse(t, 6 * rows, scale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = r * columns + c;
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            a[idx1] = t[idx2];
                            a[idx1 + 1] = t[idx2 + 1];
                            a[idx1 + 2] = t[idx3];
                            a[idx1 + 3] = t[idx3 + 1];
                            a[idx1 + 4] = t[idx4];
                            a[idx1 + 5] = t[idx4 + 1];
                            a[idx1 + 6] = t[idx5];
                            a[idx1 + 7] = t[idx5 + 1];
                        }
                    }
                }
                else if (columns == 4)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        t[idx2] = a[idx1];
                        t[idx2 + 1] = a[idx1 + 1];
                        t[idx3] = a[idx1 + 2];
                        t[idx3 + 1] = a[idx1 + 3];
                    }
                    fftRows.ComplexInverse(t, 0, scale);
                    fftRows.ComplexInverse(t, 2 * rows, scale);
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        a[idx1] = t[idx2];
                        a[idx1 + 1] = t[idx2 + 1];
                        a[idx1 + 2] = t[idx3];
                        a[idx1 + 3] = t[idx3 + 1];
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        t[idx2] = a[idx1];
                        t[idx2 + 1] = a[idx1 + 1];
                    }
                    fftRows.ComplexInverse(t, 0, scale);
                    for (int r = 0; r < rows; r++)
                    {
                        idx1 = r * columns;
                        idx2 = 2 * r;
                        a[idx1] = t[idx2];
                        a[idx1 + 1] = t[idx2 + 1];
                    }
                }
            }
        }

        private void cdft2d_sub(int isgn, float[][] a, Boolean scale)
        {
            int idx2, idx3, idx4, idx5;
            if (isgn == -1)
            {
                if (columns > 4)
                {
                    for (int c = 0; c < columns; c += 8)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            t[idx2] = a[r][c];
                            t[idx2 + 1] = a[r][c + 1];
                            t[idx3] = a[r][c + 2];
                            t[idx3 + 1] = a[r][c + 3];
                            t[idx4] = a[r][c + 4];
                            t[idx4 + 1] = a[r][c + 5];
                            t[idx5] = a[r][c + 6];
                            t[idx5 + 1] = a[r][c + 7];
                        }
                        fftRows.ComplexForward(t, 0);
                        fftRows.ComplexForward(t, 2 * rows);
                        fftRows.ComplexForward(t, 4 * rows);
                        fftRows.ComplexForward(t, 6 * rows);
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            a[r][c] = t[idx2];
                            a[r][c + 1] = t[idx2 + 1];
                            a[r][c + 2] = t[idx3];
                            a[r][c + 3] = t[idx3 + 1];
                            a[r][c + 4] = t[idx4];
                            a[r][c + 5] = t[idx4 + 1];
                            a[r][c + 6] = t[idx5];
                            a[r][c + 7] = t[idx5 + 1];
                        }
                    }
                }
                else if (columns == 4)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        t[idx2] = a[r][0];
                        t[idx2 + 1] = a[r][1];
                        t[idx3] = a[r][2];
                        t[idx3 + 1] = a[r][3];
                    }
                    fftRows.ComplexForward(t, 0);
                    fftRows.ComplexForward(t, 2 * rows);
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        a[r][0] = t[idx2];
                        a[r][1] = t[idx2 + 1];
                        a[r][2] = t[idx3];
                        a[r][3] = t[idx3 + 1];
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        t[idx2] = a[r][0];
                        t[idx2 + 1] = a[r][1];
                    }
                    fftRows.ComplexForward(t, 0);
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        a[r][0] = t[idx2];
                        a[r][1] = t[idx2 + 1];
                    }
                }
            }
            else
            {
                if (columns > 4)
                {
                    for (int c = 0; c < columns; c += 8)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            t[idx2] = a[r][c];
                            t[idx2 + 1] = a[r][c + 1];
                            t[idx3] = a[r][c + 2];
                            t[idx3 + 1] = a[r][c + 3];
                            t[idx4] = a[r][c + 4];
                            t[idx4 + 1] = a[r][c + 5];
                            t[idx5] = a[r][c + 6];
                            t[idx5 + 1] = a[r][c + 7];
                        }
                        fftRows.ComplexInverse(t, 0, scale);
                        fftRows.ComplexInverse(t, 2 * rows, scale);
                        fftRows.ComplexInverse(t, 4 * rows, scale);
                        fftRows.ComplexInverse(t, 6 * rows, scale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx2 = 2 * r;
                            idx3 = 2 * rows + 2 * r;
                            idx4 = idx3 + 2 * rows;
                            idx5 = idx4 + 2 * rows;
                            a[r][c] = t[idx2];
                            a[r][c + 1] = t[idx2 + 1];
                            a[r][c + 2] = t[idx3];
                            a[r][c + 3] = t[idx3 + 1];
                            a[r][c + 4] = t[idx4];
                            a[r][c + 5] = t[idx4 + 1];
                            a[r][c + 6] = t[idx5];
                            a[r][c + 7] = t[idx5 + 1];
                        }
                    }
                }
                else if (columns == 4)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        t[idx2] = a[r][0];
                        t[idx2 + 1] = a[r][1];
                        t[idx3] = a[r][2];
                        t[idx3 + 1] = a[r][3];
                    }
                    fftRows.ComplexInverse(t, 0, scale);
                    fftRows.ComplexInverse(t, 2 * rows, scale);
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        idx3 = 2 * rows + 2 * r;
                        a[r][0] = t[idx2];
                        a[r][1] = t[idx2 + 1];
                        a[r][2] = t[idx3];
                        a[r][3] = t[idx3 + 1];
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        t[idx2] = a[r][0];
                        t[idx2 + 1] = a[r][1];
                    }
                    fftRows.ComplexInverse(t, 0, scale);
                    for (int r = 0; r < rows; r++)
                    {
                        idx2 = 2 * r;
                        a[r][0] = t[idx2];
                        a[r][1] = t[idx2 + 1];
                    }
                }
            }
        }

        private void xdft2d0_subth1(int icr, int isgn, float[] a, Boolean scale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;

            Task[] taskArray = new Task[nthreads];
            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    if (icr == 0)
                    {
                        if (isgn == -1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexForward(a, r * columns);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexInverse(a, r * columns, scale);
                            }
                        }
                    }
                    else
                    {
                        if (isgn == 1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealForward(a, r * columns);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealInverse(a, r * columns, scale);
                            }
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

        private void xdft2d0_subth2(int icr, int isgn, float[] a, Boolean scale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;

            Task[] taskArray = new Task[nthreads];
            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    if (icr == 0)
                    {
                        if (isgn == -1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexForward(a, r * columns);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexInverse(a, r * columns, scale);
                            }
                        }
                    }
                    else
                    {
                        if (isgn == 1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealForward(a, r * columns);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealInverse2(a, r * columns, scale);
                            }
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

        private void xdft2d0_subth1(int icr, int isgn, float[][] a, Boolean scale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;

            Task[] taskArray = new Task[nthreads];
            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    if (icr == 0)
                    {
                        if (isgn == -1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexForward(a[r]);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexInverse(a[r], scale);
                            }
                        }
                    }
                    else
                    {
                        if (isgn == 1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealForward(a[r]);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealInverse(a[r], scale);
                            }
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

        private void xdft2d0_subth2(int icr, int isgn, float[][] a, Boolean scale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;

            Task[] taskArray = new Task[nthreads];
            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    if (icr == 0)
                    {
                        if (isgn == -1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexForward(a[r]);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.ComplexInverse(a[r], scale);
                            }
                        }
                    }
                    else
                    {
                        if (isgn == 1)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealForward(a[r]);
                            }
                        }
                        else
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                fftColumns.RealInverse2(a[r], 0, scale);
                            }
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

        private void cdft2d_subth(int isgn, float[] a, Boolean scale)
        {
            int nthread = Process.GetCurrentProcess().Threads.Count;
            int nt = 8 * rows;
            if (columns == 4 * nthread)
            {
                nt >>= 1;
            }
            else if (columns < 4 * nthread)
            {
                nthread = columns >> 1;
                nt >>= 2;
            }
            Task[] taskArray = new Task[nthread];
            int nthreads = nthread;
            for (int i = 0; i < nthread; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx1, idx2, idx3, idx4, idx5;
                    if (isgn == -1)
                    {
                        if (columns > 4 * nthreads)
                        {
                            for (int c = 8 * n0; c < columns; c += 8 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    t[idx2] = a[idx1];
                                    t[idx2 + 1] = a[idx1 + 1];
                                    t[idx3] = a[idx1 + 2];
                                    t[idx3 + 1] = a[idx1 + 3];
                                    t[idx4] = a[idx1 + 4];
                                    t[idx4 + 1] = a[idx1 + 5];
                                    t[idx5] = a[idx1 + 6];
                                    t[idx5 + 1] = a[idx1 + 7];
                                }
                                fftRows.ComplexForward(t, startt);
                                fftRows.ComplexForward(t, startt + 2 * rows);
                                fftRows.ComplexForward(t, startt + 4 * rows);
                                fftRows.ComplexForward(t, startt + 6 * rows);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    a[idx1] = t[idx2];
                                    a[idx1 + 1] = t[idx2 + 1];
                                    a[idx1 + 2] = t[idx3];
                                    a[idx1 + 3] = t[idx3 + 1];
                                    a[idx1 + 4] = t[idx4];
                                    a[idx1 + 5] = t[idx4 + 1];
                                    a[idx1 + 6] = t[idx5];
                                    a[idx1 + 7] = t[idx5 + 1];
                                }
                            }
                        }
                        else if (columns == 4 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 4 * n0;
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                t[idx2] = a[idx1];
                                t[idx2 + 1] = a[idx1 + 1];
                                t[idx3] = a[idx1 + 2];
                                t[idx3 + 1] = a[idx1 + 3];
                            }
                            fftRows.ComplexForward(t, startt);
                            fftRows.ComplexForward(t, startt + 2 * rows);
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 4 * n0;
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                a[idx1] = t[idx2];
                                a[idx1 + 1] = t[idx2 + 1];
                                a[idx1 + 2] = t[idx3];
                                a[idx1 + 3] = t[idx3 + 1];
                            }
                        }
                        else if (columns == 2 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 2 * n0;
                                idx2 = startt + 2 * r;
                                t[idx2] = a[idx1];
                                t[idx2 + 1] = a[idx1 + 1];
                            }
                            fftRows.ComplexForward(t, startt);
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 2 * n0;
                                idx2 = startt + 2 * r;
                                a[idx1] = t[idx2];
                                a[idx1 + 1] = t[idx2 + 1];
                            }
                        }
                    }
                    else
                    {
                        if (columns > 4 * nthreads)
                        {
                            for (int c = 8 * n0; c < columns; c += 8 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    t[idx2] = a[idx1];
                                    t[idx2 + 1] = a[idx1 + 1];
                                    t[idx3] = a[idx1 + 2];
                                    t[idx3 + 1] = a[idx1 + 3];
                                    t[idx4] = a[idx1 + 4];
                                    t[idx4 + 1] = a[idx1 + 5];
                                    t[idx5] = a[idx1 + 6];
                                    t[idx5 + 1] = a[idx1 + 7];
                                }
                                fftRows.ComplexInverse(t, startt, scale);
                                fftRows.ComplexInverse(t, startt + 2 * rows, scale);
                                fftRows.ComplexInverse(t, startt + 4 * rows, scale);
                                fftRows.ComplexInverse(t, startt + 6 * rows, scale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = r * columns + c;
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    a[idx1] = t[idx2];
                                    a[idx1 + 1] = t[idx2 + 1];
                                    a[idx1 + 2] = t[idx3];
                                    a[idx1 + 3] = t[idx3 + 1];
                                    a[idx1 + 4] = t[idx4];
                                    a[idx1 + 5] = t[idx4 + 1];
                                    a[idx1 + 6] = t[idx5];
                                    a[idx1 + 7] = t[idx5 + 1];
                                }
                            }
                        }
                        else if (columns == 4 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 4 * n0;
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                t[idx2] = a[idx1];
                                t[idx2 + 1] = a[idx1 + 1];
                                t[idx3] = a[idx1 + 2];
                                t[idx3 + 1] = a[idx1 + 3];
                            }
                            fftRows.ComplexInverse(t, startt, scale);
                            fftRows.ComplexInverse(t, startt + 2 * rows, scale);
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 4 * n0;
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                a[idx1] = t[idx2];
                                a[idx1 + 1] = t[idx2 + 1];
                                a[idx1 + 2] = t[idx3];
                                a[idx1 + 3] = t[idx3 + 1];
                            }
                        }
                        else if (columns == 2 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 2 * n0;
                                idx2 = startt + 2 * r;
                                t[idx2] = a[idx1];
                                t[idx2 + 1] = a[idx1 + 1];
                            }
                            fftRows.ComplexInverse(t, startt, scale);
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = r * columns + 2 * n0;
                                idx2 = startt + 2 * r;
                                a[idx1] = t[idx2];
                                a[idx1 + 1] = t[idx2 + 1];
                            }
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

        private void cdft2d_subth(int isgn, float[][] a, Boolean scale)
        {
            int nthread = Process.GetCurrentProcess().Threads.Count;
            int nt = 8 * rows;
            if (columns == 4 * nthread)
            {
                nt >>= 1;
            }
            else if (columns < 4 * nthread)
            {
                nthread = columns >> 1;
                nt >>= 2;
            }
            Task[] taskArray = new Task[nthread];
            int nthreads = nthread;
            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx2, idx3, idx4, idx5;
                    if (isgn == -1)
                    {
                        if (columns > 4 * nthreads)
                        {
                            for (int c = 8 * n0; c < columns; c += 8 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    t[idx2] = a[r][c];
                                    t[idx2 + 1] = a[r][c + 1];
                                    t[idx3] = a[r][c + 2];
                                    t[idx3 + 1] = a[r][c + 3];
                                    t[idx4] = a[r][c + 4];
                                    t[idx4 + 1] = a[r][c + 5];
                                    t[idx5] = a[r][c + 6];
                                    t[idx5 + 1] = a[r][c + 7];
                                }
                                fftRows.ComplexForward(t, startt);
                                fftRows.ComplexForward(t, startt + 2 * rows);
                                fftRows.ComplexForward(t, startt + 4 * rows);
                                fftRows.ComplexForward(t, startt + 6 * rows);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    a[r][c] = t[idx2];
                                    a[r][c + 1] = t[idx2 + 1];
                                    a[r][c + 2] = t[idx3];
                                    a[r][c + 3] = t[idx3 + 1];
                                    a[r][c + 4] = t[idx4];
                                    a[r][c + 5] = t[idx4 + 1];
                                    a[r][c + 6] = t[idx5];
                                    a[r][c + 7] = t[idx5 + 1];
                                }
                            }
                        }
                        else if (columns == 4 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                t[idx2] = a[r][4 * n0];
                                t[idx2 + 1] = a[r][4 * n0 + 1];
                                t[idx3] = a[r][4 * n0 + 2];
                                t[idx3 + 1] = a[r][4 * n0 + 3];
                            }
                            fftRows.ComplexForward(t, startt);
                            fftRows.ComplexForward(t, startt + 2 * rows);
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                a[r][4 * n0] = t[idx2];
                                a[r][4 * n0 + 1] = t[idx2 + 1];
                                a[r][4 * n0 + 2] = t[idx3];
                                a[r][4 * n0 + 3] = t[idx3 + 1];
                            }
                        }
                        else if (columns == 2 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                t[idx2] = a[r][2 * n0];
                                t[idx2 + 1] = a[r][2 * n0 + 1];
                            }
                            fftRows.ComplexForward(t, startt);
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                a[r][2 * n0] = t[idx2];
                                a[r][2 * n0 + 1] = t[idx2 + 1];
                            }
                        }
                    }
                    else
                    {
                        if (columns > 4 * nthreads)
                        {
                            for (int c = 8 * n0; c < columns; c += 8 * nthreads)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    t[idx2] = a[r][c];
                                    t[idx2 + 1] = a[r][c + 1];
                                    t[idx3] = a[r][c + 2];
                                    t[idx3 + 1] = a[r][c + 3];
                                    t[idx4] = a[r][c + 4];
                                    t[idx4 + 1] = a[r][c + 5];
                                    t[idx5] = a[r][c + 6];
                                    t[idx5 + 1] = a[r][c + 7];
                                }
                                fftRows.ComplexInverse(t, startt, scale);
                                fftRows.ComplexInverse(t, startt + 2 * rows, scale);
                                fftRows.ComplexInverse(t, startt + 4 * rows, scale);
                                fftRows.ComplexInverse(t, startt + 6 * rows, scale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx2 = startt + 2 * r;
                                    idx3 = startt + 2 * rows + 2 * r;
                                    idx4 = idx3 + 2 * rows;
                                    idx5 = idx4 + 2 * rows;
                                    a[r][c] = t[idx2];
                                    a[r][c + 1] = t[idx2 + 1];
                                    a[r][c + 2] = t[idx3];
                                    a[r][c + 3] = t[idx3 + 1];
                                    a[r][c + 4] = t[idx4];
                                    a[r][c + 5] = t[idx4 + 1];
                                    a[r][c + 6] = t[idx5];
                                    a[r][c + 7] = t[idx5 + 1];
                                }
                            }
                        }
                        else if (columns == 4 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                t[idx2] = a[r][4 * n0];
                                t[idx2 + 1] = a[r][4 * n0 + 1];
                                t[idx3] = a[r][4 * n0 + 2];
                                t[idx3 + 1] = a[r][4 * n0 + 3];
                            }
                            fftRows.ComplexInverse(t, startt, scale);
                            fftRows.ComplexInverse(t, startt + 2 * rows, scale);
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                idx3 = startt + 2 * rows + 2 * r;
                                a[r][4 * n0] = t[idx2];
                                a[r][4 * n0 + 1] = t[idx2 + 1];
                                a[r][4 * n0 + 2] = t[idx3];
                                a[r][4 * n0 + 3] = t[idx3 + 1];
                            }
                        }
                        else if (columns == 2 * nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                t[idx2] = a[r][2 * n0];
                                t[idx2 + 1] = a[r][2 * n0 + 1];
                            }
                            fftRows.ComplexInverse(t, startt, scale);
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = startt + 2 * r;
                                a[r][2 * n0] = t[idx2];
                                a[r][2 * n0 + 1] = t[idx2 + 1];
                            }
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

        private void fillSymmetric(float[] a)
        {
            int twon2 = 2 * columns;
            int idx1, idx2, idx3, idx4;
            int n1d2 = rows / 2;

            for (int r = (rows - 1); r >= 1; r--)
            {
                idx1 = r * columns;
                idx2 = 2 * idx1;
                for (int c = 0; c < columns; c += 2)
                {
                    a[idx2 + c] = a[idx1 + c];
                    a[idx1 + c] = 0;
                    a[idx2 + c + 1] = a[idx1 + c + 1];
                    a[idx1 + c + 1] = 0;
                }
            }
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && useThreads && (n1d2 >= nthreads))
            {
                Task[] taskArray = new Task[nthreads];
                int l1k = n1d2 / nthreads;
                int newn2 = 2 * columns;
                for (int i = 0; i < nthreads; i++)
                {
                    int l1offa, l1stopa, l2offa, l2stopa;
                    if (i == 0)
                        l1offa = i * l1k + 1;
                    else
                    {
                        l1offa = i * l1k;
                    }
                    l1stopa = i * l1k + l1k;
                    l2offa = i * l1k;
                    if (i == nthreads - 1)
                    {
                        l2stopa = i * l1k + l1k + 1;
                    }
                    else
                    {
                        l2stopa = i * l1k + l1k;
                    }
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        int idx1, idx2, idx3, idx4;

                        for (int r = l1offa; r < l1stopa; r++)
                        {
                            idx1 = r * newn2;
                            idx2 = (rows - r) * newn2;
                            idx3 = idx1 + columns;
                            a[idx3] = a[idx2 + 1];
                            a[idx3 + 1] = -a[idx2];
                        }
                        for (int r = l1offa; r < l1stopa; r++)
                        {
                            idx1 = r * newn2;
                            idx3 = (rows - r + 1) * newn2;
                            for (int c = columns + 2; c < newn2; c += 2)
                            {
                                idx2 = idx3 - c;
                                idx4 = idx1 + c;
                                a[idx4] = a[idx2];
                                a[idx4 + 1] = -a[idx2 + 1];

                            }
                        }
                        for (int r = l2offa; r < l2stopa; r++)
                        {
                            idx3 = ((rows - r) % rows) * newn2;
                            idx4 = r * newn2;
                            for (int c = 0; c < newn2; c += 2)
                            {
                                idx1 = idx3 + (newn2 - c) % newn2;
                                idx2 = idx4 + c;
                                a[idx1] = a[idx2];
                                a[idx1 + 1] = -a[idx2 + 1];
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

                for (int r = 1; r < n1d2; r++)
                {
                    idx2 = r * twon2;
                    idx3 = (rows - r) * twon2;
                    a[idx2 + columns] = a[idx3 + 1];
                    a[idx2 + columns + 1] = -a[idx3];
                }

                for (int r = 1; r < n1d2; r++)
                {
                    idx2 = r * twon2;
                    idx3 = (rows - r + 1) * twon2;
                    for (int c = columns + 2; c < twon2; c += 2)
                    {
                        a[idx2 + c] = a[idx3 - c];
                        a[idx2 + c + 1] = -a[idx3 - c + 1];

                    }
                }
                for (int r = 0; r <= rows / 2; r++)
                {
                    idx1 = r * twon2;
                    idx4 = ((rows - r) % rows) * twon2;
                    for (int c = 0; c < twon2; c += 2)
                    {
                        idx2 = idx1 + c;
                        idx3 = idx4 + (twon2 - c) % twon2;
                        a[idx3] = a[idx2];
                        a[idx3 + 1] = -a[idx2 + 1];
                    }
                }
            }
            a[columns] = -a[1];
            a[1] = 0;
            idx1 = n1d2 * twon2;
            a[idx1 + columns] = -a[idx1 + 1];
            a[idx1 + 1] = 0;
            a[idx1 + columns + 1] = 0;
        }

        private void fillSymmetric(float[][] a)
        {
            int newn2 = 2 * columns;
            int n1d2 = rows / 2;

            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if ((nthreads > 1) && useThreads && (n1d2 >= nthreads))
            {
                Task[] taskArray = new Task[nthreads];
                int l1k = n1d2 / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int l1offa, l1stopa, l2offa, l2stopa;
                    if (i == 0)
                        l1offa = i * l1k + 1;
                    else
                    {
                        l1offa = i * l1k;
                    }
                    l1stopa = i * l1k + l1k;
                    l2offa = i * l1k;
                    if (i == nthreads - 1)
                    {
                        l2stopa = i * l1k + l1k + 1;
                    }
                    else
                    {
                        l2stopa = i * l1k + l1k;
                    }
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        int idx1, idx2;
                        for (int r = l1offa; r < l1stopa; r++)
                        {
                            idx1 = rows - r;
                            a[r][columns] = a[idx1][1];
                            a[r][columns + 1] = -a[idx1][0];
                        }
                        for (int r = l1offa; r < l1stopa; r++)
                        {
                            idx1 = rows - r;
                            for (int c = columns + 2; c < newn2; c += 2)
                            {
                                idx2 = newn2 - c;
                                a[r][c] = a[idx1][idx2];
                                a[r][c + 1] = -a[idx1][idx2 + 1];

                            }
                        }
                        for (int r = l2offa; r < l2stopa; r++)
                        {
                            idx1 = (rows - r) % rows;
                            for (int c = 0; c < newn2; c = c + 2)
                            {
                                idx2 = (newn2 - c) % newn2;
                                a[idx1][idx2] = a[r][c];
                                a[idx1][idx2 + 1] = -a[r][c + 1];
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

                for (int r = 1; r < n1d2; r++)
                {
                    int idx1 = rows - r;
                    a[r][columns] = a[idx1][1];
                    a[r][columns + 1] = -a[idx1][0];
                }
                for (int r = 1; r < n1d2; r++)
                {
                    int idx1 = rows - r;
                    for (int c = columns + 2; c < newn2; c += 2)
                    {
                        int idx2 = newn2 - c;
                        a[r][c] = a[idx1][idx2];
                        a[r][c + 1] = -a[idx1][idx2 + 1];
                    }
                }
                for (int r = 0; r <= rows / 2; r++)
                {
                    int idx1 = (rows - r) % rows;
                    for (int c = 0; c < newn2; c += 2)
                    {
                        int idx2 = (newn2 - c) % newn2;
                        a[idx1][idx2] = a[r][c];
                        a[idx1][idx2 + 1] = -a[r][c + 1];
                    }
                }
            }
            a[0][columns] = -a[0][1];
            a[0][1] = 0;
            a[n1d2][columns] = -a[n1d2][1];
            a[n1d2][1] = 0;
            a[n1d2][columns + 1] = 0;
        }
    }
}
