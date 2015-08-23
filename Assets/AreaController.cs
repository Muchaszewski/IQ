using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InventoryQuest.Game;
using InventoryQuest.Components;

public class AreaController : MonoBehaviour
{
    public Spot Spot;
    private Text _spotName;
    private Text _spotLevel;
    public RectTransform RectTransform { get; set; }

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start()
    {
        _spotName = transform.GetChild(0).GetComponent<Text>();
        _spotLevel = transform.GetChild(1).GetComponent<Text>();

        _spotName.text = Spot.Name;
        _spotLevel.text = "Level " + Spot.Level.ToString();
        GetComponent<Button>().onClick.AddListener(() => ChangeSpot());
    }

    // Update is called once per frame
    void ChangeSpot()
    {
        CurrentGame.Instance.TravelToSpot = Spot.ID;
    }
}
