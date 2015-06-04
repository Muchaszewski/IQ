using System;
using UnityEngine;
using System.Collections;
using InventoryQuest.Components.Entities.Player;
using InventoryQuest.Game;

public class StatisticsManager : UIManager {

    //_____________________________________________________________________________________________________________

    private RectTransform _rectTransform;
    private Player _player;

    //__________________________________________________MonoBehaviour______________________________________________

	// Use this for initialization
	void Start ()
	{
	    _rectTransform = new RectTransform();
	    _player = CurrentGame.Instance.Player;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //______________________________________________End MonoBehaviour End__________________________________________



    //______________________________________________Public Methods_________________________________________________

    /// <summary>
    /// Create all labels for tooltip
    /// </summary>
    /// <param name="item"></param>
    [Obsolete("Only for inspector use, and SetTooltip method")]
    public void CreateLabels(InventoryQuest.Components.Items.Item item)
    {
#if UNITY_EDITOR
        _rectTransform = GetComponent<RectTransform>();
#endif
        //---------------

        //---------------
    }


    //__________________________________________End Public Methods End_____________________________________________

    void CreateTitle()
    {
        
    }

}
