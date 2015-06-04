using System;

namespace InventoryQuest.Components.Statistics
{
    /// <summary>
    ///     Single statistic
    /// </summary>
    [Serializable]
    public class StatValueInt : IStatValue<int>
    {
        private int _Base;
        private int _Current;
        private int _Extend;
        private int _Maximum = int.MaxValue;
        private int _Minimum = int.MinValue;
        private EnumTypeStat _Type = EnumTypeStat.Unknown;

        /// <summary>
        ///     Constructor
        /// </summary>
        public StatValueInt(EnumTypeStat type = EnumTypeStat.Unknown)
        {
            Type = type;
        }

        /// <summary>
        ///     Type of this statistics
        /// </summary>
        public EnumTypeStat Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        ///     Additional stat from external sources
        /// </summary>
        public int Extend
        {
            get { return _Extend; }
            set
            {
                if (value < Minimum)
                {
                    value = Minimum;
                }
                else if (value > Maximum)
                {
                    value = Maximum;
                }
                var currDiff = Current - Extend;
                _Extend = value;
                Current = Extend + currDiff;
            }
        }

        /// <summary>
        ///     Base value
        ///     Setting base value updates
        ///     Current value as well
        /// </summary>
        public int Base
        {
            get { return _Base; }
            set
            {
                if (value < Minimum)
                {
                    value = Minimum;
                }
                else if (value > Maximum)
                {
                    value = Maximum;
                }
                var currDiff = Current - Base;
                var extDiff = Extend - Base;
                _Base = value;
                Current = Base + currDiff;
                Extend = Base + extDiff;
            }
        }

        /// <summary>
        ///     Current value
        /// </summary>
        public int Current
        {
            get { return _Current; }
            set
            {
                if (value < Minimum)
                {
                    value = Minimum;
                }
                else if (value > Maximum)
                {
                    value = Maximum;
                }
                else if (value > Extend && value > Base)
                {
                    value = Extend;
                }
                _Current = value;
            }
        }

        /// <summary>
        ///     Set minimal value for stat
        /// </summary>
        public int Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                Current = Current;
                Extend = Extend;
                Base = Base;
            }
        }

        /// <summary>
        ///     Set maximal value for stat
        ///     <para />
        /// </summary>
        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                Current = Current;
                Extend = Extend;
                Base = Base;
            }
        }

        /// <summary>
        ///     Resets Current value to Base
        /// </summary>
        public void Reset()
        {
            Current = Base;
        }

        /// <summary>
        ///     Change Current to Minimum
        /// </summary>
        public void TurnOff()
        {
            Current = Minimum;
        }

        /// <summary>
        ///     Change current to random number between Base and Min
        /// </summary>
        public void Shred(int min = 0)
        {
            Current = RandomNumberGenerator.NextRandom(min, Base);
        }

        /// <summary>
        ///     Return Current procent
        /// </summary>
        /// <returns></returns>
        public float GetPercent()
        {
            return (Current / (float)Extend) * 100f;
        }

        /// <summary>
        ///     Set Current base on procentage </para>
        ///     0.1 = 10%; 1 = 100% etc...
        /// </summary>
        /// <param name="procent"></param>
        public void SetPercent(double procent)
        {
            Current = Convert.ToInt32(Base*procent);
        }

        /// <summary>
        ///     this = value
        /// </summary>
        /// <param name="value">new object</param>
        public void ChangeValues(IStatValue<int> value)
        {
            if (value.Type == Type)
            {
                Base = value.Base;
                Current = value.Current;
                Minimum = value.Minimum;
                Maximum = value.Maximum;
                Extend = value.Extend;
            }
            else
            {
                throw new Exception("New stat is diffrent type then original");
            }
        }

        public bool IsExtended()
        {
            if (Extend > Base)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            var prefix = "";
            return prefix + Current;
        }

        /// <summary>
        ///     Regenerate this stat
        /// </summary>
        /// <param name="regen">base stats from witch base regen is taken</param>
        /// <param name="regenValue">% of max value to regen</param>
        public void Regen(float regenPercent = 0, StatValueFloat regen = null)
        {
            Current += (int) (Extend*(regenPercent/100));
            if (regen != null)
            {
                Current += (int) regen.Current;
            }
        }
    }
}