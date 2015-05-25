using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Item))]
[RequireComponent(typeof(RectTransform))]
public class ShowTooltip : MonoBehaviour
{
    private ToolTipManager toolTip;
    private RectTransform RactTransform;
    private Rect ItemPosition;
    private Item ItemData;
    // Use this for initialization
    void Start()
    {
        toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<ToolTipManager>();
        ItemData = GetComponent<Item>();
        RactTransform = GetComponent<RectTransform>();
        UpdatePosition();
    }

    void UpdatePosition()
    {
        ItemPosition = new Rect(transform.position.x, transform.position.y, RactTransform.rect.width * RactTransform.localScale.x, RactTransform.rect.height * RactTransform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();

        if (ItemPosition.Contains(Input.mousePosition))
        {
            toolTip.Show = true;
            toolTip.SetTooltip(ItemData.ItemData);
        }
    }

}
