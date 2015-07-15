using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventoryQuest.Components.Items;

namespace Assets.Scripts
{
    public static class ItemBackgrounds
    {
        private const string AssetPath = "Sprites/gui/";

        private static readonly string[] BackgroundStrings =
        {
            "gui_itemBackground_grey",
            "gui_itemBackground_lightGrey",
            "gui_itemBackground_diabloBlue",
            "gui_itemBackground_diabloYellow",
            "gui_itemBackground_diabloOrange",
            "gui_itemBackground_diabloGreen",
        };

        public static string Get(EnumItemRarity rarity)
        {
            return AssetPath + BackgroundStrings[(int) rarity];
        }
    }
}
