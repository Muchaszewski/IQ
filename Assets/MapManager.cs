using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventoryQuest.Components;
using InventoryQuest.Game;

public class MapManager : MonoBehaviour
{
    public GameObject MapIcon;
    public float PositionToScale;

    private List<AreaIcon> GameObjects = new List<AreaIcon>();

    public void CreateMapIcon(Spot spot, AreaButtonController buttonController)
    {
        var areaIcon = Instantiate(MapIcon).GetComponent<AreaIcon>();

        areaIcon.Position = spot.Position * PositionToScale;
        if (Math.Abs(spot.Size) < 0.01f)
        {
            areaIcon.Size = 1;
        }
        else
        {
            areaIcon.Size = spot.Size;
        }
        areaIcon.Name = spot.Name;
        areaIcon.Category = spot.Category;
        areaIcon.AreaButtonController = buttonController;

        areaIcon.transform.SetParent(transform);
        var rectTransform = areaIcon.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = areaIcon.Position;
        rectTransform.localScale = new Vector3(areaIcon.Size, areaIcon.Size, areaIcon.Size);
        areaIcon.GetComponent<TooltipTrigger>().Text = spot.Category + "/n" + spot.Level + " Recomended Level" + "/n" + spot.Name;
        GameObjects.Add(areaIcon);
    }

    public void CenterMap()
    {
        var go = GameObjects.Find(x => x.Name.Equals(CurrentGame.Instance.Spot.Name));
        GetComponent<RectTransform>().anchoredPosition = -1 * go.GetComponent<RectTransform>().anchoredPosition;
    }

    public void RemoveAllChilds()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
            GameObjects = new List<AreaIcon>();
        }
    }
}
