using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UILabelManager : MonoBehaviour
{
    public GameObject Separator;
    public GameObject Text;
    public GameObject Image;
    public Sprite[] Images;
    public Canvas Canvas;

    /// <summary>
    /// Add new label relative to parent
    /// </summary>
    /// <param name="position">Position relative to Top center of parent</param>
    /// <param name="aligment">Text aligment</param>
    /// <returns></returns>
    protected Text AddLabel(Vector2 position, string text, TextAnchor aligment = TextAnchor.UpperLeft)
    {
        return AddLabel(position, text, 20, new Color(112, 112, 112), aligment);
    }

    /// <summary>
    /// Add new label relative to parent
    /// </summary>
    /// <param name="position">Position relative to Top center of parent</param>
    /// <param name="aligment">Text aligment</param>
    /// <returns></returns>
    protected Text AddLabel(Vector2 position, string text, int fontSize, TextAnchor aligment = TextAnchor.UpperLeft)
    {
        return AddLabel(position, text, fontSize, new Color(112, 112, 112), aligment);
    }

    /// <summary>
    /// Add new label relative to parent
    /// </summary>
    /// <param name="position">Position relative to Top center of parent</param>
    /// <param name="aligment">Text aligment</param>
    /// <returns></returns>
    protected Text AddLabel(Vector2 position, string text, int fontSize, Color fontColor, TextAnchor aligment = TextAnchor.UpperLeft)
    {
        Text textPrefab = GameObject.Instantiate(Text).GetComponent<Text>();
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

    /// <summary>
    /// Add new separator relative to parent
    /// </summary>
    /// <param name="position">Position relative to Top center of parent</param>
    /// <returns></returns>
    protected GameObject AddSeparator(Vector2 position)
    {
        GameObject separator = GameObject.Instantiate(Separator);
        CreateParent(position, separator.GetComponent<RectTransform>());
        return separator;
    }

    /// <summary>
    /// Add new Image relative to parent
    /// </summary>
    /// <param name="position">Position relative to Top center of parent</param>
    /// <param name="size">Size of sprite</param>
    protected Image AddImage(Vector2 position, Vector2 size, Sprite sprite)
    {
        Image image = GameObject.Instantiate(Image).GetComponent<Image>();
        image.sprite = sprite;
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
    protected virtual void CreateParent(Vector2 position, RectTransform rectTransform)
    {
        rectTransform.transform.SetParent(this.transform);
        rectTransform.transform.localScale = Vector3.one;
        rectTransform.pivot = new Vector2(0.5f, 1);
        rectTransform.anchoredPosition = position * -1;
    }
}

