using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;
using Mercury.Language.Math.Analysis;
using Mercury.Language.Math.Analysis.Function;

namespace Mercury.Language.Math.Analysis.Integration
{
    interface IUnivariateIntegrator: IConvergingAlgorithm
    {
        /// <summary>
        /// Get/set the lower limit for the number of iterations.
        /// <p>
        /// Minimal iteration is needed to avoid false early convergence, e.g.
        /// the sample points happen to be zeroes of the function. Users can
        /// use the default value or choose one that they see as appropriate.</p>
        /// <p>
        /// A <code>ConvergenceException</code> will be thrown if this number is not met.</p>
        /// </summary>
        /// <param name="count">minimum number of iterations</param>
        int MinimalIterationCount { get; set; }

        /// <summary>
        /// Reset the lower limit for the number of iterations to the default.
        /// <p>
        /// The default value is supplied by the implementation.</p>
        /// </summary>
        /// <see cref="setMinimalIterationCount(int)"/>
        void ResetMinimalIterationCount();

        /// <summary>
        /// Integrate the function in the given interval.
        /// </summary>
        /// <param name="min">the lower bound for the interval</param>
        /// <param name="max">the upper bound for the interval</param>
        /// <returns>the value of integral</returns>
        /// <exception cref="ConvergenceException"></exception>
        /// <exception cref="FunctionEvaluationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        double Integrate(double min, double max);

        /// <summary>
        /// Integrate the function in the given interval.
        /// </summary>
        /// <param name="f">the integrand function</param>
        /// <param name="min">the lower bound for the interval</param>
        /// <param name="max">the upper bound for the interval</param>
        /// <returns>the value of integral</returns>
        /// <exception cref="ConvergenceException">if the maximum iteration count is exceeded or the integrator detects convergence problems otherwise</exception>
        /// <exception cref="FunctionEvaluationException">if an error occurs evaluating the function</exception>
        /// <exception cref="ArgumentException">if min > max or the endpoints do not satisfy the requirements specified by the integrator</exception>
        double Integrate(IUnivariateRealFunction f, double min, double max);

        /// <summary>
        /// Get the result of the last run of the integrator.
        /// </summary>
        /// <returns>the last result</returns>
        /// <exception cref="InvalidOperationException">if there is no result available, either because no result was yet computed or the last attempt failed</exception>
        double Result();
    }
}
