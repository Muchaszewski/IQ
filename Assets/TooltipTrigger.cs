using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipTrigger : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public string Text = "Tooltip_Message";

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.TooltipShow = true;
        var textComponent = GameManager.Instance.TooltipGameObject.transform.GetChild(0).GetComponent<Text>();
        var rectTransform = GameManager.Instance.TooltipGameObject.GetComponent<RectTransform>();
        textComponent.text = Text;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, textComponent.preferredHeight);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.TooltipShow = false;
    }


}
