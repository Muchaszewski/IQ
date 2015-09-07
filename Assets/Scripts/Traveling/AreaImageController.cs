using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine.UI;
using InventoryQuest.Utils;

public class AreaImageController : MonoBehaviour
{

    public void ChangeBackground(string image)
    {
        if (image == null || string.Empty == image)
        {
            GetComponent<Image>().sprite = ResourceManager.Get("Sprites/gui/missingImage_0");
            return;
        }
        GetComponent<Image>().sprite = ResourceManager.Get("Sprites/landscapes/" + image);
    }
}
