using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolTipManager : MonoBehaviour
{

    //Show in inspector
    public Color[] RarityColors;
    public GameObject Separator;
    public GameObject Text;
    public GameObject Image;
    public Sprite[] Images;
    public Canvas Canvas;

    //Properties
    public bool Show { get; set; }

    //Private varibles
    private RectTransform _rectTransform;
    private InventoryQuest.Components.Items.Item _item;
    //_____________________________________________________________________________________________________________

    [SerializeField]
    [HideInInspector]
    private List<GameObject> _tootipObjects = new List<GameObject>();

    /// <summary>
    /// List of tooltip objects current displayed
    /// </summary>
    public List<GameObject> TootipObjects
    {
        get { return _tootipObjects; }
        set { _tootipObjects = value; }
    }

    //__________________________________________________MonoBehaviour______________________________________________

    // Use this for initialization
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
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

    //______________________________________________End MonoBehaviour End__________________________________________

    //______________________________________________Public Methods_________________________________________________

    public void SetTooltip(InventoryQuest.Components.Items.Item item)
    {
        if (item != _item)
        {
            _item = item;
            foreach (var o in _tootipObjects)
            {
                Destroy(o);
            }
            _tootipObjects = new List<GameObject>();
            if (item != null)
            {
                //Do not use outside this method or in cusotm inspector methods
#pragma warning disable 618
                CreateLabels(item);
#pragma warning restore 618
            }
        }
    }

    /// <summary>
    /// Create all labels for tooltip
    /// </summary>
    /// <param name="item"></param>
    [Obsolete("Only for inspector use, and SetTooltip method")]
    public void CreateLabels(InventoryQuest.Components.Items.Item item)
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        SetWindowSize(381);
        CreateHeader(item);
        CreateBasicMisc(item);
        //---------------

        //---------------
        CreateValue(item);
    }

    public void SetWindowSize(float newHeight)
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, newHeight);
    }

    public void AddWindowSize(float height)
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + height);
    }

    public float GetWindowHeight()
    {
        return _rectTransform.sizeDelta.y;
    }

    //__________________________________________End Public Methods End_____________________________________________

    void CreateHeader(InventoryQuest.Components.Items.Item item)
    {
        if (item.ExtraName == null)
        {
            AddLabel(new Vector2(0, 30), item.Name, 35, TextAnchor.MiddleCenter);
        }
        else
        {
            AddLabel(new Vector2(0, 15), item.Name, TextAnchor.MiddleCenter);
            AddLabel(new Vector2(0, 42), item.ExtraName, TextAnchor.MiddleCenter);
        }
    }

    void CreateBasicMisc(InventoryQuest.Components.Items.Item item)
    {
        AddLabel(new Vector2(40, 86), item.ValidSlot.ToString() + " " + item.Type.ToString(), 16, Color.gray);
        AddLabel(new Vector2(219, 334), item.Rarity.ToString(), 16, Color.gray, TextAnchor.UpperCenter);

    }

    void CreateValue(InventoryQuest.Components.Items.Item item)
    {
        var relativeHeight = GetWindowHeight();
        AddSeparator(new Vector2(0, relativeHeight));
        AddLabel(new Vector2(0, 15 + relativeHeight), item.Price.ToString(), 35, TextAnchor.MiddleCenter);
        AddWindowSize(50);
    }

    Text AddLabel(Vector2 position, string text, TextAnchor aligment = TextAnchor.UpperLeft)
    {
        return AddLabel(position, text, 20, new Color(112, 112, 112), aligment);
    }

    Text AddLabel(Vector2 position, string text, int fontSize, TextAnchor aligment = TextAnchor.UpperLeft)
    {
        return AddLabel(position, text, fontSize, new Color(112, 112, 112), aligment);
    }

    Text AddLabel(Vector2 position, string text, int fontSize, Color fontColor, TextAnchor aligment = TextAnchor.UpperLeft)
    {
        Text textPrefab = Instantiate(Text).GetComponent<Text>();
        textPrefab.name = "Text " + text;
        textPrefab.text = text;
        textPrefab.color = fontColor;
        textPrefab.fontSize = fontSize;
        textPrefab.alignment = aligment;
        textPrefab.horizontalOverflow = HorizontalWrapMode.Overflow;
        textPrefab.verticalOverflow = VerticalWrapMode.Overflow;
        var rectTransform = textPrefab.GetComponent<RectTransform>();
        CreateParent(position, rectTransform);
        return textPrefab;
    }

    GameObject AddSeparator(Vector2 position)
    {
        GameObject separator = Instantiate(Separator);
        CreateParent(position, separator.GetComponent<RectTransform>());
        return separator;
    }

    Image AddImage(Vector2 position, Vector2 size, Sprite sprite)
    {
        Image image = Instantiate(Image).GetComponent<Image>();
        var rect = image.GetComponent<RectTransform>();
        rect.sizeDelta = size;
        CreateParent(position, rect);
        return image;
    }


    /// <summary>
    /// Set position relative to tooltip parent and add to tooltipObjects
    /// </summary>
    /// <param name="position">Position relative to tooltip (parent)</param>
    /// <param name="rectTransform">Object to set position</param>
    void CreateParent(Vector2 position, RectTransform rectTransform)
    {
        rectTransform.transform.SetParent(this.transform);
        rectTransform.transform.localScale = Vector3.one;
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = position * -1;
        _tootipObjects.Add(rectTransform.gameObject);
    }
}
