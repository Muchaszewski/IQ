using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ObjectDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public GameObject TargetGameObject;

    private Vector3 _holdPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _holdPosition = TargetGameObject.transform.position - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (new Rect(0, 0, Screen.width, Screen.height).Contains(Input.mousePosition))
            {
                TargetGameObject.transform.position = Input.mousePosition + _holdPosition;
            }
        }
    }
}
