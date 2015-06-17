using UnityEngine;
using System.Collections;
using UnityEditor.AnimatedValues;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ItemIcon))]
[RequireComponent(typeof(RectTransform))]
public class ShowTooltip : MonoBehaviour
{
    private ToolTipManager toolTip;
    private RectTransform _rectTransofrm;
    private Rect ItemPosition;
    private ItemIcon ItemData;

    // Use this for initialization
    void Start()
    {
        toolTip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<ToolTipManager>();
        ItemData = GetComponent<ItemIcon>();
        _rectTransofrm = GetComponent<RectTransform>();
        UpdatePosition();
    }

    void UpdatePosition()
    {
        float rectX = _rectTransofrm.rect.width * _rectTransofrm.localScale.x;
        float rectY = _rectTransofrm.rect.height * _rectTransofrm.localScale.y;

        ItemPosition = new Rect(transform.position.x - rectX / 2, transform.position.y - rectY / 2, rectX, rectY);
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
