using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace System
{
    public static class VectorExtension
    {
        /// <summary>
        ///   Gets the inner product (scalar product) between vector v and a int array b (v'*b).
        /// </summary>
        /// 
        /// <param name="a">A vector.</param>
        /// <param name="b">A vector.</param>
        /// 
        /// <returns>The inner product of the multiplication of the vectors.</returns>
        public static int Dot(this Vector v, int[] b)
        {
            var a = v.AsArray();
            int r = 0;
            for (int i = 0; i < a.Length; i++)
                r += ((int)a[i] * b[i]);
            return r;
        }

        /// <summary>
        ///   Computes the product v * b of a Vector v and a double array b.
        /// </summary>
        /// 
        /// <param name="v">The left Vector.</param>
        /// <param name="b">The right double array.</param>
        ///
        /// <returns>The inner product of the multiplication of the vectors.</returns>
        public static double Dot(this Vector v, double[] b)
        {
            var a = v.AsArray();
            double r = 0;
            for (int i = 0; i < a.Length; i++)
                r += (a[i] * b[i]);
            return r;
        }

        /// <summary>
        ///   Gets the inner product (scalar product) between vectors v1 and v2 (v1'*v2).
        /// </summary>
        /// 
        /// <param name="v1">A vector.</param>
        /// <param name="v2">A vector.</param>
        /// 
        /// <returns>The inner product of the multiplication of the vectors.</returns>
        public static double Dot(this Vector v1, Vector v2)
        {
            var a = v1.AsArray();
            var b = v2.AsArray();
            double r = 0;
            for (int i = 0; i < a.Length; i++)
                r += (a[i] * b[i]);
            return r;
        }
    }
}
