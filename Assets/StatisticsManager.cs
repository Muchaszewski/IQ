using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<TextStringPair> _statisticsPath = new List<TextStringPair>();

    public List<TextStringPair> StatisticsPath
    {
        get { return _statisticsPath; }
        set { _statisticsPath = value; }
    }

    private List<TextObjectPair> _statisticsTexts = new List<TextObjectPair>();

    public List<TextObjectPair> StatisticsTexts
    {
        get { return _statisticsTexts; }
        set { _statisticsTexts = value; }
    }

    [Serializable]
    public class TextStringPair
    {
        [SerializeField]
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [SerializeField]
        private Text _gameObjectText;

        public Text GameObjectText
        {
            get { return _gameObjectText; }
            set { _gameObjectText = value; }
        }

        public TextStringPair(string text, Text gameObjectText)
        {
            _text = text;
            _gameObjectText = gameObjectText;
        }
    }


    public class TextObjectPair
    {
        public object Text { get; set; }

        public Text GameObjectText { get; set; }

        public TextObjectPair(object text, Text gameObjectText)
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
        ConvertStatistics();
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

    void ConvertStatistics()
    {
        for (int i = 0; i < StatisticsPath.Count; i++)
        {
            var types = StatisticsPath[i].Text.Split('.');
            List<object> list = new List<object>();
            list.Add(_player);
            for (int j = 0; j < types.Count(); j++)
            {
                var type = list[j].GetType();
                var field = type.GetProperty(types[j]);
                var value = field.GetValue(list[j], null);
                list.Add(value);
            }
            StatisticsTexts.Add(new TextObjectPair(list[types.Count() - 1], _statisticsPath[i].GameObjectText));
        }
    }


    void UpdateStatistics()
    {
        for (int i = 0; i < StatisticsTexts.Count; i++)
        {
            StatisticsTexts[i].GameObjectText.text = StatisticsTexts[i].Text.ToString();
        }
    }

    void CreateStatistics()
    {

        StatisticsPath = new List<TextStringPair>();

#if UNITY_EDITOR
        _player = CurrentGame.Instance.Player;
        //If current stat is empty, extract field from Addlabel and add it in preprocessor command
        //UNITY_EDIOTOR enter test value and surround with "if (!EditorApplication.isPlaying)"
        
        var playerName = _player.Name;
        if (!EditorApplication.isPlaying)
        {
            playerName = "Ragnar";
        }
#endif

        string playerLevelText = "LEVEL " + _player.Level;

        //If stat is updateable then extend Text with UpdateableStatistics method and add as object 
        //stat to display. This stat then will be checked and changed every update.
        //String in Addlabel is only for preview in editor, since text value will be overitten
        //Also this string will be displayed if object is null *(to consider change to String.Empty)
        //
        //UpdateableStatistics path can be intelisence finished by typing _player. ... and removeing _player. 
        //All shoudl be surrounded with quotation marks



        /////////////////
        // Player info //
        /////////////////

        AddLabel(new Vector2(3, 10), playerName, 38, TextAnchor.UpperCenter);
        AddLabel(new Vector2(3, 49), playerLevelText, 20, TextAnchor.UpperCenter);



        ///////////
        // Stats //
        ///////////

        // Base Stats

        //AddLabel(new Vector2(180, 160), _player.Stats.Strength.Type.ToString(), 30);
        AddLabel(new Vector2(155, 178), "STR", 25);
        AddLabel(new Vector2(45, 178), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this);

        AddLabel(new Vector2(155, 207), "CON", 25);
        AddLabel(new Vector2(45, 207), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?

        AddLabel(new Vector2(155, 236), "AGI", 25);
        AddLabel(new Vector2(45, 236), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?

        AddLabel(new Vector2(155, 265), "PER", 25);
        AddLabel(new Vector2(45, 265), "VAL", 25).UpdateableStatistics("Stats.Perception.Current", this);

        AddLabel(new Vector2(155, 294), "WIS", 25);
        AddLabel(new Vector2(45, 294), "VAL", 25).UpdateableStatistics("Stats.Wisdom.Current", this);

        AddLabel(new Vector2(155, 323), "INT", 25);
        AddLabel(new Vector2(45, 323), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?



        ////////////////////
        // Resource Stats //
        ////////////////////

        AddLabel(new Vector2(-116, 178), "HTH", 25);
        AddLabel(new Vector2(-255, 178), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?
        AddLabel(new Vector2(-116, 203), "REG", 12);
        AddLabel(new Vector2(-255, 203), "VAL", 12).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?

        AddLabel(new Vector2(-116, 222), "SHLD", 25);
        AddLabel(new Vector2(-255, 222), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?
        AddLabel(new Vector2(-116, 247), "REG", 12);
        AddLabel(new Vector2(-255, 247), "VAL", 12).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?

        AddLabel(new Vector2(-116, 274), "STA", 25);
        AddLabel(new Vector2(-255, 274), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?
        AddLabel(new Vector2(-116, 299), "REG", 12);
        AddLabel(new Vector2(-255, 299), "VAL", 12).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?

        AddLabel(new Vector2(-116, 318), "MANA", 25);
        AddLabel(new Vector2(-255, 318), "VAL", 25).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?
        AddLabel(new Vector2(-116, 343), "REG", 12);
        AddLabel(new Vector2(-255, 343), "VAL", 12).UpdateableStatistics("Stats.Strength.Current", this); // Stat name?

    }

    //______________________________________________Static__________________________________________________
}

static partial class UpdateableText
{
    /// <summary>
    /// Set statistic to be updated every Update
    /// </summary>
    /// <param name="content">Path to content</param>
    /// <param name="manager">Current statisccs Manager</param>
    public static void UpdateableStatistics(this Text text, string content, StatisticsManager manager)
    {
        manager.StatisticsPath.Add(new StatisticsManager.TextStringPair(content, text));
        Debug.Log(manager.StatisticsPath.Count);
    }
}
