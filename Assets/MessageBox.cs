using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text TitleText;
    public Text MessageText;
    public Button NextButton;
    public Button SkipButton;

    public RectTransform RectTransform { get; private set; }

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}
