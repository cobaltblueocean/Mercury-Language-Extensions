using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Mercury.Language.Math.Matrix;

namespace Mercury.Language.Math
{
    /// <summary>
    /// Class transforming a symmetrical matrix to tridiagonal shape.
    /// <p>A symmetrical m &times; m matrix A can be written as the product of three matrices:
    /// A = Q &times; T &times; Q<sup>T</sup> with Q an orthogonal matrix and T a symmetrical
    /// tridiagonal matrix. Both Q and T are m &times; m matrices.</p>
    /// <p>This implementation only uses the upper part of the matrix, the part below the
    /// diagonal is not accessed at all.</p>
    /// <p>Transformation to tridiagonal shape is often not a goal by itself, but it is
    /// an intermediate step in more general decomposition algorithms like {@link
    /// EigenDecomposition eigen decomposition}. This class is therefore intended for internal
    /// use by the library and is not public. As a consequence of this explicitly limited scope,
    /// many methods directly returns references to internal arrays, not copies.</p>
    /// </summary>
    public class TriDiagonalTransformer
    {
        private double[,] householderVectors;
        private double[] main;
        private double[] secondary;
        private DenseMatrix cachedQ;
        private DenseMatrix cachedQt;
        private DenseMatrix cachedT;

        public double[,] HouseholderVectors
        {
            get { return householderVectors; }
        }
        public double[] MainDiagonal
        {
            get { return main; }
        }

        public double[] SecondaryDiagonal
        {
            get { return secondary; }
        }

        public TriDiagonalTransformer(DenseMatrix matrix)
        {
            if (!matrix.IsSquare())
            {
                throw new IndexOutOfRangeException(String.Format("The matrix given parameter is not shape of square, Rows: {0}, Cols: {1}", matrix.RowCount, matrix.ColumnCount));
            }

            int m = matrix.RowCount;
            householderVectors = matrix.AsArrayEx();
            main = new double[m];
            secondary = new double[m - 1];
            cachedQ = null;
            cachedQt = null;
            cachedT = null;

            // transform matrix
            transform();
        }

        public DenseMatrix GetQ()
        {
            if (cachedQ == null)
            {
                cachedQ = (DenseMatrix)GetQT().Transpose();
            }
            return cachedQ;
        }

        public DenseMatrix GetQT()
        {
            if (cachedQt == null)
            {
                int m = householderVectors.Rows();
                cachedQt = (DenseMatrix)MathNetMatrixUtility.CreateMatrix<double>(m, m);

                // build up first part of the matrix by applying Householder transforms
                for (int k = m - 1; k >= 1; --k)
                {
                    double[] hK = householderVectors.GetRow(k - 1);
                    double inv = 1.0 / (secondary[k - 1] * hK[k]);
                    cachedQt[k, k] = 1;
                    if (hK[k] != 0.0)
                    {
                        double beta = 1.0 / secondary[k - 1];
                        cachedQt[k, k] = 1 + beta * hK[k];
                        for (int i = k + 1; i < m; ++i)
                        {
                            cachedQt[k, i] = beta * hK[i];
                        }
                        for (int j = k + 1; j < m; ++j)
                        {
                            beta = 0;
                            for (int i = k + 1; i < m; ++i)
                            {
                                beta += cachedQt[j, i] * hK[i];
                            }
                            beta *= inv;
                            cachedQt[j, k] = beta * hK[k];
                            for (int i = k + 1; i < m; ++i)
                            {
                                cachedQt[j, i] += beta * hK[i];
                            }
                        }
                    }
                }
                cachedQt[0, 0] = 1;

                //cachedQt = new DenseMatrix(qta.Rows(), qta.GetMaxColumnLength());
                //for (int i = 0; i < qta.GetLength(0); i++)
                //    for (int j = 0; j < qta.GetLength(1); j++)
                //        cachedQt.At(i, j, qta[i, j]);
            }

            // return the cached matrix
            return cachedQt;
        }

        public DenseMatrix GetT()
        {
            if (cachedT == null)
            {
                int m = main.Length;
                double[, ] ta = new double[m, m];
                for (int i = 0; i < m; ++i)
                {
                    ta[i, i] = main[i];
                    if (i > 0)
                    {
                        ta[i, i - 1] = secondary[i - 1];
                    }
                    if (i < main.Length - 1)
                    {
                        ta[i, i + 1] = secondary[i];
                    }
                }

                cachedT = new DenseMatrix(ta.GetLength(0), ta.GetLength(1));
                for (int i = 0; i < ta.GetLength(0); i++)
                    for (int j = 0; j < ta.GetLength(1); j++)
                        cachedQt.At(i, j, ta[i, j]);
            }

            // return the cached matrix
            return cachedT;
        }

        private void transform()
        {
            int m = householderVectors.Rows();

            int flag = 0, flag1 = 0, flag2 = 0, flag3 = 0;


            double[] z = new double[m];
            for (int k = 0; k < m - 1; k++)
            {

                //zero-out a row and a column simultaneously
                //double[] hK = householderVectors.GetRow(k);
                main[k] = householderVectors[k, k];
                double xNormSqr = 0;
                for (int j = k + 1; j < m; ++j)
                {
                    double c = householderVectors[k, j];
                    xNormSqr += c * c;
                }
                double a = (householderVectors[k, k + 1] > 0) ? -System.Math.Sqrt(xNormSqr) : System.Math.Sqrt(xNormSqr);
                secondary[k] = a;
                if (a != 0.0)
                {
                    // apply Householder transform from left and right simultaneously

                    householderVectors[k, k + 1] -= a;
                    double beta = -1 / (a * householderVectors[k, k + 1]);

                    // compute a = beta A v, where v is the Householder vector
                    // this loop is written in such a way
                    //   1) only the upper triangular part of the matrix is accessed
                    //   2) access is cache-friendly for a matrix stored in rows
                    //Array.Clear(z, k + 1, (m - k - 1));
                    //z.Fill(k + 1, (m - k - 1), 0);
                    z.Fill(k + 1, m, 0);

                    for (int i = k + 1; i < m; ++i)
                    {
                        //double[] hI = householderVectors.GetRow(i);
                        double hKI = householderVectors[k, i];
                        double zI = householderVectors[i, i] * hKI;
                        for (int j = i + 1; j < m; ++j)
                        {
                            if (j == 2)
                            {
                                flag1++;
                            }

                            double hIJ = householderVectors[i, j];
                            zI += hIJ * householderVectors[k, j];
                            z[j] += hIJ * hKI;
                        }

                        if (i == 2)
                        {
                            flag2++; 
                        }

                        z[i] = beta * (z[i] + zI);
                    }

                    // compute gamma = beta vT z / 2
                    double gamma = 0;
                    for (int i = k + 1; i < m; ++i)
                    {
                        gamma += z[i] * householderVectors[k, i];
                    }
                    gamma *= beta / 2;

                    // compute z = z - gamma v
                    for (int i = k + 1; i < m; ++i)
                    {
                        if (i == 2)
                        {
                            flag3++; 
                        }

                        z[i] -= gamma * householderVectors[k, i];
                    }

                    // update matrix: A = A - v zT - z vT
                    // only the upper triangular part of the matrix is updated
                    for (int i = k + 1; i < m; ++i)
                    {
                        //double[] hI = householderVectors.GetRow(i);
                        for (int j = i; j < m; ++j)
                        {
                            if (i == 2 && j == 2)
                            {
                                flag++;
                            }

                            householderVectors[i, j] -= householderVectors[k, i] * z[j] + z[i] * householderVectors[k, j];
                            //householderVectors[i, j] = hI[j];
                        }
                    }
                }
            }
            main[m - 1] = householderVectors[m - 1, m - 1];
        }
    }
}
