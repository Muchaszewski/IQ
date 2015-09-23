using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MovableAnimator : Singleton<MovableAnimator>
{
    [SerializeField]
    public List<MovableContainer> PanelGroups;

    void Start()
    {
        foreach (var panelGroup in PanelGroups)
        {
            panelGroup.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var panelGroup in PanelGroups)
        {
            if (panelGroup.IsAnimating)
            {
                panelGroup.Animate();
            }
        }
    }

    public void AddToQueue(int groupID, int panelID)
    {
        var currentGroup = PanelGroups[groupID];
        currentGroup.StartAnimation(panelID);
    }
}

[Serializable]
public class MovableContainer
{
    [Tooltip("Name to make it more estetic in editor")]
    public string Name = "";

    public float FullOutTime = 1f;
    public float FullInTime = 1f;
    [Tooltip("If false object In will be waiting for object Out to leave the scene")]
    public bool MoveSimultaneously = true;
    [Tooltip("If false first listed MovablePanel will be displayed at MoveToPosition")]
    public bool StartWithNone = false;

    [Tooltip("Move movable towards this point on Out")]
    public Vector2 MoveOutPoint;
    [Tooltip("Movable will be teleported at this point on In Begin")]
    public Vector2 MoveInPoint;
    [Tooltip("This is Netutral point of displayed a panel")]
    public Vector2 MoveToPoint;

    [SerializeField]
    [Tooltip("All panels of given group with the same destination")]
    private List<MovablePanel> _panels = new List<MovablePanel>();

    public bool IsAnimating { get; private set; }
    private MovablePanel _panelInMove;
    private MovablePanel _panelOutMove;
    private bool _isPanelOutEscaped;

    private MovablePanel _nextToMove;

    public void Init()
    {
        if (!StartWithNone)
        {
            _panelOutMove = _panels[0];
            _panelOutMove.RectTransform.anchoredPosition = MoveToPoint;
        }
    }

    public void StartAnimation(int panelID)
    {
        if (!IsAnimating)
        {
            IsAnimating = true;
            if (_panels[panelID] != _panelOutMove)
            {
                _nextToMove = _panels[panelID];
                _nextToMove.RectTransform.anchoredPosition = MoveInPoint;
            }
        }
    }

    public void StartAnimation()
    {
        IsAnimating = true;
        _nextToMove = null;
    }


    /// <summary>
    /// Progress thru one step of animation
    /// </summary>
    public void Animate()
    {

        //If Current panel is null
        //Set it to null to avoid moving it
        if (_panelOutMove == null)
        {
            _isPanelOutEscaped = true;
        }
        else
        {
            _isPanelOutEscaped = false;
        }

        //If panel exists move it
        if (!_isPanelOutEscaped)
        {
            if (_panelOutMove.CustomDestination)
            {
                if (_panelOutMove.MovePanelTowards(_panelOutMove.Destination, MoveToPoint, FullOutTime))
                {
                    _isPanelOutEscaped = true;
                }
            }
            else
            {
                if (_panelOutMove.MovePanelTowards(MoveOutPoint, MoveToPoint, FullOutTime))
                {
                    _isPanelOutEscaped = true;
                }

            }
        }

        //If there are any panels to animate
        if (_nextToMove != null)
        {
            //Get this panel and set it to panelInMove variable
            _panelInMove = _nextToMove;

            //If panel should move
            if (MoveSimultaneously || (!MoveSimultaneously && _isPanelOutEscaped))
            {
                if (_panelInMove.MovePanelTowards(MoveToPoint, MoveInPoint, FullInTime))
                {
                    //If panel is in place
                    //Swap current panel with in panel
                    _panelOutMove = _panelInMove;
                    //Remove from queue
                    _nextToMove = null;
                    if (_isPanelOutEscaped)
                    {
                        IsAnimating = false;
                    }
                }
            }
        }
        else
        {
            if (_isPanelOutEscaped)
            {
                IsAnimating = false;
                _panelOutMove = null;
            }
        }
    }
}