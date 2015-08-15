namespace InventoryQuest.Game
{
    public interface IIdle
    {
        /// <summary>
        ///     Refill stats
        ///     <para>Return time required to fill up to given percent</para>
        /// </summary>
        /// <param name="all">
        ///     False - Ready to go if any of stats will be at given percent
        ///     <para>True - Wait for all stats to fill at least to given percent</para>
        /// </param>
        /// <param name="percent">Percent when player is ready to next battle</param>
        /// <returns></returns>
        bool Refill(bool all, int percent = 100);

        bool RefillAll(int percent);
        bool RefillAny(int percent);
    }
}