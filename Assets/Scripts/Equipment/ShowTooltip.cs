using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ItemIcon))]
[RequireComponent(typeof(RectTransform))]
public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTipManager ToolTipManager { get; private set; }
    private RectTransform _rectTransofrm;
    private Rect ItemPosition;
    private ItemIcon ItemData;
    private bool _isHoover;

    // Use this for initialization
    void Start()
    {
        ToolTipManager = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<ToolTipManager>();
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

        if (_isHoover)
        {
            ToolTipManager.Show = true;
            ToolTipManager.SetTooltip(ItemData);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHoover = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHoover = false;
    }
}
