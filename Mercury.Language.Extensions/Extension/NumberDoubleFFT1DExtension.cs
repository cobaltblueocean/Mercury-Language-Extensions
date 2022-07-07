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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Log;
using Mercury.Language.Exception;

namespace System
{
    /// <summary>
    /// NumberDoubleFFT1DExtension Description
    /// </summary>
    public static class NumberDoubleFFT1DExtension
    {

        private static long THREADS_BEGIN_N_1D_FFT_2THREADS = 8192;

        private static long THREADS_BEGIN_N_1D_FFT_4THREADS = 65536;

        public static void cftbsub(this int n, ref double[] a, int offa, ref int[] ip, int nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftbsub(this int n, ref double[] a, long offa, ref long[] ip, long nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftbsub(this long n, ref double[] a, long offa, ref int[] ip, long nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftbsub(this long n, ref double[] a, long offa, ref long[] ip, long nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftbsub(this long n, ref DoubleLargeArray a, int offa, ref int[] ip, int nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftbsub(this long n, ref DoubleLargeArray a, long offa, ref long[] ip, long nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftbsub(this long n, ref DoubleLargeArray a, long offa, ref LongLargeArray ip, long nw, ref DoubleLargeArray w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftb1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2conj(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216neg(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208neg(offa);
                }
            }
            else if (n == 8)
            {
                a.cftb040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void bitrv2(this int n, ref int[] ip, ref double[] a, int offa)
        {
            int j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            int idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
        }

        public static void bitrv2(this long n, ref long[] ip, ref double[] a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
        }

        public static void bitrv2(this long n, ref long[] ip, ref DoubleLargeArray a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
        }

        public static void bitrv2(this long n, ref int[] ip, ref DoubleLargeArray a, int offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
        }

        public static void bitrv2(this long n, ref LongLargeArray ip, ref double[] a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
        }

        public static void bitrv2(this long n, ref LongLargeArray ip, ref DoubleLargeArray a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = a[idx1 + 1];
                        yr = a[idx2];
                        yi = a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = a[idx1 + 1];
                    yr = a[idx2];
                    yi = a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                }
            }
        }

        public static void bitrv2conj(this int n, ref int[] ip, ref double[] a, int offa)
        {
            int j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            int idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void bitrv2conj(this long n, ref int[] ip, ref double[] a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void bitrv2conj(this long n, ref int[] ip, ref DoubleLargeArray a, int offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void bitrv2conj(this long n, ref long[] ip, ref double[] a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void bitrv2conj(this long n, ref long[] ip, ref DoubleLargeArray a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void bitrv2conj(this long n, ref LongLargeArray ip, ref double[] a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (long k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (long j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void bitrv2conj(this long n, ref LongLargeArray ip, ref DoubleLargeArray a, long offa)
        {
            long j1, k1, l, m, nh, nm;
            double xr, xi, yr, yi;
            long idx0, idx1, idx2;

            m = 1;
            for (l = n >> 2; l > 8; l >>= 2)
            {
                m <<= 1;
            }
            nh = n >> 1;
            nm = 4 * m;
            if (l == 8)
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + 2 * ip[m + k];
                        k1 = idx0 + 2 * ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= 2 * nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + 2 * ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += 2 * nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nm;
                    k1 -= nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= 2;
                    k1 -= nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 += nh + 2;
                    k1 += nh + 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    j1 -= nh - nm;
                    k1 += 2 * nm - 2;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
            else
            {
                for (int k = 0; k < m; k++)
                {
                    idx0 = 4 * k;
                    for (int j = 0; j < k; j++)
                    {
                        j1 = 4 * j + ip[m + k];
                        k1 = idx0 + ip[m + j];
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nh;
                        k1 += 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += 2;
                        k1 += nh;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 += nm;
                        k1 += nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nh;
                        k1 -= 2;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                        j1 -= nm;
                        k1 -= nm;
                        idx1 = offa + j1;
                        idx2 = offa + k1;
                        xr = a[idx1];
                        xi = -a[idx1 + 1];
                        yr = a[idx2];
                        yi = -a[idx2 + 1];
                        a[idx1] = yr;
                        a[idx1 + 1] = yi;
                        a[idx2] = xr;
                        a[idx2 + 1] = xi;
                    }
                    k1 = idx0 + ip[m + k];
                    j1 = k1 + 2;
                    k1 += nh;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                    j1 += nm;
                    k1 += nm;
                    idx1 = offa + j1;
                    idx2 = offa + k1;
                    a[idx1 - 1] = -a[idx1 - 1];
                    xr = a[idx1];
                    xi = -a[idx1 + 1];
                    yr = a[idx2];
                    yi = -a[idx2 + 1];
                    a[idx1] = yr;
                    a[idx1 + 1] = yi;
                    a[idx2] = xr;
                    a[idx2 + 1] = xi;
                    a[idx2 + 3] = -a[idx2 + 3];
                }
            }
        }

        public static void cftb1st(this int n, ref double[] a, int offa, ref double[] w, int startw)
        {
            int j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            int idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;

            x0r = a[offa] + a[idx2];
            x0i = -a[offa + 1] - a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = -a[offa + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            a[idx2] = x1r + x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r - x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = -a[idx5 + 1] - a[idx2 + 1];
                x1r = a[idx5] - a[offa + j2];
                x1i = -a[idx5 + 1] + a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = -a[idx5 + 3] - a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = -a[idx5 + 3] + a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i - x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = -a[idx0 + 1] - a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = -a[idx0 + 1] + a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = -a[idx0 - 1] - a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = -a[idx0 - 1] + a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i - x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 - 2] = wd3i * x0r + wd3r * x0i;
                a[idx3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = -a[idx0 - 1] - a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = -a[idx0 - 1] + a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i - x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = -a[idx0 + 1] - a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = -a[idx0 + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = -a[idx0 + 3] - a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = -a[idx0 + 3] + a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i - x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftb1st(this long n, ref double[] a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;

            x0r = a[offa] + a[idx2];
            x0i = -a[offa + 1] - a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = -a[offa + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            a[idx2] = x1r + x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r - x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = -a[idx5 + 1] - a[idx2 + 1];
                x1r = a[idx5] - a[offa + j2];
                x1i = -a[idx5 + 1] + a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = -a[idx5 + 3] - a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = -a[idx5 + 3] + a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i - x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = -a[idx0 + 1] - a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = -a[idx0 + 1] + a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = -a[idx0 - 1] - a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = -a[idx0 - 1] + a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i - x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 - 2] = wd3i * x0r + wd3r * x0i;
                a[idx3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = -a[idx0 - 1] - a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = -a[idx0 - 1] + a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i - x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = -a[idx0 + 1] - a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = -a[idx0 + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = -a[idx0 + 3] - a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = -a[idx0 + 3] + a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i - x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftb1st(this long n, ref DoubleLargeArray a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;

            x0r = a[offa] + a[idx2];
            x0i = -a[offa + 1] - a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = -a[offa + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            a[idx2] = x1r + x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r - x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = -a[idx5 + 1] - a[idx2 + 1];
                x1r = a[idx5] - a[offa + j2];
                x1i = -a[idx5 + 1] + a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = -a[idx5 + 3] - a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = -a[idx5 + 3] + a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i - x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = -a[idx0 + 1] - a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = -a[idx0 + 1] + a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = -a[idx0 - 1] - a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = -a[idx0 - 1] + a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i - x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 - 2] = wd3i * x0r + wd3r * x0i;
                a[idx3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = -a[idx0 - 1] - a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = -a[idx0 - 1] + a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i - x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = -a[idx0 + 1] - a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = -a[idx0 + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = -a[idx0 + 3] - a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = -a[idx0 + 3] + a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i - x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftb1st(this long n, ref DoubleLargeArray a, long offa, ref DoubleLargeArray w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;

            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;

            x0r = a[offa] + a[idx2];
            x0i = -a[offa + 1] - a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = -a[offa + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            a[idx2] = x1r + x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r - x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = -a[idx5 + 1] - a[idx2 + 1];
                x1r = a[idx5] - a[offa + j2];
                x1i = -a[idx5 + 1] + a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = -a[idx5 + 3] - a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = -a[idx5 + 3] + a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i - x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = -a[idx0 + 1] - a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = -a[idx0 + 1] + a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = -a[idx0 - 1] - a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = -a[idx0 - 1] + a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i - x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i - y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i + x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i + y2i;
                x0r = x1r + x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r + y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r - x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r - y3i;
                x0i = y1i - y3r;
                a[idx3 - 2] = wd3i * x0r + wd3r * x0i;
                a[idx3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = -a[idx0 - 1] - a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = -a[idx0 - 1] + a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i - x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = -a[idx0 + 1] - a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = -a[idx0 + 1] + a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i - x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = -a[idx0 + 3] - a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = -a[idx0 + 3] + a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i - x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i + x2i;
            x0r = x1r + x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r - x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftrec4_th(int n, ref double[] a, int offa, int nw, ref double[] w)
        {
            int i;
            int idiv4, m, nthreads;
            //int idx = 0;
            double[] a1, w1;

            a1 = a;
            w1 = w;

            nthreads = 2;
            idiv4 = 0;
            m = n >> 1;
            if (n >= THREADS_BEGIN_N_1D_FFT_4THREADS)
            {
                nthreads = 4;
                idiv4 = 1;
                m >>= 1;
            }
            Task[] taskArray = new Task[nthreads];
            int mf = m;
            for (i = 0; i < nthreads; i++)
            {
                int firstIdx = offa + i * m;
                if (i != idiv4)
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        int isplt, j, k, m1;
                        int idx1 = firstIdx + mf;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            cftmdl1(m1, ref a1, idx1 - m1, ref w1, nw - (m1 >> 1));
                        }
                        cftleaf(m1, 1, ref a1, idx1 - m1, nw, ref w1);
                        k = 0;
                        int idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
                else
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        int isplt, j, k, m1;
                        int idx1 = firstIdx + mf;
                        k = 1;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            k <<= 2;
                            cftmdl2(m1, ref a1, idx1 - m1, ref w1, nw - m1);
                        }
                        cftleaf(m1, 0, ref a1, idx1 - m1, nw, ref w1);
                        k >>= 1;
                        int idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
            }
            try
            {
                Task.WaitAll(taskArray);
                a = a1;
                w = w1;
            }
            catch (SystemException ex)
            {

                Logger.Error(ex.ToString());
            }
        }

        public static void cftrec4_th(long n, ref double[] a, long offa, long nw, ref double[] w)
        {
            long i;
            long idiv4, m, nthreads;
            //long idx = 0;
            double[] a1, w1;

            a1 = a;
            w1 = w;

            nthreads = 2;
            idiv4 = 0;
            m = n >> 1;
            if (n >= THREADS_BEGIN_N_1D_FFT_4THREADS)
            {
                nthreads = 4;
                idiv4 = 1;
                m >>= 1;
            }
            Task[] taskArray = new Task[nthreads];
            long mf = m;
            for (i = 0; i < nthreads; i++)
            {
                long firstIdx = offa + i * m;
                if (i != idiv4)
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        long isplt, j, k, m1;
                        long idx1 = firstIdx + mf;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            cftmdl1(m1, ref a1, idx1 - m1, ref w1, nw - (m1 >> 1));
                        }
                        cftleaf(m1, 1, ref a1, idx1 - m1, nw, ref w1);
                        k = 0;
                        long idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
                else
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        long isplt, j, k, m1;
                        long idx1 = firstIdx + mf;
                        k = 1;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            k <<= 2;
                            cftmdl2(m1, ref a1, idx1 - m1, ref w1, nw - m1);
                        }
                        cftleaf(m1, 0, ref a1, idx1 - m1, nw, ref w1);
                        k >>= 1;
                        long idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
            }
            try
            {
                Task.WaitAll(taskArray);
                a = a1;
                w = w1;
            }
            catch (SystemException ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        public static void cftrec4_th(long n, ref DoubleLargeArray a, long offa, long nw, ref double[] w)
        {
            long i;
            long idiv4, m, nthreads;
            //long idx = 0;
            DoubleLargeArray a1;
            double[] w1;

            a1 = a;
            w1 = w;

            nthreads = 2;
            idiv4 = 0;
            m = n >> 1;
            if (n >= THREADS_BEGIN_N_1D_FFT_4THREADS)
            {
                nthreads = 4;
                idiv4 = 1;
                m >>= 1;
            }
            Task[] taskArray = new Task[nthreads];
            long mf = m;
            for (i = 0; i < nthreads; i++)
            {
                long firstIdx = offa + i * m;
                if (i != idiv4)
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        long isplt, j, k, m1;
                        long idx1 = firstIdx + mf;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            cftmdl1(m1, ref a1, idx1 - m1, ref w1, nw - (m1 >> 1));
                        }
                        cftleaf(m1, 1, ref a1, idx1 - m1, nw, ref w1);
                        k = 0;
                        long idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
                else
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        long isplt, j, k, m1;
                        long idx1 = firstIdx + mf;
                        k = 1;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            k <<= 2;
                            cftmdl2(m1, ref a1, idx1 - m1, ref w1, nw - m1);
                        }
                        cftleaf(m1, 0, ref a1, idx1 - m1, nw, ref w1);
                        k >>= 1;
                        long idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
            }
            try
            {
                Task.WaitAll(taskArray);
                a = a1;
                w = w1;
            }
            catch (SystemException ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        public static void cftrec4_th(this long n, ref DoubleLargeArray a, long offa, long nw, ref DoubleLargeArray w)
        {
            long i;
            long idiv4, nthreads;
            //long idx = 0;
            long m;
            DoubleLargeArray a1, w1;

            a1 = a;
            w1 = w;

            nthreads = 2;
            idiv4 = 0;
            m = n >> 1;
            if (n >= THREADS_BEGIN_N_1D_FFT_4THREADS)
            {
                nthreads = 4;
                idiv4 = 1;
                m >>= 1;
            }
            Task[] taskArray = new Task[nthreads];
            long mf = m;
            for (i = 0; i < nthreads; i++)
            {
                long firstIdx = offa + i * m;
                if (i != idiv4)
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        long isplt, j, k, m1;
                        long idx1 = firstIdx + mf;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            cftmdl1(m1, ref a1, idx1 - m1, ref w1, nw - (m1 >> 1));
                        }
                        cftleaf(m1, 1, ref a1, idx1 - m1, nw, ref w1);
                        k = 0;
                        long idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
                else
                {
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        long isplt, j, k, m1;
                        long idx1 = firstIdx + mf;
                        k = 1;
                        m1 = n;
                        while (m1 > 512)
                        {
                            m1 >>= 2;
                            k <<= 2;
                            cftmdl2(m1, ref a1, idx1 - m1, ref w1, nw - m1);
                        }
                        cftleaf(m1, 0, ref a1, idx1 - m1, nw, ref w1);
                        k >>= 1;
                        long idx2 = firstIdx - m1;
                        for (j = mf - m1; j > 0; j -= m1)
                        {
                            k++;
                            isplt = cfttree(m1, j, k, ref a1, firstIdx, nw, ref w1);
                            cftleaf(m1, isplt, ref a1, idx2 + j, nw, ref w1);
                        }

                    });
                }
            }
            try
            {
                Task.WaitAll(taskArray);
                a = a1;
                w = w1;
            }
            catch (SystemException ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        public static void cftrec4(this int n, ref double[] a, int offa, int nw, ref double[] w)
        {
            int isplt, j, k, m;

            m = n;
            int idx1 = offa + n;
            while (m > 512)
            {
                m >>= 2;
                cftmdl1(m, ref a, idx1 - m, ref w, nw - (m >> 1));
            }
            cftleaf(m, 1, ref a, idx1 - m, nw, ref w);
            k = 0;
            int idx2 = offa - m;
            for (j = n - m; j > 0; j -= m)
            {
                k++;
                isplt = cfttree(m, j, k, ref a, offa, nw, ref w);
                cftleaf(m, isplt, ref a, idx2 + j, nw, ref w);
            }
        }

        public static void cftrec4(this long n, ref double[] a, long offa, long nw, ref double[] w)
        {
            long isplt, j, k, m;

            m = n;
            long idx1 = offa + n;
            while (m > 512)
            {
                m >>= 2;
                cftmdl1(m, ref a, idx1 - m, ref w, nw - (m >> 1));
            }
            cftleaf(m, 1, ref a, idx1 - m, nw, ref w);
            k = 0;
            long idx2 = offa - m;
            for (j = n - m; j > 0; j -= m)
            {
                k++;
                isplt = cfttree(m, j, k, ref a, offa, nw, ref w);
                cftleaf(m, isplt, ref a, idx2 + j, nw, ref w);
            }
        }

        public static void cftrec4(this long n, ref DoubleLargeArray a, long offa, long nw, ref double[] w)
        {
            long isplt, j, k, m;

            m = n;
            long idx1 = offa + n;
            while (m > 512)
            {
                m >>= 2;
                cftmdl1(m, ref a, idx1 - m, ref w, nw - (m >> 1));
            }
            cftleaf(m, 1, ref a, idx1 - m, nw, ref w);
            k = 0;
            long idx2 = offa - m;
            for (j = n - m; j > 0; j -= m)
            {
                k++;
                isplt = cfttree(m, j, k, ref a, offa, nw, ref w);
                cftleaf(m, isplt, ref a, idx2 + j, nw, ref w);
            }
        }

        public static void cftrec4(this long n, ref DoubleLargeArray a, long offa, long nw, ref DoubleLargeArray w)
        {
            long isplt, j, k, m;

            m = n;
            long idx1 = offa + n;
            while (m > 512)
            {
                m >>= 2;
                cftmdl1(m, ref a, idx1 - m, ref w, nw - (m >> 1));
            }
            cftleaf(m, 1, ref a, idx1 - m, nw, ref w);
            k = 0;
            long idx2 = offa - m;
            for (j = n - m; j > 0; j -= m)
            {
                k++;
                isplt = cfttree(m, j, k, ref a, offa, nw, ref w);
                cftleaf(m, isplt, ref a, idx2 + j, nw, ref w);
            }
        }

        public static void cftleaf(this int n, int isplt, ref double[] a, int offa, int nw, ref double[] w)
        {
            if (n == 512)
            {
                cftmdl1(128, ref a, offa, ref w, nw - 64);
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
                cftmdl2(128, ref a, offa + 128, ref w, nw - 128);
                a.cftf161(offa + 128, ref w, nw - 8);
                a.cftf162(offa + 160, ref w, nw - 32);
                a.cftf161(offa + 192, ref w, nw - 8);
                a.cftf162(offa + 224, ref w, nw - 32);
                cftmdl1(128, ref a, offa + 256, ref w, nw - 64);
                a.cftf161(offa + 256, ref w, nw - 8);
                a.cftf162(offa + 288, ref w, nw - 32);
                a.cftf161(offa + 320, ref w, nw - 8);
                a.cftf161(offa + 352, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(128, ref a, offa + 384, ref w, nw - 64);
                    a.cftf161(offa + 480, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(128, ref a, offa + 384, ref w, nw - 128);
                    a.cftf162(offa + 480, ref w, nw - 32);
                }
                a.cftf161(offa + 384, ref w, nw - 8);
                a.cftf162(offa + 416, ref w, nw - 32);
                a.cftf161(offa + 448, ref w, nw - 8);
            }
            else
            {
                cftmdl1(64, ref a, offa, ref w, nw - 32);
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
                cftmdl2(64, ref a, offa + 64, ref w, nw - 64);
                a.cftf081(offa + 64, ref w, nw - 8);
                a.cftf082(offa + 80, ref w, nw - 8);
                a.cftf081(offa + 96, ref w, nw - 8);
                a.cftf082(offa + 112, ref w, nw - 8);
                cftmdl1(64, ref a, offa + 128, ref w, nw - 32);
                a.cftf081(offa + 128, ref w, nw - 8);
                a.cftf082(offa + 144, ref w, nw - 8);
                a.cftf081(offa + 160, ref w, nw - 8);
                a.cftf081(offa + 176, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(64, ref a, offa + 192, ref w, nw - 32);
                    a.cftf081(offa + 240, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(64, ref a, offa + 192, ref w, nw - 64);
                    a.cftf082(offa + 240, ref w, nw - 8);
                }
                a.cftf081(offa + 192, ref w, nw - 8);
                a.cftf082(offa + 208, ref w, nw - 8);
                a.cftf081(offa + 224, ref w, nw - 8);
            }
        }

        public static void cftleaf(this long n, long isplt, ref double[] a, long offa, long nw, ref double[] w)
        {
            if (n == 512)
            {
                cftmdl1(128, ref a, offa, ref w, nw - 64);
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
                cftmdl2(128, ref a, offa + 128, ref w, nw - 128);
                a.cftf161(offa + 128, ref w, nw - 8);
                a.cftf162(offa + 160, ref w, nw - 32);
                a.cftf161(offa + 192, ref w, nw - 8);
                a.cftf162(offa + 224, ref w, nw - 32);
                cftmdl1(128, ref a, offa + 256, ref w, nw - 64);
                a.cftf161(offa + 256, ref w, nw - 8);
                a.cftf162(offa + 288, ref w, nw - 32);
                a.cftf161(offa + 320, ref w, nw - 8);
                a.cftf161(offa + 352, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(128, ref a, offa + 384, ref w, nw - 64);
                    a.cftf161(offa + 480, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(128, ref a, offa + 384, ref w, nw - 128);
                    a.cftf162(offa + 480, ref w, nw - 32);
                }
                a.cftf161(offa + 384, ref w, nw - 8);
                a.cftf162(offa + 416, ref w, nw - 32);
                a.cftf161(offa + 448, ref w, nw - 8);
            }
            else
            {
                cftmdl1(64, ref a, offa, ref w, nw - 32);
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
                cftmdl2(64, ref a, offa + 64, ref w, nw - 64);
                a.cftf081(offa + 64, ref w, nw - 8);
                a.cftf082(offa + 80, ref w, nw - 8);
                a.cftf081(offa + 96, ref w, nw - 8);
                a.cftf082(offa + 112, ref w, nw - 8);
                cftmdl1(64, ref a, offa + 128, ref w, nw - 32);
                a.cftf081(offa + 128, ref w, nw - 8);
                a.cftf082(offa + 144, ref w, nw - 8);
                a.cftf081(offa + 160, ref w, nw - 8);
                a.cftf081(offa + 176, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(64, ref a, offa + 192, ref w, nw - 32);
                    a.cftf081(offa + 240, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(64, ref a, offa + 192, ref w, nw - 64);
                    a.cftf082(offa + 240, ref w, nw - 8);
                }
                a.cftf081(offa + 192, ref w, nw - 8);
                a.cftf082(offa + 208, ref w, nw - 8);
                a.cftf081(offa + 224, ref w, nw - 8);
            }
        }

        public static void cftleaf(this long n, long isplt, ref DoubleLargeArray a, long offa, long nw, ref double[] w)
        {
            if (n == 512)
            {
                cftmdl1(128, ref a, offa, ref w, nw - 64);
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
                cftmdl2(128, ref a, offa + 128, ref w, nw - 128);
                a.cftf161(offa + 128, ref w, nw - 8);
                a.cftf162(offa + 160, ref w, nw - 32);
                a.cftf161(offa + 192, ref w, nw - 8);
                a.cftf162(offa + 224, ref w, nw - 32);
                cftmdl1(128, ref a, offa + 256, ref w, nw - 64);
                a.cftf161(offa + 256, ref w, nw - 8);
                a.cftf162(offa + 288, ref w, nw - 32);
                a.cftf161(offa + 320, ref w, nw - 8);
                a.cftf161(offa + 352, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(128, ref a, offa + 384, ref w, nw - 64);
                    a.cftf161(offa + 480, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(128, ref a, offa + 384, ref w, nw - 128);
                    a.cftf162(offa + 480, ref w, nw - 32);
                }
                a.cftf161(offa + 384, ref w, nw - 8);
                a.cftf162(offa + 416, ref w, nw - 32);
                a.cftf161(offa + 448, ref w, nw - 8);
            }
            else
            {
                cftmdl1(64, ref a, offa, ref w, nw - 32);
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
                cftmdl2(64, ref a, offa + 64, ref w, nw - 64);
                a.cftf081(offa + 64, ref w, nw - 8);
                a.cftf082(offa + 80, ref w, nw - 8);
                a.cftf081(offa + 96, ref w, nw - 8);
                a.cftf082(offa + 112, ref w, nw - 8);
                cftmdl1(64, ref a, offa + 128, ref w, nw - 32);
                a.cftf081(offa + 128, ref w, nw - 8);
                a.cftf082(offa + 144, ref w, nw - 8);
                a.cftf081(offa + 160, ref w, nw - 8);
                a.cftf081(offa + 176, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(64, ref a, offa + 192, ref w, nw - 32);
                    a.cftf081(offa + 240, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(64, ref a, offa + 192, ref w, nw - 64);
                    a.cftf082(offa + 240, ref w, nw - 8);
                }
                a.cftf081(offa + 192, ref w, nw - 8);
                a.cftf082(offa + 208, ref w, nw - 8);
                a.cftf081(offa + 224, ref w, nw - 8);
            }
        }

        public static void cftleaf(this long n, long isplt, ref DoubleLargeArray a, long offa, long nw, ref DoubleLargeArray w)
        {
            if (n == 512)
            {
                cftmdl1(128, ref a, offa, ref w, nw - 64);
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
                cftmdl2(128, ref a, offa + 128, ref w, nw - 128);
                a.cftf161(offa + 128, ref w, nw - 8);
                a.cftf162(offa + 160, ref w, nw - 32);
                a.cftf161(offa + 192, ref w, nw - 8);
                a.cftf162(offa + 224, ref w, nw - 32);
                cftmdl1(128, ref a, offa + 256, ref w, nw - 64);
                a.cftf161(offa + 256, ref w, nw - 8);
                a.cftf162(offa + 288, ref w, nw - 32);
                a.cftf161(offa + 320, ref w, nw - 8);
                a.cftf161(offa + 352, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(128, ref a, offa + 384, ref w, nw - 64);
                    a.cftf161(offa + 480, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(128, ref a, offa + 384, ref w, nw - 128);
                    a.cftf162(offa + 480, ref w, nw - 32);
                }
                a.cftf161(offa + 384, ref w, nw - 8);
                a.cftf162(offa + 416, ref w, nw - 32);
                a.cftf161(offa + 448, ref w, nw - 8);
            }
            else
            {
                cftmdl1(64, ref a, offa, ref w, nw - 32);
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
                cftmdl2(64, ref a, offa + 64, ref w, nw - 64);
                a.cftf081(offa + 64, ref w, nw - 8);
                a.cftf082(offa + 80, ref w, nw - 8);
                a.cftf081(offa + 96, ref w, nw - 8);
                a.cftf082(offa + 112, ref w, nw - 8);
                cftmdl1(64, ref a, offa + 128, ref w, nw - 32);
                a.cftf081(offa + 128, ref w, nw - 8);
                a.cftf082(offa + 144, ref w, nw - 8);
                a.cftf081(offa + 160, ref w, nw - 8);
                a.cftf081(offa + 176, ref w, nw - 8);
                if (isplt != 0)
                {
                    cftmdl1(64, ref a, offa + 192, ref w, nw - 32);
                    a.cftf081(offa + 240, ref w, nw - 8);
                }
                else
                {
                    cftmdl2(64, ref a, offa + 192, ref w, nw - 64);
                    a.cftf082(offa + 240, ref w, nw - 8);
                }
                a.cftf081(offa + 192, ref w, nw - 8);
                a.cftf082(offa + 208, ref w, nw - 8);
                a.cftf081(offa + 224, ref w, nw - 8);
            }
        }

        public static int cfttree(this int n, int j, int k, ref double[] a, int offa, int nw, ref double[] w)
        {
            int i, isplt, m;
            int idx1 = offa - n;
            if ((k & 3) != 0)
            {
                isplt = k & 1;
                if (isplt != 0)
                {
                    cftmdl1(n, ref a, idx1 + j, ref w, nw - (n >> 1));
                }
                else
                {
                    cftmdl2(n, ref a, idx1 + j, ref w, nw - n);
                }
            }
            else
            {
                m = n;
                for (i = k; (i & 3) == 0; i >>= 2)
                {
                    m <<= 2;
                }
                isplt = i & 1;
                int idx2 = offa + j;
                if (isplt != 0)
                {
                    while (m > 128)
                    {
                        cftmdl1(m, ref a, idx2 - m, ref w, nw - (m >> 1));
                        m >>= 2;
                    }
                }
                else
                {
                    while (m > 128)
                    {
                        cftmdl2(m, ref a, idx2 - m, ref w, nw - m);
                        m >>= 2;
                    }
                }
            }
            return isplt;
        }

        public static long cfttree(this long n, long j, long k, ref double[] a, long offa, long nw, ref double[] w)
        {
            long i, isplt, m;
            long idx1 = offa - n;
            if ((k & 3) != 0)
            {
                isplt = k & 1;
                if (isplt != 0)
                {
                    cftmdl1(n, ref a, idx1 + j, ref w, nw - (n >> 1));
                }
                else
                {
                    cftmdl2(n, ref a, idx1 + j, ref w, nw - n);
                }
            }
            else
            {
                m = n;
                for (i = k; (i & 3) == 0; i >>= 2)
                {
                    m <<= 2;
                }
                isplt = i & 1;
                long idx2 = offa + j;
                if (isplt != 0)
                {
                    while (m > 128)
                    {
                        cftmdl1(m, ref a, idx2 - m, ref w, nw - (m >> 1));
                        m >>= 2;
                    }
                }
                else
                {
                    while (m > 128)
                    {
                        cftmdl2(m, ref a, idx2 - m, ref w, nw - m);
                        m >>= 2;
                    }
                }
            }
            return isplt;
        }

        public static long cfttree(this long n, long j, long k, ref DoubleLargeArray a, long offa, long nw, ref double[] w)
        {
            long i, isplt, m;
            long idx1 = offa - n;
            if ((k & 3) != 0)
            {
                isplt = k & 1;
                if (isplt != 0)
                {
                    cftmdl1(n, ref a, idx1 + j, ref w, nw - (n >> 1));
                }
                else
                {
                    cftmdl2(n, ref a, idx1 + j, ref w, nw - n);
                }
            }
            else
            {
                m = n;
                for (i = k; (i & 3) == 0; i >>= 2)
                {
                    m <<= 2;
                }
                isplt = i & 1;
                long idx2 = offa + j;
                if (isplt != 0)
                {
                    while (m > 128)
                    {
                        cftmdl1(m, ref a, idx2 - m, ref w, nw - (m >> 1));
                        m >>= 2;
                    }
                }
                else
                {
                    while (m > 128)
                    {
                        cftmdl2(m, ref a, idx2 - m, ref w, nw - m);
                        m >>= 2;
                    }
                }
            }
            return isplt;
        }

        public static long cfttree(this long n, long j, long k, ref DoubleLargeArray a, long offa, long nw, ref DoubleLargeArray w)
        {
            long i, isplt, m;
            long idx1 = offa - n;
            if ((k & 3) != 0)
            {
                isplt = k & 1;
                if (isplt != 0)
                {
                    cftmdl1(n, ref a, idx1 + j, ref w, nw - (n >> 1));
                }
                else
                {
                    cftmdl2(n, ref a, idx1 + j, ref w, nw - n);
                }
            }
            else
            {
                m = n;
                for (i = k; (i & 3) == 0; i >>= 2)
                {
                    m <<= 2;
                }
                isplt = i & 1;
                long idx2 = offa + j;
                if (isplt != 0)
                {
                    while (m > 128)
                    {
                        cftmdl1(m, ref a, idx2 - m, ref w, nw - (m >> 1));
                        m >>= 2;
                    }
                }
                else
                {
                    while (m > 128)
                    {
                        cftmdl2(m, ref a, idx2 - m, ref w, nw - m);
                        m >>= 2;
                    }
                }
            }
            return isplt;
        }

        public static void cftmdl1(this int n, ref double[] a, int offa, ref double[] w, int startw)
        {
            int j0, j1, j2, j3, k, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            int idx0, idx1, idx2, idx3, idx4, idx5;

            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            k = 0;
            for (int j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
            }
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
        }

        public static void cftmdl1(this long n, ref double[] a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;

            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            k = 0;
            for (long j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
            }
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
        }

        public static void cftmdl1(this long n, ref DoubleLargeArray a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;

            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            k = 0;
            for (long j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
            }
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
        }

        public static void cftmdl1(this long n, ref DoubleLargeArray a, long offa, ref DoubleLargeArray w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;

            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            k = 0;
            for (int j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
            }
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
        }

        public static void cftmdl2(this int n, ref float[] a, int offa, ref float[] w, int startw)
        {
            int j0, j1, j2, j3, k, kr, m, mh;
            float wn4r, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            float x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y2r, y2i;
            int idx0, idx1, idx2, idx3, idx4, idx5, idx6;

            mh = n >> 3;
            m = 2 * mh;
            wn4r = w[startw + 1];
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] - a[idx2 + 1];
            x0i = a[offa + 1] + a[idx2];
            x1r = a[offa] + a[idx2 + 1];
            x1i = a[offa + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wn4r * (x2r - x2i);
            y0i = wn4r * (x2i + x2r);
            a[offa] = x0r + y0r;
            a[offa + 1] = x0i + y0i;
            a[idx1] = x0r - y0r;
            a[idx1 + 1] = x0i - y0i;
            y0r = wn4r * (x3r - x3i);
            y0i = wn4r * (x3i + x3r);
            a[idx2] = x1r - y0i;
            a[idx2 + 1] = x1i + y0r;
            a[idx3] = x1r + y0i;
            a[idx3 + 1] = x1i - y0r;
            k = 0;
            kr = 2 * m;
            for (int j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                kr -= 4;
                idx5 = startw + kr;
                wd1i = w[idx5];
                wd1r = w[idx5 + 1];
                wd3i = w[idx5 + 2];
                wd3r = w[idx5 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx6 = offa + j;
                x0r = a[idx6] - a[idx2 + 1];
                x0i = a[idx6 + 1] + a[idx2];
                x1r = a[idx6] + a[idx2 + 1];
                x1i = a[idx6 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wk1r * x0r - wk1i * x0i;
                y0i = wk1r * x0i + wk1i * x0r;
                y2r = wd1r * x2r - wd1i * x2i;
                y2i = wd1r * x2i + wd1i * x2r;
                a[idx6] = y0r + y2r;
                a[idx6 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wk3r * x1r + wk3i * x1i;
                y0i = wk3r * x1i - wk3i * x1r;
                y2r = wd3r * x3r + wd3i * x3i;
                y2i = wd3r * x3i - wd3i * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] - a[idx2 + 1];
                x0i = a[idx0 + 1] + a[idx2];
                x1r = a[idx0] + a[idx2 + 1];
                x1i = a[idx0 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wd1i * x0r - wd1r * x0i;
                y0i = wd1i * x0i + wd1r * x0r;
                y2r = wk1i * x2r - wk1r * x2i;
                y2i = wk1i * x2i + wk1r * x2r;
                a[idx0] = y0r + y2r;
                a[idx0 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wd3i * x1r + wd3r * x1i;
                y0i = wd3i * x1i - wd3r * x1r;
                y2r = wk3i * x3r + wk3r * x3i;
                y2i = wk3i * x3i - wk3r * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
            }
            wk1r = w[startw + m];
            wk1i = w[startw + m + 1];
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] - a[idx2 + 1];
            x0i = a[idx0 + 1] + a[idx2];
            x1r = a[idx0] + a[idx2 + 1];
            x1i = a[idx0 + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wk1r * x0r - wk1i * x0i;
            y0i = wk1r * x0i + wk1i * x0r;
            y2r = wk1i * x2r - wk1r * x2i;
            y2i = wk1i * x2i + wk1r * x2r;
            a[idx0] = y0r + y2r;
            a[idx0 + 1] = y0i + y2i;
            a[idx1] = y0r - y2r;
            a[idx1 + 1] = y0i - y2i;
            y0r = wk1i * x1r - wk1r * x1i;
            y0i = wk1i * x1i + wk1r * x1r;
            y2r = wk1r * x3r - wk1i * x3i;
            y2i = wk1r * x3i + wk1i * x3r;
            a[idx2] = y0r - y2r;
            a[idx2 + 1] = y0i - y2i;
            a[idx3] = y0r + y2r;
            a[idx3 + 1] = y0i + y2i;
        }

        public static void cftmdl2(this long n, ref double[] a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, kr, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y2r, y2i;
            long idx0, idx1, idx2, idx3, idx4, idx5, idx6;

            mh = n >> 3;
            m = 2 * mh;
            wn4r = w[startw + 1];
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] - a[idx2 + 1];
            x0i = a[offa + 1] + a[idx2];
            x1r = a[offa] + a[idx2 + 1];
            x1i = a[offa + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wn4r * (x2r - x2i);
            y0i = wn4r * (x2i + x2r);
            a[offa] = x0r + y0r;
            a[offa + 1] = x0i + y0i;
            a[idx1] = x0r - y0r;
            a[idx1 + 1] = x0i - y0i;
            y0r = wn4r * (x3r - x3i);
            y0i = wn4r * (x3i + x3r);
            a[idx2] = x1r - y0i;
            a[idx2 + 1] = x1i + y0r;
            a[idx3] = x1r + y0i;
            a[idx3 + 1] = x1i - y0r;
            k = 0;
            kr = 2 * m;
            for (long j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                kr -= 4;
                idx5 = startw + kr;
                wd1i = w[idx5];
                wd1r = w[idx5 + 1];
                wd3i = w[idx5 + 2];
                wd3r = w[idx5 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx6 = offa + j;
                x0r = a[idx6] - a[idx2 + 1];
                x0i = a[idx6 + 1] + a[idx2];
                x1r = a[idx6] + a[idx2 + 1];
                x1i = a[idx6 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wk1r * x0r - wk1i * x0i;
                y0i = wk1r * x0i + wk1i * x0r;
                y2r = wd1r * x2r - wd1i * x2i;
                y2i = wd1r * x2i + wd1i * x2r;
                a[idx6] = y0r + y2r;
                a[idx6 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wk3r * x1r + wk3i * x1i;
                y0i = wk3r * x1i - wk3i * x1r;
                y2r = wd3r * x3r + wd3i * x3i;
                y2i = wd3r * x3i - wd3i * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] - a[idx2 + 1];
                x0i = a[idx0 + 1] + a[idx2];
                x1r = a[idx0] + a[idx2 + 1];
                x1i = a[idx0 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wd1i * x0r - wd1r * x0i;
                y0i = wd1i * x0i + wd1r * x0r;
                y2r = wk1i * x2r - wk1r * x2i;
                y2i = wk1i * x2i + wk1r * x2r;
                a[idx0] = y0r + y2r;
                a[idx0 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wd3i * x1r + wd3r * x1i;
                y0i = wd3i * x1i - wd3r * x1r;
                y2r = wk3i * x3r + wk3r * x3i;
                y2i = wk3i * x3i - wk3r * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
            }
            wk1r = w[startw + m];
            wk1i = w[startw + m + 1];
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] - a[idx2 + 1];
            x0i = a[idx0 + 1] + a[idx2];
            x1r = a[idx0] + a[idx2 + 1];
            x1i = a[idx0 + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wk1r * x0r - wk1i * x0i;
            y0i = wk1r * x0i + wk1i * x0r;
            y2r = wk1i * x2r - wk1r * x2i;
            y2i = wk1i * x2i + wk1r * x2r;
            a[idx0] = y0r + y2r;
            a[idx0 + 1] = y0i + y2i;
            a[idx1] = y0r - y2r;
            a[idx1 + 1] = y0i - y2i;
            y0r = wk1i * x1r - wk1r * x1i;
            y0i = wk1i * x1i + wk1r * x1r;
            y2r = wk1r * x3r - wk1i * x3i;
            y2i = wk1r * x3i + wk1i * x3r;
            a[idx2] = y0r - y2r;
            a[idx2 + 1] = y0i - y2i;
            a[idx3] = y0r + y2r;
            a[idx3 + 1] = y0i + y2i;
        }

        public static void cftmdl2(this long n, ref FloatLargeArray a, long offa, ref FloatLargeArray w, long startw)
        {
            long j0, j1, j2, j3, k, kr, m, mh;
            float wn4r, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            float x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y2r, y2i;
            long idx0, idx1, idx2, idx3, idx4, idx5, idx6;

            mh = n >> 3;
            m = 2 * mh;
            wn4r = w[startw + 1];
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] - a[idx2 + 1];
            x0i = a[offa + 1] + a[idx2];
            x1r = a[offa] + a[idx2 + 1];
            x1i = a[offa + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wn4r * (x2r - x2i);
            y0i = wn4r * (x2i + x2r);
            a[offa] = x0r + y0r;
            a[offa + 1] = x0i + y0i;
            a[idx1] = x0r - y0r;
            a[idx1 + 1] = x0i - y0i;
            y0r = wn4r * (x3r - x3i);
            y0i = wn4r * (x3i + x3r);
            a[idx2] = x1r - y0i;
            a[idx2 + 1] = x1i + y0r;
            a[idx3] = x1r + y0i;
            a[idx3 + 1] = x1i - y0r;
            k = 0;
            kr = 2 * m;
            for (int j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                kr -= 4;
                idx5 = startw + kr;
                wd1i = w[idx5];
                wd1r = w[idx5 + 1];
                wd3i = w[idx5 + 2];
                wd3r = w[idx5 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx6 = offa + j;
                x0r = a[idx6] - a[idx2 + 1];
                x0i = a[idx6 + 1] + a[idx2];
                x1r = a[idx6] + a[idx2 + 1];
                x1i = a[idx6 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wk1r * x0r - wk1i * x0i;
                y0i = wk1r * x0i + wk1i * x0r;
                y2r = wd1r * x2r - wd1i * x2i;
                y2i = wd1r * x2i + wd1i * x2r;
                a[idx6] = y0r + y2r;
                a[idx6 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wk3r * x1r + wk3i * x1i;
                y0i = wk3r * x1i - wk3i * x1r;
                y2r = wd3r * x3r + wd3i * x3i;
                y2i = wd3r * x3i - wd3i * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] - a[idx2 + 1];
                x0i = a[idx0 + 1] + a[idx2];
                x1r = a[idx0] + a[idx2 + 1];
                x1i = a[idx0 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wd1i * x0r - wd1r * x0i;
                y0i = wd1i * x0i + wd1r * x0r;
                y2r = wk1i * x2r - wk1r * x2i;
                y2i = wk1i * x2i + wk1r * x2r;
                a[idx0] = y0r + y2r;
                a[idx0 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wd3i * x1r + wd3r * x1i;
                y0i = wd3i * x1i - wd3r * x1r;
                y2r = wk3i * x3r + wk3r * x3i;
                y2i = wk3i * x3i - wk3r * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
            }
            wk1r = w[startw + m];
            wk1i = w[startw + m + 1];
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] - a[idx2 + 1];
            x0i = a[idx0 + 1] + a[idx2];
            x1r = a[idx0] + a[idx2 + 1];
            x1i = a[idx0 + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wk1r * x0r - wk1i * x0i;
            y0i = wk1r * x0i + wk1i * x0r;
            y2r = wk1i * x2r - wk1r * x2i;
            y2i = wk1i * x2i + wk1r * x2r;
            a[idx0] = y0r + y2r;
            a[idx0 + 1] = y0i + y2i;
            a[idx1] = y0r - y2r;
            a[idx1 + 1] = y0i - y2i;
            y0r = wk1i * x1r - wk1r * x1i;
            y0i = wk1i * x1i + wk1r * x1r;
            y2r = wk1r * x3r - wk1i * x3i;
            y2i = wk1r * x3i + wk1i * x3r;
            a[idx2] = y0r - y2r;
            a[idx2 + 1] = y0i - y2i;
            a[idx3] = y0r + y2r;
            a[idx3 + 1] = y0i + y2i;
        }

        public static void cftmdl2(this int n, ref double[] a, int offa, ref double[] w, int startw)
        {
            int j0, j1, j2, j3, k, kr, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y2r, y2i;
            int idx0, idx1, idx2, idx3, idx4, idx5, idx6;

            mh = n >> 3;
            m = 2 * mh;
            wn4r = w[startw + 1];
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] - a[idx2 + 1];
            x0i = a[offa + 1] + a[idx2];
            x1r = a[offa] + a[idx2 + 1];
            x1i = a[offa + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wn4r * (x2r - x2i);
            y0i = wn4r * (x2i + x2r);
            a[offa] = x0r + y0r;
            a[offa + 1] = x0i + y0i;
            a[idx1] = x0r - y0r;
            a[idx1 + 1] = x0i - y0i;
            y0r = wn4r * (x3r - x3i);
            y0i = wn4r * (x3i + x3r);
            a[idx2] = x1r - y0i;
            a[idx2 + 1] = x1i + y0r;
            a[idx3] = x1r + y0i;
            a[idx3 + 1] = x1i - y0r;
            k = 0;
            kr = 2 * m;
            for (int j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                kr -= 4;
                idx5 = startw + kr;
                wd1i = w[idx5];
                wd1r = w[idx5 + 1];
                wd3i = w[idx5 + 2];
                wd3r = w[idx5 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx6 = offa + j;
                x0r = a[idx6] - a[idx2 + 1];
                x0i = a[idx6 + 1] + a[idx2];
                x1r = a[idx6] + a[idx2 + 1];
                x1i = a[idx6 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wk1r * x0r - wk1i * x0i;
                y0i = wk1r * x0i + wk1i * x0r;
                y2r = wd1r * x2r - wd1i * x2i;
                y2i = wd1r * x2i + wd1i * x2r;
                a[idx6] = y0r + y2r;
                a[idx6 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wk3r * x1r + wk3i * x1i;
                y0i = wk3r * x1i - wk3i * x1r;
                y2r = wd3r * x3r + wd3i * x3i;
                y2i = wd3r * x3i - wd3i * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] - a[idx2 + 1];
                x0i = a[idx0 + 1] + a[idx2];
                x1r = a[idx0] + a[idx2 + 1];
                x1i = a[idx0 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wd1i * x0r - wd1r * x0i;
                y0i = wd1i * x0i + wd1r * x0r;
                y2r = wk1i * x2r - wk1r * x2i;
                y2i = wk1i * x2i + wk1r * x2r;
                a[idx0] = y0r + y2r;
                a[idx0 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wd3i * x1r + wd3r * x1i;
                y0i = wd3i * x1i - wd3r * x1r;
                y2r = wk3i * x3r + wk3r * x3i;
                y2i = wk3i * x3i - wk3r * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
            }
            wk1r = w[startw + m];
            wk1i = w[startw + m + 1];
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] - a[idx2 + 1];
            x0i = a[idx0 + 1] + a[idx2];
            x1r = a[idx0] + a[idx2 + 1];
            x1i = a[idx0 + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wk1r * x0r - wk1i * x0i;
            y0i = wk1r * x0i + wk1i * x0r;
            y2r = wk1i * x2r - wk1r * x2i;
            y2i = wk1i * x2i + wk1r * x2r;
            a[idx0] = y0r + y2r;
            a[idx0 + 1] = y0i + y2i;
            a[idx1] = y0r - y2r;
            a[idx1 + 1] = y0i - y2i;
            y0r = wk1i * x1r - wk1r * x1i;
            y0i = wk1i * x1i + wk1r * x1r;
            y2r = wk1r * x3r - wk1i * x3i;
            y2i = wk1r * x3i + wk1i * x3r;
            a[idx2] = y0r - y2r;
            a[idx2 + 1] = y0i - y2i;
            a[idx3] = y0r + y2r;
            a[idx3 + 1] = y0i + y2i;
        }

        public static void cftmdl2(this long n, ref DoubleLargeArray a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, kr, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y2r, y2i;
            long idx0, idx1, idx2, idx3, idx4, idx5, idx6;

            mh = n >> 3;
            m = 2 * mh;
            wn4r = w[startw + 1];
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] - a[idx2 + 1];
            x0i = a[offa + 1] + a[idx2];
            x1r = a[offa] + a[idx2 + 1];
            x1i = a[offa + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wn4r * (x2r - x2i);
            y0i = wn4r * (x2i + x2r);
            a[offa] = x0r + y0r;
            a[offa + 1] = x0i + y0i;
            a[idx1] = x0r - y0r;
            a[idx1 + 1] = x0i - y0i;
            y0r = wn4r * (x3r - x3i);
            y0i = wn4r * (x3i + x3r);
            a[idx2] = x1r - y0i;
            a[idx2 + 1] = x1i + y0r;
            a[idx3] = x1r + y0i;
            a[idx3 + 1] = x1i - y0r;
            k = 0;
            kr = 2 * m;
            for (long j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                kr -= 4;
                idx5 = startw + kr;
                wd1i = w[idx5];
                wd1r = w[idx5 + 1];
                wd3i = w[idx5 + 2];
                wd3r = w[idx5 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx6 = offa + j;
                x0r = a[idx6] - a[idx2 + 1];
                x0i = a[idx6 + 1] + a[idx2];
                x1r = a[idx6] + a[idx2 + 1];
                x1i = a[idx6 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wk1r * x0r - wk1i * x0i;
                y0i = wk1r * x0i + wk1i * x0r;
                y2r = wd1r * x2r - wd1i * x2i;
                y2i = wd1r * x2i + wd1i * x2r;
                a[idx6] = y0r + y2r;
                a[idx6 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wk3r * x1r + wk3i * x1i;
                y0i = wk3r * x1i - wk3i * x1r;
                y2r = wd3r * x3r + wd3i * x3i;
                y2i = wd3r * x3i - wd3i * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] - a[idx2 + 1];
                x0i = a[idx0 + 1] + a[idx2];
                x1r = a[idx0] + a[idx2 + 1];
                x1i = a[idx0 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wd1i * x0r - wd1r * x0i;
                y0i = wd1i * x0i + wd1r * x0r;
                y2r = wk1i * x2r - wk1r * x2i;
                y2i = wk1i * x2i + wk1r * x2r;
                a[idx0] = y0r + y2r;
                a[idx0 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wd3i * x1r + wd3r * x1i;
                y0i = wd3i * x1i - wd3r * x1r;
                y2r = wk3i * x3r + wk3r * x3i;
                y2i = wk3i * x3i - wk3r * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
            }
            wk1r = w[startw + m];
            wk1i = w[startw + m + 1];
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] - a[idx2 + 1];
            x0i = a[idx0 + 1] + a[idx2];
            x1r = a[idx0] + a[idx2 + 1];
            x1i = a[idx0 + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wk1r * x0r - wk1i * x0i;
            y0i = wk1r * x0i + wk1i * x0r;
            y2r = wk1i * x2r - wk1r * x2i;
            y2i = wk1i * x2i + wk1r * x2r;
            a[idx0] = y0r + y2r;
            a[idx0 + 1] = y0i + y2i;
            a[idx1] = y0r - y2r;
            a[idx1 + 1] = y0i - y2i;
            y0r = wk1i * x1r - wk1r * x1i;
            y0i = wk1i * x1i + wk1r * x1r;
            y2r = wk1r * x3r - wk1i * x3i;
            y2i = wk1r * x3i + wk1i * x3r;
            a[idx2] = y0r - y2r;
            a[idx2 + 1] = y0i - y2i;
            a[idx3] = y0r + y2r;
            a[idx3 + 1] = y0i + y2i;
        }

        public static void cftmdl2(this long n, ref DoubleLargeArray a, long offa, ref DoubleLargeArray w, long startw)
        {
            long j0, j1, j2, j3, k, kr, m, mh;
            double wn4r, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y2r, y2i;
            long idx0, idx1, idx2, idx3, idx4, idx5, idx6;

            mh = n >> 3;
            m = 2 * mh;
            wn4r = w[startw + 1];
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] - a[idx2 + 1];
            x0i = a[offa + 1] + a[idx2];
            x1r = a[offa] + a[idx2 + 1];
            x1i = a[offa + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wn4r * (x2r - x2i);
            y0i = wn4r * (x2i + x2r);
            a[offa] = x0r + y0r;
            a[offa + 1] = x0i + y0i;
            a[idx1] = x0r - y0r;
            a[idx1 + 1] = x0i - y0i;
            y0r = wn4r * (x3r - x3i);
            y0i = wn4r * (x3i + x3r);
            a[idx2] = x1r - y0i;
            a[idx2 + 1] = x1i + y0r;
            a[idx3] = x1r + y0i;
            a[idx3 + 1] = x1i - y0r;
            k = 0;
            kr = 2 * m;
            for (int j = 2; j < mh; j += 2)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = w[idx4];
                wk1i = w[idx4 + 1];
                wk3r = w[idx4 + 2];
                wk3i = w[idx4 + 3];
                kr -= 4;
                idx5 = startw + kr;
                wd1i = w[idx5];
                wd1r = w[idx5 + 1];
                wd3i = w[idx5 + 2];
                wd3r = w[idx5 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx6 = offa + j;
                x0r = a[idx6] - a[idx2 + 1];
                x0i = a[idx6 + 1] + a[idx2];
                x1r = a[idx6] + a[idx2 + 1];
                x1i = a[idx6 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wk1r * x0r - wk1i * x0i;
                y0i = wk1r * x0i + wk1i * x0r;
                y2r = wd1r * x2r - wd1i * x2i;
                y2i = wd1r * x2i + wd1i * x2r;
                a[idx6] = y0r + y2r;
                a[idx6 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wk3r * x1r + wk3i * x1i;
                y0i = wk3r * x1i - wk3i * x1r;
                y2r = wd3r * x3r + wd3i * x3i;
                y2i = wd3r * x3i - wd3i * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] - a[idx2 + 1];
                x0i = a[idx0 + 1] + a[idx2];
                x1r = a[idx0] + a[idx2 + 1];
                x1i = a[idx0 + 1] - a[idx2];
                x2r = a[idx1] - a[idx3 + 1];
                x2i = a[idx1 + 1] + a[idx3];
                x3r = a[idx1] + a[idx3 + 1];
                x3i = a[idx1 + 1] - a[idx3];
                y0r = wd1i * x0r - wd1r * x0i;
                y0i = wd1i * x0i + wd1r * x0r;
                y2r = wk1i * x2r - wk1r * x2i;
                y2i = wk1i * x2i + wk1r * x2r;
                a[idx0] = y0r + y2r;
                a[idx0 + 1] = y0i + y2i;
                a[idx1] = y0r - y2r;
                a[idx1 + 1] = y0i - y2i;
                y0r = wd3i * x1r + wd3r * x1i;
                y0i = wd3i * x1i - wd3r * x1r;
                y2r = wk3i * x3r + wk3r * x3i;
                y2i = wk3i * x3i - wk3r * x3r;
                a[idx2] = y0r + y2r;
                a[idx2 + 1] = y0i + y2i;
                a[idx3] = y0r - y2r;
                a[idx3 + 1] = y0i - y2i;
            }
            wk1r = w[startw + m];
            wk1i = w[startw + m + 1];
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0] - a[idx2 + 1];
            x0i = a[idx0 + 1] + a[idx2];
            x1r = a[idx0] + a[idx2 + 1];
            x1i = a[idx0 + 1] - a[idx2];
            x2r = a[idx1] - a[idx3 + 1];
            x2i = a[idx1 + 1] + a[idx3];
            x3r = a[idx1] + a[idx3 + 1];
            x3i = a[idx1 + 1] - a[idx3];
            y0r = wk1r * x0r - wk1i * x0i;
            y0i = wk1r * x0i + wk1i * x0r;
            y2r = wk1i * x2r - wk1r * x2i;
            y2i = wk1i * x2i + wk1r * x2r;
            a[idx0] = y0r + y2r;
            a[idx0 + 1] = y0i + y2i;
            a[idx1] = y0r - y2r;
            a[idx1 + 1] = y0i - y2i;
            y0r = wk1i * x1r - wk1r * x1i;
            y0i = wk1i * x1i + wk1r * x1r;
            y2r = wk1r * x3r - wk1i * x3i;
            y2i = wk1r * x3i + wk1i * x3r;
            a[idx2] = y0r - y2r;
            a[idx2 + 1] = y0i - y2i;
            a[idx3] = y0r + y2r;
            a[idx3 + 1] = y0i + y2i;
        }

        public static void cftf1st(int n, ref double[] a, int offa, ref double[] w, int startw)
        {
            int j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            int idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = a[idx5 + 3] + a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = a[idx5 + 3] - a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = a[idx0 - 1] + a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = a[idx0 - 1] - a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[offa + j3 - 2] = wd3i * x0r + wd3r * x0i;
                a[offa + j3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = a[idx0 - 1] + a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = a[idx0 - 1] - a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i + x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = a[idx0 + 3] + a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = a[idx0 + 3] - a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i + x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftf1st(this long n, ref double[] a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = a[idx5 + 3] + a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = a[idx5 + 3] - a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = a[idx0 - 1] + a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = a[idx0 - 1] - a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[offa + j3 - 2] = wd3i * x0r + wd3r * x0i;
                a[offa + j3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = a[idx0 - 1] + a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = a[idx0 - 1] - a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i + x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = a[idx0 + 3] + a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = a[idx0 + 3] - a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i + x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftf1st(this long n, ref DoubleLargeArray a, long offa, ref double[] w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = a[idx5 + 3] + a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = a[idx5 + 3] - a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = a[idx0 - 1] + a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = a[idx0 - 1] - a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[offa + j3 - 2] = wd3i * x0r + wd3r * x0i;
                a[offa + j3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = a[idx0 - 1] + a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = a[idx0 - 1] - a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i + x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = a[idx0 + 3] + a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = a[idx0 + 3] - a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i + x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftf1st(this long n, ref DoubleLargeArray a, long offa, ref DoubleLargeArray w, long startw)
        {
            long j0, j1, j2, j3, k, m, mh;
            double wn4r, csc1, csc3, wk1r, wk1i, wk3r, wk3i, wd1r, wd1i, wd3r, wd3i;
            double x0r, x0i, x1r, x1i, x2r, x2i, x3r, x3i, y0r, y0i, y1r, y1i, y2r, y2i, y3r, y3i;
            long idx0, idx1, idx2, idx3, idx4, idx5;
            mh = n >> 3;
            m = 2 * mh;
            j1 = m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[offa] + a[idx2];
            x0i = a[offa + 1] + a[idx2 + 1];
            x1r = a[offa] - a[idx2];
            x1i = a[offa + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[offa] = x0r + x2r;
            a[offa + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            a[idx2] = x1r - x3i;
            a[idx2 + 1] = x1i + x3r;
            a[idx3] = x1r + x3i;
            a[idx3 + 1] = x1i - x3r;
            wn4r = w[startw + 1];
            csc1 = w[startw + 2];
            csc3 = w[startw + 3];
            wd1r = 1;
            wd1i = 0;
            wd3r = 1;
            wd3i = 0;
            k = 0;
            for (int j = 2; j < mh - 2; j += 4)
            {
                k += 4;
                idx4 = startw + k;
                wk1r = csc1 * (wd1r + w[idx4]);
                wk1i = csc1 * (wd1i + w[idx4 + 1]);
                wk3r = csc3 * (wd3r + w[idx4 + 2]);
                wk3i = csc3 * (wd3i + w[idx4 + 3]);
                wd1r = w[idx4];
                wd1i = w[idx4 + 1];
                wd3r = w[idx4 + 2];
                wd3i = w[idx4 + 3];
                j1 = j + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                idx5 = offa + j;
                x0r = a[idx5] + a[idx2];
                x0i = a[idx5 + 1] + a[idx2 + 1];
                x1r = a[idx5] - a[idx2];
                x1i = a[idx5 + 1] - a[idx2 + 1];
                y0r = a[idx5 + 2] + a[idx2 + 2];
                y0i = a[idx5 + 3] + a[idx2 + 3];
                y1r = a[idx5 + 2] - a[idx2 + 2];
                y1i = a[idx5 + 3] - a[idx2 + 3];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 + 2] + a[idx3 + 2];
                y2i = a[idx1 + 3] + a[idx3 + 3];
                y3r = a[idx1 + 2] - a[idx3 + 2];
                y3i = a[idx1 + 3] - a[idx3 + 3];
                a[idx5] = x0r + x2r;
                a[idx5 + 1] = x0i + x2i;
                a[idx5 + 2] = y0r + y2r;
                a[idx5 + 3] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 + 2] = y0r - y2r;
                a[idx1 + 3] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1r * x0r - wk1i * x0i;
                a[idx2 + 1] = wk1r * x0i + wk1i * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 + 2] = wd1r * x0r - wd1i * x0i;
                a[idx2 + 3] = wd1r * x0i + wd1i * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3r * x0r + wk3i * x0i;
                a[idx3 + 1] = wk3r * x0i - wk3i * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[idx3 + 2] = wd3r * x0r + wd3i * x0i;
                a[idx3 + 3] = wd3r * x0i - wd3i * x0r;
                j0 = m - j;
                j1 = j0 + m;
                j2 = j1 + m;
                j3 = j2 + m;
                idx0 = offa + j0;
                idx1 = offa + j1;
                idx2 = offa + j2;
                idx3 = offa + j3;
                x0r = a[idx0] + a[idx2];
                x0i = a[idx0 + 1] + a[idx2 + 1];
                x1r = a[idx0] - a[idx2];
                x1i = a[idx0 + 1] - a[idx2 + 1];
                y0r = a[idx0 - 2] + a[idx2 - 2];
                y0i = a[idx0 - 1] + a[idx2 - 1];
                y1r = a[idx0 - 2] - a[idx2 - 2];
                y1i = a[idx0 - 1] - a[idx2 - 1];
                x2r = a[idx1] + a[idx3];
                x2i = a[idx1 + 1] + a[idx3 + 1];
                x3r = a[idx1] - a[idx3];
                x3i = a[idx1 + 1] - a[idx3 + 1];
                y2r = a[idx1 - 2] + a[idx3 - 2];
                y2i = a[idx1 - 1] + a[idx3 - 1];
                y3r = a[idx1 - 2] - a[idx3 - 2];
                y3i = a[idx1 - 1] - a[idx3 - 1];
                a[idx0] = x0r + x2r;
                a[idx0 + 1] = x0i + x2i;
                a[idx0 - 2] = y0r + y2r;
                a[idx0 - 1] = y0i + y2i;
                a[idx1] = x0r - x2r;
                a[idx1 + 1] = x0i - x2i;
                a[idx1 - 2] = y0r - y2r;
                a[idx1 - 1] = y0i - y2i;
                x0r = x1r - x3i;
                x0i = x1i + x3r;
                a[idx2] = wk1i * x0r - wk1r * x0i;
                a[idx2 + 1] = wk1i * x0i + wk1r * x0r;
                x0r = y1r - y3i;
                x0i = y1i + y3r;
                a[idx2 - 2] = wd1i * x0r - wd1r * x0i;
                a[idx2 - 1] = wd1i * x0i + wd1r * x0r;
                x0r = x1r + x3i;
                x0i = x1i - x3r;
                a[idx3] = wk3i * x0r + wk3r * x0i;
                a[idx3 + 1] = wk3i * x0i - wk3r * x0r;
                x0r = y1r + y3i;
                x0i = y1i - y3r;
                a[offa + j3 - 2] = wd3i * x0r + wd3r * x0i;
                a[offa + j3 - 1] = wd3i * x0i - wd3r * x0r;
            }
            wk1r = csc1 * (wd1r + wn4r);
            wk1i = csc1 * (wd1i + wn4r);
            wk3r = csc3 * (wd3r - wn4r);
            wk3i = csc3 * (wd3i - wn4r);
            j0 = mh;
            j1 = j0 + m;
            j2 = j1 + m;
            j3 = j2 + m;
            idx0 = offa + j0;
            idx1 = offa + j1;
            idx2 = offa + j2;
            idx3 = offa + j3;
            x0r = a[idx0 - 2] + a[idx2 - 2];
            x0i = a[idx0 - 1] + a[idx2 - 1];
            x1r = a[idx0 - 2] - a[idx2 - 2];
            x1i = a[idx0 - 1] - a[idx2 - 1];
            x2r = a[idx1 - 2] + a[idx3 - 2];
            x2i = a[idx1 - 1] + a[idx3 - 1];
            x3r = a[idx1 - 2] - a[idx3 - 2];
            x3i = a[idx1 - 1] - a[idx3 - 1];
            a[idx0 - 2] = x0r + x2r;
            a[idx0 - 1] = x0i + x2i;
            a[idx1 - 2] = x0r - x2r;
            a[idx1 - 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 - 2] = wk1r * x0r - wk1i * x0i;
            a[idx2 - 1] = wk1r * x0i + wk1i * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 - 2] = wk3r * x0r + wk3i * x0i;
            a[idx3 - 1] = wk3r * x0i - wk3i * x0r;
            x0r = a[idx0] + a[idx2];
            x0i = a[idx0 + 1] + a[idx2 + 1];
            x1r = a[idx0] - a[idx2];
            x1i = a[idx0 + 1] - a[idx2 + 1];
            x2r = a[idx1] + a[idx3];
            x2i = a[idx1 + 1] + a[idx3 + 1];
            x3r = a[idx1] - a[idx3];
            x3i = a[idx1 + 1] - a[idx3 + 1];
            a[idx0] = x0r + x2r;
            a[idx0 + 1] = x0i + x2i;
            a[idx1] = x0r - x2r;
            a[idx1 + 1] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2] = wn4r * (x0r - x0i);
            a[idx2 + 1] = wn4r * (x0i + x0r);
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3] = -wn4r * (x0r + x0i);
            a[idx3 + 1] = -wn4r * (x0i - x0r);
            x0r = a[idx0 + 2] + a[idx2 + 2];
            x0i = a[idx0 + 3] + a[idx2 + 3];
            x1r = a[idx0 + 2] - a[idx2 + 2];
            x1i = a[idx0 + 3] - a[idx2 + 3];
            x2r = a[idx1 + 2] + a[idx3 + 2];
            x2i = a[idx1 + 3] + a[idx3 + 3];
            x3r = a[idx1 + 2] - a[idx3 + 2];
            x3i = a[idx1 + 3] - a[idx3 + 3];
            a[idx0 + 2] = x0r + x2r;
            a[idx0 + 3] = x0i + x2i;
            a[idx1 + 2] = x0r - x2r;
            a[idx1 + 3] = x0i - x2i;
            x0r = x1r - x3i;
            x0i = x1i + x3r;
            a[idx2 + 2] = wk1i * x0r - wk1r * x0i;
            a[idx2 + 3] = wk1i * x0i + wk1r * x0r;
            x0r = x1r + x3i;
            x0i = x1i - x3r;
            a[idx3 + 2] = wk3i * x0r + wk3r * x0i;
            a[idx3 + 3] = wk3i * x0i - wk3r * x0r;
        }

        public static void cftfx41(this int n, ref double[] a, int offa, int nw, ref double[] w)
        {
            if (n == 128)
            {
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
            }
            else
            {
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
            }
        }

        public static void cftfx41(this long n, ref double[] a, long offa, long nw, ref double[] w)
        {
            if (n == 128)
            {
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
            }
            else
            {
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
            }
        }

        public static void cftfx41(this long n, ref DoubleLargeArray a, long offa, long nw, ref double[] w)
        {
            if (n == 128)
            {
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
            }
            else
            {
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
            }
        }

        public static void cftfx41(this long n, ref DoubleLargeArray a, long offa, long nw, ref DoubleLargeArray w)
        {
            if (n == 128)
            {
                a.cftf161(offa, ref w, nw - 8);
                a.cftf162(offa + 32, ref w, nw - 32);
                a.cftf161(offa + 64, ref w, nw - 8);
                a.cftf161(offa + 96, ref w, nw - 8);
            }
            else
            {
                a.cftf081(offa, ref w, nw - 8);
                a.cftf082(offa + 16, ref w, nw - 8);
                a.cftf081(offa + 32, ref w, nw - 8);
                a.cftf081(offa + 48, ref w, nw - 8);
            }
        }

        public static void cftfsub(this int n, ref double[] a, int offa, ref int[] ip, int nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftf1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);

                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208(offa);
                }
            }
            else if (n == 8)
            {
                a.cftf040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftfsub(this long n, ref DoubleLargeArray a, int offa, ref int[] ip, int nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftf1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);

                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208(offa);
                }
            }
            else if (n == 8)
            {
                a.cftf040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftfsub(this long n, ref DoubleLargeArray a, long offa, ref long[] ip, long nw, ref double[] w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftf1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);

                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208(offa);
                }
            }
            else if (n == 8)
            {
                a.cftf040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void cftfsub(this long n, ref DoubleLargeArray a, int offa, ref LongLargeArray ip, int nw, ref DoubleLargeArray w)
        {
            if (n > 8)
            {
                if (n > 32)
                {
                    cftf1st(n, ref a, offa, ref w, nw - (n >> 2));
                    if ((Process.GetCurrentProcess().Threads.Count > 1) && (n >= THREADS_BEGIN_N_1D_FFT_2THREADS))
                    {
                        cftrec4_th(n, ref a, offa, nw, ref w);
                    }
                    else if (n > 512)
                    {
                        cftrec4(n, ref a, offa, nw, ref w);

                    }
                    else if (n > 128)
                    {
                        cftleaf(n, 1, ref a, offa, nw, ref w);
                    }
                    else
                    {
                        cftfx41(n, ref a, offa, nw, ref w);
                    }
                    bitrv2(n, ref ip, ref a, offa);
                }
                else if (n == 32)
                {
                    a.cftf161(offa, ref w, nw - 8);
                    a.bitrv216(offa);
                }
                else
                {
                    a.cftf081(offa, ref w, 0);
                    a.bitrv208(offa);
                }
            }
            else if (n == 8)
            {
                a.cftf040(offa);
            }
            else if (n == 4)
            {
                a.cftxb020(offa);
            }
        }

        public static void rftbsub(this int n, ref double[] a, int offa, int nc, ref double[] c, int startc)
        {
            int k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            int idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (int j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] -= yi;
                a[idx2] += yr;
                a[idx2 + 1] -= yi;
            }
        }

        public static void rftbsub(this long n, ref double[] a, int offa, int nc, ref double[] c, int startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (long j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] -= yi;
                a[idx2] += yr;
                a[idx2 + 1] -= yi;
            }
        }

        public static void rftbsub(this long n, ref DoubleLargeArray a, long offa, long nc, ref double[] c, long startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (long j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] -= yi;
                a[idx2] += yr;
                a[idx2 + 1] -= yi;
            }
        }

        public static void rftfsub(this int n, ref double[] a, int offa, int nc, ref double[] c, int startc)
        {
            int k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            int idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (int j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] = yi - a[idx1 + 1];
                a[idx2] += yr;
                a[idx2 + 1] = yi - a[idx2 + 1];
            }
            a[offa + m + 1] = -a[offa + m + 1];
        }

        public static void rftfsub(this long n, ref double[] a, int offa, int nc, ref double[] c, int startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (int j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] = yi - a[idx1 + 1];
                a[idx2] += yr;
                a[idx2 + 1] = yi - a[idx2 + 1];
            }
            a[offa + m + 1] = -a[offa + m + 1];
        }

        public static void rftfsub(this long n, ref DoubleLargeArray a, int offa, int nc, ref double[] c, int startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (long j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] = yi - a[idx1 + 1];
                a[idx2] += yr;
                a[idx2 + 1] = yi - a[idx2 + 1];
            }
            a[offa + m + 1] = -a[offa + m + 1];
        }

        public static void rftfsub(this long n, ref DoubleLargeArray a, int offa, int nc, ref DoubleLargeArray c, int startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (long j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] = yi - a[idx1 + 1];
                a[idx2] += yr;
                a[idx2 + 1] = yi - a[idx2 + 1];
            }
            a[offa + m + 1] = -a[offa + m + 1];
        }

        public static void rftfsub(this long n, ref DoubleLargeArray a, long offa, long nc, ref double[] c, long startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (long j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] = yi - a[idx1 + 1];
                a[idx2] += yr;
                a[idx2 + 1] = yi - a[idx2 + 1];
            }
            a[offa + m + 1] = -a[offa + m + 1];
        }

        public static void rftfsub(this long n, ref DoubleLargeArray a, long offa, long nc, ref DoubleLargeArray c, long startc)
        {
            long k, kk, ks, m;
            double wkr, wki, xr, xi, yr, yi;
            long idx1, idx2;

            m = n >> 1;
            ks = 2 * nc / m;
            kk = 0;
            for (long j = 2; j < m; j += 2)
            {
                k = n - j;
                kk += ks;
                wkr = 0.5 - c[startc + nc - kk];
                wki = c[startc + kk];
                idx1 = offa + j;
                idx2 = offa + k;
                xr = a[idx1] - a[idx2];
                xi = a[idx1 + 1] + a[idx2 + 1];
                yr = wkr * xr - wki * xi;
                yi = wkr * xi + wki * xr;
                a[idx1] -= yr;
                a[idx1 + 1] = yi - a[idx1 + 1];
                a[idx2] += yr;
                a[idx2 + 1] = yi - a[idx2 + 1];
            }
            a[offa + m + 1] = -a[offa + m + 1];
        }

        public static void scale(this int n, double m, ref double[] a, int offa, Boolean complex)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            int n2;
            if (complex)
            {
                n2 = 2 * n;
            }
            else
            {
                n2 = n;
            }

            var a1 = a;

            if ((nthreads > 1) && (n2 > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                nthreads = 2;
                int k = n2 / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int i = 0; i < nthreads; i++)
                {
                    int firstIdx = offa + i * k;
                    int lastIdx = (i == (nthreads - 1)) ? offa + n2 : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (int j = firstIdx; j < lastIdx; j++)
                        {
                            a1[j] *= m;
                        }
                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    a = a1;
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {
                int firstIdx = offa;
                int lastIdx = offa + n2;
                for (int i = firstIdx; i < lastIdx; i++)
                {
                    a[i] *= m;
                }
            }
        }

        public static void scale(this long n, double m, ref DoubleLargeArray a, long offa, Boolean complex)
        {
            int nthreads = Process.GetCurrentProcess().Threads.Count;
            long n2;
            if (complex)
            {
                n2 = 2 * n;
            }
            else
            {
                n2 = n;
            }

            var a1 = a;

            if ((nthreads > 1) && (n2 > THREADS_BEGIN_N_1D_FFT_2THREADS))
            {
                nthreads = 2;
                long k = n2 / nthreads;
                Task[] taskArray = new Task[nthreads];
                for (int i = 0; i < nthreads; i++)
                {
                    long firstIdx = offa + i * k;
                    long lastIdx = (i == (nthreads - 1)) ? offa + n2 : firstIdx + k;
                    taskArray[i] = Task.Factory.StartNew(() =>
                    {
                        for (long j = firstIdx; j < lastIdx; j++)
                        {
                            a1[j] *= m;
                        }
                    });
                }
                try
                {
                    Task.WaitAll(taskArray);
                    a = a1;
                }
                catch (SystemException ex)
                {
                    Logger.Error(ex.ToString());
                }

            }
            else
            {
                long firstIdx = offa;
                long lastIdx = offa + n2;
                for (long i = firstIdx; i < lastIdx; i++)
                {
                    a[i] *= m;
                }
            }
        }

    }
}
