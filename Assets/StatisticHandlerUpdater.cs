using UnityEngine;
using System.Collections;

[RequireComponent(typeof(StatisticHandler))]
[DisallowMultipleComponent]
public class StatisticHandlerUpdater : MonoBehaviour
{

    private StatisticHandler _statisticHandler;

    void Start()
    {
        _statisticHandler = GetComponent<StatisticHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        _statisticHandler.TextComponent.text = _statisticHandler.StatReference.ToString();
    }
}
