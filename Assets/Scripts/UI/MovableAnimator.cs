using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovableAnimator : MonoBehaviour
{
    [SerializeField]
    public List<MovableContainer> PanelGroups;

    [Serializable]
    public class MovableContainer
    {
        [Tooltip("Name to make it more estetic in editor")]
        public string Name = "";

        [Tooltip("Move movable twards this point on Out")]
        public Vector2 MoveOutPoint;
        [Tooltip("Movable will be teleported at this poin on In Begin")]
        public Vector2 MoveInPoint;
        [Tooltip("This is Netutral point of displayed a panel")]
        public Vector2 MoveToPoint;

        [SerializeField]
        [Tooltip("All panels of given group with the same destination")]
        public List<MovablePanel> Panels;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
