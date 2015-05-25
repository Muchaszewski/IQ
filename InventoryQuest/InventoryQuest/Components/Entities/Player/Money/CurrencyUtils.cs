using System;
using System.Collections.Generic;
using System.Reflection;

namespace InventoryQuest.Components.Entities.Money
{
    public static class CurrencyUitls
    {
        private static List<long> cachedCurrencyValues;
        private static List<NameAttribute> cachedCurrencyNames;

        /// <summary>
        ///     Converts money from one currency to another.
        /// </summary>
        /// <param name="amount">Amount of money to convert</param>
        /// <param name="from">Currency of given <see cref="amount" /></param>
        /// <param name="to">Target currency</param>
        public static long Convert(long amount, Currency from, Currency to)
        {
            if (from == to)
            {
                return amount;
            }
            return (amount*from.GetValue())/to.GetValue();
        }

        /// <summary>
        ///     Returns nicely formated string with currency based amount of money.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="useLongNames">Use long currency names instead of short ones</param>
        /// <returns></returns>
        public static string ToNiceString(long amount, bool useLongNames)
        {
            if (cachedCurrencyNames == null || cachedCurrencyValues == null)
            {
                CacheCurrencyInfos();
            }

            var finalString = "";

            for (var i = cachedCurrencyValues.Count - 1; i > 0; i--)
            {
                var value = cachedCurrencyValues[i];
                NameAttribute name = cachedCurrencyNames[i];

                var currentValue = amount/value;

                if (currentValue > 0)
                {
                    finalString += currentValue + " " + (useLongNames ? name.LongName : name.ShortName) + " ";
                    amount -= currentValue*value;
                }
            }

            if (finalString == string.Empty || amount > 0)
            {
                finalString += amount + " " +
                               (useLongNames ? cachedCurrencyNames[0].LongName : cachedCurrencyNames[0].ShortName);
            }

            finalString = finalString.TrimEnd(' ');

            return finalString;
        }

        private static void CacheCurrencyInfos()
        {
            var currencies = (Currency[]) Enum.GetValues(typeof (Currency));
            MemberInfo[] currencyMembers =
                typeof (Currency).GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);

            Utilities.Assert(currencies.Length == currencyMembers.Length,
                "Currencies values and names count does not match");

            var currenciesValues = new List<long>();
            var currenciesNames = new List<NameAttribute>();

            for (var i = 0; i < currencies.Length; i++)
            {
                Currency c = currencies[i];

                currenciesValues.Add(c.GetValue());
                currenciesNames.Add(NameAttribute.GetNameAttribute(currencyMembers[i]));
            }

            Utilities.Assert(currenciesValues.Count == currenciesNames.Count,
                "Currencies values and names count does not match");

            cachedCurrencyNames = currenciesNames;
            cachedCurrencyValues = currenciesValues;
        }
    }
}