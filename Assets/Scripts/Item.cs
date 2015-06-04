using UnityEngine;
using System.Collections;
using System.Linq;
using InventoryQuest;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Game;
using InventoryQuest.Utils;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public InventoryQuest.Components.Items.Item ItemData { get; set; }

    private RectTransform _rectTransform;

    private static RectTransform _panel;
    private static RectTransform _inventory;
    private static Canvas _canvas;

    // Use this for initialization
    void Start()
    {
        if (_canvas == null)
        {
            _inventory = transform.parent.GetComponent<RectTransform>();
            _panel = transform.parent.parent.GetComponent<RectTransform>();
            _canvas = FindObjectOfType<Canvas>();
        }
        var spot = CurrentGame.Instance.Spot;
        ItemData = RandomItemFactory.CreateItem(1, spot);
        SetIcon();
        _rectTransform = GetComponent<RectTransform>();
    }

    void SetIcon()
    {
        var u = ImagesNames.ItemsImageNames[ItemData.ImageID.ImageIDType].FullNameList[ItemData.ImageID.ImageIDItem];
        var sprite = Resources.Load<Sprite>(FileUtility.AssetsRelativePath(u));
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Drag()
    {
        this.transform.position = Input.mousePosition;
        this.transform.SetParent(_canvas.transform);
    }

    public void Drop()
    {
        this.transform.SetParent(_inventory.transform);
        this._rectTransform.anchoredPosition = Vector2.one;
    }
}
