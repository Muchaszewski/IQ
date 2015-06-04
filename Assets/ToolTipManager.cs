using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolTipManager : UIManager
{

    //Show in inspector
    public Color[] RarityColors;

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
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, newHeight);
    }

    public void AddWindowSize(float height)
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + height);
    }

    public float GetWindowHeight()
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
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

    //__________________________________________Overrided methods_____________________________________________


    protected override void CreateParent(Vector2 position, RectTransform rectTransform)
    {
        base.CreateParent(position, rectTransform);
        _tootipObjects.Add(rectTransform.gameObject);
    }
}
