using System;

namespace InventoryQuest.Components.Items
{
    public enum EnumItemModificator
    {
        [ModificatorParameter(50)] Broken,
        [ModificatorParameter(80)] Used,
        [ModificatorParameter(100)] Good,
        [ModificatorParameter(110)] Masters
    }

    /// <summary>
    ///     Item attributes. Used to calculate item stats based on quality of an item
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    internal class ModificatorParameter : Attribute
    {
        public ModificatorParameter(int value)
        {
            Chance = value;
        }

        public int Chance { get; private set; }
    }
}