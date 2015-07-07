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
    [Tooltip("If false object In will be waitng for object Out to leave the scene")]
    public bool MoveSimultaneously = true;
    [Tooltip("If false first listed MovablePanel will be displayed at MoveToPosition")]
    public bool StartWithNone = false;

    [Tooltip("Move movable twards this point on Out")]
    public Vector2 MoveOutPoint;
    [Tooltip("Movable will be teleported at this poin on In Begin")]
    public Vector2 MoveInPoint;
    [Tooltip("This is Netutral point of displayed a panel")]
    public Vector2 MoveToPoint;

    [SerializeField]
    [Tooltip("All panels of given group with the same destination")]
    private List<MovablePanel> _panels = new List<MovablePanel>();

    public bool IsAnimating { get; private set; }
    private MovablePanel panelInMove;
    private MovablePanel panelOutMove;
    private bool panelOutEscaped;

    private readonly Queue<MovablePanel> _queue = new Queue<MovablePanel>();

    public void Init()
    {
        if (!StartWithNone)
        {
            panelOutMove = _panels[0];
            panelOutMove.RectTransform.anchoredPosition = MoveToPoint;
        }
        else
        {
            foreach (var panel in _panels)
            {
                panel.RectTransform.anchoredPosition = MoveInPoint;
            }
        }
    }

    public void StartAnimation(int panelID)
    {
        IsAnimating = true;
        _queue.Enqueue(_panels[panelID]);
        _queue.Peek().RectTransform.anchoredPosition = MoveInPoint;
    }

    public void StartAnimation()
    {
        IsAnimating = true;
        _queue.Enqueue(null);
    }

    /// <summary>
    /// Progress thru one step of animation
    /// </summary>
    public void Animate()
    {
        //If Current panel is null
        //Set it to null to avoid moving it
        if (panelOutMove == null)
        {
            panelOutEscaped = true;
        }

        //If panel exists move it
        if (!panelOutEscaped)
        {
            if (panelOutMove.CustomDestination)
            {
                if (panelOutMove.MovePanelTowards(panelOutMove.Destination, MoveToPoint, FullOutTime))
                {
                    panelOutEscaped = true;
                }
            }
            else
            {
                if (panelOutMove.MovePanelTowards(MoveOutPoint, MoveToPoint, FullOutTime))
                {
                    panelOutEscaped = true;
                }

            }
        }

        //If there are any panels to animate
        if (_queue.Any())
        {
            //Get this panel and set it to panelInMove variable
            panelInMove = _queue.Peek();

            //If panel should move
            if (MoveSimultaneously || (!MoveSimultaneously && panelOutEscaped))
            {
                if (panelInMove.MovePanelTowards(MoveToPoint, MoveInPoint, FullInTime))
                {
                    //If panel is in place
                    //Swap current panel with in panel
                    panelOutMove = panelInMove;
                    //Remove from queue
                    _queue.Dequeue();
                }
            }
        }
    }
}