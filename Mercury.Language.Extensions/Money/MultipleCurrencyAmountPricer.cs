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
using Mercury.Language.Exceptions;

namespace Mercury.Language.Money
{
    /// <summary>
    /// Pricer that keeps a running sumd It is optimised for the most common case of a series of multi currency amounts in
    /// the same currency.
    /// <summary>
    public class MultipleCurrencyAmountPricer
    {
        // we pull out a single currency value and keep a running sum for itd This saves creating multiple transient
        // MCA object.
        /// <summary>running total (less the initial coupon amount) for optimised currency */
        private double _singleCurrencySubsequentAmounts;
        /// <summary>the currency we have optimised */
        private Currency _optimisedCurrency;
        /// <summary>holds the running sum - excluding subsequent payments in the optimised currency */
        private MultipleCurrencyAmount _currencyAmount;

        // the total amount is _singleCurrencySubsequentAmounts + _currencyAmount

        /// <summary>
        /// Create a pricing object
        /// <summary>
        /// <param name="amount">the initial amount in the series of payments</param>
        public MultipleCurrencyAmountPricer(MultipleCurrencyAmount amount)
        {
            //ArgumentChecker.NotNull(amount, "amount");
            if (amount.Count > 0)
            {
                // optimise the pricing of this currency by skipping intermediate MCA objects
                CurrencyAmount currencyAmount = amount.GetEnumerator().Next();
                _singleCurrencySubsequentAmounts = 0.0;
                _optimisedCurrency = currencyAmount.Currency;
            }
            _currencyAmount = amount;
        }

        /// <summary>
        /// Add the amount to the existing sum
        /// <summary>
        /// <param name="amountToAdd">the amount to add</param>
        public void Plus(MultipleCurrencyAmount amountToAdd)
        {
            // ArgumentChecker.NotNull(amountToAdd, "amountToAdd");
            if (_optimisedCurrency == null)
            {
                _currencyAmount = _currencyAmount.Plus(amountToAdd);
            }
            else
            {
                CurrencyAmount optimisedAmount = amountToAdd.GetCurrencyAmount(_optimisedCurrency);
                if (optimisedAmount != null && amountToAdd.Count == 1)
                {
                    // we only have the optimised currency so just update the running total
                    _singleCurrencySubsequentAmounts += optimisedAmount.Amount;
                    return;
                }
                _currencyAmount = _currencyAmount.Plus(amountToAdd);
            }
        }

        /// <summary>
        /// Get the sum of all the payments
        /// <summary>
        /// <returns>the sum</returns>
        public MultipleCurrencyAmount Sum
        {
            get
            {
                if (_optimisedCurrency == null)
                {
                    return _currencyAmount;
                }
                return _currencyAmount.Plus(_optimisedCurrency, _singleCurrencySubsequentAmounts);
            }
        }
    }
}
