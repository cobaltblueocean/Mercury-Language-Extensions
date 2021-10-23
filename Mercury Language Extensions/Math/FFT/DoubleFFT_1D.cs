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

/// **** BEGIN LICENSE BLOCK ****
/// JTransforms
/// Copyright (c) 2007 onward, Piotr Wendykier
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without
/// modification, are permitted provided that the following conditions are met:
/// 
/// 1. Redistributions of source code must retain the above copyright notice, this
///    list of conditions and the following disclaimer. 
/// 2. Redistributions in binary form must reproduce the above copyright notice,
///    this list of conditions and the following disclaimer in the documentation
///    and/or other materials provided with the distribution.
///
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
/// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
/// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
/// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
/// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
/// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
/// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
/// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
/// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
/// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
///
/// **** END LICENSE BLOCK ****////
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Log;

namespace Mercury.Language.Math
{
    public sealed partial class DoubleFFT_1D
    {
        #region Local Variables
        private enum Plans
        {

            SPLIT_RADIX, MIXED_RADIX, BLUESTEIN
        }

        private static long THREADS_BEGIN_N_1D_FFT_2THREADS = 8192;

        private static long THREADS_BEGIN_N_1D_FFT_4THREADS = 65536;

        private int n;

        private long nl;

        private int nBluestein;

        private long nBluesteinl;

        private int[] ip;

        private long[] ipl;

        private double[] w;

        private double[] wl;

        private int nw;

        private long nwl;

        private int nc;

        private long ncl;

        private double[] wtable;

        private double[] wtablel;

        private double[] wtable_r;

        private double[] wtable_rl;

        private double[] bk1;

        private double[] bk1l;

        private double[] bk2;

        private double[] bk2l;

        private Plans plan;

        private Boolean useLargeArrays;

        private static int[] factors = { 4, 2, 3, 5 };

        private static double PI = 3.14159265358979311599796346854418516;

        private static double TWO_PI = 6.28318530717958623199592693708837032;

        #endregion

        public DoubleFFT_1D(long n)
        { }


        public DoubleFFT_1D(long n, Boolean isUseLargeArrays)
        {
            if (n < 1)
            {
                throw new ArgumentException("n must be greater than 0");
            }
            this.useLargeArrays = (isUseLargeArrays || 2 * n > LargeArray<Int64>.MaxSizeOf32bitArray);
            this.n = (int)n;
            this.nl = n;

            if (this.useLargeArrays == false)
            {
                if (!n.IsPowerOf2())
                {
                    if (n.GetRemainder(factors) >= 211)
                    {
                        plan = Plans.BLUESTEIN;
                        nBluestein = (this.n * 2 - 1).NextPowerOf2();
                        bk1 = new double[2 * nBluestein];
                        bk2 = new double[2 * nBluestein];
                        this.ip = new int[2 + (int)System.Math.Ceiling(2 + (double)(1 << (int)(System.Math.Log(nBluestein + 0.5) / System.Math.Log(2)) / 2))];
                        this.w = new double[nBluestein];
                        int twon = 2 * nBluestein;
                        nw = twon >> 2;
                        nw.MakeWT(ref ip, ref w);
                        nc = nBluestein >> 2;
                        nc.MakeCT(ref w, nw, ref ip);
                        bluesteini();
                    }
                    else
                    {
                        plan = Plans.MIXED_RADIX;
                        wtable = new double[4 * this.n + 15];
                        wtable_r = new double[2 * this.n + 15];
                        cffti();
                        rffti();
                    }
                }
                else
                {
                    plan = Plans.SPLIT_RADIX;
                    this.ip = new int[2 + (int)System.Math.Ceiling(2 + (double)(1 << (int)(System.Math.Log(n + 0.5) / System.Math.Log(2)) / 2))];
                    this.w = new double[this.n];
                    int twon = 2 * this.n;
                    nw = twon >> 2;
                    nw.MakeWT(ref ip, ref w);
                    nc = this.n >> 2;
                    nc.MakeCT(ref w, nw, ref ip);
                }
            }
            else if (!nl.IsPowerOf2())
            {
                if (nl.GetRemainder(factors) >= 211)
                {
                    plan = Plans.BLUESTEIN;
                    nBluesteinl = (nl * 2 - 1).NextPowerOf2();
                    bk1l = new double[2 * nBluesteinl];
                    bk2l = new double[2 * nBluesteinl];
                    this.ipl = new long[2 + (long)System.Math.Ceiling(2 + (double)(1 << (int)(System.Math.Log(nBluesteinl + 0.5) / System.Math.Log(2)) / 2))];
                    this.wl = new double[nBluesteinl];
                    long twon = 2 * nBluesteinl;
                    nwl = twon >> 2;
                    nwl.MakeWT(ref ipl, ref wl);
                    ncl = nBluesteinl >> 2;
                    ncl.MakeCT(ref wl, nwl, ref ipl);
                    bluesteinil();
                }
                else
                {
                    plan = Plans.MIXED_RADIX;
                    wtablel = new double[4 * nl + 15];
                    wtable_rl = new double[2 * nl + 15];
                    cfftil();
                    rfftil();
                }
            }
            else
            {
                plan = Plans.SPLIT_RADIX;
                this.ipl = new long[2 + (long)System.Math.Ceiling(2 + (double)(1 << (int)(System.Math.Log(nl + 0.5) / System.Math.Log(2)) / 2))];
                this.wl = new double[nl];
                long twon = 2 * nl;
                nwl = twon >> 2;
                nwl.MakeWT(ref ipl, ref wl);
                ncl = nl >> 2;
                ncl.MakeCT(ref wl, nwl, ref ipl);
            }
        }





        /* -------- initializing routines -------- */

        /*---------------------------------------------------------
         cffti: initialization of Complex FFT
         --------------------------------------------------------*/
        void cffti(int n, int offw)
        {
            if (n == 1)
            {
                return;
            }

            int twon = 2 * n;
            int fourn = 4 * n;
            double argh;
            int idot, ntry = 0, i, j;
            double argld;
            int i1, k1, l1, l2, ib;
            double fi;
            int ld, ii, nf, ipll, nll, nq, nr;
            double arg;
            int ido, ipm;

            nll = n;
            nf = 0;
            j = 0;

            factorize_loop:
            while (true)
            {
                j++;
                if (j <= 4)
                {
                    ntry = factors[j - 1];
                }
                else
                {
                    ntry += 2;
                }
                do
                {
                    nq = nll / ntry;
                    nr = nll - ntry * nq;
                    if (nr != 0)
                    {
                        goto factorize_loop;
                    }
                    nf++;
                    wtable[offw + nf + 1 + fourn] = ntry;
                    nll = nq;
                    if (ntry == 2 && nf != 1)
                    {
                        for (i = 2; i <= nf; i++)
                        {
                            ib = nf - i + 2;
                            int idx = ib + fourn;
                            wtable[offw + idx + 1] = wtable[offw + idx];
                        }
                        wtable[offw + 2 + fourn] = 2;
                    }
                } while (nll != 1);
                break;
            }
            wtable[offw + fourn] = n;
            wtable[offw + 1 + fourn] = nf;
            argh = TWO_PI / (double)n;
            i = 1;
            l1 = 1;
            for (k1 = 1; k1 <= nf; k1++)
            {
                ipll = (int)wtable[offw + k1 + 1 + fourn];
                ld = 0;
                l2 = l1 * ipll;
                ido = n / l2;
                idot = ido + ido + 2;
                ipm = ipll - 1;
                for (j = 1; j <= ipm; j++)
                {
                    i1 = i;
                    wtable[offw + i - 1 + twon] = 1;
                    wtable[offw + i + twon] = 0;
                    ld += l1;
                    fi = 0;
                    argld = ld * argh;
                    for (ii = 4; ii <= idot; ii += 2)
                    {
                        i += 2;
                        fi += 1;
                        arg = fi * argld;
                        int idx = i + twon;
                        wtable[offw + idx - 1] = System.Math.Cos(arg);
                        wtable[offw + idx] = System.Math.Sin(arg);
                    }
                    if (ipll > 5)
                    {
                        int idx1 = i1 + twon;
                        int idx2 = i + twon;
                        wtable[offw + idx1 - 1] = wtable[offw + idx2 - 1];
                        wtable[offw + idx1] = wtable[offw + idx2];
                    }
                }
                l1 = l2;
            }

        }

        void cffti()
        {
            if (n == 1)
            {
                return;
            }

            int twon = 2 * n;
            int fourn = 4 * n;
            double argh;
            int idot, ntry = 0, i, j;
            double argld;
            int i1, k1, l1, l2, ib;
            double fi;
            int ld, ii, nf, ipll, nll, nq, nr;
            double arg;
            int ido, ipm;

            nll = n;
            nf = 0;
            j = 0;

            factorize_loop:
            while (true)
            {
                j++;
                if (j <= 4)
                {
                    ntry = factors[j - 1];
                }
                else
                {
                    ntry += 2;
                }
                do
                {
                    nq = nll / ntry;
                    nr = nll - ntry * nq;
                    if (nr != 0)
                    {
                        goto factorize_loop;
                    }
                    nf++;
                    wtable[nf + 1 + fourn] = ntry;
                    nll = nq;
                    if (ntry == 2 && nf != 1)
                    {
                        for (i = 2; i <= nf; i++)
                        {
                            ib = nf - i + 2;
                            int idx = ib + fourn;
                            wtable[idx + 1] = wtable[idx];
                        }
                        wtable[2 + fourn] = 2;
                    }
                } while (nll != 1);
                break;
            }
            wtable[fourn] = n;
            wtable[1 + fourn] = nf;
            argh = TWO_PI / (double)n;
            i = 1;
            l1 = 1;
            for (k1 = 1; k1 <= nf; k1++)
            {
                ipll = (int)wtable[k1 + 1 + fourn];
                ld = 0;
                l2 = l1 * ipll;
                ido = n / l2;
                idot = ido + ido + 2;
                ipm = ipll - 1;
                for (j = 1; j <= ipm; j++)
                {
                    i1 = i;
                    wtable[i - 1 + twon] = 1;
                    wtable[i + twon] = 0;
                    ld += l1;
                    fi = 0;
                    argld = ld * argh;
                    for (ii = 4; ii <= idot; ii += 2)
                    {
                        i += 2;
                        fi += 1;
                        arg = fi * argld;
                        int idx = i + twon;
                        wtable[idx - 1] = System.Math.Cos(arg);
                        wtable[idx] = System.Math.Sin(arg);
                    }
                    if (ipll > 5)
                    {
                        int idx1 = i1 + twon;
                        int idx2 = i + twon;
                        wtable[idx1 - 1] = wtable[idx2 - 1];
                        wtable[idx1] = wtable[idx2];
                    }
                }
                l1 = l2;
            }

        }

        void cfftil()
        {
            if (nl == 1)
            {
                return;
            }

            long twon = 2 * nl;
            long fourn = 4 * nl;
            double argh;
            long idot, ntry = 0, i, j;
            double argld;
            long i1, k1, l1, l2, ib;
            double fi;
            long ld, ii, nf, ipll, nl2, nq, nr;
            double arg;
            long ido, ipm;

            nl2 = nl;
            nf = 0;
            j = 0;

            factorize_loop:
            while (true)
            {
                j++;
                if (j <= 4)
                {
                    ntry = factors[(int)(j - 1)];
                }
                else
                {
                    ntry += 2;
                }
                do
                {
                    nq = nl2 / ntry;
                    nr = nl2 - ntry * nq;
                    if (nr != 0)
                    {
                        goto factorize_loop;
                    }
                    nf++;
                    wtablel[nf + 1 + fourn] = ntry;
                    nl2 = nq;
                    if (ntry == 2 && nf != 1)
                    {
                        for (i = 2; i <= nf; i++)
                        {
                            ib = nf - i + 2;
                            long idx = ib + fourn;
                            wtablel[idx + 1] = wtablel[idx];
                        }
                        wtablel[2 + fourn] = 2;
                    }
                } while (nl2 != 1);
                break;
            }
            wtablel[fourn] = nl;
            wtablel[1 + fourn] = nf;
            argh = TWO_PI / (double)nl;
            i = 1;
            l1 = 1;
            for (k1 = 1; k1 <= nf; k1++)
            {
                ipll = (long)wtablel[k1 + 1 + fourn];
                ld = 0;
                l2 = l1 * ipll;
                ido = nl / l2;
                idot = ido + ido + 2;
                ipm = ipll - 1;
                for (j = 1; j <= ipm; j++)
                {
                    i1 = i;
                    wtablel[i - 1 + twon] = 1;
                    wtablel[i + twon] = 0;
                    ld += l1;
                    fi = 0;
                    argld = ld * argh;
                    for (ii = 4; ii <= idot; ii += 2)
                    {
                        i += 2;
                        fi += 1;
                        arg = fi * argld;
                        long idx = i + twon;
                        wtablel[idx - 1] = System.Math.Cos(arg);
                        wtablel[idx] = System.Math.Sin(arg);
                    }
                    if (ipll > 5)
                    {
                        long idx1 = i1 + twon;
                        long idx2 = i + twon;
                        wtablel[idx1 - 1] = wtablel[idx2 - 1];
                        wtablel[idx1] = wtablel[idx2];
                    }
                }
                l1 = l2;
            }

        }

        void rffti()
        {

            if (n == 1)
            {
                return;
            }
            int twon = 2 * n;
            double argh;
            int ntry = 0, i, j;
            double argld;
            int k1, l1, l2, ib;
            double fi;
            int ld, ii, nf, ipll, nll, is1, nq, nr;
            double arg;
            int ido, ipm;
            int nfm1;

            nll = n;
            nf = 0;
            j = 0;

            factorize_loop:
            while (true)
            {
                ++j;
                if (j <= 4)
                {
                    ntry = factors[j - 1];
                }
                else
                {
                    ntry += 2;
                }
                do
                {
                    nq = nll / ntry;
                    nr = nll - ntry * nq;
                    if (nr != 0)
                    {
                        goto factorize_loop;
                    }
                    ++nf;
                    wtable_r[nf + 1 + twon] = ntry;

                    nll = nq;
                    if (ntry == 2 && nf != 1)
                    {
                        for (i = 2; i <= nf; i++)
                        {
                            ib = nf - i + 2;
                            int idx = ib + twon;
                            wtable_r[idx + 1] = wtable_r[idx];
                        }
                        wtable_r[2 + twon] = 2;
                    }
                } while (nll != 1);
                break;
            }
            wtable_r[twon] = n;
            wtable_r[1 + twon] = nf;
            argh = TWO_PI / (double)(n);
            is1 = 0;
            nfm1 = nf - 1;
            l1 = 1;
            if (nfm1 == 0)
            {
                return;
            }
            for (k1 = 1; k1 <= nfm1; k1++)
            {
                ipll = (int)wtable_r[k1 + 1 + twon];
                ld = 0;
                l2 = l1 * ipll;
                ido = n / l2;
                ipm = ipll - 1;
                for (j = 1; j <= ipm; ++j)
                {
                    ld += l1;
                    i = is1;
                    argld = (double)ld * argh;

                    fi = 0;
                    for (ii = 3; ii <= ido; ii += 2)
                    {
                        i += 2;
                        fi += 1;
                        arg = fi * argld;
                        int idx = i + n;
                        wtable_r[idx - 2] = System.Math.Cos(arg);
                        wtable_r[idx - 1] = System.Math.Sin(arg);
                    }
                    is1 += ido;
                }
                l1 = l2;
            }
        }

        void rfftil()
        {

            if (nl == 1)
            {
                return;
            }
            long twon = 2 * nl;
            double argh;
            long ntry = 0, i, j;
            double argld;
            long k1, l1, l2, ib;
            double fi;
            long ld, ii, nf, ipll, nl2, is1, nq, nr;
            double arg;
            long ido, ipm;
            long nfm1;

            nl2 = nl;
            nf = 0;
            j = 0;

            factorize_loop:
            while (true)
            {
                ++j;
                if (j <= 4)
                {
                    ntry = factors[(int)(j - 1)];
                }
                else
                {
                    ntry += 2;
                }
                do
                {
                    nq = nl2 / ntry;
                    nr = nl2 - ntry * nq;
                    if (nr != 0)
                    {
                        goto factorize_loop;
                    }
                    ++nf;
                    wtable_rl[nf + 1 + twon] = ntry;

                    nl2 = nq;
                    if (ntry == 2 && nf != 1)
                    {
                        for (i = 2; i <= nf; i++)
                        {
                            ib = nf - i + 2;
                            long idx = ib + twon;
                            wtable_rl[idx + 1] = wtable_rl[idx];
                        }
                        wtable_rl[2 + twon] = 2;
                    }
                } while (nl2 != 1);
                break;
            }
            wtable_rl[twon] = nl;
            wtable_rl[1 + twon] = nf;
            argh = TWO_PI / (double)(nl);
            is1 = 0;
            nfm1 = nf - 1;
            l1 = 1;
            if (nfm1 == 0)
            {
                return;
            }
            for (k1 = 1; k1 <= nfm1; k1++)
            {
                ipll = (long)wtable_rl[k1 + 1 + twon];
                ld = 0;
                l2 = l1 * ipll;
                ido = nl / l2;
                ipm = ipll - 1;
                for (j = 1; j <= ipm; ++j)
                {
                    ld += l1;
                    i = is1;
                    argld = (double)ld * argh;

                    fi = 0;
                    for (ii = 3; ii <= ido; ii += 2)
                    {
                        i += 2;
                        fi += 1;
                        arg = fi * argld;
                        long idx = i + nl;
                        wtable_rl[idx - 2] = System.Math.Cos(arg);
                        wtable_rl[idx - 1] = System.Math.Sin(arg);
                    }
                    is1 += ido;
                }
                l1 = l2;
            }
        }

        private void bluesteini()
        {
            int k = 0;
            double arg;
            double pi_n = PI / n;
            bk1[0] = 1;
            bk1[1] = 0;
            for (int i = 1; i < n; i++)
            {
                k += 2 * i - 1;
                if (k >= 2 * n)
                {
                    k -= 2 * n;
                }
                arg = pi_n * k;
                bk1[2 * i] = System.Math.Cos(arg);
                bk1[2 * i + 1] = System.Math.Sin(arg);
            }
            double scale = 1.0 / nBluestein;
            bk2[0] = bk1[0] * scale;
            bk2[1] = bk1[1] * scale;
            for (int i = 2; i < 2 * n; i += 2)
            {
                bk2[i] = bk1[i] * scale;
                bk2[i + 1] = bk1[i + 1] * scale;
                bk2[2 * nBluestein - i] = bk2[i];
                bk2[2 * nBluestein - i + 1] = bk2[i + 1];
            }
            (2 * nBluestein).cftbsub(ref bk2, 0, ref ip, nw, ref w);
        }

        private void bluesteinil()
        {
            long k = 0;
            double arg;
            double pi_n = PI / nl;
            bk1l[0] = 1;
            bk1l[1] = 0;
            for (int i = 1; i < nl; i++)
            {
                k += 2 * i - 1;
                if (k >= 2 * nl)
                {
                    k -= 2 * nl;
                }
                arg = pi_n * k;
                bk1l[2 * i] = System.Math.Cos(arg);
                bk1l[2 * i + 1] = System.Math.Sin(arg);
            }
            double scale = 1.0 / nBluesteinl;
            bk2l[0] = bk1l[0] * scale;
            bk2l[1] = bk1l[1] * scale;
            for (int i = 2; i < 2 * nl; i += 2)
            {
                bk2l[i] = bk1l[i] * scale;
                bk2l[i + 1] = bk1l[i + 1] * scale;
                bk2l[2 * nBluesteinl - i] = bk2l[i];
                bk2l[2 * nBluesteinl - i + 1] = bk2l[i + 1];
            }
            (2 * nBluesteinl).cftbsub(ref bk2l, 0, ref ipl, nwl, ref wl);
        }

        private void bluestein_complex(double[] a, int offa, int isign)
        {
            double[] ak = new double[2 * nBluestein];
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (n >= THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                int k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                int idx3 = offa + idx1;
                                int idx4 = offa + idx2;
                                ak[idx1] = a[idx3] * bk1[idx1] - a[idx4] * bk1[idx2];
                                ak[idx2] = a[idx3] * bk1[idx2] + a[idx4] * bk1[idx1];
                            }
                        }
                        else
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                int idx3 = offa + idx1;
                                int idx4 = offa + idx2;
                                ak[idx1] = a[idx3] * bk1[idx1] + a[idx4] * bk1[idx2];
                                ak[idx2] = -a[idx3] * bk1[idx2] + a[idx4] * bk1[idx1];
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

                    (2 * nBluestein).cftbsub(ref ak, 0, ref ip, nw, ref w);

                k = nBluestein / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? nBluestein : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                                ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                                ak[idx2] = im;
                            }
                        }
                        else
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                double im = ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                                ak[idx1] = ak[idx1] * bk2[idx1] - ak[idx2] * bk2[idx2];
                                ak[idx2] = im;
                            }
                        }
                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluestein).cftfsub(ref ak, 0, ref ip, nw, ref w);

                k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                int idx3 = offa + idx1;
                                int idx4 = offa + idx2;
                                a[idx3] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                                a[idx4] = bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                            }
                        }
                        else
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                int idx3 = offa + idx1;
                                int idx4 = offa + idx2;
                                a[idx3] = bk1[idx1] * ak[idx1] + bk1[idx2] * ak[idx2];
                                a[idx4] = -bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                            }
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                if (isign > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        int idx3 = offa + idx1;
                        int idx4 = offa + idx2;
                        ak[idx1] = a[idx3] * bk1[idx1] - a[idx4] * bk1[idx2];
                        ak[idx2] = a[idx3] * bk1[idx2] + a[idx4] * bk1[idx1];
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        int idx3 = offa + idx1;
                        int idx4 = offa + idx2;
                        ak[idx1] = a[idx3] * bk1[idx1] + a[idx4] * bk1[idx2];
                        ak[idx2] = -a[idx3] * bk1[idx2] + a[idx4] * bk1[idx1];
                    }
                }

              (2 * nBluestein).cftbsub(ref ak, 0, ref ip, nw, ref w);

                if (isign > 0)
                {
                    for (int i = 0; i < nBluestein; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                        ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                        ak[idx2] = im;
                    }
                }
                else
                {
                    for (int i = 0; i < nBluestein; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        double im = ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                        ak[idx1] = ak[idx1] * bk2[idx1] - ak[idx2] * bk2[idx2];
                        ak[idx2] = im;
                    }
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ip, nw, ref w);
                if (isign > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        int idx3 = offa + idx1;
                        int idx4 = offa + idx2;
                        a[idx3] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                        a[idx4] = bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        int idx3 = offa + idx1;
                        int idx4 = offa + idx2;
                        a[idx3] = bk1[idx1] * ak[idx1] + bk1[idx2] * ak[idx2];
                        a[idx4] = -bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                    }
                }
            }
        }

        private void bluestein_complex(DoubleLargeArray a, long offa, int isign)
        {
            DoubleLargeArray ak = new DoubleLargeArray(2 * nBluesteinl);
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (nl > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (nl > THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                long k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                long idx3 = offa + idx1;
                                long idx4 = offa + idx2;
                                ak[idx1] = a[idx3] * bk1l[idx1] - a[idx4] * bk1l[idx2];
                                ak[idx2] = a[idx3] * bk1l[idx2] + a[idx4] * bk1l[idx1];
                            }
                        }
                        else
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                long idx3 = offa + idx1;
                                long idx4 = offa + idx2;
                                ak[idx1] = a[idx3] * bk1l[idx1] + a[idx4] * bk1l[idx2];
                                ak[idx2] = -a[idx3] * bk1l[idx2] + a[idx4] * bk1l[idx1];
                            }
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nBluesteinl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nBluesteinl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                                ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                                ak[idx2] = im;
                            }
                        }
                        else
                        {
                            for (long l = firstIdx; l < lastIdx; l++)
                            {
                                long idx1 = 2 * l;
                                long idx2 = idx1 + 1;
                                double im = ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                                ak[idx1] = ak[idx1] * bk2l[idx1] - ak[idx2] * bk2l[idx2];
                                ak[idx2] = im;
                            }
                        }
                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                long idx3 = offa + idx1;
                                long idx4 = offa + idx2;
                                a[idx3] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                                a[idx4] = bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                            }
                        }
                        else
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                long idx3 = offa + idx1;
                                long idx4 = offa + idx2;
                                a[idx3] = bk1l[idx1] * ak[idx1] + bk1l[idx2] * ak[idx2];
                                a[idx4] = -bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                            }
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                if (isign > 0)
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        long idx3 = offa + idx1;
                        long idx4 = offa + idx2;
                        ak[idx1] = a[idx3] * bk1l[idx1] - a[idx4] * bk1l[idx2];
                        ak[idx2] = a[idx3] * bk1l[idx2] + a[idx4] * bk1l[idx1];
                    }
                }
                else
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        long idx3 = offa + idx1;
                        long idx4 = offa + idx2;
                        ak[idx1] = a[idx3] * bk1l[idx1] + a[idx4] * bk1l[idx2];
                        ak[idx2] = -a[idx3] * bk1l[idx2] + a[idx4] * bk1l[idx1];
                    }
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                if (isign > 0)
                {
                    for (long i = 0; i < nBluesteinl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                        ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                        ak[idx2] = im;
                    }
                }
                else
                {
                    for (long i = 0; i < nBluesteinl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        double im = ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                        ak[idx1] = ak[idx1] * bk2l[idx1] - ak[idx2] * bk2l[idx2];
                        ak[idx2] = im;
                    }
                }

              (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);
                if (isign > 0)
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        long idx3 = offa + idx1;
                        long idx4 = offa + idx2;
                        a[idx3] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                        a[idx4] = bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                    }
                }
                else
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        long idx3 = offa + idx1;
                        long idx4 = offa + idx2;
                        a[idx3] = bk1l[idx1] * ak[idx1] + bk1l[idx2] * ak[idx2];
                        a[idx4] = -bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                    }
                }
            }
        }

        private void bluestein_real_full(double[] a, int offa, int isign)
        {
            double[] ak = new double[2 * nBluestein];
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (n >= THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                int k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            if (isign > 0)
            {
                for (int j = firstIdx; j < lastIdx; j++)
                {
                    int idx1 = 2 * j;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + j;
                    ak[idx1] = a[idx3] * bk1[idx1];
                    ak[idx2] = a[idx3] * bk1[idx2];
                }
            }
            else
            {
                for (int j = firstIdx; j < lastIdx; j++)
                {
                    int idx1 = 2 * j;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + j;
                    ak[idx1] = a[idx3] * bk1[idx1];
                    ak[idx2] = -a[idx3] * bk1[idx2];
                }
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluestein).cftbsub(ref ak, 0, ref ip, nw, ref w);

                k = nBluestein / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? nBluestein : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                                ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                                ak[idx2] = im;
                            }
                        }
                        else
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                double im = ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                                ak[idx1] = ak[idx1] * bk2[idx1] - ak[idx2] * bk2[idx2];
                                ak[idx2] = im;
                            }
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftbsub(ref ak, 0, ref ip, nw, ref w);

                k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                a[offa + idx1] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                                a[offa + idx2] = bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                            }
                        }
                        else
                        {
                            for (int j = firstIdx; j < lastIdx; j++)
                            {
                                int idx1 = 2 * j;
                                int idx2 = idx1 + 1;
                                a[offa + idx1] = bk1[idx1] * ak[idx1] + bk1[idx2] * ak[idx2];
                                a[offa + idx2] = -bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                            }
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                if (isign > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        int idx3 = offa + i;
                        ak[idx1] = a[idx3] * bk1[idx1];
                        ak[idx2] = a[idx3] * bk1[idx2];
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        int idx3 = offa + i;
                        ak[idx1] = a[idx3] * bk1[idx1];
                        ak[idx2] = -a[idx3] * bk1[idx2];
                    }
                }

              (2 * nBluestein).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                if (isign > 0)
                {
                    for (int i = 0; i < nBluestein; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                        ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                        ak[idx2] = im;
                    }
                }
                else
                {
                    for (int i = 0; i < nBluestein; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        double im = ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                        ak[idx1] = ak[idx1] * bk2[idx1] - ak[idx2] * bk2[idx2];
                        ak[idx2] = im;
                    }
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ip, nw, ref w);

                if (isign > 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        a[offa + idx1] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                        a[offa + idx2] = bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        int idx1 = 2 * i;
                        int idx2 = idx1 + 1;
                        a[offa + idx1] = bk1[idx1] * ak[idx1] + bk1[idx2] * ak[idx2];
                        a[offa + idx2] = -bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                    }
                }
            }
        }

        private void bluestein_real_full(DoubleLargeArray a, long offa, long isign)
        {
            DoubleLargeArray ak = new DoubleLargeArray(2 * nBluesteinl);
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (nl > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (nl > THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                long k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            if (isign > 0)
            {
                for (long j = firstIdx; j < lastIdx; j++)
                {
                    long idx1 = 2 * j;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + j;
                    ak[idx1] = a[idx3] * bk1l[idx1];
                    ak[idx2] = a[idx3] * bk1l[idx2];
                }
            }
            else
            {
                for (long j = firstIdx; j < lastIdx; j++)
                {
                    long idx1 = 2 * j;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + j;
                    ak[idx1] = a[idx3] * bk1l[idx1];
                    ak[idx2] = -a[idx3] * bk1l[idx2];
                }
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nBluesteinl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nBluesteinl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                                ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                                ak[idx2] = im;
                            }
                        }
                        else
                        {
                            for (long j = firstIdx; j < lastIdx; j++)
                            {
                                long idx1 = 2 * j;
                                long idx2 = idx1 + 1;
                                double im = ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                                ak[idx1] = ak[idx1] * bk2l[idx1] - ak[idx2] * bk2l[idx2];
                                ak[idx2] = im;
                            }
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        if (isign > 0)
                        {
                            for (long l = firstIdx; l < lastIdx; l++)
                            {
                                long idx1 = 2 * l;
                                long idx2 = idx1 + 1;
                                a[offa + idx1] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                                a[offa + idx2] = bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                            }
                        }
                        else
                        {
                            for (long l = firstIdx; l < lastIdx; l++)
                            {
                                long idx1 = 2 * l;
                                long idx2 = idx1 + 1;
                                a[offa + idx1] = bk1l[idx1] * ak[idx1] + bk1l[idx2] * ak[idx2];
                                a[offa + idx2] = -bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                            }
                        }


                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
            else
            {
                if (isign > 0)
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        long idx3 = offa + i;
                        ak[idx1] = a[idx3] * bk1l[idx1];
                        ak[idx2] = a[idx3] * bk1l[idx2];
                    }
                }
                else
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        long idx3 = offa + i;
                        ak[idx1] = a[idx3] * bk1l[idx1];
                        ak[idx2] = -a[idx3] * bk1l[idx2];
                    }
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                if (isign > 0)
                {
                    for (long i = 0; i < nBluesteinl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                        ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                        ak[idx2] = im;
                    }
                }
                else
                {
                    for (long i = 0; i < nBluesteinl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        double im = ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                        ak[idx1] = ak[idx1] * bk2l[idx1] - ak[idx2] * bk2l[idx2];
                        ak[idx2] = im;
                    }
                }

              (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

                if (isign > 0)
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        a[offa + idx1] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                        a[offa + idx2] = bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                    }
                }
                else
                {
                    for (long i = 0; i < nl; i++)
                    {
                        long idx1 = 2 * i;
                        long idx2 = idx1 + 1;
                        a[offa + idx1] = bk1l[idx1] * ak[idx1] + bk1l[idx2] * ak[idx2];
                        a[offa + idx2] = -bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                    }
                }
            }
        }

        private void bluestein_real_forward(double[] a, int offa)
        {
            double[] ak = new double[2 * nBluestein];
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (n >= THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                int k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            for (int j = firstIdx; j < lastIdx; j++)
            {
                int idx1 = 2 * j;
                int idx2 = idx1 + 1;
                int idx3 = offa + j;
                ak[idx1] = a[idx3] * bk1[idx1];
                ak[idx2] = -a[idx3] * bk1[idx2];
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluestein).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nBluestein / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? nBluestein : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (int j = firstIdx; j < lastIdx; j++)
                        {
                            int idx1 = 2 * j;
                            int idx2 = idx1 + 1;
                            double im = ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                            ak[idx1] = ak[idx1] * bk2[idx1] - ak[idx2] * bk2[idx2];
                            ak[idx2] = im;
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {

                for (int i = 0; i < n; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + i;
                    ak[idx1] = a[idx3] * bk1[idx1];
                    ak[idx2] = -a[idx3] * bk1[idx2];
                }

              (2 * nBluestein).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                for (int i = 0; i < nBluestein; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    double im = ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                    ak[idx1] = ak[idx1] * bk2[idx1] - ak[idx2] * bk2[idx2];
                    ak[idx2] = im;
                }
            }

            (2 * nBluesteinl).cftbsub(ref ak, 0, ref ip, nw, ref w);

            if (n % 2 == 0)
            {
                a[offa] = bk1[0] * ak[0] + bk1[1] * ak[1];
                a[offa + 1] = bk1[n] * ak[n] + bk1[n + 1] * ak[n + 1];
                for (int i = 1; i < n / 2; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    a[offa + idx1] = bk1[idx1] * ak[idx1] + bk1[idx2] * ak[idx2];
                    a[offa + idx2] = -bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                }
            }
            else
            {
                a[offa] = bk1[0] * ak[0] + bk1[1] * ak[1];
                a[offa + 1] = -bk1[n] * ak[n - 1] + bk1[n - 1] * ak[n];
                for (int i = 1; i < (n - 1) / 2; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    a[offa + idx1] = bk1[idx1] * ak[idx1] + bk1[idx2] * ak[idx2];
                    a[offa + idx2] = -bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                }
                a[offa + n - 1] = bk1[n - 1] * ak[n - 1] + bk1[n] * ak[n];
            }

        }

        private void bluestein_real_forward(DoubleLargeArray a, long offa)
        {
            DoubleLargeArray ak = new DoubleLargeArray(2 * nBluesteinl);
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (nl > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (nl > THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                long k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            for (long l = firstIdx; l < lastIdx; l++)
            {
                long idx1 = 2 * l;
                long idx2 = idx1 + 1;
                long idx3 = offa + l;
                ak[idx1] = a[idx3] * bk1l[idx1];
                ak[idx2] = -a[idx3] * bk1l[idx2];
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nBluesteinl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nBluesteinl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (long l = firstIdx; l < lastIdx; l++)
                        {
                            long idx1 = 2 * l;
                            long idx2 = idx1 + 1;
                            double im = ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                            ak[idx1] = ak[idx1] * bk2l[idx1] - ak[idx2] * bk2l[idx2];
                            ak[idx2] = im;
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {

                for (long i = 0; i < nl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + i;
                    ak[idx1] = a[idx3] * bk1l[idx1];
                    ak[idx2] = -a[idx3] * bk1l[idx2];
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                for (long i = 0; i < nBluesteinl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    double im = ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                    ak[idx1] = ak[idx1] * bk2l[idx1] - ak[idx2] * bk2l[idx2];
                    ak[idx2] = im;
                }
            }

            (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

            if (nl % 2 == 0)
            {
                a[offa] = bk1l[0] * ak[0] + bk1l[1] * ak[1];
                a[offa + 1] = bk1l[nl] * ak[nl] + bk1l[nl + 1] * ak[nl + 1];
                for (long i = 1; i < nl / 2; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    a[offa + idx1] = bk1l[idx1] * ak[idx1] + bk1l[idx2] * ak[idx2];
                    a[offa + idx2] = -bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                }
            }
            else
            {
                a[offa] = bk1l[0] * ak[0] + bk1l[1] * ak[1];
                a[offa + 1] = -bk1l[nl] * ak[nl - 1] + bk1l[nl - 1] * ak[nl];
                for (long i = 1; i < (nl - 1) / 2; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    a[offa + idx1] = bk1l[idx1] * ak[idx1] + bk1l[idx2] * ak[idx2];
                    a[offa + idx2] = -bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                }
                a[offa + nl - 1] = bk1l[nl - 1] * ak[nl - 1] + bk1l[nl] * ak[nl];
            }

        }

        private void bluestein_real_inverse(double[] a, int offa)
        {
            double[] ak = new double[2 * nBluestein];
            if (n % 2 == 0)
            {
                ak[0] = a[offa] * bk1[0];
                ak[1] = a[offa] * bk1[1];

                for (int i = 1; i < n / 2; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + idx1;
                    int idx4 = offa + idx2;
                    ak[idx1] = a[idx3] * bk1[idx1] - a[idx4] * bk1[idx2];
                    ak[idx2] = a[idx3] * bk1[idx2] + a[idx4] * bk1[idx1];
                }

                ak[n] = a[offa + 1] * bk1[n];
                ak[n + 1] = a[offa + 1] * bk1[n + 1];

                for (int i = n / 2 + 1; i < n; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + 2 * n - idx1;
                    int idx4 = idx3 + 1;
                    ak[idx1] = a[idx3] * bk1[idx1] + a[idx4] * bk1[idx2];
                    ak[idx2] = a[idx3] * bk1[idx2] - a[idx4] * bk1[idx1];
                }

            }
            else
            {
                ak[0] = a[offa] * bk1[0];
                ak[1] = a[offa] * bk1[1];

                for (int i = 1; i < (n - 1) / 2; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + idx1;
                    int idx4 = offa + idx2;
                    ak[idx1] = a[idx3] * bk1[idx1] - a[idx4] * bk1[idx2];
                    ak[idx2] = a[idx3] * bk1[idx2] + a[idx4] * bk1[idx1];
                }

                ak[n - 1] = a[offa + n - 1] * bk1[n - 1] - a[offa + 1] * bk1[n];
                ak[n] = a[offa + n - 1] * bk1[n] + a[offa + 1] * bk1[n - 1];

                ak[n + 1] = a[offa + n - 1] * bk1[n + 1] + a[offa + 1] * bk1[n + 2];
                ak[n + 2] = a[offa + n - 1] * bk1[n + 2] - a[offa + 1] * bk1[n + 1];

                for (int i = (n - 1) / 2 + 2; i < n; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + 2 * n - idx1;
                    int idx4 = idx3 + 1;
                    ak[idx1] = a[idx3] * bk1[idx1] + a[idx4] * bk1[idx2];
                    ak[idx2] = a[idx3] * bk1[idx2] - a[idx4] * bk1[idx1];
                }
            }

        (2 * nBluestein).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (n >= THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                int k = nBluestein / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? nBluestein : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            for (int j = firstIdx; j < lastIdx; j++)
            {
                int idx1 = 2 * j;
                int idx2 = idx1 + 1;
                double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                ak[idx2] = im;
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                    (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (int j = firstIdx; j < lastIdx; j++)
                        {
                            int idx1 = 2 * j;
                            int idx2 = idx1 + 1;
                            a[offa + j] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {

                for (int i = 0; i < nBluestein; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                    ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                    ak[idx2] = im;
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                for (int i = 0; i < n; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    a[offa + i] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                }
            }
        }

        private void bluestein_real_inverse(DoubleLargeArray a, long offa)
        {
            DoubleLargeArray ak = new DoubleLargeArray(2 * nBluesteinl);
            if (nl % 2 == 0)
            {
                ak[0] = a[offa] * bk1l[0];
                ak[1] = a[offa] * bk1l[1];

                for (long i = 1; i < nl / 2; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + idx1;
                    long idx4 = offa + idx2;
                    ak[idx1] = a[idx3] * bk1l[idx1] - a[idx4] * bk1l[idx2];
                    ak[idx2] = a[idx3] * bk1l[idx2] + a[idx4] * bk1l[idx1];
                }

                ak[nl] = a[offa + 1] * bk1l[nl];
                ak[nl + 1] = a[offa + 1] * bk1l[nl + 1];

                for (long i = nl / 2 + 1; i < nl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + 2 * nl - idx1;
                    long idx4 = idx3 + 1;
                    ak[idx1] = a[idx3] * bk1l[idx1] + a[idx4] * bk1l[idx2];
                    ak[idx2] = a[idx3] * bk1l[idx2] - a[idx4] * bk1l[idx1];
                }

            }
            else
            {
                ak[0] = a[offa] * bk1l[0];
                ak[1] = a[offa] * bk1l[1];

                for (long i = 1; i < (nl - 1) / 2; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + idx1;
                    long idx4 = offa + idx2;
                    ak[idx1] = a[idx3] * bk1l[idx1] - a[idx4] * bk1l[idx2];
                    ak[idx2] = a[idx3] * bk1l[idx2] + a[idx4] * bk1l[idx1];
                }

                ak[nl - 1] = a[offa + nl - 1] * bk1l[nl - 1] - a[offa + 1] * bk1l[nl];
                ak[nl] = a[offa + nl - 1] * bk1l[nl] + a[offa + 1] * bk1l[nl - 1];

                ak[nl + 1] = a[offa + nl - 1] * bk1l[nl + 1] + a[offa + 1] * bk1l[nl + 2];
                ak[nl + 2] = a[offa + nl - 1] * bk1l[nl + 2] - a[offa + 1] * bk1l[nl + 1];

                for (long i = (nl - 1) / 2 + 2; i < nl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + 2 * nl - idx1;
                    long idx4 = idx3 + 1;
                    ak[idx1] = a[idx3] * bk1l[idx1] + a[idx4] * bk1l[idx2];
                    ak[idx2] = a[idx3] * bk1l[idx2] - a[idx4] * bk1l[idx1];
                }
            }

            (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (nl > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (nl > THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                long k = nBluesteinl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nBluesteinl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            for (long l = firstIdx; l < lastIdx; l++)
            {
                long idx1 = 2 * l;
                long idx2 = idx1 + 1;
                double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                ak[idx2] = im;
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (long l = firstIdx; l < lastIdx; l++)
                        {
                            long idx1 = 2 * l;
                            long idx2 = idx1 + 1;
                            a[offa + l] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {

                for (long i = 0; i < nBluesteinl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                    ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                    ak[idx2] = im;
                }

              (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

                for (long i = 0; i < nl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    a[offa + i] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                }
            }
        }

        private void bluestein_real_inverse2(double[] a, int offa)
        {
            double[] ak = new double[2 * nBluestein];
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (n >= THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                int k = n / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? n : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
        {
            for (int j = firstIdx; j < lastIdx; j++)
            {
                int idx1 = 2 * j;
                int idx2 = idx1 + 1;
                int idx3 = offa + j;
                ak[idx1] = a[idx3] * bk1[idx1];
                ak[idx2] = a[idx3] * bk1[idx2];
            }

        });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                (2 * nBluestein).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nBluestein / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = i * k;
                    int lastIdx = (i == (nthreads - 1)) ? nBluestein : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (int j = firstIdx; j < lastIdx; j++)
                        {
                            int idx1 = 2 * j;
                            int idx2 = idx1 + 1;
                            double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                            ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                            ak[idx2] = im;
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {

                for (int i = 0; i < n; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    int idx3 = offa + i;
                    ak[idx1] = a[idx3] * bk1[idx1];
                    ak[idx2] = a[idx3] * bk1[idx2];
                }

              (2 * nBluestein).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                for (int i = 0; i < nBluestein; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    double im = -ak[idx1] * bk2[idx2] + ak[idx2] * bk2[idx1];
                    ak[idx1] = ak[idx1] * bk2[idx1] + ak[idx2] * bk2[idx2];
                    ak[idx2] = im;
                }
            }

            (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

            if (n % 2 == 0)
            {
                a[offa] = bk1[0] * ak[0] - bk1[1] * ak[1];
                a[offa + 1] = bk1[n] * ak[n] - bk1[n + 1] * ak[n + 1];
                for (int i = 1; i < n / 2; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    a[offa + idx1] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                    a[offa + idx2] = bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                }
            }
            else
            {
                a[offa] = bk1[0] * ak[0] - bk1[1] * ak[1];
                a[offa + 1] = bk1[n] * ak[n - 1] + bk1[n - 1] * ak[n];
                for (int i = 1; i < (n - 1) / 2; i++)
                {
                    int idx1 = 2 * i;
                    int idx2 = idx1 + 1;
                    a[offa + idx1] = bk1[idx1] * ak[idx1] - bk1[idx2] * ak[idx2];
                    a[offa + idx2] = bk1[idx2] * ak[idx1] + bk1[idx1] * ak[idx2];
                }
                a[offa + n - 1] = bk1[n - 1] * ak[n - 1] - bk1[n] * ak[n];
            }
        }

        private void bluestein_real_inverse2(DoubleLargeArray a, long offa)
        {
            DoubleLargeArray ak = new DoubleLargeArray(2 * nBluesteinl);
            int threads = Process.GetCurrentProcess().Threads.Count;
            if ((threads > 1) && (nl > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                int nthreads = 2;
                if ((threads >= 4) && (nl > THREADS_BEGIN_N_1D_FFT_4THREADS))
                {
                    nthreads = 4;
                }
                Task[] taskArray = new Task[nthreads];
                long k = nl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (long l = firstIdx; l < lastIdx; l++)
                        {
                            long idx1 = 2 * l;
                            long idx2 = idx1 + 1;
                            long idx3 = offa + l;
                            ak[idx1] = a[idx3] * bk1l[idx1];
                            ak[idx2] = a[idx3] * bk1l[idx2];
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

                (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                k = nBluesteinl / nthreads;
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = i * k;
                    long lastIdx = (i == (nthreads - 1)) ? nBluesteinl : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (long l = firstIdx; l < lastIdx; l++)
                        {
                            long idx1 = 2 * l;
                            long idx2 = idx1 + 1;
                            double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                            ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                            ak[idx2] = im;
                        }

                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    // } catch (InterruptedException ex) {
                    // Logger.Error(ex.ToString());
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {

                for (long i = 0; i < nl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    long idx3 = offa + i;
                    ak[idx1] = a[idx3] * bk1l[idx1];
                    ak[idx2] = a[idx3] * bk1l[idx2];
                }

              (2 * nBluesteinl).cftbsub(ref ak, 0, ref ipl, nwl, ref wl);

                for (long i = 0; i < nBluesteinl; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    double im = -ak[idx1] * bk2l[idx2] + ak[idx2] * bk2l[idx1];
                    ak[idx1] = ak[idx1] * bk2l[idx1] + ak[idx2] * bk2l[idx2];
                    ak[idx2] = im;
                }
            }

            (2 * nBluesteinl).cftfsub(ref ak, 0, ref ipl, nwl, ref wl);

            if (nl % 2 == 0)
            {
                a[offa] = bk1l[0] * ak[0] - bk1l[1] * ak[1];
                a[offa + 1] = bk1l[nl] * ak[nl] - bk1l[nl + 1] * ak[nl + 1];
                for (long i = 1; i < nl / 2; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    a[offa + idx1] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                    a[offa + idx2] = bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                }
            }
            else
            {
                a[offa] = bk1l[0] * ak[0] - bk1l[1] * ak[1];
                a[offa + 1] = bk1l[nl] * ak[nl - 1] + bk1l[nl - 1] * ak[nl];
                for (long i = 1; i < (nl - 1) / 2; i++)
                {
                    long idx1 = 2 * i;
                    long idx2 = idx1 + 1;
                    a[offa + idx1] = bk1l[idx1] * ak[idx1] - bk1l[idx2] * ak[idx2];
                    a[offa + idx2] = bk1l[idx2] * ak[idx1] + bk1l[idx1] * ak[idx2];
                }
                a[offa + nl - 1] = bk1l[nl - 1] * ak[nl - 1] - bk1l[nl] * ak[nl];
            }
        }

        /*---------------------------------------------------------
         rfftf1: further processing of Real forward FFT
         --------------------------------------------------------*/
        void rfftf(double[] a, int offa)
        {
            if (n == 1)
            {
                return;
            }
            int l1, l2, na, kh, nf, ipll, iw, ido, idl1;

            double[] ch = new double[n];
            int twon = 2 * n;
            nf = (int)wtable_r[1 + twon];
            na = 1;
            l2 = n;
            iw = twon - 1;
            for (int k1 = 1; k1 <= nf; ++k1)
            {
                kh = nf - k1;
                ipll = (int)wtable_r[kh + 2 + twon];
                l1 = l2 / ipll;
                ido = n / l2;
                idl1 = ido * l1;
                iw -= (ipll - 1) * ido;
                na = 1 - na;
                switch (ipll)
                {
                    case 2:
                        if (na == 0)
                        {
                            radf2(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf2(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    case 3:
                        if (na == 0)
                        {
                            radf3(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf3(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    case 4:
                        if (na == 0)
                        {
                            radf4(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf4(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    case 5:
                        if (na == 0)
                        {
                            radf5(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf5(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    default:
                        if (ido == 1)
                        {
                            na = 1 - na;
                        }
                        if (na == 0)
                        {
                            radfg(ido, ipll, l1, idl1, a, offa, ch, 0, iw);
                            na = 1;
                        }
                        else
                        {
                            radfg(ido, ipll, l1, idl1, ch, 0, a, offa, iw);
                            na = 0;
                        }
                        break;
                }
                l2 = l1;
            }
            if (na == 1)
            {
                return;
            }
            Array.Copy(ch, 0, a, offa, n);
        }

        /*---------------------------------------------------------
         rfftf1: further processing of Real forward FFT
         --------------------------------------------------------*/
        void rfftf(DoubleLargeArray a, long offa)
        {
            if (nl == 1)
            {
                return;
            }
            long l1, l2, na, kh, nf, iw, ido, idl1;
            int ipll;

            DoubleLargeArray ch = new DoubleLargeArray(nl);
            long twon = 2 * nl;
            nf = (long)wtable_rl[1 + twon];
            na = 1;
            l2 = nl;
            iw = twon - 1;
            for (long k1 = 1; k1 <= nf; ++k1)
            {
                kh = nf - k1;
                ipll = (int)wtable_rl[kh + 2 + twon];
                l1 = l2 / ipll;
                ido = nl / l2;
                idl1 = ido * l1;
                iw -= (ipll - 1) * ido;
                na = 1 - na;
                switch (ipll)
                {
                    case 2:
                        if (na == 0)
                        {
                            radf2(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf2(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    case 3:
                        if (na == 0)
                        {
                            radf3(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf3(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    case 4:
                        if (na == 0)
                        {
                            radf4(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf4(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    case 5:
                        if (na == 0)
                        {
                            radf5(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radf5(ido, l1, ch, 0, a, offa, iw);
                        }
                        break;
                    default:
                        if (ido == 1)
                        {
                            na = 1 - na;
                        }
                        if (na == 0)
                        {
                            radfg(ido, ipll, l1, idl1, a, offa, ch, 0, iw);
                            na = 1;
                        }
                        else
                        {
                            radfg(ido, ipll, l1, idl1, ch, 0, a, offa, iw);
                            na = 0;
                        }
                        break;
                }
                l2 = l1;
            }
            if (na == 1)
            {
                return;
            }
            DoubleLargeArray.ArrayCopy(ch, 0, a, offa, nl);
        }

        /*---------------------------------------------------------
         rfftb1: further processing of Real backward FFT
         --------------------------------------------------------*/
        void rfftb(double[] a, int offa)
        {
            if (n == 1)
            {
                return;
            }
            int l1, l2, na, nf, ipll, iw, ido, idl1;

            double[] ch = new double[n];
            int twon = 2 * n;
            nf = (int)wtable_r[1 + twon];
            na = 0;
            l1 = 1;
            iw = n;
            for (int k1 = 1; k1 <= nf; k1++)
            {
                ipll = (int)wtable_r[k1 + 1 + twon];
                l2 = ipll * l1;
                ido = n / l2;
                idl1 = ido * l1;
                switch (ipll)
                {
                    case 2:
                        if (na == 0)
                        {
                            radb2(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb2(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    case 3:
                        if (na == 0)
                        {
                            radb3(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb3(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    case 4:
                        if (na == 0)
                        {
                            radb4(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb4(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    case 5:
                        if (na == 0)
                        {
                            radb5(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb5(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    default:
                        if (na == 0)
                        {
                            radbg(ido, ipll, l1, idl1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radbg(ido, ipll, l1, idl1, ch, 0, a, offa, iw);
                        }
                        if (ido == 1)
                        {
                            na = 1 - na;
                        }
                        break;
                }
                l1 = l2;
                iw += (ipll - 1) * ido;
            }
            if (na == 0)
            {
                return;
            }
            Array.Copy(ch, 0, a, offa, n);
        }

        /*---------------------------------------------------------
         rfftb1: further processing of Real backward FFT
         --------------------------------------------------------*/
        void rfftb(DoubleLargeArray a, long offa)
        {
            if (nl == 1)
            {
                return;
            }
            long l1, l2, na, nf, iw, ido, idl1;
            int ipll;
            DoubleLargeArray ch = new DoubleLargeArray(nl);
            long twon = 2 * nl;
            nf = (long)wtable_rl[1 + twon];
            na = 0;
            l1 = 1;
            iw = nl;
            for (long k1 = 1; k1 <= nf; k1++)
            {
                ipll = (int)wtable_rl[k1 + 1 + twon];
                l2 = ipll * l1;
                ido = nl / l2;
                idl1 = ido * l1;
                switch (ipll)
                {
                    case 2:
                        if (na == 0)
                        {
                            radb2(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb2(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    case 3:
                        if (na == 0)
                        {
                            radb3(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb3(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    case 4:
                        if (na == 0)
                        {
                            radb4(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb4(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    case 5:
                        if (na == 0)
                        {
                            radb5(ido, l1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radb5(ido, l1, ch, 0, a, offa, iw);
                        }
                        na = 1 - na;
                        break;
                    default:
                        if (na == 0)
                        {
                            radbg(ido, ipll, l1, idl1, a, offa, ch, 0, iw);
                        }
                        else
                        {
                            radbg(ido, ipll, l1, idl1, ch, 0, a, offa, iw);
                        }
                        if (ido == 1)
                        {
                            na = 1 - na;
                        }
                        break;
                }
                l1 = l2;
                iw += (ipll - 1) * ido;
            }
            if (na == 0)
            {
                return;
            }
            DoubleLargeArray.ArrayCopy(ch, 0, a, offa, nl);
        }

        /*-------------------------------------------------
         radf2: Real FFT's forward processing of factor 2
         -------------------------------------------------*/
        void radf2(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            int i, ic, idx0, idx1, idx2, idx3, idx4;
            double t1i, t1r, w1r, w1i;
            int iw1;
            iw1 = offset;
            idx0 = l1 * ido;
            idx1 = 2 * ido;
            for (int k = 0; k < l1; k++)
            {
                int oidx1 = out_off + k * idx1;
                int oidx2 = oidx1 + idx1 - 1;
                int iidx1 = in_off + k * ido;
                int iidx2 = iidx1 + idx0;

                double i1r = inar[iidx1];
                double i2r = inar[iidx2];

                outar[oidx1] = i1r + i2r;
                outar[oidx2] = i1r - i2r;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (int k = 0; k < l1; k++)
                {
                    idx1 = k * ido;
                    idx2 = 2 * idx1;
                    idx3 = idx2 + ido;
                    idx4 = idx1 + idx0;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        int widx1 = i - 1 + iw1;
                        int oidx1 = out_off + i + idx2;
                        int oidx2 = out_off + ic + idx3;
                        int iidx1 = in_off + i + idx1;
                        int iidx2 = in_off + i + idx4;

                        double a1i = inar[iidx1 - 1];
                        double a1r = inar[iidx1];
                        double a2i = inar[iidx2 - 1];
                        double a2r = inar[iidx2];

                        w1r = wtable_r[widx1 - 1];
                        w1i = wtable_r[widx1];

                        t1r = w1r * a2i + w1i * a2r;
                        t1i = w1r * a2r - w1i * a2i;

                        outar[oidx1] = a1r + t1i;
                        outar[oidx1 - 1] = a1i + t1r;

                        outar[oidx2] = t1i - a1r;
                        outar[oidx2 - 1] = a1i - t1r;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            idx2 = 2 * idx1;
            for (int k = 0; k < l1; k++)
            {
                idx1 = k * ido;
                int oidx1 = out_off + idx2 + ido;
                int iidx1 = in_off + ido - 1 + idx1;

                outar[oidx1] = -inar[iidx1 + idx0];
                outar[oidx1 - 1] = inar[iidx1];
            }
        }

        /*-------------------------------------------------
         radf2: Real FFT's forward processing of factor 2
         -------------------------------------------------*/
        void radf2(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            long i, ic, idx0, idx1, idx2, idx3, idx4;
            double t1i, t1r, w1r, w1i;
            long iw1;
            iw1 = offset;
            idx0 = l1 * ido;
            idx1 = 2 * ido;
            for (long k = 0; k < l1; k++)
            {
                long oidx1 = out_off + k * idx1;
                long oidx2 = oidx1 + idx1 - 1;
                long iidx1 = in_off + k * ido;
                long iidx2 = iidx1 + idx0;

                double i1r = inar[iidx1];
                double i2r = inar[iidx2];

                outar[oidx1] = i1r + i2r;
                outar[oidx2] = i1r - i2r;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (long k = 0; k < l1; k++)
                {
                    idx1 = k * ido;
                    idx2 = 2 * idx1;
                    idx3 = idx2 + ido;
                    idx4 = idx1 + idx0;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        long widx1 = i - 1 + iw1;
                        long oidx1 = out_off + i + idx2;
                        long oidx2 = out_off + ic + idx3;
                        long iidx1 = in_off + i + idx1;
                        long iidx2 = in_off + i + idx4;

                        double a1i = inar[iidx1 - 1];
                        double a1r = inar[iidx1];
                        double a2i = inar[iidx2 - 1];
                        double a2r = inar[iidx2];

                        w1r = wtable_rl[widx1 - 1];
                        w1i = wtable_rl[widx1];

                        t1r = w1r * a2i + w1i * a2r;
                        t1i = w1r * a2r - w1i * a2i;

                        outar[oidx1] = a1r + t1i;
                        outar[oidx1 - 1] = a1i + t1r;

                        outar[oidx2] = t1i - a1r;
                        outar[oidx2 - 1] = a1i - t1r;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            idx2 = 2 * idx1;
            for (long k = 0; k < l1; k++)
            {
                idx1 = k * ido;
                long oidx1 = out_off + idx2 + ido;
                long iidx1 = in_off + ido - 1 + idx1;

                outar[oidx1] = -inar[iidx1 + idx0];
                outar[oidx1 - 1] = inar[iidx1];
            }
        }

        /*-------------------------------------------------
         radb2: Real FFT's backward processing of factor 2
         -------------------------------------------------*/
        void radb2(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            int i, ic;
            double t1i, t1r, w1r, w1i;
            int iw1 = offset;

            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 2 * idx1;
                int idx3 = idx2 + ido;
                int oidx1 = out_off + idx1;
                int iidx1 = in_off + idx2;
                int iidx2 = in_off + ido - 1 + idx3;
                double i1r = inar[iidx1];
                double i2r = inar[iidx2];
                outar[oidx1] = i1r + i2r;
                outar[oidx1 + idx0] = i1r - i2r;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (int k = 0; k < l1; ++k)
                {
                    int idx1 = k * ido;
                    int idx2 = 2 * idx1;
                    int idx3 = idx2 + ido;
                    int idx4 = idx1 + idx0;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        int idx5 = i - 1 + iw1;
                        int idx6 = out_off + i;
                        int idx7 = in_off + i;
                        int idx8 = in_off + ic;
                        w1r = wtable_r[idx5 - 1];
                        w1i = wtable_r[idx5];
                        int iidx1 = idx7 + idx2;
                        int iidx2 = idx8 + idx3;
                        int oidx1 = idx6 + idx1;
                        int oidx2 = idx6 + idx4;
                        t1r = inar[iidx1 - 1] - inar[iidx2 - 1];
                        t1i = inar[iidx1] + inar[iidx2];
                        double i1i = inar[iidx1];
                        double i1r = inar[iidx1 - 1];
                        double i2i = inar[iidx2];
                        double i2r = inar[iidx2 - 1];

                        outar[oidx1 - 1] = i1r + i2r;
                        outar[oidx1] = i1i - i2i;
                        outar[oidx2 - 1] = w1r * t1r - w1i * t1i;
                        outar[oidx2] = w1r * t1i + w1i * t1r;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 2 * idx1;
                int oidx1 = out_off + ido - 1 + idx1;
                int iidx1 = in_off + idx2 + ido;
                outar[oidx1] = 2 * inar[iidx1 - 1];
                outar[oidx1 + idx0] = -2 * inar[iidx1];
            }
        }

        /*-------------------------------------------------
         radb2: Real FFT's backward processing of factor 2
         -------------------------------------------------*/
        void radb2(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            long i, ic;
            double t1i, t1r, w1r, w1i;
            long iw1 = offset;

            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 2 * idx1;
                long idx3 = idx2 + ido;
                long oidx1 = out_off + idx1;
                long iidx1 = in_off + idx2;
                long iidx2 = in_off + ido - 1 + idx3;
                double i1r = inar[iidx1];
                double i2r = inar[iidx2];
                outar[oidx1] = i1r + i2r;
                outar[oidx1 + idx0] = i1r - i2r;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (long k = 0; k < l1; ++k)
                {
                    long idx1 = k * ido;
                    long idx2 = 2 * idx1;
                    long idx3 = idx2 + ido;
                    long idx4 = idx1 + idx0;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        long idx5 = i - 1 + iw1;
                        long idx6 = out_off + i;
                        long idx7 = in_off + i;
                        long idx8 = in_off + ic;
                        w1r = wtable_rl[idx5 - 1];
                        w1i = wtable_rl[idx5];
                        long iidx1 = idx7 + idx2;
                        long iidx2 = idx8 + idx3;
                        long oidx1 = idx6 + idx1;
                        long oidx2 = idx6 + idx4;
                        t1r = inar[iidx1 - 1] - inar[iidx2 - 1];
                        t1i = inar[iidx1] + inar[iidx2];
                        double i1i = inar[iidx1];
                        double i1r = inar[iidx1 - 1];
                        double i2i = inar[iidx2];
                        double i2r = inar[iidx2 - 1];

                        outar[oidx1 - 1] = i1r + i2r;
                        outar[oidx1] = i1i - i2i;
                        outar[oidx2 - 1] = w1r * t1r - w1i * t1i;
                        outar[oidx2] = w1r * t1i + w1i * t1r;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 2 * idx1;
                long oidx1 = out_off + ido - 1 + idx1;
                long iidx1 = in_off + idx2 + ido;
                outar[oidx1] = 2 * inar[iidx1 - 1];
                outar[oidx1 + idx0] = -2 * inar[iidx1];
            }
        }

        /*-------------------------------------------------
         radf3: Real FFT's forward processing of factor 3 
         -------------------------------------------------*/
        void radf3(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            double taur = -0.5;
            double taui = 0.866025403784438707610604524234076962;
            int i, ic;
            double ci2, di2, di3, cr2, dr2, dr3, ti2, ti3, tr2, tr3, w1r, w2r, w1i, w2i;
            int iw1, iw2;
            iw1 = offset;
            iw2 = iw1 + ido;

            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx3 = 2 * idx0;
                int idx4 = (3 * k + 1) * ido;
                int iidx1 = in_off + idx1;
                int iidx2 = iidx1 + idx0;
                int iidx3 = iidx1 + idx3;
                double i1r = inar[iidx1];
                double i2r = inar[iidx2];
                double i3r = inar[iidx3];
                cr2 = i2r + i3r;
                outar[out_off + 3 * idx1] = i1r + cr2;
                outar[out_off + idx4 + ido] = taui * (i3r - i2r);
                outar[out_off + ido - 1 + idx4] = i1r + taur * cr2;
            }
            if (ido == 1)
            {
                return;
            }
            for (int k = 0; k < l1; k++)
            {
                int idx3 = k * ido;
                int idx4 = 3 * idx3;
                int idx5 = idx3 + idx0;
                int idx6 = idx5 + idx0;
                int idx7 = idx4 + ido;
                int idx8 = idx7 + ido;
                for (i = 2; i < ido; i += 2)
                {
                    ic = ido - i;
                    int widx1 = i - 1 + iw1;
                    int widx2 = i - 1 + iw2;

                    w1r = wtable_r[widx1 - 1];
                    w1i = wtable_r[widx1];
                    w2r = wtable_r[widx2 - 1];
                    w2i = wtable_r[widx2];

                    int idx9 = in_off + i;
                    int idx10 = out_off + i;
                    int idx11 = out_off + ic;
                    int iidx1 = idx9 + idx3;
                    int iidx2 = idx9 + idx5;
                    int iidx3 = idx9 + idx6;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];

                    dr2 = w1r * i2i + w1i * i2r;
                    di2 = w1r * i2r - w1i * i2i;
                    dr3 = w2r * i3i + w2i * i3r;
                    di3 = w2r * i3r - w2i * i3i;
                    cr2 = dr2 + dr3;
                    ci2 = di2 + di3;
                    tr2 = i1i + taur * cr2;
                    ti2 = i1r + taur * ci2;
                    tr3 = taui * (di2 - di3);
                    ti3 = taui * (dr3 - dr2);

                    int oidx1 = idx10 + idx4;
                    int oidx2 = idx11 + idx7;
                    int oidx3 = idx10 + idx8;

                    outar[oidx1 - 1] = i1i + cr2;
                    outar[oidx1] = i1r + ci2;
                    outar[oidx2 - 1] = tr2 - tr3;
                    outar[oidx2] = ti3 - ti2;
                    outar[oidx3 - 1] = tr2 + tr3;
                    outar[oidx3] = ti2 + ti3;
                }
            }
        }

        /*-------------------------------------------------
         radf3: Real FFT's forward processing of factor 3 
         -------------------------------------------------*/
        void radf3(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            double taur = -0.5;
            double taui = 0.866025403784438707610604524234076962;
            long i, ic;
            double ci2, di2, di3, cr2, dr2, dr3, ti2, ti3, tr2, tr3, w1r, w2r, w1i, w2i;
            long iw1, iw2;
            iw1 = offset;
            iw2 = iw1 + ido;

            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx3 = 2 * idx0;
                long idx4 = (3 * k + 1) * ido;
                long iidx1 = in_off + idx1;
                long iidx2 = iidx1 + idx0;
                long iidx3 = iidx1 + idx3;
                double i1r = inar[iidx1];
                double i2r = inar[iidx2];
                double i3r = inar[iidx3];
                cr2 = i2r + i3r;
                outar[out_off + 3 * idx1] = i1r + cr2;
                outar[out_off + idx4 + ido] = taui * (i3r - i2r);
                outar[out_off + ido - 1 + idx4] = i1r + taur * cr2;
            }
            if (ido == 1)
            {
                return;
            }
            for (long k = 0; k < l1; k++)
            {
                long idx3 = k * ido;
                long idx4 = 3 * idx3;
                long idx5 = idx3 + idx0;
                long idx6 = idx5 + idx0;
                long idx7 = idx4 + ido;
                long idx8 = idx7 + ido;
                for (i = 2; i < ido; i += 2)
                {
                    ic = ido - i;
                    long widx1 = i - 1 + iw1;
                    long widx2 = i - 1 + iw2;

                    w1r = wtable_rl[widx1 - 1];
                    w1i = wtable_rl[widx1];
                    w2r = wtable_rl[widx2 - 1];
                    w2i = wtable_rl[widx2];

                    long idx9 = in_off + i;
                    long idx10 = out_off + i;
                    long idx11 = out_off + ic;
                    long iidx1 = idx9 + idx3;
                    long iidx2 = idx9 + idx5;
                    long iidx3 = idx9 + idx6;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];

                    dr2 = w1r * i2i + w1i * i2r;
                    di2 = w1r * i2r - w1i * i2i;
                    dr3 = w2r * i3i + w2i * i3r;
                    di3 = w2r * i3r - w2i * i3i;
                    cr2 = dr2 + dr3;
                    ci2 = di2 + di3;
                    tr2 = i1i + taur * cr2;
                    ti2 = i1r + taur * ci2;
                    tr3 = taui * (di2 - di3);
                    ti3 = taui * (dr3 - dr2);

                    long oidx1 = idx10 + idx4;
                    long oidx2 = idx11 + idx7;
                    long oidx3 = idx10 + idx8;

                    outar[oidx1 - 1] = i1i + cr2;
                    outar[oidx1] = i1r + ci2;
                    outar[oidx2 - 1] = tr2 - tr3;
                    outar[oidx2] = ti3 - ti2;
                    outar[oidx3 - 1] = tr2 + tr3;
                    outar[oidx3] = ti2 + ti3;
                }
            }
        }

        /*-------------------------------------------------
         radb3: Real FFT's backward processing of factor 3
         -------------------------------------------------*/
        void radb3(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            double taur = -0.5;
            double taui = 0.866025403784438707610604524234076962;
            int i, ic;
            double ci2, ci3, di2, di3, cr2, cr3, dr2, dr3, ti2, tr2, w1r, w2r, w1i, w2i;
            int iw1, iw2;
            iw1 = offset;
            iw2 = iw1 + ido;

            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int iidx1 = in_off + 3 * idx1;
                int iidx2 = iidx1 + 2 * ido;
                double i1i = inar[iidx1];

                tr2 = 2 * inar[iidx2 - 1];
                cr2 = i1i + taur * tr2;
                ci3 = 2 * taui * inar[iidx2];

                outar[out_off + idx1] = i1i + tr2;
                outar[out_off + (k + l1) * ido] = cr2 - ci3;
                outar[out_off + (k + 2 * l1) * ido] = cr2 + ci3;
            }
            if (ido == 1)
            {
                return;
            }
            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 3 * idx1;
                int idx3 = idx2 + ido;
                int idx4 = idx3 + ido;
                int idx5 = idx1 + idx0;
                int idx6 = idx5 + idx0;
                for (i = 2; i < ido; i += 2)
                {
                    ic = ido - i;
                    int idx7 = in_off + i;
                    int idx8 = in_off + ic;
                    int idx9 = out_off + i;
                    int iidx1 = idx7 + idx2;
                    int iidx2 = idx7 + idx4;
                    int iidx3 = idx8 + idx3;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];

                    tr2 = i2i + i3i;
                    cr2 = i1i + taur * tr2;
                    ti2 = i2r - i3r;
                    ci2 = i1r + taur * ti2;
                    cr3 = taui * (i2i - i3i);
                    ci3 = taui * (i2r + i3r);
                    dr2 = cr2 - ci3;
                    dr3 = cr2 + ci3;
                    di2 = ci2 + cr3;
                    di3 = ci2 - cr3;

                    int widx1 = i - 1 + iw1;
                    int widx2 = i - 1 + iw2;

                    w1r = wtable_r[widx1 - 1];
                    w1i = wtable_r[widx1];
                    w2r = wtable_r[widx2 - 1];
                    w2i = wtable_r[widx2];

                    int oidx1 = idx9 + idx1;
                    int oidx2 = idx9 + idx5;
                    int oidx3 = idx9 + idx6;

                    outar[oidx1 - 1] = i1i + tr2;
                    outar[oidx1] = i1r + ti2;
                    outar[oidx2 - 1] = w1r * dr2 - w1i * di2;
                    outar[oidx2] = w1r * di2 + w1i * dr2;
                    outar[oidx3 - 1] = w2r * dr3 - w2i * di3;
                    outar[oidx3] = w2r * di3 + w2i * dr3;
                }
            }
        }

        /*-------------------------------------------------
         radb3: Real FFT's backward processing of factor 3
         -------------------------------------------------*/
        void radb3(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            double taur = -0.5;
            double taui = 0.866025403784438707610604524234076962;
            long i, ic;
            double ci2, ci3, di2, di3, cr2, cr3, dr2, dr3, ti2, tr2, w1r, w2r, w1i, w2i;
            long iw1, iw2;
            iw1 = offset;
            iw2 = iw1 + ido;

            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long iidx1 = in_off + 3 * idx1;
                long iidx2 = iidx1 + 2 * ido;
                double i1i = inar[iidx1];

                tr2 = 2 * inar[iidx2 - 1];
                cr2 = i1i + taur * tr2;
                ci3 = 2 * taui * inar[iidx2];

                outar[out_off + idx1] = i1i + tr2;
                outar[out_off + (k + l1) * ido] = cr2 - ci3;
                outar[out_off + (k + 2 * l1) * ido] = cr2 + ci3;
            }
            if (ido == 1)
            {
                return;
            }
            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 3 * idx1;
                long idx3 = idx2 + ido;
                long idx4 = idx3 + ido;
                long idx5 = idx1 + idx0;
                long idx6 = idx5 + idx0;
                for (i = 2; i < ido; i += 2)
                {
                    ic = ido - i;
                    long idx7 = in_off + i;
                    long idx8 = in_off + ic;
                    long idx9 = out_off + i;
                    long iidx1 = idx7 + idx2;
                    long iidx2 = idx7 + idx4;
                    long iidx3 = idx8 + idx3;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];

                    tr2 = i2i + i3i;
                    cr2 = i1i + taur * tr2;
                    ti2 = i2r - i3r;
                    ci2 = i1r + taur * ti2;
                    cr3 = taui * (i2i - i3i);
                    ci3 = taui * (i2r + i3r);
                    dr2 = cr2 - ci3;
                    dr3 = cr2 + ci3;
                    di2 = ci2 + cr3;
                    di3 = ci2 - cr3;

                    long widx1 = i - 1 + iw1;
                    long widx2 = i - 1 + iw2;

                    w1r = wtable_rl[widx1 - 1];
                    w1i = wtable_rl[widx1];
                    w2r = wtable_rl[widx2 - 1];
                    w2i = wtable_rl[widx2];

                    long oidx1 = idx9 + idx1;
                    long oidx2 = idx9 + idx5;
                    long oidx3 = idx9 + idx6;

                    outar[oidx1 - 1] = i1i + tr2;
                    outar[oidx1] = i1r + ti2;
                    outar[oidx2 - 1] = w1r * dr2 - w1i * di2;
                    outar[oidx2] = w1r * di2 + w1i * dr2;
                    outar[oidx3 - 1] = w2r * dr3 - w2i * di3;
                    outar[oidx3] = w2r * di3 + w2i * dr3;
                }
            }
        }

        /*-------------------------------------------------
         radf4: Real FFT's forward processing of factor 4
         -------------------------------------------------*/
        void radf4(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            double hsqt2 = 0.707106781186547572737310929369414225;
            int i, ic;
            double ci2, ci3, ci4, cr2, cr3, cr4, ti1, ti2, ti3, ti4, tr1, tr2, tr3, tr4, w1r, w1i, w2r, w2i, w3r, w3i;
            int iw1, iw2, iw3;
            iw1 = offset;
            iw2 = offset + ido;
            iw3 = iw2 + ido;
            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 4 * idx1;
                int idx3 = idx1 + idx0;
                int idx4 = idx3 + idx0;
                int idx5 = idx4 + idx0;
                int idx6 = idx2 + ido;
                double i1r = inar[in_off + idx1];
                double i2r = inar[in_off + idx3];
                double i3r = inar[in_off + idx4];
                double i4r = inar[in_off + idx5];

                tr1 = i2r + i4r;
                tr2 = i1r + i3r;

                int oidx1 = out_off + idx2;
                int oidx2 = out_off + idx6 + ido;

                outar[oidx1] = tr1 + tr2;
                outar[oidx2 - 1 + ido + ido] = tr2 - tr1;
                outar[oidx2 - 1] = i1r - i3r;
                outar[oidx2] = i4r - i2r;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (int k = 0; k < l1; k++)
                {
                    int idx1 = k * ido;
                    int idx2 = idx1 + idx0;
                    int idx3 = idx2 + idx0;
                    int idx4 = idx3 + idx0;
                    int idx5 = 4 * idx1;
                    int idx6 = idx5 + ido;
                    int idx7 = idx6 + ido;
                    int idx8 = idx7 + ido;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        int widx1 = i - 1 + iw1;
                        int widx2 = i - 1 + iw2;
                        int widx3 = i - 1 + iw3;
                        w1r = wtable_r[widx1 - 1];
                        w1i = wtable_r[widx1];
                        w2r = wtable_r[widx2 - 1];
                        w2i = wtable_r[widx2];
                        w3r = wtable_r[widx3 - 1];
                        w3i = wtable_r[widx3];

                        int idx9 = in_off + i;
                        int idx10 = out_off + i;
                        int idx11 = out_off + ic;
                        int iidx1 = idx9 + idx1;
                        int iidx2 = idx9 + idx2;
                        int iidx3 = idx9 + idx3;
                        int iidx4 = idx9 + idx4;

                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];

                        cr2 = w1r * i2i + w1i * i2r;
                        ci2 = w1r * i2r - w1i * i2i;
                        cr3 = w2r * i3i + w2i * i3r;
                        ci3 = w2r * i3r - w2i * i3i;
                        cr4 = w3r * i4i + w3i * i4r;
                        ci4 = w3r * i4r - w3i * i4i;
                        tr1 = cr2 + cr4;
                        tr4 = cr4 - cr2;
                        ti1 = ci2 + ci4;
                        ti4 = ci2 - ci4;
                        ti2 = i1r + ci3;
                        ti3 = i1r - ci3;
                        tr2 = i1i + cr3;
                        tr3 = i1i - cr3;

                        int oidx1 = idx10 + idx5;
                        int oidx2 = idx11 + idx6;
                        int oidx3 = idx10 + idx7;
                        int oidx4 = idx11 + idx8;

                        outar[oidx1 - 1] = tr1 + tr2;
                        outar[oidx4 - 1] = tr2 - tr1;
                        outar[oidx1] = ti1 + ti2;
                        outar[oidx4] = ti1 - ti2;
                        outar[oidx3 - 1] = ti4 + tr3;
                        outar[oidx2 - 1] = tr3 - ti4;
                        outar[oidx3] = tr4 + ti3;
                        outar[oidx2] = tr4 - ti3;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 4 * idx1;
                int idx3 = idx1 + idx0;
                int idx4 = idx3 + idx0;
                int idx5 = idx4 + idx0;
                int idx6 = idx2 + ido;
                int idx7 = idx6 + ido;
                int idx8 = idx7 + ido;
                int idx9 = in_off + ido;
                int idx10 = out_off + ido;

                double i1i = inar[idx9 - 1 + idx1];
                double i2i = inar[idx9 - 1 + idx3];
                double i3i = inar[idx9 - 1 + idx4];
                double i4i = inar[idx9 - 1 + idx5];

                ti1 = -hsqt2 * (i2i + i4i);
                tr1 = hsqt2 * (i2i - i4i);

                outar[idx10 - 1 + idx2] = tr1 + i1i;
                outar[idx10 - 1 + idx7] = i1i - tr1;
                outar[out_off + idx6] = ti1 - i3i;
                outar[out_off + idx8] = ti1 + i3i;
            }
        }

        /*-------------------------------------------------
         radf4: Real FFT's forward processing of factor 4
         -------------------------------------------------*/
        void radf4(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            double hsqt2 = 0.707106781186547572737310929369414225;
            long i, ic;
            double ci2, ci3, ci4, cr2, cr3, cr4, ti1, ti2, ti3, ti4, tr1, tr2, tr3, tr4, w1r, w1i, w2r, w2i, w3r, w3i;
            long iw1, iw2, iw3;
            iw1 = offset;
            iw2 = offset + ido;
            iw3 = iw2 + ido;
            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 4 * idx1;
                long idx3 = idx1 + idx0;
                long idx4 = idx3 + idx0;
                long idx5 = idx4 + idx0;
                long idx6 = idx2 + ido;
                double i1r = inar[in_off + idx1];
                double i2r = inar[in_off + idx3];
                double i3r = inar[in_off + idx4];
                double i4r = inar[in_off + idx5];

                tr1 = i2r + i4r;
                tr2 = i1r + i3r;

                long oidx1 = out_off + idx2;
                long oidx2 = out_off + idx6 + ido;

                outar[oidx1] = tr1 + tr2;
                outar[oidx2 - 1 + ido + ido] = tr2 - tr1;
                outar[oidx2 - 1] = i1r - i3r;
                outar[oidx2] = i4r - i2r;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (long k = 0; k < l1; k++)
                {
                    long idx1 = k * ido;
                    long idx2 = idx1 + idx0;
                    long idx3 = idx2 + idx0;
                    long idx4 = idx3 + idx0;
                    long idx5 = 4 * idx1;
                    long idx6 = idx5 + ido;
                    long idx7 = idx6 + ido;
                    long idx8 = idx7 + ido;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        long widx1 = i - 1 + iw1;
                        long widx2 = i - 1 + iw2;
                        long widx3 = i - 1 + iw3;
                        w1r = wtable_rl[widx1 - 1];
                        w1i = wtable_rl[widx1];
                        w2r = wtable_rl[widx2 - 1];
                        w2i = wtable_rl[widx2];
                        w3r = wtable_rl[widx3 - 1];
                        w3i = wtable_rl[widx3];

                        long idx9 = in_off + i;
                        long idx10 = out_off + i;
                        long idx11 = out_off + ic;
                        long iidx1 = idx9 + idx1;
                        long iidx2 = idx9 + idx2;
                        long iidx3 = idx9 + idx3;
                        long iidx4 = idx9 + idx4;

                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];

                        cr2 = w1r * i2i + w1i * i2r;
                        ci2 = w1r * i2r - w1i * i2i;
                        cr3 = w2r * i3i + w2i * i3r;
                        ci3 = w2r * i3r - w2i * i3i;
                        cr4 = w3r * i4i + w3i * i4r;
                        ci4 = w3r * i4r - w3i * i4i;
                        tr1 = cr2 + cr4;
                        tr4 = cr4 - cr2;
                        ti1 = ci2 + ci4;
                        ti4 = ci2 - ci4;
                        ti2 = i1r + ci3;
                        ti3 = i1r - ci3;
                        tr2 = i1i + cr3;
                        tr3 = i1i - cr3;

                        long oidx1 = idx10 + idx5;
                        long oidx2 = idx11 + idx6;
                        long oidx3 = idx10 + idx7;
                        long oidx4 = idx11 + idx8;

                        outar[oidx1 - 1] = tr1 + tr2;
                        outar[oidx4 - 1] = tr2 - tr1;
                        outar[oidx1] = ti1 + ti2;
                        outar[oidx4] = ti1 - ti2;
                        outar[oidx3 - 1] = ti4 + tr3;
                        outar[oidx2 - 1] = tr3 - ti4;
                        outar[oidx3] = tr4 + ti3;
                        outar[oidx2] = tr4 - ti3;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 4 * idx1;
                long idx3 = idx1 + idx0;
                long idx4 = idx3 + idx0;
                long idx5 = idx4 + idx0;
                long idx6 = idx2 + ido;
                long idx7 = idx6 + ido;
                long idx8 = idx7 + ido;
                long idx9 = in_off + ido;
                long idx10 = out_off + ido;

                double i1i = inar[idx9 - 1 + idx1];
                double i2i = inar[idx9 - 1 + idx3];
                double i3i = inar[idx9 - 1 + idx4];
                double i4i = inar[idx9 - 1 + idx5];

                ti1 = -hsqt2 * (i2i + i4i);
                tr1 = hsqt2 * (i2i - i4i);

                outar[idx10 - 1 + idx2] = (tr1 + i1i);
                outar[idx10 - 1 + idx7] = (i1i - tr1);
                outar[out_off + idx6] = (ti1 - i3i);
                outar[out_off + idx8] = (ti1 + i3i);
            }
        }

        /*-------------------------------------------------
         radb4: Real FFT's backward processing of factor 4
         -------------------------------------------------*/
        void radb4(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            double sqrt2 = 1.41421356237309514547462185873882845;
            int i, ic;
            double ci2, ci3, ci4, cr2, cr3, cr4;
            double ti1, ti2, ti3, ti4, tr1, tr2, tr3, tr4, w1r, w1i, w2r, w2i, w3r, w3i;
            int iw1, iw2, iw3;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;

            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 4 * idx1;
                int idx3 = idx1 + idx0;
                int idx4 = idx3 + idx0;
                int idx5 = idx4 + idx0;
                int idx6 = idx2 + ido;
                int idx7 = idx6 + ido;
                int idx8 = idx7 + ido;

                double i1r = inar[in_off + idx2];
                double i2r = inar[in_off + idx7];
                double i3r = inar[in_off + ido - 1 + idx8];
                double i4r = inar[in_off + ido - 1 + idx6];

                tr1 = i1r - i3r;
                tr2 = i1r + i3r;
                tr3 = i4r + i4r;
                tr4 = i2r + i2r;

                outar[out_off + idx1] = tr2 + tr3;
                outar[out_off + idx3] = tr1 - tr4;
                outar[out_off + idx4] = tr2 - tr3;
                outar[out_off + idx5] = tr1 + tr4;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (int k = 0; k < l1; ++k)
                {
                    int idx1 = k * ido;
                    int idx2 = idx1 + idx0;
                    int idx3 = idx2 + idx0;
                    int idx4 = idx3 + idx0;
                    int idx5 = 4 * idx1;
                    int idx6 = idx5 + ido;
                    int idx7 = idx6 + ido;
                    int idx8 = idx7 + ido;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        int widx1 = i - 1 + iw1;
                        int widx2 = i - 1 + iw2;
                        int widx3 = i - 1 + iw3;
                        w1r = wtable_r[widx1 - 1];
                        w1i = wtable_r[widx1];
                        w2r = wtable_r[widx2 - 1];
                        w2i = wtable_r[widx2];
                        w3r = wtable_r[widx3 - 1];
                        w3i = wtable_r[widx3];

                        int idx12 = in_off + i;
                        int idx13 = in_off + ic;
                        int idx14 = out_off + i;

                        int iidx1 = idx12 + idx5;
                        int iidx2 = idx13 + idx6;
                        int iidx3 = idx12 + idx7;
                        int iidx4 = idx13 + idx8;

                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];

                        ti1 = i1r + i4r;
                        ti2 = i1r - i4r;
                        ti3 = i3r - i2r;
                        tr4 = i3r + i2r;
                        tr1 = i1i - i4i;
                        tr2 = i1i + i4i;
                        ti4 = i3i - i2i;
                        tr3 = i3i + i2i;
                        cr3 = tr2 - tr3;
                        ci3 = ti2 - ti3;
                        cr2 = tr1 - tr4;
                        cr4 = tr1 + tr4;
                        ci2 = ti1 + ti4;
                        ci4 = ti1 - ti4;

                        int oidx1 = idx14 + idx1;
                        int oidx2 = idx14 + idx2;
                        int oidx3 = idx14 + idx3;
                        int oidx4 = idx14 + idx4;

                        outar[oidx1 - 1] = tr2 + tr3;
                        outar[oidx1] = ti2 + ti3;
                        outar[oidx2 - 1] = w1r * cr2 - w1i * ci2;
                        outar[oidx2] = w1r * ci2 + w1i * cr2;
                        outar[oidx3 - 1] = w2r * cr3 - w2i * ci3;
                        outar[oidx3] = w2r * ci3 + w2i * cr3;
                        outar[oidx4 - 1] = w3r * cr4 - w3i * ci4;
                        outar[oidx4] = w3r * ci4 + w3i * cr4;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 4 * idx1;
                int idx3 = idx1 + idx0;
                int idx4 = idx3 + idx0;
                int idx5 = idx4 + idx0;
                int idx6 = idx2 + ido;
                int idx7 = idx6 + ido;
                int idx8 = idx7 + ido;
                int idx9 = in_off + ido;
                int idx10 = out_off + ido;

                double i1r = inar[idx9 - 1 + idx2];
                double i2r = inar[idx9 - 1 + idx7];
                double i3r = inar[in_off + idx6];
                double i4r = inar[in_off + idx8];

                ti1 = i3r + i4r;
                ti2 = i4r - i3r;
                tr1 = i1r - i2r;
                tr2 = i1r + i2r;

                outar[idx10 - 1 + idx1] = tr2 + tr2;
                outar[idx10 - 1 + idx3] = sqrt2 * (tr1 - ti1);
                outar[idx10 - 1 + idx4] = ti2 + ti2;
                outar[idx10 - 1 + idx5] = -sqrt2 * (tr1 + ti1);
            }
        }

        /*-------------------------------------------------
         radb4: Real FFT's backward processing of factor 4
         -------------------------------------------------*/
        void radb4(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            double sqrt2 = 1.41421356237309514547462185873882845;
            long i, ic;
            double ci2, ci3, ci4, cr2, cr3, cr4;
            double ti1, ti2, ti3, ti4, tr1, tr2, tr3, tr4, w1r, w1i, w2r, w2i, w3r, w3i;
            long iw1, iw2, iw3;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;

            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 4 * idx1;
                long idx3 = idx1 + idx0;
                long idx4 = idx3 + idx0;
                long idx5 = idx4 + idx0;
                long idx6 = idx2 + ido;
                long idx7 = idx6 + ido;
                long idx8 = idx7 + ido;

                double i1r = inar[in_off + idx2];
                double i2r = inar[in_off + idx7];
                double i3r = inar[in_off + ido - 1 + idx8];
                double i4r = inar[in_off + ido - 1 + idx6];

                tr1 = i1r - i3r;
                tr2 = i1r + i3r;
                tr3 = i4r + i4r;
                tr4 = i2r + i2r;

                outar[out_off + idx1] = tr2 + tr3;
                outar[out_off + idx3] = tr1 - tr4;
                outar[out_off + idx4] = tr2 - tr3;
                outar[out_off + idx5] = tr1 + tr4;
            }
            if (ido < 2)
            {
                return;
            }
            if (ido != 2)
            {
                for (long k = 0; k < l1; ++k)
                {
                    long idx1 = k * ido;
                    long idx2 = idx1 + idx0;
                    long idx3 = idx2 + idx0;
                    long idx4 = idx3 + idx0;
                    long idx5 = 4 * idx1;
                    long idx6 = idx5 + ido;
                    long idx7 = idx6 + ido;
                    long idx8 = idx7 + ido;
                    for (i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        long widx1 = i - 1 + iw1;
                        long widx2 = i - 1 + iw2;
                        long widx3 = i - 1 + iw3;
                        w1r = wtable_rl[widx1 - 1];
                        w1i = wtable_rl[widx1];
                        w2r = wtable_rl[widx2 - 1];
                        w2i = wtable_rl[widx2];
                        w3r = wtable_rl[widx3 - 1];
                        w3i = wtable_rl[widx3];

                        long idx12 = in_off + i;
                        long idx13 = in_off + ic;
                        long idx14 = out_off + i;

                        long iidx1 = idx12 + idx5;
                        long iidx2 = idx13 + idx6;
                        long iidx3 = idx12 + idx7;
                        long iidx4 = idx13 + idx8;

                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];

                        ti1 = i1r + i4r;
                        ti2 = i1r - i4r;
                        ti3 = i3r - i2r;
                        tr4 = i3r + i2r;
                        tr1 = i1i - i4i;
                        tr2 = i1i + i4i;
                        ti4 = i3i - i2i;
                        tr3 = i3i + i2i;
                        cr3 = tr2 - tr3;
                        ci3 = ti2 - ti3;
                        cr2 = tr1 - tr4;
                        cr4 = tr1 + tr4;
                        ci2 = ti1 + ti4;
                        ci4 = ti1 - ti4;

                        long oidx1 = idx14 + idx1;
                        long oidx2 = idx14 + idx2;
                        long oidx3 = idx14 + idx3;
                        long oidx4 = idx14 + idx4;

                        outar[oidx1 - 1] = tr2 + tr3;
                        outar[oidx1] = ti2 + ti3;
                        outar[oidx2 - 1] = w1r * cr2 - w1i * ci2;
                        outar[oidx2] = w1r * ci2 + w1i * cr2;
                        outar[oidx3 - 1] = w2r * cr3 - w2i * ci3;
                        outar[oidx3] = w2r * ci3 + w2i * cr3;
                        outar[oidx4 - 1] = w3r * cr4 - w3i * ci4;
                        outar[oidx4] = w3r * ci4 + w3i * cr4;
                    }
                }
                if (ido % 2 == 1)
                {
                    return;
                }
            }
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 4 * idx1;
                long idx3 = idx1 + idx0;
                long idx4 = idx3 + idx0;
                long idx5 = idx4 + idx0;
                long idx6 = idx2 + ido;
                long idx7 = idx6 + ido;
                long idx8 = idx7 + ido;
                long idx9 = in_off + ido;
                long idx10 = out_off + ido;

                double i1r = inar[idx9 - 1 + idx2];
                double i2r = inar[idx9 - 1 + idx7];
                double i3r = inar[in_off + idx6];
                double i4r = inar[in_off + idx8];

                ti1 = i3r + i4r;
                ti2 = i4r - i3r;
                tr1 = i1r - i2r;
                tr2 = i1r + i2r;

                outar[idx10 - 1 + idx1] = tr2 + tr2;
                outar[idx10 - 1 + idx3] = sqrt2 * (tr1 - ti1);
                outar[idx10 - 1 + idx4] = ti2 + ti2;
                outar[idx10 - 1 + idx5] = -sqrt2 * (tr1 + ti1);
            }
        }

        /*-------------------------------------------------
         radf5: Real FFT's forward processing of factor 5
         -------------------------------------------------*/
        void radf5(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            double tr11 = 0.309016994374947451262869435595348477;
            double ti11 = 0.951056516295153531181938433292089030;
            double tr12 = -0.809016994374947340240566973079694435;
            double ti12 = 0.587785252292473248125759255344746634;
            int i, ic;
            double ci2, di2, ci4, ci5, di3, di4, di5, ci3, cr2, cr3, dr2, dr3, dr4, dr5, cr5, cr4, ti2, ti3, ti5, ti4, tr2, tr3, tr4, tr5, w1r, w1i, w2r, w2i, w3r, w3i, w4r, w4i;
            int iw1, iw2, iw3, iw4;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;
            iw4 = iw3 + ido;

            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 5 * idx1;
                int idx3 = idx2 + ido;
                int idx4 = idx3 + ido;
                int idx5 = idx4 + ido;
                int idx6 = idx5 + ido;
                int idx7 = idx1 + idx0;
                int idx8 = idx7 + idx0;
                int idx9 = idx8 + idx0;
                int idx10 = idx9 + idx0;
                int idx11 = out_off + ido - 1;

                double i1r = inar[in_off + idx1];
                double i2r = inar[in_off + idx7];
                double i3r = inar[in_off + idx8];
                double i4r = inar[in_off + idx9];
                double i5r = inar[in_off + idx10];

                cr2 = i5r + i2r;
                ci5 = i5r - i2r;
                cr3 = i4r + i3r;
                ci4 = i4r - i3r;

                outar[out_off + idx2] = i1r + cr2 + cr3;
                outar[idx11 + idx3] = i1r + tr11 * cr2 + tr12 * cr3;
                outar[out_off + idx4] = ti11 * ci5 + ti12 * ci4;
                outar[idx11 + idx5] = i1r + tr12 * cr2 + tr11 * cr3;
                outar[out_off + idx6] = ti12 * ci5 - ti11 * ci4;
            }
            if (ido == 1)
            {
                return;
            }
            for (int k = 0; k < l1; ++k)
            {
                int idx1 = k * ido;
                int idx2 = 5 * idx1;
                int idx3 = idx2 + ido;
                int idx4 = idx3 + ido;
                int idx5 = idx4 + ido;
                int idx6 = idx5 + ido;
                int idx7 = idx1 + idx0;
                int idx8 = idx7 + idx0;
                int idx9 = idx8 + idx0;
                int idx10 = idx9 + idx0;
                for (i = 2; i < ido; i += 2)
                {
                    int widx1 = i - 1 + iw1;
                    int widx2 = i - 1 + iw2;
                    int widx3 = i - 1 + iw3;
                    int widx4 = i - 1 + iw4;
                    w1r = wtable_r[widx1 - 1];
                    w1i = wtable_r[widx1];
                    w2r = wtable_r[widx2 - 1];
                    w2i = wtable_r[widx2];
                    w3r = wtable_r[widx3 - 1];
                    w3i = wtable_r[widx3];
                    w4r = wtable_r[widx4 - 1];
                    w4i = wtable_r[widx4];

                    ic = ido - i;
                    int idx15 = in_off + i;
                    int idx16 = out_off + i;
                    int idx17 = out_off + ic;

                    int iidx1 = idx15 + idx1;
                    int iidx2 = idx15 + idx7;
                    int iidx3 = idx15 + idx8;
                    int iidx4 = idx15 + idx9;
                    int iidx5 = idx15 + idx10;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];
                    double i5i = inar[iidx5 - 1];
                    double i5r = inar[iidx5];

                    dr2 = w1r * i2i + w1i * i2r;
                    di2 = w1r * i2r - w1i * i2i;
                    dr3 = w2r * i3i + w2i * i3r;
                    di3 = w2r * i3r - w2i * i3i;
                    dr4 = w3r * i4i + w3i * i4r;
                    di4 = w3r * i4r - w3i * i4i;
                    dr5 = w4r * i5i + w4i * i5r;
                    di5 = w4r * i5r - w4i * i5i;

                    cr2 = dr2 + dr5;
                    ci5 = dr5 - dr2;
                    cr5 = di2 - di5;
                    ci2 = di2 + di5;
                    cr3 = dr3 + dr4;
                    ci4 = dr4 - dr3;
                    cr4 = di3 - di4;
                    ci3 = di3 + di4;

                    tr2 = i1i + tr11 * cr2 + tr12 * cr3;
                    ti2 = i1r + tr11 * ci2 + tr12 * ci3;
                    tr3 = i1i + tr12 * cr2 + tr11 * cr3;
                    ti3 = i1r + tr12 * ci2 + tr11 * ci3;
                    tr5 = ti11 * cr5 + ti12 * cr4;
                    ti5 = ti11 * ci5 + ti12 * ci4;
                    tr4 = ti12 * cr5 - ti11 * cr4;
                    ti4 = ti12 * ci5 - ti11 * ci4;

                    int oidx1 = idx16 + idx2;
                    int oidx2 = idx17 + idx3;
                    int oidx3 = idx16 + idx4;
                    int oidx4 = idx17 + idx5;
                    int oidx5 = idx16 + idx6;

                    outar[oidx1 - 1] = i1i + cr2 + cr3;
                    outar[oidx1] = i1r + ci2 + ci3;
                    outar[oidx3 - 1] = tr2 + tr5;
                    outar[oidx2 - 1] = tr2 - tr5;
                    outar[oidx3] = ti2 + ti5;
                    outar[oidx2] = ti5 - ti2;
                    outar[oidx5 - 1] = tr3 + tr4;
                    outar[oidx4 - 1] = tr3 - tr4;
                    outar[oidx5] = ti3 + ti4;
                    outar[oidx4] = ti4 - ti3;
                }
            }
        }

        /*-------------------------------------------------
         radf5: Real FFT's forward processing of factor 5
         -------------------------------------------------*/
        void radf5(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            double tr11 = 0.309016994374947451262869435595348477;
            double ti11 = 0.951056516295153531181938433292089030;
            double tr12 = -0.809016994374947340240566973079694435;
            double ti12 = 0.587785252292473248125759255344746634;
            long i, ic;
            double ci2, di2, ci4, ci5, di3, di4, di5, ci3, cr2, cr3, dr2, dr3, dr4, dr5, cr5, cr4, ti2, ti3, ti5, ti4, tr2, tr3, tr4, tr5, w1r, w1i, w2r, w2i, w3r, w3i, w4r, w4i;
            long iw1, iw2, iw3, iw4;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;
            iw4 = iw3 + ido;

            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 5 * idx1;
                long idx3 = idx2 + ido;
                long idx4 = idx3 + ido;
                long idx5 = idx4 + ido;
                long idx6 = idx5 + ido;
                long idx7 = idx1 + idx0;
                long idx8 = idx7 + idx0;
                long idx9 = idx8 + idx0;
                long idx10 = idx9 + idx0;
                long idx11 = out_off + ido - 1;

                double i1r = inar[in_off + idx1];
                double i2r = inar[in_off + idx7];
                double i3r = inar[in_off + idx8];
                double i4r = inar[in_off + idx9];
                double i5r = inar[in_off + idx10];

                cr2 = i5r + i2r;
                ci5 = i5r - i2r;
                cr3 = i4r + i3r;
                ci4 = i4r - i3r;

                outar[out_off + idx2] = i1r + cr2 + cr3;
                outar[idx11 + idx3] = i1r + tr11 * cr2 + tr12 * cr3;
                outar[out_off + idx4] = ti11 * ci5 + ti12 * ci4;
                outar[idx11 + idx5] = i1r + tr12 * cr2 + tr11 * cr3;
                outar[out_off + idx6] = ti12 * ci5 - ti11 * ci4;
            }
            if (ido == 1)
            {
                return;
            }
            for (long k = 0; k < l1; ++k)
            {
                long idx1 = k * ido;
                long idx2 = 5 * idx1;
                long idx3 = idx2 + ido;
                long idx4 = idx3 + ido;
                long idx5 = idx4 + ido;
                long idx6 = idx5 + ido;
                long idx7 = idx1 + idx0;
                long idx8 = idx7 + idx0;
                long idx9 = idx8 + idx0;
                long idx10 = idx9 + idx0;
                for (i = 2; i < ido; i += 2)
                {
                    long widx1 = i - 1 + iw1;
                    long widx2 = i - 1 + iw2;
                    long widx3 = i - 1 + iw3;
                    long widx4 = i - 1 + iw4;
                    w1r = wtable_rl[widx1 - 1];
                    w1i = wtable_rl[widx1];
                    w2r = wtable_rl[widx2 - 1];
                    w2i = wtable_rl[widx2];
                    w3r = wtable_rl[widx3 - 1];
                    w3i = wtable_rl[widx3];
                    w4r = wtable_rl[widx4 - 1];
                    w4i = wtable_rl[widx4];

                    ic = ido - i;
                    long idx15 = in_off + i;
                    long idx16 = out_off + i;
                    long idx17 = out_off + ic;

                    long iidx1 = idx15 + idx1;
                    long iidx2 = idx15 + idx7;
                    long iidx3 = idx15 + idx8;
                    long iidx4 = idx15 + idx9;
                    long iidx5 = idx15 + idx10;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];
                    double i5i = inar[iidx5 - 1];
                    double i5r = inar[iidx5];

                    dr2 = w1r * i2i + w1i * i2r;
                    di2 = w1r * i2r - w1i * i2i;
                    dr3 = w2r * i3i + w2i * i3r;
                    di3 = w2r * i3r - w2i * i3i;
                    dr4 = w3r * i4i + w3i * i4r;
                    di4 = w3r * i4r - w3i * i4i;
                    dr5 = w4r * i5i + w4i * i5r;
                    di5 = w4r * i5r - w4i * i5i;

                    cr2 = dr2 + dr5;
                    ci5 = dr5 - dr2;
                    cr5 = di2 - di5;
                    ci2 = di2 + di5;
                    cr3 = dr3 + dr4;
                    ci4 = dr4 - dr3;
                    cr4 = di3 - di4;
                    ci3 = di3 + di4;

                    tr2 = i1i + tr11 * cr2 + tr12 * cr3;
                    ti2 = i1r + tr11 * ci2 + tr12 * ci3;
                    tr3 = i1i + tr12 * cr2 + tr11 * cr3;
                    ti3 = i1r + tr12 * ci2 + tr11 * ci3;
                    tr5 = ti11 * cr5 + ti12 * cr4;
                    ti5 = ti11 * ci5 + ti12 * ci4;
                    tr4 = ti12 * cr5 - ti11 * cr4;
                    ti4 = ti12 * ci5 - ti11 * ci4;

                    long oidx1 = idx16 + idx2;
                    long oidx2 = idx17 + idx3;
                    long oidx3 = idx16 + idx4;
                    long oidx4 = idx17 + idx5;
                    long oidx5 = idx16 + idx6;

                    outar[oidx1 - 1] = i1i + cr2 + cr3;
                    outar[oidx1] = i1r + ci2 + ci3;
                    outar[oidx3 - 1] = tr2 + tr5;
                    outar[oidx2 - 1] = tr2 - tr5;
                    outar[oidx3] = ti2 + ti5;
                    outar[oidx2] = ti5 - ti2;
                    outar[oidx5 - 1] = tr3 + tr4;
                    outar[oidx4 - 1] = tr3 - tr4;
                    outar[oidx5] = ti3 + ti4;
                    outar[oidx4] = ti4 - ti3;
                }
            }
        }

        /*-------------------------------------------------
         radb5: Real FFT's backward processing of factor 5
         -------------------------------------------------*/
        void radb5(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            double tr11 = 0.309016994374947451262869435595348477;
            double ti11 = 0.951056516295153531181938433292089030;
            double tr12 = -0.809016994374947340240566973079694435;
            double ti12 = 0.587785252292473248125759255344746634;
            int i, ic;
            double ci2, ci3, ci4, ci5, di3, di4, di5, di2, cr2, cr3, cr5, cr4, ti2, ti3, ti4, ti5, dr3, dr4, dr5, dr2, tr2, tr3, tr4, tr5, w1r, w1i, w2r, w2i, w3r, w3i, w4r, w4i;
            int iw1, iw2, iw3, iw4;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;
            iw4 = iw3 + ido;

            int idx0 = l1 * ido;
            for (int k = 0; k < l1; k++)
            {
                int idx1 = k * ido;
                int idx2 = 5 * idx1;
                int idx3 = idx2 + ido;
                int idx4 = idx3 + ido;
                int idx5 = idx4 + ido;
                int idx6 = idx5 + ido;
                int idx7 = idx1 + idx0;
                int idx8 = idx7 + idx0;
                int idx9 = idx8 + idx0;
                int idx10 = idx9 + idx0;
                int idx11 = in_off + ido - 1;

                double i1r = inar[in_off + idx2];

                ti5 = 2 * inar[in_off + idx4];
                ti4 = 2 * inar[in_off + idx6];
                tr2 = 2 * inar[idx11 + idx3];
                tr3 = 2 * inar[idx11 + idx5];
                cr2 = i1r + tr11 * tr2 + tr12 * tr3;
                cr3 = i1r + tr12 * tr2 + tr11 * tr3;
                ci5 = ti11 * ti5 + ti12 * ti4;
                ci4 = ti12 * ti5 - ti11 * ti4;

                outar[out_off + idx1] = i1r + tr2 + tr3;
                outar[out_off + idx7] = cr2 - ci5;
                outar[out_off + idx8] = cr3 - ci4;
                outar[out_off + idx9] = cr3 + ci4;
                outar[out_off + idx10] = cr2 + ci5;
            }
            if (ido == 1)
            {
                return;
            }
            for (int k = 0; k < l1; ++k)
            {
                int idx1 = k * ido;
                int idx2 = 5 * idx1;
                int idx3 = idx2 + ido;
                int idx4 = idx3 + ido;
                int idx5 = idx4 + ido;
                int idx6 = idx5 + ido;
                int idx7 = idx1 + idx0;
                int idx8 = idx7 + idx0;
                int idx9 = idx8 + idx0;
                int idx10 = idx9 + idx0;
                for (i = 2; i < ido; i += 2)
                {
                    ic = ido - i;
                    int widx1 = i - 1 + iw1;
                    int widx2 = i - 1 + iw2;
                    int widx3 = i - 1 + iw3;
                    int widx4 = i - 1 + iw4;
                    w1r = wtable_r[widx1 - 1];
                    w1i = wtable_r[widx1];
                    w2r = wtable_r[widx2 - 1];
                    w2i = wtable_r[widx2];
                    w3r = wtable_r[widx3 - 1];
                    w3i = wtable_r[widx3];
                    w4r = wtable_r[widx4 - 1];
                    w4i = wtable_r[widx4];

                    int idx15 = in_off + i;
                    int idx16 = in_off + ic;
                    int idx17 = out_off + i;

                    int iidx1 = idx15 + idx2;
                    int iidx2 = idx16 + idx3;
                    int iidx3 = idx15 + idx4;
                    int iidx4 = idx16 + idx5;
                    int iidx5 = idx15 + idx6;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];
                    double i5i = inar[iidx5 - 1];
                    double i5r = inar[iidx5];

                    ti5 = i3r + i2r;
                    ti2 = i3r - i2r;
                    ti4 = i5r + i4r;
                    ti3 = i5r - i4r;
                    tr5 = i3i - i2i;
                    tr2 = i3i + i2i;
                    tr4 = i5i - i4i;
                    tr3 = i5i + i4i;

                    cr2 = i1i + tr11 * tr2 + tr12 * tr3;
                    ci2 = i1r + tr11 * ti2 + tr12 * ti3;
                    cr3 = i1i + tr12 * tr2 + tr11 * tr3;
                    ci3 = i1r + tr12 * ti2 + tr11 * ti3;
                    cr5 = ti11 * tr5 + ti12 * tr4;
                    ci5 = ti11 * ti5 + ti12 * ti4;
                    cr4 = ti12 * tr5 - ti11 * tr4;
                    ci4 = ti12 * ti5 - ti11 * ti4;
                    dr3 = cr3 - ci4;
                    dr4 = cr3 + ci4;
                    di3 = ci3 + cr4;
                    di4 = ci3 - cr4;
                    dr5 = cr2 + ci5;
                    dr2 = cr2 - ci5;
                    di5 = ci2 - cr5;
                    di2 = ci2 + cr5;

                    int oidx1 = idx17 + idx1;
                    int oidx2 = idx17 + idx7;
                    int oidx3 = idx17 + idx8;
                    int oidx4 = idx17 + idx9;
                    int oidx5 = idx17 + idx10;

                    outar[oidx1 - 1] = i1i + tr2 + tr3;
                    outar[oidx1] = i1r + ti2 + ti3;
                    outar[oidx2 - 1] = w1r * dr2 - w1i * di2;
                    outar[oidx2] = w1r * di2 + w1i * dr2;
                    outar[oidx3 - 1] = w2r * dr3 - w2i * di3;
                    outar[oidx3] = w2r * di3 + w2i * dr3;
                    outar[oidx4 - 1] = w3r * dr4 - w3i * di4;
                    outar[oidx4] = w3r * di4 + w3i * dr4;
                    outar[oidx5 - 1] = w4r * dr5 - w4i * di5;
                    outar[oidx5] = w4r * di5 + w4i * dr5;
                }
            }
        }

        /*-------------------------------------------------
         radb5: Real FFT's backward processing of factor 5
         -------------------------------------------------*/
        void radb5(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            double tr11 = 0.309016994374947451262869435595348477;
            double ti11 = 0.951056516295153531181938433292089030;
            double tr12 = -0.809016994374947340240566973079694435;
            double ti12 = 0.587785252292473248125759255344746634;
            long i, ic;
            double ci2, ci3, ci4, ci5, di3, di4, di5, di2, cr2, cr3, cr5, cr4, ti2, ti3, ti4, ti5, dr3, dr4, dr5, dr2, tr2, tr3, tr4, tr5, w1r, w1i, w2r, w2i, w3r, w3i, w4r, w4i;
            long iw1, iw2, iw3, iw4;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;
            iw4 = iw3 + ido;

            long idx0 = l1 * ido;
            for (long k = 0; k < l1; k++)
            {
                long idx1 = k * ido;
                long idx2 = 5 * idx1;
                long idx3 = idx2 + ido;
                long idx4 = idx3 + ido;
                long idx5 = idx4 + ido;
                long idx6 = idx5 + ido;
                long idx7 = idx1 + idx0;
                long idx8 = idx7 + idx0;
                long idx9 = idx8 + idx0;
                long idx10 = idx9 + idx0;
                long idx11 = in_off + ido - 1;

                double i1r = inar[in_off + idx2];

                ti5 = 2 * inar[in_off + idx4];
                ti4 = 2 * inar[in_off + idx6];
                tr2 = 2 * inar[idx11 + idx3];
                tr3 = 2 * inar[idx11 + idx5];
                cr2 = i1r + tr11 * tr2 + tr12 * tr3;
                cr3 = i1r + tr12 * tr2 + tr11 * tr3;
                ci5 = ti11 * ti5 + ti12 * ti4;
                ci4 = ti12 * ti5 - ti11 * ti4;

                outar[out_off + idx1] = i1r + tr2 + tr3;
                outar[out_off + idx7] = cr2 - ci5;
                outar[out_off + idx8] = cr3 - ci4;
                outar[out_off + idx9] = cr3 + ci4;
                outar[out_off + idx10] = cr2 + ci5;
            }
            if (ido == 1)
            {
                return;
            }
            for (long k = 0; k < l1; ++k)
            {
                long idx1 = k * ido;
                long idx2 = 5 * idx1;
                long idx3 = idx2 + ido;
                long idx4 = idx3 + ido;
                long idx5 = idx4 + ido;
                long idx6 = idx5 + ido;
                long idx7 = idx1 + idx0;
                long idx8 = idx7 + idx0;
                long idx9 = idx8 + idx0;
                long idx10 = idx9 + idx0;
                for (i = 2; i < ido; i += 2)
                {
                    ic = ido - i;
                    long widx1 = i - 1 + iw1;
                    long widx2 = i - 1 + iw2;
                    long widx3 = i - 1 + iw3;
                    long widx4 = i - 1 + iw4;
                    w1r = wtable_rl[widx1 - 1];
                    w1i = wtable_rl[widx1];
                    w2r = wtable_rl[widx2 - 1];
                    w2i = wtable_rl[widx2];
                    w3r = wtable_rl[widx3 - 1];
                    w3i = wtable_rl[widx3];
                    w4r = wtable_rl[widx4 - 1];
                    w4i = wtable_rl[widx4];

                    long idx15 = in_off + i;
                    long idx16 = in_off + ic;
                    long idx17 = out_off + i;

                    long iidx1 = idx15 + idx2;
                    long iidx2 = idx16 + idx3;
                    long iidx3 = idx15 + idx4;
                    long iidx4 = idx16 + idx5;
                    long iidx5 = idx15 + idx6;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];
                    double i5i = inar[iidx5 - 1];
                    double i5r = inar[iidx5];

                    ti5 = i3r + i2r;
                    ti2 = i3r - i2r;
                    ti4 = i5r + i4r;
                    ti3 = i5r - i4r;
                    tr5 = i3i - i2i;
                    tr2 = i3i + i2i;
                    tr4 = i5i - i4i;
                    tr3 = i5i + i4i;

                    cr2 = i1i + tr11 * tr2 + tr12 * tr3;
                    ci2 = i1r + tr11 * ti2 + tr12 * ti3;
                    cr3 = i1i + tr12 * tr2 + tr11 * tr3;
                    ci3 = i1r + tr12 * ti2 + tr11 * ti3;
                    cr5 = ti11 * tr5 + ti12 * tr4;
                    ci5 = ti11 * ti5 + ti12 * ti4;
                    cr4 = ti12 * tr5 - ti11 * tr4;
                    ci4 = ti12 * ti5 - ti11 * ti4;
                    dr3 = cr3 - ci4;
                    dr4 = cr3 + ci4;
                    di3 = ci3 + cr4;
                    di4 = ci3 - cr4;
                    dr5 = cr2 + ci5;
                    dr2 = cr2 - ci5;
                    di5 = ci2 - cr5;
                    di2 = ci2 + cr5;

                    long oidx1 = idx17 + idx1;
                    long oidx2 = idx17 + idx7;
                    long oidx3 = idx17 + idx8;
                    long oidx4 = idx17 + idx9;
                    long oidx5 = idx17 + idx10;

                    outar[oidx1 - 1] = i1i + tr2 + tr3;
                    outar[oidx1] = i1r + ti2 + ti3;
                    outar[oidx2 - 1] = w1r * dr2 - w1i * di2;
                    outar[oidx2] = w1r * di2 + w1i * dr2;
                    outar[oidx3 - 1] = w2r * dr3 - w2i * di3;
                    outar[oidx3] = w2r * di3 + w2i * dr3;
                    outar[oidx4 - 1] = w3r * dr4 - w3i * di4;
                    outar[oidx4] = w3r * di4 + w3i * dr4;
                    outar[oidx5 - 1] = w4r * dr5 - w4i * di5;
                    outar[oidx5] = w4r * di5 + w4i * dr5;
                }
            }
        }

        /*---------------------------------------------------------
         radfg: Real FFT's forward processing of general factor
         --------------------------------------------------------*/
        void radfg(int ido, int ip, int l1, int idl1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            int idij, ipph, j2, ic, jc, lc, is1, nbd;
            double dc2, ai1, ai2, ar1, ar2, ds2, dcp, arg, dsp, ar1h, ar2h, w1r, w1i;
            int iw1 = offset;

            arg = TWO_PI / (double)ip;
            dcp = System.Math.Cos(arg);
            dsp = System.Math.Sin(arg);
            ipph = (ip + 1) / 2;
            nbd = (ido - 1) / 2;
            if (ido != 1)
            {
                for (int ik = 0; ik < idl1; ik++)
                {
                    outar[out_off + ik] = inar[in_off + ik];
                }
                for (int j = 1; j < ip; j++)
                {
                    int idx1 = j * l1 * ido;
                    for (int k = 0; k < l1; k++)
                    {
                        int idx2 = k * ido + idx1;
                        outar[out_off + idx2] = inar[in_off + idx2];
                    }
                }
                if (nbd <= l1)
                {
                    is1 = -ido;
                    for (int j = 1; j < ip; j++)
                    {
                        is1 += ido;
                        idij = is1 - 1;
                        int idx1 = j * l1 * ido;
                        for (int i = 2; i < ido; i += 2)
                        {
                            idij += 2;
                            int idx2 = idij + iw1;
                            int idx4 = in_off + i;
                            int idx5 = out_off + i;
                            w1r = wtable_r[idx2 - 1];
                            w1i = wtable_r[idx2];
                            for (int k = 0; k < l1; k++)
                            {
                                int idx3 = k * ido + idx1;
                                int oidx1 = idx5 + idx3;
                                int iidx1 = idx4 + idx3;
                                double i1i = inar[iidx1 - 1];
                                double i1r = inar[iidx1];

                                outar[oidx1 - 1] = w1r * i1i + w1i * i1r;
                                outar[oidx1] = w1r * i1r - w1i * i1i;
                            }
                        }
                    }
                }
                else
                {
                    is1 = -ido;
                    for (int j = 1; j < ip; j++)
                    {
                        is1 += ido;
                        int idx1 = j * l1 * ido;
                        for (int k = 0; k < l1; k++)
                        {
                            idij = is1 - 1;
                            int idx3 = k * ido + idx1;
                            for (int i = 2; i < ido; i += 2)
                            {
                                idij += 2;
                                int idx2 = idij + iw1;
                                w1r = wtable_r[idx2 - 1];
                                w1i = wtable_r[idx2];
                                int oidx1 = out_off + i + idx3;
                                int iidx1 = in_off + i + idx3;
                                double i1i = inar[iidx1 - 1];
                                double i1r = inar[iidx1];

                                outar[oidx1 - 1] = w1r * i1i + w1i * i1r;
                                outar[oidx1] = w1r * i1r - w1i * i1i;
                            }
                        }
                    }
                }
                if (nbd >= l1)
                {
                    for (int j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        int idx1 = j * l1 * ido;
                        int idx2 = jc * l1 * ido;
                        for (int k = 0; k < l1; k++)
                        {
                            int idx3 = k * ido + idx1;
                            int idx4 = k * ido + idx2;
                            for (int i = 2; i < ido; i += 2)
                            {
                                int idx5 = in_off + i;
                                int idx6 = out_off + i;
                                int iidx1 = idx5 + idx3;
                                int iidx2 = idx5 + idx4;
                                int oidx1 = idx6 + idx3;
                                int oidx2 = idx6 + idx4;
                                double o1i = outar[oidx1 - 1];
                                double o1r = outar[oidx1];
                                double o2i = outar[oidx2 - 1];
                                double o2r = outar[oidx2];

                                inar[iidx1 - 1] = o1i + o2i;
                                inar[iidx1] = o1r + o2r;

                                inar[iidx2 - 1] = o1r - o2r;
                                inar[iidx2] = o2i - o1i;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        int idx1 = j * l1 * ido;
                        int idx2 = jc * l1 * ido;
                        for (int i = 2; i < ido; i += 2)
                        {
                            int idx5 = in_off + i;
                            int idx6 = out_off + i;
                            for (int k = 0; k < l1; k++)
                            {
                                int idx3 = k * ido + idx1;
                                int idx4 = k * ido + idx2;
                                int iidx1 = idx5 + idx3;
                                int iidx2 = idx5 + idx4;
                                int oidx1 = idx6 + idx3;
                                int oidx2 = idx6 + idx4;
                                double o1i = outar[oidx1 - 1];
                                double o1r = outar[oidx1];
                                double o2i = outar[oidx2 - 1];
                                double o2r = outar[oidx2];

                                inar[iidx1 - 1] = o1i + o2i;
                                inar[iidx1] = o1r + o2r;
                                inar[iidx2 - 1] = o1r - o2r;
                                inar[iidx2] = o2i - o1i;
                            }
                        }
                    }
                }
            }
            else
            {
                Array.Copy(outar, out_off, inar, in_off, idl1);
            }
            for (int j = 1; j < ipph; j++)
            {
                jc = ip - j;
                int idx1 = j * l1 * ido;
                int idx2 = jc * l1 * ido;
                for (int k = 0; k < l1; k++)
                {
                    int idx3 = k * ido + idx1;
                    int idx4 = k * ido + idx2;
                    int oidx1 = out_off + idx3;
                    int oidx2 = out_off + idx4;
                    double o1r = outar[oidx1];
                    double o2r = outar[oidx2];

                    inar[in_off + idx3] = o1r + o2r;
                    inar[in_off + idx4] = o2r - o1r;
                }
            }

            ar1 = 1;
            ai1 = 0;
            int idx0 = (ip - 1) * idl1;
            for (int l = 1; l < ipph; l++)
            {
                lc = ip - l;
                ar1h = dcp * ar1 - dsp * ai1;
                ai1 = dcp * ai1 + dsp * ar1;
                ar1 = ar1h;
                int idx1 = l * idl1;
                int idx2 = lc * idl1;
                for (int ik = 0; ik < idl1; ik++)
                {
                    int idx3 = out_off + ik;
                    int idx4 = in_off + ik;
                    outar[idx3 + idx1] = inar[idx4] + ar1 * inar[idx4 + idl1];
                    outar[idx3 + idx2] = ai1 * inar[idx4 + idx0];
                }
                dc2 = ar1;
                ds2 = ai1;
                ar2 = ar1;
                ai2 = ai1;
                for (int j = 2; j < ipph; j++)
                {
                    jc = ip - j;
                    ar2h = dc2 * ar2 - ds2 * ai2;
                    ai2 = dc2 * ai2 + ds2 * ar2;
                    ar2 = ar2h;
                    int idx3 = j * idl1;
                    int idx4 = jc * idl1;
                    for (int ik = 0; ik < idl1; ik++)
                    {
                        int idx5 = out_off + ik;
                        int idx6 = in_off + ik;
                        outar[idx5 + idx1] += ar2 * inar[idx6 + idx3];
                        outar[idx5 + idx2] += ai2 * inar[idx6 + idx4];
                    }
                }
            }
            for (int j = 1; j < ipph; j++)
            {
                int idx1 = j * idl1;
                for (int ik = 0; ik < idl1; ik++)
                {
                    outar[out_off + ik] += inar[in_off + ik + idx1];
                }
            }

            if (ido >= l1)
            {
                for (int k = 0; k < l1; k++)
                {
                    int idx1 = k * ido;
                    int idx2 = idx1 * ip;
                    for (int i = 0; i < ido; i++)
                    {
                        inar[in_off + i + idx2] = outar[out_off + i + idx1];
                    }
                }
            }
            else
            {
                for (int i = 0; i < ido; i++)
                {
                    for (int k = 0; k < l1; k++)
                    {
                        int idx1 = k * ido;
                        inar[in_off + i + idx1 * ip] = outar[out_off + i + idx1];
                    }
                }
            }
            int idx01 = ip * ido;
            for (int j = 1; j < ipph; j++)
            {
                jc = ip - j;
                j2 = 2 * j;
                int idx1 = j * l1 * ido;
                int idx2 = jc * l1 * ido;
                int idx3 = j2 * ido;
                for (int k = 0; k < l1; k++)
                {
                    int idx4 = k * ido;
                    int idx5 = idx4 + idx1;
                    int idx6 = idx4 + idx2;
                    int idx7 = k * idx01;
                    inar[in_off + ido - 1 + idx3 - ido + idx7] = outar[out_off + idx5];
                    inar[in_off + idx3 + idx7] = outar[out_off + idx6];
                }
            }
            if (ido == 1)
            {
                return;
            }
            if (nbd >= l1)
            {
                for (int j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    j2 = 2 * j;
                    int idx1 = j * l1 * ido;
                    int idx2 = jc * l1 * ido;
                    int idx3 = j2 * ido;
                    for (int k = 0; k < l1; k++)
                    {
                        int idx4 = k * idx01;
                        int idx5 = k * ido;
                        for (int i = 2; i < ido; i += 2)
                        {
                            ic = ido - i;
                            int idx6 = in_off + i;
                            int idx7 = in_off + ic;
                            int idx8 = out_off + i;
                            int iidx1 = idx6 + idx3 + idx4;
                            int iidx2 = idx7 + idx3 - ido + idx4;
                            int oidx1 = idx8 + idx5 + idx1;
                            int oidx2 = idx8 + idx5 + idx2;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            double o2i = outar[oidx2 - 1];
                            double o2r = outar[oidx2];

                            inar[iidx1 - 1] = o1i + o2i;
                            inar[iidx2 - 1] = o1i - o2i;
                            inar[iidx1] = o1r + o2r;
                            inar[iidx2] = o2r - o1r;
                        }
                    }
                }
            }
            else
            {
                for (int j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    j2 = 2 * j;
                    int idx1 = j * l1 * ido;
                    int idx2 = jc * l1 * ido;
                    int idx3 = j2 * ido;
                    for (int i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        int idx6 = in_off + i;
                        int idx7 = in_off + ic;
                        int idx8 = out_off + i;
                        for (int k = 0; k < l1; k++)
                        {
                            int idx4 = k * idx01;
                            int idx5 = k * ido;
                            int iidx1 = idx6 + idx3 + idx4;
                            int iidx2 = idx7 + idx3 - ido + idx4;
                            int oidx1 = idx8 + idx5 + idx1;
                            int oidx2 = idx8 + idx5 + idx2;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            double o2i = outar[oidx2 - 1];
                            double o2r = outar[oidx2];

                            inar[iidx1 - 1] = o1i + o2i;
                            inar[iidx2 - 1] = o1i - o2i;
                            inar[iidx1] = o1r + o2r;
                            inar[iidx2] = o2r - o1r;
                        }
                    }
                }
            }
        }

        /*---------------------------------------------------------
         radfg: Real FFT's forward processing of general factor
         --------------------------------------------------------*/
        void radfg(long ido, long ip, long l1, long idl1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset)
        {
            long idij, ipph, j2, ic, jc, lc, is1, nbd;
            double dc2, ai1, ai2, ar1, ar2, ds2, dcp, arg, dsp, ar1h, ar2h, w1r, w1i;
            long iw1 = offset;

            arg = TWO_PI / (double)ip;
            dcp = System.Math.Cos(arg);
            dsp = System.Math.Sin(arg);
            ipph = (ip + 1) / 2;
            nbd = (ido - 1) / 2;
            if (ido != 1)
            {
                for (long ik = 0; ik < idl1; ik++)
                {
                    outar[out_off + ik] = inar[in_off + ik];
                }
                for (long j = 1; j < ip; j++)
                {
                    long idx1 = j * l1 * ido;
                    for (long k = 0; k < l1; k++)
                    {
                        long idx2 = k * ido + idx1;
                        outar[out_off + idx2] = inar[in_off + idx2];
                    }
                }
                if (nbd <= l1)
                {
                    is1 = -ido;
                    for (long j = 1; j < ip; j++)
                    {
                        is1 += ido;
                        idij = is1 - 1;
                        long idx1 = j * l1 * ido;
                        for (long i = 2; i < ido; i += 2)
                        {
                            idij += 2;
                            long idx2 = idij + iw1;
                            long idx4 = in_off + i;
                            long idx5 = out_off + i;
                            w1r = wtable_rl[idx2 - 1];
                            w1i = wtable_rl[idx2];
                            for (long k = 0; k < l1; k++)
                            {
                                long idx3 = k * ido + idx1;
                                long oidx1 = idx5 + idx3;
                                long iidx1 = idx4 + idx3;
                                double i1i = inar[iidx1 - 1];
                                double i1r = inar[iidx1];

                                outar[oidx1 - 1] = w1r * i1i + w1i * i1r;
                                outar[oidx1] = w1r * i1r - w1i * i1i;
                            }
                        }
                    }
                }
                else
                {
                    is1 = -ido;
                    for (long j = 1; j < ip; j++)
                    {
                        is1 += ido;
                        long idx1 = j * l1 * ido;
                        for (long k = 0; k < l1; k++)
                        {
                            idij = is1 - 1;
                            long idx3 = k * ido + idx1;
                            for (long i = 2; i < ido; i += 2)
                            {
                                idij += 2;
                                long idx2 = idij + iw1;
                                w1r = wtable_rl[idx2 - 1];
                                w1i = wtable_rl[idx2];
                                long oidx1 = out_off + i + idx3;
                                long iidx1 = in_off + i + idx3;
                                double i1i = inar[iidx1 - 1];
                                double i1r = inar[iidx1];

                                outar[oidx1 - 1] = w1r * i1i + w1i * i1r;
                                outar[oidx1] = w1r * i1r - w1i * i1i;
                            }
                        }
                    }
                }
                if (nbd >= l1)
                {
                    for (long j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        long idx1 = j * l1 * ido;
                        long idx2 = jc * l1 * ido;
                        for (long k = 0; k < l1; k++)
                        {
                            long idx3 = k * ido + idx1;
                            long idx4 = k * ido + idx2;
                            for (long i = 2; i < ido; i += 2)
                            {
                                long idx5 = in_off + i;
                                long idx6 = out_off + i;
                                long iidx1 = idx5 + idx3;
                                long iidx2 = idx5 + idx4;
                                long oidx1 = idx6 + idx3;
                                long oidx2 = idx6 + idx4;
                                double o1i = outar[oidx1 - 1];
                                double o1r = outar[oidx1];
                                double o2i = outar[oidx2 - 1];
                                double o2r = outar[oidx2];

                                inar[iidx1 - 1] = o1i + o2i;
                                inar[iidx1] = o1r + o2r;

                                inar[iidx2 - 1] = o1r - o2r;
                                inar[iidx2] = o2i - o1i;
                            }
                        }
                    }
                }
                else
                {
                    for (long j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        long idx1 = j * l1 * ido;
                        long idx2 = jc * l1 * ido;
                        for (long i = 2; i < ido; i += 2)
                        {
                            long idx5 = in_off + i;
                            long idx6 = out_off + i;
                            for (long k = 0; k < l1; k++)
                            {
                                long idx3 = k * ido + idx1;
                                long idx4 = k * ido + idx2;
                                long iidx1 = idx5 + idx3;
                                long iidx2 = idx5 + idx4;
                                long oidx1 = idx6 + idx3;
                                long oidx2 = idx6 + idx4;
                                double o1i = outar[oidx1 - 1];
                                double o1r = outar[oidx1];
                                double o2i = outar[oidx2 - 1];
                                double o2r = outar[oidx2];

                                inar[iidx1 - 1] = (o1i + o2i);
                                inar[iidx1] = (o1r + o2r);
                                inar[iidx2 - 1] = (o1r - o2r);
                                inar[iidx2] = (o2i - o1i);
                            }
                        }
                    }
                }
            }
            else
            {
                DoubleLargeArray.ArrayCopy(outar, out_off, inar, in_off, idl1);
            }
            for (long j = 1; j < ipph; j++)
            {
                jc = ip - j;
                long idx1 = j * l1 * ido;
                long idx2 = jc * l1 * ido;
                for (long k = 0; k < l1; k++)
                {
                    long idx3 = k * ido + idx1;
                    long idx4 = k * ido + idx2;
                    long oidx1 = out_off + idx3;
                    long oidx2 = out_off + idx4;
                    double o1r = outar[oidx1];
                    double o2r = outar[oidx2];

                    inar[in_off + idx3] = o1r + o2r;
                    inar[in_off + idx4] = o2r - o1r;
                }
            }

            ar1 = 1;
            ai1 = 0;
            long idx0 = (ip - 1) * idl1;
            for (long l = 1; l < ipph; l++)
            {
                lc = ip - l;
                ar1h = dcp * ar1 - dsp * ai1;
                ai1 = dcp * ai1 + dsp * ar1;
                ar1 = ar1h;
                long idx1 = l * idl1;
                long idx2 = lc * idl1;
                for (long ik = 0; ik < idl1; ik++)
                {
                    long idx3 = out_off + ik;
                    long idx4 = in_off + ik;
                    outar[idx3 + idx1] = inar[idx4] + ar1 * inar[idx4 + idl1];
                    outar[idx3 + idx2] = ai1 * inar[idx4 + idx0];
                }
                dc2 = ar1;
                ds2 = ai1;
                ar2 = ar1;
                ai2 = ai1;
                for (long j = 2; j < ipph; j++)
                {
                    jc = ip - j;
                    ar2h = dc2 * ar2 - ds2 * ai2;
                    ai2 = dc2 * ai2 + ds2 * ar2;
                    ar2 = ar2h;
                    long idx3 = j * idl1;
                    long idx4 = jc * idl1;
                    for (long ik = 0; ik < idl1; ik++)
                    {
                        long idx5 = out_off + ik;
                        long idx6 = in_off + ik;
                        outar[idx5 + idx1] = outar[idx5 + idx1] + ar2 * inar[idx6 + idx3];
                        outar[idx5 + idx2] = outar[idx5 + idx2] + ai2 * inar[idx6 + idx4];
                    }
                }
            }
            for (long j = 1; j < ipph; j++)
            {
                long idx1 = j * idl1;
                for (long ik = 0; ik < idl1; ik++)
                {
                    outar[out_off + ik] = outar[out_off + ik] + inar[in_off + ik + idx1];
                }
            }

            if (ido >= l1)
            {
                for (long k = 0; k < l1; k++)
                {
                    long idx1 = k * ido;
                    long idx2 = idx1 * ip;
                    for (long i = 0; i < ido; i++)
                    {
                        inar[in_off + i + idx2] = outar[out_off + i + idx1];
                    }
                }
            }
            else
            {
                for (long i = 0; i < ido; i++)
                {
                    for (long k = 0; k < l1; k++)
                    {
                        long idx1 = k * ido;
                        inar[in_off + i + idx1 * ip] = outar[out_off + i + idx1];
                    }
                }
            }
            long idx01 = ip * ido;
            for (long j = 1; j < ipph; j++)
            {
                jc = ip - j;
                j2 = 2 * j;
                long idx1 = j * l1 * ido;
                long idx2 = jc * l1 * ido;
                long idx3 = j2 * ido;
                for (long k = 0; k < l1; k++)
                {
                    long idx4 = k * ido;
                    long idx5 = idx4 + idx1;
                    long idx6 = idx4 + idx2;
                    long idx7 = k * idx01;
                    inar[in_off + ido - 1 + idx3 - ido + idx7] = outar[out_off + idx5];
                    inar[in_off + idx3 + idx7] = outar[out_off + idx6];
                }
            }
            if (ido == 1)
            {
                return;
            }
            if (nbd >= l1)
            {
                for (long j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    j2 = 2 * j;
                    long idx1 = j * l1 * ido;
                    long idx2 = jc * l1 * ido;
                    long idx3 = j2 * ido;
                    for (long k = 0; k < l1; k++)
                    {
                        long idx4 = k * idx01;
                        long idx5 = k * ido;
                        for (long i = 2; i < ido; i += 2)
                        {
                            ic = ido - i;
                            long idx6 = in_off + i;
                            long idx7 = in_off + ic;
                            long idx8 = out_off + i;
                            long iidx1 = idx6 + idx3 + idx4;
                            long iidx2 = idx7 + idx3 - ido + idx4;
                            long oidx1 = idx8 + idx5 + idx1;
                            long oidx2 = idx8 + idx5 + idx2;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            double o2i = outar[oidx2 - 1];
                            double o2r = outar[oidx2];

                            inar[iidx1 - 1] = (o1i + o2i);
                            inar[iidx2 - 1] = (o1i - o2i);
                            inar[iidx1] = (o1r + o2r);
                            inar[iidx2] = (o2r - o1r);
                        }
                    }
                }
            }
            else
            {
                for (long j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    j2 = 2 * j;
                    long idx1 = j * l1 * ido;
                    long idx2 = jc * l1 * ido;
                    long idx3 = j2 * ido;
                    for (long i = 2; i < ido; i += 2)
                    {
                        ic = ido - i;
                        long idx6 = in_off + i;
                        long idx7 = in_off + ic;
                        long idx8 = out_off + i;
                        for (long k = 0; k < l1; k++)
                        {
                            long idx4 = k * idx01;
                            long idx5 = k * ido;
                            long iidx1 = idx6 + idx3 + idx4;
                            long iidx2 = idx7 + idx3 - ido + idx4;
                            long oidx1 = idx8 + idx5 + idx1;
                            long oidx2 = idx8 + idx5 + idx2;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            double o2i = outar[oidx2 - 1];
                            double o2r = outar[oidx2];

                            inar[iidx1 - 1] = (o1i + o2i);
                            inar[iidx2 - 1] = (o1i - o2i);
                            inar[iidx1] = (o1r + o2r);
                            inar[iidx2] = (o2r - o1r);
                        }
                    }
                }
            }
        }

        /*---------------------------------------------------------
         radbg: Real FFT's backward processing of general factor
         --------------------------------------------------------*/
        void radbg(int ido, int ip, int l1, int idl1, double[] inar, int in_off, double[] outar, int out_off, int offset)
        {
            int idij, ipph, j2, ic, jc, lc, is1;
            double dc2, ai1, ai2, ar1, ar2, ds2, w1r, w1i;
            int nbd;
            double dcp, arg, dsp, ar1h, ar2h;
            int iw1 = offset;

            arg = TWO_PI / (double)ip;
            dcp = System.Math.Cos(arg);
            dsp = System.Math.Sin(arg);
            nbd = (ido - 1) / 2;
            ipph = (ip + 1) / 2;
            int idx0 = ip * ido;
            if (ido >= l1)
            {
                for (int k = 0; k < l1; k++)
                {
                    int idx1 = k * ido;
                    int idx2 = k * idx0;
                    for (int i = 0; i < ido; i++)
                    {
                        outar[out_off + i + idx1] = inar[in_off + i + idx2];
                    }
                }
            }
            else
            {
                for (int i = 0; i < ido; i++)
                {
                    int idx1 = out_off + i;
                    int idx2 = in_off + i;
                    for (int k = 0; k < l1; k++)
                    {
                        outar[idx1 + k * ido] = inar[idx2 + k * idx0];
                    }
                }
            }
            int iidx0 = in_off + ido - 1;
            for (int j = 1; j < ipph; j++)
            {
                jc = ip - j;
                j2 = 2 * j;
                int idx1 = j * l1 * ido;
                int idx2 = jc * l1 * ido;
                int idx3 = j2 * ido;
                for (int k = 0; k < l1; k++)
                {
                    int idx4 = k * ido;
                    int idx5 = idx4 * ip;
                    int iidx1 = iidx0 + idx3 + idx5 - ido;
                    int iidx2 = in_off + idx3 + idx5;
                    double i1r = inar[iidx1];
                    double i2r = inar[iidx2];

                    outar[out_off + idx4 + idx1] = i1r + i1r;
                    outar[out_off + idx4 + idx2] = i2r + i2r;
                }
            }

            if (ido != 1)
            {
                if (nbd >= l1)
                {
                    for (int j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        int idx1 = j * l1 * ido;
                        int idx2 = jc * l1 * ido;
                        int idx3 = 2 * j * ido;
                        for (int k = 0; k < l1; k++)
                        {
                            int idx4 = k * ido + idx1;
                            int idx5 = k * ido + idx2;
                            int idx6 = k * ip * ido + idx3;
                            for (int i = 2; i < ido; i += 2)
                            {
                                ic = ido - i;
                                int idx7 = out_off + i;
                                int idx8 = in_off + ic;
                                int idx9 = in_off + i;
                                int oidx1 = idx7 + idx4;
                                int oidx2 = idx7 + idx5;
                                int iidx1 = idx9 + idx6;
                                int iidx2 = idx8 + idx6 - ido;
                                double a1i = inar[iidx1 - 1];
                                double a1r = inar[iidx1];
                                double a2i = inar[iidx2 - 1];
                                double a2r = inar[iidx2];

                                outar[oidx1 - 1] = a1i + a2i;
                                outar[oidx2 - 1] = a1i - a2i;
                                outar[oidx1] = a1r - a2r;
                                outar[oidx2] = a1r + a2r;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        int idx1 = j * l1 * ido;
                        int idx2 = jc * l1 * ido;
                        int idx3 = 2 * j * ido;
                        for (int i = 2; i < ido; i += 2)
                        {
                            ic = ido - i;
                            int idx7 = out_off + i;
                            int idx8 = in_off + ic;
                            int idx9 = in_off + i;
                            for (int k = 0; k < l1; k++)
                            {
                                int idx4 = k * ido + idx1;
                                int idx5 = k * ido + idx2;
                                int idx6 = k * ip * ido + idx3;
                                int oidx1 = idx7 + idx4;
                                int oidx2 = idx7 + idx5;
                                int iidx1 = idx9 + idx6;
                                int iidx2 = idx8 + idx6 - ido;
                                double a1i = inar[iidx1 - 1];
                                double a1r = inar[iidx1];
                                double a2i = inar[iidx2 - 1];
                                double a2r = inar[iidx2];

                                outar[oidx1 - 1] = a1i + a2i;
                                outar[oidx2 - 1] = a1i - a2i;
                                outar[oidx1] = a1r - a2r;
                                outar[oidx2] = a1r + a2r;
                            }
                        }
                    }
                }
            }

            ar1 = 1;
            ai1 = 0;
            int idx01 = (ip - 1) * idl1;
            for (int l = 1; l < ipph; l++)
            {
                lc = ip - l;
                ar1h = dcp * ar1 - dsp * ai1;
                ai1 = dcp * ai1 + dsp * ar1;
                ar1 = ar1h;
                int idx1 = l * idl1;
                int idx2 = lc * idl1;
                for (int ik = 0; ik < idl1; ik++)
                {
                    int idx3 = in_off + ik;
                    int idx4 = out_off + ik;
                    inar[idx3 + idx1] = outar[idx4] + ar1 * outar[idx4 + idl1];
                    inar[idx3 + idx2] = ai1 * outar[idx4 + idx01];
                }
                dc2 = ar1;
                ds2 = ai1;
                ar2 = ar1;
                ai2 = ai1;
                for (int j = 2; j < ipph; j++)
                {
                    jc = ip - j;
                    ar2h = dc2 * ar2 - ds2 * ai2;
                    ai2 = dc2 * ai2 + ds2 * ar2;
                    ar2 = ar2h;
                    int idx5 = j * idl1;
                    int idx6 = jc * idl1;
                    for (int ik = 0; ik < idl1; ik++)
                    {
                        int idx7 = in_off + ik;
                        int idx8 = out_off + ik;
                        inar[idx7 + idx1] += ar2 * outar[idx8 + idx5];
                        inar[idx7 + idx2] += ai2 * outar[idx8 + idx6];
                    }
                }
            }
            for (int j = 1; j < ipph; j++)
            {
                int idx1 = j * idl1;
                for (int ik = 0; ik < idl1; ik++)
                {
                    int idx2 = out_off + ik;
                    outar[idx2] += outar[idx2 + idx1];
                }
            }
            for (int j = 1; j < ipph; j++)
            {
                jc = ip - j;
                int idx1 = j * l1 * ido;
                int idx2 = jc * l1 * ido;
                for (int k = 0; k < l1; k++)
                {
                    int idx3 = k * ido;
                    int oidx1 = out_off + idx3;
                    int iidx1 = in_off + idx3 + idx1;
                    int iidx2 = in_off + idx3 + idx2;
                    double i1r = inar[iidx1];
                    double i2r = inar[iidx2];

                    outar[oidx1 + idx1] = i1r - i2r;
                    outar[oidx1 + idx2] = i1r + i2r;
                }
            }

            if (ido == 1)
            {
                return;
            }
            if (nbd >= l1)
            {
                for (int j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    int idx1 = j * l1 * ido;
                    int idx2 = jc * l1 * ido;
                    for (int k = 0; k < l1; k++)
                    {
                        int idx3 = k * ido;
                        for (int i = 2; i < ido; i += 2)
                        {
                            int idx4 = out_off + i;
                            int idx5 = in_off + i;
                            int oidx1 = idx4 + idx3 + idx1;
                            int oidx2 = idx4 + idx3 + idx2;
                            int iidx1 = idx5 + idx3 + idx1;
                            int iidx2 = idx5 + idx3 + idx2;
                            double i1i = inar[iidx1 - 1];
                            double i1r = inar[iidx1];
                            double i2i = inar[iidx2 - 1];
                            double i2r = inar[iidx2];

                            outar[oidx1 - 1] = i1i - i2r;
                            outar[oidx2 - 1] = i1i + i2r;
                            outar[oidx1] = i1r + i2i;
                            outar[oidx2] = i1r - i2i;
                        }
                    }
                }
            }
            else
            {
                for (int j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    int idx1 = j * l1 * ido;
                    int idx2 = jc * l1 * ido;
                    for (int i = 2; i < ido; i += 2)
                    {
                        int idx4 = out_off + i;
                        int idx5 = in_off + i;
                        for (int k = 0; k < l1; k++)
                        {
                            int idx3 = k * ido;
                            int oidx1 = idx4 + idx3 + idx1;
                            int oidx2 = idx4 + idx3 + idx2;
                            int iidx1 = idx5 + idx3 + idx1;
                            int iidx2 = idx5 + idx3 + idx2;
                            double i1i = inar[iidx1 - 1];
                            double i1r = inar[iidx1];
                            double i2i = inar[iidx2 - 1];
                            double i2r = inar[iidx2];

                            outar[oidx1 - 1] = i1i - i2r;
                            outar[oidx2 - 1] = i1i + i2r;
                            outar[oidx1] = i1r + i2i;
                            outar[oidx2] = i1r - i2i;
                        }
                    }
                }
            }
            Array.Copy(outar, out_off, inar, in_off, idl1);
            for (int j = 1; j < ip; j++)
            {
                int idx1 = j * l1 * ido;
                for (int k = 0; k < l1; k++)
                {
                    int idx2 = k * ido + idx1;
                    inar[in_off + idx2] = outar[out_off + idx2];
                }
            }
            if (nbd <= l1)
            {
                is1 = -ido;
                for (int j = 1; j < ip; j++)
                {
                    is1 += ido;
                    idij = is1 - 1;
                    int idx1 = j * l1 * ido;
                    for (int i = 2; i < ido; i += 2)
                    {
                        idij += 2;
                        int idx2 = idij + iw1;
                        w1r = wtable_r[idx2 - 1];
                        w1i = wtable_r[idx2];
                        int idx4 = in_off + i;
                        int idx5 = out_off + i;
                        for (int k = 0; k < l1; k++)
                        {
                            int idx3 = k * ido + idx1;
                            int iidx1 = idx4 + idx3;
                            int oidx1 = idx5 + idx3;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];

                            inar[iidx1 - 1] = w1r * o1i - w1i * o1r;
                            inar[iidx1] = w1r * o1r + w1i * o1i;
                        }
                    }
                }
            }
            else
            {
                is1 = -ido;
                for (int j = 1; j < ip; j++)
                {
                    is1 += ido;
                    int idx1 = j * l1 * ido;
                    for (int k = 0; k < l1; k++)
                    {
                        idij = is1 - 1;
                        int idx3 = k * ido + idx1;
                        for (int i = 2; i < ido; i += 2)
                        {
                            idij += 2;
                            int idx2 = idij + iw1;
                            w1r = wtable_r[idx2 - 1];
                            w1i = wtable_r[idx2];
                            int idx4 = in_off + i;
                            int idx5 = out_off + i;
                            int iidx1 = idx4 + idx3;
                            int oidx1 = idx5 + idx3;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];

                            inar[iidx1 - 1] = w1r * o1i - w1i * o1r;
                            inar[iidx1] = w1r * o1r + w1i * o1i;

                        }
                    }
                }
            }
        }

        /*---------------------------------------------------------
         radbg: Real FFT's backward processing of general factor
         --------------------------------------------------------*/
        void radbg(long ido, long ip, long l1, long idl1, DoubleLargeArray inla, long in_off, DoubleLargeArray outla, long out_off, long offset)
        {
            long idij, ipph, j2, ic, jc, lc, is1;
            double dc2, ai1, ai2, ar1, ar2, ds2, w1r, w1i;
            long nbd;
            double dcp, arg, dsp, ar1h, ar2h;
            long iw1 = offset;

            arg = TWO_PI / (double)ip;
            dcp = System.Math.Cos(arg);
            dsp = System.Math.Sin(arg);
            nbd = (ido - 1) / 2;
            ipph = (ip + 1) / 2;
            long idx0 = ip * ido;
            if (ido >= l1)
            {
                for (long k = 0; k < l1; k++)
                {
                    long idx1 = k * ido;
                    long idx2 = k * idx0;
                    for (long i = 0; i < ido; i++)
                    {
                        outla[out_off + i + idx1] = inla[in_off + i + idx2];
                    }
                }
            }
            else
            {
                for (long i = 0; i < ido; i++)
                {
                    long idx1 = out_off + i;
                    long idx2 = in_off + i;
                    for (long k = 0; k < l1; k++)
                    {
                        outla[idx1 + k * ido] = inla[idx2 + k * idx0];
                    }
                }
            }
            long iidx0 = in_off + ido - 1;
            for (long j = 1; j < ipph; j++)
            {
                jc = ip - j;
                j2 = 2 * j;
                long idx1 = j * l1 * ido;
                long idx2 = jc * l1 * ido;
                long idx3 = j2 * ido;
                for (long k = 0; k < l1; k++)
                {
                    long idx4 = k * ido;
                    long idx5 = idx4 * ip;
                    long iidx1 = iidx0 + idx3 + idx5 - ido;
                    long iidx2 = in_off + idx3 + idx5;
                    double i1r = inla[iidx1];
                    double i2r = inla[iidx2];

                    outla[out_off + idx4 + idx1] = i1r + i1r;
                    outla[out_off + idx4 + idx2] = i2r + i2r;
                }
            }

            if (ido != 1)
            {
                if (nbd >= l1)
                {
                    for (long j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        long idx1 = j * l1 * ido;
                        long idx2 = jc * l1 * ido;
                        long idx3 = 2 * j * ido;
                        for (long k = 0; k < l1; k++)
                        {
                            long idx4 = k * ido + idx1;
                            long idx5 = k * ido + idx2;
                            long idx6 = k * ip * ido + idx3;
                            for (long i = 2; i < ido; i += 2)
                            {
                                ic = ido - i;
                                long idx7 = out_off + i;
                                long idx8 = in_off + ic;
                                long idx9 = in_off + i;
                                long oidx1 = idx7 + idx4;
                                long oidx2 = idx7 + idx5;
                                long iidx1 = idx9 + idx6;
                                long iidx2 = idx8 + idx6 - ido;
                                double a1i = inla[iidx1 - 1];
                                double a1r = inla[iidx1];
                                double a2i = inla[iidx2 - 1];
                                double a2r = inla[iidx2];

                                outla[oidx1 - 1] = a1i + a2i;
                                outla[oidx2 - 1] = a1i - a2i;
                                outla[oidx1] = a1r - a2r;
                                outla[oidx2] = a1r + a2r;
                            }
                        }
                    }
                }
                else
                {
                    for (long j = 1; j < ipph; j++)
                    {
                        jc = ip - j;
                        long idx1 = j * l1 * ido;
                        long idx2 = jc * l1 * ido;
                        long idx3 = 2 * j * ido;
                        for (long i = 2; i < ido; i += 2)
                        {
                            ic = ido - i;
                            long idx7 = out_off + i;
                            long idx8 = in_off + ic;
                            long idx9 = in_off + i;
                            for (long k = 0; k < l1; k++)
                            {
                                long idx4 = k * ido + idx1;
                                long idx5 = k * ido + idx2;
                                long idx6 = k * ip * ido + idx3;
                                long oidx1 = idx7 + idx4;
                                long oidx2 = idx7 + idx5;
                                long iidx1 = idx9 + idx6;
                                long iidx2 = idx8 + idx6 - ido;
                                double a1i = inla[iidx1 - 1];
                                double a1r = inla[iidx1];
                                double a2i = inla[iidx2 - 1];
                                double a2r = inla[iidx2];

                                outla[oidx1 - 1] = a1i + a2i;
                                outla[oidx2 - 1] = a1i - a2i;
                                outla[oidx1] = a1r - a2r;
                                outla[oidx2] = a1r + a2r;
                            }
                        }
                    }
                }
            }

            ar1 = 1;
            ai1 = 0;
            long idx01 = (ip - 1) * idl1;
            for (long l = 1; l < ipph; l++)
            {
                lc = ip - l;
                ar1h = dcp * ar1 - dsp * ai1;
                ai1 = dcp * ai1 + dsp * ar1;
                ar1 = ar1h;
                long idx1 = l * idl1;
                long idx2 = lc * idl1;
                for (long ik = 0; ik < idl1; ik++)
                {
                    long idx3 = in_off + ik;
                    long idx4 = out_off + ik;
                    inla[idx3 + idx1] = outla[idx4] + ar1 * outla[idx4 + idl1];
                    inla[idx3 + idx2] = ai1 * outla[idx4 + idx01];
                }
                dc2 = ar1;
                ds2 = ai1;
                ar2 = ar1;
                ai2 = ai1;
                for (long j = 2; j < ipph; j++)
                {
                    jc = ip - j;
                    ar2h = dc2 * ar2 - ds2 * ai2;
                    ai2 = dc2 * ai2 + ds2 * ar2;
                    ar2 = ar2h;
                    long idx5 = j * idl1;
                    long idx6 = jc * idl1;
                    for (long ik = 0; ik < idl1; ik++)
                    {
                        long idx7 = in_off + ik;
                        long idx8 = out_off + ik;
                        inla[idx7 + idx1] = inla[idx7 + idx1] + ar2 * outla[idx8 + idx5];
                        inla[idx7 + idx2] = inla[idx7 + idx2] + ai2 * outla[idx8 + idx6];
                    }
                }
            }
            for (long j = 1; j < ipph; j++)
            {
                long idx1 = j * idl1;
                for (long ik = 0; ik < idl1; ik++)
                {
                    long idx2 = out_off + ik;
                    outla[idx2] = outla[idx2] + outla[idx2 + idx1];
                }
            }
            for (long j = 1; j < ipph; j++)
            {
                jc = ip - j;
                long idx1 = j * l1 * ido;
                long idx2 = jc * l1 * ido;
                for (long k = 0; k < l1; k++)
                {
                    long idx3 = k * ido;
                    long oidx1 = out_off + idx3;
                    long iidx1 = in_off + idx3 + idx1;
                    long iidx2 = in_off + idx3 + idx2;
                    double i1r = inla[iidx1];
                    double i2r = inla[iidx2];

                    outla[oidx1 + idx1] = i1r - i2r;
                    outla[oidx1 + idx2] = i1r + i2r;
                }
            }

            if (ido == 1)
            {
                return;
            }
            if (nbd >= l1)
            {
                for (long j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    long idx1 = j * l1 * ido;
                    long idx2 = jc * l1 * ido;
                    for (long k = 0; k < l1; k++)
                    {
                        long idx3 = k * ido;
                        for (long i = 2; i < ido; i += 2)
                        {
                            long idx4 = out_off + i;
                            long idx5 = in_off + i;
                            long oidx1 = idx4 + idx3 + idx1;
                            long oidx2 = idx4 + idx3 + idx2;
                            long iidx1 = idx5 + idx3 + idx1;
                            long iidx2 = idx5 + idx3 + idx2;
                            double i1i = inla[iidx1 - 1];
                            double i1r = inla[iidx1];
                            double i2i = inla[iidx2 - 1];
                            double i2r = inla[iidx2];

                            outla[oidx1 - 1] = i1i - i2r;
                            outla[oidx2 - 1] = i1i + i2r;
                            outla[oidx1] = i1r + i2i;
                            outla[oidx2] = i1r - i2i;
                        }
                    }
                }
            }
            else
            {
                for (long j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    long idx1 = j * l1 * ido;
                    long idx2 = jc * l1 * ido;
                    for (long i = 2; i < ido; i += 2)
                    {
                        long idx4 = out_off + i;
                        long idx5 = in_off + i;
                        for (long k = 0; k < l1; k++)
                        {
                            long idx3 = k * ido;
                            long oidx1 = idx4 + idx3 + idx1;
                            long oidx2 = idx4 + idx3 + idx2;
                            long iidx1 = idx5 + idx3 + idx1;
                            long iidx2 = idx5 + idx3 + idx2;
                            double i1i = inla[iidx1 - 1];
                            double i1r = inla[iidx1];
                            double i2i = inla[iidx2 - 1];
                            double i2r = inla[iidx2];

                            outla[oidx1 - 1] = i1i - i2r;
                            outla[oidx2 - 1] = i1i + i2r;
                            outla[oidx1] = i1r + i2i;
                            outla[oidx2] = i1r - i2i;
                        }
                    }
                }
            }
            DoubleLargeArray.ArrayCopy(outla, out_off, inla, in_off, idl1);
            for (long j = 1; j < ip; j++)
            {
                long idx1 = j * l1 * ido;
                for (long k = 0; k < l1; k++)
                {
                    long idx2 = k * ido + idx1;
                    inla[in_off + idx2] = outla[out_off + idx2];
                }
            }
            if (nbd <= l1)
            {
                is1 = -ido;
                for (long j = 1; j < ip; j++)
                {
                    is1 += ido;
                    idij = is1 - 1;
                    long idx1 = j * l1 * ido;
                    for (long i = 2; i < ido; i += 2)
                    {
                        idij += 2;
                        long idx2 = idij + iw1;
                        w1r = wtable_rl[idx2 - 1];
                        w1i = wtable_rl[idx2];
                        long idx4 = in_off + i;
                        long idx5 = out_off + i;
                        for (long k = 0; k < l1; k++)
                        {
                            long idx3 = k * ido + idx1;
                            long iidx1 = idx4 + idx3;
                            long oidx1 = idx5 + idx3;
                            double o1i = outla[oidx1 - 1];
                            double o1r = outla[oidx1];

                            inla[iidx1 - 1] = (w1r * o1i - w1i * o1r);
                            inla[iidx1] = (w1r * o1r + w1i * o1i);
                        }
                    }
                }
            }
            else
            {
                is1 = -ido;
                for (long j = 1; j < ip; j++)
                {
                    is1 += ido;
                    long idx1 = j * l1 * ido;
                    for (long k = 0; k < l1; k++)
                    {
                        idij = is1 - 1;
                        long idx3 = k * ido + idx1;
                        for (long i = 2; i < ido; i += 2)
                        {
                            idij += 2;
                            long idx2 = idij + iw1;
                            w1r = wtable_rl[idx2 - 1];
                            w1i = wtable_rl[idx2];
                            long idx4 = in_off + i;
                            long idx5 = out_off + i;
                            long iidx1 = idx4 + idx3;
                            long oidx1 = idx5 + idx3;
                            double o1i = outla[oidx1 - 1];
                            double o1r = outla[oidx1];

                            inla[iidx1 - 1] = (w1r * o1i - w1i * o1r);
                            inla[iidx1] = (w1r * o1r + w1i * o1i);

                        }
                    }
                }
            }
        }

        /*---------------------------------------------------------
         cfftf1: further processing of Complex forward FFT
         --------------------------------------------------------*/
        void cfftf(double[] a, int offa, int isign)
        {
            int idot;
            int l1, l2;
            int na, nf, ipll, iw, ido, idl1;
            int[] nac = new int[1];
            int twon = 2 * n;

            int iw1, iw2;
            double[] ch = new double[twon];

            iw1 = twon;
            iw2 = 4 * n;
            nac[0] = 0;
            nf = (int)wtable[1 + iw2];
            na = 0;
            l1 = 1;
            iw = iw1;
            for (int k1 = 2; k1 <= nf + 1; k1++)
            {
                ipll = (int)wtable[k1 + iw2];
                l2 = ipll * l1;
                ido = n / l2;
                idot = ido + ido;
                idl1 = idot * l1;
                switch (ipll)
                {
                    case 4:
                        if (na == 0)
                        {
                            passf4(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf4(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    case 2:
                        if (na == 0)
                        {
                            passf2(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf2(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    case 3:
                        if (na == 0)
                        {
                            passf3(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf3(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    case 5:
                        if (na == 0)
                        {
                            passf5(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf5(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    default:
                        if (na == 0)
                        {
                            passfg(nac, idot, ipll, l1, idl1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passfg(nac, idot, ipll, l1, idl1, ch, 0, a, offa, iw, isign);
                        }
                        if (nac[0] != 0)
                        {
                            na = 1 - na;
                        }
                        break;
                }
                l1 = l2;
                iw += (ipll - 1) * idot;
            }
            if (na == 0)
            {
                return;
            }
            Array.Copy(ch, 0, a, offa, twon);

        }

        /*---------------------------------------------------------
         cfftf1: further processing of Complex forward FFT
         --------------------------------------------------------*/
        void cfftf(DoubleLargeArray a, long offa, int isign)
        {
            long idot;
            long l1, l2;
            long na, nf, iw, ido, idl1;
            int[] nac = new int[1];
            long twon = 2 * nl;
            int ipll;

            long iw1, iw2;
            DoubleLargeArray ch = new DoubleLargeArray(twon);

            iw1 = twon;
            iw2 = 4 * nl;
            nac[0] = 0;
            nf = (long)wtablel[1 + iw2];
            na = 0;
            l1 = 1;
            iw = iw1;
            for (long k1 = 2; k1 <= nf + 1; k1++)
            {
                ipll = (int)wtablel[k1 + iw2];
                l2 = ipll * l1;
                ido = nl / l2;
                idot = ido + ido;
                idl1 = idot * l1;
                switch (ipll)
                {
                    case 4:
                        if (na == 0)
                        {
                            passf4(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf4(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    case 2:
                        if (na == 0)
                        {
                            passf2(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf2(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    case 3:
                        if (na == 0)
                        {
                            passf3(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf3(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    case 5:
                        if (na == 0)
                        {
                            passf5(idot, l1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passf5(idot, l1, ch, 0, a, offa, iw, isign);
                        }
                        na = 1 - na;
                        break;
                    default:
                        if (na == 0)
                        {
                            passfg(nac, idot, ipll, l1, idl1, a, offa, ch, 0, iw, isign);
                        }
                        else
                        {
                            passfg(nac, idot, ipll, l1, idl1, ch, 0, a, offa, iw, isign);
                        }
                        if (nac[0] != 0)
                        {
                            na = 1 - na;
                        }
                        break;
                }
                l1 = l2;
                iw += (ipll - 1) * idot;
            }
            if (na == 0)
            {
                return;
            }
            DoubleLargeArray.ArrayCopy(ch, 0, a, offa, twon);

        }

        /*----------------------------------------------------------------------
         passf2: Complex FFT's forward/backward processing of factor 2;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf2(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset, int isign)
        {
            double t1i, t1r;
            int iw1;
            iw1 = offset;
            int idx = ido * l1;
            if (ido <= 2)
            {
                for (int k = 0; k < l1; k++)
                {
                    int idx0 = k * ido;
                    int iidx1 = in_off + 2 * idx0;
                    int iidx2 = iidx1 + ido;
                    double a1r = inar[iidx1];
                    double a1i = inar[iidx1 + 1];
                    double a2r = inar[iidx2];
                    double a2i = inar[iidx2 + 1];

                    int oidx1 = out_off + idx0;
                    int oidx2 = oidx1 + idx;
                    outar[oidx1] = a1r + a2r;
                    outar[oidx1 + 1] = a1i + a2i;
                    outar[oidx2] = a1r - a2r;
                    outar[oidx2 + 1] = a1i - a2i;
                }
            }
            else
            {
                for (int k = 0; k < l1; k++)
                {
                    for (int i = 0; i < ido - 1; i += 2)
                    {
                        int idx0 = k * ido;
                        int iidx1 = in_off + i + 2 * idx0;
                        int iidx2 = iidx1 + ido;
                        double i1r = inar[iidx1];
                        double i1i = inar[iidx1 + 1];
                        double i2r = inar[iidx2];
                        double i2i = inar[iidx2 + 1];

                        int widx1 = i + iw1;
                        double w1r = wtable[widx1];
                        double w1i = isign * wtable[widx1 + 1];

                        t1r = i1r - i2r;
                        t1i = i1i - i2i;

                        int oidx1 = out_off + i + idx0;
                        int oidx2 = oidx1 + idx;
                        outar[oidx1] = i1r + i2r;
                        outar[oidx1 + 1] = i1i + i2i;
                        outar[oidx2] = w1r * t1r - w1i * t1i;
                        outar[oidx2 + 1] = w1r * t1i + w1i * t1r;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf2: Complex FFT's forward/backward processing of factor 2;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf2(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset, long isign)
        {
            double t1i, t1r;
            long iw1;
            iw1 = offset;
            long idx = ido * l1;
            if (ido <= 2)
            {
                for (long k = 0; k < l1; k++)
                {
                    long idx0 = k * ido;
                    long iidx1 = in_off + 2 * idx0;
                    long iidx2 = iidx1 + ido;
                    double a1r = inar[iidx1];
                    double a1i = inar[iidx1 + 1];
                    double a2r = inar[iidx2];
                    double a2i = inar[iidx2 + 1];

                    long oidx1 = out_off + idx0;
                    long oidx2 = oidx1 + idx;
                    outar[oidx1] = a1r + a2r;
                    outar[oidx1 + 1] = a1i + a2i;
                    outar[oidx2] = a1r - a2r;
                    outar[oidx2 + 1] = a1i - a2i;
                }
            }
            else
            {
                for (long k = 0; k < l1; k++)
                {
                    for (long i = 0; i < ido - 1; i += 2)
                    {
                        long idx0 = k * ido;
                        long iidx1 = in_off + i + 2 * idx0;
                        long iidx2 = iidx1 + ido;
                        double i1r = inar[iidx1];
                        double i1i = inar[iidx1 + 1];
                        double i2r = inar[iidx2];
                        double i2i = inar[iidx2 + 1];

                        long widx1 = i + iw1;
                        double w1r = wtablel[widx1];
                        double w1i = isign * wtablel[widx1 + 1];

                        t1r = i1r - i2r;
                        t1i = i1i - i2i;

                        long oidx1 = out_off + i + idx0;
                        long oidx2 = oidx1 + idx;
                        outar[oidx1] = i1r + i2r;
                        outar[oidx1 + 1] = i1i + i2i;
                        outar[oidx2] = w1r * t1r - w1i * t1i;
                        outar[oidx2 + 1] = w1r * t1i + w1i * t1r;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf3: Complex FFT's forward/backward processing of factor 3;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf3(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset, int isign)
        {
            double taur = -0.5;
            double taui = 0.866025403784438707610604524234076962;
            double ci2, ci3, di2, di3, cr2, cr3, dr2, dr3, ti2, tr2;
            int iw1, iw2;

            iw1 = offset;
            iw2 = iw1 + ido;

            int idxt = l1 * ido;

            if (ido == 2)
            {
                for (int k = 1; k <= l1; k++)
                {
                    int iidx1 = in_off + (3 * k - 2) * ido;
                    int iidx2 = iidx1 + ido;
                    int iidx3 = iidx1 - ido;
                    double i1r = inar[iidx1];
                    double i1i = inar[iidx1 + 1];
                    double i2r = inar[iidx2];
                    double i2i = inar[iidx2 + 1];
                    double i3r = inar[iidx3];
                    double i3i = inar[iidx3 + 1];

                    tr2 = i1r + i2r;
                    cr2 = i3r + taur * tr2;
                    ti2 = i1i + i2i;
                    ci2 = i3i + taur * ti2;
                    cr3 = isign * taui * (i1r - i2r);
                    ci3 = isign * taui * (i1i - i2i);

                    int oidx1 = out_off + (k - 1) * ido;
                    int oidx2 = oidx1 + idxt;
                    int oidx3 = oidx2 + idxt;
                    outar[oidx1] = inar[iidx3] + tr2;
                    outar[oidx1 + 1] = i3i + ti2;
                    outar[oidx2] = cr2 - ci3;
                    outar[oidx2 + 1] = ci2 + cr3;
                    outar[oidx3] = cr2 + ci3;
                    outar[oidx3 + 1] = ci2 - cr3;
                }
            }
            else
            {
                for (int k = 1; k <= l1; k++)
                {
                    int idx1 = in_off + (3 * k - 2) * ido;
                    int idx2 = out_off + (k - 1) * ido;
                    for (int i = 0; i < ido - 1; i += 2)
                    {
                        int iidx1 = i + idx1;
                        int iidx2 = iidx1 + ido;
                        int iidx3 = iidx1 - ido;
                        double a1r = inar[iidx1];
                        double a1i = inar[iidx1 + 1];
                        double a2r = inar[iidx2];
                        double a2i = inar[iidx2 + 1];
                        double a3r = inar[iidx3];
                        double a3i = inar[iidx3 + 1];

                        tr2 = a1r + a2r;
                        cr2 = a3r + taur * tr2;
                        ti2 = a1i + a2i;
                        ci2 = a3i + taur * ti2;
                        cr3 = isign * taui * (a1r - a2r);
                        ci3 = isign * taui * (a1i - a2i);
                        dr2 = cr2 - ci3;
                        dr3 = cr2 + ci3;
                        di2 = ci2 + cr3;
                        di3 = ci2 - cr3;

                        int widx1 = i + iw1;
                        int widx2 = i + iw2;
                        double w1r = wtable[widx1];
                        double w1i = isign * wtable[widx1 + 1];
                        double w2r = wtable[widx2];
                        double w2i = isign * wtable[widx2 + 1];

                        int oidx1 = i + idx2;
                        int oidx2 = oidx1 + idxt;
                        int oidx3 = oidx2 + idxt;
                        outar[oidx1] = a3r + tr2;
                        outar[oidx1 + 1] = a3i + ti2;
                        outar[oidx2] = w1r * dr2 - w1i * di2;
                        outar[oidx2 + 1] = w1r * di2 + w1i * dr2;
                        outar[oidx3] = w2r * dr3 - w2i * di3;
                        outar[oidx3 + 1] = w2r * di3 + w2i * dr3;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf3: Complex FFT's forward/backward processing of factor 3;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf3(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset, long isign)
        {
            double taur = -0.5;
            double taui = 0.866025403784438707610604524234076962;
            double ci2, ci3, di2, di3, cr2, cr3, dr2, dr3, ti2, tr2;
            long iw1, iw2;

            iw1 = offset;
            iw2 = iw1 + ido;

            long idxt = l1 * ido;

            if (ido == 2)
            {
                for (long k = 1; k <= l1; k++)
                {
                    long iidx1 = in_off + (3 * k - 2) * ido;
                    long iidx2 = iidx1 + ido;
                    long iidx3 = iidx1 - ido;
                    double i1r = inar[iidx1];
                    double i1i = inar[iidx1 + 1];
                    double i2r = inar[iidx2];
                    double i2i = inar[iidx2 + 1];
                    double i3r = inar[iidx3];
                    double i3i = inar[iidx3 + 1];

                    tr2 = i1r + i2r;
                    cr2 = i3r + taur * tr2;
                    ti2 = i1i + i2i;
                    ci2 = i3i + taur * ti2;
                    cr3 = isign * taui * (i1r - i2r);
                    ci3 = isign * taui * (i1i - i2i);

                    long oidx1 = out_off + (k - 1) * ido;
                    long oidx2 = oidx1 + idxt;
                    long oidx3 = oidx2 + idxt;
                    outar[oidx1] = inar[iidx3] + tr2;
                    outar[oidx1 + 1] = i3i + ti2;
                    outar[oidx2] = cr2 - ci3;
                    outar[oidx2 + 1] = ci2 + cr3;
                    outar[oidx3] = cr2 + ci3;
                    outar[oidx3 + 1] = ci2 - cr3;
                }
            }
            else
            {
                for (long k = 1; k <= l1; k++)
                {
                    long idx1 = in_off + (3 * k - 2) * ido;
                    long idx2 = out_off + (k - 1) * ido;
                    for (long i = 0; i < ido - 1; i += 2)
                    {
                        long iidx1 = i + idx1;
                        long iidx2 = iidx1 + ido;
                        long iidx3 = iidx1 - ido;
                        double a1r = inar[iidx1];
                        double a1i = inar[iidx1 + 1];
                        double a2r = inar[iidx2];
                        double a2i = inar[iidx2 + 1];
                        double a3r = inar[iidx3];
                        double a3i = inar[iidx3 + 1];

                        tr2 = a1r + a2r;
                        cr2 = a3r + taur * tr2;
                        ti2 = a1i + a2i;
                        ci2 = a3i + taur * ti2;
                        cr3 = isign * taui * (a1r - a2r);
                        ci3 = isign * taui * (a1i - a2i);
                        dr2 = cr2 - ci3;
                        dr3 = cr2 + ci3;
                        di2 = ci2 + cr3;
                        di3 = ci2 - cr3;

                        long widx1 = i + iw1;
                        long widx2 = i + iw2;
                        double w1r = wtablel[widx1];
                        double w1i = isign * wtablel[widx1 + 1];
                        double w2r = wtablel[widx2];
                        double w2i = isign * wtablel[widx2 + 1];

                        long oidx1 = i + idx2;
                        long oidx2 = oidx1 + idxt;
                        long oidx3 = oidx2 + idxt;
                        outar[oidx1] = a3r + tr2;
                        outar[oidx1 + 1] = a3i + ti2;
                        outar[oidx2] = w1r * dr2 - w1i * di2;
                        outar[oidx2 + 1] = w1r * di2 + w1i * dr2;
                        outar[oidx3] = w2r * dr3 - w2i * di3;
                        outar[oidx3 + 1] = w2r * di3 + w2i * dr3;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf4: Complex FFT's forward/backward processing of factor 4;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf4(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset, int isign)
        {
            double ci2, ci3, ci4, cr2, cr3, cr4, ti1, ti2, ti3, ti4, tr1, tr2, tr3, tr4;
            int iw1, iw2, iw3;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;

            int idx0 = l1 * ido;
            if (ido == 2)
            {
                for (int k = 0; k < l1; k++)
                {
                    int idxt1 = k * ido;
                    int iidx1 = in_off + 4 * idxt1 + 1;
                    int iidx2 = iidx1 + ido;
                    int iidx3 = iidx2 + ido;
                    int iidx4 = iidx3 + ido;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];

                    ti1 = i1r - i3r;
                    ti2 = i1r + i3r;
                    tr4 = i4r - i2r;
                    ti3 = i2r + i4r;
                    tr1 = i1i - i3i;
                    tr2 = i1i + i3i;
                    ti4 = i2i - i4i;
                    tr3 = i2i + i4i;

                    int oidx1 = out_off + idxt1;
                    int oidx2 = oidx1 + idx0;
                    int oidx3 = oidx2 + idx0;
                    int oidx4 = oidx3 + idx0;
                    outar[oidx1] = tr2 + tr3;
                    outar[oidx1 + 1] = ti2 + ti3;
                    outar[oidx2] = tr1 + isign * tr4;
                    outar[oidx2 + 1] = ti1 + isign * ti4;
                    outar[oidx3] = tr2 - tr3;
                    outar[oidx3 + 1] = ti2 - ti3;
                    outar[oidx4] = tr1 - isign * tr4;
                    outar[oidx4 + 1] = ti1 - isign * ti4;
                }
            }
            else
            {
                for (int k = 0; k < l1; k++)
                {
                    int idx1 = k * ido;
                    int idx2 = in_off + 1 + 4 * idx1;
                    for (int i = 0; i < ido - 1; i += 2)
                    {
                        int iidx1 = i + idx2;
                        int iidx2 = iidx1 + ido;
                        int iidx3 = iidx2 + ido;
                        int iidx4 = iidx3 + ido;
                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];

                        ti1 = i1r - i3r;
                        ti2 = i1r + i3r;
                        ti3 = i2r + i4r;
                        tr4 = i4r - i2r;
                        tr1 = i1i - i3i;
                        tr2 = i1i + i3i;
                        ti4 = i2i - i4i;
                        tr3 = i2i + i4i;
                        cr3 = tr2 - tr3;
                        ci3 = ti2 - ti3;
                        cr2 = tr1 + isign * tr4;
                        cr4 = tr1 - isign * tr4;
                        ci2 = ti1 + isign * ti4;
                        ci4 = ti1 - isign * ti4;

                        int widx1 = i + iw1;
                        int widx2 = i + iw2;
                        int widx3 = i + iw3;
                        double w1r = wtable[widx1];
                        double w1i = isign * wtable[widx1 + 1];
                        double w2r = wtable[widx2];
                        double w2i = isign * wtable[widx2 + 1];
                        double w3r = wtable[widx3];
                        double w3i = isign * wtable[widx3 + 1];

                        int oidx1 = out_off + i + idx1;
                        int oidx2 = oidx1 + idx0;
                        int oidx3 = oidx2 + idx0;
                        int oidx4 = oidx3 + idx0;
                        outar[oidx1] = tr2 + tr3;
                        outar[oidx1 + 1] = ti2 + ti3;
                        outar[oidx2] = w1r * cr2 - w1i * ci2;
                        outar[oidx2 + 1] = w1r * ci2 + w1i * cr2;
                        outar[oidx3] = w2r * cr3 - w2i * ci3;
                        outar[oidx3 + 1] = w2r * ci3 + w2i * cr3;
                        outar[oidx4] = w3r * cr4 - w3i * ci4;
                        outar[oidx4 + 1] = w3r * ci4 + w3i * cr4;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf4: Complex FFT's forward/backward processing of factor 4;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf4(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset, int isign)
        {
            double ci2, ci3, ci4, cr2, cr3, cr4, ti1, ti2, ti3, ti4, tr1, tr2, tr3, tr4;
            long iw1, iw2, iw3;
            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;

            long idx0 = l1 * ido;
            if (ido == 2)
            {
                for (long k = 0; k < l1; k++)
                {
                    long idxt1 = k * ido;
                    long iidx1 = in_off + 4 * idxt1 + 1;
                    long iidx2 = iidx1 + ido;
                    long iidx3 = iidx2 + ido;
                    long iidx4 = iidx3 + ido;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];

                    ti1 = i1r - i3r;
                    ti2 = i1r + i3r;
                    tr4 = i4r - i2r;
                    ti3 = i2r + i4r;
                    tr1 = i1i - i3i;
                    tr2 = i1i + i3i;
                    ti4 = i2i - i4i;
                    tr3 = i2i + i4i;

                    long oidx1 = out_off + idxt1;
                    long oidx2 = oidx1 + idx0;
                    long oidx3 = oidx2 + idx0;
                    long oidx4 = oidx3 + idx0;
                    outar[oidx1] = tr2 + tr3;
                    outar[oidx1 + 1] = ti2 + ti3;
                    outar[oidx2] = tr1 + isign * tr4;
                    outar[oidx2 + 1] = ti1 + isign * ti4;
                    outar[oidx3] = tr2 - tr3;
                    outar[oidx3 + 1] = ti2 - ti3;
                    outar[oidx4] = tr1 - isign * tr4;
                    outar[oidx4 + 1] = ti1 - isign * ti4;
                }
            }
            else
            {
                for (long k = 0; k < l1; k++)
                {
                    long idx1 = k * ido;
                    long idx2 = in_off + 1 + 4 * idx1;
                    for (long i = 0; i < ido - 1; i += 2)
                    {
                        long iidx1 = i + idx2;
                        long iidx2 = iidx1 + ido;
                        long iidx3 = iidx2 + ido;
                        long iidx4 = iidx3 + ido;
                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];

                        ti1 = i1r - i3r;
                        ti2 = i1r + i3r;
                        ti3 = i2r + i4r;
                        tr4 = i4r - i2r;
                        tr1 = i1i - i3i;
                        tr2 = i1i + i3i;
                        ti4 = i2i - i4i;
                        tr3 = i2i + i4i;
                        cr3 = tr2 - tr3;
                        ci3 = ti2 - ti3;
                        cr2 = tr1 + isign * tr4;
                        cr4 = tr1 - isign * tr4;
                        ci2 = ti1 + isign * ti4;
                        ci4 = ti1 - isign * ti4;

                        long widx1 = i + iw1;
                        long widx2 = i + iw2;
                        long widx3 = i + iw3;
                        double w1r = wtablel[widx1];
                        double w1i = isign * wtablel[widx1 + 1];
                        double w2r = wtablel[widx2];
                        double w2i = isign * wtablel[widx2 + 1];
                        double w3r = wtablel[widx3];
                        double w3i = isign * wtablel[widx3 + 1];

                        long oidx1 = out_off + i + idx1;
                        long oidx2 = oidx1 + idx0;
                        long oidx3 = oidx2 + idx0;
                        long oidx4 = oidx3 + idx0;
                        outar[oidx1] = tr2 + tr3;
                        outar[oidx1 + 1] = ti2 + ti3;
                        outar[oidx2] = w1r * cr2 - w1i * ci2;
                        outar[oidx2 + 1] = w1r * ci2 + w1i * cr2;
                        outar[oidx3] = w2r * cr3 - w2i * ci3;
                        outar[oidx3 + 1] = w2r * ci3 + w2i * cr3;
                        outar[oidx4] = w3r * cr4 - w3i * ci4;
                        outar[oidx4 + 1] = w3r * ci4 + w3i * cr4;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf5: Complex FFT's forward/backward processing of factor 5;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf5(int ido, int l1, double[] inar, int in_off, double[] outar, int out_off, int offset, int isign) /* isign==-1 for forward transform and+1 for backward transform */
        {
            double tr11 = 0.309016994374947451262869435595348477;
            double ti11 = 0.951056516295153531181938433292089030;
            double tr12 = -0.809016994374947340240566973079694435;
            double ti12 = 0.587785252292473248125759255344746634;
            double ci2, ci3, ci4, ci5, di3, di4, di5, di2, cr2, cr3, cr5, cr4, ti2, ti3, ti4, ti5, dr3, dr4, dr5, dr2, tr2, tr3, tr4, tr5;
            int iw1, iw2, iw3, iw4;

            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;
            iw4 = iw3 + ido;

            int idx0 = l1 * ido;

            if (ido == 2)
            {
                for (int k = 1; k <= l1; ++k)
                {
                    int iidx1 = in_off + (5 * k - 4) * ido + 1;
                    int iidx2 = iidx1 + ido;
                    int iidx3 = iidx1 - ido;
                    int iidx4 = iidx2 + ido;
                    int iidx5 = iidx4 + ido;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];
                    double i5i = inar[iidx5 - 1];
                    double i5r = inar[iidx5];

                    ti5 = i1r - i5r;
                    ti2 = i1r + i5r;
                    ti4 = i2r - i4r;
                    ti3 = i2r + i4r;
                    tr5 = i1i - i5i;
                    tr2 = i1i + i5i;
                    tr4 = i2i - i4i;
                    tr3 = i2i + i4i;
                    cr2 = i3i + tr11 * tr2 + tr12 * tr3;
                    ci2 = i3r + tr11 * ti2 + tr12 * ti3;
                    cr3 = i3i + tr12 * tr2 + tr11 * tr3;
                    ci3 = i3r + tr12 * ti2 + tr11 * ti3;
                    cr5 = isign * (ti11 * tr5 + ti12 * tr4);
                    ci5 = isign * (ti11 * ti5 + ti12 * ti4);
                    cr4 = isign * (ti12 * tr5 - ti11 * tr4);
                    ci4 = isign * (ti12 * ti5 - ti11 * ti4);

                    int oidx1 = out_off + (k - 1) * ido;
                    int oidx2 = oidx1 + idx0;
                    int oidx3 = oidx2 + idx0;
                    int oidx4 = oidx3 + idx0;
                    int oidx5 = oidx4 + idx0;
                    outar[oidx1] = i3i + tr2 + tr3;
                    outar[oidx1 + 1] = i3r + ti2 + ti3;
                    outar[oidx2] = cr2 - ci5;
                    outar[oidx2 + 1] = ci2 + cr5;
                    outar[oidx3] = cr3 - ci4;
                    outar[oidx3 + 1] = ci3 + cr4;
                    outar[oidx4] = cr3 + ci4;
                    outar[oidx4 + 1] = ci3 - cr4;
                    outar[oidx5] = cr2 + ci5;
                    outar[oidx5 + 1] = ci2 - cr5;
                }
            }
            else
            {
                for (int k = 1; k <= l1; k++)
                {
                    int idx1 = in_off + 1 + (k * 5 - 4) * ido;
                    int idx2 = out_off + (k - 1) * ido;
                    for (int i = 0; i < ido - 1; i += 2)
                    {
                        int iidx1 = i + idx1;
                        int iidx2 = iidx1 + ido;
                        int iidx3 = iidx1 - ido;
                        int iidx4 = iidx2 + ido;
                        int iidx5 = iidx4 + ido;
                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];
                        double i5i = inar[iidx5 - 1];
                        double i5r = inar[iidx5];

                        ti5 = i1r - i5r;
                        ti2 = i1r + i5r;
                        ti4 = i2r - i4r;
                        ti3 = i2r + i4r;
                        tr5 = i1i - i5i;
                        tr2 = i1i + i5i;
                        tr4 = i2i - i4i;
                        tr3 = i2i + i4i;
                        cr2 = i3i + tr11 * tr2 + tr12 * tr3;
                        ci2 = i3r + tr11 * ti2 + tr12 * ti3;
                        cr3 = i3i + tr12 * tr2 + tr11 * tr3;
                        ci3 = i3r + tr12 * ti2 + tr11 * ti3;
                        cr5 = isign * (ti11 * tr5 + ti12 * tr4);
                        ci5 = isign * (ti11 * ti5 + ti12 * ti4);
                        cr4 = isign * (ti12 * tr5 - ti11 * tr4);
                        ci4 = isign * (ti12 * ti5 - ti11 * ti4);
                        dr3 = cr3 - ci4;
                        dr4 = cr3 + ci4;
                        di3 = ci3 + cr4;
                        di4 = ci3 - cr4;
                        dr5 = cr2 + ci5;
                        dr2 = cr2 - ci5;
                        di5 = ci2 - cr5;
                        di2 = ci2 + cr5;

                        int widx1 = i + iw1;
                        int widx2 = i + iw2;
                        int widx3 = i + iw3;
                        int widx4 = i + iw4;
                        double w1r = wtable[widx1];
                        double w1i = isign * wtable[widx1 + 1];
                        double w2r = wtable[widx2];
                        double w2i = isign * wtable[widx2 + 1];
                        double w3r = wtable[widx3];
                        double w3i = isign * wtable[widx3 + 1];
                        double w4r = wtable[widx4];
                        double w4i = isign * wtable[widx4 + 1];

                        int oidx1 = i + idx2;
                        int oidx2 = oidx1 + idx0;
                        int oidx3 = oidx2 + idx0;
                        int oidx4 = oidx3 + idx0;
                        int oidx5 = oidx4 + idx0;
                        outar[oidx1] = i3i + tr2 + tr3;
                        outar[oidx1 + 1] = i3r + ti2 + ti3;
                        outar[oidx2] = w1r * dr2 - w1i * di2;
                        outar[oidx2 + 1] = w1r * di2 + w1i * dr2;
                        outar[oidx3] = w2r * dr3 - w2i * di3;
                        outar[oidx3 + 1] = w2r * di3 + w2i * dr3;
                        outar[oidx4] = w3r * dr4 - w3i * di4;
                        outar[oidx4 + 1] = w3r * di4 + w3i * dr4;
                        outar[oidx5] = w4r * dr5 - w4i * di5;
                        outar[oidx5 + 1] = w4r * di5 + w4i * dr5;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passf5: Complex FFT's forward/backward processing of factor 5;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passf5(long ido, long l1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset, long isign) /* isign==-1 for forward transform and+1 for backward transform */
        {
            double tr11 = 0.309016994374947451262869435595348477;
            double ti11 = 0.951056516295153531181938433292089030;
            double tr12 = -0.809016994374947340240566973079694435;
            double ti12 = 0.587785252292473248125759255344746634;
            double ci2, ci3, ci4, ci5, di3, di4, di5, di2, cr2, cr3, cr5, cr4, ti2, ti3, ti4, ti5, dr3, dr4, dr5, dr2, tr2, tr3, tr4, tr5;
            long iw1, iw2, iw3, iw4;

            iw1 = offset;
            iw2 = iw1 + ido;
            iw3 = iw2 + ido;
            iw4 = iw3 + ido;

            long idx0 = l1 * ido;

            if (ido == 2)
            {
                for (long k = 1; k <= l1; ++k)
                {
                    long iidx1 = in_off + (5 * k - 4) * ido + 1;
                    long iidx2 = iidx1 + ido;
                    long iidx3 = iidx1 - ido;
                    long iidx4 = iidx2 + ido;
                    long iidx5 = iidx4 + ido;

                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];
                    double i3i = inar[iidx3 - 1];
                    double i3r = inar[iidx3];
                    double i4i = inar[iidx4 - 1];
                    double i4r = inar[iidx4];
                    double i5i = inar[iidx5 - 1];
                    double i5r = inar[iidx5];

                    ti5 = i1r - i5r;
                    ti2 = i1r + i5r;
                    ti4 = i2r - i4r;
                    ti3 = i2r + i4r;
                    tr5 = i1i - i5i;
                    tr2 = i1i + i5i;
                    tr4 = i2i - i4i;
                    tr3 = i2i + i4i;
                    cr2 = i3i + tr11 * tr2 + tr12 * tr3;
                    ci2 = i3r + tr11 * ti2 + tr12 * ti3;
                    cr3 = i3i + tr12 * tr2 + tr11 * tr3;
                    ci3 = i3r + tr12 * ti2 + tr11 * ti3;
                    cr5 = isign * (ti11 * tr5 + ti12 * tr4);
                    ci5 = isign * (ti11 * ti5 + ti12 * ti4);
                    cr4 = isign * (ti12 * tr5 - ti11 * tr4);
                    ci4 = isign * (ti12 * ti5 - ti11 * ti4);

                    long oidx1 = out_off + (k - 1) * ido;
                    long oidx2 = oidx1 + idx0;
                    long oidx3 = oidx2 + idx0;
                    long oidx4 = oidx3 + idx0;
                    long oidx5 = oidx4 + idx0;
                    outar[oidx1] = i3i + tr2 + tr3;
                    outar[oidx1 + 1] = i3r + ti2 + ti3;
                    outar[oidx2] = cr2 - ci5;
                    outar[oidx2 + 1] = ci2 + cr5;
                    outar[oidx3] = cr3 - ci4;
                    outar[oidx3 + 1] = ci3 + cr4;
                    outar[oidx4] = cr3 + ci4;
                    outar[oidx4 + 1] = ci3 - cr4;
                    outar[oidx5] = cr2 + ci5;
                    outar[oidx5 + 1] = ci2 - cr5;
                }
            }
            else
            {
                for (long k = 1; k <= l1; k++)
                {
                    long idx1 = in_off + 1 + (k * 5 - 4) * ido;
                    long idx2 = out_off + (k - 1) * ido;
                    for (long i = 0; i < ido - 1; i += 2)
                    {
                        long iidx1 = i + idx1;
                        long iidx2 = iidx1 + ido;
                        long iidx3 = iidx1 - ido;
                        long iidx4 = iidx2 + ido;
                        long iidx5 = iidx4 + ido;
                        double i1i = inar[iidx1 - 1];
                        double i1r = inar[iidx1];
                        double i2i = inar[iidx2 - 1];
                        double i2r = inar[iidx2];
                        double i3i = inar[iidx3 - 1];
                        double i3r = inar[iidx3];
                        double i4i = inar[iidx4 - 1];
                        double i4r = inar[iidx4];
                        double i5i = inar[iidx5 - 1];
                        double i5r = inar[iidx5];

                        ti5 = i1r - i5r;
                        ti2 = i1r + i5r;
                        ti4 = i2r - i4r;
                        ti3 = i2r + i4r;
                        tr5 = i1i - i5i;
                        tr2 = i1i + i5i;
                        tr4 = i2i - i4i;
                        tr3 = i2i + i4i;
                        cr2 = i3i + tr11 * tr2 + tr12 * tr3;
                        ci2 = i3r + tr11 * ti2 + tr12 * ti3;
                        cr3 = i3i + tr12 * tr2 + tr11 * tr3;
                        ci3 = i3r + tr12 * ti2 + tr11 * ti3;
                        cr5 = isign * (ti11 * tr5 + ti12 * tr4);
                        ci5 = isign * (ti11 * ti5 + ti12 * ti4);
                        cr4 = isign * (ti12 * tr5 - ti11 * tr4);
                        ci4 = isign * (ti12 * ti5 - ti11 * ti4);
                        dr3 = cr3 - ci4;
                        dr4 = cr3 + ci4;
                        di3 = ci3 + cr4;
                        di4 = ci3 - cr4;
                        dr5 = cr2 + ci5;
                        dr2 = cr2 - ci5;
                        di5 = ci2 - cr5;
                        di2 = ci2 + cr5;

                        long widx1 = i + iw1;
                        long widx2 = i + iw2;
                        long widx3 = i + iw3;
                        long widx4 = i + iw4;
                        double w1r = wtablel[widx1];
                        double w1i = isign * wtablel[widx1 + 1];
                        double w2r = wtablel[widx2];
                        double w2i = isign * wtablel[widx2 + 1];
                        double w3r = wtablel[widx3];
                        double w3i = isign * wtablel[widx3 + 1];
                        double w4r = wtablel[widx4];
                        double w4i = isign * wtablel[widx4 + 1];

                        long oidx1 = i + idx2;
                        long oidx2 = oidx1 + idx0;
                        long oidx3 = oidx2 + idx0;
                        long oidx4 = oidx3 + idx0;
                        long oidx5 = oidx4 + idx0;
                        outar[oidx1] = i3i + tr2 + tr3;
                        outar[oidx1 + 1] = i3r + ti2 + ti3;
                        outar[oidx2] = w1r * dr2 - w1i * di2;
                        outar[oidx2 + 1] = w1r * di2 + w1i * dr2;
                        outar[oidx3] = w2r * dr3 - w2i * di3;
                        outar[oidx3 + 1] = w2r * di3 + w2i * dr3;
                        outar[oidx4] = w3r * dr4 - w3i * di4;
                        outar[oidx4 + 1] = w3r * di4 + w3i * dr4;
                        outar[oidx5] = w4r * dr5 - w4i * di5;
                        outar[oidx5 + 1] = w4r * di5 + w4i * dr5;
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passfg: Complex FFT's forward/backward processing of general factor;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passfg(int[] nac, int ido, int ip, int l1, int idl1, double[] inar, int in_off, double[] outar, int out_off, int offset, int isign)
        {
            int idij, idlj, idot, ipph, l, jc, lc, idj, idl, inc, idp;
            double w1r, w1i, w2i, w2r;
            int iw1;

            iw1 = offset;
            idot = ido / 2;
            ipph = (ip + 1) / 2;
            idp = ip * ido;
            if (ido >= l1)
            {
                for (int j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    int idx1 = j * ido;
                    int idx2 = jc * ido;
                    for (int k = 0; k < l1; k++)
                    {
                        int idx3 = k * ido;
                        int idx4 = idx3 + idx1 * l1;
                        int idx5 = idx3 + idx2 * l1;
                        int idx6 = idx3 * ip;
                        for (int i = 0; i < ido; i++)
                        {
                            int oidx1 = out_off + i;
                            double i1r = inar[in_off + i + idx1 + idx6];
                            double i2r = inar[in_off + i + idx2 + idx6];
                            outar[oidx1 + idx4] = i1r + i2r;
                            outar[oidx1 + idx5] = i1r - i2r;
                        }
                    }
                }
                for (int k = 0; k < l1; k++)
                {
                    int idxt1 = k * ido;
                    int idxt2 = idxt1 * ip;
                    for (int i = 0; i < ido; i++)
                    {
                        outar[out_off + i + idxt1] = inar[in_off + i + idxt2];
                    }
                }
            }
            else
            {
                for (int j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    int idxt1 = j * l1 * ido;
                    int idxt2 = jc * l1 * ido;
                    int idxt3 = j * ido;
                    int idxt4 = jc * ido;
                    for (int i = 0; i < ido; i++)
                    {
                        for (int k = 0; k < l1; k++)
                        {
                            int idx1 = k * ido;
                            int idx2 = idx1 * ip;
                            int idx3 = out_off + i;
                            int idx4 = in_off + i;
                            double i1r = inar[idx4 + idxt3 + idx2];
                            double i2r = inar[idx4 + idxt4 + idx2];
                            outar[idx3 + idx1 + idxt1] = i1r + i2r;
                            outar[idx3 + idx1 + idxt2] = i1r - i2r;
                        }
                    }
                }
                for (int i = 0; i < ido; i++)
                {
                    for (int k = 0; k < l1; k++)
                    {
                        int idx1 = k * ido;
                        outar[out_off + i + idx1] = inar[in_off + i + idx1 * ip];
                    }
                }
            }

            idl = 2 - ido;
            inc = 0;
            int idxt0 = (ip - 1) * idl1;
            for (l = 1; l < ipph; l++)
            {
                lc = ip - l;
                idl += ido;
                int idxt1 = l * idl1;
                int idxt2 = lc * idl1;
                int idxt3 = idl + iw1;
                w1r = wtable[idxt3 - 2];
                w1i = isign * wtable[idxt3 - 1];
                for (int ik = 0; ik < idl1; ik++)
                {
                    int idx1 = in_off + ik;
                    int idx2 = out_off + ik;
                    inar[idx1 + idxt1] = outar[idx2] + w1r * outar[idx2 + idl1];
                    inar[idx1 + idxt2] = w1i * outar[idx2 + idxt0];
                }
                idlj = idl;
                inc += ido;
                for (int j = 2; j < ipph; j++)
                {
                    jc = ip - j;
                    idlj += inc;
                    if (idlj > idp)
                    {
                        idlj -= idp;
                    }
                    int idxt4 = idlj + iw1;
                    w2r = wtable[idxt4 - 2];
                    w2i = isign * wtable[idxt4 - 1];
                    int idxt5 = j * idl1;
                    int idxt6 = jc * idl1;
                    for (int ik = 0; ik < idl1; ik++)
                    {
                        int idx1 = in_off + ik;
                        int idx2 = out_off + ik;
                        inar[idx1 + idxt1] += w2r * outar[idx2 + idxt5];
                        inar[idx1 + idxt2] += w2i * outar[idx2 + idxt6];
                    }
                }
            }
            for (int j = 1; j < ipph; j++)
            {
                int idxt1 = j * idl1;
                for (int ik = 0; ik < idl1; ik++)
                {
                    int idx1 = out_off + ik;
                    outar[idx1] += outar[idx1 + idxt1];
                }
            }
            for (int j = 1; j < ipph; j++)
            {
                jc = ip - j;
                int idx1 = j * idl1;
                int idx2 = jc * idl1;
                for (int ik = 1; ik < idl1; ik += 2)
                {
                    int idx3 = out_off + ik;
                    int idx4 = in_off + ik;
                    int iidx1 = idx4 + idx1;
                    int iidx2 = idx4 + idx2;
                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];

                    int oidx1 = idx3 + idx1;
                    int oidx2 = idx3 + idx2;
                    outar[oidx1 - 1] = i1i - i2r;
                    outar[oidx2 - 1] = i1i + i2r;
                    outar[oidx1] = i1r + i2i;
                    outar[oidx2] = i1r - i2i;
                }
            }
            nac[0] = 1;
            if (ido == 2)
            {
                return;
            }
            nac[0] = 0;
            Array.Copy(outar, out_off, inar, in_off, idl1);
            int idx0 = l1 * ido;
            for (int j = 1; j < ip; j++)
            {
                int idx1 = j * idx0;
                for (int k = 0; k < l1; k++)
                {
                    int idx2 = k * ido;
                    int oidx1 = out_off + idx2 + idx1;
                    int iidx1 = in_off + idx2 + idx1;
                    inar[iidx1] = outar[oidx1];
                    inar[iidx1 + 1] = outar[oidx1 + 1];
                }
            }
            if (idot <= l1)
            {
                idij = 0;
                for (int j = 1; j < ip; j++)
                {
                    idij += 2;
                    int idx1 = j * l1 * ido;
                    for (int i = 3; i < ido; i += 2)
                    {
                        idij += 2;
                        int idx2 = idij + iw1 - 1;
                        w1r = wtable[idx2 - 1];
                        w1i = isign * wtable[idx2];
                        int idx3 = in_off + i;
                        int idx4 = out_off + i;
                        for (int k = 0; k < l1; k++)
                        {
                            int idx5 = k * ido + idx1;
                            int iidx1 = idx3 + idx5;
                            int oidx1 = idx4 + idx5;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            inar[iidx1 - 1] = w1r * o1i - w1i * o1r;
                            inar[iidx1] = w1r * o1r + w1i * o1i;
                        }
                    }
                }
            }
            else
            {
                idj = 2 - ido;
                for (int j = 1; j < ip; j++)
                {
                    idj += ido;
                    int idx1 = j * l1 * ido;
                    for (int k = 0; k < l1; k++)
                    {
                        idij = idj;
                        int idx3 = k * ido + idx1;
                        for (int i = 3; i < ido; i += 2)
                        {
                            idij += 2;
                            int idx2 = idij - 1 + iw1;
                            w1r = wtable[idx2 - 1];
                            w1i = isign * wtable[idx2];
                            int iidx1 = in_off + i + idx3;
                            int oidx1 = out_off + i + idx3;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            inar[iidx1 - 1] = w1r * o1i - w1i * o1r;
                            inar[iidx1] = w1r * o1r + w1i * o1i;
                        }
                    }
                }
            }
        }

        /*----------------------------------------------------------------------
         passfg: Complex FFT's forward/backward processing of general factor;
         isign is1 +1 for backward and -1 for forward transforms
         ----------------------------------------------------------------------*/
        void passfg(int[] nac, long ido, long ip, long l1, long idl1, DoubleLargeArray inar, long in_off, DoubleLargeArray outar, long out_off, long offset, long isign)
        {
            long idij, idlj, idot, ipph, l, jc, lc, idj, idl, inc, idp;
            double w1r, w1i, w2i, w2r;
            long iw1;

            iw1 = offset;
            idot = ido / 2;
            ipph = (ip + 1) / 2;
            idp = ip * ido;
            if (ido >= l1)
            {
                for (long j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    long idx1 = j * ido;
                    long idx2 = jc * ido;
                    for (long k = 0; k < l1; k++)
                    {
                        long idx3 = k * ido;
                        long idx4 = idx3 + idx1 * l1;
                        long idx5 = idx3 + idx2 * l1;
                        long idx6 = idx3 * ip;
                        for (long i = 0; i < ido; i++)
                        {
                            long oidx1 = out_off + i;
                            double i1r = inar[in_off + i + idx1 + idx6];
                            double i2r = inar[in_off + i + idx2 + idx6];
                            outar[oidx1 + idx4] = i1r + i2r;
                            outar[oidx1 + idx5] = i1r - i2r;
                        }
                    }
                }
                for (long k = 0; k < l1; k++)
                {
                    long idxt1 = k * ido;
                    long idxt2 = idxt1 * ip;
                    for (long i = 0; i < ido; i++)
                    {
                        outar[out_off + i + idxt1] = inar[in_off + i + idxt2];
                    }
                }
            }
            else
            {
                for (long j = 1; j < ipph; j++)
                {
                    jc = ip - j;
                    long idxt1 = j * l1 * ido;
                    long idxt2 = jc * l1 * ido;
                    long idxt3 = j * ido;
                    long idxt4 = jc * ido;
                    for (long i = 0; i < ido; i++)
                    {
                        for (long k = 0; k < l1; k++)
                        {
                            long idx1 = k * ido;
                            long idx2 = idx1 * ip;
                            long idx3 = out_off + i;
                            long idx4 = in_off + i;
                            double i1r = inar[idx4 + idxt3 + idx2];
                            double i2r = inar[idx4 + idxt4 + idx2];
                            outar[idx3 + idx1 + idxt1] = i1r + i2r;
                            outar[idx3 + idx1 + idxt2] = i1r - i2r;
                        }
                    }
                }
                for (long i = 0; i < ido; i++)
                {
                    for (long k = 0; k < l1; k++)
                    {
                        long idx1 = k * ido;
                        outar[out_off + i + idx1] = inar[in_off + i + idx1 * ip];
                    }
                }
            }

            idl = 2 - ido;
            inc = 0;
            long idxt0 = (ip - 1) * idl1;
            for (l = 1; l < ipph; l++)
            {
                lc = ip - l;
                idl += ido;
                long idxt1 = l * idl1;
                long idxt2 = lc * idl1;
                long idxt3 = idl + iw1;
                w1r = wtablel[idxt3 - 2];
                w1i = isign * wtablel[idxt3 - 1];
                for (long ik = 0; ik < idl1; ik++)
                {
                    long idx1 = in_off + ik;
                    long idx2 = out_off + ik;
                    inar[idx1 + idxt1] = outar[idx2] + w1r * outar[idx2 + idl1];
                    inar[idx1 + idxt2] = w1i * outar[idx2 + idxt0];
                }
                idlj = idl;
                inc += ido;
                for (long j = 2; j < ipph; j++)
                {
                    jc = ip - j;
                    idlj += inc;
                    if (idlj > idp)
                    {
                        idlj -= idp;
                    }
                    long idxt4 = idlj + iw1;
                    w2r = wtablel[idxt4 - 2];
                    w2i = isign * wtablel[idxt4 - 1];
                    long idxt5 = j * idl1;
                    long idxt6 = jc * idl1;
                    for (long ik = 0; ik < idl1; ik++)
                    {
                        long idx1 = in_off + ik;
                        long idx2 = out_off + ik;
                        inar[idx1 + idxt1] = inar[idx1 + idxt1] + w2r * outar[idx2 + idxt5];
                        inar[idx1 + idxt2] = inar[idx1 + idxt2] + w2i * outar[idx2 + idxt6];
                    }
                }
            }
            for (long j = 1; j < ipph; j++)
            {
                long idxt1 = j * idl1;
                for (long ik = 0; ik < idl1; ik++)
                {
                    long idx1 = out_off + ik;
                    outar[idx1] = outar[idx1] + outar[idx1 + idxt1];
                }
            }
            for (long j = 1; j < ipph; j++)
            {
                jc = ip - j;
                long idx1 = j * idl1;
                long idx2 = jc * idl1;
                for (long ik = 1; ik < idl1; ik += 2)
                {
                    long idx3 = out_off + ik;
                    long idx4 = in_off + ik;
                    long iidx1 = idx4 + idx1;
                    long iidx2 = idx4 + idx2;
                    double i1i = inar[iidx1 - 1];
                    double i1r = inar[iidx1];
                    double i2i = inar[iidx2 - 1];
                    double i2r = inar[iidx2];

                    long oidx1 = idx3 + idx1;
                    long oidx2 = idx3 + idx2;
                    outar[oidx1 - 1] = i1i - i2r;
                    outar[oidx2 - 1] = i1i + i2r;
                    outar[oidx1] = i1r + i2i;
                    outar[oidx2] = i1r - i2i;
                }
            }
            nac[0] = 1;
            if (ido == 2)
            {
                return;
            }
            nac[0] = 0;
            DoubleLargeArray.ArrayCopy(outar, out_off, inar, in_off, idl1);
            long idx0 = l1 * ido;
            for (long j = 1; j < ip; j++)
            {
                long idx1 = j * idx0;
                for (long k = 0; k < l1; k++)
                {
                    long idx2 = k * ido;
                    long oidx1 = out_off + idx2 + idx1;
                    long iidx1 = in_off + idx2 + idx1;
                    inar[iidx1] = (outar[oidx1]);
                    inar[iidx1 + 1] = outar[oidx1 + 1];
                }
            }
            if (idot <= l1)
            {
                idij = 0;
                for (long j = 1; j < ip; j++)
                {
                    idij += 2;
                    long idx1 = j * l1 * ido;
                    for (long i = 3; i < ido; i += 2)
                    {
                        idij += 2;
                        long idx2 = idij + iw1 - 1;
                        w1r = wtablel[idx2 - 1];
                        w1i = isign * wtablel[idx2];
                        long idx3 = in_off + i;
                        long idx4 = out_off + i;
                        for (long k = 0; k < l1; k++)
                        {
                            long idx5 = k * ido + idx1;
                            long iidx1 = idx3 + idx5;
                            long oidx1 = idx4 + idx5;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            inar[iidx1 - 1] = (w1r * o1i - w1i * o1r);
                            inar[iidx1] = (w1r * o1r + w1i * o1i);
                        }
                    }
                }
            }
            else
            {
                idj = 2 - ido;
                for (long j = 1; j < ip; j++)
                {
                    idj += ido;
                    long idx1 = j * l1 * ido;
                    for (long k = 0; k < l1; k++)
                    {
                        idij = idj;
                        long idx3 = k * ido + idx1;
                        for (long i = 3; i < ido; i += 2)
                        {
                            idij += 2;
                            long idx2 = idij - 1 + iw1;
                            w1r = wtablel[idx2 - 1];
                            w1i = isign * wtablel[idx2];
                            long iidx1 = in_off + i + idx3;
                            long oidx1 = out_off + i + idx3;
                            double o1i = outar[oidx1 - 1];
                            double o1r = outar[oidx1];
                            inar[iidx1 - 1] = (w1r * o1i - w1i * o1r);
                            inar[iidx1] = (w1r * o1r + w1i * o1i);
                        }
                    }
                }
            }
        }
    }
}
