using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InventoryQuest.Components.Entities.Player;
using InventoryQuest.Game;
using UnityEditor;
using UnityEngine.UI;

public class StatisticsManager : UIManager
{

    //_____________________________________________________________________________________________________________

    private RectTransform _rectTransform;
    private Player _player;
    [SerializeField]
    [HideInInspector]
    private List<TextStringPair> _statisticsTexts = new List<TextStringPair>();

    public List<TextStringPair> StatisticsTexts
    {
        get { return _statisticsTexts; }
        set { _statisticsTexts = value; }
    }

    public class TextStringPair
    {
        [SerializeField]
        public object Text { get; set; }
        [SerializeField]
        public Text GameObjectText { get; set; }

        public TextStringPair(object text, Text gameObjectText)
        {
            Text = text;
            GameObjectText = gameObjectText;
        }
    }

    //__________________________________________________MonoBehaviour______________________________________________

    // Use this for initialization
    void Start()
    {
        _rectTransform = new RectTransform();
        _player = CurrentGame.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatistics();
    }

    //______________________________________________End MonoBehaviour End__________________________________________



    //______________________________________________Public Methods_________________________________________________

#if UNITY_EDITOR
    /// <summary>
    /// Create all labels for tooltip
    /// </summary>
    /// <param name="item"></param>
    [Obsolete("Only for inspector use")]
    public void CreateLabels()
    {
        _rectTransform = GetComponent<RectTransform>();
        CreateStatistics();
    }
#endif

    //__________________________________________End Public Methods End_____________________________________________

    void UpdateStatistics()
    {
        Debug.Log(StatisticsTexts.Count);
        for (int i = 0; i < StatisticsTexts.Count; i++)
        {
            StatisticsTexts[i].GameObjectText.text = StatisticsTexts[i].Text.ToString();
        }
    }

    void CreateStatistics()
    {
#if UNITY_EDITOR
        _player = CurrentGame.Instance.Player;
        //If current stat is empty, extract field from Addlabel and add it in preprocessor command
        //UNITY_EDIOTOR enter test value and surround with "if (!EditorApplication.isPlaying)"
        var playerName = _player.Name;
        if (!EditorApplication.isPlaying)
        {
            playerName = "Regnar";
        }
#endif
        AddLabel(new Vector2(0, -15), playerName, 50, TextAnchor.UpperCenter);
        //If stat is updateable then extend Text with UpdateableStatistics method and add as object 
        //stat to display. This stat then will be checked and changed every update.
        //String in Addlabel is only for preview in editor, since text value will be overitten anyway
        AddLabel(new Vector2(60, 20), _player.Stats.Strength.Type.ToString(), 30);
        AddLabel(new Vector2(-60, 20), "Strength", 30).UpdateableStatistics(_player.Stats.Strength.Current, this);

    }

    //______________________________________________Static__________________________________________________
}

static class UpdateableText
{
    public static void UpdateableStatistics(this Text text, object content, StatisticsManager manager)
    {
        manager.StatisticsTexts.Add(new StatisticsManager.TextStringPair(content, text));
    }
}
