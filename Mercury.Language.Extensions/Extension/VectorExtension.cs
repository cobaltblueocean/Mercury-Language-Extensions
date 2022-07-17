using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace MathNet.Numerics.LinearAlgebra
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
        public static int Dot(this MathNet.Numerics.LinearAlgebra.Double.Vector v, int[] b)
        {
            var a = v.AsArrayEx();
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
        public static double Dot(this MathNet.Numerics.LinearAlgebra.Double.Vector v, double[] b)
        {
            var a = v.AsArrayEx();
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
        public static double Dot(this MathNet.Numerics.LinearAlgebra.Double.Vector v1, MathNet.Numerics.LinearAlgebra.Double.Vector v2)
        {
            var a = v1.AsArrayEx();
            var b = v2.AsArrayEx();
            double r = 0;
            for (int i = 0; i < a.Length; i++)
                r += (a[i] * b[i]);
            return r;
        }

        public static MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.DecimalComplex> ToDecimalComplexVector(this MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex> vector)
        {
            return MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.DecimalComplex>.Build.Dense(vector.AsArrayEx().Select(x => new System.Numerics.DecimalComplex((decimal)x.Real, (decimal)x.Imaginary)).ToArray());
        }

        public static MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex> ToComplexVector(this MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.DecimalComplex> vector)
        {
            return MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex>.Build.Dense(vector.AsArrayEx().Select(x => new System.Numerics.Complex((double)x.Real, (double)x.Imaginary)).ToArray());
        }

        public static T[] AsArrayEx<T>(this MathNet.Numerics.LinearAlgebra.Vector<T> vector) where T : struct, IEquatable<T>, IFormattable
        {
            T[] _temp = vector.AsArray();
            if (_temp == null)
                _temp = vector.ToArray();

            return _temp;
        }
    }
}
