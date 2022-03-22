using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Mercury.Language.Properties;

namespace Mercury.Language
{
    public class LocalizedResources : INotifyPropertyChanged
    {
        #region Singleton Class Implementation
        private readonly Resources resources = new Resources();

        private static LocalizedResources instance;

        /// <summary>
        /// Constructor implement as Singleton pattern
        /// </summary>
        private LocalizedResources()
        {
        }

        /// <summary>
        /// Return singleton instance
        /// </summary>
        /// <returns>Return current instance</returns>
        public static LocalizedResources Instance()
        {
            if (instance == null)
                instance = new LocalizedResources();

            return instance;
        }

        /// <summary>
        /// Hangling culture changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Change resource culture change
        /// </summary>
        /// <param name="name"></param>
        public void ChangeCulture(string name)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(name);
            RaisePropertyChanged("Resources");
        }

        /// <summary>
        /// Get resource
        /// </summary>
        internal Resources Resources
        {
            get { return resources; }
        }

        #endregion

        public String ARG_MUST_BE_PERIOD
        {
            get { return Resources.ARG_MUST_BE_PERIOD; }
        }
        public String ARG_CANNOT_BE_NaN
        {
            get { return Resources.ARG_CANNOT_BE_NaN; }
        }
        public String ARGUMENT_OUTSIDE_DOMAIN
        {
            get { return Resources.ARGUMENT_OUTSIDE_DOMAIN; }
        }
        public String ARRAY_SIZE_EXCEEDS_MAX_VARIABLES
        {
            get { return Resources.ARRAY_SIZE_EXCEEDS_MAX_VARIABLES; }
        }
        public String ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1
        {
            get { return Resources.ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1; }
        }
        public String ARRAY_SUMS_TO_ZERO
        {
            get { return Resources.ARRAY_SUMS_TO_ZERO; }
        }
        public String ASSYMETRIC_EIGEN_NOT_SUPPORTED
        {
            get { return Resources.ASSYMETRIC_EIGEN_NOT_SUPPORTED; }
        }
        public String AT_LEAST_ONE_COLUMN
        {
            get { return Resources.AT_LEAST_ONE_COLUMN; }
        }
        public String AT_LEAST_ONE_ROW
        {
            get { return Resources.AT_LEAST_ONE_ROW; }
        }
        public String BANDWIDTH
        {
            get { return Resources.BANDWIDTH; }
        }
        public String BESSEL_FUNCTION_BAD_ARGUMENT
        {
            get { return Resources.BESSEL_FUNCTION_BAD_ARGUMENT; }
        }
        public String BESSEL_FUNCTION_FAILED_CONVERGENCE
        {
            get { return Resources.BESSEL_FUNCTION_FAILED_CONVERGENCE; }
        }
        public String BINOMIAL_INVALID_PARAMETERS_ORDER
        {
            get { return Resources.BINOMIAL_INVALID_PARAMETERS_ORDER; }
        }
        public String BINOMIAL_NEGATIVE_PARAMETER
        {
            get { return Resources.BINOMIAL_NEGATIVE_PARAMETER; }
        }
        public String CANNOT_CLEAR_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS
        {
            get { return Resources.CANNOT_CLEAR_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS; }
        }
        public String CANNOT_COMPUTE_0TH_ROOT_OF_UNITY
        {
            get { return Resources.CANNOT_COMPUTE_0TH_ROOT_OF_UNITY; }
        }
        public String CANNOT_COMPUTE_BETA_DENSITY_AT_0_FOR_SOME_ALPHA
        {
            get { return Resources.CANNOT_COMPUTE_BETA_DENSITY_AT_0_FOR_SOME_ALPHA; }
        }
        public String CANNOT_COMPUTE_BETA_DENSITY_AT_1_FOR_SOME_BETA
        {
            get { return Resources.CANNOT_COMPUTE_BETA_DENSITY_AT_1_FOR_SOME_BETA; }
        }
        public String CANNOT_COMPUTE_NTH_ROOT_FOR_NEGATIVE_N
        {
            get { return Resources.CANNOT_COMPUTE_NTH_ROOT_FOR_NEGATIVE_N; }
        }
        public String CANNOT_DISCARD_NEGATIVE_NUMBER_OF_ELEMENTS
        {
            get { return Resources.CANNOT_DISCARD_NEGATIVE_NUMBER_OF_ELEMENTS; }
        }
        public String CANNOT_FORMAT_INSTANCE_AS_3D_VECTOR
        {
            get { return Resources.CANNOT_FORMAT_INSTANCE_AS_3D_VECTOR; }
        }
        public String CANNOT_FORMAT_INSTANCE_AS_COMPLEX
        {
            get { return Resources.CANNOT_FORMAT_INSTANCE_AS_COMPLEX; }
        }
        public String CANNOT_FORMAT_INSTANCE_AS_REAL_VECTOR
        {
            get { return Resources.CANNOT_FORMAT_INSTANCE_AS_REAL_VECTOR; }
        }
        public String CANNOT_FORMAT_OBJECT_TO_FRACTION
        {
            get { return Resources.CANNOT_FORMAT_OBJECT_TO_FRACTION; }
        }
        public String CANNOT_INCREMENT_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS
        {
            get { return Resources.CANNOT_INCREMENT_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS; }
        }
        public String CANNOT_NORMALIZE_A_ZERO_NORM_VECTOR
        {
            get { return Resources.CANNOT_NORMALIZE_A_ZERO_NORM_VECTOR; }
        }
        public String CANNOT_RETRIEVE_AT_NEGATIVE_INDEX
        {
            get { return Resources.CANNOT_RETRIEVE_AT_NEGATIVE_INDEX; }
        }
        public String CANNOT_SET_AT_NEGATIVE_INDEX
        {
            get { return Resources.CANNOT_SET_AT_NEGATIVE_INDEX; }
        }
        public String CANNOT_SUBSTITUTE_ELEMENT_FROM_EMPTY_ARRAY
        {
            get { return Resources.CANNOT_SUBSTITUTE_ELEMENT_FROM_EMPTY_ARRAY; }
        }
        public String CANNOT_TRANSFORM_TO_DOUBLE
        {
            get { return Resources.CANNOT_TRANSFORM_TO_DOUBLE; }
        }
        public String CARDAN_ANGLES_SINGULARITY
        {
            get { return Resources.CARDAN_ANGLES_SINGULARITY; }
        }
        public String CLASS_DOESNT_IMPLEMENT_COMPARABLE
        {
            get { return Resources.CLASS_DOESNT_IMPLEMENT_COMPARABLE; }
        }
        public String CLOSEST_ORTHOGONAL_MATRIX_HAS_NEGATIVE_DETERMINANT
        {
            get { return Resources.CLOSEST_ORTHOGONAL_MATRIX_HAS_NEGATIVE_DETERMINANT; }
        }
        public String COLUMN_INDEX_OUT_OF_RANGE
        {
            get { return Resources.COLUMN_INDEX_OUT_OF_RANGE; }
        }
        public String COLUMN_INDEX
        {
            get { return Resources.COLUMN_INDEX; }
        }
        public String CONSTRAINT
        {
            get { return Resources.CONSTRAINT; }
        }
        public String CONTINUED_FRACTION_INFINITY_DIVERGENCE
        {
            get { return Resources.CONTINUED_FRACTION_INFINITY_DIVERGENCE; }
        }
        public String CONTINUED_FRACTION_NAN_DIVERGENCE
        {
            get { return Resources.CONTINUED_FRACTION_NAN_DIVERGENCE; }
        }
        public String CONTRACTION_CRITERIA_SMALLER_THAN_EXPANSION_FACTOR
        {
            get { return Resources.CONTRACTION_CRITERIA_SMALLER_THAN_EXPANSION_FACTOR; }
        }
        public String CONTRACTION_CRITERIA_SMALLER_THAN_ONE
        {
            get { return Resources.CONTRACTION_CRITERIA_SMALLER_THAN_ONE; }
        }
        public String CONVERGENCE_FAILED
        {
            get { return Resources.CONVERGENCE_FAILED; }
        }
        public String CROSSING_BOUNDARY_LOOPS
        {
            get { return Resources.CROSSING_BOUNDARY_LOOPS; }
        }
        public String CROSSOVER_RATE
        {
            get { return Resources.CROSSOVER_RATE; }
        }
        public String CUMULATIVE_PROBABILITY_RETURNED_NAN
        {
            get { return Resources.CUMULATIVE_PROBABILITY_RETURNED_NAN; }
        }
        public String DIFFERENT_ROWS_LENGTHS
        {
            get { return Resources.DIFFERENT_ROWS_LENGTHS; }
        }
        public String DIFFERENT_ORIG_AND_PERMUTED_DATA
        {
            get { return Resources.DIFFERENT_ORIG_AND_PERMUTED_DATA; }
        }
        public String DIGEST_NOT_INITIALIZED
        {
            get { return Resources.DIGEST_NOT_INITIALIZED; }
        }
        public String DIMENSIONS_MISMATCH_2x2
        {
            get { return Resources.DIMENSIONS_MISMATCH_2x2; }
        }
        public String DIMENSIONS_MISMATCH_SIMPLE
        {
            get { return Resources.DIMENSIONS_MISMATCH_SIMPLE; }
        }
        public String DIMENSIONS_MISMATCH
        {
            get { return Resources.DIMENSIONS_MISMATCH; }
        }
        public String DISCRETE_CUMULATIVE_PROBABILITY_RETURNED_NAN
        {
            get { return Resources.DISCRETE_CUMULATIVE_PROBABILITY_RETURNED_NAN; }
        }
        public String DISTRIBUTION_NOT_LOADED
        {
            get { return Resources.DISTRIBUTION_NOT_LOADED; }
        }
        public String DUPLICATED_ABSCISSA_DIVISION_BY_ZERO
        {
            get { return Resources.DUPLICATED_ABSCISSA_DIVISION_BY_ZERO; }
        }
        public String ELITISM_RATE
        {
            get { return Resources.ELITISM_RATE; }
        }
        public String EMPTY_CLUSTER_IN_K_MEANS
        {
            get { return Resources.EMPTY_CLUSTER_IN_K_MEANS; }
        }
        public String EMPTY_INTERPOLATION_SAMPLE
        {
            get { return Resources.EMPTY_INTERPOLATION_SAMPLE; }
        }
        public String EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY
        {
            get { return Resources.EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY; }
        }
        public String EMPTY_SELECTED_COLUMN_INDEX_ARRAY
        {
            get { return Resources.EMPTY_SELECTED_COLUMN_INDEX_ARRAY; }
        }
        public String EMPTY_SELECTED_ROW_INDEX_ARRAY
        {
            get { return Resources.EMPTY_SELECTED_ROW_INDEX_ARRAY; }
        }
        public String EMPTY_STRING_FOR_IMAGINARY_CHARACTER
        {
            get { return Resources.EMPTY_STRING_FOR_IMAGINARY_CHARACTER; }
        }
        public String ENDPOINTS_NOT_AN_INTERVAL
        {
            get { return Resources.ENDPOINTS_NOT_AN_INTERVAL; }
        }
        public String EQUAL_VERTICES_IN_SIMPLEX
        {
            get { return Resources.EQUAL_VERTICES_IN_SIMPLEX; }
        }
        public String EULER_ANGLES_SINGULARITY
        {
            get { return Resources.EULER_ANGLES_SINGULARITY; }
        }
        public String EVALUATION
        {
            get { return Resources.EVALUATION; }
        }
        public String EXPANSION_FACTOR_SMALLER_THAN_ONE
        {
            get { return Resources.EXPANSION_FACTOR_SMALLER_THAN_ONE; }
        }
        public String FACTORIAL_NEGATIVE_PARAMETER
        {
            get { return Resources.FACTORIAL_NEGATIVE_PARAMETER; }
        }
        public String FAILED_BRACKETING
        {
            get { return Resources.FAILED_BRACKETING; }
        }
        public String FAILED_FRACTION_CONVERSION
        {
            get { return Resources.FAILED_FRACTION_CONVERSION; }
        }
        public String FIRST_COLUMNS_NOT_INITIALIZED_YET
        {
            get { return Resources.FIRST_COLUMNS_NOT_INITIALIZED_YET; }
        }
        public String FIRST_ELEMENT_NOT_ZERO
        {
            get { return Resources.FIRST_ELEMENT_NOT_ZERO; }
        }
        public String FIRST_ROWS_NOT_INITIALIZED_YET
        {
            get { return Resources.FIRST_ROWS_NOT_INITIALIZED_YET; }
        }
        public String FRACTION_CONVERSION_OVERFLOW
        {
            get { return Resources.FRACTION_CONVERSION_OVERFLOW; }
        }
        public String FUNCTION_NOT_DIFFERENTIABLE
        {
            get { return Resources.FUNCTION_NOT_DIFFERENTIABLE; }
        }
        public String FUNCTION_NOT_POLYNOMIAL
        {
            get { return Resources.FUNCTION_NOT_POLYNOMIAL; }
        }
        public String GCD_OVERFLOW_32_BITS
        {
            get { return Resources.GCD_OVERFLOW_32_BITS; }
        }
        public String GCD_OVERFLOW_64_BITS
        {
            get { return Resources.GCD_OVERFLOW_64_BITS; }
        }
        public String HOLE_BETWEEN_MODELS_TIME_RANGES
        {
            get { return Resources.HOLE_BETWEEN_MODELS_TIME_RANGES; }
        }
        public String ILL_CONDITIONED_OPERATOR
        {
            get { return Resources.ILL_CONDITIONED_OPERATOR; }
        }
        public String INCONSISTENT_STATE_AT_2_PI_WRAPPING
        {
            get { return Resources.INCONSISTENT_STATE_AT_2_PI_WRAPPING; }
        }
        public String INDEX_LARGER_THAN_MAX
        {
            get { return Resources.INDEX_LARGER_THAN_MAX; }
        }
        public String INDEX_NOT_POSITIVE
        {
            get { return Resources.INDEX_NOT_POSITIVE; }
        }
        public String INDEX_OUT_OF_RANGE
        {
            get { return Resources.INDEX_OUT_OF_RANGE; }
        }
        public String INDEX
        {
            get { return Resources.INDEX; }
        }
        public String NOT_FINITE_NUMBER
        {
            get { return Resources.NOT_FINITE_NUMBER; }
        }
        public String INFINITE_BOUND
        {
            get { return Resources.INFINITE_BOUND; }
        }
        public String ARRAY_ELEMENT
        {
            get { return Resources.ARRAY_ELEMENT; }
        }
        public String INFINITE_ARRAY_ELEMENT
        {
            get { return Resources.INFINITE_ARRAY_ELEMENT; }
        }
        public String INFINITE_VALUE_CONVERSION
        {
            get { return Resources.INFINITE_VALUE_CONVERSION; }
        }
        public String INITIAL_CAPACITY_NOT_POSITIVE
        {
            get { return Resources.INITIAL_CAPACITY_NOT_POSITIVE; }
        }
        public String INITIAL_COLUMN_AFTER_FINAL_COLUMN
        {
            get { return Resources.INITIAL_COLUMN_AFTER_FINAL_COLUMN; }
        }
        public String INITIAL_ROW_AFTER_FINAL_ROW
        {
            get { return Resources.INITIAL_ROW_AFTER_FINAL_ROW; }
        }
        public String INPUT_DATA_FROM_UNSUPPORTED_DATASOURCE
        {
            get { return Resources.INPUT_DATA_FROM_UNSUPPORTED_DATASOURCE; }
        }
        public String INSTANCES_NOT_COMPARABLE_TO_EXISTING_VALUES
        {
            get { return Resources.INSTANCES_NOT_COMPARABLE_TO_EXISTING_VALUES; }
        }
        public String INSUFFICIENT_DATA
        {
            get { return Resources.INSUFFICIENT_DATA; }
        }
        public String INSUFFICIENT_DATA_FOR_T_STATISTIC
        {
            get { return Resources.INSUFFICIENT_DATA_FOR_T_STATISTIC; }
        }
        public String INSUFFICIENT_DIMENSION
        {
            get { return Resources.INSUFFICIENT_DIMENSION; }
        }
        public String DIMENSION
        {
            get { return Resources.DIMENSION; }
        }
        public String INSUFFICIENT_OBSERVED_POINTS_IN_SAMPLE
        {
            get { return Resources.INSUFFICIENT_OBSERVED_POINTS_IN_SAMPLE; }
        }
        public String INSUFFICIENT_ROWS_AND_COLUMNS
        {
            get { return Resources.INSUFFICIENT_ROWS_AND_COLUMNS; }
        }
        public String INTEGRATION_METHOD_NEEDS_AT_LEAST_TWO_PREVIOUS_POINTS
        {
            get { return Resources.INTEGRATION_METHOD_NEEDS_AT_LEAST_TWO_PREVIOUS_POINTS; }
        }
        public String INTERNAL_ERROR
        {
            get { return Resources.INTERNAL_ERROR; }
        }
        public String INVALID_BINARY_DIGIT
        {
            get { return Resources.INVALID_BINARY_DIGIT; }
        }
        public String INVALID_BINARY_CHROMOSOME
        {
            get { return Resources.INVALID_BINARY_CHROMOSOME; }
        }
        public String INVALID_BRACKETING_PARAMETERS
        {
            get { return Resources.INVALID_BRACKETING_PARAMETERS; }
        }
        public String INVALID_FIXED_LENGTH_CHROMOSOME
        {
            get { return Resources.INVALID_FIXED_LENGTH_CHROMOSOME; }
        }
        public String INVALID_IMPLEMENTATION
        {
            get { return Resources.INVALID_IMPLEMENTATION; }
        }
        public String INVALID_INTERVAL_INITIAL_VALUE_PARAMETERS
        {
            get { return Resources.INVALID_INTERVAL_INITIAL_VALUE_PARAMETERS; }
        }
        public String INVALID_ITERATIONS_LIMITS
        {
            get { return Resources.INVALID_ITERATIONS_LIMITS; }
        }
        public String INVALID_MAX_ITERATIONS
        {
            get { return Resources.INVALID_MAX_ITERATIONS; }
        }
        public String NOT_ENOUGH_DATA_REGRESSION
        {
            get { return Resources.NOT_ENOUGH_DATA_REGRESSION; }
        }
        public String INVALID_REGRESSION_ARRAY
        {
            get { return Resources.INVALID_REGRESSION_ARRAY; }
        }
        public String INVALID_REGRESSION_OBSERVATION
        {
            get { return Resources.INVALID_REGRESSION_OBSERVATION; }
        }
        public String INVALID_ROUNDING_METHOD
        {
            get { return Resources.INVALID_ROUNDING_METHOD; }
        }
        public String ITERATOR_EXHAUSTED
        {
            get { return Resources.ITERATOR_EXHAUSTED; }
        }
        public String ITERATIONS
        {
            get { return Resources.ITERATIONS; }
        }
        public String LCM_OVERFLOW_32_BITS
        {
            get { return Resources.LCM_OVERFLOW_32_BITS; }
        }
        public String LCM_OVERFLOW_64_BITS
        {
            get { return Resources.LCM_OVERFLOW_64_BITS; }
        }
        public String LIST_OF_CHROMOSOMES_BIGGER_THAN_POPULATION_SIZE
        {
            get { return Resources.LIST_OF_CHROMOSOMES_BIGGER_THAN_POPULATION_SIZE; }
        }
        public String LOESS_EXPECTS_AT_LEAST_ONE_POINT
        {
            get { return Resources.LOESS_EXPECTS_AT_LEAST_ONE_POINT; }
        }
        public String LOWER_BOUND_NOT_BELOW_UPPER_BOUND
        {
            get { return Resources.LOWER_BOUND_NOT_BELOW_UPPER_BOUND; }
        }
        public String LOWER_ENDPOINT_ABOVE_UPPER_ENDPOINT
        {
            get { return Resources.LOWER_ENDPOINT_ABOVE_UPPER_ENDPOINT; }
        }
        public String MAP_MODIFIED_WHILE_ITERATING
        {
            get { return Resources.MAP_MODIFIED_WHILE_ITERATING; }
        }
        public String EVALUATIONS
        {
            get { return Resources.EVALUATIONS; }
        }
        public String MAX_COUNT_EXCEEDED
        {
            get { return Resources.MAX_COUNT_EXCEEDED; }
        }
        public String MAX_ITERATIONS_EXCEEDED
        {
            get { return Resources.MAX_ITERATIONS_EXCEEDED; }
        }
        public String MINIMAL_STEPSIZE_REACHED_DURING_INTEGRATION
        {
            get { return Resources.MINIMAL_STEPSIZE_REACHED_DURING_INTEGRATION; }
        }
        public String MISMATCHED_LOESS_ABSCISSA_ORDINATE_ARRAYS
        {
            get { return Resources.MISMATCHED_LOESS_ABSCISSA_ORDINATE_ARRAYS; }
        }
        public String MUTATION_RATE
        {
            get { return Resources.MUTATION_RATE; }
        }
        public String NAN_ELEMENT_AT_INDEX
        {
            get { return Resources.NAN_ELEMENT_AT_INDEX; }
        }
        public String NAN_VALUE_CONVERSION
        {
            get { return Resources.NAN_VALUE_CONVERSION; }
        }
        public String NEGATIVE_BRIGHTNESS_EXPONENT
        {
            get { return Resources.NEGATIVE_BRIGHTNESS_EXPONENT; }
        }
        public String NEGATIVE_COMPLEX_MODULE
        {
            get { return Resources.NEGATIVE_COMPLEX_MODULE; }
        }
        public String NEGATIVE_ELEMENT_AT_2D_INDEX
        {
            get { return Resources.NEGATIVE_ELEMENT_AT_2D_INDEX; }
        }
        public String NEGATIVE_ELEMENT_AT_INDEX
        {
            get { return Resources.NEGATIVE_ELEMENT_AT_INDEX; }
        }
        public String NEGATIVE_NUMBER_OF_SUCCESSES
        {
            get { return Resources.NEGATIVE_NUMBER_OF_SUCCESSES; }
        }
        public String NUMBER_OF_SUCCESSES
        {
            get { return Resources.NUMBER_OF_SUCCESSES; }
        }
        public String NEGATIVE_NUMBER_OF_TRIALS
        {
            get { return Resources.NEGATIVE_NUMBER_OF_TRIALS; }
        }
        public String NUMBER_OF_INTERPOLATION_POINTS
        {
            get { return Resources.NUMBER_OF_INTERPOLATION_POINTS; }
        }
        public String NUMBER_OF_TRIALS
        {
            get { return Resources.NUMBER_OF_TRIALS; }
        }
        public String NOT_CONVEX
        {
            get { return Resources.NOT_CONVEX; }
        }
        public String ROBUSTNESS_ITERATIONS
        {
            get { return Resources.ROBUSTNESS_ITERATIONS; }
        }
        public String START_POSITION
        {
            get { return Resources.START_POSITION; }
        }
        public String NON_CONVERGENT_CONTINUED_FRACTION
        {
            get { return Resources.NON_CONVERGENT_CONTINUED_FRACTION; }
        }
        public String NON_INVERTIBLE_TRANSFORM
        {
            get { return Resources.NON_INVERTIBLE_TRANSFORM; }
        }
        public String NON_POSITIVE_MICROSPHERE_ELEMENTS
        {
            get { return Resources.NON_POSITIVE_MICROSPHERE_ELEMENTS; }
        }
        public String NON_POSITIVE_POLYNOMIAL_DEGREE
        {
            get { return Resources.NON_POSITIVE_POLYNOMIAL_DEGREE; }
        }
        public String NON_REAL_FINITE_ABSCISSA
        {
            get { return Resources.NON_REAL_FINITE_ABSCISSA; }
        }
        public String NON_REAL_FINITE_ORDINATE
        {
            get { return Resources.NON_REAL_FINITE_ORDINATE; }
        }
        public String NON_REAL_FINITE_WEIGHT
        {
            get { return Resources.NON_REAL_FINITE_WEIGHT; }
        }
        public String NON_SQUARE_MATRIX
        {
            get { return Resources.NON_SQUARE_MATRIX; }
        }
        public String NORM
        {
            get { return Resources.NORM; }
        }
        public String NORMALIZE_INFINITE
        {
            get { return Resources.NORMALIZE_INFINITE; }
        }
        public String NORMALIZE_NAN
        {
            get { return Resources.NORMALIZE_NAN; }
        }
        public String NOT_ADDITION_COMPATIBLE_MATRICES
        {
            get { return Resources.NOT_ADDITION_COMPATIBLE_MATRICES; }
        }
        public String NOT_DECREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_DECREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_DECREASING_SEQUENCE
        {
            get { return Resources.NOT_DECREASING_SEQUENCE; }
        }
        public String NOT_ENOUGH_DATA_FOR_NUMBER_OF_PREDICTORS
        {
            get { return Resources.NOT_ENOUGH_DATA_FOR_NUMBER_OF_PREDICTORS; }
        }
        public String NOT_ENOUGH_POINTS_IN_SPLINE_PARTITION
        {
            get { return Resources.NOT_ENOUGH_POINTS_IN_SPLINE_PARTITION; }
        }
        public String NOT_INCREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_INCREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_INCREASING_SEQUENCE
        {
            get { return Resources.NOT_INCREASING_SEQUENCE; }
        }
        public String NOT_MULTIPLICATION_COMPATIBLE_MATRICES
        {
            get { return Resources.NOT_MULTIPLICATION_COMPATIBLE_MATRICES; }
        }
        public String NOT_POSITIVE_DEFINITE_MATRIX
        {
            get { return Resources.NOT_POSITIVE_DEFINITE_MATRIX; }
        }
        public String NON_POSITIVE_DEFINITE_MATRIX
        {
            get { return Resources.NON_POSITIVE_DEFINITE_MATRIX; }
        }
        public String NON_POSITIVE_DEFINITE_OPERATOR
        {
            get { return Resources.NON_POSITIVE_DEFINITE_OPERATOR; }
        }
        public String NON_SELF_ADJOINT_OPERATOR
        {
            get { return Resources.NON_SELF_ADJOINT_OPERATOR; }
        }
        public String NON_SQUARE_OPERATOR
        {
            get { return Resources.NON_SQUARE_OPERATOR; }
        }
        public String DEGREES_OF_FREEDOM
        {
            get { return Resources.DEGREES_OF_FREEDOM; }
        }
        public String NOT_POSITIVE_DEGREES_OF_FREEDOM
        {
            get { return Resources.NOT_POSITIVE_DEGREES_OF_FREEDOM; }
        }
        public String NOT_POSITIVE_ELEMENT_AT_INDEX
        {
            get { return Resources.NOT_POSITIVE_ELEMENT_AT_INDEX; }
        }
        public String NOT_POSITIVE_EXPONENT
        {
            get { return Resources.NOT_POSITIVE_EXPONENT; }
        }
        public String NUMBER_OF_ELEMENTS_SHOULD_BE_POSITIVE
        {
            get { return Resources.NUMBER_OF_ELEMENTS_SHOULD_BE_POSITIVE; }
        }
        public String BASE
        {
            get { return Resources.BASE; }
        }
        public String EXPONENT
        {
            get { return Resources.EXPONENT; }
        }
        public String NOT_POSITIVE_LENGTH
        {
            get { return Resources.NOT_POSITIVE_LENGTH; }
        }
        public String LENGTH
        {
            get { return Resources.LENGTH; }
        }
        public String NOT_POSITIVE_MEAN
        {
            get { return Resources.NOT_POSITIVE_MEAN; }
        }
        public String MEAN
        {
            get { return Resources.MEAN; }
        }
        public String NOT_POSITIVE_NUMBER_OF_SAMPLES
        {
            get { return Resources.NOT_POSITIVE_NUMBER_OF_SAMPLES; }
        }
        public String NUMBER_OF_SAMPLES
        {
            get { return Resources.NUMBER_OF_SAMPLES; }
        }
        public String NOT_POSITIVE_PERMUTATION
        {
            get { return Resources.NOT_POSITIVE_PERMUTATION; }
        }
        public String PERMUTATION_SIZE
        {
            get { return Resources.PERMUTATION_SIZE; }
        }
        public String NOT_POSITIVE_POISSON_MEAN
        {
            get { return Resources.NOT_POSITIVE_POISSON_MEAN; }
        }
        public String NOT_POSITIVE_POPULATION_SIZE
        {
            get { return Resources.NOT_POSITIVE_POPULATION_SIZE; }
        }
        public String POPULATION_SIZE
        {
            get { return Resources.POPULATION_SIZE; }
        }
        public String NOT_POSITIVE_ROW_DIMENSION
        {
            get { return Resources.NOT_POSITIVE_ROW_DIMENSION; }
        }
        public String NOT_POSITIVE_SAMPLE_SIZE
        {
            get { return Resources.NOT_POSITIVE_SAMPLE_SIZE; }
        }
        public String NOT_POSITIVE_SCALE
        {
            get { return Resources.NOT_POSITIVE_SCALE; }
        }
        public String SCALE
        {
            get { return Resources.SCALE; }
        }
        public String NOT_POSITIVE_SHAPE
        {
            get { return Resources.NOT_POSITIVE_SHAPE; }
        }
        public String SHAPE
        {
            get { return Resources.SHAPE; }
        }
        public String NOT_POSITIVE_STANDARD_DEVIATION
        {
            get { return Resources.NOT_POSITIVE_STANDARD_DEVIATION; }
        }
        public String STANDARD_DEVIATION
        {
            get { return Resources.STANDARD_DEVIATION; }
        }
        public String NOT_POSITIVE_UPPER_BOUND
        {
            get { return Resources.NOT_POSITIVE_UPPER_BOUND; }
        }
        public String NOT_POSITIVE_WINDOW_SIZE
        {
            get { return Resources.NOT_POSITIVE_WINDOW_SIZE; }
        }
        public String NOT_POWER_OF_TWO
        {
            get { return Resources.NOT_POWER_OF_TWO; }
        }
        public String NOT_POWER_OF_TWO_CONSIDER_PADDING
        {
            get { return Resources.NOT_POWER_OF_TWO_CONSIDER_PADDING; }
        }
        public String NOT_POWER_OF_TWO_PLUS_ONE
        {
            get { return Resources.NOT_POWER_OF_TWO_PLUS_ONE; }
        }
        public String NOT_STRICTLY_DECREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_STRICTLY_DECREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_STRICTLY_DECREASING_SEQUENCE
        {
            get { return Resources.NOT_STRICTLY_DECREASING_SEQUENCE; }
        }
        public String NOT_STRICTLY_INCREASING_KNOT_VALUES
        {
            get { return Resources.NOT_STRICTLY_INCREASING_KNOT_VALUES; }
        }
        public String NOT_STRICTLY_INCREASING_NUMBER_OF_POINTS
        {
            get { return Resources.NOT_STRICTLY_INCREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_STRICTLY_INCREASING_SEQUENCE
        {
            get { return Resources.NOT_STRICTLY_INCREASING_SEQUENCE; }
        }
        public String NOT_SUBTRACTION_COMPATIBLE_MATRICES
        {
            get { return Resources.NOT_SUBTRACTION_COMPATIBLE_MATRICES; }
        }
        public String NOT_SUPPORTED_IN_DIMENSION_N
        {
            get { return Resources.NOT_SUPPORTED_IN_DIMENSION_N; }
        }
        public String NOT_SYMMETRIC_MATRIX
        {
            get { return Resources.NOT_SYMMETRIC_MATRIX; }
        }
        public String NON_SYMMETRIC_MATRIX
        {
            get { return Resources.NON_SYMMETRIC_MATRIX; }
        }
        public String NO_BIN_SELECTED
        {
            get { return Resources.NO_BIN_SELECTED; }
        }
        public String NO_CONVERGENCE_WITH_ANY_START_POINT
        {
            get { return Resources.NO_CONVERGENCE_WITH_ANY_START_POINT; }
        }
        public String NO_DATA
        {
            get { return Resources.NO_DATA; }
        }
        public String NO_DEGREES_OF_FREEDOM
        {
            get { return Resources.NO_DEGREES_OF_FREEDOM; }
        }
        public String NO_DENSITY_FOR_THIS_DISTRIBUTION
        {
            get { return Resources.NO_DENSITY_FOR_THIS_DISTRIBUTION; }
        }
        public String NO_FEASIBLE_SOLUTION
        {
            get { return Resources.NO_FEASIBLE_SOLUTION; }
        }
        public String NO_OPTIMUM_COMPUTED_YET
        {
            get { return Resources.NO_OPTIMUM_COMPUTED_YET; }
        }
        public String NO_REGRESSORS
        {
            get { return Resources.NO_REGRESSORS; }
        }
        public String NO_RESULT_AVAILABLE
        {
            get { return Resources.NO_RESULT_AVAILABLE; }
        }
        public String NO_SUCH_MATRIX_ENTRY
        {
            get { return Resources.NO_SUCH_MATRIX_ENTRY; }
        }
        public String NAN_NOT_ALLOWED
        {
            get { return Resources.NAN_NOT_ALLOWED; }
        }
        public String NULL_NOT_ALLOWED
        {
            get { return Resources.NULL_NOT_ALLOWED; }
        }
        public String ARRAY_ZERO_LENGTH_OR_NULL_NOT_ALLOWED
        {
            get { return Resources.ARRAY_ZERO_LENGTH_OR_NULL_NOT_ALLOWED; }
        }
        public String COVARIANCE_MATRIX
        {
            get { return Resources.COVARIANCE_MATRIX; }
        }
        public String DENOMINATOR
        {
            get { return Resources.DENOMINATOR; }
        }
        public String DENOMINATOR_FORMAT
        {
            get { return Resources.DENOMINATOR_FORMAT; }
        }
        public String FRACTION
        {
            get { return Resources.FRACTION; }
        }
        public String FUNCTION
        {
            get { return Resources.FUNCTION; }
        }
        public String IMAGINARY_FORMAT
        {
            get { return Resources.IMAGINARY_FORMAT; }
        }
        public String INPUT_ARRAY
        {
            get { return Resources.INPUT_ARRAY; }
        }
        public String NUMERATOR
        {
            get { return Resources.NUMERATOR; }
        }
        public String NUMERATOR_FORMAT
        {
            get { return Resources.NUMERATOR_FORMAT; }
        }
        public String OBJECT_TRANSFORMATION
        {
            get { return Resources.OBJECT_TRANSFORMATION; }
        }
        public String REAL_FORMAT
        {
            get { return Resources.REAL_FORMAT; }
        }
        public String WHOLE_FORMAT
        {
            get { return Resources.WHOLE_FORMAT; }
        }
        public String NUMBER_TOO_LARGE
        {
            get { return Resources.NUMBER_TOO_LARGE; }
        }
        public String NUMBER_TOO_SMALL
        {
            get { return Resources.NUMBER_TOO_SMALL; }
        }
        public String NUMBER_TOO_LARGE_BOUND_EXCLUDED
        {
            get { return Resources.NUMBER_TOO_LARGE_BOUND_EXCLUDED; }
        }
        public String NUMBER_TOO_SMALL_BOUND_EXCLUDED
        {
            get { return Resources.NUMBER_TOO_SMALL_BOUND_EXCLUDED; }
        }
        public String NUMBER_OF_SUCCESS_LARGER_THAN_POPULATION_SIZE
        {
            get { return Resources.NUMBER_OF_SUCCESS_LARGER_THAN_POPULATION_SIZE; }
        }
        public String NUMERATOR_OVERFLOW_AFTER_MULTIPLY
        {
            get { return Resources.NUMERATOR_OVERFLOW_AFTER_MULTIPLY; }
        }
        public String N_POINTS_GAUSS_LEGENDRE_INTEGRATOR_NOT_SUPPORTED
        {
            get { return Resources.N_POINTS_GAUSS_LEGENDRE_INTEGRATOR_NOT_SUPPORTED; }
        }
        public String OBSERVED_COUNTS_ALL_ZERO
        {
            get { return Resources.OBSERVED_COUNTS_ALL_ZERO; }
        }
        public String OBSERVED_COUNTS_BOTTH_ZERO_FOR_ENTRY
        {
            get { return Resources.OBSERVED_COUNTS_BOTTH_ZERO_FOR_ENTRY; }
        }
        public String BOBYQA_BOUND_DIFFERENCE_CONDITION
        {
            get { return Resources.BOBYQA_BOUND_DIFFERENCE_CONDITION; }
        }
        public String OUT_OF_BOUNDS_QUANTILE_VALUE
        {
            get { return Resources.OUT_OF_BOUNDS_QUANTILE_VALUE; }
        }
        public String OUT_OF_BOUNDS_CONFIDENCE_LEVEL
        {
            get { return Resources.OUT_OF_BOUNDS_CONFIDENCE_LEVEL; }
        }
        public String OUT_OF_BOUND_SIGNIFICANCE_LEVEL
        {
            get { return Resources.OUT_OF_BOUND_SIGNIFICANCE_LEVEL; }
        }
        public String SIGNIFICANCE_LEVEL
        {
            get { return Resources.SIGNIFICANCE_LEVEL; }
        }
        public String OUT_OF_ORDER_ABSCISSA_ARRAY
        {
            get { return Resources.OUT_OF_ORDER_ABSCISSA_ARRAY; }
        }
        public String OUT_OF_RANGE_ROOT_OF_UNITY_INDEX
        {
            get { return Resources.OUT_OF_RANGE_ROOT_OF_UNITY_INDEX; }
        }
        public String OUT_OF_RANGE
        {
            get { return Resources.OUT_OF_RANGE; }
        }
        public String OUT_OF_RANGE_SIMPLE
        {
            get { return Resources.OUT_OF_RANGE_SIMPLE; }
        }
        public String OUT_OF_RANGE_LEFT
        {
            get { return Resources.OUT_OF_RANGE_LEFT; }
        }
        public String OUT_OF_RANGE_RIGHT
        {
            get { return Resources.OUT_OF_RANGE_RIGHT; }
        }
        public String OUTLINE_BOUNDARY_LOOP_OPEN
        {
            get { return Resources.OUTLINE_BOUNDARY_LOOP_OPEN; }
        }
        public String OVERFLOW
        {
            get { return Resources.OVERFLOW; }
        }
        public String OVERFLOW_IN_FRACTION
        {
            get { return Resources.OVERFLOW_IN_FRACTION; }
        }
        public String OVERFLOW_IN_ADDITION
        {
            get { return Resources.OVERFLOW_IN_ADDITION; }
        }
        public String OVERFLOW_IN_SUBTRACTION
        {
            get { return Resources.OVERFLOW_IN_SUBTRACTION; }
        }
        public String OVERFLOW_IN_MULTIPLICATION
        {
            get { return Resources.OVERFLOW_IN_MULTIPLICATION; }
        }
        public String OVERFLOW_TIMESPAN_TOO_LONG
        {
            get { return Resources.OVERFLOW_TIMESPAN_TOO_LONG; }
        }
        public String OVERFLOW_DURATION
        {
            get { return Resources.OVERFLOW_DURATION; }
        }
        public String OVERFLOW_NEGATE_TWOS_COMP_NUM
        {
            get { return Resources.OVERFLOW_NEGATE_TWOS_COMP_NUM; }
        }
        public String PERCENTILE_IMPLEMENTATION_CANNOT_ACCESS_METHOD
        {
            get { return Resources.PERCENTILE_IMPLEMENTATION_CANNOT_ACCESS_METHOD; }
        }
        public String PERCENTILE_IMPLEMENTATION_UNSUPPORTED_METHOD
        {
            get { return Resources.PERCENTILE_IMPLEMENTATION_UNSUPPORTED_METHOD; }
        }
        public String PERMUTATION_EXCEEDS_N
        {
            get { return Resources.PERMUTATION_EXCEEDS_N; }
        }
        public String POLYNOMIAL
        {
            get { return Resources.POLYNOMIAL; }
        }
        public String POLYNOMIAL_INTERPOLANTS_MISMATCH_SEGMENTS
        {
            get { return Resources.POLYNOMIAL_INTERPOLANTS_MISMATCH_SEGMENTS; }
        }
        public String POPULATION_LIMIT_NOT_POSITIVE
        {
            get { return Resources.POPULATION_LIMIT_NOT_POSITIVE; }
        }
        public String POWER_NEGATIVE_PARAMETERS
        {
            get { return Resources.POWER_NEGATIVE_PARAMETERS; }
        }
        public String PROPAGATION_DIRECTION_MISMATCH
        {
            get { return Resources.PROPAGATION_DIRECTION_MISMATCH; }
        }
        public String RANDOMKEY_MUTATION_WRONG_CLASS
        {
            get { return Resources.RANDOMKEY_MUTATION_WRONG_CLASS; }
        }
        public String ROOTS_OF_UNITY_NOT_COMPUTED_YET
        {
            get { return Resources.ROOTS_OF_UNITY_NOT_COMPUTED_YET; }
        }
        public String ROTATION_MATRIX_DIMENSIONS
        {
            get { return Resources.ROTATION_MATRIX_DIMENSIONS; }
        }
        public String ROW_INDEX_OUT_OF_RANGE
        {
            get { return Resources.ROW_INDEX_OUT_OF_RANGE; }
        }
        public String ROW_INDEX
        {
            get { return Resources.ROW_INDEX; }
        }
        public String SAME_SIGN_AT_ENDPOINTS
        {
            get { return Resources.SAME_SIGN_AT_ENDPOINTS; }
        }
        public String SAMPLE_SIZE_EXCEEDS_COLLECTION_SIZE
        {
            get { return Resources.SAMPLE_SIZE_EXCEEDS_COLLECTION_SIZE; }
        }
        public String SAMPLE_SIZE_LARGER_THAN_POPULATION_SIZE
        {
            get { return Resources.SAMPLE_SIZE_LARGER_THAN_POPULATION_SIZE; }
        }
        public String SIMPLEX_NEED_ONE_POINT
        {
            get { return Resources.SIMPLEX_NEED_ONE_POINT; }
        }
        public String SIMPLE_MESSAGE
        {
            get { return Resources.SIMPLE_MESSAGE; }
        }
        public String SINGULAR_MATRIX
        {
            get { return Resources.SINGULAR_MATRIX; }
        }
        public String SINGULAR_OPERATOR
        {
            get { return Resources.SINGULAR_OPERATOR; }
        }
        public String SUBARRAY_ENDS_AFTER_ARRAY_END
        {
            get { return Resources.SUBARRAY_ENDS_AFTER_ARRAY_END; }
        }
        public String TOO_LARGE_CUTOFF_SINGULAR_VALUE
        {
            get { return Resources.TOO_LARGE_CUTOFF_SINGULAR_VALUE; }
        }
        public String TOO_LARGE_TOURNAMENT_ARITY
        {
            get { return Resources.TOO_LARGE_TOURNAMENT_ARITY; }
        }
        public String TOO_MANY_ELEMENTS_TO_DISCARD_FROM_ARRAY
        {
            get { return Resources.TOO_MANY_ELEMENTS_TO_DISCARD_FROM_ARRAY; }
        }
        public String TOO_MANY_REGRESSORS
        {
            get { return Resources.TOO_MANY_REGRESSORS; }
        }
        public String TOO_SMALL_COST_RELATIVE_TOLERANCE
        {
            get { return Resources.TOO_SMALL_COST_RELATIVE_TOLERANCE; }
        }
        public String TOO_SMALL_INTEGRATION_INTERVAL
        {
            get { return Resources.TOO_SMALL_INTEGRATION_INTERVAL; }
        }
        public String TOO_SMALL_ORTHOGONALITY_TOLERANCE
        {
            get { return Resources.TOO_SMALL_ORTHOGONALITY_TOLERANCE; }
        }
        public String TOO_SMALL_PARAMETERS_RELATIVE_TOLERANCE
        {
            get { return Resources.TOO_SMALL_PARAMETERS_RELATIVE_TOLERANCE; }
        }
        public String TRUST_REGION_STEP_FAILED
        {
            get { return Resources.TRUST_REGION_STEP_FAILED; }
        }
        public String TWO_OR_MORE_CATEGORIES_REQUIRED
        {
            get { return Resources.TWO_OR_MORE_CATEGORIES_REQUIRED; }
        }
        public String TWO_OR_MORE_VALUES_IN_CATEGORY_REQUIRED
        {
            get { return Resources.TWO_OR_MORE_VALUES_IN_CATEGORY_REQUIRED; }
        }
        public String UNABLE_TO_BRACKET_OPTIMUM_IN_LINE_SEARCH
        {
            get { return Resources.UNABLE_TO_BRACKET_OPTIMUM_IN_LINE_SEARCH; }
        }
        public String UNABLE_TO_COMPUTE_COVARIANCE_SINGULAR_PROBLEM
        {
            get { return Resources.UNABLE_TO_COMPUTE_COVARIANCE_SINGULAR_PROBLEM; }
        }
        public String UNABLE_TO_FIRST_GUESS_HARMONIC_COEFFICIENTS
        {
            get { return Resources.UNABLE_TO_FIRST_GUESS_HARMONIC_COEFFICIENTS; }
        }
        public String UNABLE_TO_ORTHOGONOLIZE_MATRIX
        {
            get { return Resources.UNABLE_TO_ORTHOGONOLIZE_MATRIX; }
        }
        public String UNABLE_TO_PERFORM_QR_DECOMPOSITION_ON_JACOBIAN
        {
            get { return Resources.UNABLE_TO_PERFORM_QR_DECOMPOSITION_ON_JACOBIAN; }
        }
        public String UNABLE_TO_SOLVE_SINGULAR_PROBLEM
        {
            get { return Resources.UNABLE_TO_SOLVE_SINGULAR_PROBLEM; }
        }
        public String UNBOUNDED_SOLUTION
        {
            get { return Resources.UNBOUNDED_SOLUTION; }
        }
        public String UNKNOWN_MODE
        {
            get { return Resources.UNKNOWN_MODE; }
        }
        public String UNKNOWN_PARAMETER
        {
            get { return Resources.UNKNOWN_PARAMETER; }
        }
        public String UNMATCHED_ODE_IN_EXPANDED_SET
        {
            get { return Resources.UNMATCHED_ODE_IN_EXPANDED_SET; }
        }
        public String CANNOT_PARSE_AS_TYPE
        {
            get { return Resources.CANNOT_PARSE_AS_TYPE; }
        }
        public String CANNOT_PARSE
        {
            get { return Resources.CANNOT_PARSE; }
        }
        public String UNPARSEABLE_3D_VECTOR
        {
            get { return Resources.UNPARSEABLE_3D_VECTOR; }
        }
        public String UNPARSEABLE_COMPLEX_NUMBER
        {
            get { return Resources.UNPARSEABLE_COMPLEX_NUMBER; }
        }
        public String UNPARSEABLE_REAL_VECTOR
        {
            get { return Resources.UNPARSEABLE_REAL_VECTOR; }
        }
        public String UNSUPPORTED_EXPANSION_MODE
        {
            get { return Resources.UNSUPPORTED_EXPANSION_MODE; }
        }
        public String UNSUPPORTED_OPERATION
        {
            get { return Resources.UNSUPPORTED_OPERATION; }
        }
        public String ARITHMETIC_EXCEPTION
        {
            get { return Resources.ARITHMETIC_EXCEPTION; }
        }
        public String ILLEGAL_STATE
        {
            get { return Resources.ILLEGAL_STATE; }
        }
        public String USER_EXCEPTION
        {
            get { return Resources.USER_EXCEPTION; }
        }
        public String URL_CONTAINS_NO_DATA
        {
            get { return Resources.URL_CONTAINS_NO_DATA; }
        }
        public String VALUES_ADDED_BEFORE_CONFIGURING_STATISTIC
        {
            get { return Resources.VALUES_ADDED_BEFORE_CONFIGURING_STATISTIC; }
        }
        public String VECTOR_LENGTH_MISMATCH
        {
            get { return Resources.VECTOR_LENGTH_MISMATCH; }
        }
        public String VECTOR_MUST_HAVE_AT_LEAST_ONE_ELEMENT
        {
            get { return Resources.VECTOR_MUST_HAVE_AT_LEAST_ONE_ELEMENT; }
        }
        public String WEIGHT_AT_LEAST_ONE_NON_ZERO
        {
            get { return Resources.WEIGHT_AT_LEAST_ONE_NON_ZERO; }
        }
        public String WRONG_BLOCK_LENGTH
        {
            get { return Resources.WRONG_BLOCK_LENGTH; }
        }
        public String WRONG_NUMBER_OF_POINTS
        {
            get { return Resources.WRONG_NUMBER_OF_POINTS; }
        }
        public String NUMBER_OF_POINTS
        {
            get { return Resources.NUMBER_OF_POINTS; }
        }
        public String ZERO_DENOMINATOR
        {
            get { return Resources.ZERO_DENOMINATOR; }
        }
        public String ZERO_DENOMINATOR_IN_FRACTION
        {
            get { return Resources.ZERO_DENOMINATOR_IN_FRACTION; }
        }
        public String ZERO_FRACTION_TO_DIVIDE_BY
        {
            get { return Resources.ZERO_FRACTION_TO_DIVIDE_BY; }
        }
        public String ZERO_NORM
        {
            get { return Resources.ZERO_NORM; }
        }
        public String ZERO_NORM_FOR_ROTATION_AXIS
        {
            get { return Resources.ZERO_NORM_FOR_ROTATION_AXIS; }
        }
        public String ZERO_NORM_FOR_ROTATION_DEFINING_VECTOR
        {
            get { return Resources.ZERO_NORM_FOR_ROTATION_DEFINING_VECTOR; }
        }
        public String ZERO_NOT_ALLOWED
        {
            get { return Resources.ZERO_NOT_ALLOWED; }
        }
        public String Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow
        {
            get { return Resources.Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow; }
        }
        public String Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime
        {
            get { return Resources.Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime; }
        }
        public String Utility_Extension_Array_GetSymmetric_MatrixTypeCanBeEitherLowerTriangularOrUpperTrianguler
        {
            get { return Resources.Utility_Extension_Array_GetSymmetric_MatrixTypeCanBeEitherLowerTriangularOrUpperTrianguler; }
        }
        public String Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace
        {
            get { return Resources.Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace; }
        }
        public String Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray
        {
            get { return Resources.Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray; }
        }
        public String Utility_ArgumentChecker_String_NotEmpty_InputParameterMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_String_NotEmpty_InputParameterMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Array_NotEmpty_InputParameterArrayMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_Array_NotEmpty_InputParameterArrayMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Enumerable_NotEmpty_InputParameterIterableMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_Enumerable_NotEmpty_InputParameterIterableMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Collection_NotEmpty_InputParameterCollectionMustNotBeEmpty
        {
            get { return Resources.Utility_ArgumentChecker_Collection_NotEmpty_InputParameterCollectionMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Generic_NotNull_InputParameterMustNotBeNull
        {
            get { return Resources.Utility_ArgumentChecker_Generic_NotNull_InputParameterMustNotBeNull; }
        }
        public String Utility_ArgumentChecker_Generic_NotNullInjected_InjectedInputParameterMustNotBeNull
        {
            get { return Resources.Utility_ArgumentChecker_Generic_NotNullInjected_InjectedInputParameterMustNotBeNull; }
        }
        public String Utility_ArgumentChecker_GenericArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericEnumerable_NoNulls_InputParameterEnumerableMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericEnumerable_NoNulls_InputParameterEnumerableMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericArray2D_NoNulls_InputParameter2DArrayMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericArray2D_NoNulls_InputParameter2DArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericList_NoNulls_InputParameterListMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_GenericList_NoNulls_InputParameterListMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_DynamicArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_DynamicArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_DynamicList_NoNulls_InputParameterListMustNotContainNullAtIndex
        {
            get { return Resources.Utility_ArgumentChecker_DynamicList_NoNulls_InputParameterListMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_Long_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero
        {
            get { return Resources.Utility_ArgumentChecker_Long_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero; }
        }
        public String Utility_ArgumentChecker_Double_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero
        {
            get { return Resources.Utility_ArgumentChecker_Double_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero; }
        }
        public String Utility_ArgumentChecker_Double_AlmostNotZero_InputParameterMustNotBeZero
        {
            get { return Resources.Utility_ArgumentChecker_Double_AlmostNotZero_InputParameterMustNotBeZero; }
        }
        public String Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero
        {
            get { return Resources.Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero; }
        }
        public String Utility_ArgumentChecker_Generic_InOrderOrEqual_InputParameterMustBeBefore
        {
            get { return Resources.Utility_ArgumentChecker_Generic_InOrderOrEqual_InputParameterMustBeBefore; }
        }
        public String Utility_FFT_Double1D_ComplexForward_TheDataArrayIsTooBig
        {
            get { return Resources.Utility_FFT_Double1D_ComplexForward_TheDataArrayIsTooBig; }
        }

        public String NANO_SECOND_BASE_IS_NOT_SUPPORTED
        {
            get { return Resources.NANO_SECOND_BASE_IS_NOT_SUPPORTED; }
        }

        public String MICRO_SECOND_BASE_IS_NOT_SUPPORTED
        {
            get { return Resources.MICRO_SECOND_BASE_IS_NOT_SUPPORTED; }
        }

        public String UNSUPPORTED_UNIT
        {
            get { return Resources.UNSUPPORTED_UNIT; }
        }

        public String UNSUPPORTED_FIELD
        {
            get { return Resources.UNSUPPORTED_FIELD; }
        }

        public String INVALID_DATE_DAYOFYEAR366_IS_NOT_A_LEAP_YEAR
        {
            get { return Resources.INVALID_DATE_DAYOFYEAR366_IS_NOT_A_LEAP_YEAR; }
        }

        public String FIELD_TOO_LARGE_FOR_AN_INT
        {
            get { return Resources.FIELD_TOO_LARGE_FOR_AN_INT; }
        }

        public String START_DATE_WAS_NULL
        {
            get { return Resources.START_DATE_WAS_NULL; }
        }

        public String END_DATE_WAS_NULL
        {
            get { return Resources.END_DATE_WAS_NULL; }
        }

        public String DATE_WAS_NULL
        {
            get { return Resources.DATE_WAS_NULL; }
        }
        public String INVALID_VALUE_FOR_FIELD_VALID_VALUE_RANGE
        {
            get { return Resources.INVALID_VALUE_FOR_FIELD_VALID_VALUE_RANGE; }
        }
        public String INVALID_VALUE_VALID_RANGE
        {
            get { return Resources.INVALID_VALUE_VALID_RANGE; }
        }
        public String INVALID_INT_VALUE_FOR_FIELD
        {
            get { return Resources.INVALID_INT_VALUE_FOR_FIELD; }
        }
        public String EVALUATION_FAILED_FOR_ARGUMENT
        {
            get { return Resources.EVALUATION_FAILED_FOR_ARGUMENT; }
        }
        public String CORE_SOURCE_DESTINATION_OBJECTS_ARE_NULL
        {
            get { return Resources.CORE_SOURCE_DESTINATION_OBJECTS_ARE_NULL; }
        }
        public String OPTIMIZATION_EXCEPTION
        {
            get { return Resources.OPTIMIZATION_EXCEPTION; }
        }
        public String ARRAY_INVALID_ORDER_DIRECTION
        {
            get { return Resources.ARRAY_INVALID_ORDER_DIRECTION; }
        }
        public String ARRAY_THE_ARRAY_IS_NON_MONOTONIC_SEQUENCE
        {
            get { return Resources.ARRAY_THE_ARRAY_IS_NON_MONOTONIC_SEQUENCE; }
        }
        public String ARRAY_ORDER
        {
            get { return Resources.ARRAY_ORDER; }
        }
        public String BIGINTEGER_VALUE
        {
            get { return Resources.BIGINTEGER_VALUE; }
        }

        public String BITARRAY_FROMINDEX_IS_NEGATIVE
        {
            get { return Resources.BITARRAY_FROMINDEX_IS_NEGATIVE; }
        }

        public String DECIMAL_COMPLEX_CANNOT_OPERATE_WITH_THIS_CONDITION
        {
            get { return Resources.DECIMAL_COMPLEX_CANNOT_OPERATE_WITH_THIS_CONDITION; }
        }

        public String DENSE_MATRIX_ROW_COUNT_COLUMN_COUNT_NOT_MATCH
        {
            get { return Resources.DENSE_MATRIX_ROW_COUNT_COLUMN_COUNT_NOT_MATCH; }
        }

        public String NUMBERS_OF_KEYS_AND_VALUE_NOT_MATCH
        {
            get { return Resources.NUMBERS_OF_KEYS_AND_VALUE_NOT_MATCH; }
        }

        public String n_MUST_BE_POSITIVE_INT
        {
            get { return Resources.n_MUST_BE_POSITIVE_INT; }
        }

        public String RANDOM_DIMENSION_MUST_BE_GREATER_THAN_ZERO
        {
            get { return Resources.RANDOM_DIMENSION_MUST_BE_GREATER_THAN_ZERO; }
        }

        public String RANDOM_NUMBER_OF_VALUES_MUST_BE_GREATER_THAN_ZERO
        {
            get { return Resources.RANDOM_NUMBER_OF_VALUES_MUST_BE_GREATER_THAN_ZERO; }
        }

        public String PRINT_ERROR
        {
            get { return Resources.PRINT_ERROR; }
        }

        public String PRINT_FAILED_IN_PRINTING
        {
            get { return Resources.PRINT_FAILED_IN_PRINTING; }
        }
        public String LARGEARRAY_SRCPOS_SIZE_ERROR
        {
            get { return Resources.LARGEARRAY_SRCPOS_SIZE_ERROR; }
        }
        public String LARGEARRAY_DESTPOS_SIZE_ERROR
        {
            get { return Resources.LARGEARRAY_DESTPOS_SIZE_ERROR; }
        }
        public String LARGEARRAY_LENGTH_ERROR
        {
            get { return Resources.LARGEARRAY_LENGTH_ERROR; }
        }
        public String LARGEARRAY_CONSTANT_ARRAYS_CANNOT_BE_MODIFIED
        {
            get { return Resources.LARGEARRAY_CONSTANT_ARRAYS_CANNOT_BE_MODIFIED; }
        }

        public String LINKEDDICTIONARY_COULD_NOT_FIND_THE_KEY
        {
            get { return Resources.LINKEDDICTIONARY_COULD_NOT_FIND_THE_KEY; }
        }

        public String LINKEDHASHSET_OTHER_CANNOT_BE_NULL
        {
            get { return Resources.LINKEDHASHSET_OTHER_CANNOT_BE_NULL; }
        }

        public String METHOD_VALUE_MUST_NOT_BE_NULL
        {
            get { return Resources.METHOD_VALUE_MUST_NOT_BE_NULL; }
        }

        public String METHOD_ADDITION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_ADDITION_OVERFLOWS_INT; }
        }

        public String METHOD_ADDITION_OVERFLOWS_LONG
        {
            get { return Resources.METHOD_ADDITION_OVERFLOWS_LONG; }
        }

        public String METHOD_SUBTRACTION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_SUBTRACTION_OVERFLOWS_INT; }
        }

        public String METHOD_SUBTRACTION_OVERFLOWS_LONG
        {
            get { return Resources.METHOD_SUBTRACTION_OVERFLOWS_LONG; }
        }

        public String METHOD_MULTIPLICTION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_MULTIPLICTION_OVERFLOWS_INT; }
        }

        public String METHOD_MULTIPLICTION_OVERFLOWS_LONG
        {
            get { return Resources.METHOD_MULTIPLICTION_OVERFLOWS_LONG; }
        }

        public String METHOD_CALCULACTION_OVERFLOWS_INT
        {
            get { return Resources.METHOD_CALCULACTION_OVERFLOWS_INT; }
        }

        public String METHOD_PARAMETER_MUST_NOT_BE_NULL
        {
            get { return Resources.METHOD_PARAMETER_MUST_NOT_BE_NULL; }
        }

        public String TREEDICTIONARY_ARRAY_NOT_SUFFICIENT_SIZE
        {
            get { return Resources.TREEDICTIONARY_ARRAY_NOT_SUFFICIENT_SIZE; }
        }

        public String TREEDICTIONARY_AN_ELEMENT_WITH_THE_SAME_KEY_ALREADY_EXISTS
        {
            get { return Resources.TREEDICTIONARY_AN_ELEMENT_WITH_THE_SAME_KEY_ALREADY_EXISTS; }
        }

        public String FUNCTION_GIVEN_DATA_IS_EMPTY
        {
            get { return Resources.FUNCTION_GIVEN_DATA_IS_EMPTY; }
        }

        public String ARRAY_ONLY_SUPPORTED_THIS_TIME
        {
            get { return Resources.ARRAY_ONLY_SUPPORTED_THIS_TIME; }
        }

        public String DSCOMPLIER_NOT_POSITIVE
        {
            get { return Resources.DSCOMPLIER_NOT_POSITIVE; }
        }

        public String BIGDECIMAL_THE_VALUE_CANNOT_FIT_INTO
        {
            get { return Resources.BIGDECIMAL_THE_VALUE_CANNOT_FIT_INTO; }
        }

        public String BIGDECIMAL_CANNOT_CAST_TO_CHAR
        {
            get { return Resources.BIGDECIMAL_CANNOT_CAST_TO_CHAR; }
        }

        public String BIGDECIMAL_CANNOT_CAST_TO_DATETIME
        {
            get { return Resources.BIGDECIMAL_CANNOT_CAST_TO_DATETIME; }
        }

        public String BIGDECIMAL_COMPARE_TO_OBJECT_MUST_BE_A_BIGDECIMAL
        {
            get { return Resources.BIGDECIMAL_COMPARE_TO_OBJECT_MUST_BE_A_BIGDECIMAL; }
        }

        public String DECOMPOSITION_DESTROYED
        {
            get { return Resources.DECOMPOSITION_DESTROYED; }
        }
        public String DECOMPOSITION_UNDEFINED
        {
            get { return Resources.DECOMPOSITION_UNDEFINED; }
        }
        public String MATRIX_IS_NOT_POSITIVE_DEFINITE
        {
            get { return Resources.MATRIX_IS_NOT_POSITIVE_DEFINITE; }
        }

        public String MATRIX_CANNOT_BE_NULL
        {
            get { return Resources.MATRIX_CANNOT_BE_NULL; }
        }
        public String MATRIX_IS_NOT_A_SQUARE_MATRIX
        {
            get { return Resources.MATRIX_IS_NOT_A_SQUARE_MATRIX; }
        }
        public String MATRIX_DIMENSIONS_DO_NOT_MATCH
        {
            get { return Resources.MATRIX_DIMENSIONS_DO_NOT_MATCH; }
        }

        public String MATRIX_MUST_BE_SQUARE
        {
            get { return Resources.MATRIX_MUST_BE_SQUARE; }
        }

        public String MATRIX_IS_SINGULAR
        {
            get { return Resources.MATRIX_IS_SINGULAR; }
        }

        public String MATRIX_SHOULD_HAVE_THE_SAME_NUMBER_OF_ROWS
        {
            get { return Resources.MATRIX_SHOULD_HAVE_THE_SAME_NUMBER_OF_ROWS; }
        }

        public String VECTOR_SHOULD_HAVE_THE_SAME_LENGTH_AS_ROW
        {
            get { return Resources.VECTOR_SHOULD_HAVE_THE_SAME_LENGTH_AS_ROW; }
        }

        public String MATRIX_HAS_MORE_COLUMN_TNAN_ROWS
        {
            get { return Resources.MATRIX_HAS_MORE_COLUMN_TNAN_ROWS; }
        }
        public String MATRIX_ROW_DIMENSIONS_MUST_AGREE
        {
            get { return Resources.MATRIX_ROW_DIMENSIONS_MUST_AGREE; }
        }
        public String MATRIX_IS_RANK_DEFICIENT
        {
            get { return Resources.MATRIX_IS_RANK_DEFICIENT; }
        }

        public String n_MUST_BE_GREATER_THAN_ZERO
        {
            get { return Resources.n_MUST_BE_GREATER_THAN_ZERO; }
        }

        public String DATA_ARRAY_IS_TOO_BIG
        {
            get { return Resources.DATA_ARRAY_IS_TOO_BIG; }
        }

        public String PARAMETER_MUST_BE_HIGHER_THAN
        {
            get { return Resources.PARAMETER_MUST_BE_HIGHER_THAN; }
        }

        public String PARAMETER_IS_OUT_OF_RANGE
        {
            get { return Resources.PARAMETER_IS_OUT_OF_RANGE; }
        }

        public String ASSERTION_FAILURE
        {
            get { return Resources.ASSERTION_FAILURE; }
        }

        public String INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER
        {
            get { return Resources.INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER; }
        }
        public String INVALID_OPERATION_NEGATIVE_BASE_AND_NON_INTEGER_POWER
        {
            get { return Resources.INVALID_OPERATION_ZERO_BASE_AND_NEGATIVE_POWER; }
        }

        public String CANNOT_CALCULATE_SQUARE_ROOT_FROM_A_NEGATIVE_NUMBER
        {
            get { return Resources.CANNOT_CALCULATE_SQUARE_ROOT_FROM_A_NEGATIVE_NUMBER; }
        }

        public String PARAMETER_MUST_BE_IN
        {
            get { return Resources.PARAMETER_MUST_BE_IN; }
        }

        public String INVALID_ARGUMENTS
        {
            get { return Resources.INVALID_ARGUMENTS; }
        }

        public String FUNCTION_COMPUTATION_RESULTED_IN_ARITHMETIC_OVERFLOW
        {
            get { return Resources.FUNCTION_COMPUTATION_RESULTED_IN_ARITHMETIC_OVERFLOW; }
        }

        public String THE_INPUT_PARAMETER_MUST_BE_POSITIVE
        {
            get { return Resources.THE_INPUT_PARAMETER_MUST_BE_POSITIVE; }
        }

        public String RESULT_TOO_LARGE_REPRESENT_IN_A_LONG_INTEGER
        {
            get { return Resources.RESULT_TOO_LARGE_REPRESENT_IN_A_LONG_INTEGER; }
        }

        public String MUST_HAVE_N_IS_MORE_THAN_OR_EQUALS_FOR_N_ABSOLUTE
        {
            get { return Resources.MUST_HAVE_N_IS_MORE_THAN_OR_EQUALS_FOR_N_ABSOLUTE; }
        }

        public String MUST_HAVE_N_IS_MORE_THAN_FOR_N_ABSOLUTE
        {
            get { return Resources.MUST_HAVE_N_IS_MORE_THAN_FOR_N_ABSOLUTE; }
        }

        public String ALL_VECTORS_MUST_HAVE_THE_SAME_DIMENSIONALITY
        {
            get { return Resources.ALL_VECTORS_MUST_HAVE_THE_SAME_DIMENSIONALITY; }
        }

        public String MATRIX_COLUMN_DIMENSIONS_MUST_AGREE
        {
            get { return Resources.MATRIX_COLUMN_DIMENSIONS_MUST_AGREE; }
        }

        public String CAN_ONLY_DO_FACTORIZATION_FOR_DENSE_MATRICES_AT_THE_MOMENT
        {
            get { return Resources.CAN_ONLY_DO_FACTORIZATION_FOR_DENSE_MATRICES_AT_THE_MOMENT; }
        }

        public String CAN_ONLY_DO_FACTORIZATION_FOR_DENSE_VECTORS_AT_THE_MOMENT
        {
            get { return Resources.CAN_ONLY_DO_FACTORIZATION_FOR_DENSE_VECTORS_AT_THE_MOMENT; }
        }

        public String MATRIX_MUST_BE_SYMMETRIC
        {
            get { return Resources.MATRIX_MUST_BE_SYMMETRIC; }
        }

        public String MATRIX_DIMENSIONS_MUST_AGREE
        {
            get { return Resources.MATRIX_DIMENSIONS_MUST_AGREE; }
        }

        public String MATRIX_MUST_BE_POSITIVE_DEFINITE
        {
            get { return Resources.MATRIX_MUST_BE_POSITIVE_DEFINITE; }
        }

        public String MATRIX_MUST_NOT_BE_RANK_DEFICIENT
        {
            get { return Resources.MATRIX_MUST_NOT_BE_RANK_DEFICIENT; }
        }

        public String SINGULAR_VECTORS_WERE_NOT_COMPUTED
        {
            get { return Resources.SINGULAR_VECTORS_WERE_NOT_COMPUTED; }
        }

        public String REQUESTED_MATRIX_DOES_NOT_EXIST
        {
            get { return Resources.REQUESTED_MATRIX_DOES_NOT_EXIST; }
        }

        public String MATRIX_MUST_NOT_BE_SINGULAR
        {
            get { return Resources.MATRIX_MUST_NOT_BE_SINGULAR; }
        }

        public String PERMUTATIONS_IN_DIAGONAL_MATRIX_ARE_NOT_ALLOWED
        {
            get { return Resources.PERMUTATIONS_IN_DIAGONAL_MATRIX_ARE_NOT_ALLOWED; }
        }

        public String THE_ARRAY_ARGUMENTS_MUST_HAVE_THE_SAME_LENGTH
        {
            get { return Resources.THE_ARRAY_ARGUMENTS_MUST_HAVE_THE_SAME_LENGTH; }
        }

        public String THE_NUMBER_OF_ROWS_MUST_GREATER_THAN_OR_EQUAL_TO_THE_NUMBER_OF_COLUMNS
        {
            get { return Resources.THE_NUMBER_OF_ROWS_MUST_GREATER_THAN_OR_EQUAL_TO_THE_NUMBER_OF_COLUMNS; }
        }
        public string MATRIX_MUST_BE_IN_SPARSE_STORAGE_FORMAT
        {
            get { return Resources.MATRIX_MUST_BE_IN_SPARSE_STORAGE_FORMAT; }
        }

        public String ZERO_PIVOT_ENCOUNTERED_ON_ROW_DURING_ILU_PROCESS
        {
            get { return Resources.ZERO_PIVOT_ENCOUNTERED_ON_ROW_DURING_ILU_PROCESS; }
        }

        public String KERNEL_SIZE_MUST_BE_ODD_AND_HIGHER_THAN_2
        {
            get { return Resources.KERNEL_SIZE_MUST_BE_ODD_AND_HIGHER_THAN_2; }
        }

        public String ARGUMENTS_MUST_BE_DIFFERENT_OBJECTS
        {
            get { return Resources.ARGUMENTS_MUST_BE_DIFFERENT_OBJECTS; }
        }

        public String THE_GIVEN_ARRAY_HAS_THE_WRONG_LENGTH
        {
            get { return Resources.THE_GIVEN_ARRAY_HAS_THE_WRONG_LENGTH; }
        }

        public String THE_GIVEN_ARRAY_IS_TOO_SMALL
        {
            get { return Resources.THE_GIVEN_ARRAY_IS_TOO_SMALL; }
        }

        public String SPECIFIC_NATIVE_PROVIDER_DISABLED_BY_AN_APPLICATION_SWITCH
        {
            get { return Resources.SPECIFIC_NATIVE_PROVIDER_DISABLED_BY_AN_APPLICATION_SWITCH; }
        }

        public String NATIVE_PROVIDER_DISABLED_BY_AN_APPLICATION_SWITCH
        {
            get { return Resources.NATIVE_PROVIDER_DISABLED_BY_AN_APPLICATION_SWITCH; }
        }

        public String NATIVE_PROVIDER_PROBING_IS_DISABLED_BY_AN_APPLICATION_SWITCH
        {
            get { return Resources.NATIVE_PROVIDER_PROBING_IS_DISABLED_BY_AN_APPLICATION_SWITCH; }
        }

        public String NATIVE_PROVIDER_PROBING_FAILED_TO_RESOLVE_CREATOR
        {
            get { return Resources.NATIVE_PROVIDER_PROBING_FAILED_TO_RESOLVE_CREATOR; }
        }

        public String ARGUMENT_CANNOT_BE_NEGATIVE
        {
            get { return Resources.ARGUMENT_CANNOT_BE_NEGATIVE; }
        }

        public String INVALID_CURRENCY_CODE
        {
            get { return Resources.INVALID_CURRENCY_CODE; }
        }

        public String UNABLE_TO_PARSE_AMOUNT_INVALID_FORMAT
        {
            get { return Resources.UNABLE_TO_PARSE_AMOUNT_INVALID_FORMAT; }
        }

        public String UNABLE_TO_PARSE_AMOUNT
        {
            get { return Resources.UNABLE_TO_PARSE_AMOUNT; }
        }

        public String DO_NOT_HAVE_AN_AMOUNT_WITH_CURRENCY
        {
            get { return Resources.DO_NOT_HAVE_AN_AMOUNT_WITH_CURRENCY; }
        }

        public String OBJECT_IS_NOT_AN_INSTANT
        {
            get { return Resources.OBJECT_IS_NOT_AN_INSTANT; }
        }

        public String INVALID_VALUE
        {
            get { return Resources.INVALID_VALUE; }
        }

        public String COULD_NOT_GET_PERIOD_FOR
        {
            get { return Resources.COULD_NOT_GET_PERIOD_FOR; }
        }

        public String COULD_NOT_GET_BUSINESS_DAY_TENOR_FOR
        {
            get { return Resources.COULD_NOT_GET_BUSINESS_DAY_TENOR_FOR; }
        }

        public String MINIMAL_NUMBER_OF_DAYS_IS_INVALID
        {
            get { return Resources.MINIMAL_NUMBER_OF_DAYS_IS_INVALID; }
        }

        public String SOURCE_AND_INDEXES_ARRAYS_MUST_HAVE_THE_SAME_DIMENSION
        {
            get { return Resources.SOURCE_AND_INDEXES_ARRAYS_MUST_HAVE_THE_SAME_DIMENSION; }
        }
        public String ARGUMENT_OUT_OF_RANGE
        {
            get { return Resources.ARGUMENT_OUT_OF_RANGE; }
        }
        public String THE_DESTINATION_MATRIX_MUST_BE_BIG_ENOUGH
        {
            get { return Resources.THE_DESTINATION_MATRIX_MUST_BE_BIG_ENOUGH; }
        }
    }
}
