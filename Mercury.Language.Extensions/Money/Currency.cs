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
using Mercury.Language.Exception;

namespace Mercury.Language.Money
{
    /// <summary>
    /// Currency Description
    /// </summary>
    public sealed class Currency
    {
        String _code;

        /// <summary>
        /// The scheme to use in object identifiers.
        /// </summary>
        public static String OBJECT_SCHEME = "CurrencyISO";

        #region a selection of commonly traded, stable currencies
        ///<summary>
        /// The currency 'USD' - United States Dollar.
        ///</summary>
        public static Currency USD = new Currency("USD");
        ///<summary>
        /// The currency 'EUR' - Euro.
        ///</summary>
        public static Currency EUR = new Currency("EUR");
        ///<summary>
        /// The currency 'JPY' - Japanese Yen.
        ///</summary>
        public static Currency JPY = new Currency("JPY");
        ///<summary>
        /// The currency 'GBP' - British pound.
        ///</summary>
        public static Currency GBP = new Currency("GBP");
        ///<summary>
        /// The currency 'EUR' - Swiss Franc.
        ///</summary>
        public static Currency CHF = new Currency("CHF");
        ///<summary>
        /// The currency 'AUD' - Australian Dollar.
        ///</summary>
        public static Currency AUD = new Currency("AUD");
        ///<summary>
        /// The currency 'CAD' - Canadian Dollar.
        ///</summary>
        public static Currency CAD = new Currency("CAD");

        // a selection Currency other currencies
        ///<summary>
        /// The currency 'NZD' - New Zealand Dollar.
        ///</summary>
        public static Currency NZD = new Currency("NZD");
        ///<summary>
        /// The currency 'DKK' - Danish Krone
        ///</summary>
        public static Currency DKK = new Currency("DKK");
        ///<summary>
        /// The currency 'DEM' - Deutsche Mark
        ///</summary>
        public static Currency DEM = new Currency("DEM");
        ///<summary>
        /// The currency 'CZK' - Czeck Krona
        ///</summary>
        public static Currency CZK = new Currency("CZK");
        ///<summary>
        /// The currency 'SEK' - Swedish Krona
        ///</summary>
        public static Currency SEK = new Currency("SEK");
        ///<summary>
        /// The currency 'SKK' - Slovak Korona
        ///</summary>
        public static Currency SKK = new Currency("SKK");
        ///<summary>
        /// The currency 'ITL' - Italian Lira
        ///</summary>
        public static Currency ITL = new Currency("ITL");
        ///<summary>
        /// The currency 'HUF' = Hugarian Forint
        ///</summary>
        public static Currency HUF = new Currency("HUF");
        ///<summary>
        /// The currency 'FRF' - French Franc
        ///</summary>
        public static Currency FRF = new Currency("FRF");
        ///<summary>
        /// The currency 'NOK' - Norwegian Krone 
        ///</summary>
        public static Currency NOK = new Currency("NOK");
        ///<summary>
        /// The currency 'HKD' - Hong Kong Dollar
        ///</summary>
        public static Currency HKD = new Currency("HKD");
        ///<summary>
        /// The currency 'BRL' - Brazil Dollar
        ///</summary>
        public static Currency BRL = new Currency("BRL");
        ///<summary>
        /// The currency 'ZAR' - South African Rand
        ///</summary>
        public static Currency ZAR = new Currency("ZAR");
        ///<summary>
        /// The currency 'PLN' - Polish Zloty
        ///</summary>
        public static Currency PLN = new Currency("PLN");
        ///<summary>
        /// The currency 'SGD' - Singapore Dollar
        ///</summary>
        public static Currency SGD = new Currency("SGD");
        ///<summary>
        /// The currency 'MXN' - Mexican Peso
        ///</summary>
        public static Currency MXN = new Currency("MXN");

        #region Other currencies
        public static Currency AED = new Currency("AED");
        public static Currency AFN = new Currency("AFN");
        public static Currency ALL = new Currency("ALL");
        public static Currency AMD = new Currency("AMD");
        public static Currency ANG = new Currency("ANG");
        public static Currency AOA = new Currency("AOA");
        public static Currency ARS = new Currency("ARS");
        public static Currency AWG = new Currency("AWG");
        public static Currency AZN = new Currency("AZN");
        public static Currency BAM = new Currency("BAM");
        public static Currency BBD = new Currency("BBD");
        public static Currency BDT = new Currency("BDT");
        public static Currency BGN = new Currency("BGN");
        public static Currency BHD = new Currency("BHD");
        public static Currency BIF = new Currency("BIF");
        public static Currency BMD = new Currency("BMD");
        public static Currency BND = new Currency("BND");
        public static Currency BOB = new Currency("BOB");
        public static Currency BOV = new Currency("BOV");
        public static Currency BSD = new Currency("BSD");
        public static Currency BTN = new Currency("BTN");
        public static Currency BWP = new Currency("BWP");
        public static Currency BYN = new Currency("BYN");
        public static Currency BZD = new Currency("BZD");
        public static Currency CDF = new Currency("CDF");
        public static Currency CHE = new Currency("CHE");
        public static Currency CHW = new Currency("CHW");
        public static Currency CLF = new Currency("CLF");
        public static Currency CLP = new Currency("CLP");
        public static Currency CNY = new Currency("CNY");
        public static Currency COP = new Currency("COP");
        public static Currency COU = new Currency("COU");
        public static Currency CRC = new Currency("CRC");
        public static Currency CUC = new Currency("CUC");
        public static Currency CUP = new Currency("CUP");
        public static Currency CVE = new Currency("CVE");
        public static Currency DJF = new Currency("DJF");
        public static Currency DOP = new Currency("DOP");
        public static Currency DZD = new Currency("DZD");
        public static Currency EGP = new Currency("EGP");
        public static Currency ERN = new Currency("ERN");
        public static Currency ETB = new Currency("ETB");
        public static Currency FJD = new Currency("FJD");
        public static Currency FKP = new Currency("FKP");
        public static Currency GEL = new Currency("GEL");
        public static Currency GGP = new Currency("GGP");
        public static Currency GHS = new Currency("GHS");
        public static Currency GIP = new Currency("GIP");
        public static Currency GMD = new Currency("GMD");
        public static Currency GNF = new Currency("GNF");
        public static Currency GTQ = new Currency("GTQ");
        public static Currency GYD = new Currency("GYD");
        public static Currency HNL = new Currency("HNL");
        public static Currency HRK = new Currency("HRK");
        public static Currency HTG = new Currency("HTG");
        public static Currency IDR = new Currency("IDR");
        public static Currency ILS = new Currency("ILS");
        public static Currency IMP = new Currency("IMP");
        public static Currency INR = new Currency("INR");
        public static Currency IQD = new Currency("IQD");
        public static Currency IRR = new Currency("IRR");
        public static Currency ISK = new Currency("ISK");
        public static Currency JEP = new Currency("JEP");
        public static Currency JMD = new Currency("JMD");
        public static Currency JOD = new Currency("JOD");
        public static Currency KES = new Currency("KES");
        public static Currency KGS = new Currency("KGS");
        public static Currency KHR = new Currency("KHR");
        public static Currency KMF = new Currency("KMF");
        public static Currency KPW = new Currency("KPW");
        public static Currency KRW = new Currency("KRW");
        public static Currency KWD = new Currency("KWD");
        public static Currency KYD = new Currency("KYD");
        public static Currency KZT = new Currency("KZT");
        public static Currency LAK = new Currency("LAK");
        public static Currency LBP = new Currency("LBP");
        public static Currency LKR = new Currency("LKR");
        public static Currency LRD = new Currency("LRD");
        public static Currency LSL = new Currency("LSL");
        public static Currency LYD = new Currency("LYD");
        public static Currency MAD = new Currency("MAD");
        public static Currency MDL = new Currency("MDL");
        public static Currency MGA = new Currency("MGA");
        public static Currency MKD = new Currency("MKD");
        public static Currency MMK = new Currency("MMK");
        public static Currency MNT = new Currency("MNT");
        public static Currency MOP = new Currency("MOP");
        public static Currency MRU = new Currency("MRU");
        public static Currency MUR = new Currency("MUR");
        public static Currency MVR = new Currency("MVR");
        public static Currency MWK = new Currency("MWK");
        public static Currency MXV = new Currency("MXV");
        public static Currency MYR = new Currency("MYR");
        public static Currency MZN = new Currency("MZN");
        public static Currency NAD = new Currency("NAD");
        public static Currency NGN = new Currency("NGN");
        public static Currency NIO = new Currency("NIO");
        public static Currency NPR = new Currency("NPR");
        public static Currency OMR = new Currency("OMR");
        public static Currency PAB = new Currency("PAB");
        public static Currency PEN = new Currency("PEN");
        public static Currency PGK = new Currency("PGK");
        public static Currency PHP = new Currency("PHP");
        public static Currency PKR = new Currency("PKR");
        public static Currency PYG = new Currency("PYG");
        public static Currency QAR = new Currency("QAR");
        public static Currency RON = new Currency("RON");
        public static Currency RSD = new Currency("RSD");
        public static Currency RUB = new Currency("RUB");
        public static Currency RWF = new Currency("RWF");
        public static Currency SAR = new Currency("SAR");
        public static Currency SBD = new Currency("SBD");
        public static Currency SCR = new Currency("SCR");
        public static Currency SDG = new Currency("SDG");
        public static Currency SHP = new Currency("SHP");
        public static Currency SLL = new Currency("SLL");
        public static Currency SOS = new Currency("SOS");
        public static Currency SRD = new Currency("SRD");
        public static Currency SSP = new Currency("SSP");
        public static Currency STN = new Currency("STN");
        public static Currency SVC = new Currency("SVC");
        public static Currency SYP = new Currency("SYP");
        public static Currency SZL = new Currency("SZL");
        public static Currency THB = new Currency("THB");
        public static Currency TJS = new Currency("TJS");
        public static Currency TMT = new Currency("TMT");
        public static Currency TND = new Currency("TND");
        public static Currency TOP = new Currency("TOP");
        public static Currency TRY = new Currency("TRY");
        public static Currency TTD = new Currency("TTD");
        public static Currency TWD = new Currency("TWD");
        public static Currency TZS = new Currency("TZS");
        public static Currency UAH = new Currency("UAH");
        public static Currency UGX = new Currency("UGX");
        public static Currency USN = new Currency("USN");
        public static Currency UYI = new Currency("UYI");
        public static Currency UYU = new Currency("UYU");
        public static Currency UZS = new Currency("UZS");
        public static Currency VEF = new Currency("VEF");
        public static Currency VES = new Currency("VES");
        public static Currency VND = new Currency("VND");
        public static Currency VUV = new Currency("VUV");
        public static Currency WST = new Currency("WST");
        public static Currency XAF = new Currency("XAF");
        public static Currency XCD = new Currency("XCD");
        public static Currency XDR = new Currency("XDR");
        public static Currency XOF = new Currency("XOF");
        public static Currency XPF = new Currency("XPF");
        public static Currency XSU = new Currency("XSU");
        public static Currency XUA = new Currency("XUA");
        public static Currency YER = new Currency("YER");
        public static Currency ZMW = new Currency("ZMW");
        public static Currency ZWL = new Currency("ZWL");
        #endregion

        private static Dictionary<String, Currency> s_instanceMap = new Dictionary<String, Currency>();

        #endregion

        static Currency()
        {
            s_instanceMap.AddOrUpdate("AED", AED);
            s_instanceMap.AddOrUpdate("AFN", AFN);
            s_instanceMap.AddOrUpdate("ALL", ALL);
            s_instanceMap.AddOrUpdate("AMD", AMD);
            s_instanceMap.AddOrUpdate("ANG", ANG);
            s_instanceMap.AddOrUpdate("AOA", AOA);
            s_instanceMap.AddOrUpdate("ARS", ARS);
            s_instanceMap.AddOrUpdate("AUD", AUD);
            s_instanceMap.AddOrUpdate("AWG", AWG);
            s_instanceMap.AddOrUpdate("AZN", AZN);
            s_instanceMap.AddOrUpdate("BAM", BAM);
            s_instanceMap.AddOrUpdate("BBD", BBD);
            s_instanceMap.AddOrUpdate("BDT", BDT);
            s_instanceMap.AddOrUpdate("BGN", BGN);
            s_instanceMap.AddOrUpdate("BHD", BHD);
            s_instanceMap.AddOrUpdate("BIF", BIF);
            s_instanceMap.AddOrUpdate("BMD", BMD);
            s_instanceMap.AddOrUpdate("BND", BND);
            s_instanceMap.AddOrUpdate("BOB", BOB);
            s_instanceMap.AddOrUpdate("BOV", BOV);
            s_instanceMap.AddOrUpdate("BRL", BRL);
            s_instanceMap.AddOrUpdate("BSD", BSD);
            s_instanceMap.AddOrUpdate("BTN", BTN);
            s_instanceMap.AddOrUpdate("BWP", BWP);
            s_instanceMap.AddOrUpdate("BYN", BYN);
            s_instanceMap.AddOrUpdate("BZD", BZD);
            s_instanceMap.AddOrUpdate("CAD", CAD);
            s_instanceMap.AddOrUpdate("CDF", CDF);
            s_instanceMap.AddOrUpdate("CHE", CHE);
            s_instanceMap.AddOrUpdate("CHF", CHF);
            s_instanceMap.AddOrUpdate("CHW", CHW);
            s_instanceMap.AddOrUpdate("CLF", CLF);
            s_instanceMap.AddOrUpdate("CLP", CLP);
            s_instanceMap.AddOrUpdate("CNY", CNY);
            s_instanceMap.AddOrUpdate("COP", COP);
            s_instanceMap.AddOrUpdate("COU", COU);
            s_instanceMap.AddOrUpdate("CRC", CRC);
            s_instanceMap.AddOrUpdate("CUC", CUC);
            s_instanceMap.AddOrUpdate("CUP", CUP);
            s_instanceMap.AddOrUpdate("CVE", CVE);
            s_instanceMap.AddOrUpdate("CZK", CZK);
            s_instanceMap.AddOrUpdate("DJF", DJF);
            s_instanceMap.AddOrUpdate("DKK", DKK);
            s_instanceMap.AddOrUpdate("DOP", DOP);
            s_instanceMap.AddOrUpdate("DZD", DZD);
            s_instanceMap.AddOrUpdate("EGP", EGP);
            s_instanceMap.AddOrUpdate("ERN", ERN);
            s_instanceMap.AddOrUpdate("ETB", ETB);
            s_instanceMap.AddOrUpdate("EUR", EUR);
            s_instanceMap.AddOrUpdate("FJD", FJD);
            s_instanceMap.AddOrUpdate("FKP", FKP);
            s_instanceMap.AddOrUpdate("GBP", GBP);
            s_instanceMap.AddOrUpdate("GEL", GEL);
            s_instanceMap.AddOrUpdate("GHS", GHS);
            s_instanceMap.AddOrUpdate("GIP", GIP);
            s_instanceMap.AddOrUpdate("GMD", GMD);
            s_instanceMap.AddOrUpdate("GNF", GNF);
            s_instanceMap.AddOrUpdate("GTQ", GTQ);
            s_instanceMap.AddOrUpdate("GYD", GYD);
            s_instanceMap.AddOrUpdate("HKD", HKD);
            s_instanceMap.AddOrUpdate("HNL", HNL);
            s_instanceMap.AddOrUpdate("HRK", HRK);
            s_instanceMap.AddOrUpdate("HTG", HTG);
            s_instanceMap.AddOrUpdate("HUF", HUF);
            s_instanceMap.AddOrUpdate("IDR", IDR);
            s_instanceMap.AddOrUpdate("ILS", ILS);
            s_instanceMap.AddOrUpdate("INR", INR);
            s_instanceMap.AddOrUpdate("IQD", IQD);
            s_instanceMap.AddOrUpdate("IRR", IRR);
            s_instanceMap.AddOrUpdate("ISK", ISK);
            s_instanceMap.AddOrUpdate("JMD", JMD);
            s_instanceMap.AddOrUpdate("JOD", JOD);
            s_instanceMap.AddOrUpdate("JPY", JPY);
            s_instanceMap.AddOrUpdate("KES", KES);
            s_instanceMap.AddOrUpdate("KGS", KGS);
            s_instanceMap.AddOrUpdate("KHR", KHR);
            s_instanceMap.AddOrUpdate("KMF", KMF);
            s_instanceMap.AddOrUpdate("KPW", KPW);
            s_instanceMap.AddOrUpdate("KRW", KRW);
            s_instanceMap.AddOrUpdate("KWD", KWD);
            s_instanceMap.AddOrUpdate("KYD", KYD);
            s_instanceMap.AddOrUpdate("KZT", KZT);
            s_instanceMap.AddOrUpdate("LAK", LAK);
            s_instanceMap.AddOrUpdate("LBP", LBP);
            s_instanceMap.AddOrUpdate("LKR", LKR);
            s_instanceMap.AddOrUpdate("LRD", LRD);
            s_instanceMap.AddOrUpdate("LSL", LSL);
            s_instanceMap.AddOrUpdate("LYD", LYD);
            s_instanceMap.AddOrUpdate("MAD", MAD);
            s_instanceMap.AddOrUpdate("MDL", MDL);
            s_instanceMap.AddOrUpdate("MGA", MGA);
            s_instanceMap.AddOrUpdate("MKD", MKD);
            s_instanceMap.AddOrUpdate("MMK", MMK);
            s_instanceMap.AddOrUpdate("MNT", MNT);
            s_instanceMap.AddOrUpdate("MOP", MOP);
            s_instanceMap.AddOrUpdate("MRU", MRU);
            s_instanceMap.AddOrUpdate("MUR", MUR);
            s_instanceMap.AddOrUpdate("MVR", MVR);
            s_instanceMap.AddOrUpdate("MWK", MWK);
            s_instanceMap.AddOrUpdate("MXN", MXN);
            s_instanceMap.AddOrUpdate("MXV", MXV);
            s_instanceMap.AddOrUpdate("MYR", MYR);
            s_instanceMap.AddOrUpdate("MZN", MZN);
            s_instanceMap.AddOrUpdate("NAD", NAD);
            s_instanceMap.AddOrUpdate("NGN", NGN);
            s_instanceMap.AddOrUpdate("NIO", NIO);
            s_instanceMap.AddOrUpdate("NOK", NOK);
            s_instanceMap.AddOrUpdate("NPR", NPR);
            s_instanceMap.AddOrUpdate("NZD", NZD);
            s_instanceMap.AddOrUpdate("OMR", OMR);
            s_instanceMap.AddOrUpdate("PAB", PAB);
            s_instanceMap.AddOrUpdate("PEN", PEN);
            s_instanceMap.AddOrUpdate("PGK", PGK);
            s_instanceMap.AddOrUpdate("PHP", PHP);
            s_instanceMap.AddOrUpdate("PKR", PKR);
            s_instanceMap.AddOrUpdate("PLN", PLN);
            s_instanceMap.AddOrUpdate("PYG", PYG);
            s_instanceMap.AddOrUpdate("QAR", QAR);
            s_instanceMap.AddOrUpdate("RON", RON);
            s_instanceMap.AddOrUpdate("RSD", RSD);
            s_instanceMap.AddOrUpdate("RUB", RUB);
            s_instanceMap.AddOrUpdate("RWF", RWF);
            s_instanceMap.AddOrUpdate("SAR", SAR);
            s_instanceMap.AddOrUpdate("SBD", SBD);
            s_instanceMap.AddOrUpdate("SCR", SCR);
            s_instanceMap.AddOrUpdate("SDG", SDG);
            s_instanceMap.AddOrUpdate("SEK", SEK);
            s_instanceMap.AddOrUpdate("SGD", SGD);
            s_instanceMap.AddOrUpdate("SHP", SHP);
            s_instanceMap.AddOrUpdate("SLL", SLL);
            s_instanceMap.AddOrUpdate("SOS", SOS);
            s_instanceMap.AddOrUpdate("SRD", SRD);
            s_instanceMap.AddOrUpdate("SSP", SSP);
            s_instanceMap.AddOrUpdate("STN", STN);
            s_instanceMap.AddOrUpdate("SVC", SVC);
            s_instanceMap.AddOrUpdate("SYP", SYP);
            s_instanceMap.AddOrUpdate("SZL", SZL);
            s_instanceMap.AddOrUpdate("THB", THB);
            s_instanceMap.AddOrUpdate("TJS", TJS);
            s_instanceMap.AddOrUpdate("TMT", TMT);
            s_instanceMap.AddOrUpdate("TND", TND);
            s_instanceMap.AddOrUpdate("TOP", TOP);
            s_instanceMap.AddOrUpdate("TRY", TRY);
            s_instanceMap.AddOrUpdate("TTD", TTD);
            s_instanceMap.AddOrUpdate("TWD", TWD);
            s_instanceMap.AddOrUpdate("TZS", TZS);
            s_instanceMap.AddOrUpdate("UAH", UAH);
            s_instanceMap.AddOrUpdate("UGX", UGX);
            s_instanceMap.AddOrUpdate("USD", USD);
            s_instanceMap.AddOrUpdate("USN", USN);
            s_instanceMap.AddOrUpdate("UYI", UYI);
            s_instanceMap.AddOrUpdate("UYU", UYU);
            s_instanceMap.AddOrUpdate("UZS", UZS);
            s_instanceMap.AddOrUpdate("VEF", VEF);
            s_instanceMap.AddOrUpdate("VND", VND);
            s_instanceMap.AddOrUpdate("VUV", VUV);
            s_instanceMap.AddOrUpdate("WST", WST);
            s_instanceMap.AddOrUpdate("XAF", XAF);
            s_instanceMap.AddOrUpdate("XCD", XCD);
            s_instanceMap.AddOrUpdate("XDR", XDR);
            s_instanceMap.AddOrUpdate("XOF", XOF);
            s_instanceMap.AddOrUpdate("XPF", XPF);
            s_instanceMap.AddOrUpdate("XSU", XSU);
            s_instanceMap.AddOrUpdate("XUA", XUA);
            s_instanceMap.AddOrUpdate("YER", YER);
            s_instanceMap.AddOrUpdate("ZAR", ZAR);
            s_instanceMap.AddOrUpdate("ZMW", ZMW);
            s_instanceMap.AddOrUpdate("ZWL", ZWL);
        }

        private Currency(String code)
        {
            _code = code;
        }

        public String Code
        {
            get { return _code; }
        }

        public static IEnumerable<Currency> AvailableCurrencies
        {
            get
            {
                foreach (var cur in s_instanceMap.Values)
                    yield return cur;
            }
        }

        public static Currency Parse(String currencyCode)
        {
            // ArgumentChecker.NotNull(currencyCode, nameof(currencyCode));
            // check cache before matching
            if (currencyCode.Matches("[A-Z][A-Z][A-Z]") == false)
            {
                throw new ArgumentException(String.Format(LocalizedResources.Instance().INVALID_CURRENCY_CODE, currencyCode));
            }

            if (!s_instanceMap.Any(x => x.Key == currencyCode))
            {
                s_instanceMap.AddOrUpdate(currencyCode, new Currency(currencyCode));
            }
            return s_instanceMap[currencyCode];
        }


        /// <summary>
        /// Compares this currency to another by alphabetical comparison of the code.
        /// </summary>
        /// <param name="other">the other currency, not null</param>
        /// <returns>negative if earlier alphabetically, 0 if equal, positive if greater alphabetically</returns>
        public int CompareTo(Currency other)
        {
            return _code.CompareTo(other._code);
        }
    }
}
