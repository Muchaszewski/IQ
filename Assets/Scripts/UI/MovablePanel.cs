﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.RectTransform))]
public class MovablePanel : MonoBehaviour
{
    public RectTransform RectTransform;

    public bool CustomDestination;

    public Vector2 Destination;

    public bool IsActive { get; set; }

    public void Start()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public bool MovePanelTowards(Vector2 destination, Vector2 begining, float time)
    {
        var current = Vector2.MoveTowards(RectTransform.anchoredPosition, destination,
                    Vector2.Distance(begining, destination) * (time * Time.deltaTime));
        RectTransform.anchoredPosition = current;
        if (current == destination)
        {
            return true;
        }
        return false;
    }
}