using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using UnityEngine.UI;
using InventoryQuest.Components.Items;

[RequireComponent(typeof(Text))]
public class CustomHandler_DefenceType : MonoBehaviour
{
    private Text _text;

    // Use this for initialization
    void Start()
    {
        EquipmentSlot.ItemEquiped += EquipmentSlot_ItemEquiped;
        EquipmentSlot.ItemUnequiped += EquipmentSlot_ItemUnequiped;
        _text = GetComponent<Text>();
        UpdateType();
    }

    void EquipmentSlot_ItemEquiped(object sender, System.EventArgs e)
    {
        UpdateType();
    }
    void EquipmentSlot_ItemUnequiped(object sender, System.EventArgs e)
    {
        UpdateType();
    }

    void UpdateType()
    {
        int resultType = -1;
        foreach (var item in CurrentGame.Instance.Player.Equipment.Items)
        {
            if (item == null) continue;
            if (item.Skill == InventoryQuest.Components.Items.EnumItemClassSkill.Heavy)
            {
                resultType = Mathf.Max(resultType, 3);
            }
            else if (item.Skill == InventoryQuest.Components.Items.EnumItemClassSkill.Medium)
            {
                resultType = Mathf.Max(resultType, 2);
            }
            else if (item.Skill == InventoryQuest.Components.Items.EnumItemClassSkill.Light)
            {
                resultType = Mathf.Max(resultType, 1);
            }
            else if (item.Skill == InventoryQuest.Components.Items.EnumItemClassSkill.Cloth)
            {
                resultType = Mathf.Max(resultType, 0);
            }
        }

        if (resultType != -1)
        {
            _text.text = ((EnumItemClassSkill)resultType).ToString();
        }
        else
        {
            _text.text = "None";
        }
    }
}
