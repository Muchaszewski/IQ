using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolTipManager : MonoBehaviour
{
    public Color[] RarityColors;
    public GameObject Separator;
    public GameObject Text;
    public Canvas Canvas;

    public bool Show { get; set; }

    private RectTransform RactTransform;

    //TODO change to one call SetTooltip and PoolManager
    private List<GameObject> TempGC = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        RactTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!Show)
        {
            transform.position = new Vector3(-10000, -10000, 0);
        }
        else
        {
            transform.position = (Vector2)Input.mousePosition;
            Show = false;
        }
    }

    public void SetTooltip(InventoryQuest.Components.Items.Item item)
    {
        //TODO change to one call SetTooltip and PoolManager
        foreach (var o in TempGC)
        {
            Destroy(o);
        }
        TempGC = new List<GameObject>();
        //Tooltip code
        AddLabel(new Vector2(50, 50), "Test");
        AddSeparator(new Vector2(0, 100));
    }

    void SetWindowSize(float newHeight)
    {
        RactTransform.sizeDelta.Set(RactTransform.sizeDelta.x, newHeight);
    }

    void AddWindowSize(float height)
    {
        RactTransform.sizeDelta.Set(RactTransform.sizeDelta.x, RactTransform.sizeDelta.y + height);
    }

    void AddLabel(Vector2 position, string text)
    {
        AddLabel(position, text, 20, new Color(112, 112, 112));
    }

    void AddLabel(Vector2 position, string text, int fontSize)
    {
        AddLabel(position, text, fontSize, new Color(112, 112, 112));
    }

    Text AddLabel(Vector2 position, string text, int fontSize, Color fontColor)
    {
        Text textPrefab = Instantiate(Text).GetComponent<Text>();
        textPrefab.name = "Text" + text;
        textPrefab.text = text;
        textPrefab.color = fontColor;
        textPrefab.fontSize = fontSize;
        textPrefab.transform.SetParent(this.transform);
        textPrefab.transform.localScale = Vector3.one;
        textPrefab.transform.position = this.transform.position + Vector3.Scale(position, Canvas.transform.localScale);
        TempGC.Add(textPrefab.gameObject);
        return textPrefab;
    }

    GameObject AddSeparator(Vector2 position)
    {
        GameObject separator = Instantiate(Separator);
        separator.transform.SetParent(this.transform);
        separator.transform.localScale = Vector3.one;
        separator.transform.position = this.transform.position + Vector3.Scale(position, Canvas.transform.localScale);
        TempGC.Add(separator.gameObject);
        return separator;
    }
}
