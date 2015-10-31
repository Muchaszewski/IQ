using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using InventoryQuest.Game;
using UnityEngine.UI;

public class AreaIcon : MonoBehaviour
{
    private Image _image;


    // Use this for initialization
    void Start()
    {
        _image = GetComponent<Image>();
        SetIcon(CurrentGame.Instance.Spot.ImageString);
    }

    public void SetIcon(string icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            _image.sprite = ResourceManager.Get("Sprites/gui/missingImage_0");
            return;
        }
        _image.sprite = ResourceManager.Get("Sprites/landscapes/" + icon + "_icon");
    }
}
