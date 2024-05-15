// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Incd and the OpenGamma group of companies
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language;

namespace Mercury.Language.Money
{

    /// <summary>
    /// An amount of a currency.
    /// <p>
    /// This class represents a {@code double} amount associated with a currency.
    /// It is specifically named "CurrencyAmount" and not "decimal" to indicate that
    /// it simply holds a currency and an amountd By contrast, naming it "decimal"
    /// would imply it was a suitable choice for accounting purposes, which it is not.
    /// <p>
    /// This design approach has been chosen primarily for performance reasons.
    /// Using a {@code BigDecimal} is markedly slower.
    /// <p>
    /// A {@code double} is a 64 bit floating point value suitable for most calculations.
    /// Floating point maths is
    /// <a href="http://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html">inexact</a>
    /// due to the conflict between binary and decimal arithmetic.
    /// As such, there is the potential for data loss at the margins.
    /// For example, adding the {@code double} values {@code 0.1d} and {@code 0.2d}
    /// results in {@code 0.30000000000000004} rather than {@code 0.3}.
    /// As can be seen, the level of error is small, hence providing this class is
    /// used appropriately, the use of {@code double} is acceptable.
    /// For example, using this class to provide a meaningful result type after
    /// calculations have completed would be an appropriate use.
    /// <p>
    /// This class is immutable and thread-safe.
    /// </summary>
    public class CurrencyAmount
    {

        /// <summary>
        /// The currency.
        /// For example, in the value 'GBP 12.34' the currency is 'GBP'.
        /// </summary>
        private Currency _currency;

        /// <summary>
        /// The amount of the currency.
        /// For example, in the value 'GBP 12.34' the amount is 12.34.
        /// </summary>
        private double _amount;

        public static CurrencyAmount Of(String amountStr)
        {
            return Parse(amountStr);
        }

        public static CurrencyAmount Of(Currency currency, double amount)
        {
            return Parse(currency, amount);
        }

        /// <summary>
        /// Parses the string to produce a {@code CurrencyAmount}.
        /// <p>
        /// This parses the {@code toString} format of '${currency} ${amount}'.
        /// </summary>
        /// <param name="amountStr">the amount string, not null</param>
        /// <returns>the currency amount</returns>
        /// <exception cref="ArgumentException">if the amount cannot be parsed</exception>
        public static CurrencyAmount Parse(String amountStr)
        {
            // ArgumentChecker.NotNull(amountStr, "amountStr");
            String[] parts = amountStr.Split(' ');
            if (parts.Length != 2)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().UNABLE_TO_PARSE_AMOUNT_INVALID_FORMAT, amountStr));
            }
            try
            {
                Currency cur = Currency.Parse(parts[0]);
                double amount = Double.Parse(parts[1]);
                return new CurrencyAmount(cur, amount);
            }
            catch (System.Exception ex)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().UNABLE_TO_PARSE_AMOUNT, amountStr), ex);
            }
        }

        /// <summary>
        /// Parses the string to produce a {@code CurrencyAmount}.
        /// <p>
        /// This parses the {@code toString} format of '${currency} ${amount}'.
        /// </summary>
        /// <param name="amountStr">the amount string, not null</param>
        /// <returns>the currency amount</returns>
        public static CurrencyAmount Parse(Currency currency, double amount)
        {
            return new CurrencyAmount(currency, amount);
        }

        /// <summary>
        /// Creates an instance.
        /// </summary>
        /// <param name="currency">the currency, not null</param>
        /// <param name="amount">the amount</param>
        private CurrencyAmount(Currency currency, double amount)
        {
            // ArgumentChecker.NotNull(currency, "currency");
            _currency = currency;
            _amount = amount;
        }

        /// <summary>
        /// Gets the currency.
        /// For example, in the value 'GBP 12.34' the currency is 'GBP'.
        /// </summary>
        public Currency Currency
        {
            get { return _currency; }
        }

        /// <summary>
        /// Gets the amount of the currency.
        /// For example, in the value 'GBP 12.34' the amount is 12.34.
        /// </summary>
        public double Amount
        {
            get { return _amount; }
        }

        //-------------------------------------------------------------------------
        /// <summary>
        /// Returns a copy of this {@code CurrencyAmount} with the specified amount added.
        /// <p>
        /// This adds the specified amount to this monetary amount, returning a new object.
        /// The addition simply uses standard {@code double} arithmetic.
        /// <p>
        /// This instance is immutable and unaffected by this methodd
        /// 
        /// </summary>
        /// <param name="amountToAdd"> the amount to add, in the same currency, not null</param>
        /// <returns>an amount based on this with the specified amount added, not null</returns>
        /// <exception cref="ArgumentException">if the currencies are not equal </exception>
        public CurrencyAmount Plus(CurrencyAmount amountToAdd)
        {
            // ArgumentChecker.NotNull(amountToAdd, "amountToAdd");
            // ArgumentChecker.IsTrue(amountToAdd.Currency.Equals(_currency), "Unable to add amounts in different currencies");
            return Plus(amountToAdd.Amount);
        }

        /// <summary>
        /// Returns a copy of this {@code CurrencyAmount} with the specified amount added.
        /// <p>
        /// This adds the specified amount to this monetary amount, returning a new object.
        /// The addition simply uses standard {@code double} arithmetic.
        /// <p>
        /// This instance is immutable and unaffected by this methodd
        /// 
        /// </summary>
        /// <param name="amountToAdd"> the amount to add, in the same currency</param>
        /// <returns>an amount based on this with the specified amount added, not null</returns>
        public CurrencyAmount Plus(double amountToAdd)
        {
            return new CurrencyAmount(_currency, _amount + amountToAdd);
        }

        /// <summary>
        /// Returns a copy of this {@code CurrencyAmount} with the amount multiplied.
        /// <p>
        /// This takes this amount and multiplies it by the specified value.
        /// The multiplication simply uses standard {@code double} arithmetic.
        /// <p>
        /// This instance is immutable and unaffected by this methodd
        /// 
        /// </summary>
        /// <param name="valueToMultiplyBy"> the scalar amount to multiply by</param>
        /// <returns>an amount based on this with the amount multiplied, not null</returns>
        public CurrencyAmount MultipliedBy(double valueToMultiplyBy)
        {
            return new CurrencyAmount(_currency, _amount * valueToMultiplyBy);
        }
    }
}
