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
    /// Computes 2D Discrete Hartley Transform (DHT) of real, double precision data.
    /// The sizes of both dimensions can be arbitrary numbersd This is a parallel
    /// implementation optimized for SMP systems.<br>
    /// <br>
    /// Part of code is derived from General Purpose FFT Package written by Takuya Ooura
    /// (http://www.kurims.kyoto-u.ac.jp/~ooura/fft.html)
    /// 
    /// @author Piotr Wendykier (piotr.wendykier@gmail.com)
    /// 
    /// </summary>
    public class DoubleDHT_2D
    {

        private int rows;

        private int columns;

        private double[] t;

        private DoubleDHT_1D dhtColumns, dhtRows;

        private int oldNthreads;

        private int nt;

        private Boolean isPowerOfTwo = false;

        private Boolean useThreads = false;

        /// <summary>
        /// Creates new instance of DoubleDHT_2D.
        /// 
        /// </summary>
        /// <param name="rows"></param>
        ///            number of rows
        /// <param name="column"></param>
        ///            number of columns
        public DoubleDHT_2D(int rows, int column)
        {
            if (rows <= 1 || column <= 1)
            {
                throw new ArgumentException("rows and columns must be greater than 1");
            }
            this.rows = rows;
            this.columns = column;
            if (rows * column >= TransformCore.THREADS_BEGIN_N_2D)
            {
                this.useThreads = true;
            }
            if (rows.IsPowerOf2() && column.IsPowerOf2())
            {
                isPowerOfTwo = true;
                oldNthreads = Process.GetCurrentProcess().Threads.Count;
                nt = 4 * oldNthreads * rows;
                if (column == 2 * oldNthreads)
                {
                    nt >>= 1;
                }
                else if (column < 2 * oldNthreads)
                {
                    nt >>= 2;
                }
                t = new double[nt];
            }
            dhtColumns = new DoubleDHT_1D(column);
            if (column == rows)
            {
                dhtRows = dhtColumns;
            }
            else
            {
                dhtRows = new DoubleDHT_1D(rows);
            }
        }

        /// <summary>
        /// Computes 2D real, forward DHT leaving the result in <code>a</code>d The
        /// data is stored in 1D array in row-major order.
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
                    nt = 4 * nthreads * rows;
                    if (columns == 2 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 2 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new double[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt2d_subth(-1, a, true);
                    ddxt2d0_subth(-1, a, true);
                }
                else
                {
                    ddxt2d_sub(-1, a, true);
                    for (int i = 0; i < rows; i++)
                    {
                        dhtColumns.Forward(a, i * columns);
                    }
                }
                yTransform(a);
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
                                dhtColumns.Forward(a, i * columns);
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
                            double[] temp = new double[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r * columns + c];
                                }
                                dhtRows.Forward(temp);
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
                        dhtColumns.Forward(a, i * columns);
                    }
                    double[] temp = new double[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r * columns + c];
                        }
                        dhtRows.Forward(temp);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r * columns + c] = temp[r];
                        }
                    }
                }
                yTransform(a);
            }
        }

        /// <summary>
        /// Computes 2D real, forward DHT leaving the result in <code>a</code>d The
        /// data is stored in 2D array.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        public void Forward(double[][] a)
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
                    t = new double[nt];
                    oldNthreads = nthreads;
                }
                if ((nthreads > 1) && useThreads)
                {
                    ddxt2d_subth(-1, a, true);
                    ddxt2d0_subth(-1, a, true);
                }
                else
                {
                    ddxt2d_sub(-1, a, true);
                    for (int i = 0; i < rows; i++)
                    {
                        dhtColumns.Forward(a[i]);
                    }
                }
                y_transform(a);
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
                                dhtColumns.Forward(a[i]);
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
                            double[] temp = new double[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r][c];
                                }
                                dhtRows.Forward(temp);
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
                        dhtColumns.Forward(a[i]);
                    }
                    double[] temp = new double[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r][c];
                        }
                        dhtRows.Forward(temp);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r][c] = temp[r];
                        }
                    }
                }
                y_transform(a);
            }
        }

        /// <summary>
        /// Computes 2D real, inverse DHT leaving the result in <code>a</code>d The
        /// data is stored in 1D array in row-major order.
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
                    nt = 4 * nthreads * rows;
                    if (columns == 2 * nthreads)
                    {
                        nt >>= 1;
                    }
                    else if (columns < 2 * nthreads)
                    {
                        nt >>= 2;
                    }
                    t = new double[nt];
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
                        dhtColumns.Inverse(a, i * columns, isScale);
                    }
                }
                yTransform(a);
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
                                dhtColumns.Inverse(a, i * columns, isScale);
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
                            double[] temp = new double[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r * columns + c];
                                }
                                dhtRows.Inverse(temp, isScale);
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
                        dhtColumns.Inverse(a, i * columns, isScale);
                    }
                    double[] temp = new double[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r * columns + c];
                        }
                        dhtRows.Inverse(temp, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r * columns + c] = temp[r];
                        }
                    }
                }
                yTransform(a);
            }
        }

        /// <summary>
        /// Computes 2D real, inverse DHT leaving the result in <code>a</code>d The
        /// data is stored in 2D array.
        /// 
        /// </summary>
        /// <param name="a"></param>
        ///            data to transform
        /// <param name="isScale"></param>
        ///            if true then scaling is performed
        public void Inverse(double[][] a, Boolean isScale)
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
                    t = new double[nt];
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
                        dhtColumns.Inverse(a[i], isScale);
                    }
                }
                y_transform(a);
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
                                dhtColumns.Inverse(a[i], isScale);
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
                            double[] temp = new double[rows];
                            for (int c = firstColumn; c < lastColumn; c++)
                            {
                                for (int r = 0; r < rows; r++)
                                {
                                    temp[r] = a[r][c];
                                }
                                dhtRows.Inverse(temp, isScale);
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
                        dhtColumns.Inverse(a[i], isScale);
                    }
                    double[] temp = new double[rows];
                    for (int c = 0; c < columns; c++)
                    {
                        for (int r = 0; r < rows; r++)
                        {
                            temp[r] = a[r][c];
                        }
                        dhtRows.Inverse(temp, isScale);
                        for (int r = 0; r < rows; r++)
                        {
                            a[r][c] = temp[r];
                        }
                    }
                }
                y_transform(a);
            }
        }

        private void ddxt2d_subth(int isgn, double[] a, Boolean isScale)
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
            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
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
                                dhtRows.Forward(t, startt);
                                dhtRows.Forward(t, startt + rows);
                                dhtRows.Forward(t, startt + 2 * rows);
                                dhtRows.Forward(t, startt + 3 * rows);
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
                                dhtRows.Inverse(t, startt, isScale);
                                dhtRows.Inverse(t, startt + rows, isScale);
                                dhtRows.Inverse(t, startt + 2 * rows, isScale);
                                dhtRows.Inverse(t, startt + 3 * rows, isScale);
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
                            dhtRows.Forward(t, startt);
                            dhtRows.Forward(t, startt + rows);
                        }
                        else
                        {
                            dhtRows.Inverse(t, startt, isScale);
                            dhtRows.Inverse(t, startt + rows, isScale);
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
                            dhtRows.Forward(t, startt);
                        }
                        else
                        {
                            dhtRows.Inverse(t, startt, isScale);
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

        private void ddxt2d_subth(int isgn, double[][] a, Boolean isScale)
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
            Task[] taskArray = new Task[nthreads];

            for (int i = 0; i < nthreads; i++)
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
                                dhtRows.Forward(t, startt);
                                dhtRows.Forward(t, startt + rows);
                                dhtRows.Forward(t, startt + 2 * rows);
                                dhtRows.Forward(t, startt + 3 * rows);
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
                                dhtRows.Inverse(t, startt, isScale);
                                dhtRows.Inverse(t, startt + rows, isScale);
                                dhtRows.Inverse(t, startt + 2 * rows, isScale);
                                dhtRows.Inverse(t, startt + 3 * rows, isScale);
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
                            dhtRows.Forward(t, startt);
                            dhtRows.Forward(t, startt + rows);
                        }
                        else
                        {
                            dhtRows.Inverse(t, startt, isScale);
                            dhtRows.Inverse(t, startt + rows, isScale);
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
                            dhtRows.Forward(t, startt);
                        }
                        else
                        {
                            dhtRows.Inverse(t, startt, isScale);
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

        private void ddxt2d0_subth(int isgn, double[] a, Boolean isScale)
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
                            dhtColumns.Forward(a, r * columns);
                        }
                    }
                    else
                    {
                        for (int r = n0; r < rows; r += nthreads)
                        {
                            dhtColumns.Inverse(a, r * columns, isScale);
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

        private void ddxt2d0_subth(int isgn, double[][] a, Boolean isScale)
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
                            dhtColumns.Forward(a[r]);
                        }
                    }
                    else
                    {
                        for (int r = n0; r < rows; r += nthreads)
                        {
                            dhtColumns.Inverse(a[r], isScale);
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

        private void ddxt2d_sub(int isgn, double[] a, Boolean isScale)
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
                        dhtRows.Forward(t, 0);
                        dhtRows.Forward(t, rows);
                        dhtRows.Forward(t, 2 * rows);
                        dhtRows.Forward(t, 3 * rows);
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
                        dhtRows.Inverse(t, 0, isScale);
                        dhtRows.Inverse(t, rows, isScale);
                        dhtRows.Inverse(t, 2 * rows, isScale);
                        dhtRows.Inverse(t, 3 * rows, isScale);
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
                    dhtRows.Forward(t, 0);
                    dhtRows.Forward(t, rows);
                }
                else
                {
                    dhtRows.Inverse(t, 0, isScale);
                    dhtRows.Inverse(t, rows, isScale);
                }
                for (int r = 0; r < rows; r++)
                {
                    idx1 = r * columns;
                    a[idx1] = t[r];
                    a[idx1 + 1] = t[rows + r];
                }
            }
        }

        private void ddxt2d_sub(int isgn, double[][] a, Boolean isScale)
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
                        dhtRows.Forward(t, 0);
                        dhtRows.Forward(t, rows);
                        dhtRows.Forward(t, 2 * rows);
                        dhtRows.Forward(t, 3 * rows);
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
                        dhtRows.Inverse(t, 0, isScale);
                        dhtRows.Inverse(t, rows, isScale);
                        dhtRows.Inverse(t, 2 * rows, isScale);
                        dhtRows.Inverse(t, 3 * rows, isScale);
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
                    dhtRows.Forward(t, 0);
                    dhtRows.Forward(t, rows);
                }
                else
                {
                    dhtRows.Inverse(t, 0, isScale);
                    dhtRows.Inverse(t, rows, isScale);
                }
                for (int r = 0; r < rows; r++)
                {
                    a[r][0] = t[r];
                    a[r][1] = t[rows + r];
                }
            }
        }

        private void yTransform(double[] a)
        {
            int mRow, mCol, idx1, idx2;
            double A, B, C, D, E;
            for (int r = 0; r <= rows / 2; r++)
            {
                mRow = (rows - r) % rows;
                idx1 = r * columns;
                idx2 = mRow * columns;
                for (int c = 0; c <= columns / 2; c++)
                {
                    mCol = (columns - c) % columns;
                    A = a[idx1 + c];
                    B = a[idx2 + c];
                    C = a[idx1 + mCol];
                    D = a[idx2 + mCol];
                    E = ((A + D) - (B + C)) / 2;
                    a[idx1 + c] = A - E;
                    a[idx2 + c] = B + E;
                    a[idx1 + mCol] = C + E;
                    a[idx2 + mCol] = D - E;
                }
            }
        }

        private void y_transform(double[][] a)
        {
            int mRow, mCol;
            double A, B, C, D, E;
            for (int r = 0; r <= rows / 2; r++)
            {
                mRow = (rows - r) % rows;
                for (int c = 0; c <= columns / 2; c++)
                {
                    mCol = (columns - c) % columns;
                    A = a[r][c];
                    B = a[mRow][c];
                    C = a[r][mCol];
                    D = a[mRow][mCol];
                    E = ((A + D) - (B + C)) / 2;
                    a[r][c] = A - E;
                    a[mRow][c] = B + E;
                    a[r][mCol] = C + E;
                    a[mRow][mCol] = D - E;
                }
            }
        }
    }
}
