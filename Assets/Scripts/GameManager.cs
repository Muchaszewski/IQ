using UnityEngine;
using System.Collections;
using InventoryQuest.Game;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Color StatisticsBuffColor = Color.blue;
    public Color StatisticsDebuffColor = Color.red;
    public Color StatisciscNormalColor = Color.white;

    public GameObject TooltipGameObject;
    public float TooltipHovertime = 2;

    private Vector3 _mousePosVector3;
    private float _tooltopTimeElapsed;

    private bool _tooltipShow = false;
    public bool TooltipShow
    {
        get { return _tooltipShow; }
        set { _tooltipShow = value; }
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        Instance = this;
    }

    public void Update()
    {
        if (TooltipShow)
        {
            if (_mousePosVector3 != Input.mousePosition)
            {
                _mousePosVector3 = Input.mousePosition;
                _tooltopTimeElapsed = 0;
                ScoopOutside(TooltipGameObject);
            }
            else
            {
                _tooltopTimeElapsed += Time.deltaTime;
                if (_tooltopTimeElapsed > TooltipHovertime)
                {
                    TooltipGameObject.transform.position = _mousePosVector3;
                }
                else
                {
                    ScoopOutside(TooltipGameObject);
                }
            }
        }
    }

    public void ScoopOutside(GameObject gameObject)
    {
        gameObject.transform.position = new Vector3(10000, 10000, 0);
    }

    public void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "LoadingScene")
        {
            foreach (Transform item in transform)
            {
                foreach (var mono in item.GetComponents<MonoBehaviour>())
                {
                    if (mono.GetType() == typeof(GameManager)) continue;
                    mono.enabled = false;
                }
            }
        }
    }
}
