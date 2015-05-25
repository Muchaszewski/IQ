using UnityEngine;
using System.Collections;
using System.Linq;
using InventoryQuest;
using InventoryQuest.Components.Generation.Items;
using InventoryQuest.Game;
using InventoryQuest.Utils;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public InventoryQuest.Components.Items.Item ItemData { get; set; }

    // Use this for initialization
    void Start()
    {
        var spot = CurrentGame.Instance.Spot;
        ItemData = RandomItemFactory.CreateItem(1, spot);
        SetIcon();
    }

    void SetIcon()
    {
        //Debug.Log(ItemData.ImageID + " " + ItemData.Name);

        var u = ImagesNames.ItemsImageNames[ItemData.ImageID.ImageIDType].FullNameList[ItemData.ImageID.ImageIDItem];
        //Debug.Log(u);
        //Debug.Log(FileUtility.AssetsRelativePath(u));
        var sprite = Resources.Load<Sprite>(FileUtility.AssetsRelativePath(u));
        transform.GetChild(0).GetComponent<Image>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
