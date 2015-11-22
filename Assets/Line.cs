using UnityEngine;
using System.Collections;
using Extensions;
using UnityEditor;

public class Line : MonoBehaviour
{
    public float Width;
    public Vector2 EndVector2;

    public void Start()
    {
        SetLine();
    }

    void SetLine()
    {
        var v3 = new Vector2(0, 0) - EndVector2;
        var angle = Mathf.Atan2(v3.y, -v3.x) * Mathf.Rad2Deg;
        var distance = Vector2.Distance(new Vector2(0, 0), Quaternion.Euler(0, 0, angle) * EndVector2);
        var rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(distance, Width);
        transform.eulerAngles = new Vector3(0, 0, -angle);
    }
}
