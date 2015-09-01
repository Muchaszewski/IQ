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

        private static readonly string[] BackgroundTooltipHeaderStrings =
        {
            "itemTooltip_header_diabloDarkGrey",
            "itemTooltip_header_diabloLightGrey",
            "itemTooltip_header_diabloBlue",
            "itemTooltip_header_diabloYellow",
            "itemTooltip_header_diabloOrange",
            "itemTooltip_header_diabloGreen",
        };

        public static string Get(EnumItemRarity rarity)
        {
            return AssetPath + BackgroundStrings[(int) rarity];
        }

        public static string GetHeader(EnumItemRarity rarity)
        {
            return AssetPath + BackgroundTooltipHeaderStrings[(int)rarity];
        }
    }
}
