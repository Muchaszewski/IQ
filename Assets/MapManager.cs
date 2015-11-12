using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components;

public class MapManager : MonoBehaviour
{
    public GameObject MapIcon;
    public float PositionToScale;

    public void CreateMapIcon(Spot spot, AreaButtonController buttonController)
    {
        var areaIcon = Instantiate(MapIcon).GetComponent<AreaIcon>();

        areaIcon.Position = spot.Position * PositionToScale;
        if (Math.Abs(spot.Size) < 0.05f)
        {
            spot.Size = 1;
        }
        areaIcon.Size = spot.Size;
        areaIcon.Name = spot.Name;
        areaIcon.Category = spot.Category;
        areaIcon.AreaButtonController = buttonController;

        areaIcon.transform.SetParent(transform);
        var rectTransform = areaIcon.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = areaIcon.Position;
        transform.localScale = new Vector3(areaIcon.Size, areaIcon.Size);
        areaIcon.GetComponent<TooltipTrigger>().Text = spot.Category + "/n" + spot.Level + " Recomended Level" + "/n" + spot.Name;
    }
}
