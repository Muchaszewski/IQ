using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class TutorialActions : MonoBehaviour
{
    public static TutorialActions Instance { get; set; }

    public MessageBox MessageBox;

    public Button[] MenuButtons;

    // Use this for initialization
    void Start()
    {
        Instance = this;
        for (int i = 0; i < MenuButtons.Length - 1; i++)
        {
            MenuButtons[i].gameObject.SetActive(false);
        }
    }

    //TODO Change methods name for proper actions @MaciejLitwin
    public void TutorialIntroduction()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(-200, 214);
        //Rough translation: Cześć
        MessageBox.TitleText.text = "おはよ！";
        //Witaj!Rowerze. jak się masz! Ten świat jest cudowny, Czy nie!?
        MessageBox.MessageText.text = "よおこそ！自転車さま。げんきですか？この世界すばらしですね？";
        MessageBox.NextButton.onClick.AddListener(TutorialIntroduction2);
    }

    //TODO Change methods name for proper actions @MaciejLitwin
    public void TutorialIntroduction2()
    {
        MessageBox.RectTransform.anchoredPosition = new Vector2(10, 214);
        //Rough translation: Ja
        MessageBox.TitleText.text = "わたし";
        //Jestem obserwatorem. Ten świat to twój ekwipunek!
        MessageBox.MessageText.text = "わたしはスーパーバイザー。この世界はあなたの装置です。";
        MessageBox.NextButton.onClick.AddListener(() => {
            //Create action without new method with anyonimus deleage
            MessageBox.RectTransform.anchoredPosition = new Vector2(10000, 10000);
        });
    }
}
