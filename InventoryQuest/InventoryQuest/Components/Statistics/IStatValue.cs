namespace InventoryQuest.Components.Statistics
{
    /// <summary>
    ///     Single statistic
    /// </summary>
    public interface IStatValue<T>
    {
        /// <summary>
        ///     Type of this statistics
        /// </summary>
        EnumTypeStat Type { get; set; }

        /// <summary>
        ///     Additional stat from external sources
        /// </summary>
        T Extend { get; set; }

        /// <summary>
        ///     Base value
        ///     Setting base value updates
        ///     Current value as well
        /// </summary>
        T Base { get; set; }

        /// <summary>
        ///     Current value
        /// </summary>
        T Current { get; set; }

        /// <summary>
        ///     Set minimal value for stat
        /// </summary>
        T Minimum { get; set; }

        /// <summary>
        ///     Set maximal value for stat
        ///     <para />
        /// </summary>
        T Maximum { get; set; }

        /// <summary>
        ///     Resets Current value to Base
        /// </summary>
        void Reset();

        /// <summary>
        ///     Change Current to Minimum
        /// </summary>
        void TurnOff();

        /// <summary>
        ///     Change current to random number between Base and Min
        /// </summary>
        void Shred(T min);

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        void ChangeValues(IStatValue<T> value);

        /// <summary>
        ///     Return Current procent
        /// </summary>
        /// <returns></returns>
        double GetPercent();

        /// <summary>
        ///     Set Current base on procentage </para>
        ///     0.1 = 10%; 1 = 100% etc...
        /// </summary>
        /// <param name="procent"></param>
        void SetPercent(double procent);

        /// <summary>
        ///     Return if this stat have extended stats
        /// </summary>
        /// <returns></returns>
        bool IsExtended();
    }
}