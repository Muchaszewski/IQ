using UnityEngine;
using System.Collections;
using Extensions;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Text TitleText;
    public Text MessageText;
    public ExtendedButton NextButton;
    public ExtendedButton SkipButton;

    public RectTransform RectTransform { get; private set; }

    void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }
}
