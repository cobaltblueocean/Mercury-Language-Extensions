// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by Microsoft Corporation.
//
// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*=========================================================================
**
** Class: DecimalComplex
**
**
** Purpose: 
** This feature is intended to create DecimalComplex Number as a type 
** that can be a part of the .NET framework (base class libraries).  
** A DecimalComplex number z is a number of the form z = x + yi, where x and y 
** are real numbers, and i is the imaginary unit, with the property i2= -1.
**
**
===========================================================================*/
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace System.Numerics
{

#if !SILVERLIGHT
    [Serializable]
#endif // !SILVERLIGHT
    public struct DecimalComplex : IEquatable<DecimalComplex>, IFormattable
    {

        // --------------SECTION: Private Data members ----------- //

        private decimal m_real;
        private decimal m_imaginary;

        // ---------------SECTION: Necessary Constants ----------- //

        private const decimal LOG_10_INV = 0.43429448190325M;


        // --------------SECTION: Public Properties -------------- //

        public decimal Real
        {
            get
            {
                return m_real;
            }
        }

        public decimal Imaginary
        {
            get
            {
                return m_imaginary;
            }
        }

        public decimal Magnitude
        {
            get
            {
                return DecimalComplex.Abs(this);
            }
        }

        public decimal Phase
        {
            get
            {
                return Math2.Atan2(m_imaginary, m_real);
            }
            
        }

        public Complex ToComplex()
        {
            try
            {
                return new Complex((double)m_real, (double)m_imaginary);
            }
            catch
            {
                return new Complex();
            }
        }

        public static DecimalComplex FromComplex(Complex cpx)
        {
            return new DecimalComplex(cpx.Real, cpx.Imaginary);
        }

        // --------------SECTION: Attributes -------------- //

        public static readonly DecimalComplex Zero = new DecimalComplex(0.0M, 0.0M);
        public static readonly DecimalComplex One = new DecimalComplex(1.0M, 0.0M);
        public static readonly DecimalComplex ImaginaryOne = new DecimalComplex(0.0M, 1.0M);

        // --------------SECTION: Constructors and factory methods -------------- //

        public DecimalComplex(decimal real, decimal imaginary)  /* Constructor to create a DecimalComplex number with rectangular co-ordinates  */
        {
            this.m_real = real;
            this.m_imaginary = imaginary;
        }

        public DecimalComplex(double real, double imaginary)
        {
            this.m_real = (decimal)real;
            this.m_imaginary = (decimal)real;
        }

        public static DecimalComplex FromPolarCoordinates(decimal magnitude, decimal phase) /* Factory method to take polar inputs and create a DecimalComplex object */
        {
            return new DecimalComplex((magnitude * Math2.Cos(phase)), (magnitude * Math2.Sin(phase)));
        }

        public static DecimalComplex Negate(DecimalComplex value)
        {
            return -value;
        }

        public static DecimalComplex Add(DecimalComplex left, DecimalComplex right)
        {
            return left + right;
        }

        public static DecimalComplex Subtract(DecimalComplex left, DecimalComplex right)
        {
            return left - right;
        }

        public static DecimalComplex Multiply(DecimalComplex left, DecimalComplex right)
        {
            return left * right;
        }

        public static DecimalComplex Divide(DecimalComplex dividend, DecimalComplex divisor)
        {
            return dividend / divisor;
        }

        // --------------SECTION: Arithmetic Operator(unary) Overloading -------------- //
        public static DecimalComplex operator -(DecimalComplex value)  /* Unary negation of a DecimalComplex number */
        {

            return (new DecimalComplex((-value.m_real), (-value.m_imaginary)));
        }

        // --------------SECTION: Arithmetic Operator(binary) Overloading -------------- //       
        public static DecimalComplex operator +(DecimalComplex left, DecimalComplex right)
        {
            return (new DecimalComplex((left.m_real + right.m_real), (left.m_imaginary + right.m_imaginary)));

        }

        public static DecimalComplex operator -(DecimalComplex left, DecimalComplex right)
        {
            return (new DecimalComplex((left.m_real - right.m_real), (left.m_imaginary - right.m_imaginary)));
        }

        public static DecimalComplex operator *(DecimalComplex left, DecimalComplex right)
        {
            // Multiplication:  (a + bi)(c + di) = (ac -bd) + (bc + ad)i
            decimal result_Realpart = (left.m_real * right.m_real) - (left.m_imaginary * right.m_imaginary);
            decimal result_Imaginarypart = (left.m_imaginary * right.m_real) + (left.m_real * right.m_imaginary);
            return (new DecimalComplex(result_Realpart, result_Imaginarypart));
        }

        public static DecimalComplex operator /(DecimalComplex left, DecimalComplex right)
        {
            // Division : Smith's formula.
            decimal a = left.m_real;
            decimal b = left.m_imaginary;
            decimal c = right.m_real;
            decimal d = right.m_imaginary;

            if (Math2.Abs(d) < Math2.Abs(c))
            {
                decimal doc = d / c;
                return new DecimalComplex((a + b * doc) / (c + d * doc), (b - a * doc) / (c + d * doc));
            }
            else
            {
                decimal cod = c / d;
                return new DecimalComplex((b + a * cod) / (d + c * cod), (-a + b * cod) / (d + c * cod));
            }
        }


        // --------------SECTION: Other arithmetic operations  -------------- //

        public static decimal Abs(DecimalComplex value)
        {

            //if (decimal.IsInfinity(value.m_real) || decimal.IsInfinity(value.m_imaginary))
            //{
            //    return decimal.PositiveInfinity;
            //}

            // |value| == sqrt(a^2 + b^2)
            // sqrt(a^2 + b^2) == a/a * sqrt(a^2 + b^2) = a * sqrt(a^2/a^2 + b^2/a^2)
            // Using the above we can factor out the square of the larger component to dodge overflow.


            decimal c = Math2.Abs(value.m_real);
            decimal d = Math2.Abs(value.m_imaginary);

            if (c > d)
            {
                decimal r = d / c;
                return c * Math2.Sqrt(1.0M + r * r);
            }
            else if (d == 0.0M)
            {
                return c;  // c is either 0.0 or NaN
            }
            else
            {
                decimal r = c / d;
                return d * Math2.Sqrt(1.0M + r * r);
            }
        }
        public static DecimalComplex Conjugate(DecimalComplex value)
        {
            // Conjugate of a DecimalComplex number: the conjugate of x+i*y is x-i*y 

            return (new DecimalComplex(value.m_real, (-value.m_imaginary)));

        }
        public static DecimalComplex Reciprocal(DecimalComplex value)
        {
            // Reciprocal of a DecimalComplex number : the reciprocal of x+i*y is 1/(x+i*y)
            if ((value.m_real == 0) && (value.m_imaginary == 0))
            {
                return DecimalComplex.Zero;
            }

            return DecimalComplex.One / value;
        }

        // --------------SECTION: Comparison Operator(binary) Overloading -------------- //

        public static bool operator ==(DecimalComplex left, DecimalComplex right)
        {
            return ((left.m_real == right.m_real) && (left.m_imaginary == right.m_imaginary));


        }
        public static bool operator !=(DecimalComplex left, DecimalComplex right)
        {
            return ((left.m_real != right.m_real) || (left.m_imaginary != right.m_imaginary));

        }

        // --------------SECTION: Comparison operations (methods implementing IEquatable<DecimalComplexNumber>,IComparable<DecimalComplexNumber>) -------------- //

        public override bool Equals(object obj)
        {
            if (!(obj is DecimalComplex)) return false;
            return this == ((DecimalComplex)obj);
        }
        public bool Equals(DecimalComplex value)
        {
            return ((this.m_real.Equals(value.m_real)) && (this.m_imaginary.Equals(value.m_imaginary)));

        }

        // --------------SECTION: Type-casting basic numeric data-types to DecimalComplexNumber  -------------- //

        public static implicit operator DecimalComplex(Int16 value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        public static implicit operator DecimalComplex(Int32 value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        public static implicit operator DecimalComplex(Int64 value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        [CLSCompliant(false)]
        public static implicit operator DecimalComplex(UInt16 value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        [CLSCompliant(false)]
        public static implicit operator DecimalComplex(UInt32 value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        [CLSCompliant(false)]
        public static implicit operator DecimalComplex(UInt64 value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        [CLSCompliant(false)]
        public static implicit operator DecimalComplex(SByte value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        public static implicit operator DecimalComplex(Byte value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        public static implicit operator DecimalComplex(Single value)
        {
            return (new DecimalComplex((decimal)value, 0.0M));
        }
        public static implicit operator DecimalComplex(decimal value)
        {
            return (new DecimalComplex(value, 0.0M));
        }
        public static explicit operator DecimalComplex(BigInteger value)
        {
            return (new DecimalComplex((decimal)value, 0.0M));
        }
        public static explicit operator DecimalComplex(double value)
        {
            return (new DecimalComplex((decimal)value, 0.0M));
        }


        // --------------SECTION: Formattig/Parsing options  -------------- //

        public override String ToString()
        {
            return (String.Format(CultureInfo.CurrentCulture, "({0}, {1})", this.m_real, this.m_imaginary));
        }

        public String ToString(String format)
        {
            return (String.Format(CultureInfo.CurrentCulture, "({0}, {1})", this.m_real.ToString(format, CultureInfo.CurrentCulture), this.m_imaginary.ToString(format, CultureInfo.CurrentCulture)));
        }

        public String ToString(IFormatProvider provider)
        {
            return (String.Format(provider, "({0}, {1})", this.m_real, this.m_imaginary));
        }

        public String ToString(String format, IFormatProvider provider)
        {
            return (String.Format(provider, "({0}, {1})", this.m_real.ToString(format, provider), this.m_imaginary.ToString(format, provider)));
        }


        public override Int32 GetHashCode()
        {
            Int32 n1 = 99999997;
            Int32 hash_real = this.m_real.GetHashCode() % n1;
            Int32 hash_imaginary = this.m_imaginary.GetHashCode();
            Int32 final_hashcode = hash_real ^ hash_imaginary;
            return (final_hashcode);
        }



        // --------------SECTION: Trigonometric operations (methods implementing ITrigonometric)  -------------- //

        public static DecimalComplex Sin(DecimalComplex value)
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(Math2.Sin(a) * Math2.Cosh(b), Math2.Cos(a) * Math2.Sinh(b));
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sinh", Justification = "Microsoft: Existing Name")]
        public static DecimalComplex Sinh(DecimalComplex value) /* Hyperbolic sin */
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(Math2.Sinh(a) * Math2.Cos(b), Math2.Cosh(a) * Math2.Sin(b));

        }
        public static DecimalComplex Asin(DecimalComplex value) /* Arcsin */
        {
            return (-ImaginaryOne) * Log(ImaginaryOne * value + Sqrt(One - value * value));
        }

        public static DecimalComplex Cos(DecimalComplex value)
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(Math2.Cos(a) * Math2.Cosh(b), -(Math2.Sin(a) * Math2.Sinh(b)));
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cosh", Justification = "Microsoft: Existing Name")]
        public static DecimalComplex Cosh(DecimalComplex value) /* Hyperbolic cos */
        {
            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            return new DecimalComplex(Math2.Cosh(a) * Math2.Cos(b), Math2.Sinh(a) * Math2.Sin(b));
        }
        public static DecimalComplex Acos(DecimalComplex value) /* Arccos */
        {
            return (-ImaginaryOne) * Log(value + ImaginaryOne * Sqrt(One - (value * value)));

        }
        public static DecimalComplex Tan(DecimalComplex value)
        {
            return (Sin(value) / Cos(value));
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tanh", Justification = "Microsoft: Existing Name")]
        public static DecimalComplex Tanh(DecimalComplex value) /* Hyperbolic tan */
        {
            return (Sinh(value) / Cosh(value));
        }
        public static DecimalComplex Atan(DecimalComplex value) /* Arctan */
        {
            DecimalComplex Two = new DecimalComplex(2.0M, 0.0M);
            return (ImaginaryOne / Two) * (Log(One - ImaginaryOne * value) - Log(One + ImaginaryOne * value));
        }

        // --------------SECTION: Other numerical functions  -------------- //        

        public static DecimalComplex Log(DecimalComplex value) /* Log of the DecimalComplex number value to the base of 'e' */
        {
            return (new DecimalComplex((Math2.Log(Abs(value))), (Math2.Atan2(value.m_imaginary, value.m_real))));

        }
        public static DecimalComplex Log(DecimalComplex value, decimal baseValue) /* Log of the DecimalComplex number to a the base of a decimal */
        {
            return (Log(value) / Log(baseValue));
        }
        public static DecimalComplex Log10(DecimalComplex value) /* Log to the base of 10 of the DecimalComplex number */
        {

            DecimalComplex temp_log = Log(value);
            return (Scale(temp_log, (decimal)LOG_10_INV));

        }
        public static DecimalComplex Exp(DecimalComplex value) /* The DecimalComplex number raised to e */
        {
            decimal temp_factor = Math2.Exp(value.m_real);
            decimal result_re = temp_factor * Math2.Cos(value.m_imaginary);
            decimal result_im = temp_factor * Math2.Sin(value.m_imaginary);
            return (new DecimalComplex(result_re, result_im));
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Sqrt", Justification = "Microsoft: Existing Name")]
        public static DecimalComplex Sqrt(DecimalComplex value) /* Square root ot the DecimalComplex number */
        {
            return DecimalComplex.FromPolarCoordinates(Math2.Sqrt(value.Magnitude), value.Phase / 2.0M);
        }

        public static DecimalComplex Pow(DecimalComplex value, DecimalComplex power) /* A DecimalComplex number raised to another DecimalComplex number */
        {

            if (power == DecimalComplex.Zero)
            {
                return DecimalComplex.One;
            }

            if (value == DecimalComplex.Zero)
            {
                return DecimalComplex.Zero;
            }

            decimal a = value.m_real;
            decimal b = value.m_imaginary;
            decimal c = power.m_real;
            decimal d = power.m_imaginary;

            decimal rho = DecimalComplex.Abs(value);
            decimal theta = Math2.Atan2(b, a);
            decimal newRho = c * theta + d * Math2.Log(rho);

            decimal t = Math2.Pow(rho, c) * Math2.Pow((decimal)Math2.E, -d * theta);

            return new DecimalComplex(t * Math2.Cos(newRho), t * Math2.Sin(newRho));
        }

        public static DecimalComplex Pow(DecimalComplex value, decimal power) // A DecimalComplex number raised to a real number 
        {
            return Pow(value, new DecimalComplex(power, 0));
        }



        //--------------- SECTION: Private member functions for internal use -----------------------------------//

        private static DecimalComplex Scale(DecimalComplex value, decimal factor)
        {

            decimal result_re = factor * value.m_real;
            decimal result_im = factor * value.m_imaginary;
            return (new DecimalComplex(result_re, result_im));
        }
    }
}
