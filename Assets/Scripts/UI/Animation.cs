using UnityEngine;
using System.Collections;

public class Animation : MonoBehaviour {

    [Tooltip("Position where panel should be invisible")]
    public Vector2 IdlePosition = new Vector2(-5000,-5000);

    [Tooltip("Position on the left side of UI. Position should be anchored to the Canvas with left anchor")]
    public Vector2 PanelLeftAnchorPosition;

    [Tooltip("Position on the center of UI. Position should be anchored to the Canvas with !@!@ ??? !@@!")]
    public Vector2 PanelCenterAnchorPosition;

    [Tooltip("Position on the right side of UI. Position should be anchored to the Canvas with right anchor")]
    public Vector2 PanelRightAnchorPosition;

    public GameObject[] LeftPanels;
    public GameObject[] CenterPanels;
    public GameObject[] RightPanels;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
