using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using UnityEngine.UI;
using InventoryQuest.Components;
using System.Linq;
using System.Text;

public class SpotManager : MonoBehaviour
{
    public GameObject AreaButton;
    public GameObject AreaButtonCategory;
    public MapManager MapManager;
    // -1 90

    // Use this for initialization
    void Start()
    {
        CreateSpotOnList();
        Spot.onNewAreaUnlocked += Spot_onNewAreaUnlocked;
    }

    public void OnDestroy()
    {
        Spot.onNewAreaUnlocked -= Spot_onNewAreaUnlocked;
    }

    void CreateSpotOnList()
    {
        RemoveAllChilds();
        var count = 0;
        var spotListSorted = GenerationStorage.Instance.Spots.OrderByDescending(x => x.Category).ThenBy(x => x.Level).ToList();
        string currentCategory = "";
        int categoryCount = 0;
        for (int i = 0; i < GenerationStorage.Instance.Spots.Count; i++)
        {
            var item = spotListSorted[i];
            if (item.IsUnlocked == false) continue;
            count++;

            if (currentCategory != item.Category)
            {
                var category = Instantiate(AreaButtonCategory);
                category.transform.GetChild(0).GetComponent<Text>().text = item.Category;
                category.transform.SetParent(transform);
                category.transform.localScale = Vector3.one;
                category.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 - 40 * (i + categoryCount));
                categoryCount++;
                currentCategory = item.Category;
            }

            var area = Instantiate(AreaButton).GetComponent<AreaButtonController>();
            area.Spot = item;
            area.transform.SetParent(transform);
            area.transform.localScale = Vector3.one;
            area.RectTransform.anchoredPosition = new Vector2(0, -20 - 40 * (i + categoryCount));

            MapManager.CreateMapIcon(item, area);
        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(552.5f, (count + categoryCount) * 40);
    }

    void RemoveAllChilds()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in MapManager.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Spot_onNewAreaUnlocked(object sender, System.EventArgs e)
    {
        CreateSpotOnList();
    }
}
