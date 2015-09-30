using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine.UI;
using InventoryQuest.Utils;

public class AreaImageController : MonoBehaviour
{
    public float FadeTime = 2f;
    public Image ImageTravelingOverlay;
    private Image _image;

    public void Start()
    {
        _image = GetComponent<Image>();
        ImageTravelingOverlay.color = Color.clear;
    }

    public void ChangeBackground(string image)
    {
        if (image == null || string.Empty == image)
        {
            _image.sprite = ResourceManager.Get("Sprites/gui/missingImage_0");
            return;
        }
        ImageTravelingOverlay.color = Color.clear;
        ImageTravelingOverlay.sprite = ResourceManager.Get("Sprites/landscapes/" + image);
        StartCoroutine(FadeEnumerator());
    }

    public IEnumerator FadeEnumerator()
    {
        do
        {
            ImageTravelingOverlay.color = new Color(1, 1, 1,
                ImageTravelingOverlay.color.a + Time.deltaTime / FadeTime);
            yield return null;
        } while (ImageTravelingOverlay.color.a < 1);
        _image.sprite = ImageTravelingOverlay.sprite;
        ImageTravelingOverlay.color = Color.clear;
    }
}
