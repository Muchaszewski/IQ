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
    // -1 90

    // Use this for initialization
    void Start()
    {
        var count = GenerationStorage.Instance.Spots.Count;
        var spotListSorted = GenerationStorage.Instance.Spots.OrderBy(x => x.Level).ToList();
        for (int i = 0; i < count; i++)
        {
            var item = spotListSorted[i];
            var area = Instantiate(AreaButton).GetComponent<AreaButtonController>();
            area.Spot = item;
            area.transform.SetParent(transform);
            area.transform.localScale = Vector3.one;
            area.RectTransform.anchoredPosition = new Vector2(0, -20 - 40 * i);
        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(552.5f, count * 40);
    }
}
