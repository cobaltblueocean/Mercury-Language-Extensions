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

namespace Mercury.Language.Math.Transform.DHT
{
    /// <summary>
    /// Computes 3D Discrete Hartley Transform (DHT) of real, double precision data.
    /// The sizes of all three dimensions can be arbitrary numbersd This is a
    /// parallel implementation optimized for SMP systems.<br>
    /// <br>
    /// Part of code is derived from General Purpose FFT Package written by Takuya Ooura
    /// (http://www.kurims.kyoto-u.ac.jp/~ooura/fft.html)
    /// 
    /// @author Piotr Wendykier (piotr.wendykier@gmail.com)
    /// 
    /// </summary>
    public class DoubleDHT_3D
    {
        private int slices;

        private int rows;

        private int columns;

        private int sliceStride;

        private int rowStride;

        private double[] t;

        private DoubleDHT_1D dhtSlices, dhtRows, dhtColumns;

        private int oldNthreads;

        private int nt;

        private Boolean isPowerOfTwo = false;

        private Boolean useThreads = false;

        /// <summary>
        /// Creates new instance of DoubleDHT_3D.
        /// 
        /// </summary>
        /// <param name="slices"></param>
        ///            number of slices
        /// <param name="rows"></param>
        ///            number of rows
        /// <param name="columns"></param>
        ///            number of columns
        public DoubleDHT_3D(int slices, int rows, int columns)
        {
            if (slices <= 1 || rows <= 1 || columns <= 1)
            {
                throw new ArgumentException("slices, rows and columns must be greater than 1");
            }
            this.slices = slices;
            this.rows = rows;
            this.columns = columns;
            this.sliceStride = rows * columns;
            this.rowStride = columns;
            if (slices * rows * columns >= TransformCore.THREADS_BEGIN_N_3D)
            {
                this.useThreads = true;
            }
            if (slices.IsPowerOf2() && rows.IsPowerOf2() && columns.IsPowerOf2())
            {
                isPowerOfTwo = true;
                oldNthreads = Process.GetCurrentProcess().Threads.Count;
                nt = slices;
                if (nt < rows)
                {
                    nt = rows;
                }
                nt *= 4;
                if (oldNthreads > 1)
                {
                    nt *= oldNthreads;
                }
                if (columns == 2)
                {
                    nt >>= 1;
                }
                t = new double[nt];
            }
            dhtSlices = new DoubleDHT_1D(slices);
            if (slices == rows)
            {
                dhtRows = dhtSlices;
            }
            else
            {
                dhtRows = new DoubleDHT_1D(rows);
            }
            if (slices == columns)
            {
                dhtColumns = dhtSlices;
            }
            else if (rows == columns)
            {
                dhtColumns = dhtRows;
            }
            else
            {
                dhtColumns = new DoubleDHT_1D(columns);
            }
        }

        /// <summary>
        /// Computes the 3D real, forward DHT leaving the result in <code>a</code>.
        /// The data is stored in 1D array addressed in slice-major, then row-major,
        /// then column-major, in order of significance, i.ed the element (i,j,k) of
        /// 3D array x[slices][rows][columns] is stored in a[i*sliceStride +
        /// j*rowStride + k], where sliceStride = rows * columns and rowStride =
        /// columns.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void Forward(double[] a)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = slices;
                    if (nt < rows)
                    {
                        nt = rows;
                    }
                    nt *= 4;
                    if (nthreads > 1)
                    {
                        nt *= nthreads;
                    }
                    if (columns == 2)
                    {
                        nt >>= 1;
                    }
                    t = new double[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt3da_subth(-1, a, true);
                    ddxt3db_subth(-1, a, true);
                }
                else
                {
                    ddxt3da_sub(-1, a, true);
                    ddxt3db_sub(-1, a, true);
                }
                yTransform(a);
            }
            else
            {
                if ((nthreads > 1) && useThreads && (slices >= nthreads) && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = slices / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                int idx1 = s * sliceStride;
                                for (int r = 0; r < rows; r++)
                                {
                                    dhtColumns.Forward(a, idx1 + r * rowStride);
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
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[rows];
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                int idx1 = s * sliceStride;
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        int idx3 = idx1 + r * rowStride + c;
                                        temp[r] = a[idx3];
                                    }
                                    dhtRows.Forward(temp);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        int idx3 = idx1 + r * rowStride + c;
                                        a[idx3] = temp[r];
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


                    p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int startRow = l * p;
                        int stopRow;
                        if (l == nthreads - 1)
                        {
                            stopRow = rows;
                        }
                        else
                        {
                            stopRow = startRow + p;
                        }
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[slices];
                            for (int r = startRow; r < stopRow; r++)
                            {
                                int idx1 = r * rowStride;
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        int idx3 = s * sliceStride + idx1 + c;
                                        temp[s] = a[idx3];
                                    }
                                    dhtSlices.Forward(temp);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        int idx3 = s * sliceStride + idx1 + c;
                                        a[idx3] = temp[s];
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
                else
                {
                    for (int s = 0; s < slices; s++)
                    {
                        int idx1 = s * sliceStride;
                        for (int r = 0; r < rows; r++)
                        {
                            dhtColumns.Forward(a, idx1 + r * rowStride);
                        }
                    }
                    double[] temp = new double[rows];
                    for (int s = 0; s < slices; s++)
                    {
                        int idx1 = s * sliceStride;
                        for (int c = 0; c < columns; c++)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                int idx3 = idx1 + r * rowStride + c;
                                temp[r] = a[idx3];
                            }
                            dhtRows.Forward(temp);
                            for (int r = 0; r < rows; r++)
                            {
                                int idx3 = idx1 + r * rowStride + c;
                                a[idx3] = temp[r];
                            }
                        }
                    }
                    temp = new double[slices];
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = r * rowStride;
                        for (int c = 0; c < columns; c++)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                int idx3 = s * sliceStride + idx1 + c;
                                temp[s] = a[idx3];
                            }
                            dhtSlices.Forward(temp);
                            for (int s = 0; s < slices; s++)
                            {
                                int idx3 = s * sliceStride + idx1 + c;
                                a[idx3] = temp[s];
                            }
                        }
                    }
                }
                yTransform(a);
            }
        }

        /// <summary>
        /// Computes the 3D real, forward DHT leaving the result in <code>a</code>.
        /// The data is stored in 3D array.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void Forward(double[][][] a)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = slices;
                    if (nt < rows)
                    {
                        nt = rows;
                    }
                    nt *= 4;
                    if (nthreads > 1)
                    {
                        nt *= nthreads;
                    }
                    if (columns == 2)
                    {
                        nt >>= 1;
                    }
                    t = new double[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt3da_subth(-1, a, true);
                    ddxt3db_subth(-1, a, true);
                }
                else
                {
                    ddxt3da_sub(-1, a, true);
                    ddxt3db_sub(-1, a, true);
                }
                yTransform(a);
            }
            else
            {
                if ((nthreads > 1) && useThreads && (slices >= nthreads) && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = slices / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    dhtColumns.Forward(a[s][r]);
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
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[rows];
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        temp[r] = a[s][r][c];
                                    }
                                    dhtRows.Forward(temp);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        a[s][r][c] = temp[r];
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

                    p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[slices];
                            for (int r = firstRow; r < lastRow; r++)
                            {
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        temp[s] = a[s][r][c];
                                    }
                                    dhtSlices.Forward(temp);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        a[s][r][c] = temp[s];
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
                else
                {
                    for (int s = 0; s < slices; s++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            dhtColumns.Forward(a[s][r]);
                        }
                    }
                    double[] temp = new double[rows];
                    for (int s = 0; s < slices; s++)
                    {
                        for (int c = 0; c < columns; c++)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                temp[r] = a[s][r][c];
                            }
                            dhtRows.Forward(temp);
                            for (int r = 0; r < rows; r++)
                            {
                                a[s][r][c] = temp[r];
                            }
                        }
                    }
                    temp = new double[slices];
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < columns; c++)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                temp[s] = a[s][r][c];
                            }
                            dhtSlices.Forward(temp);
                            for (int s = 0; s < slices; s++)
                            {
                                a[s][r][c] = temp[s];
                            }
                        }
                    }
                }
                yTransform(a);
            }
        }

        /// <summary>
        /// Computes the 3D real, inverse DHT leaving the result in <code>a</code>.
        /// The data is stored in 1D array addressed in slice-major, then row-major,
        /// then column-major, in order of significance, i.ed the element (i,j,k) of
        /// 3D array x[slices][rows][columns] is stored in a[i*sliceStride +
        /// j*rowStride + k], where sliceStride = rows * columns and rowStride =
        /// columns.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// <param name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(double[] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = slices;
                    if (nt < rows)
                    {
                        nt = rows;
                    }
                    nt *= 4;
                    if (nthreads > 1)
                    {
                        nt *= nthreads;
                    }
                    if (columns == 2)
                    {
                        nt >>= 1;
                    }
                    t = new double[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt3da_subth(1, a, isScale);
                    ddxt3db_subth(1, a, isScale);
                }
                else
                {
                    ddxt3da_sub(1, a, isScale);
                    ddxt3db_sub(1, a, isScale);
                }
                yTransform(a);
            }
            else
            {
                if ((nthreads > 1) && useThreads && (slices >= nthreads) && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = slices / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                int idx1 = s * sliceStride;
                                for (int r = 0; r < rows; r++)
                                {
                                    dhtColumns.Inverse(a, idx1 + r * rowStride, isScale);
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
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[rows];
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                int idx1 = s * sliceStride;
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        int idx3 = idx1 + r * rowStride + c;
                                        temp[r] = a[idx3];
                                    }
                                    dhtRows.Inverse(temp, isScale);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        int idx3 = idx1 + r * rowStride + c;
                                        a[idx3] = temp[r];
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


                    p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[slices];
                            for (int r = firstRow; r < lastRow; r++)
                            {
                                int idx1 = r * rowStride;
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        int idx3 = s * sliceStride + idx1 + c;
                                        temp[s] = a[idx3];
                                    }
                                    dhtSlices.Inverse(temp, isScale);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        int idx3 = s * sliceStride + idx1 + c;
                                        a[idx3] = temp[s];
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
                else
                {
                    for (int s = 0; s < slices; s++)
                    {
                        int idx1 = s * sliceStride;
                        for (int r = 0; r < rows; r++)
                        {
                            dhtColumns.Inverse(a, idx1 + r * rowStride, isScale);
                        }
                    }
                    double[] temp = new double[rows];
                    for (int s = 0; s < slices; s++)
                    {
                        int idx1 = s * sliceStride;
                        for (int c = 0; c < columns; c++)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                int idx3 = idx1 + r * rowStride + c;
                                temp[r] = a[idx3];
                            }
                            dhtRows.Inverse(temp, isScale);
                            for (int r = 0; r < rows; r++)
                            {
                                int idx3 = idx1 + r * rowStride + c;
                                a[idx3] = temp[r];
                            }
                        }
                    }
                    temp = new double[slices];
                    for (int r = 0; r < rows; r++)
                    {
                        int idx1 = r * rowStride;
                        for (int c = 0; c < columns; c++)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                int idx3 = s * sliceStride + idx1 + c;
                                temp[s] = a[idx3];
                            }
                            dhtSlices.Inverse(temp, isScale);
                            for (int s = 0; s < slices; s++)
                            {
                                int idx3 = s * sliceStride + idx1 + c;
                                a[idx3] = temp[s];
                            }
                        }
                    }
                }
                yTransform(a);
            }
        }

        /// <summary>
        /// Computes the 3D real, inverse DHT leaving the result in <code>a</code>.
        /// The data is stored in 3D array.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// <param name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(double[][][] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            if (isPowerOfTwo)
            {
                if (nthreads != oldNthreads)
                {
                    nt = slices;
                    if (nt < rows)
                    {
                        nt = rows;
                    }
                    nt *= 4;
                    if (nthreads > 1)
                    {
                        nt *= nthreads;
                    }
                    if (columns == 2)
                    {
                        nt >>= 1;
                    }
                    t = new double[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt3da_subth(1, a, isScale);
                    ddxt3db_subth(1, a, isScale);
                }
                else
                {
                    ddxt3da_sub(1, a, isScale);
                    ddxt3db_sub(1, a, isScale);
                }
                yTransform(a);
            }
            else
            {
                if ((nthreads > 1) && useThreads && (slices >= nthreads) && (rows >= nthreads) && (columns >= nthreads))
                {
                    Task[] taskArray = new Task[nthreads];
                    int p = slices / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    dhtColumns.Inverse(a[s][r], isScale);
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
                        int firstSlice = l * p;
                        int lastSlice = (l == (nthreads - 1)) ? slices : firstSlice + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[rows];
                            for (int s = firstSlice; s < lastSlice; s++)
                            {
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        temp[r] = a[s][r][c];
                                    }
                                    dhtRows.Inverse(temp, isScale);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        a[s][r][c] = temp[r];
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


                    p = rows / nthreads;
                    for (int l = 0; l < nthreads; l++)
                    {
                        int firstRow = l * p;
                        int lastRow = (l == (nthreads - 1)) ? rows : firstRow + p;
                        taskArray[l] = Task.Factory.StartNew(() =>
                        {
                            double[] temp = new double[slices];
                            for (int r = firstRow; r < lastRow; r++)
                            {
                                for (int c = 0; c < columns; c++)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        temp[s] = a[s][r][c];
                                    }
                                    dhtSlices.Inverse(temp, isScale);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        a[s][r][c] = temp[s];
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
                else
                {
                    for (int s = 0; s < slices; s++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            dhtColumns.Inverse(a[s][r], isScale);
                        }
                    }
                    double[] temp = new double[rows];
                    for (int s = 0; s < slices; s++)
                    {
                        for (int c = 0; c < columns; c++)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                temp[r] = a[s][r][c];
                            }
                            dhtRows.Inverse(temp, isScale);
                            for (int r = 0; r < rows; r++)
                            {
                                a[s][r][c] = temp[r];
                            }
                        }
                    }
                    temp = new double[slices];
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < columns; c++)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                temp[s] = a[s][r][c];
                            }
                            dhtSlices.Inverse(temp, isScale);
                            for (int s = 0; s < slices; s++)
                            {
                                a[s][r][c] = temp[s];
                            }
                        }
                    }
                }
                yTransform(a);
            }
        }

        private void ddxt3da_sub(int isgn, double[] a, Boolean isScale)
        {
            int idx0, idx1, idx2;

            if (isgn == -1)
            {
                for (int s = 0; s < slices; s++)
                {
                    idx0 = s * sliceStride;
                    for (int r = 0; r < rows; r++)
                    {
                        dhtColumns.Forward(a, idx0 + r * rowStride);
                    }
                    if (columns > 2)
                    {
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = idx0 + r * rowStride + c;
                                idx2 = rows + r;
                                t[r] = a[idx1];
                                t[idx2] = a[idx1 + 1];
                                t[idx2 + rows] = a[idx1 + 2];
                                t[idx2 + 2 * rows] = a[idx1 + 3];
                            }
                            dhtRows.Forward(t, 0);
                            dhtRows.Forward(t, rows);
                            dhtRows.Forward(t, 2 * rows);
                            dhtRows.Forward(t, 3 * rows);
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = idx0 + r * rowStride + c;
                                idx2 = rows + r;
                                a[idx1] = t[r];
                                a[idx1 + 1] = t[idx2];
                                a[idx1 + 2] = t[idx2 + rows];
                                a[idx1 + 3] = t[idx2 + 2 * rows];
                            }
                        }
                    }
                    else if (columns == 2)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = idx0 + r * rowStride;
                            t[r] = a[idx1];
                            t[rows + r] = a[idx1 + 1];
                        }
                        dhtRows.Forward(t, 0);
                        dhtRows.Forward(t, rows);
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = idx0 + r * rowStride;
                            a[idx1] = t[r];
                            a[idx1 + 1] = t[rows + r];
                        }
                    }
                }
            }
            else
            {
                for (int s = 0; s < slices; s++)
                {
                    idx0 = s * sliceStride;
                    for (int r = 0; r < rows; r++)
                    {
                        dhtColumns.Inverse(a, idx0 + r * rowStride, isScale);
                    }
                    if (columns > 2)
                    {
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = idx0 + r * rowStride + c;
                                idx2 = rows + r;
                                t[r] = a[idx1];
                                t[idx2] = a[idx1 + 1];
                                t[idx2 + rows] = a[idx1 + 2];
                                t[idx2 + 2 * rows] = a[idx1 + 3];
                            }
                            dhtRows.Inverse(t, 0, isScale);
                            dhtRows.Inverse(t, rows, isScale);
                            dhtRows.Inverse(t, 2 * rows, isScale);
                            dhtRows.Inverse(t, 3 * rows, isScale);
                            for (int r = 0; r < rows; r++)
                            {
                                idx1 = idx0 + r * rowStride + c;
                                idx2 = rows + r;
                                a[idx1] = t[r];
                                a[idx1 + 1] = t[idx2];
                                a[idx1 + 2] = t[idx2 + rows];
                                a[idx1 + 3] = t[idx2 + 2 * rows];
                            }
                        }
                    }
                    else if (columns == 2)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = idx0 + r * rowStride;
                            t[r] = a[idx1];
                            t[rows + r] = a[idx1 + 1];
                        }
                        dhtRows.Inverse(t, 0, isScale);
                        dhtRows.Inverse(t, rows, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            idx1 = idx0 + r * rowStride;
                            a[idx1] = t[r];
                            a[idx1 + 1] = t[rows + r];
                        }
                    }
                }
            }
        }

        private void ddxt3da_sub(int isgn, double[][][] a, Boolean isScale)
        {
            int idx2;

            if (isgn == -1)
            {
                for (int s = 0; s < slices; s++)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        dhtColumns.Forward(a[s][r]);
                    }
                    if (columns > 2)
                    {
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = rows + r;
                                t[r] = a[s][r][c];
                                t[idx2] = a[s][r][c + 1];
                                t[idx2 + rows] = a[s][r][c + 2];
                                t[idx2 + 2 * rows] = a[s][r][c + 3];
                            }
                            dhtRows.Forward(t, 0);
                            dhtRows.Forward(t, rows);
                            dhtRows.Forward(t, 2 * rows);
                            dhtRows.Forward(t, 3 * rows);
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = rows + r;
                                a[s][r][c] = t[r];
                                a[s][r][c + 1] = t[idx2];
                                a[s][r][c + 2] = t[idx2 + rows];
                                a[s][r][c + 3] = t[idx2 + 2 * rows];
                            }
                        }
                    }
                    else if (columns == 2)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            t[r] = a[s][r][0];
                            t[rows + r] = a[s][r][1];
                        }
                        dhtRows.Forward(t, 0);
                        dhtRows.Forward(t, rows);
                        for (int r = 0; r < rows; r++)
                        {
                            a[s][r][0] = t[r];
                            a[s][r][1] = t[rows + r];
                        }
                    }
                }
            }
            else
            {
                for (int s = 0; s < slices; s++)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        dhtColumns.Inverse(a[s][r], isScale);
                    }
                    if (columns > 2)
                    {
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = rows + r;
                                t[r] = a[s][r][c];
                                t[idx2] = a[s][r][c + 1];
                                t[idx2 + rows] = a[s][r][c + 2];
                                t[idx2 + 2 * rows] = a[s][r][c + 3];
                            }
                            dhtRows.Inverse(t, 0, isScale);
                            dhtRows.Inverse(t, rows, isScale);
                            dhtRows.Inverse(t, 2 * rows, isScale);
                            dhtRows.Inverse(t, 3 * rows, isScale);
                            for (int r = 0; r < rows; r++)
                            {
                                idx2 = rows + r;
                                a[s][r][c] = t[r];
                                a[s][r][c + 1] = t[idx2];
                                a[s][r][c + 2] = t[idx2 + rows];
                                a[s][r][c + 3] = t[idx2 + 2 * rows];
                            }
                        }
                    }
                    else if (columns == 2)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            t[r] = a[s][r][0];
                            t[rows + r] = a[s][r][1];
                        }
                        dhtRows.Inverse(t, 0, isScale);
                        dhtRows.Inverse(t, rows, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[s][r][0] = t[r];
                            a[s][r][1] = t[rows + r];
                        }
                    }
                }
            }
        }

        private void ddxt3db_sub(int isgn, double[] a, Boolean isScale)
        {
            int idx0, idx1, idx2;

            if (isgn == -1)
            {
                if (columns > 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx0 = r * rowStride;
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                idx1 = s * sliceStride + idx0 + c;
                                idx2 = slices + s;
                                t[s] = a[idx1];
                                t[idx2] = a[idx1 + 1];
                                t[idx2 + slices] = a[idx1 + 2];
                                t[idx2 + 2 * slices] = a[idx1 + 3];
                            }
                            dhtSlices.Forward(t, 0);
                            dhtSlices.Forward(t, slices);
                            dhtSlices.Forward(t, 2 * slices);
                            dhtSlices.Forward(t, 3 * slices);
                            for (int s = 0; s < slices; s++)
                            {
                                idx1 = s * sliceStride + idx0 + c;
                                idx2 = slices + s;
                                a[idx1] = t[s];
                                a[idx1 + 1] = t[idx2];
                                a[idx1 + 2] = t[idx2 + slices];
                                a[idx1 + 3] = t[idx2 + 2 * slices];
                            }
                        }
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx0 = r * rowStride;
                        for (int s = 0; s < slices; s++)
                        {
                            idx1 = s * sliceStride + idx0;
                            t[s] = a[idx1];
                            t[slices + s] = a[idx1 + 1];
                        }
                        dhtSlices.Forward(t, 0);
                        dhtSlices.Forward(t, slices);
                        for (int s = 0; s < slices; s++)
                        {
                            idx1 = s * sliceStride + idx0;
                            a[idx1] = t[s];
                            a[idx1 + 1] = t[slices + s];
                        }
                    }
                }
            }
            else
            {
                if (columns > 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx0 = r * rowStride;
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                idx1 = s * sliceStride + idx0 + c;
                                idx2 = slices + s;
                                t[s] = a[idx1];
                                t[idx2] = a[idx1 + 1];
                                t[idx2 + slices] = a[idx1 + 2];
                                t[idx2 + 2 * slices] = a[idx1 + 3];
                            }
                            dhtSlices.Inverse(t, 0, isScale);
                            dhtSlices.Inverse(t, slices, isScale);
                            dhtSlices.Inverse(t, 2 * slices, isScale);
                            dhtSlices.Inverse(t, 3 * slices, isScale);

                            for (int s = 0; s < slices; s++)
                            {
                                idx1 = s * sliceStride + idx0 + c;
                                idx2 = slices + s;
                                a[idx1] = t[s];
                                a[idx1 + 1] = t[idx2];
                                a[idx1 + 2] = t[idx2 + slices];
                                a[idx1 + 3] = t[idx2 + 2 * slices];
                            }
                        }
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        idx0 = r * rowStride;
                        for (int s = 0; s < slices; s++)
                        {
                            idx1 = s * sliceStride + idx0;
                            t[s] = a[idx1];
                            t[slices + s] = a[idx1 + 1];
                        }
                        dhtSlices.Inverse(t, 0, isScale);
                        dhtSlices.Inverse(t, slices, isScale);
                        for (int s = 0; s < slices; s++)
                        {
                            idx1 = s * sliceStride + idx0;
                            a[idx1] = t[s];
                            a[idx1 + 1] = t[slices + s];
                        }
                    }
                }
            }
        }

        private void ddxt3db_sub(int isgn, double[][][] a, Boolean isScale)
        {
            int idx2;

            if (isgn == -1)
            {
                if (columns > 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                idx2 = slices + s;
                                t[s] = a[s][r][c];
                                t[idx2] = a[s][r][c + 1];
                                t[idx2 + slices] = a[s][r][c + 2];
                                t[idx2 + 2 * slices] = a[s][r][c + 3];
                            }
                            dhtSlices.Forward(t, 0);
                            dhtSlices.Forward(t, slices);
                            dhtSlices.Forward(t, 2 * slices);
                            dhtSlices.Forward(t, 3 * slices);
                            for (int s = 0; s < slices; s++)
                            {
                                idx2 = slices + s;
                                a[s][r][c] = t[s];
                                a[s][r][c + 1] = t[idx2];
                                a[s][r][c + 2] = t[idx2 + slices];
                                a[s][r][c + 3] = t[idx2 + 2 * slices];
                            }
                        }
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        for (int s = 0; s < slices; s++)
                        {
                            t[s] = a[s][r][0];
                            t[slices + s] = a[s][r][1];
                        }
                        dhtSlices.Forward(t, 0);
                        dhtSlices.Forward(t, slices);
                        for (int s = 0; s < slices; s++)
                        {
                            a[s][r][0] = t[s];
                            a[s][r][1] = t[slices + s];
                        }
                    }
                }
            }
            else
            {
                if (columns > 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < columns; c += 4)
                        {
                            for (int s = 0; s < slices; s++)
                            {
                                idx2 = slices + s;
                                t[s] = a[s][r][c];
                                t[idx2] = a[s][r][c + 1];
                                t[idx2 + slices] = a[s][r][c + 2];
                                t[idx2 + 2 * slices] = a[s][r][c + 3];
                            }
                            dhtSlices.Inverse(t, 0, isScale);
                            dhtSlices.Inverse(t, slices, isScale);
                            dhtSlices.Inverse(t, 2 * slices, isScale);
                            dhtSlices.Inverse(t, 3 * slices, isScale);

                            for (int s = 0; s < slices; s++)
                            {
                                idx2 = slices + s;
                                a[s][r][c] = t[s];
                                a[s][r][c + 1] = t[idx2];
                                a[s][r][c + 2] = t[idx2 + slices];
                                a[s][r][c + 3] = t[idx2 + 2 * slices];
                            }
                        }
                    }
                }
                else if (columns == 2)
                {
                    for (int r = 0; r < rows; r++)
                    {
                        for (int s = 0; s < slices; s++)
                        {
                            t[s] = a[s][r][0];
                            t[slices + s] = a[s][r][1];
                        }
                        dhtSlices.Inverse(t, 0, isScale);
                        dhtSlices.Inverse(t, slices, isScale);
                        for (int s = 0; s < slices; s++)
                        {
                            a[s][r][0] = t[s];
                            a[s][r][1] = t[slices + s];
                        }
                    }
                }
            }
        }

        private void ddxt3da_subth(int isgn, double[] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > slices ? slices : Process.GetCurrentProcess().Threads.Count;
            int nt = 4 * rows;
            if (columns == 2)
            {
                nt >>= 1;
            }
            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx0, idx1, idx2;
                    if (isgn == -1)
                    {
                        for (int s = n0; s < slices; s += nthreads)
                        {
                            idx0 = s * sliceStride;
                            for (int r = 0; r < rows; r++)
                            {
                                dhtColumns.Forward(a, idx0 + r * rowStride);
                            }
                            if (columns > 2)
                            {
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx1 = idx0 + r * rowStride + c;
                                        idx2 = startt + rows + r;
                                        t[startt + r] = a[idx1];
                                        t[idx2] = a[idx1 + 1];
                                        t[idx2 + rows] = a[idx1 + 2];
                                        t[idx2 + 2 * rows] = a[idx1 + 3];
                                    }
                                    dhtRows.Forward(t, startt);
                                    dhtRows.Forward(t, startt + rows);
                                    dhtRows.Forward(t, startt + 2 * rows);
                                    dhtRows.Forward(t, startt + 3 * rows);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx1 = idx0 + r * rowStride + c;
                                        idx2 = startt + rows + r;
                                        a[idx1] = t[startt + r];
                                        a[idx1 + 1] = t[idx2];
                                        a[idx1 + 2] = t[idx2 + rows];
                                        a[idx1 + 3] = t[idx2 + 2 * rows];
                                    }
                                }
                            }
                            else if (columns == 2)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = idx0 + r * rowStride;
                                    t[startt + r] = a[idx1];
                                    t[startt + rows + r] = a[idx1 + 1];
                                }
                                dhtRows.Forward(t, startt);
                                dhtRows.Forward(t, startt + rows);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = idx0 + r * rowStride;
                                    a[idx1] = t[startt + r];
                                    a[idx1 + 1] = t[startt + rows + r];
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int s = n0; s < slices; s += nthreads)
                        {
                            idx0 = s * sliceStride;
                            for (int r = 0; r < rows; r++)
                            {
                                dhtColumns.Inverse(a, idx0 + r * rowStride, isScale);
                            }
                            if (columns > 2)
                            {
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx1 = idx0 + r * rowStride + c;
                                        idx2 = startt + rows + r;
                                        t[startt + r] = a[idx1];
                                        t[idx2] = a[idx1 + 1];
                                        t[idx2 + rows] = a[idx1 + 2];
                                        t[idx2 + 2 * rows] = a[idx1 + 3];
                                    }
                                    dhtRows.Inverse(t, startt, isScale);
                                    dhtRows.Inverse(t, startt + rows, isScale);
                                    dhtRows.Inverse(t, startt + 2 * rows, isScale);
                                    dhtRows.Inverse(t, startt + 3 * rows, isScale);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx1 = idx0 + r * rowStride + c;
                                        idx2 = startt + rows + r;
                                        a[idx1] = t[startt + r];
                                        a[idx1 + 1] = t[idx2];
                                        a[idx1 + 2] = t[idx2 + rows];
                                        a[idx1 + 3] = t[idx2 + 2 * rows];
                                    }
                                }
                            }
                            else if (columns == 2)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = idx0 + r * rowStride;
                                    t[startt + r] = a[idx1];
                                    t[startt + rows + r] = a[idx1 + 1];
                                }
                                dhtRows.Inverse(t, startt, isScale);
                                dhtRows.Inverse(t, startt + rows, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    idx1 = idx0 + r * rowStride;
                                    a[idx1] = t[startt + r];
                                    a[idx1 + 1] = t[startt + rows + r];
                                }
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

        private void ddxt3da_subth(int isgn, double[][][] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > slices ? slices : Process.GetCurrentProcess().Threads.Count;
            int nt = 4 * rows;
            if (columns == 2)
            {
                nt >>= 1;
            }
            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx2;
                    if (isgn == -1)
                    {
                        for (int s = n0; s < slices; s += nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                dhtColumns.Forward(a[s][r]);
                            }
                            if (columns > 2)
                            {
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx2 = startt + rows + r;
                                        t[startt + r] = a[s][r][c];
                                        t[idx2] = a[s][r][c + 1];
                                        t[idx2 + rows] = a[s][r][c + 2];
                                        t[idx2 + 2 * rows] = a[s][r][c + 3];
                                    }
                                    dhtRows.Forward(t, startt);
                                    dhtRows.Forward(t, startt + rows);
                                    dhtRows.Forward(t, startt + 2 * rows);
                                    dhtRows.Forward(t, startt + 3 * rows);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx2 = startt + rows + r;
                                        a[s][r][c] = t[startt + r];
                                        a[s][r][c + 1] = t[idx2];
                                        a[s][r][c + 2] = t[idx2 + rows];
                                        a[s][r][c + 3] = t[idx2 + 2 * rows];
                                    }
                                }
                            }
                            else if (columns == 2)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    t[startt + r] = a[s][r][0];
                                    t[startt + rows + r] = a[s][r][1];
                                }
                                dhtRows.Forward(t, startt);
                                dhtRows.Forward(t, startt + rows);
                                for (int r = 0; r < rows; r++)
                                {
                                    a[s][r][0] = t[startt + r];
                                    a[s][r][1] = t[startt + rows + r];
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int s = n0; s < slices; s += nthreads)
                        {
                            for (int r = 0; r < rows; r++)
                            {
                                dhtColumns.Inverse(a[s][r], isScale);
                            }
                            if (columns > 2)
                            {
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx2 = startt + rows + r;
                                        t[startt + r] = a[s][r][c];
                                        t[idx2] = a[s][r][c + 1];
                                        t[idx2 + rows] = a[s][r][c + 2];
                                        t[idx2 + 2 * rows] = a[s][r][c + 3];
                                    }
                                    dhtRows.Inverse(t, startt, isScale);
                                    dhtRows.Inverse(t, startt + rows, isScale);
                                    dhtRows.Inverse(t, startt + 2 * rows, isScale);
                                    dhtRows.Inverse(t, startt + 3 * rows, isScale);
                                    for (int r = 0; r < rows; r++)
                                    {
                                        idx2 = startt + rows + r;
                                        a[s][r][c] = t[startt + r];
                                        a[s][r][c + 1] = t[idx2];
                                        a[s][r][c + 2] = t[idx2 + rows];
                                        a[s][r][c + 3] = t[idx2 + 2 * rows];
                                    }
                                }
                            }
                            else if (columns == 2)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    t[startt + r] = a[s][r][0];
                                    t[startt + rows + r] = a[s][r][1];
                                }
                                dhtRows.Inverse(t, startt, isScale);
                                dhtRows.Inverse(t, startt + rows, isScale);
                                for (int r = 0; r < rows; r++)
                                {
                                    a[s][r][0] = t[startt + r];
                                    a[s][r][1] = t[startt + rows + r];
                                }
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

        private void ddxt3db_subth(int isgn, double[] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;
            int nt = 4 * slices;
            if (columns == 2)
            {
                nt >>= 1;
            }
            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx0, idx1, idx2;
                    if (isgn == -1)
                    {
                        if (columns > 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                idx0 = r * rowStride;
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx1 = s * sliceStride + idx0 + c;
                                        idx2 = startt + slices + s;
                                        t[startt + s] = a[idx1];
                                        t[idx2] = a[idx1 + 1];
                                        t[idx2 + slices] = a[idx1 + 2];
                                        t[idx2 + 2 * slices] = a[idx1 + 3];
                                    }
                                    dhtSlices.Forward(t, startt);
                                    dhtSlices.Forward(t, startt + slices);
                                    dhtSlices.Forward(t, startt + 2 * slices);
                                    dhtSlices.Forward(t, startt + 3 * slices);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx1 = s * sliceStride + idx0 + c;
                                        idx2 = startt + slices + s;
                                        a[idx1] = t[startt + s];
                                        a[idx1 + 1] = t[idx2];
                                        a[idx1 + 2] = t[idx2 + slices];
                                        a[idx1 + 3] = t[idx2 + 2 * slices];
                                    }
                                }
                            }
                        }
                        else if (columns == 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                idx0 = r * rowStride;
                                for (int s = 0; s < slices; s++)
                                {
                                    idx1 = s * sliceStride + idx0;
                                    t[startt + s] = a[idx1];
                                    t[startt + slices + s] = a[idx1 + 1];
                                }
                                dhtSlices.Forward(t, startt);
                                dhtSlices.Forward(t, startt + slices);
                                for (int s = 0; s < slices; s++)
                                {
                                    idx1 = s * sliceStride + idx0;
                                    a[idx1] = t[startt + s];
                                    a[idx1 + 1] = t[startt + slices + s];
                                }
                            }
                        }
                    }
                    else
                    {
                        if (columns > 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                idx0 = r * rowStride;
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx1 = s * sliceStride + idx0 + c;
                                        idx2 = startt + slices + s;
                                        t[startt + s] = a[idx1];
                                        t[idx2] = a[idx1 + 1];
                                        t[idx2 + slices] = a[idx1 + 2];
                                        t[idx2 + 2 * slices] = a[idx1 + 3];
                                    }
                                    dhtSlices.Inverse(t, startt, isScale);
                                    dhtSlices.Inverse(t, startt + slices, isScale);
                                    dhtSlices.Inverse(t, startt + 2 * slices, isScale);
                                    dhtSlices.Inverse(t, startt + 3 * slices, isScale);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx1 = s * sliceStride + idx0 + c;
                                        idx2 = startt + slices + s;
                                        a[idx1] = t[startt + s];
                                        a[idx1 + 1] = t[idx2];
                                        a[idx1 + 2] = t[idx2 + slices];
                                        a[idx1 + 3] = t[idx2 + 2 * slices];
                                    }
                                }
                            }
                        }
                        else if (columns == 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                idx0 = r * rowStride;
                                for (int s = 0; s < slices; s++)
                                {
                                    idx1 = s * sliceStride + idx0;
                                    t[startt + s] = a[idx1];
                                    t[startt + slices + s] = a[idx1 + 1];
                                }
                                dhtSlices.Inverse(t, startt, isScale);
                                dhtSlices.Inverse(t, startt + slices, isScale);
                                for (int s = 0; s < slices; s++)
                                {
                                    idx1 = s * sliceStride + idx0;
                                    a[idx1] = t[startt + s];
                                    a[idx1 + 1] = t[startt + slices + s];
                                }
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

        private void ddxt3db_subth(int isgn, double[][][] a, Boolean isScale)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count > rows ? rows : Process.GetCurrentProcess().Threads.Count;
            int nt = 4 * slices;
            if (columns == 2)
            {
                nt >>= 1;
            }
            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
            {
                int n0 = i;
                int startt = nt * i;
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    int idx2;
                    if (isgn == -1)
                    {
                        if (columns > 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx2 = startt + slices + s;
                                        t[startt + s] = a[s][r][c];
                                        t[idx2] = a[s][r][c + 1];
                                        t[idx2 + slices] = a[s][r][c + 2];
                                        t[idx2 + 2 * slices] = a[s][r][c + 3];
                                    }
                                    dhtSlices.Forward(t, startt);
                                    dhtSlices.Forward(t, startt + slices);
                                    dhtSlices.Forward(t, startt + 2 * slices);
                                    dhtSlices.Forward(t, startt + 3 * slices);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx2 = startt + slices + s;
                                        a[s][r][c] = t[startt + s];
                                        a[s][r][c + 1] = t[idx2];
                                        a[s][r][c + 2] = t[idx2 + slices];
                                        a[s][r][c + 3] = t[idx2 + 2 * slices];
                                    }
                                }
                            }
                        }
                        else if (columns == 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                for (int s = 0; s < slices; s++)
                                {
                                    t[startt + s] = a[s][r][0];
                                    t[startt + slices + s] = a[s][r][1];
                                }
                                dhtSlices.Forward(t, startt);
                                dhtSlices.Forward(t, startt + slices);
                                for (int s = 0; s < slices; s++)
                                {
                                    a[s][r][0] = t[startt + s];
                                    a[s][r][1] = t[startt + slices + s];
                                }
                            }
                        }
                    }
                    else
                    {
                        if (columns > 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                for (int c = 0; c < columns; c += 4)
                                {
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx2 = startt + slices + s;
                                        t[startt + s] = a[s][r][c];
                                        t[idx2] = a[s][r][c + 1];
                                        t[idx2 + slices] = a[s][r][c + 2];
                                        t[idx2 + 2 * slices] = a[s][r][c + 3];
                                    }
                                    dhtSlices.Inverse(t, startt, isScale);
                                    dhtSlices.Inverse(t, startt + slices, isScale);
                                    dhtSlices.Inverse(t, startt + 2 * slices, isScale);
                                    dhtSlices.Inverse(t, startt + 3 * slices, isScale);
                                    for (int s = 0; s < slices; s++)
                                    {
                                        idx2 = startt + slices + s;
                                        a[s][r][c] = t[startt + s];
                                        a[s][r][c + 1] = t[idx2];
                                        a[s][r][c + 2] = t[idx2 + slices];
                                        a[s][r][c + 3] = t[idx2 + 2 * slices];
                                    }
                                }
                            }
                        }
                        else if (columns == 2)
                        {
                            for (int r = n0; r < rows; r += nthreads)
                            {
                                for (int s = 0; s < slices; s++)
                                {
                                    t[startt + s] = a[s][r][0];
                                    t[startt + slices + s] = a[s][r][1];
                                }
                                dhtSlices.Inverse(t, startt, isScale);
                                dhtSlices.Inverse(t, startt + slices, isScale);

                                for (int s = 0; s < slices; s++)
                                {
                                    a[s][r][0] = t[startt + s];
                                    a[s][r][1] = t[startt + slices + s];
                                }
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

        private void yTransform(double[] a)
        {
            double A, B, C, D, E, F, G, H;
            int cC, rC, sC;
            int idx1, idx2, idx3, idx4, idx5, idx6, idx7, idx8, idx9, idx10, idx11, idx12;
            for (int s = 0; s <= slices / 2; s++)
            {
                sC = (slices - s) % slices;
                idx9 = s * sliceStride;
                idx10 = sC * sliceStride;
                for (int r = 0; r <= rows / 2; r++)
                {
                    rC = (rows - r) % rows;
                    idx11 = r * rowStride;
                    idx12 = rC * rowStride;
                    for (int c = 0; c <= columns / 2; c++)
                    {
                        cC = (columns - c) % columns;
                        idx1 = idx9 + idx12 + c;
                        idx2 = idx9 + idx11 + cC;
                        idx3 = idx10 + idx11 + c;
                        idx4 = idx10 + idx12 + cC;
                        idx5 = idx10 + idx12 + c;
                        idx6 = idx10 + idx11 + cC;
                        idx7 = idx9 + idx11 + c;
                        idx8 = idx9 + idx12 + cC;
                        A = a[idx1];
                        B = a[idx2];
                        C = a[idx3];
                        D = a[idx4];
                        E = a[idx5];
                        F = a[idx6];
                        G = a[idx7];
                        H = a[idx8];
                        a[idx7] = (A + B + C - D) / 2;
                        a[idx3] = (E + F + G - H) / 2;
                        a[idx1] = (G + H + E - F) / 2;
                        a[idx5] = (C + D + A - B) / 2;
                        a[idx2] = (H + G + F - E) / 2;
                        a[idx6] = (D + C + B - A) / 2;
                        a[idx8] = (B + A + D - C) / 2;
                        a[idx4] = (F + E + H - G) / 2;
                    }
                }
            }
        }

        private void yTransform(double[][][] a)
        {
            double A, B, C, D, E, F, G, H;
            int cC, rC, sC;
            for (int s = 0; s <= slices / 2; s++)
            {
                sC = (slices - s) % slices;
                for (int r = 0; r <= rows / 2; r++)
                {
                    rC = (rows - r) % rows;
                    for (int c = 0; c <= columns / 2; c++)
                    {
                        cC = (columns - c) % columns;
                        A = a[s][rC][c];
                        B = a[s][r][cC];
                        C = a[sC][r][c];
                        D = a[sC][rC][cC];
                        E = a[sC][rC][c];
                        F = a[sC][r][cC];
                        G = a[s][r][c];
                        H = a[s][rC][cC];
                        a[s][r][c] = (A + B + C - D) / 2;
                        a[sC][r][c] = (E + F + G - H) / 2;
                        a[s][rC][c] = (G + H + E - F) / 2;
                        a[sC][rC][c] = (C + D + A - B) / 2;
                        a[s][r][cC] = (H + G + F - E) / 2;
                        a[sC][r][cC] = (D + C + B - A) / 2;
                        a[s][rC][cC] = (B + A + D - C) / 2;
                        a[sC][rC][cC] = (F + E + H - G) / 2;
                    }
                }
            }
        }
    }
}
