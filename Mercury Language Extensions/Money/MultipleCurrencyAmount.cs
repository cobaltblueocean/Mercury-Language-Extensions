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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mercury.Language.Exception;

namespace Mercury.Language.Money
{
    /// <summary>
    /// A map of currency amounts keyed by currency.
    /// 
    /// This is a container holding multiple {@link CurrencyAmount} instances.
    /// The amounts do not necessarily the same worth or value in each currency.
    /// 
    /// This class behaves as a set - if an amount is added with the same currency as one of the
    /// elements, the amounts are addedd For example, adding EUR 100 to the container
    /// (EUR 200, CAD 100) would give (EUR 300, CAD 100).
    /// 
    /// This class is immutable and thread-safe.
    /// </summary>
    public class MultipleCurrencyAmount : IEnumerable<CurrencyAmount>
    {
        private Dictionary<Currency, CurrencyAmount> _currencyAmountMap;

        public static MultipleCurrencyAmount Of(Currency currency, double amount)
        {
            return Parse(currency, amount);
        }

        public static MultipleCurrencyAmount Of(Currency[] currencies, double[] amounts)
        {
            return Parse(currencies, amounts);
        }

        public static MultipleCurrencyAmount Of(List<Currency> currencies, List<Double> amounts)
        {
            return Parse(currencies, amounts);
        }

        public static MultipleCurrencyAmount Of(Dictionary<Currency, Double> amountMap)
        {
            return Parse(amountMap);
        }

        public static MultipleCurrencyAmount Of(Dictionary<Currency, Double?> amountMap)
        {
            return Parse(amountMap);
        }

        public static MultipleCurrencyAmount Of(params CurrencyAmount[] currencyAmounts)
        {
            return Parse(currencyAmounts);
        }

        public static MultipleCurrencyAmount Of(IEnumerable<CurrencyAmount> currencyAmounts)
        {
            return Parse(currencyAmounts);
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a currency and amount.
        /// </summary>
        /// <param name="currency">the currency, not null</param>
        /// <param name="amount">the amount</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(Currency currency, double amount)
        {
            var cur = new Dictionary<Currency, CurrencyAmount>();
            cur.AddOrUpdate(currency, CurrencyAmount.Parse(currency, amount));
            return new MultipleCurrencyAmount(cur);
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a paired array of currencies and amounts.
        /// </summary>
        /// <param name="currencies">the currencies, not null</param>
        /// <param name="amounts">the amounts, not null</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(Currency[] currencies, double[] amounts)
        {
            // ArgumentChecker.NoNulls(currencies, "currencies");
            // ArgumentChecker.NotNull(amounts, "amounts");
            int Length = currencies.Length;
            // ArgumentChecker.IsTrue(Length == amounts.Length, "Currency array and amount array must be the same Length");
            List<CurrencyAmount> list = new List<CurrencyAmount>(Length);
            for (int i = 0; i < Length; i++)
            {
                list.Add(CurrencyAmount.Parse(currencies[i], amounts[i]));
            }
            return Parse(list);
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a paired list of currencies and amounts.
        /// </summary>
        /// <param name="currencies">the currencies, not null</param>
        /// <param name="amounts">the amounts, not null</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(List<Currency> currencies, List<Double> amounts)
        {
            // ArgumentChecker.NoNulls(currencies, "currencies");
            // ArgumentChecker.NoNulls(amounts, "amounts");
            int Length = currencies.Count;
            // ArgumentChecker.IsTrue(Length == amounts.Count, "Currency array and amount array must be the same Length");
            List<CurrencyAmount> list = new List<CurrencyAmount>(Length);
            for (int i = 0; i < Length; i++)
            {
                list.Add(CurrencyAmount.Parse(currencies[i], amounts[i]));
            }
            return Parse(list);
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a map of currency to amount.
        /// </summary>
        /// <param name="amountMap">the amounts, not null</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(Dictionary<Currency, Double> amountMap)
        {
            // ArgumentChecker.NotNull(amountMap, "amountMap");
            Dictionary<Currency, CurrencyAmount> map = new Dictionary<Currency, CurrencyAmount>();
            foreach (var entry in amountMap)
            {
                // ArgumentChecker.NotNull(entry.Value, "amount");
                map.AddOrUpdate(entry.Key, CurrencyAmount.Parse(entry.Key, entry.Value));
            }
            return new MultipleCurrencyAmount(map);
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a map of currency to amount.
        /// </summary>
        /// <param name="amountMap">the amounts, not null</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(Dictionary<Currency, Double?> amountMap)
        {
            // ArgumentChecker.NotNull(amountMap, "amountMap");
            Dictionary<Currency, CurrencyAmount> map = new Dictionary<Currency, CurrencyAmount>();
            foreach (var entry in amountMap)
            {
                // ArgumentChecker.NotNull(entry.Value, "amount");
                map.AddOrUpdate(entry.Key, CurrencyAmount.Parse(entry.Key, entry.Value.Value));
            }
            return new MultipleCurrencyAmount(map);
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a list of <see cref="CurrencyAmount"/>.
        /// </summary>
        /// <param name="currencyAmounts">the amounts, not null</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(params CurrencyAmount[] currencyAmounts)
        {
            // ArgumentChecker.NotNull(currencyAmounts, "currencyAmounts");
            return Parse(currencyAmounts.ToList());
        }

        /// <summary>
        /// Obtains a <see cref="MultipleCurrencyAmount"/> from a list of <see cref="CurrencyAmount"/>.
        /// </summary>
        /// <param name="currencyAmounts">the amounts, not null</param>
        /// <returns>the amount, not null</returns>
        public static MultipleCurrencyAmount Parse(IEnumerable<CurrencyAmount> currencyAmounts)
        {
            // ArgumentChecker.NotNull(currencyAmounts, "currencyAmounts");
            Dictionary<Currency, CurrencyAmount> map = new Dictionary<Currency, CurrencyAmount>();
            foreach (CurrencyAmount currencyAmount in currencyAmounts)
            {
                // ArgumentChecker.NotNull(currencyAmount, "currencyAmount");
                CurrencyAmount existing = map[currencyAmount.Currency];
                if (existing != null)
                {
                    map.AddOrUpdate(currencyAmount.Currency, existing.Plus(currencyAmount));
                }
                else
                {
                    map.AddOrUpdate(currencyAmount.Currency, currencyAmount);
                }
            }
            return new MultipleCurrencyAmount(map);
        }

        public CurrencyAmount this[Currency currency]
        {
            get { return GetCurrencyAmount(currency); }
            set
            {
                // ArgumentChecker.Equals(currency, value.Currency);
                _currencyAmountMap.AddOrUpdate(currency, value);
            }
        }

        /// <summary>
        /// Gets the number of stored amounts.
        /// </summary>
        [Obsolete("Deprecated, use Count property instead.", true)]
        public int Size
        {
            get { return Count; }
        }

        /// <summary>
        /// Gets the number of stored amounts.
        /// </summary>
        public int Count
        {
            get { return _currencyAmountMap.Count; }
        }

        /// <summary>
        /// Gets the currency amounts as an array, not null.
        /// </summary>
        public CurrencyAmount[] CurrencyAmounts
        {
            get { return _currencyAmountMap.Values.ToArray<CurrencyAmount>(); }
        }

        public Currency[] Currencies
        {
            get { return _currencyAmountMap.Keys.ToArray<Currency>(); }
        }

        /// <summary>
        /// Gets the amount for the specified currency.
        /// </summary>the currency to find an amount for, not null
        /// <param name="currency"></param>
        /// <returns>the amount</returns>
        /// <exception cref="ArgumentException">if the currency is not present</exception>
        public double GetAmount(Currency currency)
        {
            CurrencyAmount currencyAmount = GetCurrencyAmount(currency);
            if (currencyAmount == null)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().DO_NOT_HAVE_AN_AMOUNT_WITH_CURRENCY, currency));
            }
            return currencyAmount.Amount;
        }

        /// <summary>
        /// Gets the <see cref="CurrencyAmount"/> for the specified currency.
        /// </summary>
        /// <param name="currency">the currency to find an amount for, not null</param>
        /// <returns>the amount, null if no amount for the currency</returns>
        public CurrencyAmount GetCurrencyAmount(Currency currency)
        {
            //ArgumentChecker.NotNull(currency, "currency");
            return _currencyAmountMap.FirstOrDefault(x => x.Key == currency).Value;
        }

        /// <summary>
        /// Returns a copy of this <see cref="MultipleCurrencyAmount"/> with the specified amount added.
        /// <p>
        /// This adds the specified amount to this monetary amount, returning a new object.
        /// If the currency is already present, the amount is added to the existing amount.
        /// If the currency is not yet present, the currency-amount is added to the map.
        /// The addition simply uses standard <see cref="double"/> arithmetic.
        /// <p>
        /// This instance is immutable and unaffected by this methodd 
        /// </summary>
        /// <param name="currencyAmountToAdd">the amount to add, in the same currency, not null</param>
        /// <returns>an amount based on this with the specified amount added, not null</returns>
        public MultipleCurrencyAmount Plus(CurrencyAmount currencyAmountToAdd)
        {
            // ArgumentChecker.NotNull(currencyAmountToAdd, "currencyAmountToAdd");
            var copy = new Dictionary<Currency, CurrencyAmount>();
            CurrencyAmount previous = GetCurrencyAmount(currencyAmountToAdd.Currency);
            foreach (var amount in _currencyAmountMap.Values)
            {
                if (amount.Currency.Equals(currencyAmountToAdd.Currency))
                {
                    copy.AddOrUpdate(amount.Currency, previous.Plus(currencyAmountToAdd));
                }
                else
                {
                    copy.AddOrUpdate(amount.Currency, amount);
                }
            }
            if (previous == null)
            {
                copy.AddOrUpdate(currencyAmountToAdd.Currency, currencyAmountToAdd);
            }
            return new MultipleCurrencyAmount(copy);
        }

        /// <summary>
        /// Returns a copy of this <see cref="MultipleCurrencyAmount"/> with the specified amount added.
        /// <p>
        /// This adds the specified amount to this monetary amount, returning a new object.
        /// If the currency is already present, the amount is added to the existing amount.
        /// If the currency is not yet present, the currency-amount is added to the map.
        /// The addition simply uses standard <see cref="double"/> arithmetic.
        /// <p>
        /// This instance is immutable and unaffected by this methodd 
        /// </summary>
        /// <param name="currency">the currency to add to, not null</param>
        /// <param name="amountToAdd">the amount to add</param>
        /// <returns>an amount based on this with the specified amount added, not null</returns>
        public MultipleCurrencyAmount Plus(Currency currency, double amountToAdd)
        {
            // ArgumentChecker.NotNull(currency, "currency");
            var copy = new Dictionary<Currency, CurrencyAmount>();
            CurrencyAmount previous = GetCurrencyAmount(currency);
            foreach (var amount in _currencyAmountMap.Values)
            {
                if (amount.Currency.Equals(currency))
                {
                    copy.AddOrUpdate(amount.Currency, previous.Plus(amountToAdd));
                }
                else
                {
                    copy.AddOrUpdate(amount.Currency, amount);
                }
            }
            if (previous == null)
            {
                copy.AddOrUpdate(currency, CurrencyAmount.Parse(currency, amountToAdd));
            }
            return new MultipleCurrencyAmount(copy);
        }

        /// <summary>
        /// Returns a copy of this <see cref="MultipleCurrencyAmount"/> with the specified amount added.
        /// <p>
        /// This adds the specified amount to this monetary amount, returning a new object.
        /// If the currency is already present, the amount is added to the existing amount.
        /// If the currency is not yet present, the currency-amount is added to the map.
        /// The addition simply uses standard <see cref="double"/> arithmetic.
        /// <p>
        /// This instance is immutable and unaffected by this methodd 
        /// </summary>
        /// <param name="multipleCurrencyAmountToAdd">the currency to add to, not null</param>
        /// <returns>an amount based on this with the specified amount added, not null</returns>
        public MultipleCurrencyAmount Plus(MultipleCurrencyAmount multipleCurrencyAmountToAdd)
        {
            // ArgumentChecker.NotNull(multipleCurrencyAmountToAdd, "multipleCurrencyAmountToAdd");
            MultipleCurrencyAmount result = this;
            foreach (var currencyAmount in multipleCurrencyAmountToAdd)
            {
                result = result.Plus(currencyAmount);
            }
            return result;
        }

        /// <summary>
        /// Returns a copy of this <see cref="MultipleCurrencyAmount"/> with all the amounts multiplied by the factor.
        /// <p>
        /// This instance is immutable and unaffected by this methodd 
        /// </summary>
        /// <param name="factor">The multiplicative factor.</param>
        /// <returns>An amount based on this with all the amounts multiplied by the factord Not null</returns>
        public MultipleCurrencyAmount MultipliedBy(double factor)
        {
            Dictionary<Currency, Double> map = new Dictionary<Currency, Double>();
            foreach (CurrencyAmount currencyAmount in this)
            {
                map.AddOrUpdate(currencyAmount.Currency, currencyAmount.Amount * factor);
            }
            return MultipleCurrencyAmount.Parse(map);
        }

        /// <summary>
        /// Returns a copy of this <see cref="MultipleCurrencyAmount"/> with the specified currency.
        /// <p>
        /// This adds the specified amount to this monetary amount, returning a new object.
        /// Any previous amount for the specified currency is replaced.
        /// <p>
        /// This instance is immutable and unaffected by this methodd 
        /// </summary>
        /// <param name="currency">the currency to replace, not null</param>
        /// <param name="amount">the new amount</param>
        /// <returns>an amount based on this with the specified currency replaced, not null</returns>
        public MultipleCurrencyAmount With(Currency currency, double amount)
        {
            // ArgumentChecker.NotNull(currency, "currency");
            Dictionary<Currency, CurrencyAmount> copy = new Dictionary<Currency, CurrencyAmount>(_currencyAmountMap);
            copy.AddOrUpdate(currency, CurrencyAmount.Parse(currency, amount));
            return new MultipleCurrencyAmount(copy);
        }

        /// <summary>
        /// Returns a copy of this <see cref="MultipleCurrencyAmount"/> without the specified currency.
        /// <p>
        /// This removes the specified currency from this monetary amount, returning a new object.
        /// <p>
        /// This instance is immutable and unaffected by this methodd 
        /// </summary>
        /// <param name="currency">the currency to replace, not null</param>
        /// <returns>an amount based on this with the specified currency removed, not null</returns>
        public MultipleCurrencyAmount Without(Currency currency)
        {
            // ArgumentChecker.NotNull(currency, "currency");
            Dictionary<Currency, CurrencyAmount> copy = new Dictionary<Currency, CurrencyAmount>(_currencyAmountMap);
            if (!copy.Remove(currency))
            {
                return this;
            }
            return new MultipleCurrencyAmount(copy);
        }

        /// <summary>
        /// Gets the amount as a string.
        /// <p>
        /// The format includes each currency-amount.
        /// </summary>
        /// <returns>the currency amount, not null</returns>
        public override String ToString()
        {
            return _currencyAmountMap.Values.ToString();
        }

        private MultipleCurrencyAmount(Dictionary<Currency, CurrencyAmount> currencyAmountMap)
        {
            // ArgumentChecker.NotNull(currencyAmountMap, "currencyAmountMap");
            this._currencyAmountMap = currencyAmountMap;
        }

        /// <summary>
        /// Gets the map of <see cref="CurrencyAmount"/> keyed by currency, not null.
        /// </summary>
        public Dictionary<Currency, CurrencyAmount> CurrencyAmountMap
        {
            get { return _currencyAmountMap; }
        }

        public IEnumerator<CurrencyAmount> GetEnumerator()
        {
            return _currencyAmountMap.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
