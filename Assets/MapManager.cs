using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InventoryQuest.Components;
using InventoryQuest.Game;

public class MapManager : MonoBehaviour
{
    public GameObject MapIcon;
    public GameObject LineGameObject;
    public GameObject EmptyGameObject;
    public float PositionToScale;

    private GameObject lineContainer;
    private GameObject areaContainer;
    private List<AreaIcon> GameObjects = new List<AreaIcon>();
    private List<ConnectionPair> ConnectionPairs = new List<ConnectionPair>();

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
        areaIcon.Spot = spot;

        areaIcon.transform.SetParent(areaContainer.transform);
        var rectTransform = areaIcon.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = areaIcon.Position;
        rectTransform.localScale = new Vector3(areaIcon.Size, areaIcon.Size, areaIcon.Size);
        areaIcon.GetComponent<TooltipTrigger>().Text = spot.Category + "/n" + spot.Level + " Recomended Level" + "/n" + spot.Name;
        GameObjects.Add(areaIcon);
        foreach (var connection in spot.ListConnections)
        {
            var connectedSpot = Spot.FindSpotByConnection(connection);
            if (ConnectionPairs.FirstOrDefault((x) => x.Form == connectedSpot.ID && x.To == spot.ID) != null)
            {
                continue;
            }
            if (connectedSpot.IsUnlocked)
            {
                ConnectionPairs.Add(new ConnectionPair { Form = spot.ID, To = connectedSpot.ID });
                var line = Instantiate(LineGameObject).GetComponent<Line>();
                line.transform.SetParent(lineContainer.transform);
                line.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition;
                line.EndVector2 = ((connectedSpot.Position * PositionToScale) - areaIcon.Position);
            }
        }

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
        }
        ConnectionPairs = new List<ConnectionPair>();
        GameObjects = new List<AreaIcon>();
        lineContainer = Instantiate(EmptyGameObject);
        lineContainer.transform.SetParent(transform);
        lineContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        lineContainer.name = "Lines";
        areaContainer = Instantiate(EmptyGameObject);
        areaContainer.transform.SetParent(transform);
        areaContainer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        areaContainer.name = "Areas";
    }
}
[Serializable]
internal class ConnectionPair
{
    public int Form { get; set; }
    public int To { get; set; }
}
