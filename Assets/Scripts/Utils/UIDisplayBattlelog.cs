using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using UnityEngine.UI;

public class UIDisplayBattlelog : MonoBehaviour
{
    private Text _text;
    private RectTransform _rect;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _text.text = CurrentGame.Instance.FightController.BattleLog.ToString();
        var delta = _rect.sizeDelta.y - _text.preferredHeight;
        if (delta < 0)
        {
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _text.preferredHeight);
            if (Mathf.Abs(_rect.anchoredPosition.y) <= 3)
            {
                _rect.anchoredPosition = Vector2.zero;
            }
            else
            {
                _rect.anchoredPosition = new Vector2(0, _rect.anchoredPosition.y + delta);
            }
        }
    }

}
