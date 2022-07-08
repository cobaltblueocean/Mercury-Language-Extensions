using System;
using System.Collections.Generic;
using System.Text;

namespace Mercury.Language.Extensions.Test.Core
{
    public class ValueObject
    {
        private double _value;

        public ValueObject(double value)
        {
            ArgumentChecker.IsTrue(value >= 0 && value <= 1.0, "Delta must be in the range (0,1)");
            _value = value;
        }

        public double Value
        {
            get { return _value; }
        }

        public ValueObject With(double value)
        {
            return new ValueObject(value);
        }

    }
}
