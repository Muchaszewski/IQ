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
        GameManager.Instance.TooltipGameObject.transform.GetChild(0).GetComponent<Text>().text = Text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.TooltipShow = false;
    }


}
