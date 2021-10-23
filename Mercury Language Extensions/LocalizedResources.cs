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
            get { return Properties.Resources.ARG_MUST_BE_PERIOD; }
        }
        public String ARG_CANNOT_BE_NaN
        {
            get { return Properties.Resources.ARG_CANNOT_BE_NaN; }
        }
        public String ARGUMENT_OUTSIDE_DOMAIN
        {
            get { return Properties.Resources.ARGUMENT_OUTSIDE_DOMAIN; }
        }
        public String ARRAY_SIZE_EXCEEDS_MAX_VARIABLES
        {
            get { return Properties.Resources.ARRAY_SIZE_EXCEEDS_MAX_VARIABLES; }
        }
        public String ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1
        {
            get { return Properties.Resources.ARRAY_SIZES_SHOULD_HAVE_DIFFERENCE_1; }
        }
        public String ARRAY_SUMS_TO_ZERO
        {
            get { return Properties.Resources.ARRAY_SUMS_TO_ZERO; }
        }
        public String ASSYMETRIC_EIGEN_NOT_SUPPORTED
        {
            get { return Properties.Resources.ASSYMETRIC_EIGEN_NOT_SUPPORTED; }
        }
        public String AT_LEAST_ONE_COLUMN
        {
            get { return Properties.Resources.AT_LEAST_ONE_COLUMN; }
        }
        public String AT_LEAST_ONE_ROW
        {
            get { return Properties.Resources.AT_LEAST_ONE_ROW; }
        }
        public String BANDWIDTH
        {
            get { return Properties.Resources.BANDWIDTH; }
        }
        public String BESSEL_FUNCTION_BAD_ARGUMENT
        {
            get { return Properties.Resources.BESSEL_FUNCTION_BAD_ARGUMENT; }
        }
        public String BESSEL_FUNCTION_FAILED_CONVERGENCE
        {
            get { return Properties.Resources.BESSEL_FUNCTION_FAILED_CONVERGENCE; }
        }
        public String BINOMIAL_INVALID_PARAMETERS_ORDER
        {
            get { return Properties.Resources.BINOMIAL_INVALID_PARAMETERS_ORDER; }
        }
        public String BINOMIAL_NEGATIVE_PARAMETER
        {
            get { return Properties.Resources.BINOMIAL_NEGATIVE_PARAMETER; }
        }
        public String CANNOT_CLEAR_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS
        {
            get { return Properties.Resources.CANNOT_CLEAR_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS; }
        }
        public String CANNOT_COMPUTE_0TH_ROOT_OF_UNITY
        {
            get { return Properties.Resources.CANNOT_COMPUTE_0TH_ROOT_OF_UNITY; }
        }
        public String CANNOT_COMPUTE_BETA_DENSITY_AT_0_FOR_SOME_ALPHA
        {
            get { return Properties.Resources.CANNOT_COMPUTE_BETA_DENSITY_AT_0_FOR_SOME_ALPHA; }
        }
        public String CANNOT_COMPUTE_BETA_DENSITY_AT_1_FOR_SOME_BETA
        {
            get { return Properties.Resources.CANNOT_COMPUTE_BETA_DENSITY_AT_1_FOR_SOME_BETA; }
        }
        public String CANNOT_COMPUTE_NTH_ROOT_FOR_NEGATIVE_N
        {
            get { return Properties.Resources.CANNOT_COMPUTE_NTH_ROOT_FOR_NEGATIVE_N; }
        }
        public String CANNOT_DISCARD_NEGATIVE_NUMBER_OF_ELEMENTS
        {
            get { return Properties.Resources.CANNOT_DISCARD_NEGATIVE_NUMBER_OF_ELEMENTS; }
        }
        public String CANNOT_FORMAT_INSTANCE_AS_3D_VECTOR
        {
            get { return Properties.Resources.CANNOT_FORMAT_INSTANCE_AS_3D_VECTOR; }
        }
        public String CANNOT_FORMAT_INSTANCE_AS_COMPLEX
        {
            get { return Properties.Resources.CANNOT_FORMAT_INSTANCE_AS_COMPLEX; }
        }
        public String CANNOT_FORMAT_INSTANCE_AS_REAL_VECTOR
        {
            get { return Properties.Resources.CANNOT_FORMAT_INSTANCE_AS_REAL_VECTOR; }
        }
        public String CANNOT_FORMAT_OBJECT_TO_FRACTION
        {
            get { return Properties.Resources.CANNOT_FORMAT_OBJECT_TO_FRACTION; }
        }
        public String CANNOT_INCREMENT_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS
        {
            get { return Properties.Resources.CANNOT_INCREMENT_STATISTIC_CONSTRUCTED_FROM_EXTERNAL_MOMENTS; }
        }
        public String CANNOT_NORMALIZE_A_ZERO_NORM_VECTOR
        {
            get { return Properties.Resources.CANNOT_NORMALIZE_A_ZERO_NORM_VECTOR; }
        }
        public String CANNOT_RETRIEVE_AT_NEGATIVE_INDEX
        {
            get { return Properties.Resources.CANNOT_RETRIEVE_AT_NEGATIVE_INDEX; }
        }
        public String CANNOT_SET_AT_NEGATIVE_INDEX
        {
            get { return Properties.Resources.CANNOT_SET_AT_NEGATIVE_INDEX; }
        }
        public String CANNOT_SUBSTITUTE_ELEMENT_FROM_EMPTY_ARRAY
        {
            get { return Properties.Resources.CANNOT_SUBSTITUTE_ELEMENT_FROM_EMPTY_ARRAY; }
        }
        public String CANNOT_TRANSFORM_TO_DOUBLE
        {
            get { return Properties.Resources.CANNOT_TRANSFORM_TO_DOUBLE; }
        }
        public String CARDAN_ANGLES_SINGULARITY
        {
            get { return Properties.Resources.CARDAN_ANGLES_SINGULARITY; }
        }
        public String CLASS_DOESNT_IMPLEMENT_COMPARABLE
        {
            get { return Properties.Resources.CLASS_DOESNT_IMPLEMENT_COMPARABLE; }
        }
        public String CLOSEST_ORTHOGONAL_MATRIX_HAS_NEGATIVE_DETERMINANT
        {
            get { return Properties.Resources.CLOSEST_ORTHOGONAL_MATRIX_HAS_NEGATIVE_DETERMINANT; }
        }
        public String COLUMN_INDEX_OUT_OF_RANGE
        {
            get { return Properties.Resources.COLUMN_INDEX_OUT_OF_RANGE; }
        }
        public String COLUMN_INDEX
        {
            get { return Properties.Resources.COLUMN_INDEX; }
        }
        public String CONSTRAINT
        {
            get { return Properties.Resources.CONSTRAINT; }
        }
        public String CONTINUED_FRACTION_INFINITY_DIVERGENCE
        {
            get { return Properties.Resources.CONTINUED_FRACTION_INFINITY_DIVERGENCE; }
        }
        public String CONTINUED_FRACTION_NAN_DIVERGENCE
        {
            get { return Properties.Resources.CONTINUED_FRACTION_NAN_DIVERGENCE; }
        }
        public String CONTRACTION_CRITERIA_SMALLER_THAN_EXPANSION_FACTOR
        {
            get { return Properties.Resources.CONTRACTION_CRITERIA_SMALLER_THAN_EXPANSION_FACTOR; }
        }
        public String CONTRACTION_CRITERIA_SMALLER_THAN_ONE
        {
            get { return Properties.Resources.CONTRACTION_CRITERIA_SMALLER_THAN_ONE; }
        }
        public String CONVERGENCE_FAILED
        {
            get { return Properties.Resources.CONVERGENCE_FAILED; }
        }
        public String CROSSING_BOUNDARY_LOOPS
        {
            get { return Properties.Resources.CROSSING_BOUNDARY_LOOPS; }
        }
        public String CROSSOVER_RATE
        {
            get { return Properties.Resources.CROSSOVER_RATE; }
        }
        public String CUMULATIVE_PROBABILITY_RETURNED_NAN
        {
            get { return Properties.Resources.CUMULATIVE_PROBABILITY_RETURNED_NAN; }
        }
        public String DIFFERENT_ROWS_LENGTHS
        {
            get { return Properties.Resources.DIFFERENT_ROWS_LENGTHS; }
        }
        public String DIFFERENT_ORIG_AND_PERMUTED_DATA
        {
            get { return Properties.Resources.DIFFERENT_ORIG_AND_PERMUTED_DATA; }
        }
        public String DIGEST_NOT_INITIALIZED
        {
            get { return Properties.Resources.DIGEST_NOT_INITIALIZED; }
        }
        public String DIMENSIONS_MISMATCH_2x2
        {
            get { return Properties.Resources.DIMENSIONS_MISMATCH_2x2; }
        }
        public String DIMENSIONS_MISMATCH_SIMPLE
        {
            get { return Properties.Resources.DIMENSIONS_MISMATCH_SIMPLE; }
        }
        public String DIMENSIONS_MISMATCH
        {
            get { return Properties.Resources.DIMENSIONS_MISMATCH; }
        }
        public String DISCRETE_CUMULATIVE_PROBABILITY_RETURNED_NAN
        {
            get { return Properties.Resources.DISCRETE_CUMULATIVE_PROBABILITY_RETURNED_NAN; }
        }
        public String DISTRIBUTION_NOT_LOADED
        {
            get { return Properties.Resources.DISTRIBUTION_NOT_LOADED; }
        }
        public String DUPLICATED_ABSCISSA_DIVISION_BY_ZERO
        {
            get { return Properties.Resources.DUPLICATED_ABSCISSA_DIVISION_BY_ZERO; }
        }
        public String ELITISM_RATE
        {
            get { return Properties.Resources.ELITISM_RATE; }
        }
        public String EMPTY_CLUSTER_IN_K_MEANS
        {
            get { return Properties.Resources.EMPTY_CLUSTER_IN_K_MEANS; }
        }
        public String EMPTY_INTERPOLATION_SAMPLE
        {
            get { return Properties.Resources.EMPTY_INTERPOLATION_SAMPLE; }
        }
        public String EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY
        {
            get { return Properties.Resources.EMPTY_POLYNOMIALS_COEFFICIENTS_ARRAY; }
        }
        public String EMPTY_SELECTED_COLUMN_INDEX_ARRAY
        {
            get { return Properties.Resources.EMPTY_SELECTED_COLUMN_INDEX_ARRAY; }
        }
        public String EMPTY_SELECTED_ROW_INDEX_ARRAY
        {
            get { return Properties.Resources.EMPTY_SELECTED_ROW_INDEX_ARRAY; }
        }
        public String EMPTY_STRING_FOR_IMAGINARY_CHARACTER
        {
            get { return Properties.Resources.EMPTY_STRING_FOR_IMAGINARY_CHARACTER; }
        }
        public String ENDPOINTS_NOT_AN_INTERVAL
        {
            get { return Properties.Resources.ENDPOINTS_NOT_AN_INTERVAL; }
        }
        public String EQUAL_VERTICES_IN_SIMPLEX
        {
            get { return Properties.Resources.EQUAL_VERTICES_IN_SIMPLEX; }
        }
        public String EULER_ANGLES_SINGULARITY
        {
            get { return Properties.Resources.EULER_ANGLES_SINGULARITY; }
        }
        public String EVALUATION
        {
            get { return Properties.Resources.EVALUATION; }
        }
        public String EXPANSION_FACTOR_SMALLER_THAN_ONE
        {
            get { return Properties.Resources.EXPANSION_FACTOR_SMALLER_THAN_ONE; }
        }
        public String FACTORIAL_NEGATIVE_PARAMETER
        {
            get { return Properties.Resources.FACTORIAL_NEGATIVE_PARAMETER; }
        }
        public String FAILED_BRACKETING
        {
            get { return Properties.Resources.FAILED_BRACKETING; }
        }
        public String FAILED_FRACTION_CONVERSION
        {
            get { return Properties.Resources.FAILED_FRACTION_CONVERSION; }
        }
        public String FIRST_COLUMNS_NOT_INITIALIZED_YET
        {
            get { return Properties.Resources.FIRST_COLUMNS_NOT_INITIALIZED_YET; }
        }
        public String FIRST_ELEMENT_NOT_ZERO
        {
            get { return Properties.Resources.FIRST_ELEMENT_NOT_ZERO; }
        }
        public String FIRST_ROWS_NOT_INITIALIZED_YET
        {
            get { return Properties.Resources.FIRST_ROWS_NOT_INITIALIZED_YET; }
        }
        public String FRACTION_CONVERSION_OVERFLOW
        {
            get { return Properties.Resources.FRACTION_CONVERSION_OVERFLOW; }
        }
        public String FUNCTION_NOT_DIFFERENTIABLE
        {
            get { return Properties.Resources.FUNCTION_NOT_DIFFERENTIABLE; }
        }
        public String FUNCTION_NOT_POLYNOMIAL
        {
            get { return Properties.Resources.FUNCTION_NOT_POLYNOMIAL; }
        }
        public String GCD_OVERFLOW_32_BITS
        {
            get { return Properties.Resources.GCD_OVERFLOW_32_BITS; }
        }
        public String GCD_OVERFLOW_64_BITS
        {
            get { return Properties.Resources.GCD_OVERFLOW_64_BITS; }
        }
        public String HOLE_BETWEEN_MODELS_TIME_RANGES
        {
            get { return Properties.Resources.HOLE_BETWEEN_MODELS_TIME_RANGES; }
        }
        public String ILL_CONDITIONED_OPERATOR
        {
            get { return Properties.Resources.ILL_CONDITIONED_OPERATOR; }
        }
        public String INCONSISTENT_STATE_AT_2_PI_WRAPPING
        {
            get { return Properties.Resources.INCONSISTENT_STATE_AT_2_PI_WRAPPING; }
        }
        public String INDEX_LARGER_THAN_MAX
        {
            get { return Properties.Resources.INDEX_LARGER_THAN_MAX; }
        }
        public String INDEX_NOT_POSITIVE
        {
            get { return Properties.Resources.INDEX_NOT_POSITIVE; }
        }
        public String INDEX_OUT_OF_RANGE
        {
            get { return Properties.Resources.INDEX_OUT_OF_RANGE; }
        }
        public String INDEX
        {
            get { return Properties.Resources.INDEX; }
        }
        public String NOT_FINITE_NUMBER
        {
            get { return Properties.Resources.NOT_FINITE_NUMBER; }
        }
        public String INFINITE_BOUND
        {
            get { return Properties.Resources.INFINITE_BOUND; }
        }
        public String ARRAY_ELEMENT
        {
            get { return Properties.Resources.ARRAY_ELEMENT; }
        }
        public String INFINITE_ARRAY_ELEMENT
        {
            get { return Properties.Resources.INFINITE_ARRAY_ELEMENT; }
        }
        public String INFINITE_VALUE_CONVERSION
        {
            get { return Properties.Resources.INFINITE_VALUE_CONVERSION; }
        }
        public String INITIAL_CAPACITY_NOT_POSITIVE
        {
            get { return Properties.Resources.INITIAL_CAPACITY_NOT_POSITIVE; }
        }
        public String INITIAL_COLUMN_AFTER_FINAL_COLUMN
        {
            get { return Properties.Resources.INITIAL_COLUMN_AFTER_FINAL_COLUMN; }
        }
        public String INITIAL_ROW_AFTER_FINAL_ROW
        {
            get { return Properties.Resources.INITIAL_ROW_AFTER_FINAL_ROW; }
        }
        public String INPUT_DATA_FROM_UNSUPPORTED_DATASOURCE
        {
            get { return Properties.Resources.INPUT_DATA_FROM_UNSUPPORTED_DATASOURCE; }
        }
        public String INSTANCES_NOT_COMPARABLE_TO_EXISTING_VALUES
        {
            get { return Properties.Resources.INSTANCES_NOT_COMPARABLE_TO_EXISTING_VALUES; }
        }
        public String INSUFFICIENT_DATA
        {
            get { return Properties.Resources.INSUFFICIENT_DATA; }
        }
        public String INSUFFICIENT_DATA_FOR_T_STATISTIC
        {
            get { return Properties.Resources.INSUFFICIENT_DATA_FOR_T_STATISTIC; }
        }
        public String INSUFFICIENT_DIMENSION
        {
            get { return Properties.Resources.INSUFFICIENT_DIMENSION; }
        }
        public String DIMENSION
        {
            get { return Properties.Resources.DIMENSION; }
        }
        public String INSUFFICIENT_OBSERVED_POINTS_IN_SAMPLE
        {
            get { return Properties.Resources.INSUFFICIENT_OBSERVED_POINTS_IN_SAMPLE; }
        }
        public String INSUFFICIENT_ROWS_AND_COLUMNS
        {
            get { return Properties.Resources.INSUFFICIENT_ROWS_AND_COLUMNS; }
        }
        public String INTEGRATION_METHOD_NEEDS_AT_LEAST_TWO_PREVIOUS_POINTS
        {
            get { return Properties.Resources.INTEGRATION_METHOD_NEEDS_AT_LEAST_TWO_PREVIOUS_POINTS; }
        }
        public String INTERNAL_ERROR
        {
            get { return Properties.Resources.INTERNAL_ERROR; }
        }
        public String INVALID_BINARY_DIGIT
        {
            get { return Properties.Resources.INVALID_BINARY_DIGIT; }
        }
        public String INVALID_BINARY_CHROMOSOME
        {
            get { return Properties.Resources.INVALID_BINARY_CHROMOSOME; }
        }
        public String INVALID_BRACKETING_PARAMETERS
        {
            get { return Properties.Resources.INVALID_BRACKETING_PARAMETERS; }
        }
        public String INVALID_FIXED_LENGTH_CHROMOSOME
        {
            get { return Properties.Resources.INVALID_FIXED_LENGTH_CHROMOSOME; }
        }
        public String INVALID_IMPLEMENTATION
        {
            get { return Properties.Resources.INVALID_IMPLEMENTATION; }
        }
        public String INVALID_INTERVAL_INITIAL_VALUE_PARAMETERS
        {
            get { return Properties.Resources.INVALID_INTERVAL_INITIAL_VALUE_PARAMETERS; }
        }
        public String INVALID_ITERATIONS_LIMITS
        {
            get { return Properties.Resources.INVALID_ITERATIONS_LIMITS; }
        }
        public String INVALID_MAX_ITERATIONS
        {
            get { return Properties.Resources.INVALID_MAX_ITERATIONS; }
        }
        public String NOT_ENOUGH_DATA_REGRESSION
        {
            get { return Properties.Resources.NOT_ENOUGH_DATA_REGRESSION; }
        }
        public String INVALID_REGRESSION_ARRAY
        {
            get { return Properties.Resources.INVALID_REGRESSION_ARRAY; }
        }
        public String INVALID_REGRESSION_OBSERVATION
        {
            get { return Properties.Resources.INVALID_REGRESSION_OBSERVATION; }
        }
        public String INVALID_ROUNDING_METHOD
        {
            get { return Properties.Resources.INVALID_ROUNDING_METHOD; }
        }
        public String ITERATOR_EXHAUSTED
        {
            get { return Properties.Resources.ITERATOR_EXHAUSTED; }
        }
        public String ITERATIONS
        {
            get { return Properties.Resources.ITERATIONS; }
        }
        public String LCM_OVERFLOW_32_BITS
        {
            get { return Properties.Resources.LCM_OVERFLOW_32_BITS; }
        }
        public String LCM_OVERFLOW_64_BITS
        {
            get { return Properties.Resources.LCM_OVERFLOW_64_BITS; }
        }
        public String LIST_OF_CHROMOSOMES_BIGGER_THAN_POPULATION_SIZE
        {
            get { return Properties.Resources.LIST_OF_CHROMOSOMES_BIGGER_THAN_POPULATION_SIZE; }
        }
        public String LOESS_EXPECTS_AT_LEAST_ONE_POINT
        {
            get { return Properties.Resources.LOESS_EXPECTS_AT_LEAST_ONE_POINT; }
        }
        public String LOWER_BOUND_NOT_BELOW_UPPER_BOUND
        {
            get { return Properties.Resources.LOWER_BOUND_NOT_BELOW_UPPER_BOUND; }
        }
        public String LOWER_ENDPOINT_ABOVE_UPPER_ENDPOINT
        {
            get { return Properties.Resources.LOWER_ENDPOINT_ABOVE_UPPER_ENDPOINT; }
        }
        public String MAP_MODIFIED_WHILE_ITERATING
        {
            get { return Properties.Resources.MAP_MODIFIED_WHILE_ITERATING; }
        }
        public String EVALUATIONS
        {
            get { return Properties.Resources.EVALUATIONS; }
        }
        public String MAX_COUNT_EXCEEDED
        {
            get { return Properties.Resources.MAX_COUNT_EXCEEDED; }
        }
        public String MAX_ITERATIONS_EXCEEDED
        {
            get { return Properties.Resources.MAX_ITERATIONS_EXCEEDED; }
        }
        public String MINIMAL_STEPSIZE_REACHED_DURING_INTEGRATION
        {
            get { return Properties.Resources.MINIMAL_STEPSIZE_REACHED_DURING_INTEGRATION; }
        }
        public String MISMATCHED_LOESS_ABSCISSA_ORDINATE_ARRAYS
        {
            get { return Properties.Resources.MISMATCHED_LOESS_ABSCISSA_ORDINATE_ARRAYS; }
        }
        public String MUTATION_RATE
        {
            get { return Properties.Resources.MUTATION_RATE; }
        }
        public String NAN_ELEMENT_AT_INDEX
        {
            get { return Properties.Resources.NAN_ELEMENT_AT_INDEX; }
        }
        public String NAN_VALUE_CONVERSION
        {
            get { return Properties.Resources.NAN_VALUE_CONVERSION; }
        }
        public String NEGATIVE_BRIGHTNESS_EXPONENT
        {
            get { return Properties.Resources.NEGATIVE_BRIGHTNESS_EXPONENT; }
        }
        public String NEGATIVE_COMPLEX_MODULE
        {
            get { return Properties.Resources.NEGATIVE_COMPLEX_MODULE; }
        }
        public String NEGATIVE_ELEMENT_AT_2D_INDEX
        {
            get { return Properties.Resources.NEGATIVE_ELEMENT_AT_2D_INDEX; }
        }
        public String NEGATIVE_ELEMENT_AT_INDEX
        {
            get { return Properties.Resources.NEGATIVE_ELEMENT_AT_INDEX; }
        }
        public String NEGATIVE_NUMBER_OF_SUCCESSES
        {
            get { return Properties.Resources.NEGATIVE_NUMBER_OF_SUCCESSES; }
        }
        public String NUMBER_OF_SUCCESSES
        {
            get { return Properties.Resources.NUMBER_OF_SUCCESSES; }
        }
        public String NEGATIVE_NUMBER_OF_TRIALS
        {
            get { return Properties.Resources.NEGATIVE_NUMBER_OF_TRIALS; }
        }
        public String NUMBER_OF_INTERPOLATION_POINTS
        {
            get { return Properties.Resources.NUMBER_OF_INTERPOLATION_POINTS; }
        }
        public String NUMBER_OF_TRIALS
        {
            get { return Properties.Resources.NUMBER_OF_TRIALS; }
        }
        public String NOT_CONVEX
        {
            get { return Properties.Resources.NOT_CONVEX; }
        }
        public String ROBUSTNESS_ITERATIONS
        {
            get { return Properties.Resources.ROBUSTNESS_ITERATIONS; }
        }
        public String START_POSITION
        {
            get { return Properties.Resources.START_POSITION; }
        }
        public String NON_CONVERGENT_CONTINUED_FRACTION
        {
            get { return Properties.Resources.NON_CONVERGENT_CONTINUED_FRACTION; }
        }
        public String NON_INVERTIBLE_TRANSFORM
        {
            get { return Properties.Resources.NON_INVERTIBLE_TRANSFORM; }
        }
        public String NON_POSITIVE_MICROSPHERE_ELEMENTS
        {
            get { return Properties.Resources.NON_POSITIVE_MICROSPHERE_ELEMENTS; }
        }
        public String NON_POSITIVE_POLYNOMIAL_DEGREE
        {
            get { return Properties.Resources.NON_POSITIVE_POLYNOMIAL_DEGREE; }
        }
        public String NON_REAL_FINITE_ABSCISSA
        {
            get { return Properties.Resources.NON_REAL_FINITE_ABSCISSA; }
        }
        public String NON_REAL_FINITE_ORDINATE
        {
            get { return Properties.Resources.NON_REAL_FINITE_ORDINATE; }
        }
        public String NON_REAL_FINITE_WEIGHT
        {
            get { return Properties.Resources.NON_REAL_FINITE_WEIGHT; }
        }
        public String NON_SQUARE_MATRIX
        {
            get { return Properties.Resources.NON_SQUARE_MATRIX; }
        }
        public String NORM
        {
            get { return Properties.Resources.NORM; }
        }
        public String NORMALIZE_INFINITE
        {
            get { return Properties.Resources.NORMALIZE_INFINITE; }
        }
        public String NORMALIZE_NAN
        {
            get { return Properties.Resources.NORMALIZE_NAN; }
        }
        public String NOT_ADDITION_COMPATIBLE_MATRICES
        {
            get { return Properties.Resources.NOT_ADDITION_COMPATIBLE_MATRICES; }
        }
        public String NOT_DECREASING_NUMBER_OF_POINTS
        {
            get { return Properties.Resources.NOT_DECREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_DECREASING_SEQUENCE
        {
            get { return Properties.Resources.NOT_DECREASING_SEQUENCE; }
        }
        public String NOT_ENOUGH_DATA_FOR_NUMBER_OF_PREDICTORS
        {
            get { return Properties.Resources.NOT_ENOUGH_DATA_FOR_NUMBER_OF_PREDICTORS; }
        }
        public String NOT_ENOUGH_POINTS_IN_SPLINE_PARTITION
        {
            get { return Properties.Resources.NOT_ENOUGH_POINTS_IN_SPLINE_PARTITION; }
        }
        public String NOT_INCREASING_NUMBER_OF_POINTS
        {
            get { return Properties.Resources.NOT_INCREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_INCREASING_SEQUENCE
        {
            get { return Properties.Resources.NOT_INCREASING_SEQUENCE; }
        }
        public String NOT_MULTIPLICATION_COMPATIBLE_MATRICES
        {
            get { return Properties.Resources.NOT_MULTIPLICATION_COMPATIBLE_MATRICES; }
        }
        public String NOT_POSITIVE_DEFINITE_MATRIX
        {
            get { return Properties.Resources.NOT_POSITIVE_DEFINITE_MATRIX; }
        }
        public String NON_POSITIVE_DEFINITE_MATRIX
        {
            get { return Properties.Resources.NON_POSITIVE_DEFINITE_MATRIX; }
        }
        public String NON_POSITIVE_DEFINITE_OPERATOR
        {
            get { return Properties.Resources.NON_POSITIVE_DEFINITE_OPERATOR; }
        }
        public String NON_SELF_ADJOINT_OPERATOR
        {
            get { return Properties.Resources.NON_SELF_ADJOINT_OPERATOR; }
        }
        public String NON_SQUARE_OPERATOR
        {
            get { return Properties.Resources.NON_SQUARE_OPERATOR; }
        }
        public String DEGREES_OF_FREEDOM
        {
            get { return Properties.Resources.DEGREES_OF_FREEDOM; }
        }
        public String NOT_POSITIVE_DEGREES_OF_FREEDOM
        {
            get { return Properties.Resources.NOT_POSITIVE_DEGREES_OF_FREEDOM; }
        }
        public String NOT_POSITIVE_ELEMENT_AT_INDEX
        {
            get { return Properties.Resources.NOT_POSITIVE_ELEMENT_AT_INDEX; }
        }
        public String NOT_POSITIVE_EXPONENT
        {
            get { return Properties.Resources.NOT_POSITIVE_EXPONENT; }
        }
        public String NUMBER_OF_ELEMENTS_SHOULD_BE_POSITIVE
        {
            get { return Properties.Resources.NUMBER_OF_ELEMENTS_SHOULD_BE_POSITIVE; }
        }
        public String BASE
        {
            get { return Properties.Resources.BASE; }
        }
        public String EXPONENT
        {
            get { return Properties.Resources.EXPONENT; }
        }
        public String NOT_POSITIVE_LENGTH
        {
            get { return Properties.Resources.NOT_POSITIVE_LENGTH; }
        }
        public String LENGTH
        {
            get { return Properties.Resources.LENGTH; }
        }
        public String NOT_POSITIVE_MEAN
        {
            get { return Properties.Resources.NOT_POSITIVE_MEAN; }
        }
        public String MEAN
        {
            get { return Properties.Resources.MEAN; }
        }
        public String NOT_POSITIVE_NUMBER_OF_SAMPLES
        {
            get { return Properties.Resources.NOT_POSITIVE_NUMBER_OF_SAMPLES; }
        }
        public String NUMBER_OF_SAMPLES
        {
            get { return Properties.Resources.NUMBER_OF_SAMPLES; }
        }
        public String NOT_POSITIVE_PERMUTATION
        {
            get { return Properties.Resources.NOT_POSITIVE_PERMUTATION; }
        }
        public String PERMUTATION_SIZE
        {
            get { return Properties.Resources.PERMUTATION_SIZE; }
        }
        public String NOT_POSITIVE_POISSON_MEAN
        {
            get { return Properties.Resources.NOT_POSITIVE_POISSON_MEAN; }
        }
        public String NOT_POSITIVE_POPULATION_SIZE
        {
            get { return Properties.Resources.NOT_POSITIVE_POPULATION_SIZE; }
        }
        public String POPULATION_SIZE
        {
            get { return Properties.Resources.POPULATION_SIZE; }
        }
        public String NOT_POSITIVE_ROW_DIMENSION
        {
            get { return Properties.Resources.NOT_POSITIVE_ROW_DIMENSION; }
        }
        public String NOT_POSITIVE_SAMPLE_SIZE
        {
            get { return Properties.Resources.NOT_POSITIVE_SAMPLE_SIZE; }
        }
        public String NOT_POSITIVE_SCALE
        {
            get { return Properties.Resources.NOT_POSITIVE_SCALE; }
        }
        public String SCALE
        {
            get { return Properties.Resources.SCALE; }
        }
        public String NOT_POSITIVE_SHAPE
        {
            get { return Properties.Resources.NOT_POSITIVE_SHAPE; }
        }
        public String SHAPE
        {
            get { return Properties.Resources.SHAPE; }
        }
        public String NOT_POSITIVE_STANDARD_DEVIATION
        {
            get { return Properties.Resources.NOT_POSITIVE_STANDARD_DEVIATION; }
        }
        public String STANDARD_DEVIATION
        {
            get { return Properties.Resources.STANDARD_DEVIATION; }
        }
        public String NOT_POSITIVE_UPPER_BOUND
        {
            get { return Properties.Resources.NOT_POSITIVE_UPPER_BOUND; }
        }
        public String NOT_POSITIVE_WINDOW_SIZE
        {
            get { return Properties.Resources.NOT_POSITIVE_WINDOW_SIZE; }
        }
        public String NOT_POWER_OF_TWO
        {
            get { return Properties.Resources.NOT_POWER_OF_TWO; }
        }
        public String NOT_POWER_OF_TWO_CONSIDER_PADDING
        {
            get { return Properties.Resources.NOT_POWER_OF_TWO_CONSIDER_PADDING; }
        }
        public String NOT_POWER_OF_TWO_PLUS_ONE
        {
            get { return Properties.Resources.NOT_POWER_OF_TWO_PLUS_ONE; }
        }
        public String NOT_STRICTLY_DECREASING_NUMBER_OF_POINTS
        {
            get { return Properties.Resources.NOT_STRICTLY_DECREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_STRICTLY_DECREASING_SEQUENCE
        {
            get { return Properties.Resources.NOT_STRICTLY_DECREASING_SEQUENCE; }
        }
        public String NOT_STRICTLY_INCREASING_KNOT_VALUES
        {
            get { return Properties.Resources.NOT_STRICTLY_INCREASING_KNOT_VALUES; }
        }
        public String NOT_STRICTLY_INCREASING_NUMBER_OF_POINTS
        {
            get { return Properties.Resources.NOT_STRICTLY_INCREASING_NUMBER_OF_POINTS; }
        }
        public String NOT_STRICTLY_INCREASING_SEQUENCE
        {
            get { return Properties.Resources.NOT_STRICTLY_INCREASING_SEQUENCE; }
        }
        public String NOT_SUBTRACTION_COMPATIBLE_MATRICES
        {
            get { return Properties.Resources.NOT_SUBTRACTION_COMPATIBLE_MATRICES; }
        }
        public String NOT_SUPPORTED_IN_DIMENSION_N
        {
            get { return Properties.Resources.NOT_SUPPORTED_IN_DIMENSION_N; }
        }
        public String NOT_SYMMETRIC_MATRIX
        {
            get { return Properties.Resources.NOT_SYMMETRIC_MATRIX; }
        }
        public String NON_SYMMETRIC_MATRIX
        {
            get { return Properties.Resources.NON_SYMMETRIC_MATRIX; }
        }
        public String NO_BIN_SELECTED
        {
            get { return Properties.Resources.NO_BIN_SELECTED; }
        }
        public String NO_CONVERGENCE_WITH_ANY_START_POINT
        {
            get { return Properties.Resources.NO_CONVERGENCE_WITH_ANY_START_POINT; }
        }
        public String NO_DATA
        {
            get { return Properties.Resources.NO_DATA; }
        }
        public String NO_DEGREES_OF_FREEDOM
        {
            get { return Properties.Resources.NO_DEGREES_OF_FREEDOM; }
        }
        public String NO_DENSITY_FOR_THIS_DISTRIBUTION
        {
            get { return Properties.Resources.NO_DENSITY_FOR_THIS_DISTRIBUTION; }
        }
        public String NO_FEASIBLE_SOLUTION
        {
            get { return Properties.Resources.NO_FEASIBLE_SOLUTION; }
        }
        public String NO_OPTIMUM_COMPUTED_YET
        {
            get { return Properties.Resources.NO_OPTIMUM_COMPUTED_YET; }
        }
        public String NO_REGRESSORS
        {
            get { return Properties.Resources.NO_REGRESSORS; }
        }
        public String NO_RESULT_AVAILABLE
        {
            get { return Properties.Resources.NO_RESULT_AVAILABLE; }
        }
        public String NO_SUCH_MATRIX_ENTRY
        {
            get { return Properties.Resources.NO_SUCH_MATRIX_ENTRY; }
        }
        public String NAN_NOT_ALLOWED
        {
            get { return Properties.Resources.NAN_NOT_ALLOWED; }
        }
        public String NULL_NOT_ALLOWED
        {
            get { return Properties.Resources.NULL_NOT_ALLOWED; }
        }
        public String ARRAY_ZERO_LENGTH_OR_NULL_NOT_ALLOWED
        {
            get { return Properties.Resources.ARRAY_ZERO_LENGTH_OR_NULL_NOT_ALLOWED; }
        }
        public String COVARIANCE_MATRIX
        {
            get { return Properties.Resources.COVARIANCE_MATRIX; }
        }
        public String DENOMINATOR
        {
            get { return Properties.Resources.DENOMINATOR; }
        }
        public String DENOMINATOR_FORMAT
        {
            get { return Properties.Resources.DENOMINATOR_FORMAT; }
        }
        public String FRACTION
        {
            get { return Properties.Resources.FRACTION; }
        }
        public String FUNCTION
        {
            get { return Properties.Resources.FUNCTION; }
        }
        public String IMAGINARY_FORMAT
        {
            get { return Properties.Resources.IMAGINARY_FORMAT; }
        }
        public String INPUT_ARRAY
        {
            get { return Properties.Resources.INPUT_ARRAY; }
        }
        public String NUMERATOR
        {
            get { return Properties.Resources.NUMERATOR; }
        }
        public String NUMERATOR_FORMAT
        {
            get { return Properties.Resources.NUMERATOR_FORMAT; }
        }
        public String OBJECT_TRANSFORMATION
        {
            get { return Properties.Resources.OBJECT_TRANSFORMATION; }
        }
        public String REAL_FORMAT
        {
            get { return Properties.Resources.REAL_FORMAT; }
        }
        public String WHOLE_FORMAT
        {
            get { return Properties.Resources.WHOLE_FORMAT; }
        }
        public String NUMBER_TOO_LARGE
        {
            get { return Properties.Resources.NUMBER_TOO_LARGE; }
        }
        public String NUMBER_TOO_SMALL
        {
            get { return Properties.Resources.NUMBER_TOO_SMALL; }
        }
        public String NUMBER_TOO_LARGE_BOUND_EXCLUDED
        {
            get { return Properties.Resources.NUMBER_TOO_LARGE_BOUND_EXCLUDED; }
        }
        public String NUMBER_TOO_SMALL_BOUND_EXCLUDED
        {
            get { return Properties.Resources.NUMBER_TOO_SMALL_BOUND_EXCLUDED; }
        }
        public String NUMBER_OF_SUCCESS_LARGER_THAN_POPULATION_SIZE
        {
            get { return Properties.Resources.NUMBER_OF_SUCCESS_LARGER_THAN_POPULATION_SIZE; }
        }
        public String NUMERATOR_OVERFLOW_AFTER_MULTIPLY
        {
            get { return Properties.Resources.NUMERATOR_OVERFLOW_AFTER_MULTIPLY; }
        }
        public String N_POINTS_GAUSS_LEGENDRE_INTEGRATOR_NOT_SUPPORTED
        {
            get { return Properties.Resources.N_POINTS_GAUSS_LEGENDRE_INTEGRATOR_NOT_SUPPORTED; }
        }
        public String OBSERVED_COUNTS_ALL_ZERO
        {
            get { return Properties.Resources.OBSERVED_COUNTS_ALL_ZERO; }
        }
        public String OBSERVED_COUNTS_BOTTH_ZERO_FOR_ENTRY
        {
            get { return Properties.Resources.OBSERVED_COUNTS_BOTTH_ZERO_FOR_ENTRY; }
        }
        public String BOBYQA_BOUND_DIFFERENCE_CONDITION
        {
            get { return Properties.Resources.BOBYQA_BOUND_DIFFERENCE_CONDITION; }
        }
        public String OUT_OF_BOUNDS_QUANTILE_VALUE
        {
            get { return Properties.Resources.OUT_OF_BOUNDS_QUANTILE_VALUE; }
        }
        public String OUT_OF_BOUNDS_CONFIDENCE_LEVEL
        {
            get { return Properties.Resources.OUT_OF_BOUNDS_CONFIDENCE_LEVEL; }
        }
        public String OUT_OF_BOUND_SIGNIFICANCE_LEVEL
        {
            get { return Properties.Resources.OUT_OF_BOUND_SIGNIFICANCE_LEVEL; }
        }
        public String SIGNIFICANCE_LEVEL
        {
            get { return Properties.Resources.SIGNIFICANCE_LEVEL; }
        }
        public String OUT_OF_ORDER_ABSCISSA_ARRAY
        {
            get { return Properties.Resources.OUT_OF_ORDER_ABSCISSA_ARRAY; }
        }
        public String OUT_OF_RANGE_ROOT_OF_UNITY_INDEX
        {
            get { return Properties.Resources.OUT_OF_RANGE_ROOT_OF_UNITY_INDEX; }
        }
        public String OUT_OF_RANGE
        {
            get { return Properties.Resources.OUT_OF_RANGE; }
        }
        public String OUT_OF_RANGE_SIMPLE
        {
            get { return Properties.Resources.OUT_OF_RANGE_SIMPLE; }
        }
        public String OUT_OF_RANGE_LEFT
        {
            get { return Properties.Resources.OUT_OF_RANGE_LEFT; }
        }
        public String OUT_OF_RANGE_RIGHT
        {
            get { return Properties.Resources.OUT_OF_RANGE_RIGHT; }
        }
        public String OUTLINE_BOUNDARY_LOOP_OPEN
        {
            get { return Properties.Resources.OUTLINE_BOUNDARY_LOOP_OPEN; }
        }
        public String OVERFLOW
        {
            get { return Properties.Resources.OVERFLOW; }
        }
        public String OVERFLOW_IN_FRACTION
        {
            get { return Properties.Resources.OVERFLOW_IN_FRACTION; }
        }
        public String OVERFLOW_IN_ADDITION
        {
            get { return Properties.Resources.OVERFLOW_IN_ADDITION; }
        }
        public String OVERFLOW_IN_SUBTRACTION
        {
            get { return Properties.Resources.OVERFLOW_IN_SUBTRACTION; }
        }
        public String OVERFLOW_IN_MULTIPLICATION
        {
            get { return Properties.Resources.OVERFLOW_IN_MULTIPLICATION; }
        }
        public String OVERFLOW_TIMESPAN_TOO_LONG
        {
            get { return Properties.Resources.OVERFLOW_TIMESPAN_TOO_LONG; }
        }
        public String OVERFLOW_DURATION
        {
            get { return Properties.Resources.OVERFLOW_DURATION; }
        }
        public String OVERFLOW_NEGATE_TWOS_COMP_NUM
        {
            get { return Properties.Resources.OVERFLOW_NEGATE_TWOS_COMP_NUM; }
        }
        public String PERCENTILE_IMPLEMENTATION_CANNOT_ACCESS_METHOD
        {
            get { return Properties.Resources.PERCENTILE_IMPLEMENTATION_CANNOT_ACCESS_METHOD; }
        }
        public String PERCENTILE_IMPLEMENTATION_UNSUPPORTED_METHOD
        {
            get { return Properties.Resources.PERCENTILE_IMPLEMENTATION_UNSUPPORTED_METHOD; }
        }
        public String PERMUTATION_EXCEEDS_N
        {
            get { return Properties.Resources.PERMUTATION_EXCEEDS_N; }
        }
        public String POLYNOMIAL
        {
            get { return Properties.Resources.POLYNOMIAL; }
        }
        public String POLYNOMIAL_INTERPOLANTS_MISMATCH_SEGMENTS
        {
            get { return Properties.Resources.POLYNOMIAL_INTERPOLANTS_MISMATCH_SEGMENTS; }
        }
        public String POPULATION_LIMIT_NOT_POSITIVE
        {
            get { return Properties.Resources.POPULATION_LIMIT_NOT_POSITIVE; }
        }
        public String POWER_NEGATIVE_PARAMETERS
        {
            get { return Properties.Resources.POWER_NEGATIVE_PARAMETERS; }
        }
        public String PROPAGATION_DIRECTION_MISMATCH
        {
            get { return Properties.Resources.PROPAGATION_DIRECTION_MISMATCH; }
        }
        public String RANDOMKEY_MUTATION_WRONG_CLASS
        {
            get { return Properties.Resources.RANDOMKEY_MUTATION_WRONG_CLASS; }
        }
        public String ROOTS_OF_UNITY_NOT_COMPUTED_YET
        {
            get { return Properties.Resources.ROOTS_OF_UNITY_NOT_COMPUTED_YET; }
        }
        public String ROTATION_MATRIX_DIMENSIONS
        {
            get { return Properties.Resources.ROTATION_MATRIX_DIMENSIONS; }
        }
        public String ROW_INDEX_OUT_OF_RANGE
        {
            get { return Properties.Resources.ROW_INDEX_OUT_OF_RANGE; }
        }
        public String ROW_INDEX
        {
            get { return Properties.Resources.ROW_INDEX; }
        }
        public String SAME_SIGN_AT_ENDPOINTS
        {
            get { return Properties.Resources.SAME_SIGN_AT_ENDPOINTS; }
        }
        public String SAMPLE_SIZE_EXCEEDS_COLLECTION_SIZE
        {
            get { return Properties.Resources.SAMPLE_SIZE_EXCEEDS_COLLECTION_SIZE; }
        }
        public String SAMPLE_SIZE_LARGER_THAN_POPULATION_SIZE
        {
            get { return Properties.Resources.SAMPLE_SIZE_LARGER_THAN_POPULATION_SIZE; }
        }
        public String SIMPLEX_NEED_ONE_POINT
        {
            get { return Properties.Resources.SIMPLEX_NEED_ONE_POINT; }
        }
        public String SIMPLE_MESSAGE
        {
            get { return Properties.Resources.SIMPLE_MESSAGE; }
        }
        public String SINGULAR_MATRIX
        {
            get { return Properties.Resources.SINGULAR_MATRIX; }
        }
        public String SINGULAR_OPERATOR
        {
            get { return Properties.Resources.SINGULAR_OPERATOR; }
        }
        public String SUBARRAY_ENDS_AFTER_ARRAY_END
        {
            get { return Properties.Resources.SUBARRAY_ENDS_AFTER_ARRAY_END; }
        }
        public String TOO_LARGE_CUTOFF_SINGULAR_VALUE
        {
            get { return Properties.Resources.TOO_LARGE_CUTOFF_SINGULAR_VALUE; }
        }
        public String TOO_LARGE_TOURNAMENT_ARITY
        {
            get { return Properties.Resources.TOO_LARGE_TOURNAMENT_ARITY; }
        }
        public String TOO_MANY_ELEMENTS_TO_DISCARD_FROM_ARRAY
        {
            get { return Properties.Resources.TOO_MANY_ELEMENTS_TO_DISCARD_FROM_ARRAY; }
        }
        public String TOO_MANY_REGRESSORS
        {
            get { return Properties.Resources.TOO_MANY_REGRESSORS; }
        }
        public String TOO_SMALL_COST_RELATIVE_TOLERANCE
        {
            get { return Properties.Resources.TOO_SMALL_COST_RELATIVE_TOLERANCE; }
        }
        public String TOO_SMALL_INTEGRATION_INTERVAL
        {
            get { return Properties.Resources.TOO_SMALL_INTEGRATION_INTERVAL; }
        }
        public String TOO_SMALL_ORTHOGONALITY_TOLERANCE
        {
            get { return Properties.Resources.TOO_SMALL_ORTHOGONALITY_TOLERANCE; }
        }
        public String TOO_SMALL_PARAMETERS_RELATIVE_TOLERANCE
        {
            get { return Properties.Resources.TOO_SMALL_PARAMETERS_RELATIVE_TOLERANCE; }
        }
        public String TRUST_REGION_STEP_FAILED
        {
            get { return Properties.Resources.TRUST_REGION_STEP_FAILED; }
        }
        public String TWO_OR_MORE_CATEGORIES_REQUIRED
        {
            get { return Properties.Resources.TWO_OR_MORE_CATEGORIES_REQUIRED; }
        }
        public String TWO_OR_MORE_VALUES_IN_CATEGORY_REQUIRED
        {
            get { return Properties.Resources.TWO_OR_MORE_VALUES_IN_CATEGORY_REQUIRED; }
        }
        public String UNABLE_TO_BRACKET_OPTIMUM_IN_LINE_SEARCH
        {
            get { return Properties.Resources.UNABLE_TO_BRACKET_OPTIMUM_IN_LINE_SEARCH; }
        }
        public String UNABLE_TO_COMPUTE_COVARIANCE_SINGULAR_PROBLEM
        {
            get { return Properties.Resources.UNABLE_TO_COMPUTE_COVARIANCE_SINGULAR_PROBLEM; }
        }
        public String UNABLE_TO_FIRST_GUESS_HARMONIC_COEFFICIENTS
        {
            get { return Properties.Resources.UNABLE_TO_FIRST_GUESS_HARMONIC_COEFFICIENTS; }
        }
        public String UNABLE_TO_ORTHOGONOLIZE_MATRIX
        {
            get { return Properties.Resources.UNABLE_TO_ORTHOGONOLIZE_MATRIX; }
        }
        public String UNABLE_TO_PERFORM_QR_DECOMPOSITION_ON_JACOBIAN
        {
            get { return Properties.Resources.UNABLE_TO_PERFORM_QR_DECOMPOSITION_ON_JACOBIAN; }
        }
        public String UNABLE_TO_SOLVE_SINGULAR_PROBLEM
        {
            get { return Properties.Resources.UNABLE_TO_SOLVE_SINGULAR_PROBLEM; }
        }
        public String UNBOUNDED_SOLUTION
        {
            get { return Properties.Resources.UNBOUNDED_SOLUTION; }
        }
        public String UNKNOWN_MODE
        {
            get { return Properties.Resources.UNKNOWN_MODE; }
        }
        public String UNKNOWN_PARAMETER
        {
            get { return Properties.Resources.UNKNOWN_PARAMETER; }
        }
        public String UNMATCHED_ODE_IN_EXPANDED_SET
        {
            get { return Properties.Resources.UNMATCHED_ODE_IN_EXPANDED_SET; }
        }
        public String CANNOT_PARSE_AS_TYPE
        {
            get { return Properties.Resources.CANNOT_PARSE_AS_TYPE; }
        }
        public String CANNOT_PARSE
        {
            get { return Properties.Resources.CANNOT_PARSE; }
        }
        public String UNPARSEABLE_3D_VECTOR
        {
            get { return Properties.Resources.UNPARSEABLE_3D_VECTOR; }
        }
        public String UNPARSEABLE_COMPLEX_NUMBER
        {
            get { return Properties.Resources.UNPARSEABLE_COMPLEX_NUMBER; }
        }
        public String UNPARSEABLE_REAL_VECTOR
        {
            get { return Properties.Resources.UNPARSEABLE_REAL_VECTOR; }
        }
        public String UNSUPPORTED_EXPANSION_MODE
        {
            get { return Properties.Resources.UNSUPPORTED_EXPANSION_MODE; }
        }
        public String UNSUPPORTED_OPERATION
        {
            get { return Properties.Resources.UNSUPPORTED_OPERATION; }
        }
        public String ARITHMETIC_EXCEPTION
        {
            get { return Properties.Resources.ARITHMETIC_EXCEPTION; }
        }
        public String ILLEGAL_STATE
        {
            get { return Properties.Resources.ILLEGAL_STATE; }
        }
        public String USER_EXCEPTION
        {
            get { return Properties.Resources.USER_EXCEPTION; }
        }
        public String URL_CONTAINS_NO_DATA
        {
            get { return Properties.Resources.URL_CONTAINS_NO_DATA; }
        }
        public String VALUES_ADDED_BEFORE_CONFIGURING_STATISTIC
        {
            get { return Properties.Resources.VALUES_ADDED_BEFORE_CONFIGURING_STATISTIC; }
        }
        public String VECTOR_LENGTH_MISMATCH
        {
            get { return Properties.Resources.VECTOR_LENGTH_MISMATCH; }
        }
        public String VECTOR_MUST_HAVE_AT_LEAST_ONE_ELEMENT
        {
            get { return Properties.Resources.VECTOR_MUST_HAVE_AT_LEAST_ONE_ELEMENT; }
        }
        public String WEIGHT_AT_LEAST_ONE_NON_ZERO
        {
            get { return Properties.Resources.WEIGHT_AT_LEAST_ONE_NON_ZERO; }
        }
        public String WRONG_BLOCK_LENGTH
        {
            get { return Properties.Resources.WRONG_BLOCK_LENGTH; }
        }
        public String WRONG_NUMBER_OF_POINTS
        {
            get { return Properties.Resources.WRONG_NUMBER_OF_POINTS; }
        }
        public String NUMBER_OF_POINTS
        {
            get { return Properties.Resources.NUMBER_OF_POINTS; }
        }
        public String ZERO_DENOMINATOR
        {
            get { return Properties.Resources.ZERO_DENOMINATOR; }
        }
        public String ZERO_DENOMINATOR_IN_FRACTION
        {
            get { return Properties.Resources.ZERO_DENOMINATOR_IN_FRACTION; }
        }
        public String ZERO_FRACTION_TO_DIVIDE_BY
        {
            get { return Properties.Resources.ZERO_FRACTION_TO_DIVIDE_BY; }
        }
        public String ZERO_NORM
        {
            get { return Properties.Resources.ZERO_NORM; }
        }
        public String ZERO_NORM_FOR_ROTATION_AXIS
        {
            get { return Properties.Resources.ZERO_NORM_FOR_ROTATION_AXIS; }
        }
        public String ZERO_NORM_FOR_ROTATION_DEFINING_VECTOR
        {
            get { return Properties.Resources.ZERO_NORM_FOR_ROTATION_DEFINING_VECTOR; }
        }
        public String ZERO_NOT_ALLOWED
        {
            get { return Properties.Resources.ZERO_NOT_ALLOWED; }
        }
        public String Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow
        {
            get { return Properties.Resources.Utility_Extension_Array_SetRow_TheValueArrayMustBeSameLengthOfTheTargetArraysRow; }
        }
        public String Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime
        {
            get { return Properties.Resources.Utility_Extension_Array_ToUpperTriangular_OnlyLowerTriangularUpperTriangularAndDiagonalMatricesAreSupportedAtThisTime; }
        }
        public String Utility_Extension_Array_GetSymmetric_MatrixTypeCanBeEitherLowerTriangularOrUpperTrianguler
        {
            get { return Properties.Resources.Utility_Extension_Array_GetSymmetric_MatrixTypeCanBeEitherLowerTriangularOrUpperTrianguler; }
        }
        public String Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace
        {
            get { return Properties.Resources.Utility_Extension_Array_Transpose_OnlySquareMatricesCanBeTransposedInPlace; }
        }
        public String Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray
        {
            get { return Properties.Resources.Utility_Extension_Array_Transpose_TheGivenObjectMustInheritFromSystemArray; }
        }
        public String Utility_ArgumentChecker_String_NotEmpty_InputParameterMustNotBeEmpty
        {
            get { return Properties.Resources.Utility_ArgumentChecker_String_NotEmpty_InputParameterMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Array_NotEmpty_InputParameterArrayMustNotBeEmpty
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Array_NotEmpty_InputParameterArrayMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Enumerable_NotEmpty_InputParameterIterableMustNotBeEmpty
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Enumerable_NotEmpty_InputParameterIterableMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Collection_NotEmpty_InputParameterCollectionMustNotBeEmpty
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Collection_NotEmpty_InputParameterCollectionMustNotBeEmpty; }
        }
        public String Utility_ArgumentChecker_Generic_NotNull_InputParameterMustNotBeNull
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Generic_NotNull_InputParameterMustNotBeNull; }
        }
        public String Utility_ArgumentChecker_Generic_NotNullInjected_InjectedInputParameterMustNotBeNull
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Generic_NotNullInjected_InjectedInputParameterMustNotBeNull; }
        }
        public String Utility_ArgumentChecker_GenericArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex
        {
            get { return Properties.Resources.Utility_ArgumentChecker_GenericArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericEnumerable_NoNulls_InputParameterEnumerableMustNotContainNullAtIndex
        {
            get { return Properties.Resources.Utility_ArgumentChecker_GenericEnumerable_NoNulls_InputParameterEnumerableMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericArray2D_NoNulls_InputParameter2DArrayMustNotContainNullAtIndex
        {
            get { return Properties.Resources.Utility_ArgumentChecker_GenericArray2D_NoNulls_InputParameter2DArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_GenericList_NoNulls_InputParameterListMustNotContainNullAtIndex
        {
            get { return Properties.Resources.Utility_ArgumentChecker_GenericList_NoNulls_InputParameterListMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_DynamicArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex
        {
            get { return Properties.Resources.Utility_ArgumentChecker_DynamicArray_NoNulls_InputParameterArrayMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_DynamicList_NoNulls_InputParameterListMustNotContainNullAtIndex
        {
            get { return Properties.Resources.Utility_ArgumentChecker_DynamicList_NoNulls_InputParameterListMustNotContainNullAtIndex; }
        }
        public String Utility_ArgumentChecker_Long_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Long_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero; }
        }
        public String Utility_ArgumentChecker_Double_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Double_NotNegativeOrZero_InputParameterMustNotBeNegativeOrZero; }
        }
        public String Utility_ArgumentChecker_Double_AlmostNotZero_InputParameterMustNotBeZero
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Double_AlmostNotZero_InputParameterMustNotBeZero; }
        }
        public String Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Double_AlmostNotNegative_InputParameterMustBeGreaterThanZero; }
        }
        public String Utility_ArgumentChecker_Generic_InOrderOrEqual_InputParameterMustBeBefore
        {
            get { return Properties.Resources.Utility_ArgumentChecker_Generic_InOrderOrEqual_InputParameterMustBeBefore; }
        }
        public String Utility_FFT_Double1D_ComplexForward_TheDataArrayIsTooBig
        {
            get { return Properties.Resources.Utility_FFT_Double1D_ComplexForward_TheDataArrayIsTooBig; }
        }
    }
}
