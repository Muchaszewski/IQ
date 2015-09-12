using System;

namespace InventoryQuest.Components.Entities.Money
{
    /// <summary>
    ///     Wallet - holds money.
    /// </summary>
    [Serializable]
    public class Wallet
    {

        public event EventHandler AddedMoney = delegate(object sender, EventArgs args) {  };

        private long _CurrentAmount;
        private long _MaxAmount;

        /// <summary>
        ///     Create new wallet.
        /// </summary>
        /// <param name="maxAmount">Max amount of money that can be holden in this wallet.</param>
        public Wallet(long maxAmount = long.MaxValue)
        {
            MaxAmount = maxAmount;
        }

        /// <summary>
        ///     Current amount of money in the wallet.
        /// </summary>
        public long CurrentAmount
        {
            get { return _CurrentAmount; }
            set { _CurrentAmount = Math.Max(Math.Min(value, MaxAmount), 0); }
        }

        /// <summary>
        ///     Maximum amount of money in this wallet.
        /// </summary>
        public long MaxAmount
        {
            get { return _MaxAmount; }
            set { _MaxAmount = Math.Max(value, 0); }
        }

        /// <summary>
        ///     Returns current amount of money in given currency.
        /// </summary>
        /// <param name="currency">Currency to return amount of money</param>
        public long GetCurrentAmount(Currency currency = Currency.Copper)
        {
            return CurrencyUitls.Convert(CurrentAmount, Currency.Copper, currency);
        }

        /// <summary>
        ///     Sets current amount of money in the wallet.
        ///     <para />
        ///     Returns: New amount of money that is in the wallet.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public long SetCurrentAmount(long amount, Currency currency = Currency.Copper)
        {
            CurrentAmount = CurrencyUitls.Convert(amount, currency, Currency.Copper);
            return CurrentAmount;
        }

        /// <summary>
        ///     Add money to the wallet.
        ///     <para />
        ///     Returns: Amount of money that could not have been added to the wallet.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public long AddMoney(long amount, Currency currency = Currency.Copper)
        {
            var prev = CurrentAmount;
            CurrentAmount = CurrencyUitls.Convert(amount, currency, Currency.Copper);
            var delta = amount - (CurrentAmount - prev);
            AddedMoney.Invoke(this, EventArgs.Empty);
            return delta;
        }

        /// <summary>
        ///     Try to spend some money. If there is not enough money in the wallet, no money have
        ///     been used and method returns false. Otherwise, money is decreased by given amount and method returns true.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public bool TrySpend(long amount, Currency currency = Currency.Copper)
        {
            var realAmount = CurrencyUitls.Convert(amount, currency, Currency.Copper);

            if (CurrentAmount < realAmount)
            {
                return false;
            }

            CurrentAmount -= realAmount;
            return true;
        }

        /// <summary>
        ///     Returns nicely formated amount of money.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CurrencyUitls.ToNiceString(CurrentAmount, false);
        }

        /// <summary>
        ///     Returns nicely formated amount of money with full currency names.
        /// </summary>
        /// <returns></returns>
        public string ToLongString()
        {
            return CurrencyUitls.ToNiceString(CurrentAmount, true);
        }
    }
}