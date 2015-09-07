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
    // -1 90

    // Use this for initialization
    void Start()
    {
        var count = GenerationStorage.Instance.Spots.Count;
        var spotListSorted = GenerationStorage.Instance.Spots.OrderByDescending(x => x.Category).ThenBy(x => x.Level).ToList();
        string currentCategory = "";
        int categoryCount = 0;
        for (int i = 0; i < count; i++)
        {
            var item = spotListSorted[i];

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
        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(552.5f, (count + categoryCount) * 40);
    }
}
